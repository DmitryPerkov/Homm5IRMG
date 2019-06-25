using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Homm5RMG.Properties;
using System.IO;
using System.Xml;


namespace Homm5RMG
{
    public partial class frmTemplateEditor : Form
    {
        #region FormGeneralMembers

        private string[] strTerrains = Enum.GetNames(typeof(eTerrain));
        public TemplateHandler thTemplate;
        private int iCopyIndex = -1;
        private bool fReadyCopySet = false;
        private XmlElement xObjects;
        private const int MAXZONES = 20; //maximum number of zones - more zones will be unrealistic
        private const int MAXCONNECTIONS = 60;
        private const int HEIGHTDIFF = 25; //gui height diff between each set of controls
        private int iZoneCount = 1;
        private int iConnectionCount = 1;
        public bool bWasShowing = false; //if other form was showing 
        public float fTotalPercentage = 0.25f;
        public bool bToCreateNewTemplateFile = false;

        #endregion


        #region Dynamic Controls - Lables
        private System.Windows.Forms.Label[] lblZoneTitle;
        private System.Windows.Forms.Label[] lblMines;
        private System.Windows.Forms.Label[] lblTowns;
        private System.Windows.Forms.Label[] lblTerrain;
        private System.Windows.Forms.Label[] lblSet3;
        private System.Windows.Forms.Label[] lblSet2;
        private System.Windows.Forms.Label[] lblSet1;
        private System.Windows.Forms.Label[] lblMonsterStrength;
        private System.Windows.Forms.Label[] lblSizePrecentage;
        private System.Windows.Forms.Label[] lblThicknes;
        #endregion

        #region Dynamic Controls - Buttons
        private System.Windows.Forms.Button[] btnSet3;
        private System.Windows.Forms.Button[] btnSet2;
        private System.Windows.Forms.Button[] btnSet1;
        private System.Windows.Forms.Button[] btnSetMines;
        private System.Windows.Forms.Button[] btnTowns;
        private System.Windows.Forms.Button[] btnCopy;
        private System.Windows.Forms.Button[] btnPaste;
        #endregion

        #region Dynamic Controls - Others
        private System.Windows.Forms.CheckBox[] chkboxIsStartingZone;
        private System.Windows.Forms.ComboBox[] cboMonsterStrength;
        private System.Windows.Forms.ComboBox[] cboTerrain;
        //private System.Windows.Forms.MaskedTextBox[] msktxtZonePrecentage;
        private PaintDotNet.ToleranceSliderControl[] msktxtZonePrecentage;
        //dwp. контрол для задания толщины границы зоны
        private PaintDotNet.ToleranceSliderControl[] msktxtZoneThickness;
        #endregion

        #region Dynamic Controls - Connection 
        private System.Windows.Forms.Label[] lblConnectsTo;
        private System.Windows.Forms.Label[] lblFirstZoneNumber;
        private System.Windows.Forms.Button[] btnSetMonster;
        private System.Windows.Forms.Label[] lblGuardings;
        private System.Windows.Forms.Label[] lblConnectionNumber;
        private System.Windows.Forms.MaskedTextBox[] msktxtSecondZoneNumber;
        private System.Windows.Forms.MaskedTextBox[] msktxtFirstZoneNumber;
        #endregion

        /* dwp. контекстное меню для копирования сетов*/
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;


        public frmTemplateEditor()
        {
            InitializeComponent();
            this.toleranceSliderControl1.bIgnoreMaximum = true;
            #region Init Lables Arrays
            this.lblZoneTitle = new System.Windows.Forms.Label[MAXZONES];
            this.lblSet3 = new Label[MAXZONES];
            this.lblSet2 = new Label[MAXZONES];
            this.lblSet1 = new Label[MAXZONES];
            this.lblMines = new Label[MAXZONES];
            this.lblTerrain = new Label[MAXZONES];
            this.lblMonsterStrength = new Label[MAXZONES];
            this.lblTowns = new Label[MAXZONES];
            this.lblSizePrecentage = new Label[MAXZONES];
            this.lblThicknes = new Label[MAXZONES];
            #endregion

            #region Init Button Arrays
            this.btnSet1 = new Button[MAXZONES];
            this.btnSet2 = new Button[MAXZONES];
            this.btnSet3 = new Button[MAXZONES];
            this.btnSetMines = new Button[MAXZONES];
            this.btnTowns = new Button[MAXZONES];
            this.btnCopy = new Button[MAXZONES];
            this.btnPaste = new Button[MAXZONES];
            //this.btnSet1 = new Button[MAXZONES];
            #endregion

            #region Init Others
            this.chkboxIsStartingZone = new CheckBox[MAXZONES];
            this.cboTerrain = new ComboBox[MAXZONES];
            this.cboMonsterStrength = new ComboBox[MAXZONES];
            this.msktxtZonePrecentage = new PaintDotNet.ToleranceSliderControl[MAXZONES];
            //dwp. инициализируем набор контролов задания толщин
            this.msktxtZoneThickness = new PaintDotNet.ToleranceSliderControl[MAXZONES];
            #endregion

            #region Init Connection
            lblConnectsTo = new Label[MAXCONNECTIONS];
            lblFirstZoneNumber = new Label[MAXCONNECTIONS];
            btnSetMonster = new Button[MAXCONNECTIONS];
            lblGuardings = new Label[MAXCONNECTIONS];
            lblConnectionNumber = new Label[MAXCONNECTIONS];
            msktxtSecondZoneNumber = new MaskedTextBox[MAXCONNECTIONS];
            msktxtFirstZoneNumber = new MaskedTextBox[MAXCONNECTIONS];
            #endregion

            InitializeZone(iZoneCount - 1);

            InitializeConnection(iConnectionCount - 1);
            

        }


        /// <summary>
        /// remove the zone controls at spacific index
        /// </summary>
        /// <param name="iIndex">the index to remove</param>
        private void RemoveZone(int iIndex)
        {
            #region Remove Labels
            this.tabPage1.Controls.Remove(this.lblZoneTitle[iIndex]);
            this.tabPage1.Controls.Remove(this.lblSet3[iIndex]);
            this.tabPage1.Controls.Remove(this.lblSet2[iIndex]);
            this.tabPage1.Controls.Remove(this.lblSet1[iIndex]);
            this.tabPage1.Controls.Remove(this.lblMines[iIndex]);
            this.tabPage1.Controls.Remove(this.lblTerrain[iIndex]);
            this.tabPage1.Controls.Remove(this.lblSizePrecentage[iIndex]);
            this.tabPage1.Controls.Remove(this.lblThicknes[iIndex]);
            this.tabPage1.Controls.Remove(this.lblMonsterStrength[iIndex]);
            this.tabPage1.Controls.Remove(this.lblTowns[iIndex]);
            #endregion

            #region Remove Buttons
            this.tabPage1.Controls.Remove(this.btnTowns[iIndex]);
            this.tabPage1.Controls.Remove(this.btnSetMines[iIndex]);
            this.tabPage1.Controls.Remove(this.btnSet1[iIndex]);
            this.tabPage1.Controls.Remove(this.btnSet2[iIndex]);
            this.tabPage1.Controls.Remove(this.btnSet3[iIndex]);
            this.tabPage1.Controls.Remove(this.btnCopy[iIndex]);
            this.tabPage1.Controls.Remove(this.btnPaste[iIndex]);
            #endregion

            #region Remove Others

            this.tabPage1.Controls.Remove(this.cboMonsterStrength[iIndex]);
            this.tabPage1.Controls.Remove(this.cboTerrain[iIndex]);
            this.tabPage1.Controls.Remove(this.chkboxIsStartingZone[iIndex]);
            this.tabPage1.Controls.Remove(this.msktxtZonePrecentage[iIndex]);
            //dwp.  удаляем контрол толщины зоны
            this.tabPage1.Controls.Remove(this.msktxtZoneThickness[iIndex]);

            #endregion
        }

        /// <summary>
        /// remove the connection controls at spacific index
        /// </summary>
        /// <param name="iIndex">the index to remove</param>
        private void RemoveConnection(int iIndex)
        {
            this.tabPage2.Controls.Remove(this.lblConnectsTo[iIndex]);
            this.tabPage2.Controls.Remove(this.lblFirstZoneNumber[iIndex]);
            this.tabPage2.Controls.Remove(this.btnSetMonster[iIndex]);
            this.tabPage2.Controls.Remove(this.lblConnectionNumber[iIndex]);
            this.tabPage2.Controls.Remove(this.msktxtSecondZoneNumber[iIndex]);
            this.tabPage2.Controls.Remove(this.msktxtFirstZoneNumber[iIndex]);
            this.tabPage2.Controls.Remove(this.lblGuardings[iIndex]);
        }

        
        private void InitializeZone(int iIndex)
        {
            //groups of 6 columns to set the point i must get the reminder to know where to place the group
            int iDivOfSix = (iIndex) / 6;

            #region Init Buttons

            btnSetMines[iIndex] = new Button();
            // 
            // btnSetMines1
            // 
            //this.btnSetMines[iIndex].BackColor = System.Drawing.SystemColors.ButtonHighlight;
            //this.btnSetMines[iIndex].FlatAppearance.BorderSize = 0;
            //this.btnSetMines[iIndex].FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnSetMines[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetMines[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetMines[iIndex].Location = new System.Drawing.Point(79 + (iIndex - iDivOfSix * 6) * 150, 196 + 450 * iDivOfSix);
            this.btnSetMines[iIndex].Name = "btnSetMines";
            this.btnSetMines[iIndex].Size = new System.Drawing.Size(56, 21);
            this.btnSetMines[iIndex].TabIndex = 8;
            this.btnSetMines[iIndex].Text = "Set";
            this.btnSetMines[iIndex].UseVisualStyleBackColor = true;
            this.btnSetMines[iIndex].Tag = iIndex;
            this.btnSetMines[iIndex].MouseLeave += new System.EventHandler(this.btnSetMines_MouseLeave);
            this.btnSetMines[iIndex].Click += new System.EventHandler(this.btnSetMines_Click);
            this.btnSetMines[iIndex].MouseHover += new System.EventHandler(this.btnSetMines_MouseHover);

            this.tabPage1.Controls.Add(this.btnSetMines[iIndex]);


            btnTowns[iIndex] = new Button();
            // 
            // btnTowns1
            // 
            //this.btnTowns[iIndex].BackColor = System.Drawing.SystemColors.ButtonHighlight;
            //this.btnTowns[iIndex].FlatAppearance.BorderSize = 0;
            //this.btnTowns[iIndex].FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnTowns[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTowns[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTowns[iIndex].Location = new System.Drawing.Point(79 + (iIndex - iDivOfSix * 6) * 150, 314 + 450 * iDivOfSix);//236 + 450 * iDivOfSix);
            this.btnTowns[iIndex].Name = "btnTowns";
            this.btnTowns[iIndex].Size = new System.Drawing.Size(56, 21);
            this.btnTowns[iIndex].TabIndex = 10;
            this.btnTowns[iIndex].Text = "Set";
            this.btnTowns[iIndex].UseVisualStyleBackColor = true;
            this.btnTowns[iIndex].Tag = iIndex;
            this.btnTowns[iIndex].Click += new System.EventHandler(this.btnTowns_Click);
            this.btnTowns[iIndex].MouseHover += new EventHandler(btnSetTowns_MouseHover);
            this.btnTowns[iIndex].MouseLeave += new EventHandler(btnSetTowns_MouseLeave);

            this.tabPage1.Controls.Add(this.btnTowns[iIndex]);

            btnSet3[iIndex] = new Button();
            // 
            // btnSet3
            // 
            //this.btnSet3[iIndex].BackColor = System.Drawing.SystemColors.ButtonHighlight;
            //this.btnSet3[iIndex].FlatAppearance.BorderSize = 0;
            //this.btnSet3[iIndex].FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnSet3[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet3[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet3[iIndex].Location = new System.Drawing.Point(79 + (iIndex - iDivOfSix * 6) * 150, 452 + 450 * iDivOfSix);
            this.btnSet3[iIndex].Name = "btnSet3";
            this.btnSet3[iIndex].Size = new System.Drawing.Size(56, 21);
            this.btnSet3[iIndex].TabIndex = 20;
            this.btnSet3[iIndex].Text = "Set";
            this.btnSet3[iIndex].Tag = iIndex;
            this.btnSet3[iIndex].UseVisualStyleBackColor = true;
            this.btnSet3[iIndex].Click += new EventHandler(btnSet3_Click);
            this.btnSet3[iIndex].ContextMenu = new ContextMenu();
            this.btnSet3[iIndex].ContextMenu.Popup += new EventHandler(btnSet_MouseClick);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Copy set";
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Paste set";
            this.menuItem1.Click += new System.EventHandler(this.menuItem_Click);
            this.menuItem2.Click += new System.EventHandler(this.menuItem_Click);
            this.btnSet3[iIndex].ContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { menuItem1, menuItem2 });
            
            this.tabPage1.Controls.Add(this.btnSet3[iIndex]);

            btnSet2[iIndex] = new Button();
            // 
            // btnSet2
            // 
            //this.btnSet2[iIndex].BackColor = System.Drawing.SystemColors.ButtonHighlight;
            //this.btnSet2[iIndex].FlatAppearance.BorderSize = 0;
            //this.btnSet2[iIndex].FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnSet2[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet2[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet2[iIndex].Location = new System.Drawing.Point(79 + (iIndex - iDivOfSix * 6) * 150, 405 + 450 * iDivOfSix);
            this.btnSet2[iIndex].Name = "btnSet2";
            this.btnSet2[iIndex].Size = new System.Drawing.Size(56, 21);
            this.btnSet2[iIndex].TabIndex = 19;
            this.btnSet2[iIndex].Text = "Set";
            this.btnSet2[iIndex].Tag = iIndex;
            this.btnSet2[iIndex].UseVisualStyleBackColor = true;
            this.btnSet2[iIndex].Click += new EventHandler(btnSet2_Click);
            this.btnSet2[iIndex].ContextMenu = new ContextMenu();
            this.btnSet2[iIndex].ContextMenu.Popup += new EventHandler(btnSet_MouseClick);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Copy set";
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Paste set";
            this.menuItem1.Click += new System.EventHandler(this.menuItem_Click);
            this.menuItem2.Click += new System.EventHandler(this.menuItem_Click);
            this.btnSet2[iIndex].ContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { menuItem1, menuItem2 });

            this.tabPage1.Controls.Add(this.btnSet2[iIndex]);


            btnSet1[iIndex] = new Button();
            // 
            // btnSet1
            // 
            //this.btnSet1[iIndex].BackColor = System.Drawing.SystemColors.ButtonHighlight;
            //this.btnSet1[iIndex].FlatAppearance.BorderSize = 0;
            //this.btnSet1[iIndex].FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnSet1[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet1[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet1[iIndex].Location = new System.Drawing.Point(79 + (iIndex - iDivOfSix * 6) * 150, 361 + 450 * iDivOfSix);
            this.btnSet1[iIndex].Name = "btnSet1";
            this.btnSet1[iIndex].Size = new System.Drawing.Size(56, 21);
            this.btnSet1[iIndex].TabIndex = 18;
            this.btnSet1[iIndex].Text = "Set";
            this.btnSet1[iIndex].Tag = iIndex;
            this.btnSet1[iIndex].UseVisualStyleBackColor = true;
            this.btnSet1[iIndex].Click += new System.EventHandler(btnSet1_Click);
            this.btnSet1[iIndex].ContextMenu = new ContextMenu();
            this.btnSet1[iIndex].ContextMenu.Popup += new EventHandler(btnSet_MouseClick);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Copy set";
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Paste set";
            this.menuItem1.Click += new System.EventHandler(this.menuItem_Click);
            this.menuItem2.Click += new System.EventHandler(this.menuItem_Click);
            this.btnSet1[iIndex].ContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { menuItem1, menuItem2 });

            this.tabPage1.Controls.Add(this.btnSet1[iIndex]);

            this.btnPaste[iIndex] = new Button();
            // 
            // btnPaste1
            // 
            this.btnPaste[iIndex].BackColor = System.Drawing.Color.Transparent;
            //this.btnPaste1.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnPaste[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaste[iIndex].Font = new System.Drawing.Font("Miriam", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnPaste[iIndex].Location = new System.Drawing.Point(108 + (iIndex - iDivOfSix * 6) * 150, 105 + 450 * iDivOfSix);
            this.btnPaste[iIndex].Name = "btnPaste1";
            this.btnPaste[iIndex].Size = new System.Drawing.Size(39, 17);
            this.btnPaste[iIndex].TabIndex = 28;
            this.btnPaste[iIndex].Text = "Paste";
            this.btnPaste[iIndex].UseVisualStyleBackColor = false;
            this.btnPaste[iIndex].Click += new EventHandler(btnPaste_Click);
            this.btnPaste[iIndex].Tag = iIndex;
            if ( iCopyIndex == -1 )
                this.btnPaste[iIndex].Enabled = false;

            this.tabPage1.Controls.Add(this.btnPaste[iIndex]);


            this.btnCopy[iIndex] = new Button();
            // 
            // btnCopy1
            // 
            this.btnCopy[iIndex].BackColor = System.Drawing.Color.Transparent;
            //this.btnCopy1.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnCopy[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy[iIndex].Font = new System.Drawing.Font("Miriam", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnCopy[iIndex].Location = new System.Drawing.Point(52 + (iIndex - iDivOfSix * 6) * 150, 105 + 450 * iDivOfSix);
            this.btnCopy[iIndex].Name = "btnCopy1";
            this.btnCopy[iIndex].Size = new System.Drawing.Size(36, 17);
            this.btnCopy[iIndex].TabIndex = 27;
            this.btnCopy[iIndex].Text = "Copy";
            this.btnCopy[iIndex].UseVisualStyleBackColor = false;
            this.btnCopy[iIndex].Click += new EventHandler(btnCopy_Click);
            this.btnCopy[iIndex].Tag = iIndex;
            this.tabPage1.Controls.Add(this.btnCopy[iIndex]);
            #endregion

            #region Init Of Others

            this.chkboxIsStartingZone[iIndex] = new CheckBox();
            // 
            // chkboxIsStartingZone1
            // 
            this.chkboxIsStartingZone[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkboxIsStartingZone[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkboxIsStartingZone[iIndex].Location = new System.Drawing.Point(79 + (iIndex - iDivOfSix * 6) * 150, 152 + 450 * iDivOfSix);
            this.chkboxIsStartingZone[iIndex].Name = "chkboxIsStartingZone1";
            this.chkboxIsStartingZone[iIndex].Size = new System.Drawing.Size(68, 45);
            this.chkboxIsStartingZone[iIndex].TabIndex = 6;
            this.chkboxIsStartingZone[iIndex].Text = "Is Starting Zone";
            this.chkboxIsStartingZone[iIndex].UseVisualStyleBackColor = true;
            this.chkboxIsStartingZone[iIndex].CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            this.chkboxIsStartingZone[iIndex].Tag = iIndex;

            this.tabPage1.Controls.Add(this.chkboxIsStartingZone[iIndex]);


            this.cboTerrain[iIndex] = new ComboBox();
            // 
            // cboTerrain
            // 
            this.cboTerrain[iIndex].DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTerrain[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboTerrain[iIndex].FormattingEnabled = true;
            //this.cboTerrain[iIndex].Items.AddRange(new object[] {
            //"Grass"});
            this.cboTerrain[iIndex].Items.AddRange( strTerrains );
            this.cboTerrain[iIndex].Location = new System.Drawing.Point(79 + (iIndex - iDivOfSix * 6) * 150, 273 + 450 * iDivOfSix);
            this.cboTerrain[iIndex].Name = "cboTerrain";
            this.cboTerrain[iIndex].Size = new System.Drawing.Size(56, 21);
            this.cboTerrain[iIndex].TabIndex = 12;
            this.cboTerrain[iIndex].Tag = iIndex;
            this.cboTerrain[iIndex].SelectedIndex = 0;
            this.cboTerrain[iIndex].SelectedIndexChanged += new System.EventHandler(this.cboTerrain_SelectedIndexChanged);

            this.tabPage1.Controls.Add(this.cboTerrain[iIndex]);

            this.cboMonsterStrength[iIndex] = new ComboBox();
            // 
            // cboMonsterStrength[iIndex]
            // 
            this.cboMonsterStrength[iIndex].DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonsterStrength[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboMonsterStrength[iIndex].FormattingEnabled = true;
            this.cboMonsterStrength[iIndex].Items.AddRange(new object[] {
            "Weak",
            "Avarage",
            "Strong",
            "Scary",
            "Impossible"});
            this.cboMonsterStrength[iIndex].Location = new System.Drawing.Point(79 + (iIndex - iDivOfSix * 6) * 150, 236 + 450 * iDivOfSix);//314 + 450 * iDivOfSix);
            this.cboMonsterStrength[iIndex].Name = "cboMonsterStrength";
            this.cboMonsterStrength[iIndex].Size = new System.Drawing.Size(56, 21);
            this.cboMonsterStrength[iIndex].TabIndex = 14;

            this.cboMonsterStrength[iIndex].Tag = iIndex;
            this.cboMonsterStrength[iIndex].SelectedIndex = 1;
            
            this.cboMonsterStrength[iIndex].SelectedIndexChanged += new System.EventHandler(this.cboMonsterStrength_SelectedIndexChanged);
            this.tabPage1.Controls.Add(this.cboMonsterStrength[iIndex]);

            msktxtZonePrecentage[iIndex] = new PaintDotNet.ToleranceSliderControl(2,43);
            msktxtZoneThickness[iIndex] = new PaintDotNet.ToleranceSliderControl(7, 15, "");

            this.msktxtZonePrecentage[iIndex].Location = new System.Drawing.Point(79 + (iIndex - iDivOfSix * 6) * 150, 128 + 450 * iDivOfSix);
            this.msktxtZoneThickness[iIndex].Location = new System.Drawing.Point(79 + (iIndex - iDivOfSix * 6) * 150, 503 + 450 * iDivOfSix);
            this.msktxtZonePrecentage[iIndex].Name = "toleranceSliderControl1";
            this.msktxtZoneThickness[iIndex].Name = "toleranceSliderControl2";
            this.msktxtZonePrecentage[iIndex].Size = new System.Drawing.Size(56, 18);
            this.msktxtZoneThickness[iIndex].Size = new System.Drawing.Size(56, 18); 
            //this.msktxtZonePrecentage[iIndex].TabIndex = 25;
            this.msktxtZonePrecentage[iIndex].Text = "toleranceSliderControl1";
            this.msktxtZoneThickness[iIndex].Text = "toleranceSliderControl2";
            this.msktxtZonePrecentage[iIndex].Tolerance = 0.5F;
            this.msktxtZoneThickness[iIndex].Tolerance = 0.5F;
            this.msktxtZonePrecentage[iIndex].ToleranceChanged += new EventHandler(Percentage_Changed);
            this.msktxtZonePrecentage[iIndex].MouseLeave += new EventHandler(Percentage_MouseLeave);

            //dwp!! обработчик события изменение толшины
            this.msktxtZoneThickness[iIndex].MouseLeave += new EventHandler(Thickness_MouseLeave);
            this.msktxtZonePrecentage[iIndex].Tag = iIndex;
            this.msktxtZoneThickness[iIndex].Tag = iIndex;
            this.msktxtZonePrecentage[iIndex].strDisplayedPrecentage = "25%";            
            this.msktxtZoneThickness[iIndex].strDisplayedPrecentage = "7";            
            this.tabPage1.Controls.Add(this.msktxtZonePrecentage[iIndex]);
            this.tabPage1.Controls.Add(this.msktxtZoneThickness[iIndex]);

            #endregion


            #region Init Of Lables


            this.lblZoneTitle[iIndex] = new System.Windows.Forms.Label();

            // 
            // lblZoneTitle
            // 
            this.lblZoneTitle[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblZoneTitle[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZoneTitle[iIndex].Location = new System.Drawing.Point(79 + (iIndex - iDivOfSix * 6) * 150, 87 + 450 * iDivOfSix);
            this.lblZoneTitle[iIndex].Name = "lblZoneTitle";
            this.lblZoneTitle[iIndex].Size = new System.Drawing.Size(84, 21);
            this.lblZoneTitle[iIndex].TabIndex = 0;
            this.lblZoneTitle[iIndex].Text = "Zone " + (iIndex + 1).ToString();

            this.tabPage1.Controls.Add(this.lblZoneTitle[iIndex]);


            this.lblSet3[iIndex] = new Label();
            // 
            // lblSet3[iIndex]
            // 
            this.lblSet3[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSet3[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSet3[iIndex].Location = new System.Drawing.Point(22 + (iIndex - iDivOfSix * 6) * 150, 452 + 450 * iDivOfSix);
            this.lblSet3[iIndex].Name = "lblSet3";
            this.lblSet3[iIndex].Size = new System.Drawing.Size(66, 34);
            this.lblSet3[iIndex].TabIndex = 17;
            this.lblSet3[iIndex].Text = "Object Set #3";

            this.tabPage1.Controls.Add(this.lblSet3[iIndex]);


            this.lblSet2[iIndex] = new Label();
            // 
            // lblSet2[iIndex]
            // 
            this.lblSet2[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSet2[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSet2[iIndex].Location = new System.Drawing.Point(22 + (iIndex - iDivOfSix * 6) * 150, 405 + 450 * iDivOfSix);
            this.lblSet2[iIndex].Name = "lblSet2";
            this.lblSet2[iIndex].Size = new System.Drawing.Size(66, 34);
            this.lblSet2[iIndex].TabIndex = 16;
            this.lblSet2[iIndex].Text = "Object Set #2";

            this.tabPage1.Controls.Add(this.lblSet2[iIndex]);



            this.lblSet1[iIndex] = new Label();
            // 
            // lblSet1[iIndex]
            // 
            this.lblSet1[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSet1[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSet1[iIndex].Location = new System.Drawing.Point(22 + (iIndex - iDivOfSix * 6) * 150, 361 + 450 * iDivOfSix);
            this.lblSet1[iIndex].Name = "lblSet1";
            this.lblSet1[iIndex].Size = new System.Drawing.Size(66, 34);
            this.lblSet1[iIndex].TabIndex = 15;
            this.lblSet1[iIndex].Text = "Object Set #1";

            this.tabPage1.Controls.Add(this.lblSet1[iIndex]);


            this.lblMines[iIndex] = new Label();
            // 
            // lblMines1
            // 
            this.lblMines[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMines[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMines[iIndex].Location = new System.Drawing.Point(22 + (iIndex - iDivOfSix * 6) * 150, 200 + 450 * iDivOfSix);
            this.lblMines[iIndex].Name = "lblMines";
            this.lblMines[iIndex].Size = new System.Drawing.Size(66, 21);
            this.lblMines[iIndex].TabIndex = 7;
            this.lblMines[iIndex].Text = "Mines";


            this.tabPage1.Controls.Add(this.lblMines[iIndex]);


            this.lblTerrain[iIndex] = new Label();
            // 
            // lblTerrain1
            // 
            this.lblTerrain[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTerrain[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTerrain[iIndex].Location = new System.Drawing.Point(22 + (iIndex - iDivOfSix * 6) * 150, 276 + 450 * iDivOfSix);
            this.lblTerrain[iIndex].Name = "lblTerrain";
            this.lblTerrain[iIndex].Size = new System.Drawing.Size(66, 21);
            this.lblTerrain[iIndex].TabIndex = 11;
            this.lblTerrain[iIndex].Text = "Terrain";

            this.tabPage1.Controls.Add(this.lblTerrain[iIndex]);


            this.lblMonsterStrength[iIndex] = new Label();
            // 
            // lblMonsterStrength1
            // 
            this.lblMonsterStrength[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMonsterStrength[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonsterStrength[iIndex].Location = new System.Drawing.Point(22 + (iIndex - iDivOfSix * 6) * 150, 240 + 450 * iDivOfSix);
            this.lblMonsterStrength[iIndex].Name = "lblMonsterStrength";
            this.lblMonsterStrength[iIndex].Size = new System.Drawing.Size(66, 34);
            this.lblMonsterStrength[iIndex].TabIndex = 13;
            this.lblMonsterStrength[iIndex].Text = "Monster Strength";

            this.tabPage1.Controls.Add(this.lblMonsterStrength[iIndex]);


            this.lblTowns[iIndex] = new Label();
            // 
            // lblTowns1
            // 
            this.lblTowns[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTowns[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTowns[iIndex].Location = new System.Drawing.Point(22 + (iIndex - iDivOfSix * 6) * 150, 314 + 450 * iDivOfSix);
            this.lblTowns[iIndex].Name = "lblTowns";
            this.lblTowns[iIndex].Size = new System.Drawing.Size(66, 21);
            this.lblTowns[iIndex].TabIndex = 9;
            this.lblTowns[iIndex].Text = "Towns";

            this.tabPage1.Controls.Add(this.lblTowns[iIndex]);


            this.lblSizePrecentage[iIndex] = new Label();
            this.lblThicknes[iIndex] = new Label();
            // 
            // lblSizePrecentage
            // 
            this.lblSizePrecentage[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblThicknes[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSizePrecentage[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThicknes[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSizePrecentage[iIndex].Location = new System.Drawing.Point(22 + (iIndex - iDivOfSix * 6) * 150, 128 + 450 * iDivOfSix);
            this.lblThicknes[iIndex].Location = new System.Drawing.Point(49 + (iIndex - iDivOfSix * 6) * 150, 486 + 450 * iDivOfSix);
            this.lblSizePrecentage[iIndex].Name = "lblSizePrecentage";
            this.lblThicknes[iIndex].Name = "lblThickness";
            this.lblSizePrecentage[iIndex].Size = new System.Drawing.Size(66, 21);
            this.lblThicknes[iIndex].Size = new System.Drawing.Size(122, 21);
            this.lblSizePrecentage[iIndex].TabIndex = 1;
            this.lblThicknes[iIndex].TabIndex = 1;
            this.lblSizePrecentage[iIndex].Text = "Size in %";
            this.lblThicknes[iIndex].Text = "Thickness of border";

            this.tabPage1.Controls.Add(this.lblSizePrecentage[iIndex]);
            this.tabPage1.Controls.Add(this.lblThicknes[iIndex]);

            #endregion


        }

        void btnCopy_Click(object sender, EventArgs e)
        {
             iCopyIndex = (int)((Button)sender).Tag;
             foreach (Button btnPaste in this.btnPaste)
             {
                 if ( btnPaste != null)
                    btnPaste.Enabled = true;
             }
        }

        /// <summary>
        /// paste entire zone info to other zone (hard copy) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnPaste_Click(object sender, EventArgs e)
        {
            //copy to source , destination
            this.thTemplate.CopyZone(iCopyIndex, (int)((Button)sender).Tag);
            UpdateZoneInfo((int)((Button)sender).Tag);
        }



        private void UpdateZoneInfo(int iUpdateIndex)
        {
            chkboxIsStartingZone[iUpdateIndex].Checked = Convert.ToBoolean(thTemplate.getZoneProperty(iUpdateIndex, eTemplateElements.IsStartingZone.ToString()));
            cboMonsterStrength[iUpdateIndex].SelectedItem = thTemplate.getZoneProperty(iUpdateIndex, eTemplateElements.MonsterStrength.ToString());
            cboTerrain[iUpdateIndex].SelectedItem = thTemplate.getZoneProperty(iUpdateIndex, eTemplateElements.TerrainType.ToString());
            msktxtZonePrecentage[iUpdateIndex].Tolerance = (float)Convert.ToInt32(thTemplate.getZoneProperty(iUpdateIndex, eTemplateElements.SizePrecentage.ToString()).Trim('%')) / 43 - (float)1 / (43 / 2);
            msktxtZoneThickness[iUpdateIndex].Tolerance = (float)(Convert.ToInt32(thTemplate.getZoneProperty(iUpdateIndex, eTemplateElements.Thickness.ToString())) - 7.0f) / 8.0f; 
            this.UpdateTotalPercentage();
            this.toleranceSliderControl1.Tolerance = fTotalPercentage;
        }

        void Percentage_MouseLeave(object sender, EventArgs e)
        {
            this.UpdateTotalPercentage();
            this.toleranceSliderControl1.Tolerance = fTotalPercentage;
            thTemplate.UpdateZoneProperty((int)((PaintDotNet.ToleranceSliderControl)sender).Tag, eTemplateElements.SizePrecentage.ToString(), ((PaintDotNet.ToleranceSliderControl)sender).strDisplayedPrecentage);
        }

        void Thickness_MouseLeave(object sender, EventArgs e)
        {
            //dwp сохраняет толщину границы
            thTemplate.UpdateZoneProperty((int)((PaintDotNet.ToleranceSliderControl)sender).Tag, eTemplateElements.Thickness.ToString(), ((PaintDotNet.ToleranceSliderControl)sender).strDisplayedPrecentage);
        }

        void msktxtZonePrecentage_Validated(object sender, EventArgs e)
        {
            thTemplate.UpdateZoneProperty( (int) ((MaskedTextBox)sender).Tag, eTemplateElements.SizePrecentage.ToString() , ((MaskedTextBox)sender).Text);
        }

        void msktxtThickness_Validated(object sender, EventArgs e)
        {
            thTemplate.UpdateZoneProperty((int)((MaskedTextBox)sender).Tag, eTemplateElements.Thickness.ToString(), ((MaskedTextBox)sender).Text);
        }

        private void InitializeConnection(int iIndex)
        {
            #region Init Controls



            this.msktxtSecondZoneNumber[iIndex] = new MaskedTextBox();
            // 
            // msktxtSecondZoneNumber[iIndex]
            // 
            this.msktxtSecondZoneNumber[iIndex].BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.msktxtSecondZoneNumber[iIndex].Location = new System.Drawing.Point(392, 72 + iIndex * HEIGHTDIFF);
            this.msktxtSecondZoneNumber[iIndex].Mask = "09";
            this.msktxtSecondZoneNumber[iIndex].Name = "msktxtSecondZoneNumber";
            this.msktxtSecondZoneNumber[iIndex].PromptChar = ' ';
            this.msktxtSecondZoneNumber[iIndex].Size = new System.Drawing.Size(27, 18);
            this.msktxtSecondZoneNumber[iIndex].TabIndex = 13;
            this.msktxtSecondZoneNumber[iIndex].Text = "2";
            this.msktxtSecondZoneNumber[iIndex].TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.msktxtSecondZoneNumber[iIndex].Validated += new EventHandler(msktxtSecondZoneNumber_Validated);
            this.msktxtSecondZoneNumber[iIndex].Tag = iIndex;

            this.tabPage2.Controls.Add(this.msktxtSecondZoneNumber[iIndex]);



            this.msktxtFirstZoneNumber[iIndex] = new MaskedTextBox();
            // 
            // msktxtFirstZoneNumber[iIndex]
            // 
            this.msktxtFirstZoneNumber[iIndex].BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.msktxtFirstZoneNumber[iIndex].Location = new System.Drawing.Point(216, 72 + iIndex * HEIGHTDIFF);
            this.msktxtFirstZoneNumber[iIndex].Mask = "09";
            this.msktxtFirstZoneNumber[iIndex].Name = "msktxtFirstZoneNumber";
            this.msktxtFirstZoneNumber[iIndex].PromptChar = ' ';
            this.msktxtFirstZoneNumber[iIndex].Size = new System.Drawing.Size(27, 18);
            this.msktxtFirstZoneNumber[iIndex].TabIndex = 12;
            this.msktxtFirstZoneNumber[iIndex].Text = "1";
            this.msktxtFirstZoneNumber[iIndex].TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.msktxtFirstZoneNumber[iIndex].Validated += new EventHandler(msktxtFirstZoneNumber_Validated);
            this.msktxtFirstZoneNumber[iIndex].Tag = iIndex;

            this.tabPage2.Controls.Add(this.msktxtFirstZoneNumber[iIndex]);


            this.lblConnectionNumber[iIndex] = new Label();
            // 
            // lblConnectionNumber[iIndex]
            // 
            this.lblConnectionNumber[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblConnectionNumber[iIndex].Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionNumber[iIndex].Location = new System.Drawing.Point(95, 69 + iIndex * HEIGHTDIFF);
            this.lblConnectionNumber[iIndex].Name = "lblConnectionNumber";
            this.lblConnectionNumber[iIndex].Size = new System.Drawing.Size(53, 21);
            this.lblConnectionNumber[iIndex].TabIndex = 11;
            this.lblConnectionNumber[iIndex].Text = "[ " + (iIndex + 1).ToString() + " ]";


            this.tabPage2.Controls.Add(this.lblConnectionNumber[iIndex]);

            this.btnSetMonster[iIndex] = new Button();
            // 
            // btnSetMonster1
            // 
            this.btnSetMonster[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetMonster[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetMonster[iIndex].Location = new System.Drawing.Point(551, 67 + iIndex * HEIGHTDIFF);
            this.btnSetMonster[iIndex].Name = "btnSetMonster";
            this.btnSetMonster[iIndex].Size = new System.Drawing.Size(64, 23);
            this.btnSetMonster[iIndex].Tag = iIndex;
            this.btnSetMonster[iIndex].TabIndex = 8;
            this.btnSetMonster[iIndex].Text = "Set";
            this.btnSetMonster[iIndex].UseVisualStyleBackColor = true;
            this.btnSetMonster[iIndex].Click += new EventHandler(btnSetMonster_Click);

            this.tabPage2.Controls.Add(this.btnSetMonster[iIndex]);



            this.lblGuardings[iIndex] = new Label();
            // 
            // lblGuardings[iIndex]
            // 
            this.lblGuardings[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblGuardings[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuardings[iIndex].Location = new System.Drawing.Point(425, 72 + iIndex * HEIGHTDIFF);
            this.lblGuardings[iIndex].Name = "lblGuardings";
            this.lblGuardings[iIndex].Size = new System.Drawing.Size(120, 21);
            this.lblGuardings[iIndex].TabIndex = 7;
            this.lblGuardings[iIndex].Text = "Guarding monster is";

            this.tabPage2.Controls.Add(this.lblGuardings[iIndex]);


            this.lblConnectsTo[iIndex] = new Label();
            // 
            // lblConnectsTo[iIndex]
            // 
            this.lblConnectsTo[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblConnectsTo[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectsTo[iIndex].Location = new System.Drawing.Point(249, 73 + iIndex * HEIGHTDIFF);
            this.lblConnectsTo1.Name = "lblConnectsTo";
            this.lblConnectsTo[iIndex].Size = new System.Drawing.Size(144, 21);
            this.lblConnectsTo[iIndex].TabIndex = 5;
            this.lblConnectsTo[iIndex].Text = "Connects To Zone No.";

            this.tabPage2.Controls.Add(this.lblConnectsTo[iIndex]);






            this.lblFirstZoneNumber[iIndex] = new Label();
            // 
            // lblFirstZoneNumber[iIndex]
            // 
            this.lblFirstZoneNumber[iIndex].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFirstZoneNumber[iIndex].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstZoneNumber[iIndex].Location = new System.Drawing.Point(154, 73 + iIndex * HEIGHTDIFF);
            this.lblFirstZoneNumber[iIndex].Name = "lblFirstZoneNumber";
            this.lblFirstZoneNumber[iIndex].Size = new System.Drawing.Size(66, 21);
            this.lblFirstZoneNumber[iIndex].TabIndex = 3;
            this.lblFirstZoneNumber[iIndex].Text = "Zone No.";

            this.tabPage2.Controls.Add(this.lblFirstZoneNumber[iIndex]);





            #endregion
        }

        void msktxtFirstZoneNumber_Validated(object sender, EventArgs e)
        {
            thTemplate.UpdateConnectionAttribute((int)((MaskedTextBox)sender).Tag, eTemplateElements.Zone.ToString(), ((MaskedTextBox)sender).Text);
        }

        void msktxtSecondZoneNumber_Validated(object sender, EventArgs e)
        {
            thTemplate.UpdateConnectionAttribute((int)((MaskedTextBox)sender).Tag, eTemplateElements.Connects_To.ToString(), ((MaskedTextBox)sender).Text);
        }

        void btnSetMonster_Click(object sender, EventArgs e)
        {
            Program.frmGuards.iConnection = (int) ( ((Button)sender).Tag );
            Program.frmGuards.Show();

            //throw new Exception("The method or operation is not implemented.");
        }

        void btnSetTowns_MouseLeave(object sender, EventArgs e)
        {
            if (!bWasShowing)
            {

                Program.frmSetTowns.Hide();
                ((Button)sender).Focus();
            }
            //throw new Exception("The method or operation is not implemented.");
        }

        void btnSetTowns_MouseHover(object sender, EventArgs e)
        {
            if (!bWasShowing)
            {
                if (((Button)sender).Location.Y > 300)
                {
                    //Program.frmSetMines.SetDesktopLocation(600, 500);
                    //Program.frmSetMines.StartPosition = FormStartPosition.Manual;
                    Program.frmSetTowns.iZoneIndex = (int)((Button)sender).Tag; 
                    Program.frmSetTowns.Location = new Point(350, 0);
                    Program.frmSetTowns.Show();
                    Program.frmSetTowns.SetPreviewVisible();
                    Program.frmSetTowns.FormBorderStyle = FormBorderStyle.None;
                    Program.frmSetTowns.Opacity = 0.90;
                }

                
                //   bIsShowing = true;
            }
            //throw new Exception("The method or operation is not implemented.");
        }



        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Application.Exit();
            Environment.Exit(-1);

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            if (bToCreateNewTemplateFile)
            {
                DialogResult diagresToDeleteForm = MessageBox.Show("Current template file has not yet been saved ", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (diagresToDeleteForm == DialogResult.OK)
                {
                    Program.frmMainMenu.Show();
                    this.Hide();
                }
            }
            else
            {
                Program.frmMainMenu.Show();
                this.Hide();
            }
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// will happend when one of the precentage control is changed
        /// </summary>
        /// <param name="sender">the precentage control</param>
        /// <param name="e"></param>
        private void Percentage_Changed(object sender, EventArgs e)
        {
            this.UpdateTotalPercentage();
            this.toleranceSliderControl1.Tolerance = fTotalPercentage;
        }

        private void UpdateTotalPercentage()
        {
            //fTotalPercentage = 0;

            int iPercentageSum = 0;
            for (int i = 0; i < this.iZoneCount; i++)
            {
                 iPercentageSum += int.Parse ( msktxtZonePrecentage[i].strDisplayedPrecentage.Trim('%') ); 
            }
            //convert the precentage sum to float for control uses
            fTotalPercentage = (float) iPercentageSum / 100;            
        }
        

        private void Form2_Load(object sender, EventArgs e)
        {
            //template file default
            #region templatefile
            openFileDialog1.DefaultExt = "irt";
            //openFileDialog1.FileName = System.Configuration.ConfigurationManager.AppSettings["DefaultTemplateFile"];
            openFileDialog1.FileName = Settings.Default.DefaultTemplateFile;
            openFileDialog1.Filter = "Idan's RMG Template files (*.irt)|*.irt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
     //       txtTemplate.Text = System.IO.Directory.GetCurrentDirectory() + @"\" + System.Configuration.ConfigurationManager.AppSettings["DefaultTemplateFile"];
            #endregion

            //save file dialog settings
            #region templatefile
            
            saveFileDialog1.DefaultExt = "irt";
            //saveFileDialog1.FileName = System.Configuration.ConfigurationManager.AppSettings["DefaultTemplateFile"];
            saveFileDialog1.FileName = Settings.Default.DefaultTemplateFile;
            saveFileDialog1.Filter = "Idan's RMG Template files (*.irt)|*.irt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            //       txtTemplate.Text = System.IO.Directory.GetCurrentDirectory() + @"\" + System.Configuration.ConfigurationManager.AppSettings["DefaultTemplateFile"];
            #endregion

            if (!File.Exists(Settings.Default.DefaultTemplateFile))
            {
                bToCreateNewTemplateFile = true;
            }

            if (bToCreateNewTemplateFile)
            {
                //bToCreateNewTemplateFile = false;
                thTemplate = new TemplateHandler();
                this.Text = "Template Editor - New Unsaved"  ;
                ReadTemplate();
            }
            else
            {
                try
                {
                    thTemplate = new TemplateHandler(Settings.Default.DefaultTemplateFile);
                    this.Text = "Template Editor - " + Settings.Default.DefaultTemplateFile;
                    ReadTemplate();
                }
                catch(Exception ee )
                {
                    MessageBox.Show( string.Format( "Error in loading last known template file {0} A new template been created instead" ,Settings.Default.DefaultTemplateFile ) + Environment.NewLine + "Error Detail : " + ee.Message);
                    thTemplate = new TemplateHandler();
                    this.Text = "Template Editor - New Unsaved";
                    ReadTemplate();
                }
            }
            

            this.UpdateTotalPercentage();
            this.toleranceSliderControl1.Tolerance = fTotalPercentage;

        }



        /// <summary>
        /// will go over all template files in template dir and create a new name for file.
        /// </summary>
        /// <returns></returns>
        //private string GetNewTemplateFileName()
        //{
        //    DirectoryInfo dirinfTemplateDir = new DirectoryInfo(Settings.Default.TemplateDirectory);
        //    int i = 0;
            
        //    foreach (FileInfo finfTemplateFile in dirinfTemplateDir.GetFiles())
        //    {
        //        if (finfTemplateFile.Name 
        //    }
        //}




        public void ReadTemplate()
        {

            #region Read Zone Related Data
            //first zone is initilized by default so its out of the loop since it don't need zoen creation code
            chkboxIsStartingZone[0].Checked = Convert.ToBoolean(thTemplate.getZoneProperty(0, eTemplateElements.IsStartingZone.ToString()));
            cboMonsterStrength[0].SelectedItem = thTemplate.getZoneProperty(0, eTemplateElements.MonsterStrength.ToString());
            cboTerrain[0].SelectedItem = thTemplate.getZoneProperty(0, eTemplateElements.TerrainType.ToString());
            msktxtZonePrecentage[0].strDisplayedPrecentage = thTemplate.getZoneProperty(0, eTemplateElements.SizePrecentage.ToString());
            msktxtZoneThickness[0].strDisplayedPrecentage = thTemplate.getZoneProperty(0, eTemplateElements.Thickness.ToString());
            msktxtZonePrecentage[0].Tolerance = (float)Convert.ToInt32(thTemplate.getZoneProperty(0, eTemplateElements.SizePrecentage.ToString()).Trim('%')) / 43 - (float)1 / (43 / 2);
            msktxtZoneThickness[0].Tolerance = ((float)(Convert.ToInt32(thTemplate.getZoneProperty(0, eTemplateElements.Thickness.ToString()))) - 7.0f)/ 8.0f;
            
            for (int i = 1; i < thTemplate.ZoneNumber; i++)
            {
                
                iZoneCount++;
                
                InitializeZone(iZoneCount - 1);
                btnRemoveZone.Enabled = true;
                chkboxIsStartingZone[i].Checked = Convert.ToBoolean(thTemplate.getZoneProperty(i, eTemplateElements.IsStartingZone.ToString()));
                cboMonsterStrength[i].SelectedItem = thTemplate.getZoneProperty(i, eTemplateElements.MonsterStrength.ToString());
                cboTerrain[i].SelectedItem = thTemplate.getZoneProperty(i, eTemplateElements.TerrainType.ToString());
                msktxtZonePrecentage[i].strDisplayedPrecentage = thTemplate.getZoneProperty(i, eTemplateElements.SizePrecentage.ToString());
                msktxtZoneThickness[i].strDisplayedPrecentage = thTemplate.getZoneProperty(i, eTemplateElements.Thickness.ToString());
                msktxtZonePrecentage[i].Tolerance = (float)Convert.ToInt32(thTemplate.getZoneProperty(i, eTemplateElements.SizePrecentage.ToString()).Trim('%')) / 43 - (float)1 / (43 / 2);
                msktxtZoneThickness[i].Tolerance = ((float)(Convert.ToInt32(thTemplate.getZoneProperty(i, eTemplateElements.Thickness.ToString()))) - 7.0f) / 8.0f;
            }
            #endregion
            #region Read connection data

            msktxtFirstZoneNumber[0].Text = thTemplate.getConnectionProperty ( 0 , eTemplateElements.Zone.ToString() );
            msktxtSecondZoneNumber[0].Text = thTemplate.getConnectionProperty(0, eTemplateElements.Connects_To.ToString());

            for (int i = 1; i < thTemplate.ConnectionNumber; i++)
            {
                iConnectionCount++;
                btnRemoveConnection.Enabled = true;
                InitializeConnection(iConnectionCount - 1);
                msktxtFirstZoneNumber[i].Text = thTemplate.getConnectionProperty(i, eTemplateElements.Zone.ToString());
                msktxtSecondZoneNumber[i].Text = thTemplate.getConnectionProperty(i, eTemplateElements.Connects_To.ToString());
            }

            #endregion
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult diagresTemplate = openFileDialog1.ShowDialog();

            if (diagresTemplate == DialogResult.OK)
            {
                Settings.Default.DefaultTemplateFile = openFileDialog1.FileName;
                this.Text = "Template Editor " + openFileDialog1.FileName;
                Settings.Default.Save();
                OpenTemplate(openFileDialog1.FileName);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Graphics formGraphics = e.Graphics;
            //System.Drawing.Drawing2D.LinearGradientBrush gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(tabControl1.Width, 0), new Point(this.Width, 0),  Color.Silver ,Color.LightSlateGray);
            //formGraphics.FillRectangle(gradientBrush,tabControl1.Width ,0 ,this.Width - tabControl1.Width ,this.Height);

            //System.Drawing.Drawing2D.LinearGradientBrush gradientBrush2 = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(tabControl1.Width, 0), Color.Silver, Color.LightSlateGray);

            //formGraphics.FillRectangle(gradientBrush2, 0 , tabControl1.Height, tabControl1.Width , this.Height - tabControl1.Height);

            //base.OnPaint(e);
        }

        public void OpenTemplate(string strFileName)
        {

            //remove all zones (but first which is mandatory
            while (iZoneCount != 1)
            {
                RemoveZone(iZoneCount - 1);
                iZoneCount--;
            }
            
            //remove all connection but first  which is mandatory
            while (iConnectionCount != 1)
            {
                RemoveConnection ( iConnectionCount - 1);
                iConnectionCount--;
            }

            try
            {
                //open new template file
                thTemplate = new TemplateHandler(strFileName);
                //update all controls based on file.
                ReadTemplate();
            }
            catch (Exception ee)
            {
                MessageBox.Show("An error occoured during loading the template file is invalid");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bToCreateNewTemplateFile)
            {
                DialogResult diagresTemplate = saveFileDialog1.ShowDialog();

                if (diagresTemplate == DialogResult.OK)
                {
                    //thTemplate.strFileName = saveFileDialog1.FileName;
                    thTemplate.SaveTemplateToFile(saveFileDialog1.FileName);
                    Settings.Default.DefaultTemplateFile = saveFileDialog1.FileName;
                    Settings.Default.Save();
                    this.Text = "Template Editor " + saveFileDialog1.FileName;
                    bToCreateNewTemplateFile = false;
                    MessageBox.Show("Saved", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {

                thTemplate.SaveTemplateToFile();
                bToCreateNewTemplateFile = false;
                MessageBox.Show("Saved", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult diagresTemplate = saveFileDialog1.ShowDialog();

            if (diagresTemplate == DialogResult.OK)
            {
                //thTemplate.strFileName = saveFileDialog1.FileName;
                thTemplate.SaveTemplateToFile(saveFileDialog1.FileName);
                Settings.Default.DefaultTemplateFile = saveFileDialog1.FileName;
                Settings.Default.Save();
                this.Text = "Template Editor " + saveFileDialog1.FileName;
                bToCreateNewTemplateFile = false;
                MessageBox.Show("Saved", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }


        public void SaveTemplate(string strFileName)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 frmAbout = new AboutBox1();
            frmAbout.Show();
            //MessageBox.Show("This option is not yet available , Maybe in a newer version . Current Version Alpha 0.7");
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This option is not yet available , Maybe in a newer version");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.heroesofmightandmagic.com/heroes5/modding_wiki/random_map_generator_by_idan:general");
            //MessageBox.Show(" No Help Exist Yet");
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnSetMines_Click(object sender, EventArgs e)
        {
            Program.frmSetMines.iZoneIndex = (int)((Button)sender).Tag;
            bWasShowing = true;
            Program.frmSetMines.Hide();
            Program.frmSetMines.Location = new Point(200, 250);
            Program.frmSetMines.Show();
        }

        private void btnTowns_Click(object sender, EventArgs e)
        {
            Program.frmSetTowns.iZoneIndex = (int)((Button)sender).Tag;
            bWasShowing = true;
            Program.frmSetTowns.Hide();
            Program.frmSetTowns.Location = new Point(200, 250);
            Program.frmSetTowns.Show();
            
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }


        private void tabPage2_Paint(object sender, PaintEventArgs e)
        {
            Graphics formGraphics = e.Graphics;
            System.Drawing.Drawing2D.LinearGradientBrush gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), Color.LightSlateGray, Color.LightSteelBlue);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle); 
        }

        private void tabPage1_Paint(object sender, PaintEventArgs e)
        {
            Graphics formGraphics = e.Graphics;
            System.Drawing.Drawing2D.LinearGradientBrush gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), Color.LightSlateGray, Color.LightSteelBlue);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle); 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            thTemplate.UpdateZoneProperty((int)((CheckBox)sender).Tag, eTemplateElements.IsStartingZone.ToString(), ((CheckBox)sender).Checked.ToString());
        }

        private void lblSizePrecentage_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void btnAddZone_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void lblMines_Click(object sender, EventArgs e)
        {

        }

        private void cboTerrain_SelectedIndexChanged(object sender, EventArgs e)
        {
            thTemplate.UpdateZoneProperty((int)((ComboBox)sender).Tag, eTemplateElements.TerrainType.ToString(), ((ComboBox)sender).SelectedItem.ToString());
        }

        private void btnAddZone_Click_1(object sender, EventArgs e)
        {
            iZoneCount++;
            InitializeZone(iZoneCount - 1);            
            btnRemoveZone.Enabled = true;
            ((Button)sender).Focus();

            thTemplate.AddNewZone();

            UpdateTotalPercentage();
            this.toleranceSliderControl1.Tolerance = fTotalPercentage;

        }

        private void btnRemoveZone_Click(object sender, EventArgs e)
        {

            

            RemoveZone(iZoneCount - 1);
            iZoneCount--;
            if (iZoneCount == 1)
                btnRemoveZone.Enabled = false;

            ((Button)sender).Focus();

            thTemplate.DeleteZone();
            UpdateTotalPercentage();
            this.toleranceSliderControl1.Tolerance = fTotalPercentage;
            
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            DialogResult diagresToDeleteForm = MessageBox.Show("Are you sure you want to clear all settings ? Notice that any unsaved changes will be lost ", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (diagresToDeleteForm == DialogResult.OK)
            {
                Program.frmTemplateEdit = new frmTemplateEditor();
                this.Dispose();
                Program.frmTemplateEdit.bToCreateNewTemplateFile = true;
                Program.frmTemplateEdit.Show();
                
            }
        }

        private void btnSet1_Click(object sender, EventArgs e)
        {
            Program.frmObjectsSelect.iZoneIndex = (int)(((Button)sender).Tag);
            Program.frmObjectsSelect.iObjectSet = 1;
            if (Settings.Default.NewBankGenerating)
            {//dwp необходимо подстветить галочки в каждой зоне в каждом сете, если их там нет конечно
                XmlElement xObjects1;
                XmlNode xndNodeToTemplate, Xlink;
                XmlNodeList xndFromObjects1, xndFromObjects2;

                ObjectsReader obrdUpdater = new ObjectsReader();

                xndFromObjects1 = obrdUpdater.GetObjectsData()["BattleObjects"].SelectNodes(".//Object[@Type='new']");
                xndFromObjects2 = obrdUpdater.GetObjectsData()["Enhancers"].SelectNodes(".//Object[@Type='new']");

                xObjects1 = Program.frmTemplateEdit.thTemplate.GetObjectsSet(Program.frmObjectsSelect.iZoneIndex, 1);
                foreach (XmlNode xndObject in xndFromObjects1)
                {
                    xndNodeToTemplate = xObjects1.SelectSingleNode(".//Object[@Name='" + xndObject.Attributes["Name"].Value + "']");
                    if (xndNodeToTemplate == null)
                    {
                        Xlink = xndObject.CloneNode(true);
                        xObjects1.SelectSingleNode(".//BattleObjects").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
                    }
                }
                foreach (XmlNode xndObject in xndFromObjects2)
                {
                    xndNodeToTemplate = xObjects1.SelectSingleNode("//Object[@Name='" + xndObject.Attributes["Name"].Value + "']");
                    if (xndNodeToTemplate == null)
                    {
                        Xlink = xndObject.CloneNode(true);
                        xObjects1.SelectSingleNode(".//Enhancers").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
                    }
                }

            }
            Program.frmObjectsSelect.Show();
        }

        private void btnSet2_Click(object sender, EventArgs e)
        {
            Program.frmObjectsSelect.iZoneIndex = (int)(((Button)sender).Tag);
            Program.frmObjectsSelect.iObjectSet = 2;
            if (Settings.Default.NewBankGenerating)
            {//dwp необходимо подстветить галочки в каждой зоне в каждом сете, если их там нет конечно
                XmlElement xObjects2;
                XmlNode xndNodeToTemplate, Xlink;
                XmlNodeList xndFromObjects1, xndFromObjects2;

                ObjectsReader obrdUpdater = new ObjectsReader();

                xndFromObjects1 = obrdUpdater.GetObjectsData()["BattleObjects"].SelectNodes(".//Object[@Type='new']");
                xndFromObjects2 = obrdUpdater.GetObjectsData()["Enhancers"].SelectNodes(".//Object[@Type='new']");

                xObjects2 = Program.frmTemplateEdit.thTemplate.GetObjectsSet(Program.frmObjectsSelect.iZoneIndex, 2);
                foreach (XmlNode xndObject in xndFromObjects1)
                {
                    xndNodeToTemplate = xObjects2.SelectSingleNode(".//Object[@Name='" + xndObject.Attributes["Name"].Value + "']");
                    if (xndNodeToTemplate == null)
                    {
                        Xlink = xndObject.CloneNode(true);
                        xObjects2.SelectSingleNode(".//BattleObjects").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
                    }
                }
                foreach (XmlNode xndObject in xndFromObjects2)
                {
                    xndNodeToTemplate = xObjects2.SelectSingleNode(".//Object[@Name='" + xndObject.Attributes["Name"].Value + "']");
                    if (xndNodeToTemplate == null)
                    {
                        Xlink = xndObject.CloneNode(true);
                        xObjects2.SelectSingleNode(".//Enhancers").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
                    }
                }

            }
            Program.frmObjectsSelect.Show();
        }

        private void btnSet3_Click(object sender, EventArgs e)
        {
            Program.frmObjectsSelect.iZoneIndex = (int)(((Button)sender).Tag);
            Program.frmObjectsSelect.iObjectSet = 3;
            if (Settings.Default.NewBankGenerating)
            {//dwp необходимо подстветить галочки в каждой зоне в каждом сете, если их там нет конечно
                XmlElement xObjects3;
                XmlNode xndNodeToTemplate, Xlink;
                XmlNodeList xndFromObjects1, xndFromObjects2;

                ObjectsReader obrdUpdater = new ObjectsReader();

                xndFromObjects1 = obrdUpdater.GetObjectsData()["BattleObjects"].SelectNodes(".//Object[@Type='new']");
                xndFromObjects2 = obrdUpdater.GetObjectsData()["Enhancers"].SelectNodes(".//Object[@Type='new']");

                xObjects3 = Program.frmTemplateEdit.thTemplate.GetObjectsSet(Program.frmObjectsSelect.iZoneIndex, 3);
                foreach (XmlNode xndObject in xndFromObjects1)
                {
                    xndNodeToTemplate = xObjects3.SelectSingleNode(".//Object[@Name='" + xndObject.Attributes["Name"].Value + "']");
                    if (xndNodeToTemplate == null)
                    {
                        Xlink = xndObject.CloneNode(true);
                        xObjects3.SelectSingleNode(".//BattleObjects").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
                    }
                }
                foreach (XmlNode xndObject in xndFromObjects2)
                {
                    xndNodeToTemplate = xObjects3.SelectSingleNode(".//Object[@Name='" + xndObject.Attributes["Name"].Value + "']");
                    if (xndNodeToTemplate == null)
                    {
                        Xlink = xndObject.CloneNode(true);
                        xObjects3.SelectSingleNode(".//Enhancers").AppendChild(Program.frmTemplateEdit.thTemplate.xdocTemplateFile.ImportNode(Xlink, true));
                    }
                }

            }
            Program.frmObjectsSelect.Show();
        }

        private void btnSetMines_MouseHover(object sender, EventArgs e)
        {
            //if form is showing once then don't reappear it again
            if ( !bWasShowing)
            {                
                //Program.frmSetMines.SetDesktopLocation(600, 500);
                //Program.frmSetMines.StartPosition = FormStartPosition.Manual;
                Program.frmSetMines.Location = new Point(300, 0);
                //set the zone index to display..
                Program.frmSetMines.iZoneIndex = (int)((Button)sender).Tag;
                Program.frmSetMines.Show();
                Program.frmSetMines.SetPreviewVisible();
                Program.frmSetMines.FormBorderStyle = FormBorderStyle.None;
                Program.frmSetMines.Opacity = 0.65;
                
                
             //   bIsShowing = true;
               
            

            }


        }

        private void btnSetMines_MouseLeave(object sender, EventArgs e)
        {
            
            if (!bWasShowing)
            {

                Program.frmSetMines.Hide();
                ((Button)sender).Focus();
            }
        }



        private void cboMonsterStrength_SelectedIndexChanged(object sender, EventArgs e)
        {
            thTemplate.UpdateZoneProperty((int)((ComboBox)sender).Tag, eTemplateElements.MonsterStrength.ToString(), ((ComboBox)sender).SelectedItem.ToString());
        }

        private void downloadTemplateCenterTohToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.toheroes.com");
            //frmTemplateSite frmDownloadCenter = new frmTemplateSite();
            //frmDownloadCenter.Show();
        }

        private void btnAddConnection_Click(object sender, EventArgs e)
        {
            iConnectionCount++;
            InitializeConnection(iConnectionCount - 1);
            btnRemoveConnection.Enabled = true;
            ((Button)sender).Focus();

            thTemplate.AddNewConnection();
            

        }

        private void btnRemoveConnection_Click(object sender, EventArgs e)
        {

            RemoveConnection(iConnectionCount - 1);
            iConnectionCount--;
            if (iConnectionCount == 1)
                btnRemoveConnection.Enabled = false;

            ((Button)sender).Focus();

            thTemplate.DeleteConnection();
        }

        private void addCustomObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddCustomObject frmAddObject = new frmAddCustomObject();
            frmAddObject.Show();

        }

        private void btnSetMonster1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            StringBuilder emailMessage = new StringBuilder();

            emailMessage.Append("mailto:Nevermind_y@yahoo.com");
            emailMessage.Append("&subject=IRMG Suggestion/Bug Report");
            emailMessage.Append("&body=Dear Idan , ");


            System.Diagnostics.Process.Start(emailMessage.ToString());
        }

        private void openObjectsExcelaidsWithObjectsSetPlanningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Data/Objects.xls");
            }
            catch (Exception ee)
            {
                
                MessageBox.Show (" Unable to run file : " + ee.Message );
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        private void menuItem_Click(object sender, EventArgs e)
        {
            switch (((MenuItem)sender).Text) {
                case "Copy set":
                    fReadyCopySet = true;
                    //dwp запоминаем сет
                    xObjects = (XmlElement)Program.frmTemplateEdit.thTemplate.GetObjectsSet(
                      (int)((Button)((ContextMenu)((MenuItem)sender).Parent).SourceControl).Tag,
                      int.Parse(((Button)((ContextMenu)((MenuItem)sender).Parent).SourceControl).Name.Replace("btnSet",""))).FirstChild;
                    break;
                case "Paste set":
                    //dwp воспроизводим запомненный сет 
                    if (fReadyCopySet) 
                    this.thTemplate.UpdateObjectSetProperty(xObjects, (int)((Button)((ContextMenu)((MenuItem)sender).Parent).SourceControl).Tag,
                         int.Parse(((Button)((ContextMenu)((MenuItem)sender).Parent).SourceControl).Name.Replace("btnSet", "")));
                    break;
            }
        }

        private void btnSet_MouseClick(object sender, EventArgs e)
        {

            ((ContextMenu)sender).MenuItems[1].Enabled = fReadyCopySet;
        }
 
    }
}