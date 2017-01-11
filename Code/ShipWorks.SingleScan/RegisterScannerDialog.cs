using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    [NamedComponent("RegisterScannerDialog", typeof(Form))]
    public partial class RegisterScannerDialog : Form
    {
        private readonly IRegisterScannerDlgViewModel viewmodel;

        public RegisterScannerDialog(IRegisterScannerDlgViewModel viewmodel)
        {
            this.viewmodel = viewmodel;
            InitializeComponent();
            registrationControl.DataContext = this.viewmodel;
        }
    }
}
