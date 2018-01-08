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
    /// Read-only representation of the entity 'GenericModuleOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyGenericModuleOrderEntity : ReadOnlyOrderEntity, IGenericModuleOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyGenericModuleOrderEntity(IGenericModuleOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AmazonOrderID = source.AmazonOrderID;
            IsFBA = source.IsFBA;
            IsPrime = source.IsPrime;
            IsSameDay = source.IsSameDay;
            
            
            

            CopyCustomGenericModuleOrderData(source);
        }

        
        /// <summary> The AmazonOrderID property of the Entity GenericModuleOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleOrder"."AmazonOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AmazonOrderID { get; }
        /// <summary> The IsFBA property of the Entity GenericModuleOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleOrder"."IsFBA"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsFBA { get; }
        /// <summary> The IsPrime property of the Entity GenericModuleOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleOrder"."IsPrime"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 IsPrime { get; }
        /// <summary> The IsSameDay property of the Entity GenericModuleOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleOrder"."IsSameDay"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsSameDay { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IGenericModuleOrderEntity AsReadOnlyGenericModuleOrder() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IGenericModuleOrderEntity AsReadOnlyGenericModuleOrder(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomGenericModuleOrderData(IGenericModuleOrderEntity source);
    }
}
