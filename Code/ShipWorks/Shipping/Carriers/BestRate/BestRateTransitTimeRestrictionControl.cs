using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.BestRate.Enums;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class BestRateTransitTimeRestrictionControl : UserControl
    {

        /// <summary>
        /// Gets or sets the expected number of days.
        /// </summary>
        public int ExpectedNumberOfDays
        {
            get
            {
                return expectedNumberOfDays.NumericValue.HasValue ? expectedNumberOfDays.NumericValue.Value : 0;
            }
            set
            {
                expectedNumberOfDays.Text = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the type of the transit time.
        /// </summary>
        public TransitTimeType TransitTimeType
        {
            get 
            {
                return anyTime.Checked ? TransitTimeType.Any : TransitTimeType.Expected;
            }
            set
            {
                anyTime.Checked = (value == TransitTimeType.Any);
                expected.Checked = (value == TransitTimeType.Expected);
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateTransitTimeRestrictionControl"/> class.
        /// </summary>
        public BestRateTransitTimeRestrictionControl()
        {
            InitializeComponent();
        }
    }
}
