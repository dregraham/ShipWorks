using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.UI.ValueConverters;
using Xunit;

namespace ShipWorks.UI.Tests.ValueConverters
{
    public class UpsEmailNotificationTypeToBoolConverterTest
    {
        private readonly UpsEmailNotificationTypeToBoolConverter testObject;
        public UpsEmailNotificationTypeToBoolConverterTest()
        {
            testObject = new UpsEmailNotificationTypeToBoolConverter();
        } 
        
        [Theory]
        [InlineData(UpsEmailNotificationType.Ship, UpsEmailNotificationType.Ship)]
        [InlineData(UpsEmailNotificationType.Ship | UpsEmailNotificationType.Exception, UpsEmailNotificationType.Ship)]
        [InlineData(UpsEmailNotificationType.Ship | UpsEmailNotificationType.Exception | UpsEmailNotificationType.Deliver, UpsEmailNotificationType.Ship)]
        public void Convert_ReturnsTrue_WhenCurrentFlagContainsParameterFlag(UpsEmailNotificationType currentFlagValue, UpsEmailNotificationType parameterFlag)
        {
            Assert.True((bool) testObject.Convert(currentFlagValue, null, parameterFlag, null));
        }
        
        [Theory]
        [InlineData(UpsEmailNotificationType.Exception, UpsEmailNotificationType.Ship)]
        [InlineData(UpsEmailNotificationType.Exception | UpsEmailNotificationType.Deliver, UpsEmailNotificationType.Ship)]
        public void Convert_ReturnsFalse_WhenCurrentFlagDoesNotContainParameterFlag(UpsEmailNotificationType currentFlagValue, UpsEmailNotificationType parameterFlag)
        {
            Assert.False((bool) testObject.Convert(currentFlagValue, null, parameterFlag, null));
        }
        
        [Fact]
        public void Convert_ReturnsFalse_WhenCurrentFlagIsNull()
        {
            Assert.False((bool) testObject.Convert(null, null, UpsEmailNotificationType.Ship, null));
        }
        
        [Fact]
        public void Convert_ReturnsFalse_WhenParameterFlagIsNull()
        {
            Assert.False((bool) testObject.Convert(UpsEmailNotificationType.Ship, null, null, null));   
        }
        
        [Fact]
        public void Convert_ReturnsFalse_WhenCurrentFlagIsNotUpsEmailNotificationType()
        {
            Assert.False((bool) testObject.Convert(1, null, UpsEmailNotificationType.Ship, null));
        }
        
        [Fact]
        public void Convert_ReturnsFalse_WhenParameterFlagIsNotUpsEmailNotificationType()
        {
            Assert.False((bool) testObject.Convert(UpsEmailNotificationType.Ship, null, 1, null));   
        }
        
        [Fact]
        public void ConvertBack_ReturnsParameterFlag()
        {
            Assert.Equal(UpsEmailNotificationType.Ship, testObject.ConvertBack(null, null, UpsEmailNotificationType.Ship, null));
        }
    }
}