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
    /// Read-only representation of the entity 'ShippingPrintOutputRule'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShippingPrintOutputRuleEntity : IShippingPrintOutputRuleEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShippingPrintOutputRuleEntity(IShippingPrintOutputRuleEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingPrintOutputRuleID = source.ShippingPrintOutputRuleID;
            ShippingPrintOutputID = source.ShippingPrintOutputID;
            FilterNodeID = source.FilterNodeID;
            TemplateID = source.TemplateID;
            
            
            ShippingPrintOutput = (IShippingPrintOutputEntity) source.ShippingPrintOutput?.AsReadOnly(objectMap);
            

            CopyCustomShippingPrintOutputRuleData(source);
        }

        
        /// <summary> The ShippingPrintOutputRuleID property of the Entity ShippingPrintOutputRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutputRule"."ShippingPrintOutputRuleID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ShippingPrintOutputRuleID { get; }
        /// <summary> The ShippingPrintOutputID property of the Entity ShippingPrintOutputRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutputRule"."ShippingPrintOutputID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShippingPrintOutputID { get; }
        /// <summary> The FilterNodeID property of the Entity ShippingPrintOutputRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutputRule"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterNodeID { get; }
        /// <summary> The TemplateID property of the Entity ShippingPrintOutputRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutputRule"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 TemplateID { get; }
        
        
        public IShippingPrintOutputEntity ShippingPrintOutput { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingPrintOutputRuleEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingPrintOutputRuleEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShippingPrintOutputRuleData(IShippingPrintOutputRuleEntity source);
    }
}
