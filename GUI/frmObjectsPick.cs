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
    public partial class frmObjectsPick : Form
    {
        public int iZoneIndex = 1;
        public int iObjectSet = 1;
        XmlElement xObjects;
        public frmObjectsPick()
        {
            InitializeComponent();
        }

        /// <summary>
        /// in this method read object values from objects.xml and place them in chklistboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmObjectsPick_Load(object sender, EventArgs e)
        {

            //xObjects = (XmlElement)Program.frmTemplateEdit.thTemplate.GetObjectsSet(iZoneIndex, iObjectSet).FirstChild;
            ////ObjectsReader obRead = new ObjectsReader();
            ////xObjects =  obRead.GetObjectsData();

            ////obRead.FixToObjects();
            ////obRead.AddNamesToObjects();
            ////obRead.AddSizeObjects();

            //chklstBattleObjects.Items.Add(new TemplateMapObject( "Select All" ));
            //chklstEnhancingObjects.Items.Add(new TemplateMapObject("Select All"));
            //chklstArtifacts.Items.Add(new TemplateMapObject("Select All"));

            ////fill combo box with all objects (stored in objects.xml ) 
            //Fillcbo(xObjects);

            ////fill overall appear chance 

        }

        /// <summary>
        /// fills a chklstbox with objects
        /// </summary>
        /// <param name="xObjects"></param>
        public void Fillcbo(XmlElement xObjects)
        {
            foreach ( XmlNode xndObjectTypes in xObjects.ChildNodes )
            {                                
                if (xndObjectTypes.Name == "BattleObjects")
                {
                    foreach (XmlNode xndObjects in xndObjectTypes.ChildNodes)
                    {
                        TemplateMapObject moItem = new TemplateMapObject( xndObjects.Attributes["Name"].Value );
                        if (xndObjects.Attributes.GetNamedItem("Value") != null)
                            moItem.Value = xndObjects.Attributes["Value"].Value;
                        else
                            moItem.Value = "1000";
                        if (xndObjects.Attributes.GetNamedItem("Chance") != null)
                            moItem.Chance = xndObjects.Attributes["Chance"].Value;
                        else
                            moItem.Chance = "0.0";
                        if (xndObjects.Attributes.GetNamedItem("MaxNumber") != null)
                            moItem.MaxNumber = xndObjects.Attributes["MaxNumber"].Value;
                        else
                            moItem.MaxNumber = "0";

                        chklstBattleObjects.Items.Add(moItem, Convert.ToDouble(moItem.Chance, System.Globalization.CultureInfo.InvariantCulture) > 0.0);
                    }
                }
                else
                {
                    if (xndObjectTypes.Name == "Enhancers")
                    {

                        foreach (XmlNode xndObjects in xndObjectTypes.ChildNodes)
                        {
                            //chklstObjects.ThreeDCheckBoxes
                            //chklstObjects.
                            TemplateMapObject moItem = new TemplateMapObject(xndObjects.Attributes["Name"].Value);
                            if (xndObjects.Attributes.GetNamedItem("Value") != null)
                                moItem.Value = xndObjects.Attributes["Value"].Value;
                            else
                                moItem.Value = "1000";
                            if (xndObjects.Attributes.GetNamedItem("Chance") != null)
                                moItem.Chance = xndObjects.Attributes["Chance"].Value;
                            else
                                moItem.Chance = "1.0";
                            if (xndObjects.Attributes.GetNamedItem("MaxNumber") != null)
                                moItem.MaxNumber = xndObjects.Attributes["MaxNumber"].Value;
                            else
                                moItem.MaxNumber = "0";

                            chklstEnhancingObjects.Items.Add(moItem, Convert.ToDouble(moItem.Chance, System.Globalization.CultureInfo.InvariantCulture) > 0.0);
                            //  cboObjectList.Items.Add(xndObjects.Attributes["Name"].Value);
                        }
                    }
                    else
                    {
                        foreach (XmlNode xndObjects in xndObjectTypes.ChildNodes)
                        {
                            //chklstObjects.ThreeDCheckBoxes
                            //chklstObjects.
                            TemplateMapObject moItem = new TemplateMapObject(xndObjects.Attributes["Name"].Value);
                            if (xndObjects.Attributes.GetNamedItem("Value") != null)
                                moItem.Value = xndObjects.Attributes["Value"].Value;
                            else
                                moItem.Value = "1000";
                            if (xndObjects.Attributes.GetNamedItem("Chance") != null)
                                moItem.Chance = xndObjects.Attributes["Chance"].Value;
                            else
                                moItem.Chance = "1.0";
                            if (xndObjects.Attributes.GetNamedItem("MaxNumber") != null)
                                moItem.MaxNumber = xndObjects.Attributes["MaxNumber"].Value;
                            else
                                moItem.MaxNumber = "0";

                            chklstArtifacts.Items.Add(moItem, Convert.ToDouble(moItem.Chance, System.Globalization.CultureInfo.InvariantCulture) > 0.0);
                            //  cboObjectList.Items.Add(xndObjects.Attributes["Name"].Value);
                        }
                    }

                }
            }


        }


        /// <summary>
        /// ADDD GRADIENT COLOR EFFECT
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics formGraphics = e.Graphics;
            System.Drawing.Drawing2D.LinearGradientBrush gradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), Color.LightSlateGray, Color.Silver);
            formGraphics.FillRectangle(gradientBrush, ClientRectangle);


            //base.OnPaint(e);
        }

        /// <summary>
        /// updates all data from gui to template
        /// </summary>
        public void UpdateTemplate()
        {
            //used to read object data and update gui changes
            ObjectsReader obrdUpdater = new ObjectsReader();

            //Program.frmTemplateEdit.thTemplate.GetObjectsSet (

            //this updates all gui changes to all objects
            #region iterates through all chklists and update all changes

            for (int i = 1; i < chklstBattleObjects.Items.Count; i++)
            {
                TemplateMapObject tmoObject = (TemplateMapObject)chklstBattleObjects.Items[i];

                if (this.chklstBattleObjects.CheckedItems.Contains(tmoObject))
                    obrdUpdater.UpdateObject(tmoObject);
                else
                    obrdUpdater.UpdateObjectWithZeroChance(tmoObject);
            }

            for (int i = 1; i < chklstEnhancingObjects.Items.Count; i++)
            {
                TemplateMapObject tmoObject = (TemplateMapObject)chklstEnhancingObjects.Items[i];

                if (this.chklstEnhancingObjects.CheckedItems.Contains(tmoObject))
                    obrdUpdater.UpdateObject(tmoObject);
                else
                    obrdUpdater.UpdateObjectWithZeroChance(tmoObject);
            }

            for (int i = 1; i < chklstArtifacts.Items.Count; i++)
            {
                TemplateMapObject tmoObject = (TemplateMapObject)chklstArtifacts.Items[i];

                if (this.chklstArtifacts.CheckedItems.Contains(tmoObject))
                    obrdUpdater.UpdateObject(tmoObject);
                else
                    obrdUpdater.UpdateObjectWithZeroChance(tmoObject);
            }

            #endregion

            //update zones general appear chance
            Program.frmTemplateEdit.thTemplate.UpdateZoneAttribute(iZoneIndex , "ObjectsSet" + iObjectSet.ToString() , msktxtChance.Text  , "Appear_Chance");
            Program.frmTemplateEdit.thTemplate.UpdateZoneAttribute(iZoneIndex, "ObjectsSet" + iObjectSet.ToString(), msktxtDwellNumber.Text, "Dwelling_Number");
           
            
            //this will insert the new Objects node inside right zone and object set
            Program.frmTemplateEdit.thTemplate.UpdateObjectSetProperty(obrdUpdater.GetObjectsData(), iZoneIndex, iObjectSet);
        }


        /// <summary>
        /// UPDATE CHANGES AND DON'T DESTROY FORM FOR LATER OPENING
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmObjectsPick_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateTemplate();
            e.Cancel = true;
            this.Hide();
        }


        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //don't allow '0' since its invalide value    
            //if (e.KeyCode == Keys.D0)
            //{                
            //    e.SuppressKeyPress = true;
            //}
        }

        private void msktxtChance_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (msktxtChance.Text[0] != '0')
            {
                msktxtChance.Text = "0.";
            }
            else
            {
                //if (e.KeyCode == Keys.D0)
                //{
                 //   e.SuppressKeyPress = true;                    
               // }
            }

        }

        private void btnAddObject_Click(object sender, EventArgs e)
        {
            //if (!cboObjectList.SelectedItem.ToString().Contains("*"))
            //{
            //    lbxSelectedObjects.Items.Add(cboObjectList.SelectedItem);
            //}
        }

        //private void btnSelect_Click(object sender, EventArgs e)
        //{
        //    chklstBattleObjects.SetItemChecked(0, false);
        //    chklstArtifacts.SetItemChecked(0, false);
        //    chklstEnhancingObjects.SetItemChecked(0, false);
        //    SetAllInCheckedListBox(chklstBattleObjects, false);
        //    SetAllInCheckedListBox(chklstArtifacts, false);
        //    SetAllInCheckedListBox(chklstEnhancingObjects, false);
        //}

        private void chklstBattleObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            if (chklstBattleObjects.SelectedIndex != -1)
            {
                msktxtItemChance.Text = ((TemplateMapObject)chklstBattleObjects.SelectedItem).Chance;
                msktxtItemValue.Text = ((TemplateMapObject)chklstBattleObjects.SelectedItem).Value;
                msktxtItemMaxNumber.Text = ((TemplateMapObject)chklstBattleObjects.SelectedItem).MaxNumber;

              //  int iSelected = chklstBattleObjects.SelectedIndex;
                chklstArtifacts.SelectedIndex = -1;
                chklstEnhancingObjects.SelectedIndex = -1;
                UpdateStatistics();
              //  chklstBattleObjects.SelectedIndex = iSelected;
            }
            //chklstBattleObjects.Focus();

        }

        private void SetAllInCheckedListBox(CheckedListBox chklstTheList, bool bIsChecked)
        {
            for (int i = 1; i < chklstTheList.Items.Count; i++)
            {                
                chklstTheList.SetItemChecked(i, bIsChecked);
            }

        }

        private void chklstBattleObjects_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void chklstBattleObjects_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
            if (e.Index == 0 )
            {
                if (e.NewValue == CheckState.Checked)
                {
                    SetAllInCheckedListBox(chklstBattleObjects, true);
                }
                else
                {
                    SetAllInCheckedListBox(chklstBattleObjects, false);
                }
                UpdateStatistics();
            }
        }

        private void chklstEnhancingObjects_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index == 0)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    SetAllInCheckedListBox(chklstEnhancingObjects, true);
                }
                else
                {
                    SetAllInCheckedListBox(chklstEnhancingObjects, false);
                }
                UpdateStatistics();
            }
        }

        private void chklstArtifacts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //if its first place - select all
            if (e.Index == 0)
            {
                //do all its state
                if (e.NewValue == CheckState.Checked)
                {
                    SetAllInCheckedListBox(chklstArtifacts, true);
                }
                else
                {
                    SetAllInCheckedListBox(chklstArtifacts, false);
                }
                UpdateStatistics();
            }
        }

        private void msktxtChance_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void chklstEnhancingObjects_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (chklstEnhancingObjects.SelectedIndex != -1)
            {

                msktxtItemChance.Text = ((TemplateMapObject)chklstEnhancingObjects.SelectedItem).Chance;
                msktxtItemValue.Text = ((TemplateMapObject)chklstEnhancingObjects.SelectedItem).Value;
                msktxtItemMaxNumber.Text = ((TemplateMapObject)chklstEnhancingObjects.SelectedItem).MaxNumber;

               // int iSelected = chklstEnhancingObjects.SelectedIndex;

                chklstArtifacts.SelectedIndex = -1;
                chklstBattleObjects.SelectedIndex = -1;
                UpdateStatistics();

             //   chklstEnhancingObjects.SelectedIndex = iSelected;
            }
            //chklstArtifacts.
        }

        private void chklstArtifacts_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (chklstArtifacts.SelectedIndex != -1)
            {
                msktxtItemChance.Text = ((TemplateMapObject)chklstArtifacts.SelectedItem).Chance;
                msktxtItemValue.Text = ((TemplateMapObject)chklstArtifacts.SelectedItem).Value;
                msktxtItemMaxNumber.Text = ((TemplateMapObject)chklstArtifacts.SelectedItem).MaxNumber;

               // int iSelected = chklstArtifacts.SelectedIndex;

                chklstBattleObjects.SelectedIndex = -1;
                chklstEnhancingObjects.SelectedIndex = -1;
             //   chklstArtifacts.SelectedIndex = iSelected;
                //chklstArtifacts.Focus();
                UpdateStatistics();
            }
        }

        private void msktxtItemChance_KeyDown(object sender, KeyEventArgs e)
        {

            
        }

        private void msktxtItemChance_TextChanged(object sender, EventArgs e)
        {
            //if (msktxtItemChance.Text[0] > '1')
            //{
            //    char[] carr = msktxtItemChance.Text.ToCharArray();
            //    carr[0] = '1';
            //    msktxtItemChance.Text = Convert.ToString( carr );
            //}
        }

        private void msktxtItemChance_Validating(object sender, CancelEventArgs e)
        {
            //dwp чтобы не вылетало при некооректном вводе
            float fChance;
            try { fChance = float.Parse(msktxtItemChance.Text); }
            catch { fChance = 0; }
            if (fChance > 1.0)
            {
                toolTip1.Show("Chance can only be between 0.0 and 1.0", msktxtItemChance, 2000);
                msktxtItemChance.Text = "1.0";
            }
            TemplateMapObject moSelectedItem = GetSelectedItem();
            if (moSelectedItem != null)
            {
                if (ItemIsChecked(moSelectedItem))
                    moSelectedItem.Chance = msktxtItemChance.Text;
                else
                {
                    if (msktxtItemChance.Text != "0.0")
                    {
                        msktxtItemChance.Text = "0.0";
                        toolTip1.Show("When the selected item is not checked chance can only be 0.0", msktxtItemChance, 2000);
                    }
                }
                UpdateStatistics();
            }
        }


        /// <summary>
        /// checks is the a member of one of the lists is checked or not
        /// </summary>
        /// <param name="moSelectedItem"></param>
        /// <returns></returns>
        private bool ItemIsChecked(TemplateMapObject moSelectedItem)
        {

            if (chklstArtifacts.CheckedItems.IndexOf(moSelectedItem) >= 0)
                return true;
            if (chklstBattleObjects.CheckedItems.IndexOf(moSelectedItem) >= 0)
                return true;
            if (chklstEnhancingObjects.CheckedItems.IndexOf(moSelectedItem) >= 0)
                return true;
            return false;
        }

        /// <summary>
        /// gets the selected item out of the 3 checkedlistboxes
        /// </summary>
        private TemplateMapObject GetSelectedItem()
        {
            if (chklstArtifacts.SelectedIndex != -1)
                return (TemplateMapObject) chklstArtifacts.SelectedItem;
            if (chklstBattleObjects.SelectedIndex != -1)
                return (TemplateMapObject) chklstBattleObjects.SelectedItem;
            if (chklstEnhancingObjects.SelectedIndex != -1)
                return (TemplateMapObject) chklstEnhancingObjects.SelectedItem;
            return null;
        }


        /// <summary>
        /// sets the value of object value
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void msktxtItemValue_Validated(object sender, EventArgs e)
        {
            TemplateMapObject moSelectedItem = GetSelectedItem();
            if (moSelectedItem != null)
            {
                moSelectedItem.Value = msktxtItemValue.Text;
                UpdateStatistics();
            }
        }

        private void msktxtChance_Validating(object sender, CancelEventArgs e)
        {
            /* dwp!!! закоментировал потому что не понял этой лажи
            if (msktxtChance.Text[0] != '0')
            {
                e.Cancel = true;
            }
            else
            {*/

                UpdateStatistics();

            /*
            }
            */
        }

        private void msktxtItemValue_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void frmObjectsPick_VisibleChanged(object sender, EventArgs e)
        {

            chklstArtifacts.Items.Clear();
            chklstBattleObjects.Items.Clear();
            chklstEnhancingObjects.Items.Clear();
            xObjects = (XmlElement)Program.frmTemplateEdit.thTemplate.GetObjectsSet(iZoneIndex, iObjectSet).FirstChild;
            //ObjectsReader obRead = new ObjectsReader();
            //xObjects =  obRead.GetObjectsData();

            //obRead.FixToObjects();
            //obRead.AddNamesToObjects();
            //obRead.AddSizeObjects();

            chklstBattleObjects.Items.Add(new TemplateMapObject("Select All"));
            chklstEnhancingObjects.Items.Add(new TemplateMapObject("Select All"));
            chklstArtifacts.Items.Add(new TemplateMapObject("Select All"));

            //fill combo box with all objects (stored in objects.xml ) 
            Fillcbo(xObjects);

            msktxtChance.Text = xObjects.ParentNode.Attributes["Appear_Chance"].Value;
            try {
                msktxtDwellNumber.Text = xObjects.ParentNode.Attributes["Dwelling_Number"].Value;
            }
            catch (Exception) {
                msktxtDwellNumber.Text = "0-0";
            }

            UpdateStatistics();
        
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }


        /// <summary>
        /// finds objects appear chance statistics
        /// </summary>
        private void UpdateStatistics()
        {
            //go over all lists and count objects which are selected and their chance is greater then 0.0
            //also total the chance for those objects
            int iObjectCount = 0;
            double dTotalObjectChance = 0;


            for (int i = 0; i < chklstArtifacts.CheckedItems.Count; i++)
            {
                TemplateMapObject tmoItem = (TemplateMapObject)chklstArtifacts.CheckedItems[i];
                if ( tmoItem.Chance != "0.0" && tmoItem.Chance!=null)
                {
                    iObjectCount++;
                    dTotalObjectChance += double.Parse(tmoItem.Chance, System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            for (int i = 0; i < chklstBattleObjects.CheckedItems.Count; i++)
            {
                TemplateMapObject tmoItem = (TemplateMapObject)chklstBattleObjects.CheckedItems[i];
                if (tmoItem.Chance != "0.0" && tmoItem.Chance != null)
                {
                    iObjectCount++;
                    dTotalObjectChance += double.Parse(tmoItem.Chance, System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            for (int i = 0; i < chklstEnhancingObjects.CheckedItems.Count; i++)
            {
                TemplateMapObject tmoItem = (TemplateMapObject)chklstEnhancingObjects.CheckedItems[i];
                if (tmoItem.Chance != "0.0" && tmoItem.Chance != null)
                {
                    iObjectCount++;
                    dTotalObjectChance += double.Parse(tmoItem.Chance, System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            lblObjectsCount.Text = iObjectCount.ToString();

            double d1DevidedObjectCount = (double)1 / iObjectCount;

            lbl1DevidedObjectCount.Text = d1DevidedObjectCount.ToString("P", System.Globalization.CultureInfo.InvariantCulture);

            lbl1DevidedTimesSetChance.Text = (d1DevidedObjectCount * double.Parse(msktxtChance.Text, System.Globalization.CultureInfo.InvariantCulture)).ToString("P", System.Globalization.CultureInfo.InvariantCulture);



            TemplateMapObject tmoSelectedItem = GetSelectedItem();
            if (tmoSelectedItem != null)
            {
                if (tmoSelectedItem.Name != "Select All")
                {
                    double dObjectChace = double.Parse(tmoSelectedItem.Chance, System.Globalization.CultureInfo.InvariantCulture) / dTotalObjectChance;
                    lblObjectSetChance.Text = dObjectChace.ToString("P", System.Globalization.CultureInfo.InvariantCulture);
                    lblZoneChance.Text = (double.Parse(msktxtChance.Text, System.Globalization.CultureInfo.InvariantCulture) * dObjectChace).ToString("P", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    lblObjectSetChance.Text = "None Selected";
                    lblZoneChance.Text = "None Selected";
                }
            }
            else
            {
                lblObjectSetChance.Text = "None Selected";
                lblZoneChance.Text = "None Selected";
            }

        }

        private void btnSetChance1_Click(object sender, EventArgs e)
        {
            foreach (TemplateMapObject tmoItem in chklstBattleObjects.CheckedItems)
            {
                tmoItem.Chance = msktxtViewChance1.Text;
            }
        }

        private void btnSetChance2_Click(object sender, EventArgs e)
        {
            foreach (TemplateMapObject tmoItem in chklstEnhancingObjects.CheckedItems)
            {
                tmoItem.Chance = msktxtViewChance2.Text;
            }
        }

        private void btnSetChance3_Click(object sender, EventArgs e)
        {
            foreach (TemplateMapObject tmoItem in chklstArtifacts.CheckedItems)
            {
                tmoItem.Chance = msktxtViewChance3.Text;
            }
        }

        private void msktxtDwellNumber_KeyDown(object sender, KeyEventArgs e)
        {
            int min, max;
            try {
                min = Convert.ToInt32(msktxtDwellNumber.Text[0].ToString());
                max = Convert.ToInt32(msktxtDwellNumber.Text[2].ToString());
                min = Math.Min(min, 5);
                max = Math.Min(max, 5);
                max = Math.Max(min, max);
                msktxtDwellNumber.Text = min.ToString() + "-" + max.ToString();
            }
            catch (Exception) {
            }
        }

        private void msktxtDwellNumber_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void msktxtItemMaxNumber_Validating(object sender, CancelEventArgs e)
        {
            //dwp чтобы не вылетало при некооректном вводе
            int n ;
            try {
                n = int.Parse(msktxtItemMaxNumber.Text);
            }
            catch {
                msktxtItemMaxNumber.Text = "0";
                n = 0;
            }
            if (n < 0 || n > 99) {
                toolTip1.Show("Number can only be between 0 and 99", msktxtItemMaxNumber, 2000);
                msktxtItemChance.Text = "0";
            }
            TemplateMapObject moSelectedItem = GetSelectedItem();
            if (moSelectedItem != null) {
                if (ItemIsChecked(moSelectedItem))
                    moSelectedItem.MaxNumber = msktxtItemMaxNumber.Text;
                else {
                    if (msktxtItemMaxNumber.Text != "" || msktxtItemMaxNumber.Text != "0")
                    {
                        msktxtItemMaxNumber.Text = "0";
                        toolTip1.Show("When the selected item is not checked max number can only be 0", msktxtItemMaxNumber, 2000);
                    }
                }
            }
        }
    }
}