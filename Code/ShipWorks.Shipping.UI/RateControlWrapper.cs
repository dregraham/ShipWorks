using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Wraps the RateControl so that we can bind to the RateGroup easily
    /// </summary>
    public partial class RateControlWrapper : UserControl
    {
        private ShipmentTypeCode shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public RateControlWrapper()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get/Sets the RateControl RateGroup
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RateGroup RateGroup
        {
            get
            {
                return rateControl.RateGroup;
            }
            set
            {
                rateControl.LoadRates(value);
            }
        }

        /// <summary>
        /// Get/Sets the RateControl ShowSpinner
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowSpinner
        {
            get
            {
                return rateControl.ShowSpinner;
            }
            set
            {
                if (rateControl.InvokeRequired)
                {
                    rateControl.Invoke((Action)(() => rateControl.ShowSpinner = value));
                }
                else
                {
                    rateControl.ShowSpinner = value;
                }
            }
        }

        /// <summary>
        /// Calls ClearRates with given message on the RateControl.
        /// This should be used to clear the rate grid and message displayed in the rate control.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ClearRates
        {
            get
            {
                return string.Empty;
            }
            set
            {
                rateControl.ClearRates(value);
            }
        }

        /// <summary>
        /// Calls ClearRates with given message on the RateControl
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ErrorMessage
        {
            get
            {
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    rateControl.ClearRates(value);
                }
            }
        }

        /// <summary>
        /// Wrapper property to apply ShippingPolicies to the rate control based on changed ShipmentType
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentTypeCode ShipmentType
        {
            set
            {
                shipmentType = value;

                if (ShippingPolicies.Current != null)
                {
                    ShippingPolicies.Current.Apply(shipmentType, rateControl);
                }
            }
            get
            {
                return shipmentType;
            }
        }
    }
}
