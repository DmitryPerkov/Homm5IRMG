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
    public partial class frmTowns : Form
    {
        //get its value when button is clicked in other form
        public int iZoneIndex = 1;

        public frmTowns()
        {
            InitializeComponent();
        }

        private void frmTowns_Load(object sender, EventArgs e)
        {


            //set town type combo values
            ComboBox[] cboCombos = new ComboBox[3];
            cboCombos[0] = cboTownType;
            cboCombos[1] = cboTownType2;
            cboCombos[2] = cboTownType3;


            string[] strarrNames = Enum.GetNames(typeof(eTownType));
            foreach (string strTownType in strarrNames)
            {
                for (int i = 0; i < 3; i++)
                {
                    cboCombos[i].Items.Add(strTownType);
                }
            }

            //set Income level combo values
            cboCombos[0] = cboIncomeLevel;
            cboCombos[1] = cboIncomeLevel2;
            cboCombos[2] = cboIncomeLevel3;
            strarrNames = Enum.GetNames(typeof(eIncome));
            foreach (string strIncome in strarrNames)
            {
                for (int i = 0; i < 3; i++)
                {
                    cboCombos[i].Items.Add(strIncome);
                }
            }

            
            //set Town Walls combo values
            cboCombos[0] = cboTownWalls;
            cboCombos[1] = cboTownWalls2;
            cboCombos[2] = cboTownWalls3;
            strarrNames = Enum.GetNames(typeof(eWalls));
            foreach (string strWalls in strarrNames)
            {
                for (int i = 0; i < 3; i++)
                {
                    cboCombos[i].Items.Add(strWalls);
                }
            }

            //set Town Resources combo values
            cboCombos[0] = cboResources;
            cboCombos[1] = cboResources2;
            cboCombos[2] = cboResources3;
            strarrNames = Enum.GetNames(typeof(eResource));
            foreach (string strResources in strarrNames)
            {
                for (int i = 0; i < 3; i++)
                {
                    cboCombos[i].Items.Add(strResources);
                }
            }
            //if (cboIncomeLevel.SelectedIndex == -1)
            //{
            //    cboIncomeLevel.SelectedIndex = 0;
            //    cboTownType.SelectedIndex = 0;
            //    cboTownWalls.SelectedIndex = 0;
            //    cboResources.SelectedIndex = 0;
            //}
            //if (cboIncomeLevel2.SelectedIndex == -1)
            //{
            //    cboIncomeLevel2.SelectedIndex = 0;
            //    cboTownType2.SelectedIndex = 0;
            //    cboTownWalls2.SelectedIndex = 0;
            //    cboResources2.SelectedIndex = 0;
            //}
            //if (cboIncomeLevel3.SelectedIndex == -1)
            //{
            //    cboIncomeLevel3.SelectedIndex = 0;
            //    cboTownType3.SelectedIndex = 0;
            //    cboTownWalls3.SelectedIndex = 0;
            //    cboResources3.SelectedIndex = 0;
            //}
            
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics formGraphics = e.Graphics;
            System.Drawing.Drawing2D.LinearGradientBrush gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), Color.LightSlateGray, Color.Silver);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle);


            //base.OnPaint(e);
        }

        public void SetPreviewVisible()
        {
            this.lblPreview.Visible = true;
        }

        private void frmTowns_FormClosing(object sender, FormClosingEventArgs e)
        {

            //Program.frmTemplateEdit.thTemplate.
            
            Program.frmTemplateEdit.bWasShowing = false;
            e.Cancel = true;
            this.Hide();
            Program.frmTemplateEdit.Refresh();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > '5' || e.KeyChar == '0')
            {
                MessageBox.Show("Error the mage guild level can be between 1-5 only");
                e.KeyChar = msktxtMageLevel.Text[0];
            }
            //maskedTextBox1.Text = "1" ;
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void cboTownType_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (cboTownType.SelectedItem.ToString() == "None")
            {

                cboResources.Enabled = false;
                cboIncomeLevel.Enabled = false;
                cboTownWalls.Enabled = false;

                chkboxBlacksmith.Enabled = false;
                chkboxDwellings.Enabled = false;
                chkboxIsStarting.Enabled = false;
                chkboxMageGuild.Enabled = false;
                chkboxSpecial.Enabled = false;
                chkboxTavern.Enabled = false;
                chkboxUpgraded.Enabled = false;
                msktxtDwelling.Enabled = false;
                msktxtMageLevel.Enabled = false;
                btnSetGuards.Enabled = false;

            }
            else
            {
                cboResources.Enabled = true;
                cboIncomeLevel.Enabled = true;
                cboTownWalls.Enabled = true;

                chkboxBlacksmith.Enabled = true;
                chkboxDwellings.Enabled = true;
                chkboxIsStarting.Enabled = true;
                chkboxMageGuild.Enabled = true;
                //chkboxSpecial.Enabled = true;
                chkboxTavern.Enabled = true;
                chkboxUpgraded.Enabled = true;
                msktxtDwelling.Enabled = true;
                msktxtMageLevel.Enabled = true;
                //btnSetGuards.Enabled = true;
            }

            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.Type.ToString(), cboTownType.SelectedItem.ToString());
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmTowns_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible) //load data if form is in visible state
            {
                this.FormBorderStyle = FormBorderStyle.Fixed3D;
                this.Opacity = 1;
                this.lblPreview.Visible = false;

                //load town 1 data
                msktxtMageLevel.Text = Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.MageGuildLevel.ToString());
                msktxtDwelling.Text = Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.DwellingLevel.ToString());
                cboTownWalls.SelectedIndex = (int)Enum.Parse(typeof(eWalls), Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.TownWalls.ToString()));
                cboResources.SelectedIndex = (int)Enum.Parse(typeof(eResource), Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.ResourceLevel.ToString()));
                cboIncomeLevel.SelectedIndex =  (int)  Enum.Parse( typeof(eIncome) , Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.IncomeLevel.ToString()) );
                cboTownType.SelectedIndex = (int)  Enum.Parse( typeof(eTownType) , Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.Type.ToString()) );
                chkboxBlacksmith.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.BlackSmith.ToString()));
                checkBox1.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetLinkAttribute(iZoneIndex, "Towns"));
                chkboxTavern.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.Tavern.ToString()));
                chkboxMageGuild.Checked = Convert.ToBoolean(Convert.ToInt32(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.MageGuildLevel.ToString())));
                chkboxIsStarting.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.IsStartingTown.ToString()));
                chkboxDwellings.Checked = Convert.ToBoolean(Convert.ToInt32(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.DwellingLevel.ToString())));
                chkboxUpgraded.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.DwellingsUpgrades.ToString()));
                chkboxSpecial.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 1, eTown.TownSpecialStracture.ToString()));

                //load town 1 data
                msktxtMageLevel2.Text = Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.MageGuildLevel.ToString());
                msktxtDwelling2.Text = Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.DwellingLevel.ToString());
                cboTownWalls2.SelectedIndex = (int)Enum.Parse(typeof(eWalls), Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.TownWalls.ToString()));
                cboResources2.SelectedIndex = (int)Enum.Parse(typeof(eResource), Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.ResourceLevel.ToString()));
                cboIncomeLevel2.SelectedIndex = (int)Enum.Parse(typeof(eIncome), Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.IncomeLevel.ToString()));
                cboTownType2.SelectedIndex = (int)Enum.Parse(typeof(eTownType), Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.Type.ToString()));
                chkboxBlacksmith2.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.BlackSmith.ToString()));
                chkboxTavern2.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.Tavern.ToString()));
                chkboxMageGuild2.Checked = Convert.ToBoolean(Convert.ToInt32(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.MageGuildLevel.ToString())));
                chkboxIsStarting2.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.IsStartingTown.ToString()));
                chkboxDwellings2.Checked = Convert.ToBoolean(Convert.ToInt32(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.DwellingLevel.ToString())));
                chkboxUpgraded2.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.DwellingsUpgrades.ToString()));
                chkboxSpecial2.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 2, eTown.TownSpecialStracture.ToString()));

                //load town 1 data
                msktxtMageLevel3.Text = Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.MageGuildLevel.ToString());
                msktxtDwelling3.Text = Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.DwellingLevel.ToString());
                cboTownWalls3.SelectedIndex = (int)Enum.Parse(typeof(eWalls), Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.TownWalls.ToString()));
                cboResources3.SelectedIndex = (int)Enum.Parse(typeof(eResource), Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.ResourceLevel.ToString()));
                cboIncomeLevel3.SelectedIndex = (int)Enum.Parse(typeof(eIncome), Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.IncomeLevel.ToString()));
                cboTownType3.SelectedIndex = (int)Enum.Parse(typeof(eTownType), Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.Type.ToString()));
                chkboxBlacksmith3.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.BlackSmith.ToString()));
                chkboxTavern3.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.Tavern.ToString()));
                chkboxMageGuild3.Checked = Convert.ToBoolean(Convert.ToInt32(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.MageGuildLevel.ToString())));
                chkboxIsStarting3.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.IsStartingTown.ToString()));
                chkboxDwellings3.Checked = Convert.ToBoolean(Convert.ToInt32(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.DwellingLevel.ToString())));
                chkboxUpgraded3.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.DwellingsUpgrades.ToString()));
                chkboxSpecial3.Checked = Convert.ToBoolean(Program.frmTemplateEdit.thTemplate.GetTownsAttributes(iZoneIndex, 3, eTown.TownSpecialStracture.ToString()));
            }
        }

        private void cboTownType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTownType2.SelectedItem.ToString() == "None")
            {

                cboResources2.Enabled = false;
                cboIncomeLevel2.Enabled = false;
                cboTownWalls2.Enabled = false;

                chkboxBlacksmith2.Enabled = false;
                chkboxDwellings2.Enabled = false;
                chkboxIsStarting2.Enabled = false;
                chkboxMageGuild2.Enabled = false;
                chkboxSpecial2.Enabled = false;
                chkboxTavern2.Enabled = false;
                chkboxUpgraded2.Enabled = false;
                msktxtDwelling2.Enabled = false;
                msktxtMageLevel2.Enabled = false;
               // btnSetGuards2.Enabled = false;
            }
            else
            {
                cboResources2.Enabled = true;
                cboIncomeLevel2.Enabled = true;
                cboTownWalls2.Enabled = true;

                chkboxBlacksmith2.Enabled = true;
                chkboxDwellings2.Enabled = true;
                chkboxIsStarting2.Enabled = true;
                chkboxMageGuild2.Enabled = true;
               // chkboxSpecial2.Enabled = true;
                chkboxTavern2.Enabled = true;
                chkboxUpgraded2.Enabled = true;
                msktxtDwelling2.Enabled = true;
                msktxtMageLevel2.Enabled = true;
               // btnSetGuards2.Enabled = true;
            }
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.Type.ToString(), cboTownType2.SelectedItem.ToString());
        }

        private void cboTownType3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTownType3.SelectedItem.ToString() == "None")
            {

                cboResources3.Enabled = false;
                cboIncomeLevel3.Enabled = false;
                cboTownWalls3.Enabled = false;

                chkboxBlacksmith3.Enabled = false;
                chkboxDwellings3.Enabled = false;
                chkboxIsStarting3.Enabled = false;
                chkboxMageGuild3.Enabled = false;
                chkboxSpecial3.Enabled = false;
                chkboxTavern3.Enabled = false;
                chkboxUpgraded3.Enabled = false;
                msktxtDwelling3.Enabled = false;
                msktxtMageLevel3.Enabled = false;
             //   btnSetGuards3.Enabled = false;
            }
            else
            {
                cboResources3.Enabled = true;
                cboIncomeLevel3.Enabled = true;
                cboTownWalls3.Enabled = true;

                chkboxBlacksmith3.Enabled = true;
                chkboxDwellings3.Enabled = true;
                chkboxIsStarting3.Enabled = true;
                chkboxMageGuild3.Enabled = true;
               // chkboxSpecial3.Enabled = true;
                chkboxTavern3.Enabled = true;
                chkboxUpgraded3.Enabled = true;
                msktxtDwelling3.Enabled = true;
                msktxtMageLevel3.Enabled = true;
             //   btnSetGuards3.Enabled = true;
            }
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.Type.ToString(), cboTownType3.SelectedItem.ToString());
        }

        private void btnSetGuards_Click(object sender, EventArgs e)
        {
            Program.frmGuards.EnableTownMode();
            Program.frmGuards.Show();
        }

        private void btnSetGuards2_Click(object sender, EventArgs e)
        {
            Program.frmGuards.EnableTownMode();
            Program.frmGuards.Show();
        }

        private void btnSetGuards3_Click(object sender, EventArgs e)
        {
            Program.frmGuards.EnableTownMode();
            Program.frmGuards.Show();
        }

        private void chkbox1_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetLinkAttribute(iZoneIndex, "Towns", checkBox1.Checked.ToString());

           if (checkBox1.Checked)
           {//dwp необходимо подстветить галочки в каждой зоне в каждом сете, если их там нет конечно
               XmlElement xObjects1, xObjects2, xObjects3;
               XmlNode xndNodeToTemplate, Xlink, xndFromObjects;
               
               ObjectsReader obrdUpdater = new ObjectsReader();

               xndFromObjects = obrdUpdater.GetObjectsData().SelectSingleNode("//Object[@Name='Towns']");
               xObjects1 = Program.frmTemplateEdit.thTemplate.GetObjectsSet(iZoneIndex, 1);
               xndNodeToTemplate = xObjects1.SelectSingleNode(".//Object[@Name='Towns']");

               if (xndNodeToTemplate == null) {
                   Xlink = xndFromObjects.CloneNode(true);
                   xObjects1.SelectSingleNode(".//BattleObjects").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
               }
               xObjects2 = Program.frmTemplateEdit.thTemplate.GetObjectsSet(iZoneIndex, 2);
               xndNodeToTemplate = xObjects2.SelectSingleNode(".//Object[@Name='Towns']");
               if (xndNodeToTemplate == null) {
                   Xlink = xndFromObjects.CloneNode(true);
                   xObjects2.SelectSingleNode(".//BattleObjects").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
               }
               xObjects3 = Program.frmTemplateEdit.thTemplate.GetObjectsSet(iZoneIndex, 3);
               xndNodeToTemplate = xObjects3.SelectSingleNode(".//Object[@Name='Towns']");
               if (xndNodeToTemplate == null) {
                   Xlink = xndFromObjects.CloneNode(true);
                   xObjects3.SelectSingleNode(".//BattleObjects").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
               }
           }
        }

        private void chkboxBlacksmith_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.BlackSmith.ToString(), chkboxBlacksmith.Checked.ToString());
        }

        private void chkboxBlacksmith2_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.BlackSmith.ToString(), chkboxBlacksmith2.Checked.ToString());
        }

        private void chkboxBlacksmith3_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.BlackSmith.ToString(), chkboxBlacksmith3.Checked.ToString());
        }

        private void chkboxTavern_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.Tavern.ToString(), chkboxTavern.Checked.ToString());
        }

        private void chkboxTavern2_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.Tavern.ToString(), chkboxTavern2.Checked.ToString());
        }

        private void chkboxTavern3_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.Tavern.ToString(), chkboxTavern3.Checked.ToString());
        }

        private void chkboxMageGuild_CheckedChanged(object sender, EventArgs e)
        {

            msktxtMageLevel.Enabled = ((CheckBox)sender).Checked ;
            
            if ( ((CheckBox)sender).Checked )
            {
                msktxtMageLevel.Text = "1";
            }
            else
            {
                msktxtMageLevel.Text = "0";
            }
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.MageGuildLevel.ToString(), msktxtMageLevel.Text);
        }

        private void chkboxMageGuild2_CheckedChanged(object sender, EventArgs e)
        {
            msktxtMageLevel2.Enabled = ((CheckBox)sender).Checked;

            if (((CheckBox)sender).Checked)
            {
                msktxtMageLevel2.Text = "1";
            }
            else
            {
                msktxtMageLevel2.Text = "0";
            }
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.MageGuildLevel.ToString(), msktxtMageLevel2.Text);
        }

        private void chkboxMageGuild3_CheckedChanged(object sender, EventArgs e)
        {
            msktxtMageLevel3.Enabled = ((CheckBox)sender).Checked;

            if (((CheckBox)sender).Checked)
            {
                msktxtMageLevel3.Text = "1";
            }
            else
            {
                msktxtMageLevel3.Text = "0";
            }
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.MageGuildLevel.ToString(), msktxtMageLevel3.Text);
        }

        private void chkboxIsStarting_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.IsStartingTown.ToString(), chkboxIsStarting.Checked.ToString());
        }

        private void chkboxIsStarting2_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.IsStartingTown.ToString(), chkboxIsStarting2.Checked.ToString());
        }

        private void chkboxIsStarting3_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.IsStartingTown.ToString(), chkboxIsStarting3.Checked.ToString());
        }

        private void chkboxSpecial_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.TownSpecialStracture.ToString(), chkboxSpecial.Checked.ToString());
        }

        private void chkboxSpecial2_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.TownSpecialStracture.ToString(), chkboxSpecial2.Checked.ToString());
        }

        private void chkboxSpecial3_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.TownSpecialStracture.ToString(), chkboxSpecial3.Checked.ToString());
        }

        private void chkboxDwellings_CheckedChanged(object sender, EventArgs e)
        {
            msktxtDwelling.Enabled = ((CheckBox)sender).Checked;

            if (((CheckBox)sender).Checked)
            {
                msktxtDwelling.Text = "1";
            }
            else
            {
                msktxtDwelling.Text = "0";
            }
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.DwellingLevel.ToString(), msktxtDwelling.Text);

        }

        private void chkboxDwellings2_CheckedChanged(object sender, EventArgs e)
        {
            msktxtDwelling2.Enabled = ((CheckBox)sender).Checked;

            if (((CheckBox)sender).Checked)
            {
                msktxtDwelling2.Text = "1";
            }
            else
            {
                msktxtDwelling2.Text = "0";
            }
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.DwellingLevel.ToString(), msktxtDwelling2.Text);
        }

        private void chkboxDwellings3_CheckedChanged(object sender, EventArgs e)
        {
            msktxtDwelling3.Enabled = ((CheckBox)sender).Checked;

            if (((CheckBox)sender).Checked)
            {
                msktxtDwelling3.Text = "1";
            }
            else
            {
                msktxtDwelling3.Text = "0";
            }
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.DwellingLevel.ToString(), msktxtDwelling3.Text);
        }

        private void chkboxUpgraded_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.DwellingsUpgrades.ToString(), chkboxUpgraded.Checked.ToString());
        }

        private void chkboxUpgraded2_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.DwellingsUpgrades.ToString(), chkboxUpgraded2.Checked.ToString());
        }

        private void chkboxUpgraded3_CheckedChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.DwellingsUpgrades.ToString(), chkboxUpgraded3.Checked.ToString());
        }

        private void cboTownWalls_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.TownWalls.ToString(), cboTownWalls.SelectedItem.ToString());
        }

        private void cboTownWalls2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.TownWalls.ToString(), cboTownWalls2.SelectedItem.ToString());
        }

        private void cboTownWalls3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.TownWalls.ToString(), cboTownWalls3.SelectedItem.ToString());
        }

        private void cboIncomeLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.IncomeLevel.ToString(), cboIncomeLevel.SelectedItem.ToString());
        }

        private void cboIncomeLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.IncomeLevel.ToString(), cboIncomeLevel2.SelectedItem.ToString());
        }

        private void cboIncomeLevel3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.IncomeLevel.ToString(), cboIncomeLevel3.SelectedItem.ToString());
        }

        private void cboResources_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.ResourceLevel.ToString(), cboResources.SelectedItem.ToString());
        }

        private void cboResources3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.ResourceLevel.ToString(), cboResources3.SelectedItem.ToString());
        }

        private void cboResources2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.ResourceLevel.ToString(), cboResources2.SelectedItem.ToString());
        }

        private void msktxtMageLevel_Validated(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.MageGuildLevel.ToString(), msktxtMageLevel.Text);
        }

        private void msktxtMageLevel2_Validated(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.MageGuildLevel.ToString(), msktxtMageLevel2.Text);
        }

        private void msktxtMageLevel3_Validated(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.MageGuildLevel.ToString(), msktxtMageLevel3.Text);
        }

        private void msktxtDwelling_Validated(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 1, eTown.DwellingLevel.ToString(), msktxtDwelling.Text);
        }

        private void msktxtDwelling2_Validated(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 2, eTown.DwellingLevel.ToString(), msktxtDwelling2.Text);
        }

        private void msktxtDwelling3_Validated(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.thTemplate.SetTownsAttributes(iZoneIndex, 3, eTown.DwellingLevel.ToString(), msktxtDwelling3.Text);

        }
    }
}