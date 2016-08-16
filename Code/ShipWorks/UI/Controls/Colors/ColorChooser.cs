using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared;

namespace ShipWorks.UI.Controls.Colors
{
   /// <summary>
   /// A color chooser dialog that can be used instead of the standard dialog
   /// </summary>
   public class ColorChooser : System.Windows.Forms.Form
   {
		#region Windows Form Designer generated code

      private System.Windows.Forms.Button cancel;
      private System.Windows.Forms.Button ok;
      private System.Windows.Forms.Label Label3;
      private System.Windows.Forms.NumericUpDown saturation;
      private System.Windows.Forms.Label Label7;
      private System.Windows.Forms.NumericUpDown brightness;
      private System.Windows.Forms.NumericUpDown red;
      private System.Windows.Forms.Panel panelColorWheel;
      private System.Windows.Forms.Label Label6;
      private System.Windows.Forms.Label Label1;
      private System.Windows.Forms.Label Label5;
      private System.Windows.Forms.Panel panelSelectedColor;
      private System.Windows.Forms.Panel panelBrightness;
      private System.Windows.Forms.NumericUpDown blue;
      private System.Windows.Forms.Label Label4;
      private System.Windows.Forms.NumericUpDown green;
      private System.Windows.Forms.Label Label2;
      private System.Windows.Forms.NumericUpDown hue;
      private System.Windows.Forms.Panel panelOriginalColor;
      private System.Windows.Forms.Label label8;
       private System.Windows.Forms.Panel panel1;
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.Container components = null;


      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose( bool disposing )
      {
         if( disposing )
         {
            if(components != null)
            {
               components.Dispose();
            }
         }
         base.Dispose( disposing );
      }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [NDependIgnoreLongMethod]
        private void InitializeComponent()
      {
          this.cancel = new System.Windows.Forms.Button();
          this.ok = new System.Windows.Forms.Button();
          this.Label3 = new System.Windows.Forms.Label();
          this.saturation = new System.Windows.Forms.NumericUpDown();
          this.Label7 = new System.Windows.Forms.Label();
          this.brightness = new System.Windows.Forms.NumericUpDown();
          this.red = new System.Windows.Forms.NumericUpDown();
          this.panelColorWheel = new System.Windows.Forms.Panel();
          this.Label6 = new System.Windows.Forms.Label();
          this.Label1 = new System.Windows.Forms.Label();
          this.Label5 = new System.Windows.Forms.Label();
          this.panelSelectedColor = new System.Windows.Forms.Panel();
          this.panelBrightness = new System.Windows.Forms.Panel();
          this.blue = new System.Windows.Forms.NumericUpDown();
          this.Label4 = new System.Windows.Forms.Label();
          this.green = new System.Windows.Forms.NumericUpDown();
          this.Label2 = new System.Windows.Forms.Label();
          this.hue = new System.Windows.Forms.NumericUpDown();
          this.panelOriginalColor = new System.Windows.Forms.Panel();
          this.label8 = new System.Windows.Forms.Label();
          this.panel1 = new System.Windows.Forms.Panel();
          ((System.ComponentModel.ISupportInitialize)(this.saturation)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.brightness)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.red)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.blue)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.green)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.hue)).BeginInit();
          this.panel1.SuspendLayout();
          this.SuspendLayout();
          // 
          // cancel
          // 
          this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
          this.cancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
          this.cancel.Location = new System.Drawing.Point(182, 340);
          this.cancel.Name = "cancel";
          this.cancel.TabIndex = 55;
          this.cancel.Text = "Cancel";
          // 
          // ok
          // 
          this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
          this.ok.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
          this.ok.Location = new System.Drawing.Point(100, 340);
          this.ok.Name = "ok";
          this.ok.TabIndex = 54;
          this.ok.Text = "OK";
          // 
          // Label3
          // 
          this.Label3.Location = new System.Drawing.Point(26, 308);
          this.Label3.Name = "Label3";
          this.Label3.Size = new System.Drawing.Size(38, 23);
          this.Label3.TabIndex = 45;
          this.Label3.Text = "Blue:";
          this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // saturation
          // 
          this.saturation.Location = new System.Drawing.Point(210, 284);
          this.saturation.Maximum = new System.Decimal(new int[] {
                                                                     255,
                                                                     0,
                                                                     0,
                                                                     0});
          this.saturation.Name = "saturation";
          this.saturation.Size = new System.Drawing.Size(48, 21);
          this.saturation.TabIndex = 42;
          this.saturation.TextChanged += new System.EventHandler(this.HandleTextChanged);
          this.saturation.ValueChanged += new System.EventHandler(this.HandleHSVChange);
          // 
          // Label7
          // 
          this.Label7.Location = new System.Drawing.Point(138, 308);
          this.Label7.Name = "Label7";
          this.Label7.Size = new System.Drawing.Size(68, 23);
          this.Label7.TabIndex = 50;
          this.Label7.Text = "Brightness:";
          this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // brightness
          // 
          this.brightness.Location = new System.Drawing.Point(210, 308);
          this.brightness.Maximum = new System.Decimal(new int[] {
                                                                     255,
                                                                     0,
                                                                     0,
                                                                     0});
          this.brightness.Name = "brightness";
          this.brightness.Size = new System.Drawing.Size(48, 21);
          this.brightness.TabIndex = 47;
          this.brightness.TextChanged += new System.EventHandler(this.HandleTextChanged);
          this.brightness.ValueChanged += new System.EventHandler(this.HandleHSVChange);
          // 
          // red
          // 
          this.red.Location = new System.Drawing.Point(68, 260);
          this.red.Maximum = new System.Decimal(new int[] {
                                                              255,
                                                              0,
                                                              0,
                                                              0});
          this.red.Name = "red";
          this.red.Size = new System.Drawing.Size(48, 21);
          this.red.TabIndex = 38;
          this.red.TextChanged += new System.EventHandler(this.HandleTextChanged);
          this.red.ValueChanged += new System.EventHandler(this.HandleRGBChange);
          // 
          // panelColorWheel
          // 
          this.panelColorWheel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
          this.panelColorWheel.Location = new System.Drawing.Point(8, 8);
          this.panelColorWheel.Name = "panelColorWheel";
          this.panelColorWheel.Size = new System.Drawing.Size(176, 176);
          this.panelColorWheel.TabIndex = 51;
          this.panelColorWheel.Visible = false;
          this.panelColorWheel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseUp);
          // 
          // Label6
          // 
          this.Label6.Location = new System.Drawing.Point(140, 284);
          this.Label6.Name = "Label6";
          this.Label6.Size = new System.Drawing.Size(64, 23);
          this.Label6.TabIndex = 49;
          this.Label6.Text = "Saturation:";
          this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // Label1
          // 
          this.Label1.Location = new System.Drawing.Point(28, 260);
          this.Label1.Name = "Label1";
          this.Label1.Size = new System.Drawing.Size(34, 23);
          this.Label1.TabIndex = 43;
          this.Label1.Text = "Red:";
          this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // Label5
          // 
          this.Label5.Location = new System.Drawing.Point(172, 260);
          this.Label5.Name = "Label5";
          this.Label5.Size = new System.Drawing.Size(30, 23);
          this.Label5.TabIndex = 48;
          this.Label5.Text = "Hue:";
          this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // panelSelectedColor
          // 
          this.panelSelectedColor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
          this.panelSelectedColor.Location = new System.Drawing.Point(-2, -2);
          this.panelSelectedColor.Name = "panelSelectedColor";
          this.panelSelectedColor.Size = new System.Drawing.Size(76, 20);
          this.panelSelectedColor.TabIndex = 53;
          // 
          // panelBrightness
          // 
          this.panelBrightness.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
          this.panelBrightness.Location = new System.Drawing.Point(208, 8);
          this.panelBrightness.Name = "panelBrightness";
          this.panelBrightness.Size = new System.Drawing.Size(16, 176);
          this.panelBrightness.TabIndex = 52;
          this.panelBrightness.Visible = false;
          // 
          // blue
          // 
          this.blue.Location = new System.Drawing.Point(68, 308);
          this.blue.Maximum = new System.Decimal(new int[] {
                                                               255,
                                                               0,
                                                               0,
                                                               0});
          this.blue.Name = "blue";
          this.blue.Size = new System.Drawing.Size(48, 21);
          this.blue.TabIndex = 40;
          this.blue.TextChanged += new System.EventHandler(this.HandleTextChanged);
          this.blue.ValueChanged += new System.EventHandler(this.HandleRGBChange);
          // 
          // Label4
          // 
          this.Label4.Location = new System.Drawing.Point(142, 198);
          this.Label4.Name = "Label4";
          this.Label4.Size = new System.Drawing.Size(48, 24);
          this.Label4.TabIndex = 46;
          this.Label4.Text = "New:";
          this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // green
          // 
          this.green.Location = new System.Drawing.Point(68, 284);
          this.green.Maximum = new System.Decimal(new int[] {
                                                                255,
                                                                0,
                                                                0,
                                                                0});
          this.green.Name = "green";
          this.green.Size = new System.Drawing.Size(48, 21);
          this.green.TabIndex = 39;
          this.green.TextChanged += new System.EventHandler(this.HandleTextChanged);
          this.green.ValueChanged += new System.EventHandler(this.HandleRGBChange);
          // 
          // Label2
          // 
          this.Label2.Location = new System.Drawing.Point(18, 284);
          this.Label2.Name = "Label2";
          this.Label2.Size = new System.Drawing.Size(44, 23);
          this.Label2.TabIndex = 44;
          this.Label2.Text = "Green:";
          this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // hue
          // 
          this.hue.Location = new System.Drawing.Point(210, 260);
          this.hue.Maximum = new System.Decimal(new int[] {
                                                              255,
                                                              0,
                                                              0,
                                                              0});
          this.hue.Name = "hue";
          this.hue.Size = new System.Drawing.Size(48, 21);
          this.hue.TabIndex = 41;
          this.hue.TextChanged += new System.EventHandler(this.HandleTextChanged);
          this.hue.ValueChanged += new System.EventHandler(this.HandleHSVChange);
          // 
          // panelOriginalColor
          // 
          this.panelOriginalColor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
          this.panelOriginalColor.Location = new System.Drawing.Point(-2, 18);
          this.panelOriginalColor.Name = "panelOriginalColor";
          this.panelOriginalColor.Size = new System.Drawing.Size(76, 20);
          this.panelOriginalColor.TabIndex = 56;
          // 
          // label8
          // 
          this.label8.Location = new System.Drawing.Point(124, 218);
          this.label8.Name = "label8";
          this.label8.Size = new System.Drawing.Size(48, 24);
          this.label8.TabIndex = 57;
          this.label8.Text = "Original:";
          this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // panel1
          // 
          this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
          this.panel1.Controls.Add(this.panelSelectedColor);
          this.panel1.Controls.Add(this.panelOriginalColor);
          this.panel1.Location = new System.Drawing.Point(180, 200);
          this.panel1.Name = "panel1";
          this.panel1.Size = new System.Drawing.Size(76, 40);
          this.panel1.TabIndex = 58;
          // 
          // ColorChooser
          // 
          this.AcceptButton = this.ok;
          this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
          this.CancelButton = this.cancel;
          this.ClientSize = new System.Drawing.Size(266, 374);
          this.Controls.Add(this.panel1);
          this.Controls.Add(this.label8);
          this.Controls.Add(this.cancel);
          this.Controls.Add(this.ok);
          this.Controls.Add(this.Label3);
          this.Controls.Add(this.saturation);
          this.Controls.Add(this.Label7);
          this.Controls.Add(this.brightness);
          this.Controls.Add(this.red);
          this.Controls.Add(this.panelColorWheel);
          this.Controls.Add(this.Label6);
          this.Controls.Add(this.Label1);
          this.Controls.Add(this.Label5);
          this.Controls.Add(this.panelBrightness);
          this.Controls.Add(this.blue);
          this.Controls.Add(this.Label4);
          this.Controls.Add(this.green);
          this.Controls.Add(this.Label2);
          this.Controls.Add(this.hue);
          this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "ColorChooser";
          this.ShowInTaskbar = false;
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
          this.Text = "Select Color";
          this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouse);
          this.Load += new System.EventHandler(this.OnLoad);
          this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseUp);
          this.Paint += new System.Windows.Forms.PaintEventHandler(this.ColorChooser1_Paint);
          this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HandleMouse);
          ((System.ComponentModel.ISupportInitialize)(this.saturation)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.brightness)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.red)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.blue)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.green)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.hue)).EndInit();
          this.panel1.ResumeLayout(false);
          this.ResumeLayout(false);

      }
		#endregion

      enum ChangeStyle
      {
         MouseMove,
         RGB,
         HSV,
         None
      }

      ChangeStyle changeType = ChangeStyle.None;
      Point selectedPoint;

      ColorWheel myColorWheel;
      ColorHandler.RGB rgbHandler;
      ColorHandler.HSV HSV;

      bool isInUpdate = false;

      /// <summary>
      /// Constructor
      /// </summary>
      public ColorChooser(Color originalColor)
      {
         InitializeComponent();

         panelOriginalColor.BackColor = originalColor;
         this.Color = originalColor;
      }

      /// <summary>
      /// Initialization
      /// </summary>
      private void OnLoad(object sender, System.EventArgs e)
      {
         // Turn on double-buffering, so the form looks better. 
         this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
         this.SetStyle(ControlStyles.UserPaint, true);
         this.SetStyle(ControlStyles.DoubleBuffer, true);

         // These properties are set in design view, as well, but they
         // have to be set to false in order for the Paint
         // event to be able to display their contents.
         // Never hurts to make sure they're invisible.
         panelBrightness.Visible = false;
         panelColorWheel.Visible = false;

         // Calculate the coordinates of the three
         // required regions on the form.
         Rectangle SelectedColorRectangle =  
            new Rectangle(panelSelectedColor.Location, panelSelectedColor.Size);
         Rectangle BrightnessRectangle = 
            new Rectangle(panelBrightness.Location, panelBrightness.Size);
         Rectangle ColorRectangle = 
            new Rectangle(panelColorWheel.Location, panelColorWheel.Size);

         // Create the new ColorWheel class, indicating
         // the locations of the color wheel itself, the
         // brightness area, and the position of the selected color.
         myColorWheel = new ColorWheel(
            ColorRectangle, BrightnessRectangle, 
            SelectedColorRectangle);
         myColorWheel.ColorChanged += 
            new ColorWheel.ColorChangedEventHandler(
            this.myColorWheel_ColorChanged);

         // Set the RGB and HSV values 
         // of the NumericUpDown controls.
         SetRGB(rgbHandler);
         SetHSV(HSV);		
      }

      private void HandleMouse(object sender,  MouseEventArgs e)
      {
         // If you have the left mouse button down, 
         // then update the selectedPoint value and 
         // force a repaint of the color wheel.
         if ( e.Button == MouseButtons.Left ) 
         {
            changeType = ChangeStyle.MouseMove;
            selectedPoint = new Point(e.X, e.Y);
            this.Invalidate();
         }
      }

      private void frmMain_MouseUp( object sender,  MouseEventArgs e)
      {
         myColorWheel.SetMouseUp();
         changeType = ChangeStyle.None;
      }

      private void HandleRGBChange(object sender,  System.EventArgs e)
      {

         // If the R, G, or B values change, use this 
         // code to update the HSV values and invalidate
         // the color wheel (so it updates the pointers).
         // Check the isInUpdate flag to avoid recursive events
         // when you update the NumericUpdownControls.
         if (!isInUpdate ) 
         {
            changeType = ChangeStyle.RGB;
            rgbHandler = new ColorHandler.RGB((int)red.Value, (int)green.Value, (int)blue.Value);
            SetHSV(ColorHandler.RGBtoHSV(rgbHandler));
            this.Invalidate();
         }
      }

      private void HandleHSVChange(  object sender,  EventArgs e)  
      {
         // If the H, S, or V values change, use this 
         // code to update the RGB values and invalidate
         // the color wheel (so it updates the pointers).
         // Check the isInUpdate flag to avoid recursive events
         // when you update the NumericUpdownControls.
         if (! isInUpdate ) 
         {
            changeType = ChangeStyle.HSV;
            HSV = new ColorHandler.HSV((int)(hue.Value), (int)(saturation.Value), (int)(brightness.Value));
            SetRGB(ColorHandler.HSVtoRGB(HSV));
            this.Invalidate();
         }
      }

      private void SetRGB( ColorHandler.RGB rgbHandler) 
      {
         // Update the RGB values on the form, but don't trigger
         // the ValueChanged event of the form. The isInUpdate
         // variable ensures that the event procedures
         // exit without doing anything.
         isInUpdate = true;
         RefreshValue(red, rgbHandler.Red);
         RefreshValue(blue, rgbHandler.Blue);
         RefreshValue(green, rgbHandler.Green);
         panelSelectedColor.BackColor = Color.FromArgb(rgbHandler.Red, rgbHandler.Green, rgbHandler.Blue);
         isInUpdate = false;
      }

      private void SetHSV(ColorHandler.HSV HSV) 
      {
         // Update the HSV values on the form, but don't trigger
         // the ValueChanged event of the form. The isInUpdate
         // variable ensures that the event procedures
         // exit without doing anything.
         isInUpdate = true;
         RefreshValue(hue, HSV.Hue);
         RefreshValue(saturation, HSV.Saturation);
         RefreshValue(brightness, HSV.value);
         panelSelectedColor.BackColor =  ColorHandler.HSVtoColor(HSV);
         isInUpdate = false;
      }

      private void HandleTextChanged(object sender, System.EventArgs e)
      {
         // This step works around a bug -- unless you actively
         // retrieve the value, the min and max settings for the 
         // control aren't honored when you type text. This may
         // be fixed in the 1.1 version, but in VS.NET 1.0, this 
         // step is required.
         Decimal x = ((NumericUpDown)sender).Value;
      }

      private void RefreshValue(  NumericUpDown nud,  int value) 
      {
         // Update the value of the NumericUpDown control, 
         // if the value is different than the current value.
         // Refresh the control, causing an immediate repaint.
         if ( nud.Value != value ) 
         {
            nud.Value = value;
            nud.Refresh();
         }
      }

      public Color Color  
      {
         // Get or set the color to be
         // displayed in the color wheel.
         get 
         {
            return myColorWheel.Color;
         }

         set 
         {
            // Indicate the color change type. Either RGB or HSV
            // will cause the color wheel to update the position
            // of the pointer.
            changeType = ChangeStyle.RGB;
            rgbHandler = new ColorHandler.RGB(value.R, value.G, value.B);
            HSV = ColorHandler.RGBtoHSV(rgbHandler);
         }
      }

      private void myColorWheel_ColorChanged(object sender,  ColorChangedEventArgs e)  
      {
         SetRGB(e.RGB);
         SetHSV(e.HSV);
      }

      private void ColorChooser1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
      {
         // Depending on the circumstances, force a repaint
         // of the color wheel passing different information.
         switch (changeType)
         {
            case ChangeStyle.HSV:
               myColorWheel.Draw(e.Graphics, HSV);
               break;
            case ChangeStyle.MouseMove:
            case ChangeStyle.None:
               myColorWheel.Draw(e.Graphics, selectedPoint);
               break;
            case ChangeStyle.RGB:
               myColorWheel.Draw(e.Graphics, rgbHandler);
               break;
         }
      }
   }
}
