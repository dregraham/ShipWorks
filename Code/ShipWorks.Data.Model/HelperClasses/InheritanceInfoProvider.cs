///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates.NET20
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
	
	/// <summary>
	/// Singleton implementation of the inheritanceInfoProvider. This class is the singleton wrapper through which the actual instance is retrieved.
	/// </summary>
	/// <remarks>It uses a single instance of an internal class. The access isn't marked with locks as the InheritanceInfoProviderBase class is threadsafe.</remarks>
	public sealed class InheritanceInfoProviderSingleton
	{
		#region Class Member Declarations
		private static readonly IInheritanceInfoProvider _providerInstance = new InheritanceInfoProviderCore();
		#endregion
		
		/// <summary>private ctor to prevent instances of this class.</summary>
		private InheritanceInfoProviderSingleton()
		{
		}

		/// <summary>Dummy static constructor to make sure threadsafe initialization is performed.</summary>
		static InheritanceInfoProviderSingleton()
		{
		}

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
			base.AddEntityInfo("AmazonOrderEntity", "OrderEntity", new AmazonOrderRelations(), new AmazonOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("AmazonOrderItemEntity", "OrderItemEntity", new AmazonOrderItemRelations(), new AmazonOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("AmazonStoreEntity", "StoreEntity", new AmazonStoreRelations(), new AmazonStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("AmeriCommerceStoreEntity", "StoreEntity", new AmeriCommerceStoreRelations(), new AmeriCommerceStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("BigCommerceOrderItemEntity", "OrderItemEntity", new BigCommerceOrderItemRelations(), new BigCommerceOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("BigCommerceStoreEntity", "StoreEntity", new BigCommerceStoreRelations(), new BigCommerceStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("BuyDotComOrderItemEntity", "OrderItemEntity", new BuyDotComOrderItemRelations(), new BuyDotComOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("BuyDotComStoreEntity", "StoreEntity", new BuyDotComStoreRelations(), new BuyDotComStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("ChannelAdvisorOrderEntity", "OrderEntity", new ChannelAdvisorOrderRelations(), new ChannelAdvisorOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("ChannelAdvisorOrderItemEntity", "OrderItemEntity", new ChannelAdvisorOrderItemRelations(), new ChannelAdvisorOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("ChannelAdvisorStoreEntity", "StoreEntity", new ChannelAdvisorStoreRelations(), new ChannelAdvisorStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("ClickCartProOrderEntity", "OrderEntity", new ClickCartProOrderRelations(), new ClickCartProOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("CommerceInterfaceOrderEntity", "OrderEntity", new CommerceInterfaceOrderRelations(), new CommerceInterfaceOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("EbayOrderEntity", "OrderEntity", new EbayOrderRelations(), new EbayOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("EbayOrderItemEntity", "OrderItemEntity", new EbayOrderItemRelations(), new EbayOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("EbayStoreEntity", "StoreEntity", new EbayStoreRelations(), new EbayStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("EtsyOrderEntity", "OrderEntity", new EtsyOrderRelations(), new EtsyOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("EtsyStoreEntity", "StoreEntity", new EtsyStoreRelations(), new EtsyStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("GenericFileStoreEntity", "StoreEntity", new GenericFileStoreRelations(), new GenericFileStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("GenericModuleStoreEntity", "StoreEntity", new GenericModuleStoreRelations(), new GenericModuleStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("GrouponOrderEntity", "OrderEntity", new GrouponOrderRelations(), new GrouponOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("GrouponOrderItemEntity", "OrderItemEntity", new GrouponOrderItemRelations(), new GrouponOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("GrouponStoreEntity", "StoreEntity", new GrouponStoreRelations(), new GrouponStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("InfopiaOrderItemEntity", "OrderItemEntity", new InfopiaOrderItemRelations(), new InfopiaOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("InfopiaStoreEntity", "StoreEntity", new InfopiaStoreRelations(), new InfopiaStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("LemonStandOrderEntity", "OrderEntity", new LemonStandOrderRelations(), new LemonStandOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("LemonStandOrderItemEntity", "OrderItemEntity", new LemonStandOrderItemRelations(), new LemonStandOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("LemonStandStoreEntity", "StoreEntity", new LemonStandStoreRelations(), new LemonStandStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("MagentoOrderEntity", "OrderEntity", new MagentoOrderRelations(), new MagentoOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("MagentoStoreEntity", "GenericModuleStoreEntity", new MagentoStoreRelations(), new MagentoStoreEntityFactory(),  (50-50));
			base.AddEntityInfo("MarketplaceAdvisorOrderEntity", "OrderEntity", new MarketplaceAdvisorOrderRelations(), new MarketplaceAdvisorOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("MarketplaceAdvisorStoreEntity", "StoreEntity", new MarketplaceAdvisorStoreRelations(), new MarketplaceAdvisorStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("MivaOrderItemAttributeEntity", "OrderItemAttributeEntity", new MivaOrderItemAttributeRelations(), new MivaOrderItemAttributeEntityFactory(),  (7-7));
			base.AddEntityInfo("MivaStoreEntity", "GenericModuleStoreEntity", new MivaStoreRelations(), new MivaStoreEntityFactory(),  (50-50));
			base.AddEntityInfo("NetworkSolutionsOrderEntity", "OrderEntity", new NetworkSolutionsOrderRelations(), new NetworkSolutionsOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("NetworkSolutionsStoreEntity", "StoreEntity", new NetworkSolutionsStoreRelations(), new NetworkSolutionsStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("NeweggOrderEntity", "OrderEntity", new NeweggOrderRelations(), new NeweggOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("NeweggOrderItemEntity", "OrderItemEntity", new NeweggOrderItemRelations(), new NeweggOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("NeweggStoreEntity", "StoreEntity", new NeweggStoreRelations(), new NeweggStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("OdbcStoreEntity", "StoreEntity", new OdbcStoreRelations(), new OdbcStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("OrderEntity", string.Empty, new OrderRelations(), new OrderEntityFactory());
			base.AddEntityInfo("OrderItemEntity", string.Empty, new OrderItemRelations(), new OrderItemEntityFactory());
			base.AddEntityInfo("OrderItemAttributeEntity", string.Empty, new OrderItemAttributeRelations(), new OrderItemAttributeEntityFactory());
			base.AddEntityInfo("OrderMotionOrderEntity", "OrderEntity", new OrderMotionOrderRelations(), new OrderMotionOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("OrderMotionStoreEntity", "StoreEntity", new OrderMotionStoreRelations(), new OrderMotionStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("PayPalOrderEntity", "OrderEntity", new PayPalOrderRelations(), new PayPalOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("PayPalStoreEntity", "StoreEntity", new PayPalStoreRelations(), new PayPalStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("ProStoresOrderEntity", "OrderEntity", new ProStoresOrderRelations(), new ProStoresOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("ProStoresStoreEntity", "StoreEntity", new ProStoresStoreRelations(), new ProStoresStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("SearsOrderEntity", "OrderEntity", new SearsOrderRelations(), new SearsOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("SearsOrderItemEntity", "OrderItemEntity", new SearsOrderItemRelations(), new SearsOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("SearsStoreEntity", "StoreEntity", new SearsStoreRelations(), new SearsStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("ShopifyOrderEntity", "OrderEntity", new ShopifyOrderRelations(), new ShopifyOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("ShopifyOrderItemEntity", "OrderItemEntity", new ShopifyOrderItemRelations(), new ShopifyOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("ShopifyStoreEntity", "StoreEntity", new ShopifyStoreRelations(), new ShopifyStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("ShopSiteStoreEntity", "StoreEntity", new ShopSiteStoreRelations(), new ShopSiteStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("SparkPayStoreEntity", "StoreEntity", new SparkPayStoreRelations(), new SparkPayStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("StoreEntity", string.Empty, new StoreRelations(), new StoreEntityFactory());
			base.AddEntityInfo("ThreeDCartOrderItemEntity", "OrderItemEntity", new ThreeDCartOrderItemRelations(), new ThreeDCartOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("ThreeDCartStoreEntity", "StoreEntity", new ThreeDCartStoreRelations(), new ThreeDCartStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("VolusionStoreEntity", "StoreEntity", new VolusionStoreRelations(), new VolusionStoreEntityFactory(),  (30-30));
			base.AddEntityInfo("YahooOrderEntity", "OrderEntity", new YahooOrderRelations(), new YahooOrderEntityFactory(),  (74-74));
			base.AddEntityInfo("YahooOrderItemEntity", "OrderItemEntity", new YahooOrderItemRelations(), new YahooOrderItemEntityFactory(),  (18-18));
			base.AddEntityInfo("YahooStoreEntity", "StoreEntity", new YahooStoreRelations(), new YahooStoreEntityFactory(),  (30-30));
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




