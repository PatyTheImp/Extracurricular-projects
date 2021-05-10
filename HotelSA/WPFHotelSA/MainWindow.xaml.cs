using System.Windows;

namespace WPFHotelSA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnEmpregados_Click(object sender, RoutedEventArgs e)
        {
            Empregados empregado = new();
            empregado.Show();
            this.Close();
        }

        private void BtnMovimentos_Click(object sender, RoutedEventArgs e)
        {
            Movimentos movimento = new();
            movimento.Show();
            this.Close();
        }

        private void BtnHoteis_Click(object sender, RoutedEventArgs e)
        {
            Hoteis hoteis = new();
            hoteis.Show();
            this.Close();
        }

        private void BtnConsultas_Click(object sender, RoutedEventArgs e)
        {
            Consultas consultas = new();
            consultas.Show();
            this.Close();
        }
    }
}
