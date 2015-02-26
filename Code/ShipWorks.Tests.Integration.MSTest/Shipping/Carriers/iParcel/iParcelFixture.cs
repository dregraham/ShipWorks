using System;
using System.Data;
using System.IO;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Tests.Integration.MSTest.Fixtures;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.iParcel
{
    public class iParcelFixture : ShipWorksFixtureBase
    {
        public iParcelFixture()
        {
        }

        protected override void AccountManagerInitializeForCurrentUser()
        {
            iParcelAccountManager.InitializeForCurrentSession();
            ShippingOriginManager.InitializeForCurrentSession();
        }

        protected override void InitializeShipmentFields(ShipmentEntity shipment)
        {
        }

        public string TrackByEmail { get; set; }
        public string TrackBySMS { get; set; }
        public string IsDeliveryDutyPaid { get; set; }

        public override bool Ship()
        {
            ShipmentTransactionId = "";
            PackageLineItemWeightUnits = "LBS";
            ShipmentTypeCode = ShipmentTypeCode.iParcel;

            return base.Ship();
        }

        public override decimal GetRates()
        {
            ShipmentTransactionId = "";
            PackageLineItemWeightUnits = "LBS";
            ShipmentTypeCode = ShipmentTypeCode.iParcel;

            return base.GetRates();
        }

        protected override decimal RateShipment(ShipmentEntity shipment)
        {
            iParcelShipmentType iParcelShipmentType = new iParcelShipmentType();
            RateGroup response = iParcelShipmentType.GetRates(shipment);


            return response.Rates.Sum(x => x.Amount);
        }

        protected override void ShipShipment(ShipmentEntity shipment)
        {
            iParcelShipmentType iParcelShipmentType = new iParcelShipmentType();

            // remove the default package that gets created in the ConfigureNewShipment method
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                IParcelPackageEntity packageEntity = shipment.IParcel.Packages[0];
                shipment.IParcel.Packages.RemoveAt(0);
                adapter.DeleteEntity(packageEntity);

                // Save the shipment
                adapter.SaveAndRefetch(shipment);
                adapter.Commit();
            }

            // Default to the Origin country code
            shipment.IParcel.IParcelAccountID = GetFedExAccountId(AccountID, ShipCountryCode);

            shipment.IParcel.Service = (int)GetServiceType();

            DataSet response = iParcelShipmentType.ProcessShipmentAndReturnResponse(shipment);

            decimal responseInsuranceValue = 0m;

            responseInsuranceValue = decimal.Parse(response.Tables["CostInfo"].Rows[0]["PackageInsurance"].ToString());

            if (!string.IsNullOrWhiteSpace(ExpectedInsuranceValue))
            {
                decimal expectedInsuranceValue = decimal.Parse(ExpectedInsuranceValue);
                if (expectedInsuranceValue != responseInsuranceValue)
                {
                    throw new Exception(string.Format("Expected insurance value {0} doesn't equal response insurancevalue {1}",
                        expectedInsuranceValue,
                        responseInsuranceValue));
                }
            }

            shipment.ContentWeight = shipment.IParcel.Packages.Sum(p => p.DimsWeight);
        }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unrecognized service type.</exception>
        private iParcelServiceType GetServiceType()
        {
            switch (Service)
            {
                case "112":
                case "0":
                    return iParcelServiceType.Immediate;
                case "115": return iParcelServiceType.Preferred;
                case "1": return iParcelServiceType.Saver;
                case "2": return iParcelServiceType.SaverDeferred;
            }

            throw new Exception("Unrecognized service type.");
        }

        /// <summary>
        /// Gets a ShipWorks AccountID for a userName
        /// </summary>
        /// <param name="userName"></param>
        protected override long GetAccountId(string userName)
        {
            IParcelAccountEntity accountEntity = iParcelAccountManager.Accounts.FirstOrDefault(x => x.Username == userName);
            if (accountEntity == null)
            {
                throw new InvalidDataException("Test account " + userName + " not found in DB");
            }

            return accountEntity.IParcelAccountID;
        }

        /// <summary>
        /// Gets a ShipWorks AccountID for a userName AND account country code
        /// </summary>
        protected override long GetFedExAccountId(string userName, string countryCode)
        {
            IParcelAccountEntity accountEntity = iParcelAccountManager.Accounts
                .FirstOrDefault(x => x.Username == userName && x.CountryCode.ToUpperInvariant() == countryCode.ToUpperInvariant());

            if (accountEntity == null)
            {
                throw new InvalidDataException(string.Format("Test account {0} with country code {1} not found in DB", userName, countryCode));
            }

            return accountEntity.IParcelAccountID;
        }

        /// <summary>
        /// Adds the customer references.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected override void AddCustomerReferences(ShipmentEntity shipment)
        {
            if (!string.IsNullOrWhiteSpace(ReferenceCustomer))
            {
                shipment.IParcel.Reference = ReferenceCustomer;
            }
            else
            {
                shipment.IParcel.Reference = string.Empty;
            }
        }

        /// <summary>
        /// Sets the package data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected override void SetPackageData(ShipmentEntity shipment)
        {
            for (int i = 0; i < PackageCount; i++)
            {
                IParcelPackageEntity package = new IParcelPackageEntity();
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

                if (!string.IsNullOrEmpty(SkuAndQuantity))
                {
                    package.SkuAndQuantities = SkuAndQuantity;    
                }
                

                shipment.IParcel.Packages.Add(package);
            }
        }

        private void InitializePackage(IParcelPackageEntity package)
        {
            if (package.Weight <= 0)
            {
                package.Weight = package.DimsWeight;
            }

            package.DimsProfileID = 0;
            package.TrackingNumber = string.Empty;
            package.ParcelNumber = string.Empty;


            package.DimsLength = 2;
            package.DimsHeight = 1;
            package.DimsWidth = 1;
            package.DimsWeight = 0;
            package.DimsAddWeight = false;

            package.Insurance = false;
            package.InsuranceValue = 0M;
            package.InsurancePennyOne = false;
            package.DeclaredValue = 0M;

            package.SkuAndQuantities = "1,2";
        }
    }
}
