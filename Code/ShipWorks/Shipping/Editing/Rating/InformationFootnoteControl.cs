using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Shows Rate not supported and allows a user 
    /// </summary>
    public partial class InformationFootnoteControl : RateFootnoteControl
    {
        public InformationFootnoteControl(string infoText)
        {
            InitializeComponent();
            informationMessageLabel.Text = infoText;
        }
    }
}
