using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    [TestClass]
    public class Express1UspsShipmentTypeTest
    {
        private List<PostalServicePackagingCombination> allCombinations = new List<PostalServicePackagingCombination>();

        [TestInitialize]
        public void Initialize()
        {
            LoadAllPostalServicePackageTypes();
        }

        [TestMethod]
        public void AdultSignatureRequred_IsNotReturned_WhenExpress1UspsShipmentTypeAndUsingCombinationsThatAreNotAllowed()
        {
            Express1UspsShipmentType express1UspsShipmentType = new Express1UspsShipmentType();

            // No combination is allowed, so use all combinations.
            foreach (PostalServicePackagingCombination combo in allCombinations)
            {
                List<PostalConfirmationType> returnedConfirmationTypes = express1UspsShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType);

                Assert.IsTrue(returnedConfirmationTypes.All(ct => ct != PostalConfirmationType.AdultSignatureRequired), "AdultSignatureRequired should not have been returned for {0}, {1}.", combo.ServiceType, combo.PackagingType);
            }
        }

        [TestMethod]
        public void AdultSignatureRestricted_IsNotReturned_WhenExpress1UspsShipmentTypeAndUsingCombinationsThatAreNotAllowed()
        {
            Express1UspsShipmentType express1UspsShipmentType = new Express1UspsShipmentType();

            // No combination is allowed, so use all combinations.
            foreach (PostalServicePackagingCombination combo in allCombinations)
            {
                List<PostalConfirmationType> returnedConfirmationTypes = express1UspsShipmentType.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType);

                Assert.IsTrue(returnedConfirmationTypes.All(ct => ct != PostalConfirmationType.AdultSignatureRestricted), "AdultSignatureRestricted should not have been returned for {0}, {1}.", combo.ServiceType, combo.PackagingType);
            }
        }

        private void LoadAllPostalServicePackageTypes()
        {
            allCombinations.Clear();
            foreach (var postalServiceType in EnumHelper.GetEnumList<PostalServiceType>())
            {
                foreach (var postalPackageType in EnumHelper.GetEnumList<PostalPackagingType>())
                {
                    allCombinations.Add(new PostalServicePackagingCombination(postalServiceType.Value, postalPackageType.Value));
                }
            }
        }
    }
}
