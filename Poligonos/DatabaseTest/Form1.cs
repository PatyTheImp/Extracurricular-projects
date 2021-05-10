using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseTest
{
    public partial class Form1 : Form
    {
        List<Point> pontos;
        List<Poligono> poligonos;

        Graphics graphPanel;
        Color fundo;
        Pen tracejado;

        public Form1()
        {
            InitializeComponent();

            pontos = new List<Point>();
            poligonos = new List<Poligono>();

            graphPanel = painel.CreateGraphics();
            fundo = Color.White;
            tracejado = new Pen(Color.Black)
            {
                DashStyle = DashStyle.DashDot
            };
        }

        private void painel_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = new(e.X, e.Y);
            pontos.Add(p);
            
            if (pontos.Count == 1)
                graphPanel.DrawEllipse(tracejado, p.X, p.Y, 1, 1);
            else
                graphPanel.DrawLine(tracejado, pontos[^2], pontos[^1]);
        }

        private void btnLigarPontos_Click(object sender, EventArgs e)
        {
            int espessura = (int)nupdEspessura.Value;

            if (pontos.Count >= 3)
            {
                if (espessura > 0)
                {
                    if (cdCorPreenchimento.ShowDialog() == DialogResult.OK)
                    {
                        Color cor = cdCorPreenchimento.Color;
                        CriarPoligono(espessura, cor);
                    }
                }
                else
                    MessageBox.Show("Escolha uma espessura");
            }
            else
            {
                int pontosEmFalta = 3 - pontos.Count;
                string s = pontosEmFalta == 1 ? "ponto" : "pontos";

                MessageBox.Show(String.Format("Acrescente mais {0} {1}", pontosEmFalta, s));
            }
        }

        private void btnLimparPainel_Click(object sender, EventArgs e)
        {
            if (poligonos.Count > 0)
            {
                if (MessageBox.Show("Tem certeza que quer limpar o ecrã?", "Limpar Ecrã", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    LimparPainel();
                }
                return;
            }

            MessageBox.Show("O painel já está limpo");
        }

        private void btnMudarCor_Click(object sender, EventArgs e)
        {
            if (cdCorPreenchimento.ShowDialog() == DialogResult.OK)
            {
                graphPanel.Clear(fundo = cdCorPreenchimento.Color);
                tracejado.Color = CorComplementar(tracejado.Color);

                if (poligonos.Count > 0)
                {
                    foreach (Poligono p in poligonos)
                    {
                        DesenharPoligono(p);
                    }
                }
            }
        }

        private void btnApagarPoligono_Click(object sender, EventArgs e)
        {
            if (poligonos.Count == 0)
            {
                MessageBox.Show("Não há nenhum poligono para apagar!");
                return;
            }

            int indice = cbPoligonos.SelectedIndex;
            cbPoligonos.Text = "";

            if (indice >= 0)
            {
                ApagarPoligono(indice);
                return;
            }

            MessageBox.Show("Escolha um poligono");
        }

        private void btnInverterCores_Click(object sender, EventArgs e)
        {
            graphPanel.Clear(fundo = CorComplementar(fundo));
            tracejado.Color = CorComplementar(tracejado.Color);

            if (poligonos.Count == 0)
                return;

            foreach (Poligono p in poligonos)
            {
                p.Preechimento = CorComplementar(p.Preechimento);
                p.Identificador.BackColor = CorComplementar(p.Identificador.BackColor);
                p.Identificador.ForeColor = CorComplementar(p.Identificador.ForeColor);
                DesenharPoligono(p);
            }

        }

        private void checkbIdentificarPoligonos_CheckedChanged(object sender, EventArgs e)
        {
            if (poligonos.Count == 0)
                return;

            if (checkbIdentificarPoligonos.Checked)
            {
                foreach (Poligono p in poligonos)
                    painel.Controls.Add(p.Identificador);
            }
            else
            {
                foreach (Poligono p in poligonos)
                    painel.Controls.Remove(p.Identificador);

                graphPanel.Clear(fundo);

                foreach (Poligono p in poligonos)
                    DesenharPoligono(p);
            }
        }

        private void LimparPainel()
        {
            foreach (Poligono p in poligonos)
                painel.Controls.Remove(p.Identificador);

            graphPanel.Clear(fundo);
            poligonos.Clear();
            cbPoligonos.Items.Clear();
            cbPoligonos.Text = "";
            cbPoligonos.Enabled = false;
        }

        private void ApagarPoligono(int indice)
        {
            painel.Controls.Remove(poligonos[indice].Identificador);
            poligonos.RemoveAt(indice);
            CarregaCombobox(cbPoligonos, new List<object>(poligonos));

            graphPanel.Clear(fundo);

            if (poligonos.Count > 0)
            {
                foreach (Poligono p in poligonos)
                    DesenharPoligono(p);
            }
            else
                cbPoligonos.Enabled = false;
        }

        private void CriarPoligono(int espessura, Color cor)
        {
            Poligono poligono = new(new List<Point>(pontos), espessura, cor);

            poligonos.Add(poligono);
            CarregaCombobox(cbPoligonos, new List<object>(poligonos));
            cbPoligonos.Enabled = true;

            DesenharPoligono(poligono);
            if (checkbIdentificarPoligonos.Checked)
                painel.Controls.Add(poligono.Identificador);

            pontos.Clear();
        }

        private void DesenharPoligono(Poligono p)
        {
            Pen caneta = new(Color.Black, p.Espessura);
            SolidBrush preenchimento = new(p.Preechimento);

            graphPanel.FillPolygon(preenchimento, p.Points.ToArray());
            graphPanel.DrawPolygon(caneta, p.Points.ToArray());
        }

        public static void CarregaCombobox(ComboBox cmb, List<object> lista)
        {
            cmb.Items.Clear();
            cmb.Items.AddRange(lista.ToArray());
        }

        public static Color CorComplementar(Color cor)
        {
            return Color.FromArgb(255 - cor.R, 255 - cor.G, 255 - cor.B);
        }
    }
}
