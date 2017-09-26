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
    /// Read-only representation of the entity 'UpsLetterRate'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUpsLetterRateEntity : IUpsLetterRateEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUpsLetterRateEntity(IUpsLetterRateEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UpsLetterRateID = source.UpsLetterRateID;
            UpsRateTableID = source.UpsRateTableID;
            Zone = source.Zone;
            Service = source.Service;
            Rate = source.Rate;
            
            
            UpsRateTable = (IUpsRateTableEntity) source.UpsRateTable?.AsReadOnly(objectMap);
            

            CopyCustomUpsLetterRateData(source);
        }

        
        /// <summary> The UpsLetterRateID property of the Entity UpsLetterRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLetterRate"."UpsLetterRateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 UpsLetterRateID { get; }
        /// <summary> The UpsRateTableID property of the Entity UpsLetterRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLetterRate"."UpsRateTableID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UpsRateTableID { get; }
        /// <summary> The Zone property of the Entity UpsLetterRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLetterRate"."Zone"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Zone { get; }
        /// <summary> The Service property of the Entity UpsLetterRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLetterRate"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Service { get; }
        /// <summary> The Rate property of the Entity UpsLetterRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLetterRate"."Rate"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal Rate { get; }
        
        
        public IUpsRateTableEntity UpsRateTable { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLetterRateEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLetterRateEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUpsLetterRateData(IUpsLetterRateEntity source);
    }
}
