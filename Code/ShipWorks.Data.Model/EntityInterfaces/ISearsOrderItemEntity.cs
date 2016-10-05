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
    /// Entity interface which represents the entity 'SearsOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ISearsOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The LineNumber property of the Entity SearsOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderItem"."LineNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 LineNumber { get; }
        /// <summary> The ItemID property of the Entity SearsOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderItem"."ItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ItemID { get; }
        /// <summary> The Commission property of the Entity SearsOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderItem"."Commission"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Commission { get; }
        /// <summary> The Shipping property of the Entity SearsOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderItem"."Shipping"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Shipping { get; }
        /// <summary> The OnlineStatus property of the Entity SearsOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderItem"."OnlineStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OnlineStatus { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new ISearsOrderItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new ISearsOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'SearsOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class SearsOrderItemEntity : ISearsOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ISearsOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new ISearsOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ISearsOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlySearsOrderItemEntity(this, objectMap);
        }
    }
}
