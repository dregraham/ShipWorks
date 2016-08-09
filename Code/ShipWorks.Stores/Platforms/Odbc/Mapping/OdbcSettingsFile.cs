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
        protected OdbcSettingsFile(IMessageHelper messageHelper, IOdbcFieldMap fieldMap)
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
        /// Reads the additional paramaters from map.
        /// </summary>
        protected abstract void ReadAdditionalParamatersFromMap(JObject settings);

        /// <summary>
        /// Writes the additional paramaters to map.
        /// </summary>
        protected abstract void WriteAdditionalParamatersToMap(JObject settings);

        /// <summary>
        /// Opens the load file dialog to load the map
        /// </summary>
        public void Open(StreamReader reader)
        {
            try
            {
                JObject map = JObject.Parse(reader.ReadToEnd());

                ColumnSourceType = EnumHelper.GetEnumByApiValue<OdbcColumnSourceType>(map.GetValue("ColumnSourceType").ToString());
                ColumnSource = map.GetValue("ColumnSource").ToString();
                OdbcFieldMap.Load(map.GetValue("FieldMap").ToString());

                ReadAdditionalParamatersFromMap(map);
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
        /// Opens the save file dialog to save the map
        /// </summary>
        public void Save(TextWriter textWriter)
        {
            try
            {
                JObject settings = new JObject
                {
                    {"ColumnSourceType", EnumHelper.GetApiValue(ColumnSourceType)},
                    {"ColumnSource", ColumnSource},
                    {"FieldMap", OdbcFieldMap.Serialize()}
                };

                WriteAdditionalParamatersToMap(settings);

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
    }
}