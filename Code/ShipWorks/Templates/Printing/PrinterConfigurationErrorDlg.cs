using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Window for showing the user detailed information about what to do about errors configuring a printer
    /// </summary>
    public partial class PrinterConfigurationErrorDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PrinterConfigurationErrorDlg(string printer)
        {
            InitializeComponent();

            printerName.Text = printer;
        }
    }
}
