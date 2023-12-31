﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'SearsOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ISearsOrderEntity: IOrderEntity
    {
        
        /// <summary> The PoNumber property of the Entity SearsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrder"."PoNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PoNumber { get; }
        /// <summary> The PoNumberWithDate property of the Entity SearsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrder"."PoNumberWithDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PoNumberWithDate { get; }
        /// <summary> The LocationID property of the Entity SearsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrder"."LocationID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 LocationID { get; }
        /// <summary> The Commission property of the Entity SearsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrder"."Commission"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Commission { get; }
        /// <summary> The CustomerPickup property of the Entity SearsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrder"."CustomerPickup"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CustomerPickup { get; }
        
        
        
        IEnumerable<ISearsOrderSearchEntity> SearsOrderSearch { get; }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ISearsOrderEntity AsReadOnlySearsOrder();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ISearsOrderEntity AsReadOnlySearsOrder(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'SearsOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class SearsOrderEntity : ISearsOrderEntity
    {
        
        
        IEnumerable<ISearsOrderSearchEntity> ISearsOrderEntity.SearsOrderSearch => SearsOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ISearsOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlySearsOrderEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public ISearsOrderEntity AsReadOnlySearsOrder() =>
            (ISearsOrderEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public ISearsOrderEntity AsReadOnlySearsOrder(IDictionary<object, object> objectMap) =>
            (ISearsOrderEntity) AsReadOnly(objectMap);
        
    }
}
