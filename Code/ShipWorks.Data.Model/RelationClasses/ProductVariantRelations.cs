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
using System.Collections;
using System.Collections.Generic;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.RelationClasses
{
	/// <summary>Implements the relations factory for the entity: ProductVariant. </summary>
	public partial class ProductVariantRelations
	{
		/// <summary>CTor</summary>
		public ProductVariantRelations()
		{
		}

		/// <summary>Gets all relations of the ProductVariantEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ProductBundleEntityUsingChildProductVariantID);
			toReturn.Add(this.ProductVariantAliasEntityUsingProductVariantID);
			toReturn.Add(this.ProductVariantTypeAndValueEntityUsingProductVariantID);
			toReturn.Add(this.ProductEntityUsingProductID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ProductVariantEntity and ProductBundleEntity over the 1:n relation they have, using the relation between the fields:
		/// ProductVariant.ProductVariantID - ProductBundle.ChildProductVariantID
		/// </summary>
		public virtual IEntityRelation ProductBundleEntityUsingChildProductVariantID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ProductBundle" , true);
				relation.AddEntityFieldPair(ProductVariantFields.ProductVariantID, ProductBundleFields.ChildProductVariantID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductVariantEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductBundleEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProductVariantEntity and ProductVariantAliasEntity over the 1:n relation they have, using the relation between the fields:
		/// ProductVariant.ProductVariantID - ProductVariantAlias.ProductVariantID
		/// </summary>
		public virtual IEntityRelation ProductVariantAliasEntityUsingProductVariantID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ProductVariantAlias" , true);
				relation.AddEntityFieldPair(ProductVariantFields.ProductVariantID, ProductVariantAliasFields.ProductVariantID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductVariantEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductVariantAliasEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProductVariantEntity and ProductVariantTypeAndValueEntity over the 1:n relation they have, using the relation between the fields:
		/// ProductVariant.ProductVariantID - ProductVariantTypeAndValue.ProductVariantID
		/// </summary>
		public virtual IEntityRelation ProductVariantTypeAndValueEntityUsingProductVariantID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ProductVariantTypeAndValue" , true);
				relation.AddEntityFieldPair(ProductVariantFields.ProductVariantID, ProductVariantTypeAndValueFields.ProductVariantID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductVariantEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductVariantTypeAndValueEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between ProductVariantEntity and ProductEntity over the m:1 relation they have, using the relation between the fields:
		/// ProductVariant.ProductID - Product.ProductID
		/// </summary>
		public virtual IEntityRelation ProductEntityUsingProductID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Product", false);
				relation.AddEntityFieldPair(ProductFields.ProductID, ProductVariantFields.ProductID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductVariantEntity", true);
				return relation;
			}
		}
		/// <summary>stub, not used in this entity, only for TargetPerEntity entities.</summary>
		public virtual IEntityRelation GetSubTypeRelation(string subTypeEntityName) { return null; }
		/// <summary>stub, not used in this entity, only for TargetPerEntity entities.</summary>
		public virtual IEntityRelation GetSuperTypeRelation() { return null;}
		#endregion

		#region Included Code

		#endregion
	}
	
	/// <summary>Static class which is used for providing relationship instances which are re-used internally for syncing</summary>
	internal static class StaticProductVariantRelations
	{
		internal static readonly IEntityRelation ProductBundleEntityUsingChildProductVariantIDStatic = new ProductVariantRelations().ProductBundleEntityUsingChildProductVariantID;
		internal static readonly IEntityRelation ProductVariantAliasEntityUsingProductVariantIDStatic = new ProductVariantRelations().ProductVariantAliasEntityUsingProductVariantID;
		internal static readonly IEntityRelation ProductVariantTypeAndValueEntityUsingProductVariantIDStatic = new ProductVariantRelations().ProductVariantTypeAndValueEntityUsingProductVariantID;
		internal static readonly IEntityRelation ProductEntityUsingProductIDStatic = new ProductVariantRelations().ProductEntityUsingProductID;

		/// <summary>CTor</summary>
		static StaticProductVariantRelations()
		{
		}
	}
}
