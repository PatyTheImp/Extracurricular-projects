using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace JogoDaMemoria
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand = new();
        Label firstClicked = null;
        Label secondClicked = null;
        DispatcherTimer timer = new();

        List<string> icons = new() { "!", "!", "N", "N", ",", ",", "'", "'", "b", "b", "v", "v", "w", "w", "z", "z" };
        List<Label> labels; 

        public MainWindow()
        {
            InitializeComponent();

            labels = new List<Label> 
            { 
                cell1, cell2, cell3, cell4, 
                cell5, cell6, cell7, cell8, 
                cell9, cell10, cell11, cell12, 
                cell13, cell14, cell15, cell16
            };

            timer.IsEnabled = false;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 750);
            timer.Tick += new EventHandler(TimerTick);

            AssignIconsToSquare();
        }

        public void AssignIconsToSquare()
        {
            foreach (Label l in labels)
            {
                int randomNum = rand.Next(icons.Count);
                l.Content = icons[randomNum];
                l.Foreground = rectangulo.Fill;

                icons.RemoveAt(randomNum);
            }
        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (timer.IsEnabled)
                return;

            Label clickeLabel = sender as Label;

            if (secondClicked != null)
                return;

            if (clickeLabel.Foreground != rectangulo.Fill)
                return;

            if (firstClicked == null)
            {
                firstClicked = clickeLabel;
                clickeLabel.Foreground = Brushes.White;
                return;
            }

            secondClicked = clickeLabel;
            clickeLabel.Foreground = Brushes.White;

            CheckForWinner();

            if (firstClicked.Content == secondClicked.Content)
            {
                firstClicked = null;
                secondClicked = null;
                return;
            }

            timer.Start();
        }

        void TimerTick(object sender, EventArgs e)
        {
            timer.Stop();

            firstClicked.Foreground = rectangulo.Fill;
            secondClicked.Foreground = rectangulo.Fill;

            secondClicked = null;
            firstClicked = null;
        }

        void CheckForWinner()
        {
            foreach (Label l in labels)
            {
                if (l.Foreground != Brushes.White)
                    return;
            }

            MessageBox.Show("GANHOU!!!", "PARABÉNS!!!");
            Close();
        }
    }
}
