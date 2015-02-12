using System;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    public partial class UpsAccountCharacteristicsControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsAccountCharacteristicsControl" /> class.
        /// </summary>
        public UpsAccountCharacteristicsControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<UpsPrimaryReason>(primaryReason);
            EnumHelper.BindComboBox<UpsOtherCarrierType>(carrierType);
        }

        /// <summary>
        /// Hide/Show carrier type when primaryReason changes.
        /// </summary>
        private void OnPrimaryReasonChanged(object sender, EventArgs e)
        {
            bool shouldCarrierTypeBeVisible = (UpsPrimaryReason) primaryReason.SelectedValue == UpsPrimaryReason.OtherCarrier;

            carrierType.Visible = shouldCarrierTypeBeVisible;
            labelCarrierType.Visible = shouldCarrierTypeBeVisible;
        }
        
        /// <summary>
        /// Saves to request.
        /// </summary>
        public void SaveToRequest(OpenAccountRequest request)
        {
            if (request.AccountCharacteristics == null)
            {
                request.AccountCharacteristics = new AccountCharacteristicsType();
            }

            request.AccountCharacteristics.CustomerClassification = new CodeOnlyType
            {
                Code = EnumHelper.GetApiValue(UpsCustomerClassificationCode.Business)
            };

            request.AccountCharacteristics.PrimaryReason = new PrimaryReasonType
            {
                Code = EnumHelper.GetApiValue((UpsPrimaryReason) primaryReason.SelectedValue), CarrierType =
                    carrierType.Visible ?
                        new CodeOnlyType
                        {
                            Code = EnumHelper.GetApiValue((UpsOtherCarrierType) carrierType.SelectedValue)
                        } : null
            };
        }
    }
}