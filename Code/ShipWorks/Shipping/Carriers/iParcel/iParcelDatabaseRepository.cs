using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using System.Data;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Settings;
using log4net;
using System.Text;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// An implementation of the IiParcelRepository interface that reads/writes to the ShipWorks database.
    /// </summary>
    public class iParcelDatabaseRepository : IiParcelRepository
    {
        private const string LabelTableName = "LabelInfo";
        private const string Base64ColumnName = "ParcelLabelBase64JPG";
        private const string ThermalColumnName = "ParcelLabelEPL";

        private const string PackageInfoTableName = "PackageInfo";
        private const string CostInfoTableName = "CostInfo";
        private const string TrackingNumberColumnName = "TrackingNumber";
        private const string ParcelNumberColumnName = "ParcelNumber";
        private const string PackageTotalCurrencyColumnName = "PackageTotalCurrency";

        private readonly ILog log = LogManager.GetLogger(typeof(iParcelDatabaseRepository));
        
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelDatabaseRepository" /> class.
        /// </summary>
        public iParcelDatabaseRepository(Func<Type, ILog> logFactory)
        {
            log = logFactory(GetType());
        }


        /// <summary>
        /// Saves the tracking info contained in the data set to the shipment entity.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="iParcelResponse">The i-parcel response.</param>
        public void SaveTrackingInfoToEntity(ShipmentEntity shipment, DataSet iParcelResponse)
        {
            if (iParcelResponse == null || !iParcelResponse.Tables.Contains(PackageInfoTableName))
            {
                throw new iParcelException("Unable to find tracking information in the i-parcel response.");
            }

            // Verify we have a tracking number column
            if (!iParcelResponse.Tables[PackageInfoTableName].Columns.Contains(TrackingNumberColumnName))
            {
                throw new iParcelException("No tracking information was returned for shipment.");
            }

            // Verify we have a parcel number column
            if (!iParcelResponse.Tables[PackageInfoTableName].Columns.Contains(ParcelNumberColumnName))
            {
                throw new iParcelException("No parcel number was returned for shipment.");
            }

            try
            {
                decimal shipmentCost = 0m;

                for (int rowIndex = 0; rowIndex < iParcelResponse.Tables[PackageInfoTableName].Rows.Count; rowIndex++)
                {
                    IParcelPackageEntity packageEntity = shipment.IParcel.Packages[rowIndex];
                    packageEntity.TrackingNumber = iParcelResponse.Tables[PackageInfoTableName].Rows[rowIndex][TrackingNumberColumnName].ToString();
                    packageEntity.ParcelNumber = iParcelResponse.Tables[PackageInfoTableName].Rows[rowIndex][ParcelNumberColumnName].ToString();

                    if (rowIndex == 0)
                    {
                        shipment.TrackingNumber = packageEntity.TrackingNumber;
                    }

                    decimal packageCost = 0;
                    if (iParcelResponse.Tables[CostInfoTableName] != null &&
                        rowIndex <= (iParcelResponse.Tables[CostInfoTableName].Rows.Count - 1) &&
                        iParcelResponse.Tables[CostInfoTableName].Rows[rowIndex][PackageTotalCurrencyColumnName] != null)
                    {
                        decimal.TryParse(iParcelResponse.Tables[CostInfoTableName].Rows[rowIndex][PackageTotalCurrencyColumnName].ToString(), out packageCost);
                    }
                    shipmentCost += packageCost;
                }

                shipment.ShipmentCost = shipmentCost;
            }
            catch (Exception ex)
            {
                string message = string.Format("Error reading i-parcel tracking info: {0}.", ex.Message);
                log.Error(message, ex);

                throw new iParcelException(message, ex);
            }
        }

        /// <summary>
        /// Saves the label(s) contained in the i-parcel response for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="iParcelResponse">The i-parcel response.</param>
        public void SaveLabel(ShipmentEntity shipment, DataSet iParcelResponse)
        {
            if (iParcelResponse == null || !iParcelResponse.Tables.Contains(LabelTableName))
            {
                throw new iParcelException("Unable to find label information in the i-parcel response.");
            }

            // Interapptive users have an unprocess button.  If we are re-processing we need to clear the old images
            ObjectReferenceManager.ClearReferences(shipment.ShipmentID);

            try
            {
                for (int rowIndex = 0; rowIndex < iParcelResponse.Tables[PackageInfoTableName].Rows.Count; rowIndex++)
                {
                    byte[] imageData;

                    if (shipment.ActualLabelFormat.HasValue)
                    {
                        // Extract the thermal label
                        imageData = GetThermalLabel(iParcelResponse, rowIndex);
                    }
                    else
                    {
                        // Extract a standard/laser label (assuming base 64 encoded)
                        imageData = GetImageLabel(iParcelResponse, rowIndex);
                    }

                    DataResourceManager.CreateFromBytes(imageData, shipment.ShipmentID, "LabelPrimary");
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Error reading i-parcel label: {0}.", ex.Message);
                log.Error(message, ex);

                throw new iParcelException(message, ex);
            }
        }

        /// <summary>
        /// Extracts the thermal label from the i-parcel response.
        /// </summary>
        /// <param name="iParcelResponse">The i-parcel response.</param>
        /// <param name="rowIndex">Index of the row in the data table to pull the label from.</param>
        /// <returns>A byte array of the image.</returns>
        /// <exception cref="iParcelException">Unable to find label information in the i-parcel response.</exception>
        private byte[] GetThermalLabel(DataSet iParcelResponse, int rowIndex)
        {
            if (!iParcelResponse.Tables[LabelTableName].Columns.Contains(ThermalColumnName))
            {
                throw new iParcelException("Unable to find label information in the i-parcel response.");
            }

            // Grab the first row in the label table for the EPL text
            int thermalLabelColumnOrdinal = iParcelResponse.Tables[LabelTableName].Columns[ThermalColumnName].Ordinal;
            string thermalText = iParcelResponse.Tables[LabelTableName].Rows[rowIndex][thermalLabelColumnOrdinal].ToString();

            return Encoding.ASCII.GetBytes(thermalText);
        }

        /// <summary>
        /// Extracts the label image from the i-parcel response.
        /// </summary>
        /// <param name="iParcelResponse">The i-parcel response.</param>
        /// <param name="rowIndex">Index of the row in the data table to pull the label from.</param>
        /// <returns>A byte array of the image.</returns>
        /// <exception cref="iParcelException">Unable to find label information in the i-parcel response.</exception>
        private byte[] GetImageLabel(DataSet iParcelResponse, int rowIndex)
        {
            if (!iParcelResponse.Tables[LabelTableName].Columns.Contains(Base64ColumnName))
            {
                throw new iParcelException("Unable to find label information in the i-parcel response.");
            }

            // Grab the first row in the label table for the base 64 encoded string column (this may need to be updated for
            // multiple package shipments)
            int encodedLabelColumnOrdinal = iParcelResponse.Tables[LabelTableName].Columns[Base64ColumnName].Ordinal;
            string encodedLabel = iParcelResponse.Tables[LabelTableName].Rows[rowIndex][encodedLabelColumnOrdinal].ToString();

            return Convert.FromBase64String(encodedLabel);
        }

        /// <summary>
        /// Gets the shipping settings from the data source.
        /// </summary>
        /// <returns>The ShippingSettingsEntity.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ShippingSettingsEntity GetShippingSettings()
        {
            return ShippingSettings.Fetch();
        }


        /// <summary>
        /// Gets the parcel account for the given shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An iParcelAccountEntity object.</returns>
        /// <exception cref="iParcelException">No i-parcel account is selected for the shipment.</exception>
        public IParcelAccountEntity GetiParcelAccount(ShipmentEntity shipment)
        {
            IParcelAccountEntity account = iParcelAccountManager.GetAccount(shipment.IParcel.IParcelAccountID);
            if (account == null)
            {
                throw new iParcelException("No i-parcel account is selected for the shipment.");
            }

            return account;
        }
    }
}
