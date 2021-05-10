using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Data;
using System.Data.SqlClient;
using FontAwesome5;
using System.Text.RegularExpressions;

namespace WPFHotelSA
{
    /// <summary>
    /// Interaction logic for Movimentos.xaml
    /// </summary>
    public partial class Movimentos : Window
    {
        HotelSADBTableAdapters.MovimentoTableAdapter movimentoTableAdapter;
        HotelSADBTableAdapters.EmpregadoTableAdapter empregadoTableAdapter;
        HotelSADBTableAdapters.HotelTableAdapter hotelTableAdapter;

        DataTable movimentos;
        DataTable empregados;
        DataTable hoteis;

        SqlConnection connection;
        int indiceRegisto;
        bool paraEditar;
        BrushConverter converter = new();
        Brush botaoAtivo;
        Brush botaoInativo;

        public Movimentos()
        {
            InitializeComponent();
            GetTableInfo();

            connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=K:\OneDrive\informática\Portfolio_Projects\WPFHotelSA\WPFHotelSA\Database1.mdf;Integrated Security=True");
            indiceRegisto = 0;
            paraEditar = false;
            botaoAtivo = (Brush)converter.ConvertFromString("#033A40");
            botaoInativo = (Brush)converter.ConvertFromString("#1A6870");

            PopulaComboBox();
            PreencheCampos(indiceRegisto);
            PreencheCalculos();
        }

        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            this.Close();
            mainWindow.Show();
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

        private void BtnSeguinte_Click(object sender, RoutedEventArgs e)
        {
            if (indiceRegisto < movimentos.Rows.Count - 1)
            {
                indiceRegisto++;
                PreencheCampos(indiceRegisto);
                PreencheCalculos();
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
                PreencheCalculos();
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
                PreencheCalculos();
            }
            else
                MessageBox.Show("Já está no primeiro registo", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnUltimo_Click(object sender, RoutedEventArgs e)
        {
            if (indiceRegisto < movimentos.Rows.Count - 1)
            {
                indiceRegisto = movimentos.Rows.Count - 1;
                PreencheCampos(indiceRegisto);
                PreencheCalculos();
            }
            else
                MessageBox.Show("Já está no último registo", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnApagarRegisto_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que quer apagar este registo?", "Aviso", MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                int id = (int)movimentos.Rows[indiceRegisto]["id"];

                try
                {
                    connection.Open();
                    SqlCommand cmd = new("DELETE FROM Movimento WHERE id = @id", connection);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Registo eliminado com sucesso");

                    if (indiceRegisto >= movimentos.Rows.Count - 1)
                        indiceRegisto--;

                    GetTableInfo();
                    PreencheCampos(indiceRegisto);
                    PreencheCalculos();
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
                    PreencheCalculos();
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
                    PreencheCalculos();
                    AtivarBotoes();
                    DesativarCampos();
                }
            }
        }

        private void TxtbHorasRealizadas_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TxtbPrecoHora_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //------------Meus métodos-----------------

        private void PopulaComboBox()
        {
            var getNrEmpregados = from empregado in empregados.AsEnumerable()
                                  select empregado.Field<int>("nr_de_empregado");

            cmbNrEmpregado.ItemsSource = getNrEmpregados;

            var getHoteis = from hotel in hoteis.AsEnumerable()
                            select hotel.Field<string>("nome_do_hotel");

            cmbNomeHotel.ItemsSource = getHoteis;
        }

        private void GetTableInfo()
        {
            empregadoTableAdapter = new();
            empregados = empregadoTableAdapter.GetData();
            movimentoTableAdapter = new();
            movimentos = movimentoTableAdapter.GetData();
            hotelTableAdapter = new();
            hoteis = hotelTableAdapter.GetData();
        }

        private void PreencheCampos(int indice)
        {
            DataRow registo = movimentos.Rows[indice];

            lblContador.Content = String.Format("Movimento {0} de {1}", indice + 1, movimentos.Rows.Count);

            cmbNrEmpregado.SelectedItem = registo["nr_empregado"];
            cmbNomeHotel.SelectedItem = registo["nome_do_hotel"];
            dpDataEntrada.SelectedDate = (DateTime?)registo["data_de_entrada"];
            dpDataSaida.SelectedDate = (DateTime?)registo["data_de_saida"];
            txtbHorasRealizadas.Text = registo["horas_realizadas"].ToString();
            txtbPrecoHora.Text = String.Format("{0:C2}", registo["preço_hora"]);
        }

        private void PreencheCalculos()
        {
            var somaTotalMovimentos = from movimento in movimentos.AsEnumerable()
                                      select movimento.Field<int>("horas_realizadas");
            
            lblTotalDias.Content = String.Format("{0} horas", somaTotalMovimentos.Sum());

            DataRow registo = movimentos.Rows[indiceRegisto];

            lblTotalMovNr.Content = String.Format("Total do mov. {0}: ", registo["id"]);

            decimal totalPagar = (int)registo["horas_realizadas"] * (decimal)registo["preço_hora"];
            lblTotalMovCusto.Content = String.Format("{0:C2}", totalPagar);

            int dias = ((DateTime?)registo["data_de_saida"] - (DateTime?)registo["data_de_entrada"]).Value.Days;
            lblDias.Content = String.Format("{0} {1}", dias, dias == 1 ? "dia" : "dias");

            if (dias <= 0) dias = 1;
            lblPorDia.Content = String.Format("{0:C2}", totalPagar / dias);
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

        private void AtivarCampos()
        {
            cmbNrEmpregado.IsEnabled = true;
            cmbNomeHotel.IsEnabled = true;
            dpDataEntrada.IsEnabled = true;
            dpDataSaida.IsEnabled = true;
            txtbHorasRealizadas.IsEnabled = true;
            txtbPrecoHora.IsEnabled = true;
        }

        private void DesativarCampos()
        {
            cmbNrEmpregado.IsEnabled = false;
            cmbNomeHotel.IsEnabled = false;
            dpDataEntrada.IsEnabled = false;
            dpDataSaida.IsEnabled = false;
            txtbHorasRealizadas.IsEnabled = false;
            txtbPrecoHora.IsEnabled = false;
        }

        private void LimpaCampos()
        {
            lblContador.Content = String.Format("Movimento {0} de {1}", movimentos.Rows.Count + 1, movimentos.Rows.Count + 1);

            cmbNomeHotel.SelectedItem = null;
            cmbNrEmpregado.SelectedItem = null;
            dpDataEntrada.SelectedDate = null;
            dpDataSaida.SelectedDate = null;
            txtbHorasRealizadas.Text = null;
            txtbPrecoHora.Text = null;

            lblTotalDias.Content = null;
            lblTotalMovNr.Content = "Total do mov. ???: ";
            lblTotalMovCusto.Content = null;
            lblDias.Content = null;
            lblPorDia.Content = null;
        }

        private bool ValidaCampos()
        {
            if (txtbHorasRealizadas.Text.Trim() == string.Empty || txtbPrecoHora.Text.Trim() == string.Empty || cmbNomeHotel.SelectedItem == null ||
                cmbNrEmpregado.SelectedItem == null || dpDataEntrada.SelectedDate == null || dpDataSaida.SelectedDate == null || 
                txtbPrecoHora.Text.Trim() == "€")
            {
                MessageBox.Show("Preencha todos os campos", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (Regex.Matches(txtbPrecoHora.Text, ",").Count > 1)
            {
                MessageBox.Show("Campo preço hora inválido", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                txtbPrecoHora.Focus();
                return false;
            }

            if (dpDataEntrada.SelectedDate > dpDataSaida.SelectedDate)
            {
                MessageBox.Show("A data de saída tem de ser mais recente que a data de entrada", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                dpDataEntrada.Focus();
                return false;
            }

            return true;
        }

        private bool EditaDBSucesso()
        {
            int id = (int)movimentos.Rows[indiceRegisto]["id"];
            string precoHora = txtbPrecoHora.Text;

            if (txtbPrecoHora.Text.Contains("€"))
                precoHora = txtbPrecoHora.Text.Trim('€');

            try
            {
                connection.Open();
                SqlCommand cmd = new("UPDATE Movimento SET nr_empregado = @nrEmpregado, nome_do_hotel = @nomeDoHotel, " +
                    "data_de_entrada = @dataDeEntrada, data_de_saida = @dataDeSaida, horas_realizadas = @horasRealizadas, " +
                    "preço_hora = @precoHora WHERE id = @id", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nrEmpregado", cmbNrEmpregado.SelectedItem);
                cmd.Parameters.AddWithValue("@nomeDoHotel", cmbNomeHotel.SelectedItem);
                cmd.Parameters.AddWithValue("@dataDeEntrada", dpDataEntrada.SelectedDate);
                cmd.Parameters.AddWithValue("@dataDeSaida", dpDataSaida.SelectedDate);
                cmd.Parameters.AddWithValue("@horasRealizadas", Int32.Parse(txtbHorasRealizadas.Text));
                cmd.Parameters.AddWithValue("@precoHora", Decimal.Parse(precoHora));
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
                SqlCommand cmd = new("INSERT INTO Movimento(nr_empregado, nome_do_hotel, data_de_entrada, data_de_saida, horas_realizadas, preço_hora)" +
                    " VALUES(@nrEmpregado, @nomeDoHotel, @dataDeEntrada, @dataDeSaida, @horasRealizadas, @precoHora)", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@nrEmpregado", cmbNrEmpregado.SelectedItem);
                cmd.Parameters.AddWithValue("@nomeDoHotel", cmbNomeHotel.SelectedItem);
                cmd.Parameters.AddWithValue("@dataDeEntrada", dpDataEntrada.SelectedDate);
                cmd.Parameters.AddWithValue("@dataDeSaida", dpDataSaida.SelectedDate);
                cmd.Parameters.AddWithValue("@horasRealizadas", Int32.Parse(txtbHorasRealizadas.Text));
                cmd.Parameters.AddWithValue("@precoHora", Decimal.Parse(txtbPrecoHora.Text));
                cmd.ExecuteNonQuery();
                connection.Close();

                indiceRegisto = movimentos.Rows.Count;
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
