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
    /// Read-only representation of the entity 'ShippingPrintOutput'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShippingPrintOutputEntity : IShippingPrintOutputEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShippingPrintOutputEntity(IShippingPrintOutputEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingPrintOutputID = source.ShippingPrintOutputID;
            RowVersion = source.RowVersion;
            ShipmentType = source.ShipmentType;
            Name = source.Name;
            
            
            
            Rules = source.Rules?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IShippingPrintOutputRuleEntity>();

            CopyCustomShippingPrintOutputData(source);
        }

        
        /// <summary> The ShippingPrintOutputID property of the Entity ShippingPrintOutput<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutput"."ShippingPrintOutputID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ShippingPrintOutputID { get; }
        /// <summary> The RowVersion property of the Entity ShippingPrintOutput<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutput"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ShipmentType property of the Entity ShippingPrintOutput<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutput"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipmentType { get; }
        /// <summary> The Name property of the Entity ShippingPrintOutput<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutput"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        
        
        
        public IEnumerable<IShippingPrintOutputRuleEntity> Rules { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingPrintOutputEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingPrintOutputEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShippingPrintOutputData(IShippingPrintOutputEntity source);
    }
}
