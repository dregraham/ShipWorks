using System;
using System.IO;
using System.Windows.Forms;
using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    ///
    /// </summary>
    public abstract class OdbcSettingsFile
    {
        private readonly IIndex<FileDialogType, IFileDialog> fileDialogFactory;
        private readonly IOdbcFieldMap fieldMap;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcSettingsFile"/> class.
        /// </summary>
        protected OdbcSettingsFile(IIndex<FileDialogType, IFileDialog> fileDialogFactory, IOdbcFieldMap fieldMap, IMessageHelper messageHelper)
        {
            this.fileDialogFactory = fileDialogFactory;
            this.fieldMap = fieldMap;
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
        /// Opens the load file dialog to load the map
        /// </summary>
        public void Open()
        {
            IFileDialog fileDialog = fileDialogFactory[FileDialogType.Open];
            fileDialog.DefaultExt = Extension;
            fileDialog.Filter = Filter;
            fileDialog.DefaultFileName = fieldMap.Name;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream fileStream = fileDialog.CreateFileStream())
                {
                    try
                    {
                        fieldMap.Load(fileStream);
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

        /// <summary>
        /// Opens the save file dialog to save the map
        /// </summary>
        public void Save()
        {
            IFileDialog fileDialog = fileDialogFactory[FileDialogType.Save];
            fileDialog.DefaultExt = Extension;
            fileDialog.Filter = Filter;
            fileDialog.DefaultFileName = fieldMap.Name;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream fileStream = fileDialog.CreateFileStream())
                {
                    try
                    {
                        fieldMap.Save(fileStream);
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
    }
}