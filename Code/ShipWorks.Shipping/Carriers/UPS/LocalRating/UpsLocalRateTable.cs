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
        private UpsRateTableEntity rateTableEntity;

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

            rateTableEntity = new UpsRateTableEntity();
        }

        /// <summary>
        /// Date of the upload
        /// </summary>
        public DateTime? UploadDate => rateTableEntity?.UploadDate;

        /// <summary>
        /// Load the rate table from a stream
        /// </summary>
        public void Load(Stream stream)
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

        /// <summary>
        /// Loads the specified ups account.
        /// </summary>
        public void Load(UpsAccountEntity upsAccount)
        {
            UpsRateTableEntity table;

            try
            {
                table = localRateTableRepository.Get(upsAccount);
            }
            catch (Exception e) when (e is ORMException || e is SqlException)
            {
                throw new UpsLocalRatingException(
                    "An error occured loading the rate table associated with this account");
            }

            if (table != null)
            {
                rateTableEntity = table;
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

            newRateTable.UpsPackageRate.AddRange(rateTableEntity.UpsPackageRate.ToList());
            newRateTable.UpsLetterRate.AddRange(rateTableEntity.UpsLetterRate.ToList());
            newRateTable.UpsPricePerPound.AddRange(rateTableEntity.UpsPricePerPound.ToList());
            newRateTable.UpsRateSurcharge.AddRange(rateTableEntity.UpsRateSurcharge.ToList());

            localRateTableRepository.Save(newRateTable, accountEntity);
            localRateTableRepository.CleanupRates();

            rateTableEntity = newRateTable;
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

            rateTableEntity.UpsPackageRate.Clear();
            rateTableEntity.UpsPackageRate.AddRange(packageRateList);

            rateTableEntity.UpsLetterRate.Clear();
            rateTableEntity.UpsLetterRate.AddRange(letterRateList);


            rateTableEntity.UpsPricePerPound.Clear();
            rateTableEntity.UpsPricePerPound.AddRange(pricePerPoundList);
        }

        /// <summary>
        /// Replace current surcharge collection with surcharges from the new rate table
        /// </summary>
        public void ReplaceSurcharges(IEnumerable<UpsRateSurchargeEntity> surcharges)
        {
            rateTableEntity.UpsRateSurcharge.Clear();
            rateTableEntity.UpsRateSurcharge.AddRange(surcharges);
        }
    }
}