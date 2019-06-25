using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml; 

namespace Homm5RMG
{
    //enum GuardOrigin
    //{
    //    Connection = 0,
    //    Towns ,

    //}


    public partial class frmPickGuard : Form
    {
        private bool bIsTownMode = false;
        public int iConnection;
        public frmPickGuard()
        {
            InitializeComponent();
        }

        private void frmPickGuard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Guards gTemplateXMLGenerator = new Guards();

            if (radbtnNormal.Checked)
            {
                XmlElement xTemplateGuard = gTemplateXMLGenerator.GetTemplateGuardXML( Convert.ToInt32( msktxtMainValue.Text ));
                if (bIsTownMode)
                {

                }
                else
                {
                    //set connection guard value
                    Program.frmTemplateEdit.thTemplate.ImportGuardToConnection(xTemplateGuard ,iConnection);
                }
            }


            bIsTownMode = false;
            e.Cancel = true;
            this.Hide();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics formGraphics = e.Graphics;
            System.Drawing.Drawing2D.LinearGradientBrush gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), Color.LightSlateGray, Color.Silver);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle);


            //base.OnPaint(e);
        }

        public void EnableTownMode()
        {

            bIsTownMode = true;
        }

        private void frmPickGuard_Load(object sender, EventArgs e)
        {


        }

        private void frmPickGuard_Activated(object sender, EventArgs e)
        {
            if (bIsTownMode)
            {
                radbtnInsideGate.Visible = false;
                radbtnInsideHero.Visible = false;
                radbtnNormal.Visible = false;
            }
            else
            {
                radbtnInsideGate.Visible = true;
                radbtnInsideHero.Visible = true;
                radbtnNormal.Visible = true;
            }

            //get value of connection 
            msktxtMainValue.Text = Program.frmTemplateEdit.thTemplate.GetConnectionGuardValue(iConnection  );


            //Guards gReader = new Guards();

            //System.Collections.ArrayList strarrGuardList = gReader.GetGuardsNameList();


            //#region AddGuardsListToCombos
            //object[] objarrGuardList = strarrGuardList.ToArray();

            //cboMonsterSlot1.Items.AddRange(objarrGuardList);
            //cboMonsterSlot2.Items.AddRange(objarrGuardList);
            //cboMonsterSlot3.Items.AddRange(objarrGuardList);
            //cboMonsterSlot4.Items.AddRange(objarrGuardList);
            //cboMonsterSlot5.Items.AddRange(objarrGuardList);
            //cboMonsterSlot6.Items.AddRange(objarrGuardList);
            //cboMonsterSlot7.Items.AddRange(objarrGuardList); 
            //#endregion

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radbtnNormal_CheckedChanged(object sender, EventArgs e)
        {
            lblNumber.Visible = false;

            msktxtMainValue.Visible = true;
            lblMainValue.Visible = true;

            msktxtValueSlot1.Visible = false;
            msktxtValueSlot2.Visible = false;
            msktxtValueSlot3.Visible = false;
            msktxtValueSlot4.Visible = false;
            msktxtValueSlot5.Visible = false;
            msktxtValueSlot6.Visible = false;
            msktxtValueSlot7.Visible = false;

            lblSlot1.Visible = false;
            lblSlot2.Visible = false;
            lblSlot3.Visible = false;
            lblSlot4.Visible = false;
            lblSlot5.Visible = false;
            lblSlot6.Visible = false;
            lblSlot7.Visible = false;

            cboMonsterSlot1.Visible = false;
            cboMonsterSlot2.Visible = false;
            cboMonsterSlot3.Visible = false;
            cboMonsterSlot4.Visible = false;
            cboMonsterSlot5.Visible = false;
            cboMonsterSlot6.Visible = false;
            cboMonsterSlot7.Visible = false;

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void radbtnInsideGate_CheckedChanged(object sender, EventArgs e)
        {
            lblNumber.Visible = true;

            msktxtMainValue.Visible = false;
            lblMainValue.Visible = false;

            msktxtValueSlot1.Visible = true;
            msktxtValueSlot2.Visible = true;
            msktxtValueSlot3.Visible = true;
            msktxtValueSlot4.Visible = true;
            msktxtValueSlot5.Visible = true;
            msktxtValueSlot6.Visible = true;
            msktxtValueSlot7.Visible = true;

            lblSlot1.Visible = true;
            lblSlot2.Visible = true;
            lblSlot3.Visible = true;
            lblSlot4.Visible = true;
            lblSlot5.Visible = true;
            lblSlot6.Visible = true;
            lblSlot7.Visible = true;

            cboMonsterSlot1.Visible = true;
            cboMonsterSlot2.Visible = true;
            cboMonsterSlot3.Visible = true;
            cboMonsterSlot4.Visible = true;
            cboMonsterSlot5.Visible = true;
            cboMonsterSlot6.Visible = true;
            cboMonsterSlot7.Visible = true;

        }

        private void radbtnInsideHero_CheckedChanged(object sender, EventArgs e)
        {
            lblNumber.Visible = true;

            msktxtMainValue.Visible = false;
            lblMainValue.Visible = false;

            msktxtValueSlot1.Visible = true;
            msktxtValueSlot2.Visible = true;
            msktxtValueSlot3.Visible = true;
            msktxtValueSlot4.Visible = true;
            msktxtValueSlot5.Visible = true;
            msktxtValueSlot6.Visible = true;
            msktxtValueSlot7.Visible = true;

            lblSlot1.Visible = true;
            lblSlot2.Visible = true;
            lblSlot3.Visible = true;
            lblSlot4.Visible = true;
            lblSlot5.Visible = true;
            lblSlot6.Visible = true;
            lblSlot7.Visible = true;


            cboMonsterSlot1.Visible = true;
            cboMonsterSlot2.Visible = true;
            cboMonsterSlot3.Visible = true;
            cboMonsterSlot4.Visible = true;
            cboMonsterSlot5.Visible = true;
            cboMonsterSlot6.Visible = true;
            cboMonsterSlot7.Visible = true;
        }

        private void radbtnCustom_CheckedChanged(object sender, EventArgs e)
        {
            lblNumber.Visible = true;

            msktxtMainValue.Visible = false;
            lblMainValue.Visible = false;

            msktxtValueSlot1.Visible = true;
            msktxtValueSlot2.Visible = true;
            msktxtValueSlot3.Visible = true;
            msktxtValueSlot4.Visible = true;
            msktxtValueSlot5.Visible = true;
            msktxtValueSlot6.Visible = true;
            msktxtValueSlot7.Visible = true;

            lblSlot1.Visible = true;
            lblSlot2.Visible = true;
            lblSlot3.Visible = true;
            lblSlot4.Visible = true;
            lblSlot5.Visible = true;
            lblSlot6.Visible = true;
            lblSlot7.Visible = true;

            cboMonsterSlot1.Visible = true;
            cboMonsterSlot2.Visible = true;
            cboMonsterSlot3.Visible = true;
            cboMonsterSlot4.Visible = true;
            cboMonsterSlot5.Visible = true;
            cboMonsterSlot6.Visible = true;
            cboMonsterSlot7.Visible = true;
        }


    }
}