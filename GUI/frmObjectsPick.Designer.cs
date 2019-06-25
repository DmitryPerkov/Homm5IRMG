namespace Homm5RMG
{
    partial class frmObjectsPick
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmObjectsPick));
            this.label1 = new System.Windows.Forms.Label();
            this.msktxtChance = new System.Windows.Forms.MaskedTextBox();
            this.chklstBattleObjects = new System.Windows.Forms.CheckedListBox();
            this.chklstEnhancingObjects = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chklstArtifacts = new System.Windows.Forms.CheckedListBox();
            this.btnSetChance1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.msktxtItemChance = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.msktxtItemValue = new System.Windows.Forms.MaskedTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.msktxtItemMaxNumber = new System.Windows.Forms.MaskedTextBox();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSetChance2 = new System.Windows.Forms.Button();
            this.btnSetChance3 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl1DevidedTimesSetChance = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblZoneChance = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblObjectSetChance = new System.Windows.Forms.Label();
            this.lbl1DevidedObjectCount = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblObjectsCount = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.msktxtViewChance1 = new System.Windows.Forms.MaskedTextBox();
            this.msktxtViewChance2 = new System.Windows.Forms.MaskedTextBox();
            this.msktxtViewChance3 = new System.Windows.Forms.MaskedTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.msktxtDwellNumber = new System.Windows.Forms.MaskedTextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(261, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(281, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "Chance for objects in the list to appear in zone : (Chance For Entire Set)";
            // 
            // msktxtChance
            // 
            this.msktxtChance.Location = new System.Drawing.Point(507, 38);
            this.msktxtChance.Mask = "0.00";
            this.msktxtChance.Name = "msktxtChance";
            this.msktxtChance.PromptChar = ' ';
            this.msktxtChance.Size = new System.Drawing.Size(24, 20);
            this.msktxtChance.TabIndex = 6;
            this.msktxtChance.Text = "0";
            this.msktxtChance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.msktxtChance_KeyDown);
            this.msktxtChance.Validating += new System.ComponentModel.CancelEventHandler(this.msktxtChance_Validating);
            this.msktxtChance.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.msktxtChance_MaskInputRejected);
            // 
            // chklstBattleObjects
            // 
            this.chklstBattleObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chklstBattleObjects.FormattingEnabled = true;
            this.chklstBattleObjects.HorizontalScrollbar = true;
            this.chklstBattleObjects.Location = new System.Drawing.Point(161, 136);
            this.chklstBattleObjects.Name = "chklstBattleObjects";
            this.chklstBattleObjects.Size = new System.Drawing.Size(157, 240);
            this.chklstBattleObjects.TabIndex = 8;
            this.chklstBattleObjects.SelectedIndexChanged += new System.EventHandler(this.chklstBattleObjects_SelectedIndexChanged);
            this.chklstBattleObjects.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chklstBattleObjects_ItemCheck);
            this.chklstBattleObjects.SelectedValueChanged += new System.EventHandler(this.chklstBattleObjects_SelectedValueChanged);
            // 
            // chklstEnhancingObjects
            // 
            this.chklstEnhancingObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chklstEnhancingObjects.FormattingEnabled = true;
            this.chklstEnhancingObjects.HorizontalScrollbar = true;
            this.chklstEnhancingObjects.Location = new System.Drawing.Point(324, 136);
            this.chklstEnhancingObjects.Name = "chklstEnhancingObjects";
            this.chklstEnhancingObjects.Size = new System.Drawing.Size(157, 240);
            this.chklstEnhancingObjects.TabIndex = 9;
            this.chklstEnhancingObjects.SelectedIndexChanged += new System.EventHandler(this.chklstEnhancingObjects_SelectedIndexChanged);
            this.chklstEnhancingObjects.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chklstEnhancingObjects_ItemCheck);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(200, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Battle Objects";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(350, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Enhancing Objects";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(539, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Artifacts";
            // 
            // chklstArtifacts
            // 
            this.chklstArtifacts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chklstArtifacts.FormattingEnabled = true;
            this.chklstArtifacts.HorizontalScrollbar = true;
            this.chklstArtifacts.Location = new System.Drawing.Point(487, 136);
            this.chklstArtifacts.Name = "chklstArtifacts";
            this.chklstArtifacts.Size = new System.Drawing.Size(157, 240);
            this.chklstArtifacts.TabIndex = 12;
            this.chklstArtifacts.SelectedIndexChanged += new System.EventHandler(this.chklstArtifacts_SelectedIndexChanged);
            this.chklstArtifacts.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chklstArtifacts_ItemCheck);
            // 
            // btnSetChance1
            // 
            this.btnSetChance1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSetChance1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.btnSetChance1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetChance1.Font = new System.Drawing.Font("Miriam", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSetChance1.Location = new System.Drawing.Point(203, 384);
            this.btnSetChance1.Name = "btnSetChance1";
            this.btnSetChance1.Size = new System.Drawing.Size(103, 18);
            this.btnSetChance1.TabIndex = 14;
            this.btnSetChance1.Text = "Set Chance For All";
            this.btnSetChance1.UseVisualStyleBackColor = false;
            this.btnSetChance1.Click += new System.EventHandler(this.btnSetChance1_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label5.Location = new System.Drawing.Point(-1, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 42);
            this.label5.TabIndex = 15;
            this.label5.Text = "Selected Object Attributes:";
            // 
            // msktxtItemChance
            // 
            this.msktxtItemChance.Location = new System.Drawing.Point(161, 41);
            this.msktxtItemChance.Mask = "0.00";
            this.msktxtItemChance.Name = "msktxtItemChance";
            this.msktxtItemChance.PromptChar = ' ';
            this.msktxtItemChance.Size = new System.Drawing.Size(24, 20);
            this.msktxtItemChance.TabIndex = 16;
            this.msktxtItemChance.Text = "0";
            this.toolTip1.SetToolTip(this.msktxtItemChance, "When the selected item is not checked chance can only be 0.0");
            this.msktxtItemChance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.msktxtItemChance_KeyDown);
            this.msktxtItemChance.Validating += new System.ComponentModel.CancelEventHandler(this.msktxtItemChance_Validating);
            this.msktxtItemChance.TextChanged += new System.EventHandler(this.msktxtItemChance_TextChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label6.Location = new System.Drawing.Point(12, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 20);
            this.label6.TabIndex = 17;
            this.label6.Text = "Chance of object to appear";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label7.Location = new System.Drawing.Point(12, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 69);
            this.label7.TabIndex = 18;
            this.label7.Text = "Object value (Determines how strong object is guarded )";
            // 
            // msktxtItemValue
            // 
            this.msktxtItemValue.Location = new System.Drawing.Point(70, 136);
            this.msktxtItemValue.Mask = "0000000000";
            this.msktxtItemValue.Name = "msktxtItemValue";
            this.msktxtItemValue.PromptChar = ' ';
            this.msktxtItemValue.Size = new System.Drawing.Size(66, 20);
            this.msktxtItemValue.TabIndex = 19;
            this.msktxtItemValue.Text = "1";
            this.msktxtItemValue.Validated += new System.EventHandler(this.msktxtItemValue_Validated);
            this.msktxtItemValue.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.msktxtItemValue_MaskInputRejected);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.toolTip1.ToolTipTitle = "Take Notice";
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // msktxtItemMaxNumber
            // 
            this.msktxtItemMaxNumber.Location = new System.Drawing.Point(161, 64);
            this.msktxtItemMaxNumber.Mask = "00";
            this.msktxtItemMaxNumber.Name = "msktxtItemMaxNumber";
            this.msktxtItemMaxNumber.PromptChar = ' ';
            this.msktxtItemMaxNumber.Size = new System.Drawing.Size(24, 20);
            this.msktxtItemMaxNumber.TabIndex = 29;
            this.msktxtItemMaxNumber.Text = "0";
            this.toolTip1.SetToolTip(this.msktxtItemMaxNumber, "When the selected item is not checked max number can only be 0");
            this.msktxtItemMaxNumber.Validating += new System.ComponentModel.CancelEventHandler(this.msktxtItemMaxNumber_Validating);
            // 
            // btnSetChance2
            // 
            this.btnSetChance2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSetChance2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.btnSetChance2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetChance2.Font = new System.Drawing.Font("Miriam", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSetChance2.Location = new System.Drawing.Point(363, 384);
            this.btnSetChance2.Name = "btnSetChance2";
            this.btnSetChance2.Size = new System.Drawing.Size(102, 18);
            this.btnSetChance2.TabIndex = 20;
            this.btnSetChance2.Text = "Set Chance For All";
            this.btnSetChance2.UseVisualStyleBackColor = false;
            this.btnSetChance2.Click += new System.EventHandler(this.btnSetChance2_Click);
            // 
            // btnSetChance3
            // 
            this.btnSetChance3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSetChance3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.btnSetChance3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetChance3.Font = new System.Drawing.Font("Miriam", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSetChance3.Location = new System.Drawing.Point(526, 384);
            this.btnSetChance3.Name = "btnSetChance3";
            this.btnSetChance3.Size = new System.Drawing.Size(107, 18);
            this.btnSetChance3.TabIndex = 21;
            this.btnSetChance3.Text = "Set Chance For All";
            this.btnSetChance3.UseVisualStyleBackColor = false;
            this.btnSetChance3.Click += new System.EventHandler(this.btnSetChance3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.lbl1DevidedTimesSetChance);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lblZoneChance);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.lblObjectSetChance);
            this.groupBox1.Controls.Add(this.lbl1DevidedObjectCount);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.lblObjectsCount);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(12, 168);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(143, 243);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Statistics";
            // 
            // lbl1DevidedTimesSetChance
            // 
            this.lbl1DevidedTimesSetChance.AutoSize = true;
            this.lbl1DevidedTimesSetChance.Location = new System.Drawing.Point(23, 121);
            this.lbl1DevidedTimesSetChance.Name = "lbl1DevidedTimesSetChance";
            this.lbl1DevidedTimesSetChance.Size = new System.Drawing.Size(78, 13);
            this.lbl1DevidedTimesSetChance.TabIndex = 32;
            this.lbl1DevidedTimesSetChance.Text = "None Selected";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 85);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 36);
            this.label10.TabIndex = 31;
            this.label10.Text = "1/Total Object set count * Entire Set Chance";
            // 
            // lblZoneChance
            // 
            this.lblZoneChance.AutoSize = true;
            this.lblZoneChance.Location = new System.Drawing.Point(23, 224);
            this.lblZoneChance.Name = "lblZoneChance";
            this.lblZoneChance.Size = new System.Drawing.Size(78, 13);
            this.lblZoneChance.TabIndex = 30;
            this.lblZoneChance.Text = "None Selected";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(6, 195);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(131, 29);
            this.label15.TabIndex = 29;
            this.label15.Text = "Zone chance for selected object to appear:";
            // 
            // lblObjectSetChance
            // 
            this.lblObjectSetChance.AutoSize = true;
            this.lblObjectSetChance.Location = new System.Drawing.Point(23, 172);
            this.lblObjectSetChance.Name = "lblObjectSetChance";
            this.lblObjectSetChance.Size = new System.Drawing.Size(78, 13);
            this.lblObjectSetChance.TabIndex = 28;
            this.lblObjectSetChance.Text = "None Selected";
            // 
            // lbl1DevidedObjectCount
            // 
            this.lbl1DevidedObjectCount.AutoSize = true;
            this.lbl1DevidedObjectCount.Location = new System.Drawing.Point(23, 63);
            this.lbl1DevidedObjectCount.Name = "lbl1DevidedObjectCount";
            this.lbl1DevidedObjectCount.Size = new System.Drawing.Size(78, 13);
            this.lbl1DevidedObjectCount.TabIndex = 26;
            this.lbl1DevidedObjectCount.Text = "None Selected";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(6, 143);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(131, 29);
            this.label13.TabIndex = 27;
            this.label13.Text = "In Object set chance for selected object to appear:";
            // 
            // lblObjectsCount
            // 
            this.lblObjectsCount.AutoSize = true;
            this.lblObjectsCount.Location = new System.Drawing.Point(119, 16);
            this.lblObjectsCount.Name = "lblObjectsCount";
            this.lblObjectsCount.Size = new System.Drawing.Size(13, 13);
            this.lblObjectsCount.TabIndex = 24;
            this.lblObjectsCount.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(126, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "1/Total Object set count:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Total object set count:";
            // 
            // msktxtViewChance1
            // 
            this.msktxtViewChance1.Location = new System.Drawing.Point(173, 382);
            this.msktxtViewChance1.Mask = "0.00";
            this.msktxtViewChance1.Name = "msktxtViewChance1";
            this.msktxtViewChance1.PromptChar = ' ';
            this.msktxtViewChance1.Size = new System.Drawing.Size(24, 20);
            this.msktxtViewChance1.TabIndex = 23;
            this.msktxtViewChance1.Text = "0";
            // 
            // msktxtViewChance2
            // 
            this.msktxtViewChance2.Location = new System.Drawing.Point(333, 382);
            this.msktxtViewChance2.Mask = "0.00";
            this.msktxtViewChance2.Name = "msktxtViewChance2";
            this.msktxtViewChance2.PromptChar = ' ';
            this.msktxtViewChance2.Size = new System.Drawing.Size(24, 20);
            this.msktxtViewChance2.TabIndex = 24;
            this.msktxtViewChance2.Text = "0";
            // 
            // msktxtViewChance3
            // 
            this.msktxtViewChance3.Location = new System.Drawing.Point(496, 382);
            this.msktxtViewChance3.Mask = "0.00";
            this.msktxtViewChance3.Name = "msktxtViewChance3";
            this.msktxtViewChance3.PromptChar = ' ';
            this.msktxtViewChance3.Size = new System.Drawing.Size(24, 20);
            this.msktxtViewChance3.TabIndex = 25;
            this.msktxtViewChance3.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(250, 379);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "^";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(407, 379);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "^";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Location = new System.Drawing.Point(571, 379);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 13);
            this.label14.TabIndex = 28;
            this.label14.Text = "^";
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label16.Location = new System.Drawing.Point(261, 68);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(220, 18);
            this.label16.TabIndex = 5;
            this.label16.Text = "Dwellings number to appear in zone:";
            // 
            // msktxtDwellNumber
            // 
            this.msktxtDwellNumber.Location = new System.Drawing.Point(507, 64);
            this.msktxtDwellNumber.Mask = "0-0";
            this.msktxtDwellNumber.Name = "msktxtDwellNumber";
            this.msktxtDwellNumber.PromptChar = ' ';
            this.msktxtDwellNumber.Size = new System.Drawing.Size(24, 20);
            this.msktxtDwellNumber.TabIndex = 6;
            this.msktxtDwellNumber.Text = "0";
            this.msktxtDwellNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.msktxtDwellNumber_KeyDown);
            this.msktxtDwellNumber.Validating += new System.ComponentModel.CancelEventHandler(this.msktxtChance_Validating);
            this.msktxtDwellNumber.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.msktxtDwellNumber_MaskInputRejected);
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label17.Location = new System.Drawing.Point(12, 68);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(143, 12);
            this.label17.TabIndex = 30;
            this.label17.Text = "Amount of attempts";
            // 
            // frmObjectsPick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 417);
            this.Controls.Add(this.msktxtItemMaxNumber);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.msktxtViewChance3);
            this.Controls.Add(this.msktxtViewChance2);
            this.Controls.Add(this.msktxtViewChance1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSetChance3);
            this.Controls.Add(this.btnSetChance2);
            this.Controls.Add(this.msktxtItemValue);
            this.Controls.Add(this.msktxtItemChance);
            this.Controls.Add(this.btnSetChance1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chklstArtifacts);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chklstEnhancingObjects);
            this.Controls.Add(this.chklstBattleObjects);
            this.Controls.Add(this.msktxtDwellNumber);
            this.Controls.Add(this.msktxtChance);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label14);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmObjectsPick";
            this.Text = "Select Objects Set";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmObjectsPick_Load);
            this.VisibleChanged += new System.EventHandler(this.frmObjectsPick_VisibleChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmObjectsPick_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox msktxtChance;
        private System.Windows.Forms.CheckedListBox chklstBattleObjects;
        private System.Windows.Forms.CheckedListBox chklstEnhancingObjects;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckedListBox chklstArtifacts;
        private System.Windows.Forms.Button btnSetChance1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox msktxtItemChance;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox msktxtItemValue;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.Button btnSetChance2;
        private System.Windows.Forms.Button btnSetChance3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl1DevidedObjectCount;
        private System.Windows.Forms.Label lblObjectsCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblObjectSetChance;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblZoneChance;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lbl1DevidedTimesSetChance;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.MaskedTextBox msktxtViewChance1;
        private System.Windows.Forms.MaskedTextBox msktxtViewChance2;
        private System.Windows.Forms.MaskedTextBox msktxtViewChance3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.MaskedTextBox msktxtDwellNumber;
        private System.Windows.Forms.MaskedTextBox msktxtItemMaxNumber;
        private System.Windows.Forms.Label label17;

    }
}