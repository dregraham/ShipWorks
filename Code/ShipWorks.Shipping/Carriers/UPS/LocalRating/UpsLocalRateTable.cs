using System;
using System.Collections.Generic;
using System.IO;
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
        private UpsRateTableEntity rateTableEntity;

        public DateTime? UploadDate => rateTableEntity?.UploadDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateTable"/> class.
        /// </summary>
        public UpsLocalRateTable(IUpsLocalRateTableRepo localRateTableRepo, IEnumerable<IUpsRateExcelReader> upsRateExcelReaders)
        {
            this.localRateTableRepo = localRateTableRepo;
            this.upsRateExcelReaders = upsRateExcelReaders;

            rateTableEntity = new UpsRateTableEntity();
        }

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

        /// <summary>
        /// Add a rates collection to the rate table
        /// </summary>
        public void AddRates(IEnumerable<UpsPackageRateEntity> rates)
        {
            rateTableEntity.UpsPackageRate.Clear();
            rateTableEntity.UpsPackageRate.AddRange(rates);
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