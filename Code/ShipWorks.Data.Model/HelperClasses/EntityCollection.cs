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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
#if !CF
using System.Runtime.Serialization;
#endif
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

using ShipWorks.Data.Model.EntityClasses;

using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.HelperClasses
{
	
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END

	/// <summary>
	/// EntityCollection implementation which is used for backwards compatibility and for design time databinding. 
	/// This EntityCollection is an EntityCollection(Of EntityBase2)
	/// </summary>
	[Serializable]
	public partial class EntityCollection : EntityCollectionNonGeneric
	{
		/// <summary>CTor</summary>
		public EntityCollection():base()
		{
		}

		/// <summary>CTor</summary>
		/// <param name="entityFactoryToUse">The entity factory object to use when this collection has to construct new objects.
		/// This is the case when the collection is bound to a grid-like control for example.</param>
		public EntityCollection(IEntityFactory2 entityFactoryToUse):base(entityFactoryToUse)
		{
		}

		/// <summary>Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected EntityCollection(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		
		#region Custom EntityCollection code
		
		// __LLBLGENPRO_USER_CODE_REGION_START CustomEntityCollectionCode
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion
		
		#region Included Code

		#endregion
	}
	

	/// <summary>
	/// Generic entity collection class which replaces the original generated, non strongly typed EntityCollection variant. 
	/// </summary>
	/// <remarks>Use the generated, non-generic EntityCollection class for design-time databinding, as VS.NET 2005 doesn't support 
	/// design time databinding (runtime-databinding works fine though) with generic classes</remarks>
	[Serializable]
	public partial class EntityCollection<TEntity> : EntityCollectionBase2<TEntity>
		where TEntity : EntityBase2, IEntity2
	{
		/// <summary>
		/// Constructor for WCF serialization. In your code, please use the constructors which accept a factory to avoid having a collection without
		/// a factory.
		/// </summary>
		public EntityCollection()
			: base( (IEntityFactory2)null )
		{
		}

		/// <summary>
		/// CTor
		/// </summary>
		/// <param name="entityFactoryToUse">The entity factory object to use when this collection has to construct new objects.
		/// This is the case when the collection is bound to a grid-like control for example.</param>
		public EntityCollection( IEntityFactory2 entityFactoryToUse )
			: base( entityFactoryToUse )
		{
		}


		/// <summary>
		/// CTor
		/// </summary>
		/// <param name="initialContents">initial contents for this collection</param>
		public EntityCollection( IList<TEntity> initialContents )
			: base( initialContents )
		{
		}


		/// <summary>
		/// Protected CTor for deserialization
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected EntityCollection( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
		}

		
		#region Custom EntityCollection code
		
		// __LLBLGENPRO_USER_CODE_REGION_START CustomEntityCollectionCodeGeneric
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion
		
		#region Included Code

		#endregion		
	}
}
