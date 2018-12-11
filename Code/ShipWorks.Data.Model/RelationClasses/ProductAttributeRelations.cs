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
	/// <summary>Implements the relations factory for the entity: ProductAttribute. </summary>
	public partial class ProductAttributeRelations
	{
		/// <summary>CTor</summary>
		public ProductAttributeRelations()
		{
		}

		/// <summary>Gets all relations of the ProductAttributeEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ProductVariantAttributeEntityUsingProductAttributeID);
			toReturn.Add(this.ProductEntityUsingProductID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ProductAttributeEntity and ProductVariantAttributeEntity over the 1:n relation they have, using the relation between the fields:
		/// ProductAttribute.ProductAttributeID - ProductVariantAttribute.ProductAttributeID
		/// </summary>
		public virtual IEntityRelation ProductVariantAttributeEntityUsingProductAttributeID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ProductVariantAttribute" , true);
				relation.AddEntityFieldPair(ProductAttributeFields.ProductAttributeID, ProductVariantAttributeFields.ProductAttributeID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductAttributeEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductVariantAttributeEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between ProductAttributeEntity and ProductEntity over the m:1 relation they have, using the relation between the fields:
		/// ProductAttribute.ProductID - Product.ProductID
		/// </summary>
		public virtual IEntityRelation ProductEntityUsingProductID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Product", false);
				relation.AddEntityFieldPair(ProductFields.ProductID, ProductAttributeFields.ProductID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProductAttributeEntity", true);
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
	internal static class StaticProductAttributeRelations
	{
		internal static readonly IEntityRelation ProductVariantAttributeEntityUsingProductAttributeIDStatic = new ProductAttributeRelations().ProductVariantAttributeEntityUsingProductAttributeID;
		internal static readonly IEntityRelation ProductEntityUsingProductIDStatic = new ProductAttributeRelations().ProductEntityUsingProductID;

		/// <summary>CTor</summary>
		static StaticProductAttributeRelations()
		{
		}
	}
}
