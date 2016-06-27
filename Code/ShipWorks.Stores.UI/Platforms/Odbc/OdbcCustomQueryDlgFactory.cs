using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using log4net;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.UI.Controls.ChannelLimit;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcCustomQueryDlgFactory : IOdbcCustomQueryDlgFactory
    {
        private readonly IWin32Window defaultOwner;
        private readonly IOdbcColumnSourceFactory columnSourceFactory;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly Func<Type, ILog> logFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCustomQueryDlgFactory"/> class.
        /// </summary>
        /// <param name="defaultOwner">The default owner.</param>
        /// <param name="columnSourceFactory"></param>
        /// <param name="dbProviderFactory"></param>
        /// <param name="logFactory"></param>
        public OdbcCustomQueryDlgFactory(IWin32Window defaultOwner, IOdbcColumnSourceFactory columnSourceFactory,
            IShipWorksDbProviderFactory dbProviderFactory, Func<Type, ILog> logFactory)
        {
            this.defaultOwner = defaultOwner;
            this.columnSourceFactory = columnSourceFactory;
            this.dbProviderFactory = dbProviderFactory;
            this.logFactory = logFactory;
        }

        /// <summary>
        /// Shows the custom query dialog.
        /// </summary>
        public void ShowCustomQueryDlg(OdbcImportFieldMappingControl owner, IOdbcDataSource dataSource)
        {
            OdbcCustomQueryDlg dlg = new OdbcCustomQueryDlg();
            dlg.LoadOwner(GetOwner(owner));

            IOdbcColumnSource columnSource = columnSourceFactory.CreateTable("Custom Query");
            IOdbcSampleDataCommand sampleDataCommand = new OdbcSampleDataCommand(dbProviderFactory,
                logFactory(typeof (OdbcSampleDataCommand)));

            dlg.DataContext = new OdbcCustomQueryDlgViewModel(dataSource, dbProviderFactory, sampleDataCommand, columnSource, logFactory);
            dlg.ShowDialog();
        }

        /// <summary>
        /// Gets the IWin32Window owner from the control.
        /// </summary>
        private IWin32Window GetOwner(OdbcImportFieldMappingControl owner)
        {
            if (owner == null)
            {
                return defaultOwner;
            }

            // Get handle for wpf control
            HwndSource wpfHandle = PresentationSource.FromVisual(owner) as HwndSource;

            if (wpfHandle != null)
            {
                // Get the ElementHost if the control is owned by one.
                ElementHost host = Control.FromChildHandle(wpfHandle.Handle) as ElementHost;

                if (host != null)
                {
                    // We go up the parent chain here because the dialogs opened with the wizard page
                    // as the owner were not appearing center screen. So we set the owner as the AddStoreWizard instead.
                    // ElementHost -> ActivationErrorWizardPage -> WizardPage -> AddStoreWizard
                    return host.Parent.Parent.Parent;
                }
            }

            // if all else fails, set the parent as the default owner(Main Form)
            return defaultOwner;
        }
    }
}