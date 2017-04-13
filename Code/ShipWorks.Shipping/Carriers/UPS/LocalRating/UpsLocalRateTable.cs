using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using Syncfusion.XlsIO;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Collection of UpsRates and surcharges that are account specific
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.IUpsLocalRateTable" />
    [Component]
    public class UpsLocalRateTable : IUpsLocalRateTable
    {
        private readonly IUpsLocalRateTableRepository localRateTableRepository;
        private readonly IEnumerable<IUpsRateExcelReader> upsRateExcelReaders;
        private readonly IUpsImportedRateValidator importedRateValidator;

        private List<UpsPackageRateEntity> packageRates;
        private List<UpsLetterRateEntity> letterRates;
        private List<UpsPricePerPoundEntity> pricePerPound;
        private IEnumerable<UpsRateSurchargeEntity> surcharges;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateTable"/> class.
        /// </summary>
        public UpsLocalRateTable(IUpsLocalRateTableRepository localRateTableRepository,
            IEnumerable<IUpsRateExcelReader> upsRateExcelReaders,
            IUpsImportedRateValidator importedRateValidator)
        {
            this.localRateTableRepository = localRateTableRepository;
            this.upsRateExcelReaders = upsRateExcelReaders;
            this.importedRateValidator = importedRateValidator;
        }

        /// <summary>
        /// Date of the upload
        /// </summary>
        public DateTime? UploadDate { get; private set; } = null;

        /// <summary>
        /// Load the rate table from a stream
        /// </summary>
        public void Load(Stream stream)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);

                    foreach (IUpsRateExcelReader excelReader in upsRateExcelReaders)
                    {
                        excelReader.Read(workbook.Worksheets, this);
                    }
                }
            }
            catch (Exception ex) when (!(ex is UpsLocalRatingException))
            {
                throw new UpsLocalRatingException("Error loading Excel file.", ex);
            }
        }

        /// <summary>
        /// Loads the specified ups account.
        /// </summary>
        public void Load(UpsAccountEntity upsAccount)
        {
            UpsRateTableEntity rateTable;

            try
            {
                rateTable = localRateTableRepository.Get(upsAccount);
            }
            catch (Exception e) when (e is ORMException || e is SqlException)
            {
                throw new UpsLocalRatingException(
                    "An error occurred loading the rate table associated with this account");
            }

            if (rateTable != null)
            {
                UploadDate = rateTable.UploadDate;
            }
        }

        /// <summary>
        /// Save the rate table
        /// </summary>
        public void Save(UpsAccountEntity accountEntity)
        {
            // Creating new table so that a ups account can still get the old rates while
            // we save the new rates.
            UpsRateTableEntity newRateTable = new UpsRateTableEntity
            {
                UploadDate = DateTime.UtcNow
            };

            newRateTable.UpsPackageRate.AddRange(packageRates);
            newRateTable.UpsLetterRate.AddRange(letterRates);
            newRateTable.UpsPricePerPound.AddRange(pricePerPound);
            newRateTable.UpsRateSurcharge.AddRange(surcharges);

            localRateTableRepository.Save(newRateTable, accountEntity);
            localRateTableRepository.CleanupRates();

            UploadDate = newRateTable.UploadDate;
        }

        /// <summary>
        /// Replace current rates with rates from the new rate table
        /// </summary>
        public void ReplaceRates(IEnumerable<UpsPackageRateEntity> packageRates,
            IEnumerable<UpsLetterRateEntity> letterRates,
            IEnumerable<UpsPricePerPoundEntity> pricesPerPound)
        {
            List<UpsPackageRateEntity> packageRateList = packageRates.ToList();
            List<UpsLetterRateEntity> letterRateList = letterRates.ToList();
            List<UpsPricePerPoundEntity> pricePerPoundList = pricesPerPound.ToList();

            importedRateValidator.Validate(packageRateList.Select(r => r.AsReadOnly()).ToList(),
                letterRateList.Select(r => r.AsReadOnly()).ToList(),
                pricePerPoundList.Select(r => r.AsReadOnly()).ToList());

            this.packageRates = packageRateList;
            this.letterRates = letterRateList;
            this.pricePerPound = pricePerPoundList;
        }

        /// <summary>
        /// Replace current surcharge collection with surcharges from the new rate table
        /// </summary>
        public void ReplaceSurcharges(IEnumerable<UpsRateSurchargeEntity> surcharges)
        {
            this.surcharges = surcharges;
        }
    }
}