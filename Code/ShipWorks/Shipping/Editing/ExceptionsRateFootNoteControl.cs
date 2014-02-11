using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Editing
{
    public partial class ExceptionsRateFootNoteControl : RateFootnoteControl
    {
        private string errorMessageText;

        public ExceptionsRateFootNoteControl(string errorMessage)
        {
            errorMessageText = errorMessage;

            InitializeComponent();
        }

        private void OnClickExceptionsLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageHelper.ShowInformation(this, errorMessageText);
        }
    }
}
