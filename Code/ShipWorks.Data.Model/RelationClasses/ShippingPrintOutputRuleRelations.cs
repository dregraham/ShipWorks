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
	/// <summary>Implements the relations factory for the entity: ShippingPrintOutputRule. </summary>
	public partial class ShippingPrintOutputRuleRelations
	{
		/// <summary>CTor</summary>
		public ShippingPrintOutputRuleRelations()
		{
		}

		/// <summary>Gets all relations of the ShippingPrintOutputRuleEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ShippingPrintOutputEntityUsingShippingPrintOutputID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between ShippingPrintOutputRuleEntity and ShippingPrintOutputEntity over the m:1 relation they have, using the relation between the fields:
		/// ShippingPrintOutputRule.ShippingPrintOutputID - ShippingPrintOutput.ShippingPrintOutputID
		/// </summary>
		public virtual IEntityRelation ShippingPrintOutputEntityUsingShippingPrintOutputID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ShippingPrintOutput", false);
				relation.AddEntityFieldPair(ShippingPrintOutputFields.ShippingPrintOutputID, ShippingPrintOutputRuleFields.ShippingPrintOutputID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingPrintOutputEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingPrintOutputRuleEntity", true);
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
	internal static class StaticShippingPrintOutputRuleRelations
	{
		internal static readonly IEntityRelation ShippingPrintOutputEntityUsingShippingPrintOutputIDStatic = new ShippingPrintOutputRuleRelations().ShippingPrintOutputEntityUsingShippingPrintOutputID;

		/// <summary>CTor</summary>
		static StaticShippingPrintOutputRuleRelations()
		{
		}
	}
}
