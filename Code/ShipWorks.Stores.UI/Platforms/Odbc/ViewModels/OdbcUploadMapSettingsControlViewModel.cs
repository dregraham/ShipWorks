using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Templates.Tokens;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    /// <summary>
    /// ViewModel for OdbcUploadMapSettingsControl
    /// </summary>
    public class OdbcUploadMapSettingsControlViewModel : OdbcMapSettingsControlViewModel
    {
        private readonly Func<string, IDialog> dialogFactory;
        private readonly ITemplateTokenEditorDlg tokenEditorDlg;
        private readonly IWin32Window owner;
        private bool columnSourceIsTable = true;

        private const string InitialQueryComment =
            "/****************************************************************/\n" +
            "/*                                                              */\n" +
            "/* A sample query highlighting a few of the tokens that can be  */\n" +
            "/* used for uploading shipment details to your database has     */\n" +
            "/* been provided below.                                         */\n" +
            "/*                                                              */\n" +
            "/* For more samples and additional information on how to        */\n" +
            "/* leverage ShipWorks tokens when uploading shipment details    */\n" +
            "/* using a custom query, please visit                           */\n" +
            "/* http://www.support.shipworks.com/                            */\n" +
            "/*                                                              */\n" +
            "/****************************************************************/\n\n" +
            "UPDATE ShipmentDetails\n" +
            "SET TrackingNumber = '{//TrackingNumber}'\n" +
            "WHERE OrderID = {//Order/Number}";

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploadMapSettingsControlViewModel"/> class.
        /// </summary>
        public OdbcUploadMapSettingsControlViewModel(Func<string, IDialog> dialogFactory, IMessageHelper messageHelper,
            Func<string, IOdbcColumnSource> columnSourceFactory, ITemplateTokenEditorDlg tokenEditorDlg, IWin32Window owner) :
                base(messageHelper, columnSourceFactory)
        {
            this.dialogFactory = dialogFactory;
            this.tokenEditorDlg = tokenEditorDlg;
            this.owner = owner;
            OpenTemplateEditorDlgCommand = new RelayCommand(OpenTemplateEditorDlg);
            CustomQuery = InitialQueryComment;
        }

        /// <summary>
        /// Whether the column source selected is table
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override bool ColumnSourceIsTable
        {
            get { return columnSourceIsTable; }
            set
            {
                Handler.Set(nameof(ColumnSourceIsTable), ref columnSourceIsTable, value);

                // Show warning dlg when query is selected
                if (!value)
                {
                    IDialog warningDlg = dialogFactory("OdbcCustomQueryWarningDlg");
                    warningDlg.ShowDialog();
                }

                ColumnSource = value ? SelectedTable : CustomQueryColumnSource;
            }
        }

        /// <summary>
        /// The open template editor dialog command.
        /// </summary>
        public object OpenTemplateEditorDlgCommand { get; set; }

        /// <summary>
        /// Opens the template editor dialog.
        /// </summary>
        public void OpenTemplateEditorDlg()
        {
            tokenEditorDlg.TokenText = CustomQuery;
            DialogResult result = tokenEditorDlg.ShowDialog(owner);

            if (result==DialogResult.OK)
            {
                CustomQuery = tokenEditorDlg.TokenText;
            }
        }

        /// <summary>
        /// Saves the map settings.
        /// </summary>
        /// <param name="store"></param>
        public override void SaveMapSettings(OdbcStoreEntity store)
        {
            store.UploadColumnSourceType = ColumnSourceIsTable ?
                (int)OdbcColumnSourceType.Table :
                (int)OdbcColumnSourceType.CustomQuery;

            store.UploadColumnSource = ColumnSourceIsTable ?
                SelectedTable.Name :
                CustomQuery;
        }
    }
}