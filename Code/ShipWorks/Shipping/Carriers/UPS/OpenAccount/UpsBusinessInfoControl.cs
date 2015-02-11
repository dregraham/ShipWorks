using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using SpreadsheetGear.CustomFunctions;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    public partial class UpsBusinessInfoControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsBusinessInfoControl" /> class.
        /// </summary>
        public UpsBusinessInfoControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<UpsBusinessIndustry>(industry);
            EnumHelper.BindComboBox<UpsNumberOfEmployees>(numberOfEmployees);
        }

        /// <summary>
        /// Gets or sets Action that gets called when industry changed.
        /// </summary>
        public Action<UpsBusinessIndustry> IndustryChanged { get; set; }
        
        /// <summary>
        /// Saves to request.
        /// </summary>
        public void SaveToRequest(OpenAccountRequest request)
        {
            if (request.AccountCharacteristics == null)
            {
                request.AccountCharacteristics = new AccountCharacteristicsType();
            }
            if (Visible)
            {
                if (request.AccountCharacteristics.BusinessInformation == null)
                {
                    request.AccountCharacteristics.BusinessInformation = new BusinessInformationType();
                }

                request.AccountCharacteristics.BusinessInformation.Industry = new CodeOnlyType
                {
                    Code = EnumHelper.GetApiValue((UpsBusinessIndustry)industry.SelectedValue)
                };

                request.AccountCharacteristics.BusinessInformation.NumberOfEmployees = new CodeOnlyType
                {
                    Code = EnumHelper.GetApiValue((UpsNumberOfEmployees)numberOfEmployees.SelectedValue)
                };
            }
            else
            {
                request.AccountCharacteristics.BusinessInformation = null;
            }
        }

        /// <summary>
        /// Called when [industry changed].
        /// </summary>
        private void OnIndustryChanged(object sender, EventArgs e)
        {
            if (IndustryChanged!=null)
            {
                IndustryChanged.Invoke((UpsBusinessIndustry)industry.SelectedValue);
            }
        }
    }
}
