using System;
using System.Data;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Editing;
using fit;
using Interapptive.Shared.Business;
using ShipWorks.Users.Security;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Tests.Integration.Fitnesse
{
    public class iParcelFixture : ShipWorksFixtureBase
    {

        public iParcelFixture()
        {
        }

        public override void AccountManagerInitializeForCurrentUser()
        {
            iParcelAccountManager.InitializeForCurrentUser();
            ShippingOriginManager.InitializeForCurrentUser();
        }

        public string TrackByEmail { get; set; }
        public string TrackBySMS { get; set; }
        public string IsDeliveryDutyPaid { get; set; }

        public override bool Ship()
        {
            ShipmentTransactionId = "";
            PackageLineItemWeightUnits = "LBS";
            ShipmentTypeCode = ShipmentTypeCode.iParcel;

            base.RunShipShipment = ShipShipmentProcessor;

            return base.Ship();
        }

        public override decimal GetRates()
        {
            ShipmentTransactionId = "";
            PackageLineItemWeightUnits = "LBS";
            ShipmentTypeCode = ShipmentTypeCode.iParcel;

            base.RunRateShipment = GetRates;

            return base.GetRates();
        }

        public decimal GetRates(ShipmentEntity shipment)
        {
            iParcelShipmentType iParcelShipmentType = new iParcelShipmentType();
            RateGroup response = iParcelShipmentType.GetRates(shipment);


            return response.Rates.Sum(x => x.Amount);
        }

        public void ShipShipmentProcessor(ShipmentEntity shipment)
        {
            iParcelShipmentType iParcelShipmentType = new iParcelShipmentType();
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


            package.DimsLength = 0;
            package.DimsHeight = 0;
            package.DimsWidth = 0;
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
