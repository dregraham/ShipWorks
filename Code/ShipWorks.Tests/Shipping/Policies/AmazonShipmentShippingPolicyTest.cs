using System;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Policies;
using Xunit;

namespace ShipWorks.Tests.Shipping.Policies
{
    public class AmazonShipmentShippingPolicyTest
    {
        private AmazonShipmentShippingPolicy testObject;
        private AmazonPrimeShippingPolicyTarget testTarget;

        public AmazonShipmentShippingPolicyTest()
        {
            testObject = new AmazonShipmentShippingPolicy();
            testTarget = new AmazonPrimeShippingPolicyTarget()
            {
                Shipment = new ShipmentEntity()
                {
                    Order = new OrderEntity()
                },
                Allowed = false,
                AmazonCredentials = null,
                AmazonOrder = null
            };
        }

        [Theory]
        [InlineData("0", false, false)]
        [InlineData("1", true, false)]
        [InlineData("2", false, true)]
        [InlineData("3", true, true)]
        public void Configure_SetsValuesCorrectly(string restrictionType, bool expectedPrimeOnlyValue, bool expectedAllValue)
        {
            testObject.Configure(restrictionType);
            Assert.Equal(expectedPrimeOnlyValue, testObject.OnlyAmazonPrimeOrdersAllowed);
            Assert.Equal(expectedAllValue, testObject.AllAmazonOrdersAllowed);
        }

        [Fact]
        public void Configure_ThrowsArgumentException_WhenValueIsNotANumber()
        {
            var ex = Assert.Throws<ArgumentException>(() => testObject.Configure("asdf"));
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenTargetIsNull()
        {
            testObject.Configure("1");

            Assert.False(testObject.IsApplicable(null));
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenTargetIsInt()
        {
            testObject.Configure("1");

            Assert.False(testObject.IsApplicable(3));
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_WhenTargetAmazonOrderIsNull()
        {
            testObject.Configure("1");
            testTarget.AmazonOrder = null;

            Assert.True(testObject.IsApplicable(testTarget));
        }

        [Fact]
        public void IsApplicable_ReturnsTrue_WhenAllAmazonOrdersAllowedIsTrueAndCredentialsIsNull()
        {
            testObject.Configure("2");

            Assert.True(testObject.IsApplicable(testTarget));
        }

        [Fact]
        public void IsApplicable_ReturnsFalse_WhenAllAmazonOrdersAllowedIsTrueAndCredentialsIsNotNull()
        {
            testObject.Configure("2");
            testTarget.AmazonCredentials = new AmazonStoreEntity();

            Assert.True(testObject.IsApplicable(testTarget));
        }

        [Theory]
        [InlineData(AmazonIsPrime.Yes, true, true)]
        [InlineData(AmazonIsPrime.Yes, false, true)]
        [InlineData(AmazonIsPrime.No, true, true)]
        [InlineData(AmazonIsPrime.No, false, true)]
        [InlineData(AmazonIsPrime.Unknown, true, true)]
        [InlineData(AmazonIsPrime.Unknown, false, true)]
        public void IsApplicable_ReturnsCorrectValues_WhenOnlyPrimeAllowed(AmazonIsPrime amazonIsPrime, bool isNullCreds, bool expectedResult)
        {
            testObject.Configure("1");
            testTarget.AmazonOrder = new AmazonOrderEntity()
            {
                IsPrime = (int) amazonIsPrime
            };
            testTarget.AmazonCredentials = isNullCreds ? null : new AmazonStoreEntity();

            Assert.Equal(expectedResult, testObject.IsApplicable(testTarget));
        }

        [Fact]
        public void Apply_ThrowsArgumentNullException_WhenTargetIsNull()
        {
            testObject.Configure("1");
            Assert.Throws<ArgumentNullException>(() => testObject.Apply(null));
        }

        [Theory]
        [InlineData(AmazonIsPrime.Yes, true, false)]
        [InlineData(AmazonIsPrime.Yes, false, true)]
        [InlineData(AmazonIsPrime.No, true, false)]
        [InlineData(AmazonIsPrime.No, false, false)]
        [InlineData(AmazonIsPrime.Unknown, true, false)]
        [InlineData(AmazonIsPrime.Unknown, false, false)]
        public void Apply_ReturnsCorrectValues_WhenOnlyPrimeAllowed(AmazonIsPrime amazonIsPrime, bool isNullCreds, bool expectedResult)
        {
            testObject.Configure("1");
            testTarget.AmazonOrder = new AmazonOrderEntity()
            {
                IsPrime = (int) amazonIsPrime
            };
            testTarget.AmazonCredentials = isNullCreds ? null : new AmazonStoreEntity();
            testObject.Apply(testTarget);

            Assert.Equal(expectedResult, testTarget.Allowed);
        }
    }
}
