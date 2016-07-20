///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Entity interface which represents the entity 'NeweggOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyNeweggOrderItemEntity : ReadOnlyOrderItemEntity, INeweggOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyNeweggOrderItemEntity(INeweggOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            SellerPartNumber = source.SellerPartNumber;
            NeweggItemNumber = source.NeweggItemNumber;
            ManufacturerPartNumber = source.ManufacturerPartNumber;
            ShippingStatusID = source.ShippingStatusID;
            ShippingStatusDescription = source.ShippingStatusDescription;
            QuantityShipped = source.QuantityShipped;
            
            
            

            CopyCustomNeweggOrderItemData(source);
        }

        
        /// <summary> The SellerPartNumber property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."SellerPartNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String SellerPartNumber { get; }
        /// <summary> The NeweggItemNumber property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."NeweggItemNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String NeweggItemNumber { get; }
        /// <summary> The ManufacturerPartNumber property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."ManufacturerPartNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ManufacturerPartNumber { get; }
        /// <summary> The ShippingStatusID property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."ShippingStatusID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ShippingStatusID { get; }
        /// <summary> The ShippingStatusDescription property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."ShippingStatusDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ShippingStatusDescription { get; }
        /// <summary> The QuantityShipped property of the Entity NeweggOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrderItem"."QuantityShipped"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> QuantityShipped { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INeweggOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INeweggOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomNeweggOrderItemData(INeweggOrderItemEntity source);
    }
}
