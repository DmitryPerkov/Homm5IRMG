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
    public partial class frmMines : Form
    {
        public int iZoneIndex = 1;
        public frmMines()
        {
            InitializeComponent();
        }

        private void frmMines_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        /// <summary>
        /// Handles saveing of data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMines_FormClosing(object sender, FormClosingEventArgs e)
        {

            //save all data set in form to template
            #region Mine Count
            int[] iarrMines = new int [TemplateHandler.IMINENUMBER];            

            iarrMines[(int)eMines.Sawmill] = Convert.ToInt32( msktxtWood.Text);
            iarrMines[(int)eMines.Ore_Pit] = Convert.ToInt32(msktxtOre.Text);
            iarrMines[(int)eMines.Sulfur_Dune] = Convert.ToInt32(msktxtSulfur.Text);
            iarrMines[(int)eMines.Gold_Mine] = Convert.ToInt32(msktxtGold.Text);
            iarrMines[(int)eMines.Crystal_Cavern] = Convert.ToInt32(msktxtCrystal.Text);
            iarrMines[(int)eMines.Gem_Pond] = Convert.ToInt32(msktxtGem.Text);
            iarrMines[(int)eMines.Alchemist_Lab] = Convert.ToInt32(msktxtMercury.Text);
            iarrMines[(int)eMines.Abandoned_Mine] = Convert.ToInt32(msktxtAbandoned.Text);
            iarrMines[(int)eMines.Player_Related] = Convert.ToInt32(msktxtPlayer_Related.Text);

            Program.frmTemplateEdit.thTemplate.SetMinesData(iZoneIndex, iarrMines);
            #endregion

            #region Mines Value
            int[] iarrMinesValue = new int[TemplateHandler.IMINENUMBER];

            iarrMinesValue[(int)eMines.Sawmill] = Convert.ToInt32(msktxtWoodValue.Text);
            iarrMinesValue[(int)eMines.Ore_Pit] = Convert.ToInt32(msktxtOreValue.Text);
            iarrMinesValue[(int)eMines.Sulfur_Dune] = Convert.ToInt32(msktxtSulfurValue.Text);
            iarrMinesValue[(int)eMines.Gold_Mine] = Convert.ToInt32(msktxtGoldValue.Text);
            iarrMinesValue[(int)eMines.Crystal_Cavern] = Convert.ToInt32(msktxtCrystalValue.Text);
            iarrMinesValue[(int)eMines.Gem_Pond] = Convert.ToInt32(msktxtGemValue.Text);
            iarrMinesValue[(int)eMines.Alchemist_Lab] = Convert.ToInt32(msktxtMercuryValue.Text);
            iarrMinesValue[(int)eMines.Abandoned_Mine] = Convert.ToInt32(msktxtAbandonedValue.Text);
            iarrMinesValue[(int)eMines.Player_Related] = Convert.ToInt32(msktxtPlayer_RelatedValue.Text);

            Program.frmTemplateEdit.thTemplate.SetMinesAdditionalData(iZoneIndex, iarrMinesValue , "Value" );

            #endregion


            #region Mines Chance
            string[] strarrMinesChance = new string[TemplateHandler.IMINENUMBER];

            strarrMinesChance[(int)eMines.Sawmill] = msktxtWoodChance.Text;
            strarrMinesChance[(int)eMines.Ore_Pit] = msktxtOreChance.Text;
            strarrMinesChance[(int)eMines.Sulfur_Dune] = msktxtSulfurChance.Text;
            strarrMinesChance[(int)eMines.Gold_Mine] = msktxtGoldChance.Text;
            strarrMinesChance[(int)eMines.Crystal_Cavern] = msktxtCrystalChance.Text;
            strarrMinesChance[(int)eMines.Gem_Pond] = msktxtGemChance.Text;
            strarrMinesChance[(int)eMines.Alchemist_Lab] = msktxtMercuryChance.Text;
            strarrMinesChance[(int)eMines.Abandoned_Mine] = msktxtAbandonedChance.Text;
            strarrMinesChance[(int)eMines.Player_Related] = msktxtPlayer_RelatedChance.Text;

            Program.frmTemplateEdit.thTemplate.SetMinesAdditionalData(iZoneIndex, strarrMinesChance, "Chance");

            #endregion

            //set playeriddata
            Program.frmTemplateEdit.thTemplate.SetMinesPlayerIdData(iZoneIndex, cboPlayer_Related.SelectedIndex + 1);

            Program.frmTemplateEdit.bWasShowing = false;
            e.Cancel = true;
            this.Hide();
            Program.frmTemplateEdit.Refresh();
        }

        private void frmMines_Load(object sender, EventArgs e)
        {

        }



        public void SetPreviewVisible()
        {
            this.lblPreview.Visible = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics formGraphics = e.Graphics;
            System.Drawing.Drawing2D.LinearGradientBrush gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), Color.LightSlateGray, Color.Silver);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle);


            //base.OnPaint(e);
        }

        private void frmMines_Shown(object sender, EventArgs e)
        {

        }

        //get data from template file
        private void frmMines_VisibleChanged(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Opacity = 1;
            this.lblPreview.Visible = false;

            //get data from template
            int [] iarrMines = Program.frmTemplateEdit.thTemplate.GetMinesData(iZoneIndex);
            msktxtWood.Text = iarrMines[(int)eMines.Sawmill].ToString();
            msktxtOre.Text = iarrMines[(int)eMines.Ore_Pit].ToString();
            msktxtSulfur.Text = iarrMines[(int)eMines.Sulfur_Dune].ToString();
            msktxtGold.Text = iarrMines[(int)eMines.Gold_Mine].ToString();
            msktxtCrystal.Text = iarrMines[(int)eMines.Crystal_Cavern].ToString();
            msktxtGem.Text = iarrMines[(int)eMines.Gem_Pond].ToString();
            msktxtMercury.Text = iarrMines[(int)eMines.Alchemist_Lab].ToString();
            msktxtAbandoned.Text = iarrMines[(int)eMines.Abandoned_Mine].ToString();
            msktxtPlayer_Related.Text = iarrMines[(int)eMines.Player_Related].ToString();
            checkBox1.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetLinkAttribute(iZoneIndex, "Mines"));

            //get data from template
            string[] strarrMinesValue = Program.frmTemplateEdit.thTemplate.GetAdditionalMinesData(iZoneIndex, "Value");
            msktxtWoodValue.Text = strarrMinesValue[(int)eMines.Sawmill].ToString();
            msktxtOreValue.Text = strarrMinesValue[(int)eMines.Ore_Pit].ToString();
            msktxtSulfurValue.Text = strarrMinesValue[(int)eMines.Sulfur_Dune].ToString();
            msktxtGoldValue.Text = strarrMinesValue[(int)eMines.Gold_Mine].ToString();
            msktxtCrystalValue.Text = strarrMinesValue[(int)eMines.Crystal_Cavern].ToString();
            msktxtGemValue.Text = strarrMinesValue[(int)eMines.Gem_Pond].ToString();
            msktxtMercuryValue.Text = strarrMinesValue[(int)eMines.Alchemist_Lab].ToString();
            msktxtAbandonedValue.Text = strarrMinesValue[(int)eMines.Abandoned_Mine].ToString();
            msktxtPlayer_RelatedValue.Text = strarrMinesValue[(int)eMines.Player_Related].ToString();



            //get data from template
            string[] strarrMinesChance = Program.frmTemplateEdit.thTemplate.GetAdditionalMinesData(iZoneIndex, "Chance");
            msktxtWoodChance.Text = strarrMinesChance[(int)eMines.Sawmill].ToString();
            msktxtOreChance.Text = strarrMinesChance[(int)eMines.Ore_Pit].ToString();
            msktxtSulfurChance.Text = strarrMinesChance[(int)eMines.Sulfur_Dune].ToString();
            msktxtGoldChance.Text = strarrMinesChance[(int)eMines.Gold_Mine].ToString();
            msktxtCrystalChance.Text = strarrMinesChance[(int)eMines.Crystal_Cavern].ToString();
            msktxtGemChance.Text = strarrMinesChance[(int)eMines.Gem_Pond].ToString();
            msktxtMercuryChance.Text = strarrMinesChance[(int)eMines.Alchemist_Lab].ToString();
            msktxtAbandonedChance.Text = strarrMinesChance[(int)eMines.Abandoned_Mine].ToString();
            msktxtPlayer_RelatedChance.Text = strarrMinesChance[(int)eMines.Player_Related].ToString();

            cboPlayer_Related.SelectedIndex = Convert.ToInt32 ( Program.frmTemplateEdit.thTemplate.GetMinesPlayerIdData(iZoneIndex) ) - 1;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           Program.frmTemplateEdit.thTemplate.SetLinkAttribute(iZoneIndex, "Mines", checkBox1.Checked.ToString());
           if (checkBox1.Checked)
           {//dwp необходимо подстветить галочки в каждой зоне в каждом сете, если их там нет конечно
               XmlElement xObjects1, xObjects2, xObjects3;
               XmlNode xndNodeToTemplate, Xlink, xndFromObjects;
               
               ObjectsReader obrdUpdater = new ObjectsReader();

               xndFromObjects = obrdUpdater.GetObjectsData().SelectSingleNode("//Object[@Name='Mines']");
               xObjects1 = Program.frmTemplateEdit.thTemplate.GetObjectsSet(iZoneIndex, 1);
               xndNodeToTemplate = xObjects1.SelectSingleNode(".//Object[@Name='Mines']");

               if (xndNodeToTemplate == null) {
                   Xlink = xndFromObjects.CloneNode(true);
                   xObjects1.SelectSingleNode(".//BattleObjects").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
               }
               xObjects2 = Program.frmTemplateEdit.thTemplate.GetObjectsSet(iZoneIndex, 2);
               xndNodeToTemplate = xObjects2.SelectSingleNode(".//Object[@Name='Mines']");
               if (xndNodeToTemplate == null) {
                   Xlink = xndFromObjects.CloneNode(true);
                   xObjects2.SelectSingleNode(".//BattleObjects").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
               }
               xObjects3 = Program.frmTemplateEdit.thTemplate.GetObjectsSet(iZoneIndex, 3);
               xndNodeToTemplate = xObjects3.SelectSingleNode(".//Object[@Name='Mines']");
               if (xndNodeToTemplate == null) {
                   Xlink = xndFromObjects.CloneNode(true);
                   xObjects3.SelectSingleNode(".//BattleObjects").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
               }
           }
        }
    }
}