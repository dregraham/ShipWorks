using System.Windows.Controls;
using ShipWorks.Shipping.UI.ShippingPanel.Weight;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ValidationRules
{
    public class WeightValidationRuleTest
    {
        [Theory]
        [InlineData("1lbs")]
        [InlineData("1lbs.")]
        [InlineData("1lb")]
        [InlineData("1lb.")]
        [InlineData("1l")]
        [InlineData("1pounds")]
        [InlineData("1pound")]
        [InlineData("1o")]
        [InlineData("1oz")]
        [InlineData("1oz.")]
        [InlineData("1ounce")]
        [InlineData("1ounces")]
        [InlineData("1 lbs")]
        [InlineData("1 lbs.")]
        [InlineData("1 lb")]
        [InlineData("1 lb.")]
        [InlineData("1 l")]
        [InlineData("1 pounds")]
        [InlineData("1 pound")]
        [InlineData("1 o")]
        [InlineData("1 oz")]
        [InlineData("1 oz.")]
        [InlineData("1 ounce")]
        [InlineData("1 ounces")]
        [InlineData("1 pounds 1 ounces")]
        [InlineData("1 lbs 1 oz")]
        [InlineData("1 lbs. 1 oz.")]
        [InlineData("0 lbs. 1 oz.")]
        public void ValidationResult_IsValid_WhenWeightIsValid_Test(string input)
        {
            WeightValidationRule rule = new WeightValidationRule();

            ValidationResult result = rule.Validate(input, null);

            Assert.Equal(true, result.IsValid);
        }

        [Theory]
        [InlineData("adsf")]
        [InlineData("-3oz")]
        [InlineData("1lb -2oz")]
        public void ValidationResult_IsNotValid_WhenWeightIsNotValid_Test(string input)
        {
            WeightValidationRule rule = new WeightValidationRule();

            ValidationResult result = rule.Validate(input, null);

            Assert.Equal(false, result.IsValid);
        }
    }
}
