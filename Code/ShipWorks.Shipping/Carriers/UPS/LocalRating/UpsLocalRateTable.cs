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
        private readonly IEnumerable<IUpsZoneExcelReader> zoneExcelReaders;
        private List<UpsPackageRateEntity> packageRates;
        private List<UpsLetterRateEntity> letterRates;
        private List<UpsPricePerPoundEntity> pricePerPound;
        private IEnumerable<UpsRateSurchargeEntity> surcharges;
        private IEnumerable<UpsLocalRatingDeliveryAreaSurchargeEntity> deliveryAreaSurcharges;
        private string rateFileName;
        private string zoneFileName;
        private UpsLocalRatingZoneFileEntity zoneFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateTable"/> class.
        /// </summary>
        public UpsLocalRateTable(IUpsLocalRateTableRepository localRateTableRepository,
            IEnumerable<IUpsRateExcelReader> upsRateExcelReaders,
            IEnumerable<IUpsZoneExcelReader> zoneExcelReaders,
            IUpsImportedRateValidator importedRateValidator)
        {
            this.localRateTableRepository = localRateTableRepository;
            this.upsRateExcelReaders = upsRateExcelReaders;
            this.zoneExcelReaders = zoneExcelReaders;
            this.importedRateValidator = importedRateValidator;

            zoneFile = new UpsLocalRatingZoneFileEntity();
        }

        /// <summary>
        /// Date of the upload
        /// </summary>
        public DateTime? RateUploadDate { get; private set; } = null;

        public DateTime? ZoneUploadDate { get; private set; } = null;

        /// <summary>
        /// Load the rate table from a stream
        /// </summary>
        public void LoadRates(Stream stream)
        {
            rateFileName = (stream as FileStream)?.Name;

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
                throw new UpsLocalRatingException($"Error loading Excel file '{rateFileName}'.", ex);
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
                zoneFile = localRateTableRepository.GetLatestZoneFile();
            }
            catch (Exception e) when (e is ORMException || e is SqlException)
            {
                throw new UpsLocalRatingException(
                    "An error occurred loading the rate table associated with this account");
            }

            if (rateTable != null)
            {
                RateUploadDate = rateTable.UploadDate;
            }

            if (zoneFile != null)
            {
                ZoneUploadDate = zoneFile.UploadDate;
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

            // Throw if the selected file does not contain any rate information
            if (!newRateTable.UpsPackageRate.Any() && !newRateTable.UpsLetterRate.Any() &&
                !newRateTable.UpsPricePerPound.Any() && !newRateTable.UpsRateSurcharge.Any())
            {
                throw new UpsLocalRatingException($"The selected file '{rateFileName}' does not contain any rates.");
            }

            localRateTableRepository.Save(newRateTable, accountEntity);
            localRateTableRepository.CleanupRates();

            RateUploadDate = newRateTable.UploadDate;
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

        /// <summary>
        /// Loads the zones from a stream
        /// </summary>
        public void LoadZones(Stream stream)
        {
            zoneFileName = (stream as FileStream)?.Name;

            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);

                    foreach (IUpsZoneExcelReader excelReader in zoneExcelReaders)
                    {
                        excelReader.Read(workbook.Worksheets, this);
                    }
                }
            }
            catch (Exception ex) when (!(ex is UpsLocalRatingException))
            {
                throw new UpsLocalRatingException($"Error loading Excel file '{zoneFileName}'.", ex);
            }
        }

        /// <summary>
        /// Replaces the zones.
        /// </summary>
        public void ReplaceZones(IEnumerable<UpsLocalRatingZoneEntity> zones)
        {
            List<UpsLocalRatingZoneEntity> zoneList = zones.ToList();

            foreach (UpsLocalRatingZoneEntity zoneEntity in zoneList)
            {
                zoneFile.UpsLocalRatingZone.Add(zoneEntity);
            }
        }

        /// <summary>
        /// Replaces the delivery area surcharges.
        /// </summary>
        public void ReplaceDeliveryAreaSurcharges(IEnumerable<UpsLocalRatingDeliveryAreaSurchargeEntity> localRatingDeliveryAreaSurcharges)
        {
            deliveryAreaSurcharges = localRatingDeliveryAreaSurcharges;
        }

        /// <summary>
        /// Writes the zones to stream.
        /// </summary>
        public void WriteZonesToStream(Stream stream)
        {
            stream.Write(zoneFile.FileContent, 0, zoneFile.FileContent.Length);
        }
    }
}