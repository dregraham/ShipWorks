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
	/// <summary>Implements the relations factory for the entity: OnTracProfile. </summary>
	public partial class OnTracProfileRelations
	{
		/// <summary>CTor</summary>
		public OnTracProfileRelations()
		{
		}

		/// <summary>Gets all relations of the OnTracProfileEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ShippingProfileEntityUsingShippingProfileID);
			return toReturn;
		}

		#region Class Property Declarations


		/// <summary>Returns a new IEntityRelation object, between OnTracProfileEntity and ShippingProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// OnTracProfile.ShippingProfileID - ShippingProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation ShippingProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "ShippingProfile", false);



				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, OnTracProfileFields.ShippingProfileID);

				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OnTracProfileEntity", true);
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
	internal static class StaticOnTracProfileRelations
	{
		internal static readonly IEntityRelation ShippingProfileEntityUsingShippingProfileIDStatic = new OnTracProfileRelations().ShippingProfileEntityUsingShippingProfileID;

		/// <summary>CTor</summary>
		static StaticOnTracProfileRelations()
		{
		}
	}
}
