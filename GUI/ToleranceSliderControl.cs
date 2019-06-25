/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// ***this code was borrowed from open source program , Not Written by idan    //
// hollander (only minor tweaks ) *********************************************//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PaintDotNet
{
    public class ToleranceSliderControl 
        : Control
    {
        private bool tracking = false;
        private bool hovering = false;
        private bool isValid;
        private string toleranceText;
        private string percentageFormat;
        private int iMaxValue = 100;
        private int iMinValue = 0;
        public bool bExceedMaximum;
        public bool bIgnoreMaximum = false;
        public string strDisplayedPrecentage;

        private float tolerance;
    
        public float Tolerance
        {
            get 
            {
                return tolerance;
            }
            set 
            {
                if (tolerance != value) 
                {
                    if ( bIgnoreMaximum )
                        tolerance = value;
                    else
                        tolerance = Clamp(value, 0, 1);
                    
                    OnToleranceChanged();
                }
            }
        }

        public float Clamp(float x, float min, float max)
        {
            if (x < min)
            {
                return min;
            }
            else if (x > max)
            {
                return max;
            }
            else
            {
                return x;
            }
        }

        public EventHandler ToleranceChanged;
        protected void OnToleranceChanged() 
        {
            this.isValid = false;
            this.Invalidate();
            this.Update();
            if (ToleranceChanged != null) 
            {
                ToleranceChanged(this, EventArgs.Empty);
            }
        }

        public void PerformToleranceChanged() 
        {
            OnToleranceChanged();
        }

        protected Bitmap buffer = null;
        protected Graphics bufferGraphics = null;

        protected void UpdateBitmap() 
        {
            this.Invalidate();

            if (buffer == null || buffer.Width != this.ClientSize.Width || buffer.Height != this.ClientSize.Height) 
            {
                if (buffer != null)
                {
                    buffer.Dispose();
                    buffer = null;
                }
                
                buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

                if (bufferGraphics != null) 
                {
                    bufferGraphics.Dispose();
                    bufferGraphics = null;
                }

                bufferGraphics = Graphics.FromImage(buffer);
            }

            bufferGraphics.Clear(this.BackColor);

            using (LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, Color.Black, Color.White, 0, false))
            {
                bufferGraphics.FillRectangle(lgb, 0, 0, ClientSize.Width, ClientSize.Height);
            }

            if ( tolerance < 1.0f )
                bufferGraphics.FillRectangle(Brushes.DarkBlue, 0.0f, 0.0f, ClientRectangle.Width * tolerance, this.ClientRectangle.Height);
            else
                if ( tolerance < 1.01f && bIgnoreMaximum )
                    bufferGraphics.FillRectangle(Brushes.Green, 0.0f, 0.0f, ClientRectangle.Width * tolerance, this.ClientRectangle.Height);
                else
                    bufferGraphics.FillRectangle(Brushes.Red, 0.0f, 0.0f, ClientRectangle.Width * tolerance, this.ClientRectangle.Height);
            bufferGraphics.DrawRectangle(hovering ? Pens.White : Pens.Black, 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
            bufferGraphics.SmoothingMode = SmoothingMode.HighQuality;
            bufferGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            using (Font ourFont = new Font(this.Font.FontFamily, 8.0f, this.Font.Style))
            {
                Brush textBrush;

                if (hovering)
                {
                    textBrush = Brushes.White;
                }
                else
                {
                    textBrush = Brushes.White;
                }

                if (tolerance < 1.0f)
                {
                    int number = Convert.ToInt32(percentageFormat == "" ? iMinValue + tolerance * (iMaxValue - iMinValue) : tolerance * iMaxValue + iMinValue);
                    string text = string.Format("{0}" + percentageFormat, number);
                    strDisplayedPrecentage = text;
                    bufferGraphics.DrawString(text, ourFont, textBrush, 2, 1);
                }
                else
                {
                    if (tolerance < 1.01f && bIgnoreMaximum)
                    {
                        int number = Convert.ToInt32(tolerance * iMaxValue + iMinValue);
                        string text = string.Format("{0}% OK", number);
                        strDisplayedPrecentage = text;
                        bufferGraphics.DrawString(text, ourFont, textBrush, 2, 1);
                    }
                    else
                    {
                        int number = Convert.ToInt32(percentageFormat == "" ? iMinValue + tolerance * (iMaxValue - iMinValue) : tolerance * iMaxValue + iMinValue);
                        string text;
                        if (bIgnoreMaximum)
                        {
                            text = string.Format("{0}% Exceeded", number);
                        }
                        else
                        {
                            text = string.Format("{0}" + percentageFormat, number);
                        }
                        strDisplayedPrecentage = text;
                        bufferGraphics.DrawString(text, ourFont, textBrush, 2, 1);
                    }
                }
            }

            this.isValid = true;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!isValid) 
            {
                UpdateBitmap();
            }

            if (buffer != null)
            {
                Rectangle bounds = new Rectangle(0, 0, buffer.Width, buffer.Height);
                e.Graphics.DrawImage(buffer, bounds, bounds, GraphicsUnit.Pixel);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!tracking && (e.Button & MouseButtons.Left) == MouseButtons.Left) 
            {
                tracking = true;
                isValid = false;
                this.Invalidate();
                this.Update();
                OnMouseMove(e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove (e);

            if (tracking) 
            {
                Tolerance = (float)e.X / this.ClientSize.Width;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (tracking && (e.Button & MouseButtons.Left) == MouseButtons.Left) 
            {
                tracking = false;
                isValid = false;
                this.Invalidate();
                this.Update();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.hovering = true;
            this.UpdateBitmap();
            this.Update();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.hovering = false;
            this.UpdateBitmap();
            this.Update();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize (e);

            if (bufferGraphics != null) 
            {
                bufferGraphics.Dispose();
                bufferGraphics = null;
            }

            if (buffer != null) 
            {
                buffer.Dispose();
                buffer = null;
            }
        }

        public ToleranceSliderControl()
        {
            InitializeComponent();
            this.tolerance = 0.5f;
            percentageFormat = "%";
            //this.toleranceText = PdnResources.GetString("ToleranceSliderControl.Tolerance");
            //this.percentageFormat = PdnResources.GetString("ToleranceSliderControl.Percentage.Format");
        }

        public ToleranceSliderControl(int iInputMinValue , int iInputMaxValue )
        {
            InitializeComponent();
            this.tolerance = 0.5f;
            iMinValue = iInputMinValue;
            iMaxValue = iInputMaxValue;
            percentageFormat = "%";
        }

        public ToleranceSliderControl(int iInputMinValue, int iInputMaxValue, string iPercentageFormat)
        {
            InitializeComponent();
            this.tolerance = 0.5f;
            iMinValue = iInputMinValue;
            iMaxValue = iInputMaxValue;
            percentageFormat = iPercentageFormat;
            bIgnoreMaximum = false;
        }


        public ToleranceSliderControl(int iInputMinValue, int iInputMaxValue, bool bInputIgnoreMaximum)
        {
            InitializeComponent();
            this.tolerance = 0.5f;
            iMinValue = iInputMinValue;
            iMaxValue = iInputMaxValue;
            bIgnoreMaximum = bInputIgnoreMaximum;
            
            //this.toleranceText = PdnResources.GetString("ToleranceSliderControl.Tolerance");
            //this.percentageFormat = PdnResources.GetString("ToleranceSliderControl.Percentage.Format");
        }
       

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (bufferGraphics != null) 
                {
                    bufferGraphics.Dispose();
                    bufferGraphics = null;
                }

                if (buffer != null) 
                {
                    buffer.Dispose();
                    buffer = null;
                }
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Name = "ToleranceSliderControl";
        }
    }
}