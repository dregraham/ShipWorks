using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Microsoft.ApplicationInsights.DataContracts;
using ShipWorks.ApplicationCore.Licensing.TangoRequests;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Templates.Printing;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Option page for keyboard and barcode warehouse
    /// </summary>
    public partial class SettingsPageWarehouse : SettingsPageBase
    {
        private IUserSession userSession;
        private readonly IMessenger messenger;
        private readonly IMessageHelper messageHelper;
        private readonly IConfigurationData configurationData;
        private UserSettingsEntity settings;
        private readonly IWin32Window owner;
        private readonly ILifetimeScope scope;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsPageWarehouse(IWin32Window owner, ILifetimeScope scope)
        {
            InitializeComponent();
            messageHelper = scope.Resolve<IMessageHelper>();
            userSession = scope.Resolve<IUserSession>();
            messenger = scope.Resolve<IMessenger>();
            settings = userSession?.User?.Settings;
            configurationData = scope.Resolve<IConfigurationData>();
            this.owner = owner;
            this.scope = scope;
        }

        /// <summary>
        /// Save the selected settings
        /// </summary>
        public override void Save()
        {
            if (userSession.IsLoggedOn && userSession.User.IsAdmin)
            {
                configurationData.UpdateConfiguration(x => x.WarehouseID = "");

                //UpdateSingleScanTelemetry(warehouseID == configurationData.FetchReadOnly().WarehouseID);
            }
        }

        /// <summary>
        /// Load selected warehouse
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            IConfigurationEntity config = configurationData.FetchReadOnly();

            if (!string.IsNullOrWhiteSpace(config.WarehouseName))
            {
                selectedWarehouseName.Text = config.WarehouseName;
            }
        }

        /// <summary>
        /// Log the Single Scan Settings to Telemetry
        /// </summary>
        private void UpdateSingleScanTelemetry(bool warehouseChanged)
        {
            if (warehouseChanged)
            {
                EventTelemetry telemetryEvent = new EventTelemetry("ShipWorks.Warehouse.Changed");
                telemetryEvent.Properties.Add("ShipWorks.Warehouse.Value", "true");

                Telemetry.TrackEvent(telemetryEvent);
            }
        }
        
        /// <summary>
        /// Handle select warehouse button click
        /// </summary>
        private void OnSelectWarehouse(object sender, EventArgs e)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IWarehouseRemoteLoginWithToken remoteLoginWithToken = scope.Resolve<IWarehouseRemoteLoginWithToken>();
                var tokenResponse = remoteLoginWithToken.RemoteLoginWithToken();
                if (tokenResponse.Success)
                {
                    // Save tokens to memory for use elsewhere.

                    IWarehouseList warehouseListRequest = scope.Resolve<IWarehouseList>();
                    var results = warehouseListRequest.GetList(tokenResponse.Value);
                }
            }
        }
    }
}
