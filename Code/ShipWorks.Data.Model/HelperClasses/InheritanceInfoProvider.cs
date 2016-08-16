///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.RelationClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.HelperClasses
{
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	
	/// <summary>Singleton implementation of the inheritanceInfoProvider. This class is the singleton wrapper through which the actual instance is retrieved.</summary>
	/// <remarks>It uses a single instance of an internal class. The access isn't marked with locks as the InheritanceInfoProviderBase class is threadsafe.</remarks>
	public static class InheritanceInfoProviderSingleton
	{
		#region Class Member Declarations
		private static readonly IInheritanceInfoProvider _providerInstance = new InheritanceInfoProviderCore();
		#endregion
		
		/// <summary>Dummy static constructor to make sure threadsafe initialization is performed.</summary>
		static InheritanceInfoProviderSingleton() { }

		/// <summary>Gets the singleton instance of the InheritanceInfoProviderCore</summary>
		/// <returns>Instance of the InheritanceInfoProvider.</returns>
		public static IInheritanceInfoProvider GetInstance()
		{
			return _providerInstance;
		}

		#region Custom InheritanceInfoProviderSingleton code
		
		// __LLBLGENPRO_USER_CODE_REGION_START CustomInheritanceInfoProviderSingletonCode
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion
	}

	/// <summary>Actual implementation of the InheritanceInfoProvider. Used by singleton wrapper.</summary>
	internal class InheritanceInfoProviderCore : InheritanceInfoProviderBase
	{
		/// <summary>Initializes a new instance of the <see cref="InheritanceInfoProviderCore"/> class.</summary>
		internal InheritanceInfoProviderCore()
		{
			Init();
		}

		/// <summary>Method which initializes the internal datastores with the structure of hierarchical types.</summary>
		private void Init()
		{
			this.AddEntityInfo("AmazonOrderEntity", "OrderEntity", new AmazonOrderRelations(), new AmazonOrderEntityFactory(), 1-1);
			this.AddEntityInfo("AmazonOrderItemEntity", "OrderItemEntity", new AmazonOrderItemRelations(), new AmazonOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("AmazonStoreEntity", "StoreEntity", new AmazonStoreRelations(), new AmazonStoreEntityFactory(), 1-1);
			this.AddEntityInfo("AmeriCommerceStoreEntity", "StoreEntity", new AmeriCommerceStoreRelations(), new AmeriCommerceStoreEntityFactory(), 1-1);
			this.AddEntityInfo("BigCommerceOrderItemEntity", "OrderItemEntity", new BigCommerceOrderItemRelations(), new BigCommerceOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("BigCommerceStoreEntity", "StoreEntity", new BigCommerceStoreRelations(), new BigCommerceStoreEntityFactory(), 1-1);
			this.AddEntityInfo("BuyDotComOrderItemEntity", "OrderItemEntity", new BuyDotComOrderItemRelations(), new BuyDotComOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("BuyDotComStoreEntity", "StoreEntity", new BuyDotComStoreRelations(), new BuyDotComStoreEntityFactory(), 1-1);
			this.AddEntityInfo("ChannelAdvisorOrderEntity", "OrderEntity", new ChannelAdvisorOrderRelations(), new ChannelAdvisorOrderEntityFactory(), 1-1);
			this.AddEntityInfo("ChannelAdvisorOrderItemEntity", "OrderItemEntity", new ChannelAdvisorOrderItemRelations(), new ChannelAdvisorOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("ChannelAdvisorStoreEntity", "StoreEntity", new ChannelAdvisorStoreRelations(), new ChannelAdvisorStoreEntityFactory(), 1-1);
			this.AddEntityInfo("ClickCartProOrderEntity", "OrderEntity", new ClickCartProOrderRelations(), new ClickCartProOrderEntityFactory(), 1-1);
			this.AddEntityInfo("CommerceInterfaceOrderEntity", "OrderEntity", new CommerceInterfaceOrderRelations(), new CommerceInterfaceOrderEntityFactory(), 1-1);
			this.AddEntityInfo("EbayOrderEntity", "OrderEntity", new EbayOrderRelations(), new EbayOrderEntityFactory(), 1-1);
			this.AddEntityInfo("EbayOrderItemEntity", "OrderItemEntity", new EbayOrderItemRelations(), new EbayOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("EbayStoreEntity", "StoreEntity", new EbayStoreRelations(), new EbayStoreEntityFactory(), 1-1);
			this.AddEntityInfo("EtsyOrderEntity", "OrderEntity", new EtsyOrderRelations(), new EtsyOrderEntityFactory(), 1-1);
			this.AddEntityInfo("EtsyStoreEntity", "StoreEntity", new EtsyStoreRelations(), new EtsyStoreEntityFactory(), 1-1);
			this.AddEntityInfo("GenericFileStoreEntity", "StoreEntity", new GenericFileStoreRelations(), new GenericFileStoreEntityFactory(), 1-1);
			this.AddEntityInfo("GenericModuleStoreEntity", "StoreEntity", new GenericModuleStoreRelations(), new GenericModuleStoreEntityFactory(), 1-1);
			this.AddEntityInfo("GrouponOrderEntity", "OrderEntity", new GrouponOrderRelations(), new GrouponOrderEntityFactory(), 1-1);
			this.AddEntityInfo("GrouponOrderItemEntity", "OrderItemEntity", new GrouponOrderItemRelations(), new GrouponOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("GrouponStoreEntity", "StoreEntity", new GrouponStoreRelations(), new GrouponStoreEntityFactory(), 1-1);
			this.AddEntityInfo("InfopiaOrderItemEntity", "OrderItemEntity", new InfopiaOrderItemRelations(), new InfopiaOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("InfopiaStoreEntity", "StoreEntity", new InfopiaStoreRelations(), new InfopiaStoreEntityFactory(), 1-1);
			this.AddEntityInfo("LemonStandOrderEntity", "OrderEntity", new LemonStandOrderRelations(), new LemonStandOrderEntityFactory(), 1-1);
			this.AddEntityInfo("LemonStandOrderItemEntity", "OrderItemEntity", new LemonStandOrderItemRelations(), new LemonStandOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("LemonStandStoreEntity", "StoreEntity", new LemonStandStoreRelations(), new LemonStandStoreEntityFactory(), 1-1);
			this.AddEntityInfo("MagentoOrderEntity", "OrderEntity", new MagentoOrderRelations(), new MagentoOrderEntityFactory(), 1-1);
			this.AddEntityInfo("MagentoStoreEntity", "GenericModuleStoreEntity", new MagentoStoreRelations(), new MagentoStoreEntityFactory(), 1-1);
			this.AddEntityInfo("MarketplaceAdvisorOrderEntity", "OrderEntity", new MarketplaceAdvisorOrderRelations(), new MarketplaceAdvisorOrderEntityFactory(), 1-1);
			this.AddEntityInfo("MarketplaceAdvisorStoreEntity", "StoreEntity", new MarketplaceAdvisorStoreRelations(), new MarketplaceAdvisorStoreEntityFactory(), 1-1);
			this.AddEntityInfo("MivaOrderItemAttributeEntity", "OrderItemAttributeEntity", new MivaOrderItemAttributeRelations(), new MivaOrderItemAttributeEntityFactory(), 1-1);
			this.AddEntityInfo("MivaStoreEntity", "GenericModuleStoreEntity", new MivaStoreRelations(), new MivaStoreEntityFactory(), 1-1);
			this.AddEntityInfo("NetworkSolutionsOrderEntity", "OrderEntity", new NetworkSolutionsOrderRelations(), new NetworkSolutionsOrderEntityFactory(), 1-1);
			this.AddEntityInfo("NetworkSolutionsStoreEntity", "StoreEntity", new NetworkSolutionsStoreRelations(), new NetworkSolutionsStoreEntityFactory(), 1-1);
			this.AddEntityInfo("NeweggOrderEntity", "OrderEntity", new NeweggOrderRelations(), new NeweggOrderEntityFactory(), 1-1);
			this.AddEntityInfo("NeweggOrderItemEntity", "OrderItemEntity", new NeweggOrderItemRelations(), new NeweggOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("NeweggStoreEntity", "StoreEntity", new NeweggStoreRelations(), new NeweggStoreEntityFactory(), 1-1);
			this.AddEntityInfo("OdbcStoreEntity", "StoreEntity", new OdbcStoreRelations(), new OdbcStoreEntityFactory(), 1-1);
			this.AddEntityInfo("OrderEntity", string.Empty, new OrderRelations(), new OrderEntityFactory());
			this.AddEntityInfo("OrderItemEntity", string.Empty, new OrderItemRelations(), new OrderItemEntityFactory());
			this.AddEntityInfo("OrderItemAttributeEntity", string.Empty, new OrderItemAttributeRelations(), new OrderItemAttributeEntityFactory());
			this.AddEntityInfo("OrderMotionOrderEntity", "OrderEntity", new OrderMotionOrderRelations(), new OrderMotionOrderEntityFactory(), 1-1);
			this.AddEntityInfo("OrderMotionStoreEntity", "StoreEntity", new OrderMotionStoreRelations(), new OrderMotionStoreEntityFactory(), 1-1);
			this.AddEntityInfo("PayPalOrderEntity", "OrderEntity", new PayPalOrderRelations(), new PayPalOrderEntityFactory(), 1-1);
			this.AddEntityInfo("PayPalStoreEntity", "StoreEntity", new PayPalStoreRelations(), new PayPalStoreEntityFactory(), 1-1);
			this.AddEntityInfo("ProStoresOrderEntity", "OrderEntity", new ProStoresOrderRelations(), new ProStoresOrderEntityFactory(), 1-1);
			this.AddEntityInfo("ProStoresStoreEntity", "StoreEntity", new ProStoresStoreRelations(), new ProStoresStoreEntityFactory(), 1-1);
			this.AddEntityInfo("SearsOrderEntity", "OrderEntity", new SearsOrderRelations(), new SearsOrderEntityFactory(), 1-1);
			this.AddEntityInfo("SearsOrderItemEntity", "OrderItemEntity", new SearsOrderItemRelations(), new SearsOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("SearsStoreEntity", "StoreEntity", new SearsStoreRelations(), new SearsStoreEntityFactory(), 1-1);
			this.AddEntityInfo("ShopifyOrderEntity", "OrderEntity", new ShopifyOrderRelations(), new ShopifyOrderEntityFactory(), 1-1);
			this.AddEntityInfo("ShopifyOrderItemEntity", "OrderItemEntity", new ShopifyOrderItemRelations(), new ShopifyOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("ShopifyStoreEntity", "StoreEntity", new ShopifyStoreRelations(), new ShopifyStoreEntityFactory(), 1-1);
			this.AddEntityInfo("ShopSiteStoreEntity", "StoreEntity", new ShopSiteStoreRelations(), new ShopSiteStoreEntityFactory(), 1-1);
			this.AddEntityInfo("SparkPayStoreEntity", "StoreEntity", new SparkPayStoreRelations(), new SparkPayStoreEntityFactory(), 1-1);
			this.AddEntityInfo("StoreEntity", string.Empty, new StoreRelations(), new StoreEntityFactory());
			this.AddEntityInfo("ThreeDCartOrderEntity", "OrderEntity", new ThreeDCartOrderRelations(), new ThreeDCartOrderEntityFactory(), 1-1);
			this.AddEntityInfo("ThreeDCartOrderItemEntity", "OrderItemEntity", new ThreeDCartOrderItemRelations(), new ThreeDCartOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("ThreeDCartStoreEntity", "StoreEntity", new ThreeDCartStoreRelations(), new ThreeDCartStoreEntityFactory(), 1-1);
			this.AddEntityInfo("VolusionStoreEntity", "StoreEntity", new VolusionStoreRelations(), new VolusionStoreEntityFactory(), 1-1);
			this.AddEntityInfo("YahooOrderEntity", "OrderEntity", new YahooOrderRelations(), new YahooOrderEntityFactory(), 1-1);
			this.AddEntityInfo("YahooOrderItemEntity", "OrderItemEntity", new YahooOrderItemRelations(), new YahooOrderItemEntityFactory(), 1-1);
			this.AddEntityInfo("YahooStoreEntity", "StoreEntity", new YahooStoreRelations(), new YahooStoreEntityFactory(), 1-1);
			base.BuildHierarchyInfoStore();
		}

		/// <summary>Gets the entity fields for the entity passed in. Only the fields defined in the entity are returned</summary>
		/// <param name="entityName">Name of the entity to grab the fields for</param>
		/// <returns>array of IEntityFieldCore fields</returns>
		public override IEntityFieldCore[] GetEntityFields(string entityName)
		{
			return EntityFieldsFactory.CreateFields(entityName);
		}
	}
}




