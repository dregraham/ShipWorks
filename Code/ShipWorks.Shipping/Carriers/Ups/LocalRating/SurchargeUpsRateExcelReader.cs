using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.XlsIO;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Reads the surcharges from the worksheet and adds them to the rate table
    /// </summary>
    public class SurchargeUpsRateExcelReader : IUpsRateExcelReader
    {
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
        private static IEnumerable<UpsRateSurchargeEntity> GetSurcharges(IRange[] rows)
        {
            foreach (IRange row in rows)
            {
                // We need a name/value if this row has less than 2 cells skip it
                if (row.Cells.Length >= 2)
                {
                    string name = row.Cells[0].Value;

                    // Skip the headers
                    if (name != "Value Added Service" && name != "Surcharge")
                    {
                        // try to parse the surcharge type
                        UpsSurchargeType surcharge = GetSurchargType(name);

                        double amount;
                        if (double.TryParse(row.Cells[1].Value, out amount))
                        {
                            yield return new UpsRateSurchargeEntity() { SurchargeType = (int)surcharge, Amount = amount };
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the surcharge type from the string
        /// </summary>
        /// <exception cref="UpsLocalRatingException">When the surcharge type is unknown</exception>
        private static UpsSurchargeType GetSurchargType(string name)
        {
            UpsSurchargeType? surcharge = EnumHelper.TryParseEnum<UpsSurchargeType>(name);

            if (surcharge == null)
            {
                throw new UpsLocalRatingException($"Unknown Surcharge or Value Add {name}");
            }

            return (UpsSurchargeType) surcharge;
        }
    }
}
