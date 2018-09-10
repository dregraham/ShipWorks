using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Filters.Search;
using ShipWorks.Settings;
using ShipWorks.Stores.Communication;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup
{
    public class OrderLookupSingleScanPipeline
    {
        private readonly IMessenger messenger;
        private readonly IOnDemandDownloaderFactory onDemandDownloaderFactory;
        private readonly SearchDefinitionProviderFactory searchDefinitionProviderFactory;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IMainForm mainForm;
        private readonly ICurrentUserSettings userSettings;

        private IDisposable subscription;

        public OrderLookupSingleScanPipeline(IMessenger messenger,
            IOnDemandDownloaderFactory onDemandDownloaderFactory,
            SearchDefinitionProviderFactory searchDefinitionProviderFactory,
            ISqlAdapterFactory sqlAdapterFactory,
            IMainForm mainForm,
            ICurrentUserSettings userSettings)
        {
            this.messenger = messenger;
            this.onDemandDownloaderFactory = onDemandDownloaderFactory;
            this.searchDefinitionProviderFactory = searchDefinitionProviderFactory;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.mainForm = mainForm;
            this.userSettings = userSettings;

            //subscription - messenger.OfType<OrderFoundMessage>()
            //    .Where(_=>!mainForm.AdditionalFormsOpen() && userSettings.GetUIMode() == UIMode.OrderLookup)
            //    .SelectMany(scanMsg => Observable.FromAsync(() => PerformBarcodeDownloadOnDemand(scanMsg.ScannedText)).Select(_ => scanMsg)))
        }


    }
}
