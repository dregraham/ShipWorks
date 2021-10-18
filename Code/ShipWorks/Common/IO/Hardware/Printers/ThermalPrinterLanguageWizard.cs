using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Common.IO.Hardware.Printers
{
    /// <summary>
    /// Wizard for helping a user choose the thermal language of a printer
    /// </summary>
    public partial class ThermalPrinterLanguageWizard : WizardForm
    {
        string printer;

        ThermalLanguage thermalLanguage = ThermalLanguage.None;

        class PrinterTest
        {
            public string PrinterContent { get; set; }
            public bool IsThermal { get; set; }
            public bool IsPrinted { get; set; }
            public Panel ResultPanel { get; set; }
            public RadioButton RadioYes { get; set; }
            public RadioButton RadioNo { get; set; }
            public Action<bool, WizardStepEventArgs> OnNext { get; set; }
        }

        #region Thermal code

        string zplTest =
            "^XA^CFD\r\n" +
            "^FO100,100^AQ\r\n" +
            "^FDShipWorks Test 2^FS\r\n" +
            "^FO100,180\r\n" +
            "^AQ\r\n" +
            "^FDWe're glad you're here!^FS\r\n" +
            "^XZ";

        string eplTest =
            "ZT\n" +
            "N\n" +
            "ZB\n" +
            "R100,100\n" +
            "A0,0,0,3,1,1,N,\"ShipWorks Test 3\"\n" +
            "A0,80,0,3,1,1,N,\"You are rocking this!\"\n" +
            "P1\n";

        #endregion

        List<PrinterTest> tests = new List<PrinterTest>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ThermalPrinterLanguageWizard(string printer)
        {
            InitializeComponent();

            this.printer = printer;
            printerName.Text = printer;

            CreateTests();
        }

        /// <summary>
        /// Create the tests
        /// </summary>
        private void CreateTests()
        {
            tests.Add(new PrinterTest()
            {
                PrinterContent = "ShipWorks Test 1: Welcome to ShipWorks!",
                IsThermal = false,
                ResultPanel = panelTest1Result,
                RadioYes = radioTest1Yes,
                RadioNo = radioTest1No,
                OnNext = OnStep1Next
            });

            tests.Add(new PrinterTest()
            {
                PrinterContent = zplTest,
                IsThermal = true,
                ResultPanel = panelTest2Result,
                RadioYes = radioTest2Yes,
                RadioNo = radioTest2No,
                OnNext = OnStep2Next
            });

            tests.Add(new PrinterTest()
            {
                PrinterContent = eplTest,
                IsThermal = true,
                ResultPanel = panelTest3Result,
                RadioYes = radioTest3Yes,
                RadioNo = radioTest3No,
                OnNext = OnStep3Next
            });

        }

        /// <summary>
        /// The thermal language that has been determined.  Only valid if DialogResult is OK
        /// </summary>
        public ThermalLanguage ThermalLanguage
        {
            get
            {
                return thermalLanguage;
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Printing test
        /// </summary>
        private void OnPrintTest(object sender, EventArgs e)
        {
            PrinterTest test = tests[Pages.IndexOf((WizardPage) CurrentPage)];

            if (test.IsThermal)
            {
                RawPrinter rawPrinter = new RawPrinter(printer);
                rawPrinter.SendStringToPrinter("ShipWorks Test", test.PrinterContent);

                test.IsPrinted = true;
            }
            else
            {
                test.IsPrinted = PrintTest(test.PrinterContent);
            }

            if (test.IsPrinted)
            {
                test.ResultPanel.Visible = true;
            }
        }

        /// <summary>
        /// Stepping next from test
        /// </summary>
        private void OnStepNextTest(object sender, WizardStepEventArgs e)
        {
            PrinterTest test = tests[Pages.IndexOf((WizardPage) CurrentPage)];

            if (!test.IsPrinted)
            {
                MessageHelper.ShowInformation(this, "Please print the test before continuing.");
                e.NextPage = CurrentPage;
                return;
            }

            // Verified
            if (test.RadioYes.Checked)
            {
                test.OnNext(true, e);
            }
            // Didn't work
            else if (test.RadioNo.Checked)
            {
                test.OnNext(false, e);
            }
            // Nothing chosen
            else
            {
                MessageHelper.ShowInformation(this, "Please indicate the result of the test before continuing.");
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Stepping next from test1
        /// </summary>
        private void OnStep1Next(bool success, WizardStepEventArgs args)
        {
            if (!success)
            {
                MessageHelper.ShowInformation(this, "Check that your printer is properly connected and turned on, and try again.");
                args.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Stepping next from test 2
        /// </summary>
        private void OnStep2Next(bool success, WizardStepEventArgs args)
        {
            if (success)
            {
                TestComplete(Printers.ThermalLanguage.ZPL, args);
            }
        }

        /// <summary>
        /// Stepping next from test 2
        /// </summary>
        private void OnStep3Next(bool success, WizardStepEventArgs args)
        {
            if (success)
            {
                TestComplete(Printers.ThermalLanguage.EPL, args);
            }
            else
            {
                TestComplete(Printers.ThermalLanguage.None, args);
            }
        }

        /// <summary>
        /// Called once we know what the thermal language is
        /// </summary>
        private void TestComplete(ThermalLanguage language, WizardStepEventArgs args)
        {
            this.thermalLanguage = language;
            labelLanguage.Text = EnumHelper.GetDescription(language);

            args.NextPage = wizardPageFinish;
        }

        /// <summary>
        /// Print the given content to the printer
        /// </summary>
        private bool PrintTest(string content)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.DocumentName = "ShipWorks Test";
            printDocument.PrinterSettings.PrinterName = printer;
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custom", 300, 400);

            printDocument.PrintPage += (object unused, PrintPageEventArgs printArgs) =>
            {
                using (Font font = new Font("Times New Roman", 12))
                {
                    StringFormat stringFormat = new StringFormat(StringFormatFlags.LineLimit);
                    stringFormat.Trimming = StringTrimming.Word;

                    printArgs.Graphics.DrawString(
                        content,
                        font,
                        Brushes.Black,
                        printArgs.MarginBounds,
                        stringFormat);

                    // If more lines exist, print another page.
                    printArgs.HasMorePages = false;
                }
            };

            try
            {
                printDocument.Print();
                return true;
            }
            catch (InvalidPrinterException ex)
            {
                MessageHelper.ShowError(this, "There was a problem printing the test: " + ex.Message);
            }
            catch (Win32Exception ex)
            {
                string message = ex.Message;

                if (ex.NativeErrorCode == 1722)
                {
                    message = "The printer could not be found.";
                }

                MessageHelper.ShowError(this, "There was a problem printing the test: " + message);
            }
            finally
            {
                printDocument.Dispose();
            }

            return false;
        }
    }
}
