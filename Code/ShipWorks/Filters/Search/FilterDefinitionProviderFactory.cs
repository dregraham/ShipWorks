using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Filters.Content;
using ShipWorks.Stores;

namespace ShipWorks.Filters.Search
{
    [Component(RegistrationType.Self)]
    public class FilterDefinitionProviderFactory
    {
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterDefinitionProviderFactory"/> class.
        /// </summary>
        /// <param name="storeManager">The store manager.</param>
        public FilterDefinitionProviderFactory(IStoreManager storeManager)
        {
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Creates a FitlerDefinitionProvider for the specified target and FilterDefinition
        /// </summary>
        public IFilterDefinitionProvider Create(FilterTarget target, FilterDefinition definition)
        {
            IFilterDefinitionProvider quickSearchDefinitionProvider;

            switch (target)
            {
                case FilterTarget.Customers:
                    quickSearchDefinitionProvider = new CustomerDefinitionProvider();
                    break;
                case FilterTarget.Orders:
                    quickSearchDefinitionProvider = new OrderDefinitionProvider(storeManager);
                    break;
                default:
                    throw new IndexOutOfRangeException($"Unknown target {target} in FilterDefinitionProviderFactory.Create");
            }

            return definition == null
                ? quickSearchDefinitionProvider
                : new AdvancedSearchDefinitionProvider(definition, quickSearchDefinitionProvider);
        }

        /// <summary>
        /// Creates a FitlerDefinitionProvider for the specified target.
        /// </summary>
        public IFilterDefinitionProvider Create(FilterTarget target) => Create(target, null);
    }
}
