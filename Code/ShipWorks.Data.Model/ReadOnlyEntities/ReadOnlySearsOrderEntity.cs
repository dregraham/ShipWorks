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
    /// Read-only representation of the entity 'SearsOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlySearsOrderEntity : ReadOnlyOrderEntity, ISearsOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlySearsOrderEntity(ISearsOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            PoNumber = source.PoNumber;
            PoNumberWithDate = source.PoNumberWithDate;
            LocationID = source.LocationID;
            Commission = source.Commission;
            CustomerPickup = source.CustomerPickup;
            
            
            
            SearsOrderSearch = source.SearsOrderSearch?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<ISearsOrderSearchEntity>();

            CopyCustomSearsOrderData(source);
        }

        
        /// <summary> The PoNumber property of the Entity SearsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrder"."PoNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PoNumber { get; }
        /// <summary> The PoNumberWithDate property of the Entity SearsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrder"."PoNumberWithDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PoNumberWithDate { get; }
        /// <summary> The LocationID property of the Entity SearsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrder"."LocationID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 LocationID { get; }
        /// <summary> The Commission property of the Entity SearsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrder"."Commission"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal Commission { get; }
        /// <summary> The CustomerPickup property of the Entity SearsOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrder"."CustomerPickup"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean CustomerPickup { get; }
        
        
        
        public IEnumerable<ISearsOrderSearchEntity> SearsOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ISearsOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ISearsOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomSearsOrderData(ISearsOrderEntity source);
    }
}
