using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using FontAwesome5;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace WPFHotelSA
{
    /// <summary>
    /// Interaction logic for Hoteis.xaml
    /// </summary>
    public partial class Hoteis : Window
    {
        HotelSADBTableAdapters.HotelTableAdapter hotelTableAdapter;
        HotelSADBTableAdapters.MovimentoTableAdapter movimentoTableAdapter;
        HotelSADBTableAdapters.LocalidadesTableAdapter localidadesTableAdapter;

        DataTable hoteis;
        DataTable movimentos;
        DataTable localidades;

        SqlConnection connection;
        int indiceRegisto;
        BrushConverter converter = new();
        Brush botaoAtivo;
        Brush botaoInativo;
        bool paraEditar;

        public Hoteis()
        {
            InitializeComponent();
            GetTableInfo();

            connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=K:\\OneDrive\\informática\\Portfolio_Projects\\WPFHotelSA\\WPFHotelSA\\Database1.mdf;Integrated Security=True");
            indiceRegisto = 0;
            botaoAtivo = (Brush)converter.ConvertFromString("#033A40");
            botaoInativo = (Brush)converter.ConvertFromString("#1A6870");
            paraEditar = false;

            PopulaComboBox();
            PreencheCampos();
            PopulaTabelaMovimentos();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!btnMenu.IsEnabled)
            {
                if (MessageBox.Show("Qualquer alteração não guardada será perdida ao fechar a aplicação. Tem certeza que quer continuar?",
                "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }

        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            this.Close();
            mainWindow.Show();
        }

        private void BtnSeguinte_Click(object sender, RoutedEventArgs e)
        {
            if (indiceRegisto < hoteis.Rows.Count - 1)
            {
                indiceRegisto++;
                PreencheCampos();
                PopulaTabelaMovimentos();
            }
            else
                MessageBox.Show("Já está no último registo", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (indiceRegisto > 0)
            {
                indiceRegisto--;
                PreencheCampos();
                PopulaTabelaMovimentos();
            }
            else
                MessageBox.Show("Já está no primeiro registo", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnPrimeiro_Click(object sender, RoutedEventArgs e)
        {
            if (indiceRegisto > 0)
            {
                indiceRegisto = 0;
                PreencheCampos();
                PopulaTabelaMovimentos();
            }
            else
                MessageBox.Show("Já está no primeiro registo", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnUltimo_Click(object sender, RoutedEventArgs e)
        {
            if (indiceRegisto < hoteis.Rows.Count - 1)
            {
                indiceRegisto = hoteis.Rows.Count - 1;
                PreencheCampos();
                PopulaTabelaMovimentos();
            }
            else
                MessageBox.Show("Já está no último registo", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnApagarRegisto_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que quer apagar este registo?", "Aviso", MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                string nomeHotel = hoteis.Rows[indiceRegisto]["nome_do_hotel"].ToString();

                try
                {
                    connection.Open();
                    SqlCommand cmd = new("DELETE FROM Hotel WHERE nome_do_hotel = @nomeHotel", connection);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@nomeHotel", nomeHotel);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Registo eliminado com sucesso");

                    if (indiceRegisto >= hoteis.Rows.Count - 1)
                        indiceRegisto--;

                    GetTableInfo();
                    PreencheCampos();
                    PopulaTabelaMovimentos();
                }
                catch (Exception erro)
                {
                    connection.Close();
                    MessageBox.Show(erro.ToString());
                }
            }
        }

        private void BtnNovoRegisto_Click(object sender, RoutedEventArgs e)
        {
            if (btnAnterior.IsEnabled)
            {
                paraEditar = false;
                LimpaCampos();
                DesativarBotoes();
                AtivarCampos(paraEditar);
            }
            //Botão cancelar
            else
            {
                if (MessageBox.Show("Se cancelar a operação, vai perder o progresso não salvo. Tem certeza que quer cancelar?",
                    "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    PreencheCampos();
                    PopulaTabelaMovimentos();
                    AtivarBotoes();
                    DesativarCampos();
                }
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (btnAnterior.IsEnabled)
            {
                paraEditar = true;
                DesativarBotoes();
                AtivarCampos(paraEditar);
            }
            //Botão guardar
            else
            {
                if (MessageBox.Show("Quer guardar as alterações?", "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.Yes)
                {
                    if (!ValidaCampos())
                        return;

                    if (paraEditar)
                    {
                        if (!EditaDBSucesso())
                            return;
                    }
                    else
                    {
                        if (!NovoRegistoSucesso())
                            return;
                    }
                }

                GetTableInfo();
                PreencheCampos();
                PopulaTabelaMovimentos();
                AtivarBotoes();
                DesativarCampos();
            }
        }

        private void BtnMudarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                Uri ficheiro = new(openFileDialog.FileName);
                imgFoto.Source = new BitmapImage(ficheiro);
                imgFoto2.Source = imgFoto.Source;
            }
        }

        private void TxtbDiretor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^a-zA-Zçáàãâéèêíìóòõôúùû]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TxtbCodPostal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9-]+");

            if (txtbCodPostal.Text.Length == 8)
                e.Handled = true;
            else
                e.Handled = regex.IsMatch(e.Text);
        }

        private void TxtbNrQuartos_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //-------------Meus Métodos................................

        private void GetTableInfo()
        {
            movimentoTableAdapter = new();
            movimentos = movimentoTableAdapter.GetData();
            localidadesTableAdapter = new();
            localidades = localidadesTableAdapter.GetData();
            hotelTableAdapter = new();
            hoteis = hotelTableAdapter.GetData();
        }

        private void PopulaComboBox()
        {
            var getLocalidades = from localidade in localidades.AsEnumerable()
                                 select localidade.Field<string>("localidade");

            cmbLocalidade.ItemsSource = getLocalidades;
        }

        private void PopulaTabelaMovimentos()
        {
            var movimentacoes = from movimento in movimentos.AsEnumerable()
                                join hotel in hoteis.AsEnumerable()
                                on movimento.Field<string>("nome_do_hotel") equals hotel.Field<string>("nome_do_hotel")
                                where hotel.Field<string>("nome_do_hotel") == txtbNomeHotel.Text
                                select new
                                {
                                    NrEmpregado = movimento.Field<int>("nr_empregado"),
                                    DataEntrada = movimento.Field<DateTime>("data_de_entrada").Date.ToString("d"),
                                    DataSaida = movimento.Field<DateTime>("data_de_saida").Date.ToString("d"),
                                    HorasRealizadas = movimento.Field<int>("horas_realizadas"),
                                    PrecoHora = movimento.Field<decimal>("preço_hora")
                                };

            dgMovimentos.ItemsSource = movimentacoes;
        }

        private void PreencheCampos()
        {
            DataRow registo = hoteis.Rows[indiceRegisto];

            lblContador.Content = String.Format("Hotel {0} de {1}", indiceRegisto + 1, hoteis.Rows.Count);

            txtbNomeHotel.Text = registo["nome_do_hotel"].ToString();
            txtbDiretor.Text = registo["diretor"].ToString();
            txtbMorada.Text = registo["morada"].ToString();
            cmbLocalidade.SelectedItem = registo["localidade"].ToString();
            txtbCodPostal.Text = registo["cod_postal"].ToString();

            txtbNrQuartos.Text = registo["nr_quartos"].ToString();
            chkbEstacionamento.IsChecked = (bool?)registo["estacionamento_privado"];
            chkbPiscina.IsChecked = (bool?)registo["piscina"];
            chkbArCond.IsChecked = (bool?)registo["ar_condicionado"];
            txtbObservacoes.Text = registo["observacoes"].ToString();

            if (registo["foto"] != DBNull.Value)
            {
                imgFoto.Source = CriarImagem((byte[])registo["foto"]);
                imgFoto2.Source = imgFoto.Source;
            }
            else
            {
                imgFoto.Source = null;
                imgFoto2.Source = null;
            }
        }

        private static BitmapImage CriarImagem(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            var image = new BitmapImage();

            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();

            return image;
        }

        private byte[] CriarByteArray(BitmapImage bitmapImage)
        {
            byte[] data;

            if (bitmapImage == null)
                data = Array.Empty<byte>();
            else
            {
                JpegBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

                using (MemoryStream ms = new())
                {
                    encoder.Save(ms);
                    data = ms.ToArray();
                }
            }
            
            return data;
        }

        private void DesativarBotoes()
        {
            btnAnterior.IsEnabled = false;
            btnSeguinte.IsEnabled = false;
            btnPrimeiro.IsEnabled = false;
            btnUltimo.IsEnabled = false;
            btnApagarRegisto.IsEnabled = false;
            btnMenu.IsEnabled = false;

            btnAnterior.Background = botaoInativo;
            btnSeguinte.Background = botaoInativo;
            btnPrimeiro.Background = botaoInativo;
            btnUltimo.Background = botaoInativo;
            btnApagarRegisto.Background = botaoInativo;
            btnMenu.Background = botaoInativo;

            iconNovo.Icon = EFontAwesomeIcon.Solid_Ban;
            btnNovoRegisto.ToolTip = "Cancelar";
            iconEditar.Icon = EFontAwesomeIcon.Regular_Save;
            btnEditar.ToolTip = "Guardar";
        }

        private void AtivarBotoes()
        {
            btnAnterior.IsEnabled = true;
            btnSeguinte.IsEnabled = true;
            btnPrimeiro.IsEnabled = true;
            btnUltimo.IsEnabled = true;
            btnApagarRegisto.IsEnabled = true;
            btnMenu.IsEnabled = true;

            btnAnterior.Background = botaoAtivo;
            btnSeguinte.Background = botaoAtivo;
            btnPrimeiro.Background = botaoAtivo;
            btnUltimo.Background = botaoAtivo;
            btnApagarRegisto.Background = botaoAtivo;
            btnMenu.Background = botaoAtivo;

            iconNovo.Icon = EFontAwesomeIcon.Solid_Plus;
            btnNovoRegisto.ToolTip = "Novo Registo";
            iconEditar.Icon = EFontAwesomeIcon.Solid_PencilAlt;
            btnEditar.ToolTip = "Editar Registo";
        }

        private void AtivarCampos(bool paraEditar)
        {
            if (!paraEditar)
                txtbNomeHotel.IsEnabled = true;

            txtbDiretor.IsEnabled = true;
            txtbMorada.IsEnabled = true;
            cmbLocalidade.IsEnabled = true;
            txtbCodPostal.IsEnabled = true;

            txtbNrQuartos.IsEnabled = true;
            chkbEstacionamento.IsEnabled = true;
            chkbPiscina.IsEnabled = true;
            chkbArCond.IsEnabled = true;
            txtbObservacoes.IsEnabled = true;

            btnMudarFoto.Visibility = Visibility.Visible;
        }

        private void DesativarCampos()
        {
            txtbNomeHotel.IsEnabled = false;
            txtbDiretor.IsEnabled = false;
            txtbMorada.IsEnabled = false;
            cmbLocalidade.IsEnabled = false;
            txtbCodPostal.IsEnabled = false;

            txtbNrQuartos.IsEnabled = false;
            chkbEstacionamento.IsEnabled = false;
            chkbPiscina.IsEnabled = false;
            chkbArCond.IsEnabled = false;
            txtbObservacoes.IsEnabled = false;

            btnMudarFoto.Visibility = Visibility.Hidden;
        }

        private void LimpaCampos()
        {
            lblContador.Content = String.Format("Hotel {0} de {1}", hoteis.Rows.Count + 1, hoteis.Rows.Count + 1);

            txtbNomeHotel.Text = null;
            txtbDiretor.Text = null;
            txtbMorada.Text = null;
            cmbLocalidade.SelectedItem = null;
            txtbCodPostal.Text = null;

            txtbNrQuartos.Text = null;
            chkbEstacionamento.IsChecked = false;
            chkbPiscina.IsChecked = false;
            chkbArCond.IsChecked = false;
            txtbObservacoes.Text = null;

            imgFoto.Source = null;
            imgFoto2.Source = null;

            dgMovimentos.ItemsSource = null;
        }

        private bool ValidaCampos()
        {
            if (txtbNomeHotel.Text.Trim() == string.Empty || txtbDiretor.Text.Trim() == string.Empty || txtbMorada.Text.Trim() == string.Empty ||
                cmbLocalidade.SelectedItem == null || txtbCodPostal.Text.Trim() == string.Empty || txtbNrQuartos.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Ainda há campos por preencher", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (txtbCodPostal.Text.Length < 8 || txtbCodPostal.Text[4] != '-' || Regex.Matches(txtbCodPostal.Text, "-").Count > 1)
            {
                MessageBox.Show("O campo Código Postal tem de ter o formato ####-###", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                txtbCodPostal.Focus();
                return false;
            }

            Regex regex = new("[^0-9]+");

            if (regex.IsMatch(txtbNrQuartos.Text.Trim()) || Int32.Parse(txtbNrQuartos.Text) <= 0)
            {
                MessageBox.Show("Nº de quartos inválido.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                txtbNrQuartos.Focus();
                return false;
            }

            return true;
        }

        private bool EditaDBSucesso()
        {
            string nomeHotel = hoteis.Rows[indiceRegisto]["nome_do_hotel"].ToString();

            try
            {
                connection.Open();
                SqlCommand cmd = new("UPDATE Hotel SET diretor = @diretor, morada = @morada, localidade = @localidade, " +
                    "cod_postal = @codPostal, nr_quartos = @nrQuartos, estacionamento_privado = @estacionamento, piscina = @piscina, " +
                    "ar_condicionado = @arCondicionado, observacoes = @observacoes, foto = @foto WHERE nome_do_hotel = @nomeHotel", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@nomeHotel", nomeHotel);
                cmd.Parameters.AddWithValue("@diretor", txtbDiretor.Text);
                cmd.Parameters.AddWithValue("@morada", txtbMorada.Text);
                cmd.Parameters.AddWithValue("@localidade", cmbLocalidade.SelectedItem);
                cmd.Parameters.AddWithValue("@codPostal", txtbCodPostal.Text);
                cmd.Parameters.AddWithValue("@nrQuartos", txtbNrQuartos.Text);
                cmd.Parameters.AddWithValue("@estacionamento", chkbEstacionamento.IsChecked);
                cmd.Parameters.AddWithValue("@piscina", chkbPiscina.IsChecked);
                cmd.Parameters.AddWithValue("@arCondicionado", chkbArCond.IsChecked);
                cmd.Parameters.AddWithValue("@observacoes", txtbObservacoes.Text);
                cmd.Parameters.AddWithValue("@foto", CriarByteArray((BitmapImage)imgFoto.Source));
                cmd.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Registo editado com sucesso");

                return true;
            }
            catch (Exception erro)
            {
                connection.Close();
                MessageBox.Show(erro.ToString());
                return false;
            }
        }

        private bool NovoRegistoSucesso()
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new("INSERT INTO Hotel(nome_do_hotel, diretor, morada, localidade, cod_postal, nr_quartos, estacionamento_privado, " +
                    "piscina, ar_condicionado, observacoes, foto) " +
                    "VALUES(@nomeHotel, @diretor, @morada, @localidade, @codPostal, @nrQuartos, @estacionamento, @piscina, @arCondicionado, " +
                    "@observacoes, @foto)", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@nomeHotel", txtbNomeHotel.Text);
                cmd.Parameters.AddWithValue("@diretor", txtbDiretor.Text);
                cmd.Parameters.AddWithValue("@morada", txtbMorada.Text);
                cmd.Parameters.AddWithValue("@localidade", cmbLocalidade.SelectedItem);
                cmd.Parameters.AddWithValue("@codPostal", txtbCodPostal.Text);
                cmd.Parameters.AddWithValue("@nrQuartos", txtbNrQuartos.Text);
                cmd.Parameters.AddWithValue("@estacionamento", chkbEstacionamento.IsChecked);
                cmd.Parameters.AddWithValue("@piscina", chkbPiscina.IsChecked);
                cmd.Parameters.AddWithValue("@arCondicionado", chkbArCond.IsChecked);
                cmd.Parameters.AddWithValue("@observacoes", txtbObservacoes.Text);
                cmd.Parameters.AddWithValue("@foto", CriarByteArray((BitmapImage)imgFoto.Source));
                cmd.ExecuteNonQuery();
                connection.Close();

                indiceRegisto = hoteis.Rows.Count;
                MessageBox.Show("Registo criado com sucesso");

                return true;
            }
            catch (Exception erro)
            {
                connection.Close();
                MessageBox.Show(erro.ToString());
                return false;
            }
        }

        
    }
}
