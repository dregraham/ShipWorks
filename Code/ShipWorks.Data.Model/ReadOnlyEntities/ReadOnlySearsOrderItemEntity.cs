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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'SearsOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlySearsOrderItemEntity : ReadOnlyOrderItemEntity, ISearsOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlySearsOrderItemEntity(ISearsOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            LineNumber = source.LineNumber;
            ItemID = source.ItemID;
            Commission = source.Commission;
            Shipping = source.Shipping;
            OnlineStatus = source.OnlineStatus;
            
            
            

            CopyCustomSearsOrderItemData(source);
        }

        
        /// <summary> The LineNumber property of the Entity SearsOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderItem"."LineNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 LineNumber { get; }
        /// <summary> The ItemID property of the Entity SearsOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderItem"."ItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ItemID { get; }
        /// <summary> The Commission property of the Entity SearsOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderItem"."Commission"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal Commission { get; }
        /// <summary> The Shipping property of the Entity SearsOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderItem"."Shipping"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal Shipping { get; }
        /// <summary> The OnlineStatus property of the Entity SearsOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderItem"."OnlineStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OnlineStatus { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ISearsOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ISearsOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomSearsOrderItemData(ISearsOrderItemEntity source);
    }
}
