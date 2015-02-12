using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Editing.Rating;

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

        public override bool Ship(StampsResellerType stampsResellerType)
        {
            try
            {
                IStampsWebClient stampsWebClient;
                ShipmentEntity shipment = CreateShipment();

                 //If you want to create the shipments, but NOT process them, press the magic keys
                 //This is helpful to get all the shipments into SW unprocessed so that you can process them with the UI
                if (!MagicKeysDown)
                {
                    stampsWebClient = GetWebClient(stampsResellerType);
                    stampsWebClient.ProcessShipment(shipment);

                    //shipment.ContentWeight = shipment.FedEx.Packages.Sum(p => p.Weight) + shipment.FedEx.Packages.Sum(p => p.DimsWeight) + shipment.FedEx.Packages.Sum(p => p.DryIceWeight);
                    shipment.Processed = true;
                    shipment.ProcessedDate = DateTime.UtcNow;
                }

                shipment.CustomsGenerated = true;

                ShippingManager.SaveShipment(shipment);

                if (stampsResellerType == StampsResellerType.Express1)
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


        public List<RateResult> GetRates(StampsResellerType stampsResellerType)
        {
            IStampsWebClient stampsWebClient;
            ShipmentEntity shipment = CreateShipment();

            stampsWebClient = GetWebClient(stampsResellerType);
            return stampsWebClient.GetRates(shipment);
        }

        private IStampsWebClient GetWebClient(StampsResellerType stampsResellerType)
        {
            switch (stampsResellerType)
            {
                case StampsResellerType.None:
                case StampsResellerType.StampsExpedited:
                    return new StampsWebClient(GetAccountRepository(stampsResellerType), new LogEntryFactory(), new TrustingCertificateInspector(), stampsResellerType);

                case StampsResellerType.Express1:
                    return new Express1StampsWebClient(GetAccountRepository(stampsResellerType), new LogEntryFactory(), new TrustingCertificateInspector());
                default:
                    throw new ArgumentOutOfRangeException("stampsResellerType");
            }
        }

        private IStampsWebClient GetWebClient(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

            IStampsWebClient webClient = null;
            if (shipmentType.ShipmentTypeCode == global::ShipWorks.Shipping.ShipmentTypeCode.Stamps)
            {
                webClient = GetWebClient(StampsResellerType.None);
            }
            else if (shipmentType.ShipmentTypeCode == global::ShipWorks.Shipping.ShipmentTypeCode.Usps)
            {
                webClient = GetWebClient(StampsResellerType.StampsExpedited);
            }
            else
            {
                webClient = GetWebClient(StampsResellerType.Express1);
            }

            return webClient;
        }

        private ICarrierAccountRepository<UspsAccountEntity> GetAccountRepository(StampsResellerType stampsResellerType)
        {
            switch (stampsResellerType)
            {
                case StampsResellerType.None:
                    return new StampsAccountRepository();
                case StampsResellerType.Express1:
                    return new Express1StampsAccountRepository();
                case StampsResellerType.StampsExpedited:
                    return new UspsAccountRepository();
                default:
                    throw new ArgumentOutOfRangeException("stampsResellerType");
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

            shipment.Postal.Usps.ScanFormBatchID = null;
            shipment.Postal.Usps.Memo = Memo;
            shipment.Postal.Usps.RequireFullAddressValidation = Convert.ToInt16(RequireFullAddressValidation) == 1;
            shipment.Postal.Usps.HidePostage = Convert.ToInt16(HidePostage) == 1;
            shipment.Postal.Usps.UspsAccountID = Convert.ToInt16(StampsAccountID);

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
            IStampsWebClient webClient = GetWebClient(shipment);

            webClient.VoidShipment(shipment);
        }

        public void PurchasePostage(decimal amount)
        {
            ShipmentEntity shipment = CreateShipment();
            UspsAccountEntity uspsAccount = StampsAccountManager.GetAccount(shipment.Postal.Usps.UspsAccountID);
            StampsPostageWebClient stampsPostageWebClient = new StampsPostageWebClient(uspsAccount);

            stampsPostageWebClient.Purchase(amount);
        }

        public decimal CheckPostage()
        {
            ShipmentEntity shipment = CreateShipment();
            UspsAccountEntity uspsAccount = StampsAccountManager.GetAccount(shipment.Postal.Usps.UspsAccountID);
            StampsPostageWebClient stampsPostageWebClient = new StampsPostageWebClient(uspsAccount);

            return stampsPostageWebClient.GetBalance();
        }
    }
}
