using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Interapptive.Shared.Win32;
using log4net;
using log4net.Core;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Data;
using ShipWorks.Templates;
using ShipWorks.Tests.Integration.MSTest.Utilities;
using ShipWorks.Users;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Stores;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Users.Audit;
using ShipWorks.Shipping;
using Moq;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Postal.Stamps
{   
    public class StampsPrototypeFixture : PostalPrototypeFixture
    {
        public StampsPrototypeFixture()
        {
        }

        public string ScanFormBatchID { get; set; }
        public string Memo { get; set; }
        public string RequireFullAddressValidation { get; set; }
        public string HidePostage { get; set; }
        public string StampsAccountID { get; set; }

        public override bool Ship()
        {
            try
            {
                StampsWebClient stampsWebClient;
                ShipmentEntity shipment = CreateShipment();

                // If you want to create the shipments, but NOT process them, press the magic keys
                // This is helpful to get all the shipments into SW unprocessed so that you can process them with the UI
                if (!MagicKeysDown)
                {
                    stampsWebClient = new StampsWebClient(new UspsAccountRepository(), new LogEntryFactory(), new TrustingCertificateInspector(), StampsResellerType.StampsExpedited);
                    stampsWebClient.ProcessShipment(shipment);

                    //shipment.ContentWeight = shipment.FedEx.Packages.Sum(p => p.Weight) + shipment.FedEx.Packages.Sum(p => p.DimsWeight) + shipment.FedEx.Packages.Sum(p => p.DryIceWeight);
                    shipment.Processed = true;
                    shipment.ProcessedDate = DateTime.UtcNow;
                }

                shipment.CustomsGenerated = true;

                ShippingManager.SaveShipment(shipment);
                
                return true;
            }
            finally
            {
                CleanupLabel();
            }
        }

        protected override void CleanupLabel()
        {
            if (!IsSaveLabel)
            {
                string certificationDirectory = LogSession.LogFolder + "\\StampsCertification\\";

                if (Directory.Exists(certificationDirectory))
                {
                    string[] filesToDelete = Directory.GetFiles(certificationDirectory, "*.png");
                    foreach (string fileToDelete in filesToDelete)
                    {
                        File.Delete(fileToDelete);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public override ShipmentEntity CreateShipment()
        {
            ShipmentEntity shipment = base.CreateShipment();

            shipment.Postal.Stamps.ScanFormBatchID = null;
            shipment.Postal.Stamps.Memo = Memo;
            shipment.Postal.Stamps.RequireFullAddressValidation = Convert.ToInt16(RequireFullAddressValidation) == 1;
            shipment.Postal.Stamps.HidePostage = Convert.ToInt16(HidePostage) == 1;
            shipment.Postal.Stamps.StampsAccountID = Convert.ToInt16(StampsAccountID);

            // Save the record
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Save the shipment
                adapter.SaveAndRefetch(shipment);
                adapter.Commit();
            }

            return shipment;
        }
    }
}
