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
    /// <summary>
    /// Gathers pharmacutical choices made by customer.
    /// </summary>
    public partial class UpsPharmaceuticalControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsPharmaceuticalControl"/> class.
        /// </summary>
        public UpsPharmaceuticalControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when [will ship yes checked changed].
        /// </summary>
        private void OnWillShipYesCheckedChanged(object sender, EventArgs e)
        {
            followUpQuestionPanel.Visible = willShipYes.Checked;
        }

        /// <summary>
        /// Saves to request.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "request"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void SaveToRequest(OpenAccountRequest request)
        {
            // Take out above suppression

            if (request.AccountCharacteristics == null)
            {
                request.AccountCharacteristics = new AccountCharacteristicsType();
            }
            request.AccountCharacteristics.PrescriptionToPatients = string.Empty;
            request.AccountCharacteristics.LicensedInStateOrTerritory = string.Empty;
            request.AccountCharacteristics.OnlineOrMailOrder = string.Empty;

            if (Visible)
            {
                if (willShipYes.Checked)
                {
                    request.AccountCharacteristics.PrescriptionToPatients = EnumHelper.GetApiValue(UpsYesNoType.Yes);
                    request.AccountCharacteristics.LicensedInStateOrTerritory = EnumHelper.GetApiValue(licensedYes.Checked ? UpsYesNoType.Yes : UpsYesNoType.No);
                    request.AccountCharacteristics.OnlineOrMailOrder = EnumHelper.GetApiValue(onlinePharmacyYes.Checked ? UpsYesNoType.Yes : UpsYesNoType.No);
                }
                else
                {
                    request.AccountCharacteristics.PrescriptionToPatients = EnumHelper.GetApiValue(UpsYesNoType.No);
                }
            }
        }
    }
}
