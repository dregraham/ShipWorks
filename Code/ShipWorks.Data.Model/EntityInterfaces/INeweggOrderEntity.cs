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
    /// Entity interface which represents the entity 'NeweggOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface INeweggOrderEntity: IOrderEntity
    {
        
        /// <summary> The InvoiceNumber property of the Entity NeweggOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrder"."InvoiceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> InvoiceNumber { get; }
        /// <summary> The RefundAmount property of the Entity NeweggOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrder"."RefundAmount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Decimal> RefundAmount { get; }
        /// <summary> The IsAutoVoid property of the Entity NeweggOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggOrder"."IsAutoVoid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> IsAutoVoid { get; }
        
        
        
        IEnumerable<INeweggOrderSearchEntity> NeweggOrderSearch { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new INeweggOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new INeweggOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'NeweggOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class NeweggOrderEntity : INeweggOrderEntity
    {
        
        
        IEnumerable<INeweggOrderSearchEntity> INeweggOrderEntity.NeweggOrderSearch => NeweggOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INeweggOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new INeweggOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (INeweggOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyNeweggOrderEntity(this, objectMap);
        }
    }
}
