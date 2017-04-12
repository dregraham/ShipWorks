using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly IUpsLocalRateTableRepo localRateTableRepo;
        private readonly IEnumerable<IUpsRateExcelReader> upsRateExcelReaders;
        private readonly IUpsImportedRateValidator importedRateValidator;
        private UpsRateTableEntity rateTableEntity;

        private List<UpsPackageRateEntity> newPackageRates;
        private List<UpsLetterRateEntity> newLetterRates;
        private List<UpsPricePerPoundEntity> newPricesPerPound;
        private List<UpsRateSurchargeEntity> newSurcharges;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateTable"/> class.
        /// </summary>
        public UpsLocalRateTable(IUpsLocalRateTableRepo localRateTableRepo, 
            IEnumerable<IUpsRateExcelReader> upsRateExcelReaders,
            IUpsImportedRateValidator importedRateValidator)
        {
            this.localRateTableRepo = localRateTableRepo;
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
            UpsRateTableEntity table = localRateTableRepo.Get(upsAccount);

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
            UpsRateTableEntity newRateTable = new UpsRateTableEntity
            {
                UploadDate = DateTime.UtcNow
            };

            newRateTable.UpsPackageRate.AddRange(newPackageRates);
            newRateTable.UpsLetterRate.AddRange(newLetterRates);
            newRateTable.UpsPricePerPound.AddRange(newPricesPerPound);
            newRateTable.UpsRateSurcharge.AddRange(newSurcharges);

            localRateTableRepo.Save(newRateTable, accountEntity);
            localRateTableRepo.CleanupRates();

            rateTableEntity = newRateTable;
        }

        /// <summary>
        /// Adds the rates to the rate tables
        /// </summary>
        public void AddRates(IEnumerable<UpsPackageRateEntity> packageRates,
            IEnumerable<UpsLetterRateEntity> letterRates,
            IEnumerable<UpsPricePerPoundEntity> pricesPerPound)
        {
            List<UpsPackageRateEntity> packageRateList = packageRates.ToList();
            List<UpsLetterRateEntity> letterRateList = letterRates.ToList();
            List<UpsPricePerPoundEntity> pricePerPoundList = pricesPerPound.ToList();

            importedRateValidator.Validate(packageRateList.Select(r=>r.AsReadOnly()).ToList(),
                letterRateList.Select(r=>r.AsReadOnly()).ToList(),
                pricePerPoundList.Select(r=>r.AsReadOnly()).ToList());

            newPackageRates = packageRateList;
            newLetterRates = letterRateList;
            newPricesPerPound = pricePerPoundList;
        }

        /// <summary>
        /// Add a surcharge collection to the rate table
        /// </summary>
        public void AddSurcharges(IEnumerable<UpsRateSurchargeEntity> surcharges)
        {
            newSurcharges = surcharges.ToList();
        }
    }
}