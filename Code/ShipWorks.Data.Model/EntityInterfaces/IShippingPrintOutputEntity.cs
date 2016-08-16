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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'ShippingPrintOutput'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShippingPrintOutputEntity
    {
        
        /// <summary> The ShippingPrintOutputID property of the Entity ShippingPrintOutput<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutput"."ShippingPrintOutputID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShippingPrintOutputID { get; }
        /// <summary> The RowVersion property of the Entity ShippingPrintOutput<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutput"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ShipmentType property of the Entity ShippingPrintOutput<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutput"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipmentType { get; }
        /// <summary> The Name property of the Entity ShippingPrintOutput<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutput"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        
        
        
        IEnumerable<IShippingPrintOutputRuleEntity> Rules { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingPrintOutputEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingPrintOutputEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShippingPrintOutput'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShippingPrintOutputEntity : IShippingPrintOutputEntity
    {
        
        
        IEnumerable<IShippingPrintOutputRuleEntity> IShippingPrintOutputEntity.Rules => Rules;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingPrintOutputEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShippingPrintOutputEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShippingPrintOutputEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShippingPrintOutputEntity(this, objectMap);
        }
    }
}
