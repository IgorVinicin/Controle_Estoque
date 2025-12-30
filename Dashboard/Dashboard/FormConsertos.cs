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

namespace Dashboard
{
    public partial class FormConsertos : Form
    {
        public FormConsertos()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true; // redesenha ao redimensionar
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCadastrarProd_Click(object sender, EventArgs e)
        {

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // NÃO chamar base.OnPaintBackground(e);
            // Isso evita que o fundo padrão sobrescreva o degradê

            Rectangle area = this.ClientRectangle;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                area,
                Color.Black,
                Color.Black,
                LinearGradientMode.Vertical))
            {
                ColorBlend blend = new ColorBlend
                {
                    Colors = new Color[]
                    {
                Color.FromArgb(2, 6, 23),    // #020617 (topo)
                Color.FromArgb(15, 23, 42),  // #0F172A (meio)
                Color.FromArgb(30, 41, 59)   // #1E293B (base)
                    },
                    Positions = new float[] { 0f, 0.5f, 1f }
                };

                brush.InterpolationColors = blend;
                e.Graphics.FillRectangle(brush, area);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
}
