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
    /// Entity interface which represents the entity 'NeweggOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface INeweggOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The SellerPartNumber property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."SellerPartNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String SellerPartNumber { get; }
        /// <summary> The NeweggItemNumber property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."NeweggItemNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String NeweggItemNumber { get; }
        /// <summary> The ManufacturerPartNumber property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."ManufacturerPartNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ManufacturerPartNumber { get; }
        /// <summary> The ShippingStatusID property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."ShippingStatusID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ShippingStatusID { get; }
        /// <summary> The ShippingStatusDescription property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."ShippingStatusDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ShippingStatusDescription { get; }
        /// <summary> The QuantityShipped property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."QuantityShipped"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> QuantityShipped { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new INeweggOrderItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new INeweggOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'NeweggOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class NeweggOrderItemEntity : INeweggOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INeweggOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new INeweggOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (INeweggOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyNeweggOrderItemEntity(this, objectMap);
        }
    }
}
