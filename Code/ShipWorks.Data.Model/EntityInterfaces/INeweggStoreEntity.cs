///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'NeweggStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface INeweggStoreEntity: IStoreEntity
    {
        
        /// <summary> The SellerID property of the Entity NeweggStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggStore"."SellerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SellerID { get; }
        /// <summary> The SecretKey property of the Entity NeweggStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggStore"."SecretKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SecretKey { get; }
        /// <summary> The ExcludeFulfilledByNewegg property of the Entity NeweggStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggStore"."ExcludeFulfilledByNewegg"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ExcludeFulfilledByNewegg { get; }
        /// <summary> The Channel property of the Entity NeweggStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggStore"."Channel"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Channel { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new INeweggStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new INeweggStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'NeweggStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class NeweggStoreEntity : INeweggStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INeweggStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new INeweggStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (INeweggStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyNeweggStoreEntity(this, objectMap);
        }
    }
}
