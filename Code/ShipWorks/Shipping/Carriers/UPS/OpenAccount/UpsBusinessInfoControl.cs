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
        /// Saves to request.
        /// </summary>
        public void SaveToRequest(OpenAccountRequest request)
        {
            if (industry.SelectedValue.ToString() == UpsBusinessIndustry.Unselected.ToString())
            {
                throw new UpsOpenAccountException("Please select an industry.", UpsOpenAccountErrorCode.MissingRequiredFields);
            }

            if (numberOfEmployees.SelectedValue.ToString() == UpsNumberOfEmployees.Unselected.ToString())
            {
                throw new UpsOpenAccountException("Please select a number of employees.", UpsOpenAccountErrorCode.MissingRequiredFields);
            }

            if (request.AccountCharacteristics == null)
            {
                request.AccountCharacteristics = new AccountCharacteristicsType();
            }
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
    }
}
