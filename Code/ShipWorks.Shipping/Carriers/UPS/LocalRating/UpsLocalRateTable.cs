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
            rateTableEntity = localRateTableRepo.Get(upsAccount);
        }

        /// <summary>
        /// Save the rate table
        /// </summary>
        public void Save(UpsAccountEntity accountEntity)
        {
            rateTableEntity.UploadDate = DateTime.UtcNow;
            localRateTableRepo.Save(rateTableEntity, accountEntity);
        }

        public void AddRates(IEnumerable<UpsPackageRateEntity> packageRates,
            IEnumerable<UpsLetterRateEntity> letterRates,
            IEnumerable<UpsPricePerPoundEntity> pricesPerPound)
        {
            IEnumerable<UpsPackageRateEntity> packageRateList = packageRates as IList<UpsPackageRateEntity> ?? packageRates.ToList();
            IEnumerable<UpsLetterRateEntity> letterRateList = letterRates as IList<UpsLetterRateEntity> ?? letterRates.ToList();
            IEnumerable<UpsPricePerPoundEntity> pricePerPoundList = pricesPerPound as IList<UpsPricePerPoundEntity> ?? pricesPerPound.ToList();

            importedRateValidator.Validate(packageRateList.Select(r=>r.AsReadOnly()).ToList(),
                letterRateList.Select(r=>r.AsReadOnly()).ToList(),
                pricePerPoundList.Select(r=>r.AsReadOnly()).ToList());

            AddPackageRates(packageRateList);
            AddLetterRates(letterRateList);
            AddPricesPerPound(pricePerPoundList);
        }

        /// <summary>
        /// Add a package rates collection to UpsLocalRateTable
        /// </summary>
        private void AddPackageRates(IEnumerable<UpsPackageRateEntity> rates)
        {
            rateTableEntity.UpsPackageRate.Clear();
            rateTableEntity.UpsPackageRate.AddRange(rates);
        }

        /// <summary>
        /// Add a letter rates collection to UpsLocalRateTable
        /// </summary>
        private void AddLetterRates(IEnumerable<UpsLetterRateEntity> rates)
        {
            rateTableEntity.UpsLetterRate.Clear();
            rateTableEntity.UpsLetterRate.AddRange(rates);
        }

        /// <summary>
        /// Add price per pound collection to UpsLocalRateTable
        /// </summary>
        private void AddPricesPerPound(IEnumerable<UpsPricePerPoundEntity> rates)
        {
            rateTableEntity.UpsPricePerPound.Clear();
            rateTableEntity.UpsPricePerPound.AddRange(rates);
        }

        /// <summary>
        /// Add a surcharge collection to the rate table
        /// </summary>
        public void AddSurcharges(IEnumerable<UpsRateSurchargeEntity> surcharges)
        {
            rateTableEntity.UpsRateSurcharge.Clear();
            rateTableEntity.UpsRateSurcharge.AddRange(surcharges);
        }
    }
}