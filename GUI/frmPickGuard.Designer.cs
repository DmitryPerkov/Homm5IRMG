namespace Homm5RMG
{
    partial class frmPickGuard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPickGuard));
            this.cboMonsterSlot1 = new System.Windows.Forms.ComboBox();
            this.cboMonsterSlot2 = new System.Windows.Forms.ComboBox();
            this.cboMonsterSlot3 = new System.Windows.Forms.ComboBox();
            this.lblSlot1 = new System.Windows.Forms.Label();
            this.lblSlot2 = new System.Windows.Forms.Label();
            this.lblSlot3 = new System.Windows.Forms.Label();
            this.lblMainValue = new System.Windows.Forms.Label();
            this.msktxtValueSlot1 = new System.Windows.Forms.MaskedTextBox();
            this.msktxtValueSlot2 = new System.Windows.Forms.MaskedTextBox();
            this.msktxtValueSlot3 = new System.Windows.Forms.MaskedTextBox();
            this.radbtnInsideGate = new System.Windows.Forms.RadioButton();
            this.radbtnInsideHero = new System.Windows.Forms.RadioButton();
            this.radbtnNormal = new System.Windows.Forms.RadioButton();
            this.msktxtValueSlot6 = new System.Windows.Forms.MaskedTextBox();
            this.msktxtValueSlot5 = new System.Windows.Forms.MaskedTextBox();
            this.msktxtValueSlot4 = new System.Windows.Forms.MaskedTextBox();
            this.lblSlot6 = new System.Windows.Forms.Label();
            this.lblSlot5 = new System.Windows.Forms.Label();
            this.lblSlot4 = new System.Windows.Forms.Label();
            this.cboMonsterSlot6 = new System.Windows.Forms.ComboBox();
            this.cboMonsterSlot5 = new System.Windows.Forms.ComboBox();
            this.cboMonsterSlot4 = new System.Windows.Forms.ComboBox();
            this.maskedTextBox7 = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.msktxtValueSlot7 = new System.Windows.Forms.MaskedTextBox();
            this.lblSlot7 = new System.Windows.Forms.Label();
            this.cboMonsterSlot7 = new System.Windows.Forms.ComboBox();
            this.radbtnCustom = new System.Windows.Forms.RadioButton();
            this.lblNumber = new System.Windows.Forms.Label();
            this.msktxtMainValue = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // cboMonsterSlot1
            // 
            this.cboMonsterSlot1.FormattingEnabled = true;
            this.cboMonsterSlot1.Items.AddRange(new object[] {
            "Random Level 1-2",
            "Random Level 3-5",
            "Random Level 6-7"});
            this.cboMonsterSlot1.Location = new System.Drawing.Point(56, 194);
            this.cboMonsterSlot1.Name = "cboMonsterSlot1";
            this.cboMonsterSlot1.Size = new System.Drawing.Size(132, 21);
            this.cboMonsterSlot1.TabIndex = 0;
            this.cboMonsterSlot1.Text = "None";
            this.cboMonsterSlot1.Visible = false;
            // 
            // cboMonsterSlot2
            // 
            this.cboMonsterSlot2.FormattingEnabled = true;
            this.cboMonsterSlot2.Location = new System.Drawing.Point(194, 194);
            this.cboMonsterSlot2.Name = "cboMonsterSlot2";
            this.cboMonsterSlot2.Size = new System.Drawing.Size(132, 21);
            this.cboMonsterSlot2.TabIndex = 1;
            this.cboMonsterSlot2.Text = "None";
            this.cboMonsterSlot2.Visible = false;
            this.cboMonsterSlot2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // cboMonsterSlot3
            // 
            this.cboMonsterSlot3.FormattingEnabled = true;
            this.cboMonsterSlot3.Location = new System.Drawing.Point(332, 194);
            this.cboMonsterSlot3.Name = "cboMonsterSlot3";
            this.cboMonsterSlot3.Size = new System.Drawing.Size(132, 21);
            this.cboMonsterSlot3.TabIndex = 2;
            this.cboMonsterSlot3.Text = "None";
            this.cboMonsterSlot3.Visible = false;
            this.cboMonsterSlot3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // lblSlot1
            // 
            this.lblSlot1.AutoSize = true;
            this.lblSlot1.BackColor = System.Drawing.Color.Transparent;
            this.lblSlot1.Location = new System.Drawing.Point(53, 178);
            this.lblSlot1.Name = "lblSlot1";
            this.lblSlot1.Size = new System.Drawing.Size(34, 13);
            this.lblSlot1.TabIndex = 3;
            this.lblSlot1.Text = "Slot 1";
            this.lblSlot1.Visible = false;
            // 
            // lblSlot2
            // 
            this.lblSlot2.AutoSize = true;
            this.lblSlot2.BackColor = System.Drawing.Color.Transparent;
            this.lblSlot2.Location = new System.Drawing.Point(190, 178);
            this.lblSlot2.Name = "lblSlot2";
            this.lblSlot2.Size = new System.Drawing.Size(34, 13);
            this.lblSlot2.TabIndex = 4;
            this.lblSlot2.Text = "Slot 2";
            this.lblSlot2.Visible = false;
            // 
            // lblSlot3
            // 
            this.lblSlot3.AutoSize = true;
            this.lblSlot3.BackColor = System.Drawing.Color.Transparent;
            this.lblSlot3.Location = new System.Drawing.Point(329, 178);
            this.lblSlot3.Name = "lblSlot3";
            this.lblSlot3.Size = new System.Drawing.Size(34, 13);
            this.lblSlot3.TabIndex = 5;
            this.lblSlot3.Text = "Slot 3";
            this.lblSlot3.Visible = false;
            // 
            // lblMainValue
            // 
            this.lblMainValue.BackColor = System.Drawing.Color.Transparent;
            this.lblMainValue.Location = new System.Drawing.Point(368, 40);
            this.lblMainValue.Name = "lblMainValue";
            this.lblMainValue.Size = new System.Drawing.Size(103, 48);
            this.lblMainValue.TabIndex = 6;
            this.lblMainValue.Text = "Value:                   (Determins Strength)";
            // 
            // msktxtValueSlot1
            // 
            this.msktxtValueSlot1.Location = new System.Drawing.Point(57, 229);
            this.msktxtValueSlot1.Mask = "0000000";
            this.msktxtValueSlot1.Name = "msktxtValueSlot1";
            this.msktxtValueSlot1.PromptChar = ' ';
            this.msktxtValueSlot1.Size = new System.Drawing.Size(131, 20);
            this.msktxtValueSlot1.TabIndex = 7;
            this.msktxtValueSlot1.Text = "1000";
            this.msktxtValueSlot1.Visible = false;
            // 
            // msktxtValueSlot2
            // 
            this.msktxtValueSlot2.Location = new System.Drawing.Point(194, 229);
            this.msktxtValueSlot2.Mask = "0000000";
            this.msktxtValueSlot2.Name = "msktxtValueSlot2";
            this.msktxtValueSlot2.PromptChar = ' ';
            this.msktxtValueSlot2.Size = new System.Drawing.Size(132, 20);
            this.msktxtValueSlot2.TabIndex = 8;
            this.msktxtValueSlot2.Text = "1000";
            this.msktxtValueSlot2.Visible = false;
            // 
            // msktxtValueSlot3
            // 
            this.msktxtValueSlot3.Location = new System.Drawing.Point(332, 229);
            this.msktxtValueSlot3.Mask = "0000000";
            this.msktxtValueSlot3.Name = "msktxtValueSlot3";
            this.msktxtValueSlot3.PromptChar = ' ';
            this.msktxtValueSlot3.Size = new System.Drawing.Size(132, 20);
            this.msktxtValueSlot3.TabIndex = 9;
            this.msktxtValueSlot3.Text = "1000";
            this.msktxtValueSlot3.Visible = false;
            // 
            // radbtnInsideGate
            // 
            this.radbtnInsideGate.AutoSize = true;
            this.radbtnInsideGate.BackColor = System.Drawing.Color.Transparent;
            this.radbtnInsideGate.Enabled = false;
            this.radbtnInsideGate.Location = new System.Drawing.Point(31, 15);
            this.radbtnInsideGate.Name = "radbtnInsideGate";
            this.radbtnInsideGate.Size = new System.Drawing.Size(116, 17);
            this.radbtnInsideGate.TabIndex = 10;
            this.radbtnInsideGate.Text = "Guards Inside Gate";
            this.radbtnInsideGate.UseVisualStyleBackColor = false;
            this.radbtnInsideGate.CheckedChanged += new System.EventHandler(this.radbtnInsideGate_CheckedChanged);
            // 
            // radbtnInsideHero
            // 
            this.radbtnInsideHero.AutoSize = true;
            this.radbtnInsideHero.BackColor = System.Drawing.Color.Transparent;
            this.radbtnInsideHero.Enabled = false;
            this.radbtnInsideHero.Location = new System.Drawing.Point(31, 38);
            this.radbtnInsideHero.Name = "radbtnInsideHero";
            this.radbtnInsideHero.Size = new System.Drawing.Size(190, 17);
            this.radbtnInsideHero.TabIndex = 11;
            this.radbtnInsideHero.Text = "Guards With Random Neutral Hero";
            this.radbtnInsideHero.UseVisualStyleBackColor = false;
            this.radbtnInsideHero.CheckedChanged += new System.EventHandler(this.radbtnInsideHero_CheckedChanged);
            // 
            // radbtnNormal
            // 
            this.radbtnNormal.AutoSize = true;
            this.radbtnNormal.BackColor = System.Drawing.Color.Transparent;
            this.radbtnNormal.Checked = true;
            this.radbtnNormal.Location = new System.Drawing.Point(31, 61);
            this.radbtnNormal.Name = "radbtnNormal";
            this.radbtnNormal.Size = new System.Drawing.Size(88, 17);
            this.radbtnNormal.TabIndex = 12;
            this.radbtnNormal.TabStop = true;
            this.radbtnNormal.Text = "Normal Mode";
            this.radbtnNormal.UseVisualStyleBackColor = false;
            this.radbtnNormal.CheckedChanged += new System.EventHandler(this.radbtnNormal_CheckedChanged);
            // 
            // msktxtValueSlot6
            // 
            this.msktxtValueSlot6.Location = new System.Drawing.Point(746, 229);
            this.msktxtValueSlot6.Mask = "0000000";
            this.msktxtValueSlot6.Name = "msktxtValueSlot6";
            this.msktxtValueSlot6.PromptChar = ' ';
            this.msktxtValueSlot6.Size = new System.Drawing.Size(132, 20);
            this.msktxtValueSlot6.TabIndex = 22;
            this.msktxtValueSlot6.Text = "1000";
            this.msktxtValueSlot6.Visible = false;
            // 
            // msktxtValueSlot5
            // 
            this.msktxtValueSlot5.Location = new System.Drawing.Point(608, 229);
            this.msktxtValueSlot5.Mask = "0000000";
            this.msktxtValueSlot5.Name = "msktxtValueSlot5";
            this.msktxtValueSlot5.PromptChar = ' ';
            this.msktxtValueSlot5.Size = new System.Drawing.Size(132, 20);
            this.msktxtValueSlot5.TabIndex = 21;
            this.msktxtValueSlot5.Text = "1000";
            this.msktxtValueSlot5.Visible = false;
            // 
            // msktxtValueSlot4
            // 
            this.msktxtValueSlot4.Location = new System.Drawing.Point(471, 229);
            this.msktxtValueSlot4.Mask = "0000000";
            this.msktxtValueSlot4.Name = "msktxtValueSlot4";
            this.msktxtValueSlot4.PromptChar = ' ';
            this.msktxtValueSlot4.Size = new System.Drawing.Size(132, 20);
            this.msktxtValueSlot4.TabIndex = 20;
            this.msktxtValueSlot4.Text = "1000";
            this.msktxtValueSlot4.Visible = false;
            // 
            // lblSlot6
            // 
            this.lblSlot6.AutoSize = true;
            this.lblSlot6.BackColor = System.Drawing.Color.Transparent;
            this.lblSlot6.Location = new System.Drawing.Point(743, 178);
            this.lblSlot6.Name = "lblSlot6";
            this.lblSlot6.Size = new System.Drawing.Size(34, 13);
            this.lblSlot6.TabIndex = 19;
            this.lblSlot6.Text = "Slot 6";
            this.lblSlot6.Visible = false;
            this.lblSlot6.Click += new System.EventHandler(this.label5_Click);
            // 
            // lblSlot5
            // 
            this.lblSlot5.AutoSize = true;
            this.lblSlot5.BackColor = System.Drawing.Color.Transparent;
            this.lblSlot5.Location = new System.Drawing.Point(604, 178);
            this.lblSlot5.Name = "lblSlot5";
            this.lblSlot5.Size = new System.Drawing.Size(34, 13);
            this.lblSlot5.TabIndex = 18;
            this.lblSlot5.Text = "Slot 5";
            this.lblSlot5.Visible = false;
            // 
            // lblSlot4
            // 
            this.lblSlot4.AutoSize = true;
            this.lblSlot4.BackColor = System.Drawing.Color.Transparent;
            this.lblSlot4.Location = new System.Drawing.Point(467, 178);
            this.lblSlot4.Name = "lblSlot4";
            this.lblSlot4.Size = new System.Drawing.Size(34, 13);
            this.lblSlot4.TabIndex = 17;
            this.lblSlot4.Text = "Slot 4";
            this.lblSlot4.Visible = false;
            // 
            // cboMonsterSlot6
            // 
            this.cboMonsterSlot6.FormattingEnabled = true;
            this.cboMonsterSlot6.Location = new System.Drawing.Point(746, 194);
            this.cboMonsterSlot6.Name = "cboMonsterSlot6";
            this.cboMonsterSlot6.Size = new System.Drawing.Size(132, 21);
            this.cboMonsterSlot6.TabIndex = 16;
            this.cboMonsterSlot6.Text = "None";
            this.cboMonsterSlot6.Visible = false;
            // 
            // cboMonsterSlot5
            // 
            this.cboMonsterSlot5.FormattingEnabled = true;
            this.cboMonsterSlot5.Location = new System.Drawing.Point(608, 194);
            this.cboMonsterSlot5.Name = "cboMonsterSlot5";
            this.cboMonsterSlot5.Size = new System.Drawing.Size(132, 21);
            this.cboMonsterSlot5.TabIndex = 15;
            this.cboMonsterSlot5.Text = "None";
            this.cboMonsterSlot5.Visible = false;
            // 
            // cboMonsterSlot4
            // 
            this.cboMonsterSlot4.FormattingEnabled = true;
            this.cboMonsterSlot4.Location = new System.Drawing.Point(470, 194);
            this.cboMonsterSlot4.Name = "cboMonsterSlot4";
            this.cboMonsterSlot4.Size = new System.Drawing.Size(132, 21);
            this.cboMonsterSlot4.TabIndex = 14;
            this.cboMonsterSlot4.Text = "None";
            this.cboMonsterSlot4.Visible = false;
            // 
            // maskedTextBox7
            // 
            this.maskedTextBox7.Location = new System.Drawing.Point(-76, -93);
            this.maskedTextBox7.Mask = "0000000";
            this.maskedTextBox7.Name = "maskedTextBox7";
            this.maskedTextBox7.PromptChar = ' ';
            this.maskedTextBox7.Size = new System.Drawing.Size(67, 20);
            this.maskedTextBox7.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(-79, -144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Slot 3";
            // 
            // comboBox7
            // 
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Location = new System.Drawing.Point(-76, -128);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(67, 21);
            this.comboBox7.TabIndex = 23;
            // 
            // msktxtValueSlot7
            // 
            this.msktxtValueSlot7.Location = new System.Drawing.Point(884, 229);
            this.msktxtValueSlot7.Mask = "0000000";
            this.msktxtValueSlot7.Name = "msktxtValueSlot7";
            this.msktxtValueSlot7.PromptChar = ' ';
            this.msktxtValueSlot7.Size = new System.Drawing.Size(132, 20);
            this.msktxtValueSlot7.TabIndex = 28;
            this.msktxtValueSlot7.Text = "1000";
            this.msktxtValueSlot7.Visible = false;
            // 
            // lblSlot7
            // 
            this.lblSlot7.AutoSize = true;
            this.lblSlot7.BackColor = System.Drawing.Color.Transparent;
            this.lblSlot7.Location = new System.Drawing.Point(884, 178);
            this.lblSlot7.Name = "lblSlot7";
            this.lblSlot7.Size = new System.Drawing.Size(34, 13);
            this.lblSlot7.TabIndex = 27;
            this.lblSlot7.Text = "Slot 7";
            this.lblSlot7.Visible = false;
            // 
            // cboMonsterSlot7
            // 
            this.cboMonsterSlot7.FormattingEnabled = true;
            this.cboMonsterSlot7.Location = new System.Drawing.Point(884, 194);
            this.cboMonsterSlot7.Name = "cboMonsterSlot7";
            this.cboMonsterSlot7.Size = new System.Drawing.Size(132, 21);
            this.cboMonsterSlot7.TabIndex = 26;
            this.cboMonsterSlot7.Text = "None";
            this.cboMonsterSlot7.Visible = false;
            // 
            // radbtnCustom
            // 
            this.radbtnCustom.AutoSize = true;
            this.radbtnCustom.BackColor = System.Drawing.Color.Transparent;
            this.radbtnCustom.Enabled = false;
            this.radbtnCustom.Location = new System.Drawing.Point(31, 84);
            this.radbtnCustom.Name = "radbtnCustom";
            this.radbtnCustom.Size = new System.Drawing.Size(60, 17);
            this.radbtnCustom.TabIndex = 29;
            this.radbtnCustom.Text = "Custom";
            this.radbtnCustom.UseVisualStyleBackColor = false;
            this.radbtnCustom.CheckedChanged += new System.EventHandler(this.radbtnCustom_CheckedChanged);
            // 
            // lblNumber
            // 
            this.lblNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblNumber.Location = new System.Drawing.Point(2, 232);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(60, 24);
            this.lblNumber.TabIndex = 30;
            this.lblNumber.Text = "Number";
            this.lblNumber.Visible = false;
            // 
            // msktxtMainValue
            // 
            this.msktxtMainValue.Location = new System.Drawing.Point(493, 40);
            this.msktxtMainValue.Mask = "0000000";
            this.msktxtMainValue.Name = "msktxtMainValue";
            this.msktxtMainValue.PromptChar = ' ';
            this.msktxtMainValue.Size = new System.Drawing.Size(132, 20);
            this.msktxtMainValue.TabIndex = 31;
            this.msktxtMainValue.Text = "1000";
            // 
            // frmPickGuard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 260);
            this.Controls.Add(this.msktxtMainValue);
            this.Controls.Add(this.radbtnCustom);
            this.Controls.Add(this.msktxtValueSlot7);
            this.Controls.Add(this.lblSlot7);
            this.Controls.Add(this.cboMonsterSlot7);
            this.Controls.Add(this.maskedTextBox7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox7);
            this.Controls.Add(this.msktxtValueSlot6);
            this.Controls.Add(this.msktxtValueSlot5);
            this.Controls.Add(this.msktxtValueSlot4);
            this.Controls.Add(this.lblSlot6);
            this.Controls.Add(this.lblSlot5);
            this.Controls.Add(this.lblSlot4);
            this.Controls.Add(this.cboMonsterSlot6);
            this.Controls.Add(this.cboMonsterSlot5);
            this.Controls.Add(this.cboMonsterSlot4);
            this.Controls.Add(this.radbtnNormal);
            this.Controls.Add(this.radbtnInsideHero);
            this.Controls.Add(this.radbtnInsideGate);
            this.Controls.Add(this.msktxtValueSlot3);
            this.Controls.Add(this.msktxtValueSlot2);
            this.Controls.Add(this.msktxtValueSlot1);
            this.Controls.Add(this.lblSlot3);
            this.Controls.Add(this.lblSlot2);
            this.Controls.Add(this.lblSlot1);
            this.Controls.Add(this.cboMonsterSlot3);
            this.Controls.Add(this.cboMonsterSlot2);
            this.Controls.Add(this.cboMonsterSlot1);
            this.Controls.Add(this.lblMainValue);
            this.Controls.Add(this.lblNumber);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPickGuard";
            this.Text = "Select Guardians";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPickGuard_Load);
            this.Activated += new System.EventHandler(this.frmPickGuard_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPickGuard_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboMonsterSlot1;
        private System.Windows.Forms.ComboBox cboMonsterSlot2;
        private System.Windows.Forms.ComboBox cboMonsterSlot3;
        private System.Windows.Forms.Label lblSlot1;
        private System.Windows.Forms.Label lblSlot2;
        private System.Windows.Forms.Label lblSlot3;
        private System.Windows.Forms.Label lblMainValue;
        private System.Windows.Forms.MaskedTextBox msktxtValueSlot1;
        private System.Windows.Forms.MaskedTextBox msktxtValueSlot2;
        private System.Windows.Forms.MaskedTextBox msktxtValueSlot3;
        private System.Windows.Forms.RadioButton radbtnInsideGate;
        private System.Windows.Forms.RadioButton radbtnInsideHero;
        private System.Windows.Forms.RadioButton radbtnNormal;
        private System.Windows.Forms.MaskedTextBox msktxtValueSlot6;
        private System.Windows.Forms.MaskedTextBox msktxtValueSlot5;
        private System.Windows.Forms.MaskedTextBox msktxtValueSlot4;
        private System.Windows.Forms.Label lblSlot6;
        private System.Windows.Forms.Label lblSlot5;
        private System.Windows.Forms.Label lblSlot4;
        private System.Windows.Forms.ComboBox cboMonsterSlot6;
        private System.Windows.Forms.ComboBox cboMonsterSlot5;
        private System.Windows.Forms.ComboBox cboMonsterSlot4;
        private System.Windows.Forms.MaskedTextBox maskedTextBox7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.MaskedTextBox msktxtValueSlot7;
        private System.Windows.Forms.Label lblSlot7;
        private System.Windows.Forms.ComboBox cboMonsterSlot7;
        private System.Windows.Forms.RadioButton radbtnCustom;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.MaskedTextBox msktxtMainValue;
    }
}