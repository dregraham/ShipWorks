using System;
using System.IO;
using System.Windows.Forms;
using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    ///
    /// </summary>
    public abstract class OdbcSettingsFile
    {
        private readonly IIndex<FileDialogType, IFileDialog> fileDialogFactory;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcSettingsFile"/> class.
        /// </summary>
        protected OdbcSettingsFile(IIndex<FileDialogType, IFileDialog> fileDialogFactory, IMessageHelper messageHelper)
        {
            this.fileDialogFactory = fileDialogFactory;
            this.messageHelper = messageHelper;
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
        private string Filter => $"ShipWorks ODBC {Action} Map (*{Extension})|*{Extension}";

        /// <summary>
        /// Reads the additional paramaters from map.
        /// </summary>
        protected abstract void ReadAdditionalParamatersFromMap(JObject map);

        /// <summary>
        /// Writes the additional paramaters to map.
        /// </summary>
        protected abstract void WriteAdditionalParamatersToMap(JObject map);

        /// <summary>
        /// Opens the load file dialog to load the map
        /// </summary>
        public void Open()
        {
            IFileDialog fileDialog = fileDialogFactory[FileDialogType.Open];
            fileDialog.DefaultExt = Extension;
            fileDialog.Filter = Filter;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (Stream fileStream = fileDialog.CreateFileStream())
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    JObject map = JObject.Parse(streamReader.ReadToEnd());

                    ColumnSourceType = EnumHelper.GetEnumByApiValue<OdbcColumnSourceType>(map.GetValue("ColumnSourceType").ToString());
                    ColumnSource = map.GetValue("ColumnSource").ToString();
                    OdbcFieldMap.Load(map.GetValue("FieldMap").ToString());

                    ReadAdditionalParamatersFromMap(map);
                }
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
        public void Save()
        {
            IFileDialog fileDialog = fileDialogFactory[FileDialogType.Save];
            fileDialog.DefaultExt = Extension;
            fileDialog.Filter = Filter;
            fileDialog.DefaultFileName = OdbcFieldMap.Name;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (Stream fileStream = fileDialog.CreateFileStream())
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    JObject map = new JObject
                    {
                        {"FieldMap", new JObject(OdbcFieldMap.Serialize())},
                        {"ColumnSourceType", new JObject(EnumHelper.GetApiValue(ColumnSourceType))},
                        {"ColumnSource", new JObject(ColumnSource)}
                    };

                    WriteAdditionalParamatersToMap(map);

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(streamWriter, map);
                }
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