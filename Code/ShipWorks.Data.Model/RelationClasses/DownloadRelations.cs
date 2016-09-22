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
	/// <summary>Implements the relations factory for the entity: Download. </summary>
	public partial class DownloadRelations
	{
		/// <summary>CTor</summary>
		public DownloadRelations()
		{
		}

		/// <summary>Gets all relations of the DownloadEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.DownloadDetailEntityUsingDownloadID);
			toReturn.Add(this.ComputerEntityUsingComputerID);
			toReturn.Add(this.StoreEntityUsingStoreID);
			toReturn.Add(this.UserEntityUsingUserID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between DownloadEntity and DownloadDetailEntity over the 1:n relation they have, using the relation between the fields:
		/// Download.DownloadID - DownloadDetail.DownloadID
		/// </summary>
		public virtual IEntityRelation DownloadDetailEntityUsingDownloadID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(DownloadFields.DownloadID, DownloadDetailFields.DownloadID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadDetailEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between DownloadEntity and ComputerEntity over the m:1 relation they have, using the relation between the fields:
		/// Download.ComputerID - Computer.ComputerID
		/// </summary>
		public virtual IEntityRelation ComputerEntityUsingComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Computer", false);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, DownloadFields.ComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between DownloadEntity and StoreEntity over the m:1 relation they have, using the relation between the fields:
		/// Download.StoreID - Store.StoreID
		/// </summary>
		public virtual IEntityRelation StoreEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Store", false);
				relation.AddEntityFieldPair(StoreFields.StoreID, DownloadFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StoreEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between DownloadEntity and UserEntity over the m:1 relation they have, using the relation between the fields:
		/// Download.UserID - User.UserID
		/// </summary>
		public virtual IEntityRelation UserEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "User", false);
				relation.AddEntityFieldPair(UserFields.UserID, DownloadFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", true);
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
	internal static class StaticDownloadRelations
	{
		internal static readonly IEntityRelation DownloadDetailEntityUsingDownloadIDStatic = new DownloadRelations().DownloadDetailEntityUsingDownloadID;
		internal static readonly IEntityRelation ComputerEntityUsingComputerIDStatic = new DownloadRelations().ComputerEntityUsingComputerID;
		internal static readonly IEntityRelation StoreEntityUsingStoreIDStatic = new DownloadRelations().StoreEntityUsingStoreID;
		internal static readonly IEntityRelation UserEntityUsingUserIDStatic = new DownloadRelations().UserEntityUsingUserID;

		/// <summary>CTor</summary>
		static StaticDownloadRelations()
		{
		}
	}
}
