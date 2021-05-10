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
    /// Interaction logic for Empregados.xaml
    /// </summary>
    public partial class Empregados : Window
    {
        //Adaptadores
        HotelSADBTableAdapters.EmpregadoTableAdapter empregadoTableAdapter;
        HotelSADBTableAdapters.LocalidadesTableAdapter localidadesTableAdapter;
        HotelSADBTableAdapters.CargosTableAdapter cargosTableAdapter;
        HotelSADBTableAdapters.MovimentoTableAdapter movimentoTableAdapter;

        //Tabelas
        DataTable empregados;
        DataTable localidades;
        DataTable cargos;
        DataTable movimentos;

        SqlConnection connection;
        int indiceRegisto;
        BrushConverter converter = new();
        Brush botaoAtivo;
        Brush botaoInativo;
        bool paraEditar;

        public Empregados()
        {
            InitializeComponent();
            GetTableInfo();

            connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=K:\OneDrive\informática\Portfolio_Projects\WPFHotelSA\WPFHotelSA\Database1.mdf;Integrated Security=True");
            indiceRegisto = 0;
            botaoAtivo = (Brush)converter.ConvertFromString("#033A40");
            botaoInativo = (Brush)converter.ConvertFromString("#1A6870");
            paraEditar = false;

            PopulaComboBox();
            PreencheCampos(indiceRegisto);
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
            if (indiceRegisto < empregados.Rows.Count - 1)
            {
                indiceRegisto++;
                PreencheCampos(indiceRegisto);
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
                PreencheCampos(indiceRegisto);
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
                PreencheCampos(indiceRegisto);
                PopulaTabelaMovimentos();
            }
            else
                MessageBox.Show("Já está no primeiro registo", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);            
        }

        private void BtnUltimo_Click(object sender, RoutedEventArgs e)
        {
            if (indiceRegisto < empregados.Rows.Count - 1)
            {
                indiceRegisto = empregados.Rows.Count - 1;
                PreencheCampos(indiceRegisto);
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
                int id = (int)empregados.Rows[indiceRegisto]["nr_de_empregado"];

                try
                {
                    connection.Open();
                    SqlCommand cmd = new("DELETE FROM Empregado WHERE nr_de_empregado = @id", connection);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Registo eliminado com sucesso");

                    if (indiceRegisto >= empregados.Rows.Count - 1)
                        indiceRegisto--;

                    GetTableInfo();
                    PreencheCampos(indiceRegisto);
                    PopulaTabelaMovimentos();
                }
                catch (Exception)
                {
                    connection.Close();
                    MessageBox.Show("Algo correu mal");
                }
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (btnAnterior.IsEnabled)
            {
                paraEditar = true;
                DesativarBotoes();
                AtivarCampos();
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

                    GetTableInfo();
                    PreencheCampos(indiceRegisto);
                    PopulaTabelaMovimentos();
                    AtivarBotoes();
                    DesativarCampos();
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
                AtivarCampos();
            }
            //Botão cancelar
            else
            {
                if (MessageBox.Show("Se cancelar a operação, vai perder o progresso não salvo. Tem certeza que quer cancelar?",
                    "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    PreencheCampos(indiceRegisto);
                    PopulaTabelaMovimentos();
                    AtivarBotoes();
                    DesativarCampos();
                }
            }
        }

        private void TxtbNome_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void TelefoneNumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");

            if (txtbTelefone.Text.Length == 9)
                e.Handled = true;
            else
                e.Handled = regex.IsMatch(e.Text);
        }

        private void TelemovelNumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");

            if (txtbTelemovel.Text.Length == 9)
                e.Handled = true;
            else
                e.Handled = regex.IsMatch(e.Text);
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

        private bool NovoRegistoSucesso()
        {
            string horario;

            if (rbDiurno.IsChecked == true)
                horario = "Diurno";
            else
                horario = "Noturno";

            try
            {
                connection.Open();
                SqlCommand cmd = new("INSERT INTO Empregado(nome, morada, localidade, cod_postal, telefone, data_de_contratação, cargo, horário, telemovel, foto)" +
                    "  VALUES(@nome, @morada, @localidade, @codPostal, @telefone, @data, @cargo, @horario, @telemovel, @foto)", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@nome", txtbNome.Text);
                cmd.Parameters.AddWithValue("@morada", txtbMorada.Text);
                cmd.Parameters.AddWithValue("@localidade", cmbLocalidade.SelectedItem);
                cmd.Parameters.AddWithValue("@codPostal", txtbCodPostal.Text);
                cmd.Parameters.AddWithValue("@telefone", txtbTelefone.Text);
                cmd.Parameters.AddWithValue("@data", dpDataContrat.SelectedDate);
                cmd.Parameters.AddWithValue("@cargo", cmbCargo.SelectedItem);
                cmd.Parameters.AddWithValue("@horario", horario);
                cmd.Parameters.AddWithValue("@telemovel", txtbTelemovel.Text);
                cmd.Parameters.AddWithValue("@foto", CriarByteArray((BitmapImage)imgFoto.Source));
                cmd.ExecuteNonQuery();
                connection.Close();

                indiceRegisto = empregados.Rows.Count;
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

        private bool EditaDBSucesso()
        {
            int id = (int)empregados.Rows[indiceRegisto]["nr_de_empregado"];
            string horario;

            if (rbDiurno.IsChecked == true)
                horario = "Diurno";
            else
                horario = "Noturno";

            try
            {
                connection.Open();
                SqlCommand cmd = new("UPDATE Empregado SET nome = @nome, morada = @morada, localidade = @localidade," +
                    "cod_postal = @codPostal, telefone = @telefone, data_de_contratação = @data, cargo = @cargo, horário = @horario, " +
                    "telemovel = @telemovel, foto = @foto WHERE nr_de_empregado = @id", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nome", txtbNome.Text);
                cmd.Parameters.AddWithValue("@morada", txtbMorada.Text);
                cmd.Parameters.AddWithValue("@localidade", cmbLocalidade.SelectedItem);
                cmd.Parameters.AddWithValue("@codPostal", txtbCodPostal.Text);
                cmd.Parameters.AddWithValue("@telefone", txtbTelefone.Text);
                cmd.Parameters.AddWithValue("@data", dpDataContrat.SelectedDate);
                cmd.Parameters.AddWithValue("@cargo", cmbCargo.SelectedItem);
                cmd.Parameters.AddWithValue("@horario", horario);
                cmd.Parameters.AddWithValue("@telemovel", txtbTelemovel.Text);
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

        private void LimpaCampos()
        {
            lblContador.Content = String.Format("Empregado {0} de {1}", empregados.Rows.Count + 1, empregados.Rows.Count + 1);

            lblNrEmpregado.Content = null;
            txtbNome.Text = null;
            txtbMorada.Text = null;
            cmbLocalidade.SelectedItem = null;
            txtbCodPostal.Text = null;
            txtbTelefone.Text = null;
            txtbTelemovel.Text = null;

            imgFoto.Source = null;
            imgFoto2.Source = null;

            cmbCargo.SelectedItem = null;
            dpDataContrat.SelectedDate = null;
            rbDiurno.IsChecked = false;
            rbNoturno.IsChecked = false;

            dgMovimentos.ItemsSource = null;
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

        private void GetTableInfo()
        {
            empregadoTableAdapter = new();
            empregados = empregadoTableAdapter.GetData();
            localidadesTableAdapter = new();
            localidades = localidadesTableAdapter.GetData();
            cargosTableAdapter = new();
            cargos = cargosTableAdapter.GetData();
            movimentoTableAdapter = new();
            movimentos = movimentoTableAdapter.GetData();
        }


        private void PreencheCampos(int indice)
        {
            DataRow registo = empregados.Rows[indice];

            lblContador.Content = String.Format("Empregado {0} de {1}", indice + 1, empregados.Rows.Count);

            lblNrEmpregado.Content = registo["nr_de_empregado"].ToString();
            txtbNome.Text = registo["nome"].ToString();
            txtbMorada.Text = registo["morada"].ToString();
            cmbLocalidade.SelectedItem = registo["localidade"].ToString();
            txtbCodPostal.Text = registo["cod_postal"].ToString();
            txtbTelefone.Text = registo["telefone"].ToString();
            txtbTelemovel.Text = registo["telemovel"].ToString();

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

            cmbCargo.SelectedItem = registo["cargo"].ToString();
            dpDataContrat.SelectedDate = (DateTime?)registo["data_de_contratação"];
            if (registo["horário"].ToString() == "Diurno")
                rbDiurno.IsChecked = true;
            else
                rbNoturno.IsChecked = true;
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

        private void PopulaComboBox()
        {
            var getLocalidades = from localidade in localidades.AsEnumerable()
                                 select localidade.Field<string>("localidade");

            cmbLocalidade.ItemsSource = getLocalidades;

            var getCargos = from cargo in cargos.AsEnumerable()
                            select cargo.Field<string>("cargo");

            cmbCargo.ItemsSource = getCargos;
        }

        private void PopulaTabelaMovimentos()
        {
            var movimentacoes = from movimento in movimentos.AsEnumerable()
                                join empregado in empregados.AsEnumerable()
                                on movimento.Field<int>("nr_empregado") equals empregado.Field<int>("nr_de_empregado")
                                where empregado.Field<int>("nr_de_empregado") == Int32.Parse((string)lblNrEmpregado.Content)
                                select new 
                                {
                                    NomeHotel = movimento.Field<string>("nome_do_hotel"),
                                    DataEntrada = movimento.Field<DateTime>("data_de_entrada").Date.ToString("d"),
                                    DataSaida = movimento.Field<DateTime>("data_de_saida").Date.ToString("d")
                                };

            dgMovimentos.ItemsSource = movimentacoes;
        }

        private void AtivarCampos()
        {
            txtbNome.IsReadOnly = false;
            txtbMorada.IsReadOnly = false;
            txtbCodPostal.IsReadOnly = false;
            txtbTelefone.IsReadOnly = false;
            txtbTelemovel.IsReadOnly = false;

            cmbCargo.IsEnabled = true;
            cmbLocalidade.IsEnabled = true;
            dpDataContrat.IsEnabled = true;
            rbDiurno.IsEnabled = true;
            rbNoturno.IsEnabled = true;

            btnMudarFoto.Visibility = Visibility.Visible;
            btnMudarFoto2.Visibility = Visibility.Visible;
        }

        private void DesativarCampos()
        {
            txtbNome.IsReadOnly = true;
            txtbMorada.IsReadOnly = true;
            txtbCodPostal.IsReadOnly = true;
            txtbTelefone.IsReadOnly = true;
            txtbTelemovel.IsReadOnly = true;

            cmbCargo.IsEnabled = false;
            cmbLocalidade.IsEnabled = false;
            dpDataContrat.IsEnabled = false;
            rbDiurno.IsEnabled = false;
            rbNoturno.IsEnabled = false;

            btnMudarFoto.Visibility = Visibility.Hidden;
            btnMudarFoto2.Visibility = Visibility.Hidden;
        }

        private bool ValidaCampos()
        {
            if (txtbCodPostal.Text.Trim() == string.Empty || txtbMorada.Text.Trim() == string.Empty || txtbNome.Text.Trim() == string.Empty ||
                txtbTelefone.Text.Trim() == string.Empty || txtbTelemovel.Text.Trim() == string.Empty || cmbCargo.SelectedItem == null ||
                cmbLocalidade.SelectedItem == null || dpDataContrat.SelectedDate == null || (!(bool)rbDiurno.IsChecked && !(bool)rbNoturno.IsChecked))
            {
                MessageBox.Show("Preencha todos os campos", "Info", 
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

            if (txtbTelefone.Text.Length < 9 || txtbTelefone.Text.Substring(0, 2) != "21")
            {
                MessageBox.Show("O campo Telefone tem de ter 9 digitos e começar por 21", "Info", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                txtbTelefone.Focus();
                return false;
            }

            if (txtbTelemovel.Text.Length < 9 || (txtbTelemovel.Text.Substring(0, 2) != "91" && txtbTelemovel.Text.Substring(0, 2) != "92" 
                && txtbTelemovel.Text.Substring(0, 2) != "93" && txtbTelemovel.Text.Substring(0, 2) != "96"))
            {
                MessageBox.Show("O campo Telemóvel tem de ter 9 digitos e começar por 91, 92, 93 ou 96", "Info", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                txtbTelemovel.Focus();
                return false;
            }

            return true;
        }
    }
}
