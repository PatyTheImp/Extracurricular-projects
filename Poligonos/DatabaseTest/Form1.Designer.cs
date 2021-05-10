
namespace DatabaseTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.painel = new System.Windows.Forms.Panel();
            this.btnLigarPontos = new System.Windows.Forms.Button();
            this.lblEspessura = new System.Windows.Forms.Label();
            this.nupdEspessura = new System.Windows.Forms.NumericUpDown();
            this.cdCorPreenchimento = new System.Windows.Forms.ColorDialog();
            this.lblPoligonos = new System.Windows.Forms.Label();
            this.cbPoligonos = new System.Windows.Forms.ComboBox();
            this.btnLimparPainel = new System.Windows.Forms.Button();
            this.btnMudarCor = new System.Windows.Forms.Button();
            this.btnApagarPoligono = new System.Windows.Forms.Button();
            this.btnInverterCores = new System.Windows.Forms.Button();
            this.checkbIdentificarPoligonos = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nupdEspessura)).BeginInit();
            this.SuspendLayout();
            // 
            // painel
            // 
            this.painel.BackColor = System.Drawing.Color.White;
            this.painel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.painel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.painel.Location = new System.Drawing.Point(192, 12);
            this.painel.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.painel.Name = "painel";
            this.painel.Size = new System.Drawing.Size(563, 306);
            this.painel.TabIndex = 0;
            this.painel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.painel_MouseClick);
            // 
            // btnLigarPontos
            // 
            this.btnLigarPontos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLigarPontos.Location = new System.Drawing.Point(12, 43);
            this.btnLigarPontos.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.btnLigarPontos.Name = "btnLigarPontos";
            this.btnLigarPontos.Size = new System.Drawing.Size(157, 23);
            this.btnLigarPontos.TabIndex = 2;
            this.btnLigarPontos.Text = "COMPLETAR POLIGONO";
            this.btnLigarPontos.UseVisualStyleBackColor = true;
            this.btnLigarPontos.Click += new System.EventHandler(this.btnLigarPontos_Click);
            // 
            // lblEspessura
            // 
            this.lblEspessura.AutoSize = true;
            this.lblEspessura.Location = new System.Drawing.Point(12, 14);
            this.lblEspessura.Name = "lblEspessura";
            this.lblEspessura.Size = new System.Drawing.Size(67, 15);
            this.lblEspessura.TabIndex = 3;
            this.lblEspessura.Text = "ESPESSURA";
            // 
            // nupdEspessura
            // 
            this.nupdEspessura.Location = new System.Drawing.Point(85, 12);
            this.nupdEspessura.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.nupdEspessura.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nupdEspessura.Name = "nupdEspessura";
            this.nupdEspessura.Size = new System.Drawing.Size(84, 23);
            this.nupdEspessura.TabIndex = 4;
            // 
            // lblPoligonos
            // 
            this.lblPoligonos.AutoSize = true;
            this.lblPoligonos.Location = new System.Drawing.Point(12, 81);
            this.lblPoligonos.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            this.lblPoligonos.Name = "lblPoligonos";
            this.lblPoligonos.Size = new System.Drawing.Size(76, 15);
            this.lblPoligonos.TabIndex = 5;
            this.lblPoligonos.Text = "POLIGONOS:";
            // 
            // cbPoligonos
            // 
            this.cbPoligonos.Enabled = false;
            this.cbPoligonos.FormattingEnabled = true;
            this.cbPoligonos.Location = new System.Drawing.Point(12, 104);
            this.cbPoligonos.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.cbPoligonos.Name = "cbPoligonos";
            this.cbPoligonos.Size = new System.Drawing.Size(157, 23);
            this.cbPoligonos.TabIndex = 6;
            // 
            // btnLimparPainel
            // 
            this.btnLimparPainel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimparPainel.Location = new System.Drawing.Point(12, 295);
            this.btnLimparPainel.Name = "btnLimparPainel";
            this.btnLimparPainel.Size = new System.Drawing.Size(157, 23);
            this.btnLimparPainel.TabIndex = 7;
            this.btnLimparPainel.Text = "LIMPAR PAINEL";
            this.btnLimparPainel.UseVisualStyleBackColor = true;
            this.btnLimparPainel.Click += new System.EventHandler(this.btnLimparPainel_Click);
            // 
            // btnMudarCor
            // 
            this.btnMudarCor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMudarCor.Location = new System.Drawing.Point(12, 254);
            this.btnMudarCor.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.btnMudarCor.Name = "btnMudarCor";
            this.btnMudarCor.Size = new System.Drawing.Size(157, 23);
            this.btnMudarCor.TabIndex = 8;
            this.btnMudarCor.Text = "MUDAR COR DE FUNDO";
            this.btnMudarCor.UseVisualStyleBackColor = true;
            this.btnMudarCor.Click += new System.EventHandler(this.btnMudarCor_Click);
            // 
            // btnApagarPoligono
            // 
            this.btnApagarPoligono.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApagarPoligono.Location = new System.Drawing.Point(12, 135);
            this.btnApagarPoligono.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.btnApagarPoligono.Name = "btnApagarPoligono";
            this.btnApagarPoligono.Size = new System.Drawing.Size(157, 23);
            this.btnApagarPoligono.TabIndex = 9;
            this.btnApagarPoligono.Text = "APAGAR POLIGONO";
            this.btnApagarPoligono.UseVisualStyleBackColor = true;
            this.btnApagarPoligono.Click += new System.EventHandler(this.btnApagarPoligono_Click);
            // 
            // btnInverterCores
            // 
            this.btnInverterCores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInverterCores.Location = new System.Drawing.Point(12, 213);
            this.btnInverterCores.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.btnInverterCores.Name = "btnInverterCores";
            this.btnInverterCores.Size = new System.Drawing.Size(157, 23);
            this.btnInverterCores.TabIndex = 10;
            this.btnInverterCores.Text = "INVERTER CORES";
            this.btnInverterCores.UseVisualStyleBackColor = true;
            this.btnInverterCores.Click += new System.EventHandler(this.btnInverterCores_Click);
            // 
            // checkbIdentificarPoligonos
            // 
            this.checkbIdentificarPoligonos.AutoSize = true;
            this.checkbIdentificarPoligonos.Location = new System.Drawing.Point(12, 176);
            this.checkbIdentificarPoligonos.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.checkbIdentificarPoligonos.Name = "checkbIdentificarPoligonos";
            this.checkbIdentificarPoligonos.Size = new System.Drawing.Size(162, 19);
            this.checkbIdentificarPoligonos.TabIndex = 11;
            this.checkbIdentificarPoligonos.Text = "IDENTIFICAR POLIGONOS";
            this.checkbIdentificarPoligonos.UseVisualStyleBackColor = true;
            this.checkbIdentificarPoligonos.CheckedChanged += new System.EventHandler(this.checkbIdentificarPoligonos_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(767, 330);
            this.Controls.Add(this.checkbIdentificarPoligonos);
            this.Controls.Add(this.btnInverterCores);
            this.Controls.Add(this.btnApagarPoligono);
            this.Controls.Add(this.btnMudarCor);
            this.Controls.Add(this.btnLimparPainel);
            this.Controls.Add(this.cbPoligonos);
            this.Controls.Add(this.lblPoligonos);
            this.Controls.Add(this.nupdEspessura);
            this.Controls.Add(this.lblEspessura);
            this.Controls.Add(this.btnLigarPontos);
            this.Controls.Add(this.painel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(232)))), ((int)(((byte)(223)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = ".: POLIGONOS DA PATY :.";
            ((System.ComponentModel.ISupportInitialize)(this.nupdEspessura)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel painel;
        private System.Windows.Forms.Button btnLigarPontos;
        private System.Windows.Forms.Label lblEspessura;
        private System.Windows.Forms.NumericUpDown nupdEspessura;
        private System.Windows.Forms.ColorDialog cdCorPreenchimento;
        private System.Windows.Forms.Label lblPoligonos;
        private System.Windows.Forms.ComboBox cbPoligonos;
        private System.Windows.Forms.Button btnLimparPainel;
        private System.Windows.Forms.Button btnMudarCor;
        private System.Windows.Forms.Button btnApagarPoligono;
        private System.Windows.Forms.Button btnInverterCores;
        private System.Windows.Forms.CheckBox checkbIdentificarPoligonos;
    }
}

