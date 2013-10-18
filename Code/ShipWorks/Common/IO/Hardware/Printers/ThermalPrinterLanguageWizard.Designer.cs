namespace ShipWorks.Common.IO.Hardware.Printers
{
    partial class ThermalPrinterLanguageWizard
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
            this.wizardPageStep1 = new ShipWorks.UI.Wizard.WizardPage();
            this.panelTest1Result = new System.Windows.Forms.Panel();
            this.labelTest1Result = new System.Windows.Forms.Label();
            this.labelQuestionTest1Result = new System.Windows.Forms.Label();
            this.radioTest1No = new System.Windows.Forms.RadioButton();
            this.radioTest1Yes = new System.Windows.Forms.RadioButton();
            this.printTest1 = new System.Windows.Forms.Button();
            this.labelTest1 = new System.Windows.Forms.Label();
            this.labelPrinter = new System.Windows.Forms.Label();
            this.picturePrinter = new System.Windows.Forms.PictureBox();
            this.printerName = new System.Windows.Forms.Label();
            this.labelPrinterTests2 = new System.Windows.Forms.Label();
            this.wizardPageStep2 = new ShipWorks.UI.Wizard.WizardPage();
            this.label4 = new System.Windows.Forms.Label();
            this.panelTest2Result = new System.Windows.Forms.Panel();
            this.labelTest2Result = new System.Windows.Forms.Label();
            this.labelTest2Question = new System.Windows.Forms.Label();
            this.radioTest2No = new System.Windows.Forms.RadioButton();
            this.radioTest2Yes = new System.Windows.Forms.RadioButton();
            this.printTest2 = new System.Windows.Forms.Button();
            this.labelTest2 = new System.Windows.Forms.Label();
            this.wizardPageStep3 = new ShipWorks.UI.Wizard.WizardPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panelTest3Result = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTest3Question = new System.Windows.Forms.Label();
            this.radioTest3No = new System.Windows.Forms.RadioButton();
            this.radioTest3Yes = new System.Windows.Forms.RadioButton();
            this.printTest3 = new System.Windows.Forms.Button();
            this.labelTest3 = new System.Windows.Forms.Label();
            this.wizardPageFinish = new ShipWorks.UI.Wizard.WizardPage();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.labelPrinterType = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageStep1.SuspendLayout();
            this.panelTest1Result.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePrinter)).BeginInit();
            this.wizardPageStep2.SuspendLayout();
            this.panelTest2Result.SuspendLayout();
            this.wizardPageStep3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panelTest3Result.SuspendLayout();
            this.wizardPageFinish.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(277, 318);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(358, 318);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(196, 318);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageStep3);
            this.mainPanel.Size = new System.Drawing.Size(445, 246);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 308);
            this.etchBottom.Size = new System.Drawing.Size(449, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.printer21;
            this.pictureBox.Location = new System.Drawing.Point(390, 3);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(445, 56);
            // 
            // wizardPageStep1
            // 
            this.wizardPageStep1.Controls.Add(this.panelTest1Result);
            this.wizardPageStep1.Controls.Add(this.printTest1);
            this.wizardPageStep1.Controls.Add(this.labelTest1);
            this.wizardPageStep1.Controls.Add(this.labelPrinter);
            this.wizardPageStep1.Controls.Add(this.picturePrinter);
            this.wizardPageStep1.Controls.Add(this.printerName);
            this.wizardPageStep1.Controls.Add(this.labelPrinterTests2);
            this.wizardPageStep1.Description = "ShipWorks will help you determine the language of your printer.";
            this.wizardPageStep1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageStep1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageStep1.Location = new System.Drawing.Point(0, 0);
            this.wizardPageStep1.Name = "wizardPageStep1";
            this.wizardPageStep1.Size = new System.Drawing.Size(445, 246);
            this.wizardPageStep1.TabIndex = 0;
            this.wizardPageStep1.Title = "Thermal Language";
            this.wizardPageStep1.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextTest);
            // 
            // panelTest1Result
            // 
            this.panelTest1Result.Controls.Add(this.labelTest1Result);
            this.panelTest1Result.Controls.Add(this.labelQuestionTest1Result);
            this.panelTest1Result.Controls.Add(this.radioTest1No);
            this.panelTest1Result.Controls.Add(this.radioTest1Yes);
            this.panelTest1Result.Location = new System.Drawing.Point(18, 142);
            this.panelTest1Result.Name = "panelTest1Result";
            this.panelTest1Result.Size = new System.Drawing.Size(397, 76);
            this.panelTest1Result.TabIndex = 56;
            this.panelTest1Result.Visible = false;
            // 
            // labelTest1Result
            // 
            this.labelTest1Result.AutoSize = true;
            this.labelTest1Result.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTest1Result.Location = new System.Drawing.Point(3, 6);
            this.labelTest1Result.Name = "labelTest1Result";
            this.labelTest1Result.Size = new System.Drawing.Size(43, 13);
            this.labelTest1Result.TabIndex = 55;
            this.labelTest1Result.Text = "Result";
            // 
            // labelQuestionTest1Result
            // 
            this.labelQuestionTest1Result.AutoSize = true;
            this.labelQuestionTest1Result.Location = new System.Drawing.Point(17, 26);
            this.labelQuestionTest1Result.Name = "labelQuestionTest1Result";
            this.labelQuestionTest1Result.Size = new System.Drawing.Size(348, 13);
            this.labelQuestionTest1Result.TabIndex = 49;
            this.labelQuestionTest1Result.Text = "Did a label print that says \"ShipWorks Test 1: Welcome to ShipWorks!\"?";
            // 
            // radioTest1No
            // 
            this.radioTest1No.AutoSize = true;
            this.radioTest1No.Location = new System.Drawing.Point(82, 45);
            this.radioTest1No.Name = "radioTest1No";
            this.radioTest1No.Size = new System.Drawing.Size(38, 17);
            this.radioTest1No.TabIndex = 54;
            this.radioTest1No.TabStop = true;
            this.radioTest1No.Text = "No";
            this.radioTest1No.UseVisualStyleBackColor = true;
            // 
            // radioTest1Yes
            // 
            this.radioTest1Yes.AutoSize = true;
            this.radioTest1Yes.Location = new System.Drawing.Point(34, 45);
            this.radioTest1Yes.Name = "radioTest1Yes";
            this.radioTest1Yes.Size = new System.Drawing.Size(42, 17);
            this.radioTest1Yes.TabIndex = 53;
            this.radioTest1Yes.TabStop = true;
            this.radioTest1Yes.Text = "Yes";
            this.radioTest1Yes.UseVisualStyleBackColor = true;
            // 
            // printTest1
            // 
            this.printTest1.Location = new System.Drawing.Point(38, 113);
            this.printTest1.Name = "printTest1";
            this.printTest1.Size = new System.Drawing.Size(75, 23);
            this.printTest1.TabIndex = 52;
            this.printTest1.Text = "Print Test 1";
            this.printTest1.UseVisualStyleBackColor = true;
            this.printTest1.Click += new System.EventHandler(this.OnPrintTest);
            // 
            // labelTest1
            // 
            this.labelTest1.AutoSize = true;
            this.labelTest1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTest1.Location = new System.Drawing.Point(21, 93);
            this.labelTest1.Name = "labelTest1";
            this.labelTest1.Size = new System.Drawing.Size(42, 13);
            this.labelTest1.TabIndex = 48;
            this.labelTest1.Text = "Test 1";
            // 
            // labelPrinter
            // 
            this.labelPrinter.AutoSize = true;
            this.labelPrinter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPrinter.Location = new System.Drawing.Point(20, 9);
            this.labelPrinter.Name = "labelPrinter";
            this.labelPrinter.Size = new System.Drawing.Size(76, 13);
            this.labelPrinter.TabIndex = 46;
            this.labelPrinter.Text = "Instructions";
            // 
            // picturePrinter
            // 
            this.picturePrinter.Image = global::ShipWorks.Properties.Resources.printer2;
            this.picturePrinter.Location = new System.Drawing.Point(37, 29);
            this.picturePrinter.Name = "picturePrinter";
            this.picturePrinter.Size = new System.Drawing.Size(16, 16);
            this.picturePrinter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picturePrinter.TabIndex = 45;
            this.picturePrinter.TabStop = false;
            // 
            // printerName
            // 
            this.printerName.AutoSize = true;
            this.printerName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printerName.Location = new System.Drawing.Point(55, 30);
            this.printerName.Name = "printerName";
            this.printerName.Size = new System.Drawing.Size(107, 13);
            this.printerName.TabIndex = 44;
            this.printerName.Text = "Zebra Whatever XYZ";
            // 
            // labelPrinterTests2
            // 
            this.labelPrinterTests2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPrinterTests2.Location = new System.Drawing.Point(34, 56);
            this.labelPrinterTests2.Name = "labelPrinterTests2";
            this.labelPrinterTests2.Size = new System.Drawing.Size(337, 29);
            this.labelPrinterTests2.TabIndex = 43;
            this.labelPrinterTests2.Text = "ShipWorks will print a series of tests to your printer, and ask you to verify wha" +
    "t prints out.";
            // 
            // wizardPageStep2
            // 
            this.wizardPageStep2.Controls.Add(this.label4);
            this.wizardPageStep2.Controls.Add(this.panelTest2Result);
            this.wizardPageStep2.Controls.Add(this.printTest2);
            this.wizardPageStep2.Controls.Add(this.labelTest2);
            this.wizardPageStep2.Description = "ShipWorks will help you determine the language of your printer.";
            this.wizardPageStep2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageStep2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageStep2.Location = new System.Drawing.Point(0, 0);
            this.wizardPageStep2.Name = "wizardPageStep2";
            this.wizardPageStep2.Size = new System.Drawing.Size(445, 246);
            this.wizardPageStep2.TabIndex = 0;
            this.wizardPageStep2.Title = "Thermal Language";
            this.wizardPageStep2.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextTest);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 13);
            this.label4.TabIndex = 60;
            this.label4.Text = "Good job!  Let\'s try another one.";
            // 
            // panelTest2Result
            // 
            this.panelTest2Result.Controls.Add(this.labelTest2Result);
            this.panelTest2Result.Controls.Add(this.labelTest2Question);
            this.panelTest2Result.Controls.Add(this.radioTest2No);
            this.panelTest2Result.Controls.Add(this.radioTest2Yes);
            this.panelTest2Result.Location = new System.Drawing.Point(17, 84);
            this.panelTest2Result.Name = "panelTest2Result";
            this.panelTest2Result.Size = new System.Drawing.Size(380, 76);
            this.panelTest2Result.TabIndex = 59;
            this.panelTest2Result.Visible = false;
            // 
            // labelTest2Result
            // 
            this.labelTest2Result.AutoSize = true;
            this.labelTest2Result.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTest2Result.Location = new System.Drawing.Point(3, 6);
            this.labelTest2Result.Name = "labelTest2Result";
            this.labelTest2Result.Size = new System.Drawing.Size(43, 13);
            this.labelTest2Result.TabIndex = 55;
            this.labelTest2Result.Text = "Result";
            // 
            // labelTest2Question
            // 
            this.labelTest2Question.AutoSize = true;
            this.labelTest2Question.Location = new System.Drawing.Point(17, 26);
            this.labelTest2Question.Name = "labelTest2Question";
            this.labelTest2Question.Size = new System.Drawing.Size(348, 13);
            this.labelTest2Question.TabIndex = 49;
            this.labelTest2Question.Text = "Did a label print that says \"ShipWorks Test 2: We\'re glad you\'re here!\"?";
            // 
            // radioTest2No
            // 
            this.radioTest2No.AutoSize = true;
            this.radioTest2No.Location = new System.Drawing.Point(82, 45);
            this.radioTest2No.Name = "radioTest2No";
            this.radioTest2No.Size = new System.Drawing.Size(38, 17);
            this.radioTest2No.TabIndex = 54;
            this.radioTest2No.TabStop = true;
            this.radioTest2No.Text = "No";
            this.radioTest2No.UseVisualStyleBackColor = true;
            // 
            // radioTest2Yes
            // 
            this.radioTest2Yes.AutoSize = true;
            this.radioTest2Yes.Location = new System.Drawing.Point(34, 45);
            this.radioTest2Yes.Name = "radioTest2Yes";
            this.radioTest2Yes.Size = new System.Drawing.Size(42, 17);
            this.radioTest2Yes.TabIndex = 53;
            this.radioTest2Yes.TabStop = true;
            this.radioTest2Yes.Text = "Yes";
            this.radioTest2Yes.UseVisualStyleBackColor = true;
            // 
            // printTest2
            // 
            this.printTest2.Location = new System.Drawing.Point(37, 52);
            this.printTest2.Name = "printTest2";
            this.printTest2.Size = new System.Drawing.Size(75, 23);
            this.printTest2.TabIndex = 58;
            this.printTest2.Text = "Print Test 2";
            this.printTest2.UseVisualStyleBackColor = true;
            this.printTest2.Click += new System.EventHandler(this.OnPrintTest);
            // 
            // labelTest2
            // 
            this.labelTest2.AutoSize = true;
            this.labelTest2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTest2.Location = new System.Drawing.Point(20, 32);
            this.labelTest2.Name = "labelTest2";
            this.labelTest2.Size = new System.Drawing.Size(42, 13);
            this.labelTest2.TabIndex = 57;
            this.labelTest2.Text = "Test 2";
            // 
            // wizardPageStep3
            // 
            this.wizardPageStep3.Controls.Add(this.label5);
            this.wizardPageStep3.Controls.Add(this.label3);
            this.wizardPageStep3.Controls.Add(this.label2);
            this.wizardPageStep3.Controls.Add(this.pictureBox2);
            this.wizardPageStep3.Controls.Add(this.panelTest3Result);
            this.wizardPageStep3.Controls.Add(this.printTest3);
            this.wizardPageStep3.Controls.Add(this.labelTest3);
            this.wizardPageStep3.Description = "ShipWorks will help you determine the language of your printer.";
            this.wizardPageStep3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageStep3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageStep3.Location = new System.Drawing.Point(0, 0);
            this.wizardPageStep3.Name = "wizardPageStep3";
            this.wizardPageStep3.Size = new System.Drawing.Size(445, 246);
            this.wizardPageStep3.TabIndex = 0;
            this.wizardPageStep3.Title = "Thermal Language";
            this.wizardPageStep3.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextTest);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(278, 13);
            this.label5.TabIndex = 66;
            this.label5.Text = "We\'ve almost got this figured out.  Just one more test...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(344, 13);
            this.label3.TabIndex = 65;
            this.label3.Text = "Turn your printer off, and then back on, before running the next test.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 64;
            this.label2.Text = "Important";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ShipWorks.Properties.Resources.exclamation16;
            this.pictureBox2.Location = new System.Drawing.Point(23, 34);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 63;
            this.pictureBox2.TabStop = false;
            // 
            // panelTest3Result
            // 
            this.panelTest3Result.Controls.Add(this.label1);
            this.panelTest3Result.Controls.Add(this.labelTest3Question);
            this.panelTest3Result.Controls.Add(this.radioTest3No);
            this.panelTest3Result.Controls.Add(this.radioTest3Yes);
            this.panelTest3Result.Location = new System.Drawing.Point(17, 128);
            this.panelTest3Result.Name = "panelTest3Result";
            this.panelTest3Result.Size = new System.Drawing.Size(384, 76);
            this.panelTest3Result.TabIndex = 62;
            this.panelTest3Result.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 55;
            this.label1.Text = "Result";
            // 
            // labelTest3Question
            // 
            this.labelTest3Question.AutoSize = true;
            this.labelTest3Question.Location = new System.Drawing.Point(17, 26);
            this.labelTest3Question.Name = "labelTest3Question";
            this.labelTest3Question.Size = new System.Drawing.Size(333, 13);
            this.labelTest3Question.TabIndex = 49;
            this.labelTest3Question.Text = "Did a label print that says \"ShipWorks Test 3: You are rocking this!\"?";
            // 
            // radioTest3No
            // 
            this.radioTest3No.AutoSize = true;
            this.radioTest3No.Location = new System.Drawing.Point(82, 45);
            this.radioTest3No.Name = "radioTest3No";
            this.radioTest3No.Size = new System.Drawing.Size(38, 17);
            this.radioTest3No.TabIndex = 54;
            this.radioTest3No.TabStop = true;
            this.radioTest3No.Text = "No";
            this.radioTest3No.UseVisualStyleBackColor = true;
            // 
            // radioTest3Yes
            // 
            this.radioTest3Yes.AutoSize = true;
            this.radioTest3Yes.Location = new System.Drawing.Point(34, 45);
            this.radioTest3Yes.Name = "radioTest3Yes";
            this.radioTest3Yes.Size = new System.Drawing.Size(42, 17);
            this.radioTest3Yes.TabIndex = 53;
            this.radioTest3Yes.TabStop = true;
            this.radioTest3Yes.Text = "Yes";
            this.radioTest3Yes.UseVisualStyleBackColor = true;
            // 
            // printTest3
            // 
            this.printTest3.Location = new System.Drawing.Point(37, 99);
            this.printTest3.Name = "printTest3";
            this.printTest3.Size = new System.Drawing.Size(75, 23);
            this.printTest3.TabIndex = 61;
            this.printTest3.Text = "Print Test 3";
            this.printTest3.UseVisualStyleBackColor = true;
            this.printTest3.Click += new System.EventHandler(this.OnPrintTest);
            // 
            // labelTest3
            // 
            this.labelTest3.AutoSize = true;
            this.labelTest3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTest3.Location = new System.Drawing.Point(20, 79);
            this.labelTest3.Name = "labelTest3";
            this.labelTest3.Size = new System.Drawing.Size(42, 13);
            this.labelTest3.TabIndex = 60;
            this.labelTest3.Text = "Test 3";
            // 
            // wizardPageFinish
            // 
            this.wizardPageFinish.Controls.Add(this.labelLanguage);
            this.wizardPageFinish.Controls.Add(this.labelPrinterType);
            this.wizardPageFinish.Controls.Add(this.pictureBox1);
            this.wizardPageFinish.Description = "ShipWorks will help you determine the language of your printer.";
            this.wizardPageFinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFinish.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageFinish.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFinish.Name = "wizardPageFinish";
            this.wizardPageFinish.Size = new System.Drawing.Size(445, 246);
            this.wizardPageFinish.TabIndex = 0;
            this.wizardPageFinish.Title = "Thermal Language";
            // 
            // labelLanguage
            // 
            this.labelLanguage.AutoSize = true;
            this.labelLanguage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLanguage.Location = new System.Drawing.Point(321, 12);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(26, 13);
            this.labelLanguage.TabIndex = 4;
            this.labelLanguage.Text = "EPL";
            // 
            // labelPrinterType
            // 
            this.labelPrinterType.AutoSize = true;
            this.labelPrinterType.Location = new System.Drawing.Point(43, 12);
            this.labelPrinterType.Name = "labelPrinterType";
            this.labelPrinterType.Size = new System.Drawing.Size(281, 13);
            this.labelPrinterType.TabIndex = 3;
            this.labelPrinterType.Text = "ShipWorks has determined that your printer language is: ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.check16;
            this.pictureBox1.Location = new System.Drawing.Point(24, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // PrinterThermalLanguageWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 353);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PrinterThermalLanguageWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageStep1,
            this.wizardPageStep2,
            this.wizardPageStep3,
            this.wizardPageFinish});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Printer Thermal Language";
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageStep1.ResumeLayout(false);
            this.wizardPageStep1.PerformLayout();
            this.panelTest1Result.ResumeLayout(false);
            this.panelTest1Result.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePrinter)).EndInit();
            this.wizardPageStep2.ResumeLayout(false);
            this.wizardPageStep2.PerformLayout();
            this.panelTest2Result.ResumeLayout(false);
            this.panelTest2Result.PerformLayout();
            this.wizardPageStep3.ResumeLayout(false);
            this.wizardPageStep3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panelTest3Result.ResumeLayout(false);
            this.panelTest3Result.PerformLayout();
            this.wizardPageFinish.ResumeLayout(false);
            this.wizardPageFinish.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageStep1;
        private UI.Wizard.WizardPage wizardPageStep2;
        private UI.Wizard.WizardPage wizardPageStep3;
        private UI.Wizard.WizardPage wizardPageFinish;
        private System.Windows.Forms.Label labelPrinter;
        private System.Windows.Forms.PictureBox picturePrinter;
        private System.Windows.Forms.Label printerName;
        private System.Windows.Forms.Label labelPrinterTests2;
        private System.Windows.Forms.Label labelTest1;
        private System.Windows.Forms.Label labelQuestionTest1Result;
        private System.Windows.Forms.Panel panelTest1Result;
        private System.Windows.Forms.Label labelTest1Result;
        private System.Windows.Forms.RadioButton radioTest1No;
        private System.Windows.Forms.RadioButton radioTest1Yes;
        private System.Windows.Forms.Button printTest1;
        private System.Windows.Forms.Panel panelTest2Result;
        private System.Windows.Forms.Label labelTest2Result;
        private System.Windows.Forms.Label labelTest2Question;
        private System.Windows.Forms.RadioButton radioTest2No;
        private System.Windows.Forms.RadioButton radioTest2Yes;
        private System.Windows.Forms.Button printTest2;
        private System.Windows.Forms.Label labelTest2;
        private System.Windows.Forms.Panel panelTest3Result;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTest3Question;
        private System.Windows.Forms.RadioButton radioTest3No;
        private System.Windows.Forms.RadioButton radioTest3Yes;
        private System.Windows.Forms.Button printTest3;
        private System.Windows.Forms.Label labelTest3;
        private System.Windows.Forms.Label labelPrinterType;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelLanguage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}