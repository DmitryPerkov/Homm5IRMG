using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Homm5RMG
{
    public partial class frmAddCustomObject : Form
    {
        public frmAddCustomObject()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics formGraphics = e.Graphics;
            System.Drawing.Drawing2D.LinearGradientBrush gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), Color.LightSlateGray, Color.Silver);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle);


            //base.OnPaint(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}