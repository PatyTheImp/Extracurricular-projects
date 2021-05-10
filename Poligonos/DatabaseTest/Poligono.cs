using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseTest
{
    class Poligono
    {
        static int indice = 0;
        int pontoMedioX;
        int pontoMedioY;
        Label l = new();
        

        public int Indice { get; set; }
        public List<Point> Points { get; set; }
        public int Espessura { get; set; }
        public Color Preechimento { get; set; }
        public Label Identificador
        {
            get { return l; }
            set { l = value; }
        }

        public Poligono(List<Point> points, int espessura, Color preenchimento)
        {
            Indice = CriaIndice();
            Points = points;
            Espessura = espessura;
            Preechimento = preenchimento;
            Identificador = CriaIdentificador();
        }

        private int CriaIndice()
        {
            return ++indice;
        }

        private Label CriaIdentificador()
        {
            pontoMedioX = (int)Points.Average(s => s.X);
            pontoMedioY = (int)Points.Average(s => s.Y);

            l.Text = Indice.ToString();
            l.Location = new Point(pontoMedioX, pontoMedioY);
            l.AutoSize = true;
            l.BackColor = Form1.CorComplementar(Preechimento);
            l.ForeColor = Preechimento;

            return l;
        }

        public override string ToString()
        {
            return "Poligono " + Indice;
        }
    }
}
