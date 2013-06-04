using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Shipment type for Express 1 shipments.
    /// </summary>
    public class Express1ShipmentType : EndiciaShipmentType
    {
        /// <summary>
        /// Postal Shipment Type
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return ShipmentTypeCode.PostalExpress1;
            }
        }

        /// <summary>
        /// Reseller type
        /// </summary>
        public override EndiciaReseller EndiciaReseller
        {
            get
            {
                return EndiciaReseller.Express1;
            }
        }

        /// <summary>
        /// Gets the configured Express1 Accounts
        /// </summary>
        public override List<EndiciaAccountEntity> Accounts
        {
            get
            {
                return EndiciaAccountManager.Express1Accounts;
            }
        }

        /// <summary>
        /// Create the Service Control
        /// </summary>
        public override ServiceControlBase CreateServiceControl()
        {
            return new Express1ServiceControl();
        }

        /// <summary>
        /// Process the label server shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            try
            {
                EndiciaApiClient.ProcessShipment(shipment);
            }
            catch (EndiciaException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Void the given endicia shipment
        /// </summary>
        public override void VoidShipment(ShipmentEntity shipment)
        {
            try
            {
                Express1CustomerServiceClient.RequestRefund(shipment);
            }
            catch (EndiciaException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Create the setup wizard for configuring an Express 1 account.
        /// </summary>
        public override Form CreateSetupWizard()
        {
            return new Express1SetupWizard();
        }
    }
}
