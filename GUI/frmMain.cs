using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Homm5RMG.Properties;
using Homm5RMG.BL;
using System.Reflection;
using Homm5RMG.Data_Layer;
using Homm5RMG.Testing;
using System.Xml;

namespace Homm5RMG
{
    enum eRoadStatus
    {
        No_Roads=0,
        Roads_To_Towns,
        Roads_To_Towns_And_Mines
    }

    enum eDwellingStatus
    {
        Random=0, //number: 1-3, lvl: 1-7 
        Standard, //number: 2, lvl: 1,2 
        Extended  //number: 3, lvl: 1-3 
    }

    public partial class frmMain : Form
    {
        #region Constant
        public static readonly string STR_VERSION = "Version 1.8.3";
        public static string templatelist;
        #endregion

        #region FormMembers
        public int[] iarrSelectedPlayersFactions = new int[4];
        public int[] SaveiarrSelectedPlayersFactions = new int[4];
        public int[][] iarrExcludedPlayersFactions = new int[][] 
            {new int[] {0,0,0,0,0,0,0,0},
             new int[] {0,0,0,0,0,0,0,0},
             new int[] {0,0,0,0,0,0,0,0},
             new int[] {0,0,0,0,0,0,0,0}};

        public string strProgressStatus;
        private string strMapSize = "Large";
        ImageListPopup ilp1;
        ImageListPopup ilp2;
        ImageListPopup ilp3;
        ImageListPopup ilp4;
        #endregion
        
        /// <summary>
        /// constructor , Used to initilize the imagelistpopups and set faction defaults
        /// </summary>
        public frmMain()
        {
            // cli interface

            InitializeComponent();
            #region imagelistpopup for player 1
            ilp1 = new ImageListPopup();
            ilp1.BackgroundColor = Color.FromArgb(241, 241, 241);
            ilp1.BackgroundOverColor = Color.FromArgb(102, 154, 204);
            ilp1.HLinesColor = Color.FromArgb(182, 189, 210);
            ilp1.VLinesColor = Color.FromArgb(182, 189, 210);
            ilp1.BorderColor = Color.FromArgb(0, 0, 0);
            ilp1.Init(imageList1, 11, 11, 3, 3, 0, iarrExcludedPlayersFactions[0]);
            ilp1.ItemClick += new ImageListPopupEventHandler(OnItemClicked1);
            #endregion

            #region imagelistpopup for player 2
            ilp2 = new ImageListPopup();
            ilp2.BackgroundColor = Color.FromArgb(241, 241, 241);
            ilp2.BackgroundOverColor = Color.FromArgb(102, 154, 204);
            ilp2.HLinesColor = Color.FromArgb(182, 189, 210);
            ilp2.VLinesColor = Color.FromArgb(182, 189, 210);
            ilp2.BorderColor = Color.FromArgb(0, 0, 0);
            ilp2.Init(imageList1, 11, 11, 3, 3, 1, iarrExcludedPlayersFactions[1]);
            ilp2.ItemClick += new ImageListPopupEventHandler(OnItemClicked2);
            #endregion
            #region imagelistpopup for player 3
            ilp3 = new ImageListPopup();
            ilp3.BackgroundColor = Color.FromArgb(241, 241, 241);
            ilp3.BackgroundOverColor = Color.FromArgb(102, 154, 204);
            ilp3.HLinesColor = Color.FromArgb(182, 189, 210);
            ilp3.VLinesColor = Color.FromArgb(182, 189, 210);
            ilp3.BorderColor = Color.FromArgb(0, 0, 0);
            ilp3.Init(imageList1, 11, 11, 3, 3, 2, iarrExcludedPlayersFactions[2]);
            ilp3.ItemClick += new ImageListPopupEventHandler(OnItemClicked3);
            #endregion
            #region imagelistpopup for player 4
            ilp4 = new ImageListPopup();
            ilp4.BackgroundColor = Color.FromArgb(241, 241, 241);
            ilp4.BackgroundOverColor = Color.FromArgb(102, 154, 204);
            ilp4.HLinesColor = Color.FromArgb(182, 189, 210);
            ilp4.VLinesColor = Color.FromArgb(182, 189, 210);
            ilp4.BorderColor = Color.FromArgb(0, 0, 0);
            ilp4.Init(imageList1, 11, 11, 3, 3, 3, iarrExcludedPlayersFactions[3]);
            ilp4.ItemClick += new ImageListPopupEventHandler(OnItemClicked4);
            #endregion

            //set selected factions defaults - 1 is random
            iarrSelectedPlayersFactions[0] = 1;
            iarrSelectedPlayersFactions[1] = 1;
            iarrSelectedPlayersFactions[2] = 1;
            iarrSelectedPlayersFactions[3] = 1;
            //dwp! для новой рандомизации рас, без выхода из генератора
            SaveiarrSelectedPlayersFactions[0] = 1;
            SaveiarrSelectedPlayersFactions[1] = 1;
            SaveiarrSelectedPlayersFactions[2] = 1;
            SaveiarrSelectedPlayersFactions[3] = 1;
            Settings.Default.FactionRed = iarrSelectedPlayersFactions[0];
            Settings.Default.FactionBlue = iarrSelectedPlayersFactions[1];
            Settings.Default.FactionGreen = iarrSelectedPlayersFactions[2];
            Settings.Default.FactionYellow = iarrSelectedPlayersFactions[3];

        }

        /// <summary>
        /// opens folder dialog to select folder (OutputDirectory)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private bool EntireMenuMapType;
        private void BtnFolder_Click(object sender, EventArgs e)
        {

            folDiag1.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult diagresFolder = folDiag1.ShowDialog();

            if (diagresFolder == DialogResult.OK)
            {
                //set output directory settings and path
                Settings.Default.DefaultDirectory = folDiag1.SelectedPath;
                Settings.Default.Save();
                txtDir.Text = folDiag1.SelectedPath;
            }
 
        }


        

        /// <summary>
        /// draws a gradient background to the form
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics formGraphics = e.Graphics;
            System.Drawing.Drawing2D.LinearGradientBrush gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), Color.LightSlateGray, Color.LightSteelBlue);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle);


            //base.OnPaint(e);
        }


        /// <summary>
        /// initilize all relavent to this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

            //form title contains form name and version
            this.Text = "Idan's RMG, Edited by DWP " + STR_VERSION;
            toolStripStatusLabel1.Text = STR_VERSION;

            //Program.fSplash.Dispose();
            Settings MyAppSettings = Settings.Default;

            //repairs bug from version 1.0
            if (MyAppSettings.DefaultTemplateFile.Contains("/"))
                MyAppSettings.DefaultTemplateFile = (Assembly.GetExecutingAssembly().CodeBase.Substring(0, Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + @"Templates/JebusV5.irt").Replace("/", "\\");

            //flag to set the template path for the first time
            if (MyAppSettings.DefaultTemplateFile == "JebusV5.irt")
                MyAppSettings.DefaultTemplateFile = (Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + @"Templates/JebusV5.irt").Replace("/","\\");

            if (MyAppSettings.TemplatesDirectory == "None")
            {
                MyAppSettings.TemplatesDirectory = (Assembly.GetExecutingAssembly().CodeBase.Substring(0, Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + @"Templates").Replace("/", "\\");
            }

            //get values stored in program settings section
            msktxtMonsterFactor.Text = MyAppSettings.MonsterFactor;
            msktxtCompatiblityFactor.Text = MyAppSettings.CompatibilityFactor;

            //default path dir
            //txtDir.Text = System.Configuration.ConfigurationManager.AppSettings["DefaultDirectory"];
            txtDir.Text = MyAppSettings.DefaultDirectory;
            msktxtSeed.Text = "0";//MyAppSettings.RandomSeed;
            chkChaos.Checked = MyAppSettings.ChaosWeeks;
            chkIT.Checked = MyAppSettings.DisableIT;
            chkToPlaySound.Checked = MyAppSettings.ToPlaySound;
            //dwp. новые настройки
            checkBox1.Checked = MyAppSettings.OnlyPortals;
            checkBox2.Checked = MyAppSettings.NoBlocks;
            checkBox3.Checked = MyAppSettings.SingleFactionGuards;
            checkBox4.Checked = MyAppSettings.SelectedFactionGuards;
            checkBox5.Checked = MyAppSettings.NoNewHighDwelling;
            checkBox6.Checked = MyAppSettings.NewBankGenerating;

            //chkNoRoads.Checked = MyAppSettings.NoRoads;
            btnRoadStatus.Text = Enum.GetNames(typeof(eRoadStatus))[MyAppSettings.RoadStatus].Replace('_', ' ');
            btnDwellingsStatus.Text = Enum.GetNames(typeof(eDwellingStatus))[MyAppSettings.DwellingStatus];
            txtRandomTepmlate.Text = MyAppSettings.TemplatesDirectory;

            if (MyAppSettings.AutoNameMap)
            {
                chkAutoName.Checked = true;
                txtMapName.Enabled = false;
                txtMapName.Text = "YourMapNameHere";
            }
            else
            {
                chkAutoName.Checked = false;
                txtMapName.Enabled = true;
                txtMapName.Text = "YourMapNameHere";
            }

            if (!MyAppSettings.RandomTemplate)
            {
                toolTip1.SetToolTip(btnSwitch, "Click To Switch To Random Template Mode");
                txtRandomTepmlate.Visible = false;
                btnRandomTemplate.Visible = false;
                txtTemplate.Visible = true;
                btnTemplate.Visible = true;
                btnSwitch.Image = imageList2.Images[1];
                //btnSwitch.
            }
            else
            {
                toolTip1.SetToolTip(btnSwitch, "Click To Switch To Single Template Selection Mode");
                txtRandomTepmlate.Visible = true;
                btnRandomTemplate.Visible = true;
                txtTemplate.Visible = false;
                btnTemplate.Visible = false;
                btnSwitch.Image = imageList2.Images[0];
            }



            //checks if the default output directory exist ,If not set the program dir as target output directory
            if (!System.IO.Directory.Exists(txtDir.Text))
            {
                txtDir.Text = Assembly.GetExecutingAssembly().CodeBase.Substring(0, Assembly.GetExecutingAssembly().CodeBase.Length - 13).Substring(8).Replace("/", "\\");
            }
            folDiag1.SelectedPath = txtDir.Text;//System.Configuration.ConfigurationManager.AppSettings["DefaultDirectory"];
            folderBrowserDialog1.SelectedPath = txtRandomTepmlate.Text;



            //first image is random race picture , Set it initially for both pictureboxes that indicates player's faction selection
            pictureBox2.Image = imageList1.Images[0];
            pictureBox5.Image = imageList1.Images[0];
            pictureBox3.Image = imageList1.Images[0];
            pictureBox4.Image = imageList1.Images[0];

            //to prevet focus on unwanted controls
            chkboxAdvanced.Focus();

            //template file default
            #region templatefile
            openFileDialog1.DefaultExt = "irt";
            openFileDialog1.FileName = System.Configuration.ConfigurationManager.AppSettings["DefaultTemplateFile"];
            openFileDialog1.Filter = "Idan's RMG Template files (*.irt)|*.irt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.InitialDirectory = Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + @"\Templates";

            txtTemplate.Text = MyAppSettings.DefaultTemplateFile; //System.IO.Directory.GetCurrentDirectory() + @"\" + System.Configuration.ConfigurationManager.AppSettings["DefaultTemplateFile"]; 
            #endregion


            //Map Editor Default
            #region mapedit default
            openFileDialogMapEditor.DefaultExt = "exe";
            openFileDialogMapEditor.FileName = System.Configuration.ConfigurationManager.AppSettings["MapEditorFilenName"];
            openFileDialogMapEditor.Filter = "MapEditor files (H5_MapEditor.exe)|H5_MapEditor.exe|All files (*.*)|*.*";
            openFileDialogMapEditor.FilterIndex = 0;
            openFileDialogMapEditor.InitialDirectory = Assembly.GetExecutingAssembly().CodeBase.Substring(0,Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8);

            txtMapEditor.Text = MyAppSettings.MapEditorPath; //System.IO.Directory.GetCurrentDirectory() + @"\" + System.Configuration.ConfigurationManager.AppSettings["DefaultTemplateFile"]; 
            #endregion


            //get names of sizes out of enum type and adds them for the user selection
            string[] strarrSizes = Enum.GetNames(typeof(MapSize));

            foreach (string strSize in strarrSizes)
            {
                cboSizes.Items.Add(strSize); 
            }

            //reads settings last selected size
            cboSizes.SelectedIndex = MyAppSettings.SelectedSizeIndex;
            chkDifferentFactions.Checked = Settings.Default.DifferentFactions;
#if (DEBUG)
            //dmitrik: set visible debug buttons
            btnTestObjectsArea.Visible = true;
            btnShowGarrisonsPower.Visible = true;
            msktxtSeed.ReadOnly = false;
#endif
        }




        /// <summary>
        /// shows error in case of invalide input as defined monster faction value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void maskedTextBox1_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput)
            {
                toolTip1.ToolTipTitle = "Invalid Value";
                toolTip1.Show("I'm sorry, but the value you entered is not a valid factor. Please change the value.", msktxtMonsterFactor, 5000);
                e.Cancel = true;
            }

            //throw new Exception("The method or operation is not implemented.");
        }


        /// <summary>
        /// show template form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.Show();
            this.Hide();
            
        }


        /// <summary>
        /// close any form and any loose ends possible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(-1);
        }


        /// <summary>
        /// pops up a file open dialog to select a template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTemplate_Click(object sender, EventArgs e)
        {
            DialogResult diagresTemplate = openFileDialog1.ShowDialog();

            if (diagresTemplate == DialogResult.OK) 
            {
                Settings.Default.DefaultTemplateFile = openFileDialog1.FileName;
                Settings.Default.Save();
                txtTemplate.Text = openFileDialog1.FileName;
            }
        }



        /// <summary>
        /// launches the map generation proccess in a different thread worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            #region old code
            //progressBar1.Visible = true;
            //progressBar1.Value = 0;
            #endregion

            try
            {
#if (DEBUG) // {
                if (msktxtSeed.Text == "0")
//4294960
#endif // DEBUG }
                msktxtSeed.Text = Randomizer.GenTimeSeed().ToString();
                Settings.Default.RandomSeed = msktxtSeed.Text;
                //can't generate multiple simultaniusly
                btnGenerate.Enabled = false;
                btnCancel.Visible = true;
                toolStripButton9.Enabled = false;
                //runs the whole thing in a different thread
                backgroundWorker1.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(/*"Oops.. A bug just occoured - Please try again and send me the error report.. (PrintScreen and send )/n Error Message : " +*/ 
                ex.Message /* + "/n Stack Trace : " + ex.StackTrace*/ 
                );
            }
        }
 
        /// <summary>
        /// help link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.heroesofmightandmagic.com/heroes5/modding_wiki/random_map_generator_by_idan:general");
            //MessageBox.Show(" No Help Exist Yet");
        }


        #region some old code to filter the strength input
        //private void txtStrength_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtStrength.Text == string.Empty)
        //    {
        //        MessageBox.Show("Value Cannot Be Null"); 
        //        txtStrength.Text = "1.0";
        //    }

        //    double ResultStrengthFactor;

        //    if (!double.TryParse(txtStrength.Text, out ResultStrengthFactor))
        //    {
        //        MessageBox.Show("Value Must Be Numeric");
        //        txtStrength.Text = "1.0";
        //    }

        //}

        #endregion

 

        /// <summary>
        /// event for the button pop up thingy (the image selection for factions)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemClicked1(object sender, ImageListPopupEventArgs e)
        {
            pictureBox2.Image = imageList1.Images[e.SelectedItem];
            //set selected for player 1
            iarrSelectedPlayersFactions[0] = e.SelectedItem + 1 ;
            SaveiarrSelectedPlayersFactions[0] = e.SelectedItem + 1 ;
            Settings.Default.FactionRed = iarrSelectedPlayersFactions[0];
            Settings.Default.Save();
            EnableDifferentFactionsOption();
        }

        /// <summary>
        /// event for the button pop up thingy (the image selection for factions)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemClicked2(object sender, ImageListPopupEventArgs e)
        {
            pictureBox5.Image = imageList1.Images[e.SelectedItem];
            //set selected for player 2
            iarrSelectedPlayersFactions[1] = e.SelectedItem + 1 ;
            SaveiarrSelectedPlayersFactions[1] = e.SelectedItem + 1 ;
            Settings.Default.FactionBlue = iarrSelectedPlayersFactions[1];
            Settings.Default.Save();
            EnableDifferentFactionsOption();
        }

        private void OnItemClicked3(object sender, ImageListPopupEventArgs e)
        {
            pictureBox3.Image = imageList1.Images[e.SelectedItem];
            //set selected for player 3
            iarrSelectedPlayersFactions[2] = e.SelectedItem + 1;
            SaveiarrSelectedPlayersFactions[2] = e.SelectedItem + 1;
            Settings.Default.FactionGreen = iarrSelectedPlayersFactions[2];
            Settings.Default.Save();
            EnableDifferentFactionsOption();
        }

        private void OnItemClicked4(object sender, ImageListPopupEventArgs e)
        {
            pictureBox4.Image = imageList1.Images[e.SelectedItem];
            //set selected for player 4
            iarrSelectedPlayersFactions[3] = e.SelectedItem + 1;
            SaveiarrSelectedPlayersFactions[3] = e.SelectedItem + 1;
            Settings.Default.FactionYellow = iarrSelectedPlayersFactions[3];
            Settings.Default.Save();
            EnableDifferentFactionsOption();
        }

        private void BtnSelect1_Click(object sender, EventArgs e)
        {
            Point pt = PointToScreen(new Point(this.Left + 10 , BtnSelect1.Bottom - 3 ));
            ilp1.Show(pt.X, pt.Y);

        }

        private void BtnSelect2_Click(object sender, EventArgs e)
        {
            Point pt = PointToScreen(new Point(BtnSelect1.Left , BtnSelect1.Bottom - 3));
            ilp2.Show(pt.X, pt.Y);
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
 
           // if (!e.IsValidInput)
           // {
                toolTip1.ToolTipTitle = "Invalid Value";
                toolTip1.Show("I'm sorry, but the value you entered is not a valid factor. Please enter only numbers.", msktxtMonsterFactor, 2000);
              //  e.Cancel = true;
           // }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void txtTemplate_MouseHover(object sender, EventArgs e)
        {
            txtTemplate.ForeColor = Color.DarkBlue;
        }

        private void txtTemplate_MouseLeave(object sender, EventArgs e)
        {
            txtTemplate.ForeColor = Color.Black;
        }

        private void txtDir_MouseHover(object sender, EventArgs e)
        {
            txtDir.ForeColor = Color.DarkBlue;
        }

        private void txtDir_MouseLeave(object sender, EventArgs e)
        {
            txtDir.ForeColor = Color.Black;
        }

        //return path of template as in txttemplate text box
        public string TemplatePath
        {
            get
            {
                return this.txtTemplate.Text;
            }

        }

        public string TemplatesDirectoryPath
        {
            get
            {
                return this.txtRandomTepmlate.Text;
            }
        }

        /// <summary>
        /// in a seperate thread executes the written code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Console_DoWork(int ch,int pl1, int[] exc1, int pl2, int[] exc2, int pl3, int[] exc3, int pl4, int[] exc4, 
                                   bool set1, string dir1,bool set2,string dir2, string dir3,
                                   bool set3,bool set4,string sz, string dw, string seed)
        {
                if (!set2)
                {
                    if (MapCreator.CheckMapExistance(dir3, dir2))
                    {
                        System.Console.WriteLine("Map name already exist , Please select another map name or delete the map");
                        return;
                    }
                }

                Settings.Default.DwellingStatus = (int)Enum.Parse(typeof(eDwellingStatus), dw);
                if (seed != "0")
                {
                    Randomizer.SetSeed(int.Parse(seed));
                    msktxtSeed.Text = seed;
                }
                else
                {
                    msktxtSeed.Text = Randomizer.GenTimeSeed().ToString();
                }

                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
                MapSize emzSizeOfMap = (MapSize)Enum.Parse(typeof(MapSize), sz);
                ObjectsMap RMGMap = new ObjectsMap(emzSizeOfMap);
                int i = Array.IndexOf(Enum.GetNames(typeof(MapSize)), sz);
                Settings.Default.SelectedSizeIndex = i;

                Settings.Default.RandomSeed = msktxtSeed.Text;
                Settings.Default.RandomTemplate = set1;
                Settings.Default.DefaultDirectory = dir3;
                if (set1) RMGMap.thSelectedTemplate = GetRandomTemplatePath(dir1);
                else
                {
                    RMGMap.thSelectedTemplate = new TemplateHandler(dir1);
                    Settings.Default.DefaultTemplateFile = dir1;
                    string strErrors = RMGMap.thSelectedTemplate.TemplateValidationCheck();
                    if (strErrors != string.Empty)
                    {
                        System.Console.WriteLine(strErrors);
                        return;
                    }
                }
                Settings.Default.Save();
                RMGMap.thSelectedTemplate.iarrSelectedPlayersFactions = iarrSelectedPlayersFactions;
                RMGMap.thSelectedTemplate.SaveiarrSelectedPlayersFactions = SaveiarrSelectedPlayersFactions;
                RMGMap.thSelectedTemplate.iarrExcludedPlayersFactions = new int[4][];
                RMGMap.thSelectedTemplate.iarrExcludedPlayersFactions[0] = exc1;
                RMGMap.thSelectedTemplate.iarrExcludedPlayersFactions[1] = exc2;
                RMGMap.thSelectedTemplate.iarrExcludedPlayersFactions[2] = exc3;
                RMGMap.thSelectedTemplate.iarrExcludedPlayersFactions[3] = exc4;
                RMGMap.thSelectedTemplate.iarrSelectedPlayersFactions[0] = pl1 + 1;
                RMGMap.thSelectedTemplate.iarrSelectedPlayersFactions[1] = pl2 + 1;
                RMGMap.thSelectedTemplate.iarrSelectedPlayersFactions[2] = pl3 + 1;
                RMGMap.thSelectedTemplate.iarrSelectedPlayersFactions[3] = pl4 + 1;
                RMGMap.thSelectedTemplate.SaveiarrSelectedPlayersFactions[0] = pl1 + 1;
                RMGMap.thSelectedTemplate.SaveiarrSelectedPlayersFactions[1] = pl2 + 1;
                RMGMap.thSelectedTemplate.SaveiarrSelectedPlayersFactions[2] = pl3 + 1;
                RMGMap.thSelectedTemplate.SaveiarrSelectedPlayersFactions[3] = pl4 + 1;

                RMGMap.thSelectedTemplate.GenerateRandomIdPerZone();
                int maxexcl = 6 - (ch==2? 1 : 0) - (ch>2 ? 2 : 0);
                for (int pl = 0; pl < RMGMap.thSelectedTemplate.IPLAYERNUMBER; pl++)
                {
                    if (iarrSelectedPlayersFactions[pl] == (int)eTownType.Random)
                    {
                        int colexcl = 0;
                        for (int fac = 0; fac < 8; fac++)
                            colexcl += RMGMap.thSelectedTemplate.iarrExcludedPlayersFactions[pl][fac];
                        if (colexcl >= maxexcl)
                        {
                            System.Console.WriteLine("Too many exception factions in random selection in Player " + (pl + 1).ToString());
                            return;
                        }
                    }
                }
                RMGMap.thSelectedTemplate.SetPlayerFactions(set3);
                RMGMap.thSelectedTemplate.SetTerrainTable();
                while (RMGMap.ZonesGenerator() == "Failed") { }
                RMGMap.FillBlockMask();
                RMGMap.SetTerrainSeperationObjects();
                RMGMap.FixIsolatedMaybeBlockSpace();
                RMGMap.FixDiagnolOnlyAccessToMaybeBlock();
#if (DEBUG)
                //  RMGMap.FlagZonesForDebug();
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                RMGMap.ComputeMaxDistancePointInZones();
                RMGMap.MarkPathWay();
#if (DEBUG)
                RMGMap.dumpAStarToFile();
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                RMGMap.MarkSquaresAroundPreservedAsFree();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                RMGMap.thSelectedTemplate.CalcLinkedZones();
                RMGMap.OpenConnections();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                RMGMap.MarkSquaresAroundPreservedAsFree();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                RMGMap.ComputeAllDirections();
                RMGMap.thSelectedTemplate.xSelectedObjectSets = new XmlElement[RMGMap.thSelectedTemplate.ZoneNumber];
                RMGMap.PopulateTowns();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                RMGMap.ComputeAllDirections();
                RMGMap.PopulateMines();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                RMGMap.PopulateDwellings();
                RMGMap.PopulateDwellingsInZones();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                RMGMap.ComputeAllDirections();
                RMGMap.PopulateObjectsInZones();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                RMGMap.TransformDoneAdjacentToPreserved();
                RMGMap.TransformFreeToBlockByRandomChance();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                RMGMap.PlaceBlockObjects();
                string strMapFileName;
                if (set2)
                {
                    strMapFileName = "IRMG_D_" + DateTime.Now.ToShortDateString().Replace('/', '-') + "_T_" + DateTime.Now.ToShortTimeString().Replace(':', '-');
                }
                else
                {
                    strMapFileName = dir2;
                }
                MapCreator.CreateTempMapFiles(emzSizeOfMap, strMapFileName, Settings.Default,
                                              RMGMap.thSelectedTemplate.SaveiarrSelectedPlayersFactions,
                                              RMGMap.thSelectedTemplate.iarrExcludedPlayersFactions);
                RMGMap.SetTerrains();
                RMGMap.CreateRoads(2); //для консоли пока что дороги константно до городов и шахт
#if (DEBUG)
                RMGMap.dumpTerrainsToFile(); 
#endif
                TerrainWriter trr = new TerrainWriter(MapCreator.MAPS_INITIAL_DIR + MapCreator.strMapName + @"\GroundTerrain.bin", emzSizeOfMap);
                trr.WriteTerrainData(RMGMap);
                strMapFileName += ".h5m";
                RMGMap.RenderObjectsToMapFile(MapCreator.strMapName, set4, false);//!!!!!
                #region create the new map
                string strMapName = dir3 + "/" + strMapFileName;
                Environment.CurrentDirectory = Assembly.GetExecutingAssembly().CodeBase.Substring(0, Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8);
                MapCreator.ZipTempMapFiles(strMapName);
                MapCreator.DeleteTempMapFiles();
                #endregion
                Settings.Default.LastUserCreatedMapName = strMapName;
                Settings.Default.Save();
                RMGMap = null;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
//#if (!DEBUG)
            try
            {
//#endif

            //first if map name is selected first check if map already exists
            if (!Settings.Default.AutoNameMap)
            {
                //check if destination target contains the file name selected
                if (MapCreator.CheckMapExistance(txtDir.Text, txtMapName.Text))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        backgroundWorker1.ReportProgress(0);
                    }
                    MessageBox.Show("Map name already exist , Please select another map name or delete the map");
                    return;                    
                }
            }

            //string strTimeMessage = "Start: " + DateTime.Now.ToLongTimeString();
            //get player faction selection
            
                //a fix to localization problem..
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
                //#if (DEBUG)
                //Randomizer.SetSeed(2);
                //#endi
                if (msktxtSeed.Text != "0")
                    Randomizer.SetSeed(int.Parse(msktxtSeed.Text));

                MapSize emzSizeOfMap = (MapSize)Enum.Parse(typeof(MapSize), strMapSize);
                ObjectsMap RMGMap = new ObjectsMap(emzSizeOfMap);

                if (Settings.Default.RandomTemplate)
                {
                    //get a random template file out of given path
                    RMGMap.thSelectedTemplate = GetRandomTemplatePath(TemplatesDirectoryPath);
                }

                else
                {
                    //create a new template handler
                    RMGMap.thSelectedTemplate = new TemplateHandler(Program.frmMainMenu.TemplatePath);
                    string strErrors = RMGMap.thSelectedTemplate.TemplateValidationCheck();
                    if (strErrors != string.Empty)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            backgroundWorker1.ReportProgress(0);
                        }
                        MessageBox.Show(strErrors);
                        return;
                    }
                }

                //set random player id's for towns.
                //randomize which player and where he will be
                RMGMap.thSelectedTemplate.GenerateRandomIdPerZone();
                //set selected factions for the template(players will be randomized)               
                RMGMap.thSelectedTemplate.iarrSelectedPlayersFactions = iarrSelectedPlayersFactions;
                RMGMap.thSelectedTemplate.SaveiarrSelectedPlayersFactions = SaveiarrSelectedPlayersFactions;
                //dwp  проверим не вычеркнуты ли все расы в рандомном выборе у игроков
                //для двух игроков нельзя исключать более 6 рас, для трех более 5, для четырех более 4
                int maxexcl = 7 - (Program.frmMainMenu.RadioButton2.Checked ? 1 : 0) - (Program.frmMainMenu.RadioButton4.Checked ? 2 : 0) -
                    (Program.frmMainMenu.RadioButton3.Checked ? 2 : 0);
                for (int pl = 0; pl < RMGMap.thSelectedTemplate.IPLAYERNUMBER; pl++)
                {
                    if (iarrSelectedPlayersFactions[pl] == (int)eTownType.Random)
                    {
                        int colexcl = 0;
                        for (int fac = 0; fac < 8; fac++)
                            colexcl += iarrExcludedPlayersFactions[pl][fac];
                        if (colexcl >= maxexcl)
                        {
                            MessageBox.Show("Too many exception factions in random selection in Player " + (pl + 1).ToString());
                            return;
                        }
                        
                    }
                }
                RMGMap.thSelectedTemplate.iarrExcludedPlayersFactions = iarrExcludedPlayersFactions;
                RMGMap.thSelectedTemplate.SetPlayerFactions(Settings.Default.DifferentFactions);
                RMGMap.thSelectedTemplate.SetTerrainTable();

                //set status text
                strProgressStatus = "Generating Zones";
                backgroundWorker1.ReportProgress(1);

                while (RMGMap.ZonesGenerator() == "Failed") {}

                RMGMap.FillBlockMask();
                RMGMap.SetTerrainSeperationObjects();
                RMGMap.FixIsolatedMaybeBlockSpace();
                RMGMap.FixDiagnolOnlyAccessToMaybeBlock();
#if (DEBUG)
                //  RMGMap.FlagZonesForDebug();
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                strProgressStatus = "Reserving space for roads";
                backgroundWorker1.ReportProgress(2);
                RMGMap.ComputeMaxDistancePointInZones();
                RMGMap.MarkPathWay();

#if (DEBUG)
                RMGMap.dumpAStarToFile();
                RMGMap.dumpBlockMaskToFileErgo();
#endif
                RMGMap.MarkSquaresAroundPreservedAsFree();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif

                strProgressStatus = "Connecting zones ...";
                backgroundWorker1.ReportProgress(3);
                RMGMap.thSelectedTemplate.CalcLinkedZones();
                RMGMap.OpenConnections();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif

                RMGMap.MarkSquaresAroundPreservedAsFree();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif

                RMGMap.ComputeAllDirections();
                strProgressStatus = "Populating Towns";
                backgroundWorker1.ReportProgress(4);

                RMGMap.thSelectedTemplate.xSelectedObjectSets = new XmlElement[RMGMap.thSelectedTemplate.ZoneNumber];
                RMGMap.PopulateTowns();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif

                RMGMap.ComputeAllDirections();

                strProgressStatus = "Populating Mines";
                backgroundWorker1.ReportProgress(5);
                RMGMap.PopulateMines();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif

                strProgressStatus = "Populating Dwellings";
                backgroundWorker1.ReportProgress(6);
                RMGMap.PopulateDwellings();
                RMGMap.PopulateDwellingsInZones();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif

                //RMGMap.PopulateBlocksInsideZones();
                

                RMGMap.ComputeAllDirections();

                strProgressStatus = "Populating other objects,wait please....";
                backgroundWorker1.ReportProgress(7);
                RMGMap.PopulateObjectsInZones();
#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif

                RMGMap.TransformDoneAdjacentToPreserved();
                RMGMap.TransformFreeToBlockByRandomChance();

#if (DEBUG)
                RMGMap.dumpBlockMaskToFileErgo();
#endif

                //strTimeMessage += Environment.NewLine + "End Object spreading: " + DateTime.Now.ToLongTimeString();

                //set status text
                strProgressStatus = "Generating Roads and Terrains";
                RMGMap.PlaceBlockObjects();
                backgroundWorker1.ReportProgress(8);

                string strMapFileName;


                if (Settings.Default.AutoNameMap)
                {
                    strMapFileName = "IRMG_D_" + DateTime.Now.ToShortDateString().Replace('/', '-') + "_T_" + DateTime.Now.ToShortTimeString().Replace(':', '-');
                }
                else
                {
                    strMapFileName = txtMapName.Text;
                }
                //create the map files and directory in a temp folder
                MapCreator.CreateTempMapFiles(emzSizeOfMap, strMapFileName, Settings.Default,
                                              RMGMap.thSelectedTemplate.SaveiarrSelectedPlayersFactions,
                                              RMGMap.thSelectedTemplate.iarrExcludedPlayersFactions);

                //set terrains data 

                RMGMap.SetTerrains();
                if ( Settings.Default.RoadStatus > 0)
                {
                    RMGMap.CreateRoads(Settings.Default.RoadStatus);
                }
#if (DEBUG)
                RMGMap.dumpTerrainsToFile(); 
#endif

                TerrainWriter trr = new TerrainWriter(MapCreator.MAPS_INITIAL_DIR + MapCreator.strMapName + @"\GroundTerrain.bin", emzSizeOfMap);
                trr.WriteTerrainData(RMGMap);

                strProgressStatus = "Generating Map File";
                backgroundWorker1.ReportProgress(9);
                strMapFileName += ".h5m";

                RMGMap.RenderObjectsToMapFile(MapCreator.strMapName, chkChaos.Checked , chkIT.Checked);

                #region create the new map
                ////create the file name of the map - format dir + initial + date
                string strMapName = txtDir.Text + "/" + strMapFileName;

                Environment.CurrentDirectory = Assembly.GetExecutingAssembly().CodeBase.Substring(0, Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8);

                MapCreator.ZipTempMapFiles(strMapName);
                if (chkToPlaySound.Checked)
                {
                    WSounds NewFinishSound = new WSounds();
                    NewFinishSound.Play("Resources\\MapReady.Wav", NewFinishSound.SND_FILENAME | NewFinishSound.SND_ASYNC);
                }
                // MapCreator.RemoveReadOnly( new System.IO.DirectoryInfo( "Maps" ));

                MapCreator.DeleteTempMapFiles();
                #endregion

                //update the setting entry for last created file name
                Settings.Default.LastUserCreatedMapName = strMapName;
                Settings.Default.Save();
                RMGMap = null;

//#if(!DEBUG)
            }
            catch (Exception ex)
            {
                if (backgroundWorker1.threadAborted != true)
                {
                    MessageBox.Show(/*"Bad zones created or canceled. Try again...." + Environment.NewLine + " Error Message : " + */ 
                    ex.Message      /*+ Environment.NewLine + " Stack Trace : " + ex.StackTrace */
                    );
                    backgroundWorker1.StopImmediately();
                }   
            }
//#endif
             backgroundWorker1.ReportProgress(10);
        }



        /// <summary>
        /// go over all files in given path and returns a random irt file
        /// </summary>
        /// <param name="TemplatesDirectoryPath"></param>
        /// <returns></returns>
        private TemplateHandler GetRandomTemplatePath(string strTemplatesDirectoryPath)
        {
            System.IO.DirectoryInfo dirinfTemplates = new System.IO.DirectoryInfo(strTemplatesDirectoryPath);
            System.Collections.ArrayList arrTemplateFiles = new System.Collections.ArrayList();
            ObjectsMap RM = new ObjectsMap((MapSize)Enum.Parse(typeof(MapSize), strMapSize));

            templatelist = "";
            foreach (System.IO.FileInfo fiTemplate in dirinfTemplates.GetFiles())
            {
                if (fiTemplate.Extension == ".irt")
                {
                    arrTemplateFiles.Add(fiTemplate);
                    templatelist = string.Concat(templatelist==""? templatelist:templatelist+",", fiTemplate.Name.Split('.')[0]);
                }
            }

            while (true)
            {
                if (arrTemplateFiles.Count == 0)
                {
                    throw new Exception("No template files found in given directory for current players selection");
                }
                else
                {   
           // dwp. выбор шаблона основан на типе генерируемой карты и TemplateValidationCheck теперь это проверяет
                    int i = Randomizer.rnd.Next(arrTemplateFiles.Count),j;
                    RM.thSelectedTemplate = new TemplateHandler(((System.IO.FileInfo)arrTemplateFiles[i]).FullName);
                    j = templatelist.IndexOf(((System.IO.FileInfo)arrTemplateFiles[i]).Name.Split('.')[0])+((System.IO.FileInfo)arrTemplateFiles[i]).Name.Split('.')[0].Length;
                    templatelist = string.Concat(templatelist.Substring(0, j),
                                                 "(T:"+((System.IO.FileInfo)arrTemplateFiles[i]).LastWriteTime.ToString()+
                                                 ",S:"+((System.IO.FileInfo)arrTemplateFiles[i]).Length.ToString()+")",
                                                 templatelist.Substring(j));
                    string strErrors = RM.thSelectedTemplate.TemplateValidationCheck();
                    if (strErrors != string.Empty)
                    {
                        arrTemplateFiles.RemoveAt(i);
                    }
                    else return RM.thSelectedTemplate;
                }
            }
        }


        /// <summary>
        /// report progress of thread work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //if progress just started
            if (progressBar1.Value == 0)
            {
                progressBar1.Visible = true;
                lblProgressStatus.Visible = true;
                btnCancel.Visible = true;
            }
            //adds progress to the progress bar
            progressBar1.Value +=25;
            lblProgressStatus.Text = strProgressStatus;
            
            //progress has finished and reset to default
            if (progressBar1.Maximum == progressBar1.Value)
            {
                btnGenerate.Enabled = true;
                progressBar1.Visible = false;
                toolStripButton9.Enabled = true;
                progressBar1.Value = 0;
                lblProgressStatus.Visible = false;
                btnCancel.Visible = false;
            }
        }

        private void chkboxAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkboxAdvanced.Checked)
                grpboxAdvanced.Visible = true;
            else
                grpboxAdvanced.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.StopImmediately();
            

            btnGenerate.Enabled = true;
            toolStripButton9.Enabled = true;
            progressBar1.Visible = false;
            progressBar1.Value = 0;
            lblProgressStatus.Visible = false;
            btnCancel.Visible = false;
        }

        private void msktxtCompatiblityFactor_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void msktxtCompatiblityFactor_Validated(object sender, EventArgs e)
        {
            Settings.Default.CompatibilityFactor = msktxtCompatiblityFactor.Text;
            Settings.Default.Save();
        }

        private void msktxtMonsterFactor_Validated(object sender, EventArgs e)
        {
            Settings.Default.MonsterFactor = msktxtMonsterFactor.Text;
            Settings.Default.Save();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            //MapCreator.CreateTempMapFiles(MapSize.ExtraLarge, "IRMGNEW");
            //MapCreator.DeleteTempMapFiles();
            //MapCreator.RemoveReadOnly(new System.IO.DirectoryInfo("Maps"));


            try
            {
                if (Settings.Default.LastUserCreatedMapName != "None")
                {

                    System.Diagnostics.Process.Start(txtMapEditor.Text, Settings.Default.LastUserCreatedMapName);

                }
                else
                    MessageBox.Show("Error No Maps Generated Yet");
            }
            catch (Exception ee)
            {
                MessageBox.Show("Error While Luanching Editor Application:/n " + ee.Message);
            }


            //ObjectsReader obrd = new ObjectsReader("Blocks.xml");

            //obrd.FixToObjects();
            //obrd.AddNamesToObjects();
            //obrd.AddType(eObjectType.Block);
            //obrd.SetObjectsPower(obrd.GetObjectsData());

            //obrd.AddArea();

        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            Program.frmTemplateEdit.Show();
            this.Hide();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.heroesofmightandmagic.com/heroes5/modding_wiki/random_map_generator_by_idan:general");
            //MessageBox.Show(" No Help Exist Yet");
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            StringBuilder emailMessage = new StringBuilder();

            emailMessage.Append("mailto:Nevermind_y@yahoo.com");
            emailMessage.Append("&subject=IRMG Suggestion/Bug Report");
            emailMessage.Append("&body=Dear Idan , ");

            System.Diagnostics.Process.Start(emailMessage.ToString());
        }

        private void btnEditor_Click(object sender, EventArgs e)
        {
            DialogResult diagresTemplate = openFileDialogMapEditor.ShowDialog();

            if (diagresTemplate == DialogResult.OK)
            {
                Settings.Default.MapEditorPath = openFileDialogMapEditor.FileName;
                Settings.Default.Save();
                txtMapEditor.Text = openFileDialogMapEditor.FileName;
            }
        }

        private void txtMapEditor_MouseHover(object sender, EventArgs e)
        {
            txtMapEditor.ForeColor = Color.DarkBlue;
        }

        private void txtMapEditor_MouseLeave(object sender, EventArgs e)
        {
            txtMapEditor.ForeColor = Color.Black;
        }

        private void msktxtSeed_Validated(object sender, EventArgs e)
        {
            Settings.Default.RandomSeed = msktxtSeed.Text;
            Settings.Default.Save();
        }

        private void chkChaos_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ChaosWeeks = chkChaos.Checked;
            Settings.Default.Save();
        }

        private void cboSizes_SelectedIndexChanged(object sender, EventArgs e)
        {
   //         if (cboSizes.SelectedValue != null)
            Settings.Default.SelectedSizeIndex = cboSizes.SelectedIndex;
            Settings.Default.Save();
            strMapSize = cboSizes.SelectedItem.ToString();//.ToString();
        }

        private void chkToPlaySound_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ToPlaySound = chkToPlaySound.Checked;
            Settings.Default.Save();
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {

        }

        private void frmMain_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                System.IO.DirectoryInfo dirinfBackgrounds = new System.IO.DirectoryInfo("BackGrounds");
                System.IO.FileInfo[] fileinfarrBackgrounds = dirinfBackgrounds.GetFiles();
                int iRandomPicture = Randomizer.rnd.Next(fileinfarrBackgrounds.Length);
                pictureBox1.WaitOnLoad = true;
                pictureBox1.ImageLocation = fileinfarrBackgrounds[iRandomPicture].FullName;
                pictureBox1.Refresh();
            }
            catch (Exception)
            {//if no backgrounds folder then just show regular pic
            }
        }

        private void msktxtSeed_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void chkIT_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DisableIT = chkIT.Checked;
            Settings.Default.Save();
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            Settings.Default.RandomTemplate = !Settings.Default.RandomTemplate;
            
            if (Settings.Default.RandomTemplate)
            {
                toolTip1.SetToolTip(btnSwitch, "Click To Switch To Single Template Selection Mode");
                //toolTip1.InitialDelay = 500;
                txtRandomTepmlate.Visible = true;
                btnRandomTemplate.Visible = true;
                txtTemplate.Visible = false;
                btnTemplate.Visible = false;
                btnSwitch.Image = imageList2.Images[0];
            }
            else
            {
                toolTip1.SetToolTip(btnSwitch, "Click To Switch To Random Template Mode");
                //toolTip1.InitialDelay = 500;
                txtRandomTepmlate.Visible = false;
                btnRandomTemplate.Visible = false;
                txtTemplate.Visible = true;
                btnTemplate.Visible = true;
                btnSwitch.Image = imageList2.Images[1];
            }
            BtnSelect1.Focus();
        }
        
        private void btnRandomTemplate_Click(object sender, EventArgs e)
        {

            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult diagresFolder = folderBrowserDialog1.ShowDialog();

            if (diagresFolder == DialogResult.OK)
            {
                //set output directory settings and path
                Settings.Default.TemplatesDirectory = folderBrowserDialog1.SelectedPath;
                Settings.Default.Save();
                txtRandomTepmlate.Text = folderBrowserDialog1.SelectedPath;
            }

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void chkAutoName_CheckedChanged(object sender, EventArgs e)
        {

            Settings.Default.AutoNameMap = chkAutoName.Checked;
            Settings.Default.Save();

            if (chkAutoName.Checked)
                txtMapName.Enabled = false;
            else
                txtMapName.Enabled = true;

        }

        private void txtMapName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRoadStatus_Click(object sender, EventArgs e)
        {
            if (Settings.Default.RoadStatus != 2)
            {
                Settings.Default.RoadStatus++;
                Settings.Default.Save();
                btnRoadStatus.Text = Enum.GetNames(typeof(eRoadStatus))[Settings.Default.RoadStatus].Replace('_', ' ');                
            }
            else
            {
                Settings.Default.RoadStatus = 0;
                Settings.Default.Save();
                btnRoadStatus.Text = Enum.GetNames(typeof(eRoadStatus))[Settings.Default.RoadStatus].Replace('_',' ');                
            }
            btnDwellingsStatus.Focus();
        }

        private void btnTestObjectsArea_Click(object sender, EventArgs e)
        {
            Testing.TestingUtility.TestObjectsArea();
        }

        private void chkDifferentFactions_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DifferentFactions = chkDifferentFactions.Checked;
            Settings.Default.Save();
        }

        private void EnableDifferentFactionsOption()
        {
            chkDifferentFactions.Enabled =
                (iarrSelectedPlayersFactions[0] == (int)eTownType.Random ||
                 iarrSelectedPlayersFactions[1] == (int)eTownType.Random ||
                (iarrSelectedPlayersFactions[2] == (int)eTownType.Random && pictureBox3.Visible)||
                (iarrSelectedPlayersFactions[3] == (int)eTownType.Random && pictureBox4.Visible));
            checkBox4.Enabled = !chkDifferentFactions.Enabled;
            if(!checkBox4.Enabled) checkBox4.Checked = false;
            Settings.Default.SelectedFactionGuards = checkBox4.Checked;
            Settings.Default.Save();

        }

        private void btnShowGarrisonsPower_Click(object sender, EventArgs e)
        {
            XmlDocument xdocGuards = new XmlDocument();
            xdocGuards.Load(Assembly.GetExecutingAssembly().CodeBase.Substring(0, Assembly.GetExecutingAssembly().CodeBase.Length - 12).Substring(8) + "Data/Monsters.xml");
            MonstersParser m_parser = new MonstersParser(xdocGuards.DocumentElement);
            TownGarrisonParser tg_parser = new TownGarrisonParser(xdocGuards.DocumentElement,
                                                EnumConvert.MapSizeFromInt(Settings.Default.SelectedSizeIndex));
            TownGarrisonTester.ShowAllTownGarrisonsPower(tg_parser, m_parser);
        }

        private void btnDwellingStatus_Click(object sender, EventArgs e)
        {
            Settings.Default.DwellingStatus = Settings.Default.DwellingStatus == 2 ?
                0 :
                Settings.Default.DwellingStatus + 1;

            Settings.Default.Save();
            btnDwellingsStatus.Text = Enum.GetNames(typeof(eDwellingStatus))[Settings.Default.DwellingStatus];
            chkboxAdvanced.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.OnlyPortals = checkBox1.Checked;
            Settings.Default.Save();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.NoBlocks = checkBox2.Checked;
            Settings.Default.Save();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.SingleFactionGuards = checkBox3.Checked;
            Settings.Default.Save();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.SelectedFactionGuards = checkBox4.Checked;
            Settings.Default.Save();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.NoNewHighDwelling = checkBox5.Checked;
            Settings.Default.Save();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.NewBankGenerating = checkBox6.Checked;
            Settings.Default.Save();
        }

        private void BtnSelect3_Click(object sender, EventArgs e)
        {
            Point pt = PointToScreen(new Point(button2.Left, button2.Bottom - 3));
            ilp3.Show(pt.X, pt.Y);
        }

        private void BtnSelect4_Click(object sender, EventArgs e)
        {
            Point pt = PointToScreen(new Point(button1.Left, button1.Bottom - 3));
            ilp4.Show(pt.X, pt.Y);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        { //dwp  меню выбора игроков и союзов 
            if (this.MapType.Visible)
            {
                if (this.RadioButton1.Checked)
                {
                    //dwp player3
                    this.pictureBox3.Visible = false;
                    this.label6.Visible = false;
                    this.button2.Visible = false;
                    //dwp player4
                    this.pictureBox4.Visible = false;
                    this.label7.Visible = false;
                    this.button1.Visible = false;
                }
                if (this.RadioButton2.Checked)
                {
                    //dwp player3
                    this.pictureBox3.Visible = true;
                    this.label6.Visible = true;
                    this.button2.Visible = true;
                    //dwp player4
                    this.pictureBox4.Visible = false;
                    this.label7.Visible = false;
                    this.button1.Visible = false;
                }
                if (this.RadioButton3.Checked)
                {
                    //dwp player3
                    this.pictureBox3.Visible = true;
                    this.label6.Visible = true;
                    this.button2.Visible = true;
                    //dwp player4
                    this.pictureBox4.Visible = true;
                    this.label7.Visible = true;
                    this.button1.Visible = true;
                    this.label4.Text = "Team 1";
                    this.label7.Text = "Team 1";
                    this.label5.Text = "Team 2";
                    this.label6.Text = "Team 2";
                }
                else
                {
                    this.label4.Text = "Player 1 :";
                    this.label5.Text = "Player 2 :";
                    this.label6.Text = "Player 3 :";
                    this.label7.Text = "Player 4 :";
                }
                if (this.RadioButton4.Checked)
                {
                     //dwp player3
                     this.pictureBox3.Visible = true;
                     this.label6.Visible = true;
                     this.button2.Visible = true;
                     //dwp player4
                     this.pictureBox4.Visible = true;
                     this.label7.Visible = true;
                     this.button1.Visible = true;
                 }
                 EnableDifferentFactionsOption();
                 this.Refresh();
            }
            this.MapType.Visible = ! this.MapType.Visible;
        }

        private void MapType_MouseHover(object sender, EventArgs e)
        {
            if (EntireMenuMapType)
            {
                EntireMenuMapType = false;
                if (this.RadioButton1.Checked)
                {
                    //dwp player3
                    this.pictureBox3.Visible = false;
                    this.label6.Visible = false;
                    this.button2.Visible = false;
                    //dwp player4
                    this.pictureBox4.Visible = false;
                    this.label7.Visible = false;
                    this.button1.Visible = false;
                }
                if (this.RadioButton2.Checked)
                {
                    //dwp player3
                    this.pictureBox3.Visible = true;
                    this.label6.Visible = true;
                    this.button2.Visible = true;
                    //dwp player4
                    this.pictureBox4.Visible = false;
                    this.label7.Visible = false;
                    this.button1.Visible = false;
                }
                if (this.RadioButton3.Checked)
                {
                    //dwp player3
                    this.pictureBox3.Visible = true;
                    this.label6.Visible = true;
                    this.button2.Visible = true;
                    //dwp player4
                    this.pictureBox4.Visible = true;
                    this.label7.Visible = true;
                    this.button1.Visible = true;
                    this.label4.Text = "Team 1";
                    this.label7.Text = "Team 1";
                    this.label5.Text = "Team 2";
                    this.label6.Text = "Team 2";
                }
                else
                {
                    this.label4.Text = "Player 1 :";
                    this.label5.Text = "Player 2 :";
                    this.label6.Text = "Player 3 :";
                    this.label7.Text = "Player 4 :";
                }
                if (this.RadioButton4.Checked)
                {
                    //dwp player3
                    this.pictureBox3.Visible = true;
                    this.label6.Visible = true;
                    this.button2.Visible = true;
                    //dwp player4
                    this.pictureBox4.Visible = true;
                    this.label7.Visible = true;
                    this.button1.Visible = true;
                }
                ((GroupBox)sender).Visible = false;
                EnableDifferentFactionsOption();
                this.Refresh();
            }
        }

        private void MapType_MouseHover2(object sender, MouseEventArgs e)
        {
            if (e.X >= ((GroupBox)sender).Location.X &&
                e.X <= ((GroupBox)sender).Location.X + ((GroupBox)sender).Size.Width &&
                e.Y >= ((GroupBox)sender).Location.Y &&
                e.Y <= ((GroupBox)sender).Location.Y + ((GroupBox)sender).Size.Height)
            {
               if(!EntireMenuMapType) EntireMenuMapType = true;
            }
            else {
                if (EntireMenuMapType)
                {
                    EntireMenuMapType = false;
                    if (this.RadioButton1.Checked)
                    {
                        //dwp player3
                        this.pictureBox3.Visible = false;
                        this.label6.Visible = false;
                        this.button2.Visible = false;
                        //dwp player4
                        this.pictureBox4.Visible = false;
                        this.label7.Visible = false;
                        this.button1.Visible = false;
                    }
                    if (this.RadioButton2.Checked)
                    {
                        //dwp player3
                        this.pictureBox3.Visible = true;
                        this.label6.Visible = true;
                        this.button2.Visible = true;
                        //dwp player4
                        this.pictureBox4.Visible = false;
                        this.label7.Visible = false;
                        this.button1.Visible = false;
                    }
                    if (this.RadioButton3.Checked)
                    {
                        //dwp player3
                        this.pictureBox3.Visible = true;
                        this.label6.Visible = true;
                        this.button2.Visible = true;
                        //dwp player4
                        this.pictureBox4.Visible = true;
                        this.label7.Visible = true;
                        this.button1.Visible = true;
                        this.label4.Text = "Team 1";
                        this.label7.Text = "Team 1";
                        this.label5.Text = "Team 2";
                        this.label6.Text = "Team 2";
                    }
                    else {
                        this.label4.Text = "Player 1 :";
                        this.label5.Text = "Player 2 :";
                        this.label6.Text = "Player 3 :";
                        this.label7.Text = "Player 4 :";
                    }
                    if (this.RadioButton4.Checked)
                    {
                        //dwp player3
                        this.pictureBox3.Visible = true;
                        this.label6.Visible = true;
                        this.button2.Visible = true;
                        //dwp player4
                        this.pictureBox4.Visible = true;
                        this.label7.Visible = true;
                        this.button1.Visible = true;
                    }
                    ((GroupBox)sender).Visible = false;
                    EnableDifferentFactionsOption();
                    this.Refresh();
                }
            }
        }

        private void RadioButton_MouseEnter(object sender, EventArgs e)
        {
            if (!EntireMenuMapType) EntireMenuMapType = true;
        }
    }
}