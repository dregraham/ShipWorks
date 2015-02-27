using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Postal.Usps
{   
    public class UspsPrototypeFixture : PostalPrototypeFixture
    {
        public UspsPrototypeFixture()
        {
        }

        public string ScanFormBatchID { get; set; }
        public string Memo { get; set; }
        public string RequireFullAddressValidation { get; set; }
        public string HidePostage { get; set; }
        public string UspsAccountID { get; set; }

        public override bool Ship(UspsResellerType resellerType)
        {
            try
            {
                ShipmentEntity shipment = CreateShipment();

                 //If you want to create the shipments, but NOT process them, press the magic keys
                 //This is helpful to get all the shipments into SW unprocessed so that you can process them with the UI
                if (!MagicKeysDown)
                {
                    IUspsWebClient webClient = GetWebClient(resellerType);
                    webClient.ProcessShipment(shipment);

                    //shipment.ContentWeight = shipment.FedEx.Packages.Sum(p => p.Weight) + shipment.FedEx.Packages.Sum(p => p.DimsWeight) + shipment.FedEx.Packages.Sum(p => p.DryIceWeight);
                    shipment.Processed = true;
                    shipment.ProcessedDate = DateTime.UtcNow;
                }

                shipment.CustomsGenerated = true;

                ShippingManager.SaveShipment(shipment);

                if (resellerType == UspsResellerType.Express1)
                {
                    // Now void to get our money back.  Sleep for a few seconds so that the carrier can process the void on their side.
                    VoidShipment(shipment);
                    Thread.Sleep(3000);
                }

                return true;
            }
            finally
            {
                CleanupLabel();
            }
        }

        public List<RateResult> GetRates(UspsResellerType resellerType)
        {
            ShipmentEntity shipment = CreateShipment();

            IUspsWebClient webClient = GetWebClient(resellerType);
            return webClient.GetRates(shipment);
        }

        private IUspsWebClient GetWebClient(UspsResellerType resellerType)
        {
            switch (resellerType)
            {
                case UspsResellerType.None:
                    return new UspsWebClient(GetAccountRepository(resellerType), new LogEntryFactory(), new TrustingCertificateInspector(), resellerType);

                case UspsResellerType.Express1:
                    return new Express1UspsWebClient(GetAccountRepository(resellerType), new LogEntryFactory(), new TrustingCertificateInspector());
                default:
                    throw new ArgumentOutOfRangeException("resellerType");
            }
        }

        private IUspsWebClient GetWebClient(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

            IUspsWebClient webClient = null;
            if (shipmentType.ShipmentTypeCode == global::ShipWorks.Shipping.ShipmentTypeCode.Usps)
            {
                webClient = GetWebClient(UspsResellerType.None);
            }
            else
            {
                webClient = GetWebClient(UspsResellerType.Express1);
            }

            return webClient;
        }

        private ICarrierAccountRepository<UspsAccountEntity> GetAccountRepository(UspsResellerType resellerType)
        {
            switch (resellerType)
            {
                case UspsResellerType.None:
                    return new UspsAccountRepository();
                case UspsResellerType.Express1:
                    return new Express1UspsAccountRepository();
                default:
                    throw new ArgumentOutOfRangeException("resellerType");
            }
        }

        protected override void CleanupLabel()
        {
            if (!IsSaveLabel)
            {
                string certificationDirectory = LogSession.LogFolder + "\\UspsCertification\\";

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

            shipment.Postal.Usps.ScanFormBatchID = null;
            shipment.Postal.Memo1 = Memo;
            shipment.Postal.Usps.RequireFullAddressValidation = Convert.ToInt16(RequireFullAddressValidation) == 1;
            shipment.Postal.Usps.HidePostage = Convert.ToInt16(HidePostage) == 1;
            shipment.Postal.Usps.UspsAccountID = Convert.ToInt64(UspsAccountID);

            // Save the record
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Save the shipment
                adapter.SaveAndRefetch(shipment);
                adapter.Commit();
            }

            return shipment;
        }

        public void VoidShipment(ShipmentEntity shipment)
        {
            IUspsWebClient webClient = GetWebClient(shipment);

            webClient.VoidShipment(shipment);
        }

        public void PurchasePostage(decimal amount)
        {
            ShipmentEntity shipment = CreateShipment();
            UspsAccountEntity uspsAccount = UspsAccountManager.GetAccount(shipment.Postal.Usps.UspsAccountID);
            UspsPostageWebClient uspsPostageWebClient = new UspsPostageWebClient(uspsAccount);

            uspsPostageWebClient.Purchase(amount);
        }

        public decimal CheckPostage()
        {
            ShipmentEntity shipment = CreateShipment();
            UspsAccountEntity uspsAccount = UspsAccountManager.GetAccount(shipment.Postal.Usps.UspsAccountID);
            UspsPostageWebClient uspsPostageWebClient = new UspsPostageWebClient(uspsAccount);

            return uspsPostageWebClient.GetBalance();
        }
    }
}
