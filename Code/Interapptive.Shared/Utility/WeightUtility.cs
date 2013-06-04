using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Enums;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Helper class for weight manipulations
    /// </summary>
    public static class WeightUtility
    {
        private static readonly List<Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>> conversionMatrix;

        /// <summary>
        /// Static constructor to set up the conversion matrix
        /// </summary>
        static WeightUtility()
        {
            conversionMatrix = new List<Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>>();

            // Grams to X
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Grams, 1.0));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Kilograms, 0.001));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Ounces, 0.035274));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Pounds, 0.00220462));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Grams, WeightUnitOfMeasure.Tonnes, 1.0e-6));

            // Kilograms to X
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Grams, 1000));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Kilograms, 1.0));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Ounces, 35.274));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Pounds, 2.20462));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Kilograms, WeightUnitOfMeasure.Tonnes, 0.001));

            // Ounces to X
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Grams, 28.3495));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Kilograms, 0.0283495));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Ounces, 1.0));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Pounds, 0.0625));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Tonnes, 2.83495e-5));

            // Pounds to X
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Grams, 453.592));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Kilograms, 0.453592));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Ounces, 16.0));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Pounds, 1.0));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Pounds, WeightUnitOfMeasure.Tonnes, 0.000453592));

            // Tonnes to X
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Grams, 1000000.0));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Kilograms, 1000.0));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Ounces, 35274.0));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Pounds, 2204.62));
            conversionMatrix.Add(new Tuple<WeightUnitOfMeasure, WeightUnitOfMeasure, double>(WeightUnitOfMeasure.Tonnes, WeightUnitOfMeasure.Tonnes, 1.0));
        }

        /// <summary>
        /// Converts from one weight unit of measure to another
        /// </summary>
        /// <param name="fromWeightUnitOfMeasure">The weight unit of measure from which you want to convert.</param>
        /// <param name="toWeightUnitOfMeasure">The weight unit of measure to which you want to convert.</param>
        /// <param name="weight">The weight amount to convert.</param>
        /// <returns>The converted weight in toWeightUnitOfMeasure.</returns>
        public static double Convert(WeightUnitOfMeasure fromWeightUnitOfMeasure, WeightUnitOfMeasure toWeightUnitOfMeasure, double weight)
        {
            if (!conversionMatrix.Any(t => t.Item1 == fromWeightUnitOfMeasure))
            {
                throw new InvalidOperationException(string.Format("{0} is not a supported WeightUnitOfMeasure", fromWeightUnitOfMeasure));
            }

            if (!conversionMatrix.Any(t => t.Item2 == toWeightUnitOfMeasure))
            {
                throw new InvalidOperationException(string.Format("{0} is not a supported WeightUnitOfMeasure", toWeightUnitOfMeasure));
            }

            double conversionAmount =
                conversionMatrix.First(t => t.Item1 == fromWeightUnitOfMeasure && t.Item2 == toWeightUnitOfMeasure)
                                .Item3;

            return conversionAmount * weight;
        }

    }
}
