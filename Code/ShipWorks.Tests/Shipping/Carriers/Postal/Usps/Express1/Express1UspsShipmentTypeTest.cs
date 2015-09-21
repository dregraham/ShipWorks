using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Express1
{
    public class Express1UspsShipmentTypeTest
    {
        private Express1UspsShipmentType testObject;

        private List<PostalServicePackagingCombination> allCombinations = new List<PostalServicePackagingCombination>();
        private List<PostalServicePackagingCombination> adultSignatureCombinationsAllowed = new List<PostalServicePackagingCombination>();

        public Express1UspsShipmentTypeTest()
        {
            testObject = new Express1UspsShipmentType();

            LoadAdultSignatureServiceAndPackagingCombinations();

            LoadAllPostalServicePackageTypes();
        }

        [Fact]
        public void AdultSignatureRequred_IsReturned_WhenUspsShipmentTypeAndUsingAllowedCombinations()
        {
            foreach (PostalServicePackagingCombination combo in adultSignatureCombinationsAllowed)
            {
                Assert.Contains(PostalConfirmationType.AdultSignatureRequired, testObject.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType));
                //Assert.True(testObject.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType).Any(ct => ct == PostalConfirmationType.AdultSignatureRequired), "{0}, {1} should be included in the allowed confirmation types", combo.ServiceType, combo.PackagingType);
            }
        }

        [Fact]
        public void AdultSignatureRestricted_IsReturned_WhenUspsShipmentTypeAndUsingAllowedCombinations()
        {
            foreach (PostalServicePackagingCombination combo in adultSignatureCombinationsAllowed)
            {
                Assert.Contains(PostalConfirmationType.AdultSignatureRestricted, testObject.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType));
                //Assert.True(testObject.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType).Any(ct => ct == PostalConfirmationType.AdultSignatureRestricted), "{0}, {1} should be included in the allowed confirmation types", combo.ServiceType, combo.PackagingType);
            }
        }

        [Fact]
        public void AdultSignatureRequred_IsNotReturned_WhenUspsShipmentTypeAndUsingCombinationsThatAreNotAllowed()
        {
            foreach (PostalServicePackagingCombination combo in allCombinations.Except(adultSignatureCombinationsAllowed))
            {
                List<PostalConfirmationType> returnedConfirmationTypes = testObject.GetAvailableConfirmationTypes("US", combo.ServiceType, combo.PackagingType);

                Assert.DoesNotContain(PostalConfirmationType.AdultSignatureRequired, returnedConfirmationTypes);
                //Assert.True(returnedConfirmationTypes.All(ct => ct != PostalConfirmationType.AdultSignatureRequired), "AdultSignatureRequired should not have been returned for {0}, {1}.", combo.ServiceType, combo.PackagingType);
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

        /// <summary>
        /// Add adult signature restricted values
        /// </summary>
        private void LoadAdultSignatureServiceAndPackagingCombinations()
        {
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.LargeEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.Package));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateSmallBox));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateMediumBox));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateLargeBox));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRatePaddedEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.FlatRateLegalEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.RateRegionalBoxA));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.RateRegionalBoxB));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.PriorityMail, PostalPackagingType.RateRegionalBoxC));

            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.LargeEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.Package));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRateEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRateMediumBox));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRatePaddedEnvelope));
            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ExpressMail, PostalPackagingType.FlatRateLegalEnvelope));

            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.ParcelSelect, PostalPackagingType.Package));

            adultSignatureCombinationsAllowed.Add(new PostalServicePackagingCombination(PostalServiceType.CriticalMail, PostalPackagingType.LargeEnvelope));
        }
    }
}
