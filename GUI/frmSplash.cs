using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Homm5RMG
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Hide();
            Program.frmMainMenu.Show();
            timer1.Enabled = false;
            timer2.Enabled = false;                    
            
            
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.03;
            if (this.Opacity <= 0)
            {

                
            }
        }
    }
}