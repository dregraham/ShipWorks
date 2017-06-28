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
    /// Read-only representation of the entity 'NeweggOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyNeweggOrderEntity : ReadOnlyOrderEntity, INeweggOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyNeweggOrderEntity(INeweggOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            InvoiceNumber = source.InvoiceNumber;
            RefundAmount = source.RefundAmount;
            IsAutoVoid = source.IsAutoVoid;
            
            
            
            NeweggOrderSearch = source.NeweggOrderSearch?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<INeweggOrderSearchEntity>();

            CopyCustomNeweggOrderData(source);
        }

        
        /// <summary> The InvoiceNumber property of the Entity NeweggOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrder"."InvoiceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> InvoiceNumber { get; }
        /// <summary> The RefundAmount property of the Entity NeweggOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrder"."RefundAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> RefundAmount { get; }
        /// <summary> The IsAutoVoid property of the Entity NeweggOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrder"."IsAutoVoid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> IsAutoVoid { get; }
        
        
        
        public IEnumerable<INeweggOrderSearchEntity> NeweggOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INeweggOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INeweggOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomNeweggOrderData(INeweggOrderEntity source);
    }
}
