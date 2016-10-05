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
	/// <summary>Implements the relations factory for the entity: ServerMessageSignoff. </summary>
	public partial class ServerMessageSignoffRelations
	{
		/// <summary>CTor</summary>
		public ServerMessageSignoffRelations()
		{
		}

		/// <summary>Gets all relations of the ServerMessageSignoffEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ComputerEntityUsingComputerID);
			toReturn.Add(this.ServerMessageEntityUsingServerMessageID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between ServerMessageSignoffEntity and ComputerEntity over the m:1 relation they have, using the relation between the fields:
		/// ServerMessageSignoff.ComputerID - Computer.ComputerID
		/// </summary>
		public virtual IEntityRelation ComputerEntityUsingComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, ServerMessageSignoffFields.ComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ServerMessageSignoffEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ServerMessageSignoffEntity and ServerMessageEntity over the m:1 relation they have, using the relation between the fields:
		/// ServerMessageSignoff.ServerMessageID - ServerMessage.ServerMessageID
		/// </summary>
		public virtual IEntityRelation ServerMessageEntityUsingServerMessageID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ServerMessage", false);
				relation.AddEntityFieldPair(ServerMessageFields.ServerMessageID, ServerMessageSignoffFields.ServerMessageID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ServerMessageEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ServerMessageSignoffEntity", true);
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
	internal static class StaticServerMessageSignoffRelations
	{
		internal static readonly IEntityRelation ComputerEntityUsingComputerIDStatic = new ServerMessageSignoffRelations().ComputerEntityUsingComputerID;
		internal static readonly IEntityRelation ServerMessageEntityUsingServerMessageIDStatic = new ServerMessageSignoffRelations().ServerMessageEntityUsingServerMessageID;

		/// <summary>CTor</summary>
		static StaticServerMessageSignoffRelations()
		{
		}
	}
}
