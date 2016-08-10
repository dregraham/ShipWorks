using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using System;
using System.IO;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Used to save and load OdbcSettings from disk.
    /// </summary>
    public abstract class OdbcSettingsFile : IOdbcSettingsFile
    {
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcSettingsFile"/> class.
        /// </summary>
        protected OdbcSettingsFile(IOdbcFieldMap fieldMap, IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            OdbcFieldMap = fieldMap;
        }

        /// <summary>
        /// The action to perform on this file (Import/Upload)
        /// </summary>
        public abstract string Action { get; }

        /// <summary>
        /// The file extension.
        /// </summary>
        public abstract string Extension { get; }

        /// <summary>
        /// The type of the column source.
        /// </summary>
        public OdbcColumnSourceType ColumnSourceType { get; set; }

        /// <summary>
        /// The column source.
        /// </summary>
        public string ColumnSource { get; set; }

        /// <summary>
        /// The ODBC field map.
        /// </summary>
        public IOdbcFieldMap OdbcFieldMap { get; set; }

        /// <summary>
        /// Gets the filter value.
        /// </summary>
        public string Filter => $"ShipWorks ODBC {Action} Map (*{Extension})|*{Extension}";

        /// <summary>
        /// Opens the load file dialog to load the map
        /// </summary>
        public virtual void Open(TextReader reader)
        {
            try
            {
                string json = reader.ReadToEnd();

                JObject settings = JObject.Parse(json);

                PopulateOdbcSettingsFrom(settings);
            }
            catch (IOException ex)
            {
                messageHelper.ShowError(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                messageHelper.ShowError(ex.Message);
            }
            catch (ShipWorksOdbcException ex)
            {
                messageHelper.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Populates the ODBC settings from.
        /// </summary>
        protected virtual void PopulateOdbcSettingsFrom(JObject settings)
        {
            string columnSourceTypeFromDisk = settings.GetValue("ColumnSourceType")?.ToString();
            if (!string.IsNullOrWhiteSpace(columnSourceTypeFromDisk))
            {
                ColumnSourceType = EnumHelper.GetEnumByApiValue<OdbcColumnSourceType>(columnSourceTypeFromDisk);
            }

            string columnSourceFromDisk = settings.GetValue("ColumnSource")?.ToString();
            if (!string.IsNullOrWhiteSpace(columnSourceFromDisk))
            {
                ColumnSource = columnSourceFromDisk;
            }

            string mapFromDisk = settings.GetValue("FieldMap")?.ToString();
            if (!string.IsNullOrWhiteSpace(mapFromDisk))
            {
                OdbcFieldMap.Load(mapFromDisk);
            }
        }

        /// <summary>
        /// Opens the save file dialog to save the map
        /// </summary>
        public virtual void Save(TextWriter textWriter)
        {
            try
            {
                JObject settings = new JObject();

                SaveOdbcSettingsTo(settings);

                textWriter.Write(settings.ToString());
                textWriter.Flush();
            }
            catch (IOException ex)
            {
                messageHelper.ShowError(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                messageHelper.ShowError(ex.Message);
            }
            catch (ShipWorksOdbcException ex)
            {
                messageHelper.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Saves the settings to the JObject
        /// </summary>
        /// <param name="settings">The settings.</param>
        protected virtual void SaveOdbcSettingsTo(JObject settings)
        {
            settings.Add("ColumnSourceType", EnumHelper.GetApiValue(ColumnSourceType));
            settings.Add("ColumnSource", ColumnSource);
            settings.Add("FieldMap", OdbcFieldMap.Serialize());
        }
    }
}