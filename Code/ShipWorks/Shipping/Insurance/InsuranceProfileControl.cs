using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// UserControl to drop on profile windows for configuring insurance
    /// </summary>
    public partial class InsuranceProfileControl : UserControl
    {
        bool cleared = false;

        /// <summary>
        /// Contructor
        /// </summary>
        public InsuranceProfileControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<InsuranceInitialValueSource>(source);
        }

        /// <summary>
        /// Changing the source
        /// </summary>
        private void OnChangeSource(object sender, EventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// The source for the initial value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseInsurance
        {
            get
            {
                return cleared ?
                    false :
                    useInsurance.Checked;
            }
            set
            {
                useInsurance.Checked = value;
            }
        }


        /// <summary>
        /// The source for the initial value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InsuranceInitialValueSource Source
        {
            get
            {
                return cleared ?
                    InsuranceInitialValueSource.OrderTotal :
                    (InsuranceInitialValueSource) source.SelectedValue;
            }
            set
            {
                source.SelectedValue = value;
            }
        }

        /// <summary>
        /// The amount to use when the source is 'Other'
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal OtherAmount
        {
            get { return amount.Amount; }
            set { amount.Amount = value; }
        }

        /// <summary>
        /// The label for the box to enable using insurance
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string UseInsuranceBoxLabel
        {
            get { return useInsurance.Text; }
            set { useInsurance.Text = value; }
        }

        /// <summary>
        /// The label for the box to specify a declared value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string InsuredValueLabel
        {
            get { return labelInsuredValue.Text; }
            set { labelInsuredValue.Text = value; }
        }

        /// <summary>
        /// Update the UI based on the settings
        /// </summary>
        private void UpdateUI()
        {
            amount.Visible = !cleared && (Source == InsuranceInitialValueSource.OtherAmount);
        }

        /// <summary>
        /// Indicates if all the values in the control have been cleared and are not visible
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Cleared
        {
            get
            {
                return cleared;
            }
            set
            {
                if (cleared == value)
                {
                    return;
                }

                cleared = value;

                if (cleared)
                {
                    source.SelectedIndex = -1;
                }
                else
                {
                    source.SelectedIndex = 0;
                }

                useInsurance.Checked = false;
            }
        }

    }
}
