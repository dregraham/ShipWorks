using System;
using ShipWorks.ApplicationCore.ComponentRegistration;
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
        private readonly int singleScanSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDefinitionProviderFactory"/> class.
        /// </summary>
        /// <param name="storeManager">The store manager.</param>
        /// <param name="userSession">The user session</param>
        public SearchDefinitionProviderFactory(IStoreManager storeManager, IUserSession userSession)
        {
            this.storeManager = storeManager;
            singleScanSettings = userSession.User.Settings.SingleScanSettings;
        }

        /// <summary>
        /// Creates a SearchDefinitionProvider for the specified target and FilterDefinition
        /// </summary>
        /// <remarks>
        /// If advancedSearchDefinition is null, a quick search definition provider is returned. If not, an AdvancedSearch provider is returned.
        /// </remarks>
        public ISearchDefinitionProvider Create(FilterTarget target, FilterDefinition advancedSearchDefinition)
        {
            ISearchDefinitionProvider quickSearchDefinitionProvider;

            switch (target)
            {
                case FilterTarget.Customers:
                    quickSearchDefinitionProvider = new CustomerQuickSearchDefinitionProvider();
                    break;
                case FilterTarget.Orders:
                    if (singleScanSettings != (int) SingleScanSettings.Disabled)
                    {
                        quickSearchDefinitionProvider = new SingleScanSearchDefinitionProvider();
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
                new AdvancedSearchDefinitionProvider(advancedSearchDefinition, quickSearchDefinitionProvider);
        }

        /// <summary>
        /// Creates a SearchDefinitionProvider for the specified target.
        /// </summary>
        public ISearchDefinitionProvider Create(FilterTarget target) => Create(target, null);
    }
}
