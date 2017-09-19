using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Filters.Content;
using ShipWorks.Stores;
using ShipWorks.Users;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Creates SearchDefinitionProviderFactory
    /// </summary>
    [Component(RegistrationType.Self)]
    public class SearchDefinitionProviderFactory
    {
        private readonly IStoreManager storeManager;
        private readonly ISingleScanOrderShortcut singleScanShortcut;
        private readonly SingleScanSettings singleScanSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDefinitionProviderFactory"/> class.
        /// </summary>
        /// <param name="storeManager">The store manager.</param>
        /// <param name="userSession">The user session</param>
        /// <param name="singleScanShortcut">Prefix that identifies a scan result as a ShipWorks order</param>
        public SearchDefinitionProviderFactory(IStoreManager storeManager, IUserSession userSession, ISingleScanOrderShortcut singleScanShortcut)
        {
            this.storeManager = storeManager;
            this.singleScanShortcut = singleScanShortcut;
            singleScanSettings = (SingleScanSettings) (userSession?.Settings?.SingleScanSettings ??
                                                       (int) SingleScanSettings.Disabled);
        }

        /// <summary>
        /// Creates a SearchDefinitionProvider for the specified target.
        /// </summary>
        public ISearchDefinitionProvider Create(FilterTarget target, bool isBarcodeSearch) => Create(target, null, isBarcodeSearch);

        /// <summary>
        /// Creates a SearchDefinitionProvider for the specified target and FilterDefinition
        /// </summary>
        /// <remarks>
        /// If advancedSearchDefinition is null, a quick search definition provider is returned. If not, an AdvancedSearch provider is returned.
        /// </remarks>
        public ISearchDefinitionProvider Create(FilterTarget target, FilterDefinition advancedSearchDefinition, bool isBarcodeSearch)
        {
            ISearchDefinitionProvider quickSearchDefinitionProvider;

            switch (target)
            {
                case FilterTarget.Customers:
                    quickSearchDefinitionProvider = new CustomerQuickSearchDefinitionProvider();
                    break;
                case FilterTarget.Orders:
                    if (singleScanSettings != SingleScanSettings.Disabled && isBarcodeSearch)
                    {
                        quickSearchDefinitionProvider = new SingleScanSearchDefinitionProvider(singleScanShortcut);
                    }
                    else
                    {
                        quickSearchDefinitionProvider = new OrderQuickSearchDefinitionProvider(storeManager);
                    }
                    break;
                default:
                    throw new IndexOutOfRangeException($"Unknown target {target} in FilterDefinitionProviderFactory.Create");
            }

            return advancedSearchDefinition == null ?
                quickSearchDefinitionProvider :
                new AdvancedSearchDefinitionProvider(advancedSearchDefinition, quickSearchDefinitionProvider,
                    isBarcodeSearch ? FilterDefinitionSourceType.Scan : FilterDefinitionSourceType.Search);
        }
    }
}
