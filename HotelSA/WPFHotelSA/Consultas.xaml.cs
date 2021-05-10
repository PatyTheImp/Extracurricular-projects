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
    /// Interaction logic for Consultas.xaml
    /// </summary>
    public partial class Consultas : Window
    {
        HotelSADBTableAdapters.EmpregadoTableAdapter empregadoTableAdapter;
        HotelSADBTableAdapters.MovimentoTableAdapter movimentoTableAdapter;

        DataTable empregados;
        DataTable movimentos;

        char[] letras = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'L', 'K', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public Consultas()
        {
            InitializeComponent();
            GetTableInfo();
            cmbLetra.ItemsSource = letras;
        }

        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            this.Close();
            mainWindow.Show();
        }

        private void BtnConsulta1_Click(object sender, RoutedEventArgs e)
        {
            if (dpData.SelectedDate == null)
                MessageBox.Show("Seleccione uma data", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                PopulaTabela1();
        }

        private void BtnConsulta2_Click(object sender, RoutedEventArgs e)
        {
            if (cmbLetra.SelectedItem == null)
                MessageBox.Show("Seleccione uma letra", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                PopulaTabela2();
        }

        //--------------------Meus Métodos-------------------------------------

        private void GetTableInfo()
        {
            empregadoTableAdapter = new();
            empregados = empregadoTableAdapter.GetData();
            movimentoTableAdapter = new();
            movimentos = movimentoTableAdapter.GetData();
        }

        private void PopulaTabela1()
        {
            dgTabela.ItemsSource = null;

            var consulta = from movimento in movimentos.AsEnumerable()
                           join empregado in empregados.AsEnumerable()
                           on movimento.Field<int>("nr_empregado") equals empregado.Field<int>("nr_de_empregado")
                           where movimento.Field<DateTime>("data_de_entrada") == dpData.SelectedDate
                           select new
                           {
                               NomeEmpregado = empregado.Field<string>("nome"),
                               DataSaida = movimento.Field<DateTime>("data_de_saida").Date.ToString("d"),
                               NomeHotel = movimento.Field<string>("nome_do_hotel")
                           };

            dgTabela.ItemsSource = consulta;
        }

        private void PopulaTabela2()
        {
            dgTabela.ItemsSource = null;

            var consulta = from empregado in empregados.AsEnumerable()
                           where empregado.Field<string>("nome").StartsWith((char)cmbLetra.SelectedItem)
                           select new
                           {
                               Nome = empregado.Field<string>("nome"),
                               Cargo = empregado.Field<string>("cargo"),
                               Horario = empregado.Field<string>("horário")
                           };

            dgTabela.ItemsSource = consulta;
        }
    }
}
