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
	/// <summary>Implements the relations factory for the entity: EmailOutbound. </summary>
	public partial class EmailOutboundRelations
	{
		/// <summary>CTor</summary>
		public EmailOutboundRelations()
		{
		}

		/// <summary>Gets all relations of the EmailOutboundEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.EmailOutboundRelationEntityUsingEmailOutboundID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between EmailOutboundEntity and EmailOutboundRelationEntity over the 1:n relation they have, using the relation between the fields:
		/// EmailOutbound.EmailOutboundID - EmailOutboundRelation.EmailOutboundID
		/// </summary>
		public virtual IEntityRelation EmailOutboundRelationEntityUsingEmailOutboundID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "RelatedObjects" , true);
				relation.AddEntityFieldPair(EmailOutboundFields.EmailOutboundID, EmailOutboundRelationFields.EmailOutboundID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EmailOutboundEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EmailOutboundRelationEntity", false);
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
	internal static class StaticEmailOutboundRelations
	{
		internal static readonly IEntityRelation EmailOutboundRelationEntityUsingEmailOutboundIDStatic = new EmailOutboundRelations().EmailOutboundRelationEntityUsingEmailOutboundID;

		/// <summary>CTor</summary>
		static StaticEmailOutboundRelations()
		{
		}
	}
}
