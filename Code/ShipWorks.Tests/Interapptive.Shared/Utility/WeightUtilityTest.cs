using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using Xunit;

namespace ShipWorks.Tests.Interapptive.Shared.Utility
{
    /// <summary>
    /// Test class to verify that the WeightUtility method(s) work correctly.
    /// </summary>
    public class WeightUtilityTest
    {
        // Verify that a one to one conversion works correctly
        [Fact]
        public void Convert_OneToOne_ConvertsCorrectly_Test()
        {
            // Grams to X
            Assert.Equal(1.0, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Grams, 1.0));
            Assert.Equal(0.001, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Kilograms, 1.0));
            Assert.Equal(0.035274, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Ounces, 1.0));
            Assert.Equal(0.00220462, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Pounds, 1.0));
            Assert.Equal(1e-6, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Tonnes, 1.0));

            // Kilograms to X
            Assert.Equal(1000.0, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Grams, 1.0));
            Assert.Equal(1.0, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Kilograms, 1.0));
            Assert.Equal(35.274, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Ounces, 1.0));
            Assert.Equal(2.20462, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Pounds, 1.0));
            Assert.Equal(0.001, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Tonnes, 1.0));

            // Ounces to X
            Assert.Equal(28.3495, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Grams, 1.0));
            Assert.Equal(0.0283495, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Kilograms, 1.0));
            Assert.Equal(1, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Ounces, 1.0));
            Assert.Equal(0.0625, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Pounds, 1.0));
            Assert.Equal(2.83495e-5, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Tonnes, 1.0));

            // Pounds to X
            Assert.Equal(453.592, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Grams, 1.0));
            Assert.Equal(0.453592, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Kilograms, 1.0));
            Assert.Equal(16, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Ounces, 1.0));
            Assert.Equal(1, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Pounds, 1.0));
            Assert.Equal(0.000453592, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Tonnes, 1.0));

            // Tonnes to X
            Assert.Equal(1e+6, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Grams, 1.0));
            Assert.Equal(1000, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Kilograms, 1.0));
            Assert.Equal(35274, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Ounces, 1.0));
            Assert.Equal(2204.62, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Pounds, 1.0));
            Assert.Equal(1, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Tonnes, 1.0));
        }

        // Verify no rounding errors occur for a double greater than 1.0
        [Fact]
        public void Convert_MultipleOfDoubleGreaterThan1_ConvertsCorrectly_Test()
        {
            double multiplier = 3.291773;

            // Grams to X
            Assert.Equal(multiplier * 1.0, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Grams, multiplier * 1.0));
            Assert.Equal(multiplier * 0.001, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Kilograms, multiplier * 1.0));
            Assert.Equal(multiplier * 0.035274, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Ounces, multiplier * 1.0));
            Assert.Equal(multiplier * 0.00220462, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Pounds, multiplier * 1.0));
            Assert.Equal(multiplier * 1e-6, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Tonnes, multiplier * 1.0));

            // Kilograms to X
            Assert.Equal(multiplier * 1000.0, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Grams, multiplier * 1.0));
            Assert.Equal(multiplier * 1.0, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Kilograms, multiplier * 1.0));
            Assert.Equal(multiplier * 35.274, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Ounces, multiplier * 1.0));
            Assert.Equal(multiplier * 2.20462, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Pounds, multiplier * 1.0));
            Assert.Equal(multiplier * 0.001, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Tonnes, multiplier * 1.0));

            // Ounces to X
            Assert.Equal(multiplier * 28.3495, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Grams, multiplier * 1.0));
            Assert.Equal(multiplier * 0.0283495, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Kilograms, multiplier * 1.0));
            Assert.Equal(multiplier * 1, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Ounces, multiplier * 1.0));
            Assert.Equal(multiplier * 0.0625, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Pounds, multiplier * 1.0));
            Assert.Equal(multiplier * 2.83495e-5, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Tonnes, multiplier * 1.0));

            // Pounds to X
            Assert.Equal(multiplier * 453.592, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Grams, multiplier * 1.0));
            Assert.Equal(multiplier * 0.453592, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Kilograms, multiplier * 1.0));
            Assert.Equal(multiplier * 16, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Ounces, multiplier * 1.0));
            Assert.Equal(multiplier * 1, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Pounds, multiplier * 1.0));
            Assert.Equal(multiplier * 0.000453592, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Tonnes, multiplier * 1.0));

            // Tonnes to X
            Assert.Equal(multiplier * 1e+6, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Grams, multiplier * 1.0));
            Assert.Equal(multiplier * 1000, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Kilograms, multiplier * 1.0));
            Assert.Equal(multiplier * 35274, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Ounces, multiplier * 1.0));
            Assert.Equal(multiplier * 2204.62, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Pounds, multiplier * 1.0));
            Assert.Equal(multiplier * 1, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Tonnes, multiplier * 1.0));
        }

        // Verify no rounding errors occur for a double less than 1.0
        [Fact]
        public void Convert_MultipleOfDoubleLessThan1_ConvertsCorrectly_Test()
        {
            double multiplier = 0.137529;

            // Grams to X
            Assert.Equal(multiplier * 1.0, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Grams, multiplier * 1.0));
            Assert.Equal(multiplier * 0.001, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Kilograms, multiplier * 1.0));
            Assert.Equal(multiplier * 0.035274, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Ounces, multiplier * 1.0));
            Assert.Equal(multiplier * 0.00220462, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Pounds, multiplier * 1.0));
            Assert.Equal(multiplier * 1e-6, WeightUtility.Convert(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Tonnes, multiplier * 1.0));

            // Kilograms to X
            Assert.Equal(multiplier * 1000.0, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Grams, multiplier * 1.0));
            Assert.Equal(multiplier * 1.0, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Kilograms, multiplier * 1.0));
            Assert.Equal(multiplier * 35.274, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Ounces, multiplier * 1.0));
            Assert.Equal(multiplier * 2.20462, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Pounds, multiplier * 1.0));
            Assert.Equal(multiplier * 0.001, WeightUtility.Convert(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Tonnes, multiplier * 1.0));

            // Ounces to X
            Assert.Equal(multiplier * 28.3495, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Grams, multiplier * 1.0));
            Assert.Equal(multiplier * 0.0283495, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Kilograms, multiplier * 1.0));
            Assert.Equal(multiplier * 1, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Ounces, multiplier * 1.0));
            Assert.Equal(multiplier * 0.0625, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Pounds, multiplier * 1.0));
            Assert.Equal(multiplier * 2.83495e-5, WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Tonnes, multiplier * 1.0));

            // Pounds to X
            Assert.Equal(multiplier * 453.592, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Grams, multiplier * 1.0));
            Assert.Equal(multiplier * 0.453592, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Kilograms, multiplier * 1.0));
            Assert.Equal(multiplier * 16, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Ounces, multiplier * 1.0));
            Assert.Equal(multiplier * 1, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Pounds, multiplier * 1.0));
            Assert.Equal(multiplier * 0.000453592, WeightUtility.Convert(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Tonnes, multiplier * 1.0));

            // Tonnes to X
            Assert.Equal(multiplier * 1e+6, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Grams, multiplier * 1.0));
            Assert.Equal(multiplier * 1000, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Kilograms, multiplier * 1.0));
            Assert.Equal(multiplier * 35274, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Ounces, multiplier * 1.0));
            Assert.Equal(multiplier * 2204.62, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Pounds, multiplier * 1.0));
            Assert.Equal(multiplier * 1, WeightUtility.Convert(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Tonnes, multiplier * 1.0));
        }

    }
}
