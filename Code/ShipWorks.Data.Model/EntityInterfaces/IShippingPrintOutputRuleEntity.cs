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
    /// Entity interface which represents the entity 'ShippingPrintOutputRule'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShippingPrintOutputRuleEntity
    {
        
        /// <summary> The ShippingPrintOutputRuleID property of the Entity ShippingPrintOutputRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutputRule"."ShippingPrintOutputRuleID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShippingPrintOutputRuleID { get; }
        /// <summary> The ShippingPrintOutputID property of the Entity ShippingPrintOutputRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutputRule"."ShippingPrintOutputID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShippingPrintOutputID { get; }
        /// <summary> The FilterNodeID property of the Entity ShippingPrintOutputRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutputRule"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterNodeID { get; }
        /// <summary> The TemplateID property of the Entity ShippingPrintOutputRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingPrintOutputRule"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 TemplateID { get; }
        
        
        IShippingPrintOutputEntity ShippingPrintOutput { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingPrintOutputRuleEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingPrintOutputRuleEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShippingPrintOutputRule'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShippingPrintOutputRuleEntity : IShippingPrintOutputRuleEntity
    {
        
        IShippingPrintOutputEntity IShippingPrintOutputRuleEntity.ShippingPrintOutput => ShippingPrintOutput;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingPrintOutputRuleEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShippingPrintOutputRuleEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShippingPrintOutputRuleEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShippingPrintOutputRuleEntity(this, objectMap);
        }
    }
}
