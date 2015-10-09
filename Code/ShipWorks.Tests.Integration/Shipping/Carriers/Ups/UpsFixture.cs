using System;
using System.IO;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping;
using ShipWorks.Tests.Integration.MSTest.Fixtures;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Ups
{
    public class UpsFixture : ShipWorksFixtureBase
    {
        public string Subclassification;

        public UpsFixture()
        {
        }

        protected override void AccountManagerInitializeForCurrentUser()
        {
            UpsAccountManager.InitializeForCurrentSession();
            ShippingOriginManager.InitializeForCurrentSession();
        }

        public override bool Ship()
        {
            bool retVal = false;

            ShipmentTransactionId = "";
            PackageLineItemWeightUnits = "LBS";
            ShipmentTypeCode = ShipmentTypeCode.UpsOnLineTools;

            retVal = base.Ship();

            return retVal;
        }

        protected override void InitializeShipmentFields(ShipmentEntity shipment)
        {
            if (shipment.Ups == null)
            {
                shipment.Ups = new UpsShipmentEntity();
            }

            UpsOltShipmentType shipmentType = (UpsOltShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode.UpsOnLineTools);

            shipmentType.ConfigureNewShipment(shipment);

            shipment.Ups.Subclassification = (int)UpsPostalSubclassificationType.Irregular;

            if (UspsEndorsement != "NULL")
            {
                shipment.Ups.Endorsement = Int32.Parse(UspsEndorsement);
            }

            shipment.Ups.UpsAccountID = GetAccountId(AccountID);

            shipment.Ups.Service = GetServiceType();
        }

        public override decimal GetRates()
        {
            ShipmentTransactionId = "";
            PackageLineItemWeightUnits = "LBS";
            ShipmentTypeCode = ShipmentTypeCode.UpsOnLineTools;

            return base.GetRates();
        }

        protected override decimal RateShipment(ShipmentEntity shipment)
        {
            UpsOltShipmentType UpsShipmentType = (UpsOltShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode.UpsOnLineTools);
            RateGroup response = UpsShipmentType.GetRates(shipment);

            return response.Rates.Sum(x => x.Amount);
        }

        protected override void ShipShipment(ShipmentEntity shipment)
        {
            shipment.Ups.Service = (int) GetServiceType();

            // Default to the Origin country code
            shipment.Ups.UpsAccountID = GetAccountId(AccountID);

            UpsPostalSubclassificationType subClassification;
            if (UpsPostalSubclassificationType.TryParse(Subclassification, true, out subClassification))
            {
                shipment.Ups.Subclassification = (int) subClassification;
            }
            else
            {
                shipment.Ups.Subclassification = (int)UpsPostalSubclassificationType.Irregular;
            }

            UpsOltShipmentType upsOltShipmentType = (UpsOltShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode.UpsOnLineTools);
            upsOltShipmentType.ProcessShipment(shipment);

            shipment.ContentWeight = shipment.Ups.Packages.Sum(p => p.DimsWeight);
        }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unrecognized service type.</exception>
        private int GetServiceType()
        {
            return int.Parse(Service);
        }

        /// <summary>
        /// Gets a ShipWorks AccountID for an accountNumber
        /// </summary>
        /// <param name="accountNumber"></param>
        protected override long GetAccountId(string accountNumber)
        {
            UpsAccountEntity accountEntity = UpsAccountManager.Accounts.FirstOrDefault(x => x.AccountNumber == accountNumber);
            if (accountEntity == null)
            {
                throw new InvalidDataException("Test account " + accountNumber + " not found in DB");
            }

            return accountEntity.UpsAccountID;
        }

        protected override long GetFedExAccountId(string accountNumber, string countryCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the customer references.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected override void AddCustomerReferences(ShipmentEntity shipment)
        {
            if (!string.IsNullOrWhiteSpace(ReferenceCustomer))
            {
                shipment.Ups.ReferenceNumber = ReferenceCustomer;
            }
            else
            {
                shipment.Ups.ReferenceNumber = string.Empty;
            }
        }

        /// <summary>
        /// Sets the package data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected override void SetPackageData(ShipmentEntity shipment)
        {
            if (shipment.Ups == null)
            { 
                shipment.Ups = new UpsShipmentEntity();
            }

            for (int i = 0; i < PackageCount; i++)
            {
                UpsPackageEntity package;
                if (shipment.Ups.Packages.Count > i)
                {
                    package = shipment.Ups.Packages[i];
                }
                else
                {
                    package = new UpsPackageEntity();
                    shipment.Ups.Packages.Add(package);
                }
                
                InitializePackage(package);

                if (!string.IsNullOrWhiteSpace(InsuranceValuePerPackage))
                {
                    decimal amount = decimal.Parse(InsuranceValuePerPackage);
                    package.InsuranceValue = amount;
                }

                if (!string.IsNullOrWhiteSpace(DeclaredValue))
                {
                    decimal amount = decimal.Parse(DeclaredValue);
                    package.DeclaredValue = amount;
                }

                if (!string.IsNullOrWhiteSpace(InsurancePennyOne))
                {
                    bool insurancePennyOne = bool.Parse(InsurancePennyOne);
                    package.InsurancePennyOne = insurancePennyOne;
                }

                if (!string.IsNullOrWhiteSpace(Insurance))
                {
                    bool insurance = bool.Parse(Insurance);
                    shipment.Insurance = insurance;
                    package.Insurance = insurance;
                }

                if (!string.IsNullOrWhiteSpace(InsuranceProvider))
                {
                    int insuranceProvider = int.Parse(InsuranceProvider);
                    shipment.InsuranceProvider = insuranceProvider;
                }

                package.Weight = string.IsNullOrEmpty(WeightPerPackage) ? 0 : double.Parse(WeightPerPackage);

                if (!string.IsNullOrEmpty(LengthPerPackage))
                {
                    package.DimsLength = double.Parse(LengthPerPackage);
                }

                if (!string.IsNullOrEmpty(HeightPerPackage))
                {
                    package.DimsHeight = double.Parse(HeightPerPackage);
                }

                if (!string.IsNullOrEmpty(WidthPerPackage))
                {
                    package.DimsWidth = double.Parse(WidthPerPackage);
                }
            }
        }

        private void InitializePackage(UpsPackageEntity package)
        {
            if (package.Weight <= 0)
            {
                package.Weight = package.DimsWeight;
            }

            package.DimsProfileID = 0;
            package.TrackingNumber = string.Empty;
            package.UspsTrackingNumber = string.Empty;

            package.DimsLength = 2;
            package.DimsHeight = 1;
            package.DimsWidth = 1;
            package.DimsWeight = 0;
            package.DimsAddWeight = false;

            package.Insurance = false;
            package.InsuranceValue = 0M;
            package.InsurancePennyOne = false;
            package.DeclaredValue = 0M;

            package.PackagingType = (int)UpsPackagingType.Custom;
        }
    }
}
