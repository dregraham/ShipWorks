using System;
using System.IO;
using System.Linq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping;
using ShipWorks.Tests.Integration.MSTest.Fixtures;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Ups
{
    public class UpsMIFixture : ShipWorksFixtureBase
    {
        public UpsMIFixture()
        {
        }

        protected override void AccountManagerInitializeForCurrentUser()
        {
            UpsAccountManager.InitializeForCurrentSession();
            ShippingOriginManager.InitializeForCurrentSession();
        }

        public override bool Ship()
        {
            ShipmentTransactionId = "";
            PackageLineItemWeightUnits = "LBS";
            ShipmentTypeCode = ShipmentTypeCode.UpsWorldShip;

            return base.Ship();
        }

        protected override void InitializeShipmentFields(ShipmentEntity shipment)
        {
            if (shipment.Ups == null)
            {
                shipment.Ups = new UpsShipmentEntity();
            }

            WorldShipShipmentType shipmentType = new WorldShipShipmentType();

            // remove the default package that gets created in the ConfigureNewShipment method
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                UpsPackageEntity packageEntity = shipment.Ups.Packages[0];
                shipment.Ups.Packages.RemoveAt(0);
                adapter.DeleteEntity(packageEntity);

                // Save the shipment
                adapter.SaveAndRefetch(shipment);
                adapter.Commit();
            }

            shipmentType.ConfigureNewShipment(shipment);

            if (UspsEndorsement != "NULL")
            {
                shipment.Ups.Endorsement = Int32.Parse(UspsEndorsement);
            }

            shipment.Ups.UpsAccountID = GetAccountId(AccountID);

            shipment.Ups.Service = GetServiceType();
        }
        
        protected override void ShipShipment(ShipmentEntity shipment)
        {
            shipment.Ups.Service = (int) GetServiceType();

            // Default to the Origin country code
            shipment.Ups.UpsAccountID = GetAccountId(AccountID);

            WorldShipShipmentType upsWorldShipShipmentType = new WorldShipShipmentType();
            upsWorldShipShipmentType.ProcessShipment(shipment);

            shipment.ContentWeight = shipment.Ups.Packages.Sum(p => p.DimsWeight);
        }

        /// <summary>
        /// Rating is not available with WorldShip
        /// </summary>
        protected override decimal RateShipment(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
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

                UpsShipmentEntity ups = shipment.Ups;
                UpsServiceType serviceType = (UpsServiceType) ups.Service;
                switch (serviceType)
                {
                    case UpsServiceType.UpsMailInnovationsExpedited:
                        package.PackagingType = (int)UpsPackagingType.BPMFlats;

                        package.Weight = GetPackageWeight(package.Weight, ups.Service, (UpsPackagingType)package.PackagingType);
                        break;
                    case UpsServiceType.UpsMailInnovationsPriority:
                        shipment.Ups.Packages[0].PackagingType = (int)UpsPackagingType.PriorityMail;

                        package.Weight = GetPackageWeight(package.Weight, ups.Service, (UpsPackagingType)package.PackagingType);
                        break;
                    case UpsServiceType.UpsMailInnovationsFirstClass:
                        shipment.Ups.Packages[0].PackagingType = (int)UpsPackagingType.FirstClassMail;

                        package.Weight = GetPackageWeight(package.Weight, ups.Service, (UpsPackagingType)package.PackagingType);
                        break;
                    case UpsServiceType.UpsMailInnovationsIntEconomy:
                        shipment.Ups.Packages[0].PackagingType = (int)UpsPackagingType.BPM;

                        package.Weight = GetPackageWeight(package.Weight, ups.Service, (UpsPackagingType)package.PackagingType);
                        break;
                    case UpsServiceType.UpsMailInnovationsIntPriority:
                        shipment.Ups.Packages[0].PackagingType = (int)UpsPackagingType.Flats;

                        package.Weight = GetPackageWeight(package.Weight, ups.Service, (UpsPackagingType)package.PackagingType);
                        break;
                }

                if (shipment.Ups.Service == (int) UpsServiceType.UpsMailInnovationsFirstClass)
                {
                    shipment.Ups.Packages[0].PackagingType = (int)UpsPackagingType.FirstClassMail;
                }

                shipment.ContentWeight = package.Weight;
                shipment.TotalWeight = package.Weight;

                shipment.BilledWeight = shipment.TotalWeight;
                shipment.BilledType = (int)BilledType.Unknown;

                if (shipment.ShipCountryCode != "US")
                {
                    ShipmentCustomsItemEntity shipmentCustomsItem = new ShipmentCustomsItemEntity();
                    shipmentCustomsItem.CountryOfOrigin = "US";
                    shipmentCustomsItem.Description = "Goods";
                    shipmentCustomsItem.HarmonizedCode = "1tariff";
                    shipmentCustomsItem.NumberOfPieces = 1;
                    shipmentCustomsItem.Quantity = 1;
                    shipmentCustomsItem.ShipmentID = shipment.ShipmentID;
                    shipmentCustomsItem.UnitPriceAmount = package.DeclaredValue;
                    shipmentCustomsItem.UnitValue = package.DeclaredValue;
                    shipmentCustomsItem.Weight = package.Weight;

                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        // Save the shipment
                        adapter.SaveAndRefetch(shipmentCustomsItem);
                        adapter.Commit();
                    }
                }
            }
        }

        private double GetPackageWeight(double currentWeight, int service, UpsPackagingType packageType)
        {
            UpsServicePackageTypeSetting setting = UpsServicePackageTypeSetting.ServicePackageValidationSettings.FirstOrDefault(x => x.ServiceType == (UpsServiceType)service
                                                                                                                                     && x.PackageType == packageType);

            currentWeight = WeightUtility.Convert(WeightUnitOfMeasure.Pounds, setting.WeightUnitOfMeasure, currentWeight);

            if (currentWeight < setting.MinimumWeight || currentWeight > setting.MaximumWeight)
            {
                currentWeight = (setting.MinimumWeight + setting.MaximumWeight) / 2;
            }

            currentWeight = WeightUtility.Convert(setting.WeightUnitOfMeasure, WeightUnitOfMeasure.Pounds, currentWeight);

            return currentWeight;
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
