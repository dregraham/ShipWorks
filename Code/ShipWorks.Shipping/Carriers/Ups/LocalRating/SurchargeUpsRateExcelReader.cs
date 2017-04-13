using System.Collections.Generic;
using System.Linq;
using Syncfusion.XlsIO;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using System.Globalization;
using Interapptive.Shared.Collections;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Reads the surcharges from the worksheet and adds them to the rate table
    /// </summary>
    public class SurchargeUpsRateExcelReader : IUpsRateExcelReader
    {
        private readonly IEnumerable<EnumEntry<UpsSurchargeType>> surchargeTypeMap;

        /// <summary>
        /// Constructor
        /// </summary>
        public SurchargeUpsRateExcelReader()
        {
            surchargeTypeMap = EnumHelper.GetEnumList<UpsSurchargeType>();
        }

        /// <summary>
        /// the numbers styles that we support when parsing the surcharge amount
        /// </summary>
        private static NumberStyles NumberStyles =>
            NumberStyles.AllowLeadingWhite |
            NumberStyles.AllowTrailingWhite |
            NumberStyles.AllowLeadingSign |
            NumberStyles.AllowTrailingSign |
            NumberStyles.AllowCurrencySymbol |
            NumberStyles.Integer |
            NumberStyles.AllowDecimalPoint |
            NumberStyles.AllowThousands;

        /// <summary>
        /// Read the Value Add and Surcharges from the worksheet
        /// </summary>
        public void Read(IWorksheets rateWorkSheets, IUpsLocalRateTable upsLocalRateTable)
        {
            List<UpsRateSurchargeEntity> result = new List<UpsRateSurchargeEntity>();

            IWorksheet valueAddSheet = rateWorkSheets["Value Add"];
            if (valueAddSheet != null)
            {
                result.AddRange(GetSurcharges(valueAddSheet.Rows));
            }

            IWorksheet surchargesSheet = rateWorkSheets["Surcharges"];
            if (surchargesSheet != null)
            {
                result.AddRange(GetSurcharges(surchargesSheet.Rows));
            }
            
            upsLocalRateTable.AddSurcharges(result);
        }

        /// <summary>
        /// Get all of the surcharges from the collection of rows
        /// </summary>
        private IEnumerable<UpsRateSurchargeEntity> GetSurcharges(IRange[] rows)
        {
            IList<UpsRateSurchargeEntity> result = new List<UpsRateSurchargeEntity>();
            rows.ToList().Where(IsSurchargeRow).ForEach(r => AddRowToSurcharges(r, result));

            // check the results before returning
            ValidateResult(result);

            return result;
        }
        
        /// <summary>
        /// Parshe the surcharge from the row and add it to the list of surcharges
        /// </summary>
        private void AddRowToSurcharges(IRange row, IList<UpsRateSurchargeEntity> surcharges)
        {
            // try to parse the surcharge type
            UpsSurchargeType surcharge = GetSurchargType(row.Cells[0].Value);

            double amount;
            if (double.TryParse(row.Cells[1].Value, NumberStyles, CultureInfo.CurrentCulture, out amount))
            {
                surcharges.Add(new UpsRateSurchargeEntity { SurchargeType = (int)surcharge, Amount = amount });
            }
            else
            {
                throw new UpsLocalRatingException($"The rate for {row.Cells[0].Value} is invalid '{row.Cells[1].Value}'.");
            }
        }

        /// <summary>
        /// Check to see if the row is of the correct length and not a header
        /// </summary>
        private static bool IsSurchargeRow(IRange row)
        {
            if (row.Cells.Length >= 2)
            {
                string name = row.Cells[0].Value;
                return !string.IsNullOrEmpty(name) && name != "Value Added Service" && name != "Surcharge";
            }

            return false;
        }

        /// <summary>
        /// Check to see if there are any duplicate SurchargeTypes if so throw
        /// </summary>
        private static void ValidateResult(IEnumerable<UpsRateSurchargeEntity> result)
        {
            IEnumerable<int> duplicates = result.GroupBy(x => x.SurchargeType)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicates.Any())
            {
                throw new UpsLocalRatingException(
                    $"The surcharge {EnumHelper.GetDescription((UpsSurchargeType) duplicates.First())} was specified more than once.");
            }
        }

        /// <summary>
        /// Get the surcharge type from the string
        /// </summary>
        /// <exception cref="UpsLocalRatingException">When the surcharge type is unknown</exception>
        private UpsSurchargeType GetSurchargType(string name)
        {
            IEnumerable<EnumEntry<UpsSurchargeType>> surcharge =
                surchargeTypeMap.Where(e => e.Description == name).ToList();
            
            if (surcharge.Count() != 1)
            {
                throw new UpsLocalRatingException($"Unknown Surcharge or Value Add {name}");
            }

            return surcharge.First().Value;
        }
    }
}
