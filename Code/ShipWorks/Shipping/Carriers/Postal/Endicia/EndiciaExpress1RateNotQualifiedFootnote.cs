using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// UserControl for letting the user know their current packging options didn't qualify for an Express1 rate
    /// </summary>
    public partial class EndiciaExpress1RateNotQualifiedFootnote : RateFootnoteControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaExpress1RateNotQualifiedFootnote()
        {
            InitializeComponent();
        }
    }
}
