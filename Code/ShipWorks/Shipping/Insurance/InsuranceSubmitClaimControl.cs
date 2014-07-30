using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IdentityModel.Claims;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using Microsoft.Web.Services3.Referral;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Control to submit a claim to InsureShip
    /// </summary>
    public partial class InsuranceSubmitClaimControl : UserControl
    {
        private ShipmentEntity shipment;
        static readonly ILog log = LogManager.GetLogger(typeof(InsuranceSubmitClaimControl));

        /// <summary>
        /// Initializes a new instance of the <see cref="InsuranceSubmitClaimControl"/> class.
        /// </summary>
        public InsuranceSubmitClaimControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<InsureShipClaimType>(claimType);
        }

        /// <summary>
        /// Gets or sets the claim submitted.
        /// </summary>
        public Action ClaimSubmitted { get; set; }

        /// <summary>
        /// Loads the shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public void LoadShipment(ShipmentEntity shipment)
        {
            this.shipment = shipment;

            bool show = !shipment.Voided && shipment.InsurancePolicy != null && !shipment.InsurancePolicy.ClaimID.HasValue;
            Visible = show;

            if (show)
            {
                if (shipment.InsurancePolicy.ClaimType.HasValue)
                {
                    claimType.SelectedValue = (InsureShipClaimType) shipment.InsurancePolicy.ClaimType;
                }

                if (shipment.InsurancePolicy.DamageValue.HasValue)
                {
                    damageValue.Amount = shipment.InsurancePolicy.DamageValue.Value;
                }

                itemName.Text = shipment.InsurancePolicy.ItemName;
            }
        }

        /// <summary>
        /// Called when [submit claim click].
        /// </summary>
        private void OnSubmitClaimClick(object sender, EventArgs e)
        {
            try
            {
                StoreEntity storeEntity = StoreManager.GetStore(shipment.Order.StoreID);
                if (storeEntity == null)
                {
                    throw new InsureShipException("The store the shipment was in has been deleted.");
                }

                InsureShipAffiliate insureShipAffiliate = TangoWebClient.GetInsureShipAffiliate(storeEntity);
                InsureShipClaim claim = new InsureShipClaim(shipment, insureShipAffiliate);

                claim.Submit((InsureShipClaimType) claimType.SelectedValue, itemName.Text, damageValue.Amount);

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    adapter.SaveAndRefetch(shipment);

                    adapter.Commit();
                }

                ClaimSubmitted.Invoke();
            }
            catch (InsureShipException ex)
            {
                log.Error(ex);
                MessageHelper.ShowError(this, ex.Message);
            }
        }
    }
}
