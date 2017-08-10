using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Interface for ChannelAdvisor store type
    /// </summary>
    public interface IChannelAdvisorStoreType
    {
        /// <summary>
        /// Store Type
        /// </summary>
        StoreTypeCode TypeCode { get; }

        /// <summary>
        /// Gets the Authorization URL parameters
        /// </summary>
        string AuthorizeUrlParameters { get; }

        /// <summary>
        /// Creates a store entity instance
        /// </summary>
        StoreEntity CreateStoreInstance();

        /// <summary>
        /// Creates a custom order item entity
        /// </summary>
        OrderItemEntity CreateOrderItemInstance();

        /// <summary>
        /// Creates the order identifier
        /// </summary>
        OrderIdentifier CreateOrderIdentifier(OrderEntity order);

        /// <summary>
        /// Create the control for generating the online update shipment tasks
        /// </summary>
        OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl();

        /// <summary>
        /// Get any filters that should be created as an initial filter set when the store is first created.  If the list is non-empty they will
        /// be automatically put in a folder that is filtered on the store... so their is no need to test for that in the generated filter conditions.
        /// </summary>
        List<FilterEntity> CreateInitialFilters();

        /// <summary>
        /// Create the CA store settings
        /// </summary>
        StoreSettingsControlBase CreateStoreSettingsControl();

        /// <summary>
        /// Create the condition group for searching on Channel Advisor Order ID
        /// </summary>
        ConditionGroup CreateBasicSearchOrderConditions(string search);

        /// <summary>
        /// ChannelAdvisor does not have an Online Status
        /// </summary>
        bool GridOnlineColumnSupported(OnlineGridColumnSupport column);

        /// <summary>
        /// Create the CA download policy
        /// </summary>
        InitialDownloadPolicy InitialDownloadPolicy { get; }

        /// <summary>
        /// Generate CA specific template order elements
        /// </summary>
        void GenerateTemplateOrderElements(ElementOutline container, Func<OrderEntity> orderSource);

        /// <summary>
        /// Generate CA specific template item elements
        /// </summary>
        void GenerateTemplateOrderItemElements(ElementOutline container, Func<OrderItemEntity> itemSource);

        /// <summary>
        /// Create menu commands for uploading shipment details
        /// </summary>
        IEnumerable<IMenuCommand> CreateOnlineUpdateInstanceCommands();

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        int GetOnlineOrderIdentifier(IOrderEntity order);

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        Task<IEnumerable<int>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order);
    }
}