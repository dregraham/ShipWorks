﻿///////////////////////////////////////////////////////////////
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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'EtsyOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEtsyOrderEntity: IOrderEntity
    {
        
        /// <summary> The WasPaid property of the Entity EtsyOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyOrder"."WasPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean WasPaid { get; }
        /// <summary> The WasShipped property of the Entity EtsyOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyOrder"."WasShipped"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean WasShipped { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IEtsyOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IEtsyOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EtsyOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class EtsyOrderEntity : IEtsyOrderEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IEtsyOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IEtsyOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEtsyOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEtsyOrderEntity(this, objectMap);
        }
    }
}
