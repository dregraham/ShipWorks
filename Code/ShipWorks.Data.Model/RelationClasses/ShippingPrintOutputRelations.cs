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
	/// <summary>Implements the relations factory for the entity: ShippingPrintOutput. </summary>
	public partial class ShippingPrintOutputRelations
	{
		/// <summary>CTor</summary>
		public ShippingPrintOutputRelations()
		{
		}

		/// <summary>Gets all relations of the ShippingPrintOutputEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ShippingPrintOutputRuleEntityUsingShippingPrintOutputID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ShippingPrintOutputEntity and ShippingPrintOutputRuleEntity over the 1:n relation they have, using the relation between the fields:
		/// ShippingPrintOutput.ShippingPrintOutputID - ShippingPrintOutputRule.ShippingPrintOutputID
		/// </summary>
		public virtual IEntityRelation ShippingPrintOutputRuleEntityUsingShippingPrintOutputID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Rules" , true);
				relation.AddEntityFieldPair(ShippingPrintOutputFields.ShippingPrintOutputID, ShippingPrintOutputRuleFields.ShippingPrintOutputID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingPrintOutputEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingPrintOutputRuleEntity", false);
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
	internal static class StaticShippingPrintOutputRelations
	{
		internal static readonly IEntityRelation ShippingPrintOutputRuleEntityUsingShippingPrintOutputIDStatic = new ShippingPrintOutputRelations().ShippingPrintOutputRuleEntityUsingShippingPrintOutputID;

		/// <summary>CTor</summary>
		static StaticShippingPrintOutputRelations()
		{
		}
	}
}
