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
	/// <summary>Implements the relations factory for the entity: ProductVariantAttributeValue. </summary>
	public partial class ProductVariantAttributeValueRelations
	{
		/// <summary>CTor</summary>
		public ProductVariantAttributeValueRelations()
		{
		}

		/// <summary>Gets all relations of the ProductVariantAttributeValueEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ProductAttributeEntityUsingProductAttributeID);
			toReturn.Add(this.ProductVariantEntityUsingProductVariantID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between ProductVariantAttributeValueEntity and ProductAttributeEntity over the m:1 relation they have, using the relation between the fields:
		/// ProductVariantAttributeValue.ProductAttributeID - ProductAttribute.ProductAttributeID
		/// </summary>
		public virtual IEntityRelation ProductAttributeEntityUsingProductAttributeID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ProductAttribute", false);
				relation.AddEntityFieldPair(ProductAttributeFields.ProductAttributeID, ProductVariantAttributeValueFields.ProductAttributeID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductAttributeEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductVariantAttributeValueEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ProductVariantAttributeValueEntity and ProductVariantEntity over the m:1 relation they have, using the relation between the fields:
		/// ProductVariantAttributeValue.ProductVariantID - ProductVariant.ProductVariantID
		/// </summary>
		public virtual IEntityRelation ProductVariantEntityUsingProductVariantID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ProductVariant", false);
				relation.AddEntityFieldPair(ProductVariantFields.ProductVariantID, ProductVariantAttributeValueFields.ProductVariantID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductVariantEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductVariantAttributeValueEntity", true);
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
	internal static class StaticProductVariantAttributeValueRelations
	{
		internal static readonly IEntityRelation ProductAttributeEntityUsingProductAttributeIDStatic = new ProductVariantAttributeValueRelations().ProductAttributeEntityUsingProductAttributeID;
		internal static readonly IEntityRelation ProductVariantEntityUsingProductVariantIDStatic = new ProductVariantAttributeValueRelations().ProductVariantEntityUsingProductVariantID;

		/// <summary>CTor</summary>
		static StaticProductVariantAttributeValueRelations()
		{
		}
	}
}
