using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
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
        private readonly IUpsLocalRateTableRepository rateRepository;
        private readonly IEnumerable<IUpsRateExcelReader> upsRateExcelReaders;
        private readonly IUpsImportedRateValidator importedRateValidator;
        private readonly IEnumerable<IUpsZoneExcelReader> zoneExcelReaders;
        private List<UpsPackageRateEntity> packageRates;
        private List<UpsLetterRateEntity> letterRates;
        private List<UpsPricePerPoundEntity> pricePerPound;
        private IEnumerable<UpsRateSurchargeEntity> surcharges;
        private IEnumerable<UpsLocalRatingDeliveryAreaSurchargeEntity> deliveryAreaSurcharges;
        private IEnumerable<UpsLocalRatingZoneEntity> zones;
        private byte[] zoneFileContent;
        private readonly IEnumerable<IServiceFilter> serviceFilters;
        private readonly IUpsSurchargeFactory surchargeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRateTable"/> class.
        /// </summary>
        public UpsLocalRateTable(IUpsLocalRateTableRepository rateRepository,
            IUpsLocalRateExcelReaderFactory excelReaderFactory,
            IUpsImportedRateValidator importedRateValidator,
            IEnumerable<IServiceFilter> serviceFilters,
            IUpsSurchargeFactory surchargeFactory)
        {
            this.surchargeFactory = surchargeFactory;
            this.serviceFilters = serviceFilters;
            this.rateRepository = rateRepository;
            upsRateExcelReaders = excelReaderFactory.CreateRateExcelReaders();
            zoneExcelReaders = excelReaderFactory.CreateZoneExcelReaders();
            this.importedRateValidator = importedRateValidator;
        }

        #region Import Local Rating Data

        /// <summary>
        /// Date of the rates upload
        /// </summary>
        public DateTime? RateUploadDate { get; private set; } = null;

        /// <summary>
        /// Date of the zones upload
        /// </summary>
        public DateTime? ZoneUploadDate { get; private set; } = null;

        /// <summary>
        /// Load the rate table from a stream
        /// </summary>
        public void LoadRates(Stream stream)
        {
            if (stream == null)
            {
                throw new UpsLocalRatingException("Error loading rate file from stream.");
            }

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

            UpsLocalRatingZoneFileEntity zoneFile;
            try
            {
                rateTable = rateRepository.Get(upsAccount);
                zoneFile = rateRepository.GetLatestZoneFile();
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
                zoneFileContent = zoneFile.FileContent;
            }
        }

        /// <summary>
        /// Save the rate table
        /// </summary>
        public void SaveRates(UpsAccountEntity accountEntity)
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
                throw new UpsLocalRatingException($"The selected file does not contain any rates.");
            }

            rateRepository.Save(newRateTable, accountEntity);
            rateRepository.CleanupRates();

            RateUploadDate = newRateTable.UploadDate;
        }

        /// <summary>
        /// Saves the zones.
        /// </summary>
        public void SaveZones()
        {
            UpsLocalRatingZoneFileEntity zoneFile = new UpsLocalRatingZoneFileEntity()
            {
                UploadDate = DateTime.UtcNow
            };

            zoneFile.UpsLocalRatingDeliveryAreaSurcharge.AddRange(deliveryAreaSurcharges);
            zoneFile.UpsLocalRatingZone.AddRange(zones);
            zoneFile.FileContent = zoneFileContent;

            rateRepository.Save(zoneFile);
            rateRepository.CleanupZones();

            ZoneUploadDate = zoneFile.UploadDate;
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
        public void ReplaceSurcharges(IEnumerable<UpsRateSurchargeEntity> newSurcharges)
        {
            surcharges = newSurcharges;
        }

        /// <summary>
        /// Loads the zones from a stream
        /// </summary>
        public void LoadZones(Stream stream)
        {
            if (stream == null)
            {
                throw new UpsLocalRatingException("Error loading zone file from stream.");
            }

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

                stream.Position = 0;
                zoneFileContent = stream.ToArray();
            }
            catch (Exception ex) when (!(ex is UpsLocalRatingException))
            {
                throw new UpsLocalRatingException($"Error loading Excel file.", ex);
            }
        }

        /// <summary>
        /// Replaces the zones.
        /// </summary>
        public void ReplaceZones(IEnumerable<UpsLocalRatingZoneEntity> zones)
        {
            this.zones = zones;
        }

        /// <summary>
        /// Replaces the delivery area surcharges.
        /// </summary>
        public void ReplaceDeliveryAreaSurcharges(IEnumerable<UpsLocalRatingDeliveryAreaSurchargeEntity> localRatingDeliveryAreaSurcharges)
        {
            deliveryAreaSurcharges = localRatingDeliveryAreaSurcharges;
        }

        #endregion Import Local Rating Data

        #region Calculate Rates

        /// <summary>
        /// Calculates shipment rates. Success is true only when rates are found.
        /// </summary>
        public GenericResult<IEnumerable<UpsLocalServiceRate>> CalculateRates(ShipmentEntity shipment)
        {
            try
            {
                IEnumerable<UpsLocalServiceRate> upsLocalServiceRates =
                    rateRepository.GetServiceRates(shipment.Ups).ToList();

                upsLocalServiceRates = ApplyServiceFilters(shipment, upsLocalServiceRates);

                ApplyRateSurcharges(shipment, upsLocalServiceRates);

                return upsLocalServiceRates.Any() ? 
                    GenericResult.FromSuccess(upsLocalServiceRates) : 
                    GenericResult.FromError<IEnumerable<UpsLocalServiceRate>>("No local rates found.");
            }
            catch (UpsLocalRatingException ex)
            {
                return GenericResult.FromError<IEnumerable<UpsLocalServiceRate>>(ex);
            }
        }

        private IEnumerable<UpsLocalServiceRate> ApplyServiceFilters(ShipmentEntity shipment,
            IEnumerable<UpsLocalServiceRate> upsLocalServiceRates)
        {
            IEnumerable<UpsServiceType> eligibleServices = upsLocalServiceRates.Select(r => r.Service);

            foreach (IServiceFilter serviceFilter in serviceFilters)
            {
                eligibleServices = serviceFilter.GetEligibleServices(shipment.Ups, eligibleServices);
            }

            return upsLocalServiceRates.Where(r => eligibleServices.Contains(r.Service));
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        private void ApplyRateSurcharges(ShipmentEntity shipment, IEnumerable<UpsLocalServiceRate> upsLocalServiceRates)
        {
            IDictionary<UpsSurchargeType, double> surchargeAmounts = rateRepository.GetSurcharges(shipment.Ups.UpsAccountID);
            UpsLocalRatingZoneFileEntity zoneFile = rateRepository.GetLatestZoneFile();
            IEnumerable<IUpsSurcharge> upsSurcharges = surchargeFactory.Get(surchargeAmounts, zoneFile);

            foreach (UpsLocalServiceRate serviceRate in upsLocalServiceRates)
            {
                foreach (IUpsSurcharge upsSurcharge in upsSurcharges)
                {
                    upsSurcharge.Apply(shipment.Ups, serviceRate);
                }
            }
        }

        #endregion Calculate Rates
    }
}