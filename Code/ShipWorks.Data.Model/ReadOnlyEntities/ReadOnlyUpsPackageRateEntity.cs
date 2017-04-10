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
    /// Read-only representation of the entity 'UpsPackageRate'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUpsPackageRateEntity : IUpsPackageRateEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUpsPackageRateEntity(IUpsPackageRateEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UpsPackageRateID = source.UpsPackageRateID;
            UpsRateTableID = source.UpsRateTableID;
            Zone = source.Zone;
            WeightInPounds = source.WeightInPounds;
            Service = source.Service;
            Rate = source.Rate;
            
            
            UpsRateTable = source.UpsRateTable?.AsReadOnly(objectMap);
            

            CopyCustomUpsPackageRateData(source);
        }

        
        /// <summary> The UpsPackageRateID property of the Entity UpsPackageRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackageRate"."UpsPackageRateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 UpsPackageRateID { get; }
        /// <summary> The UpsRateTableID property of the Entity UpsPackageRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackageRate"."UpsRateTableID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UpsRateTableID { get; }
        /// <summary> The Zone property of the Entity UpsPackageRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackageRate"."Zone"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Zone { get; }
        /// <summary> The WeightInPounds property of the Entity UpsPackageRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackageRate"."WeightInPounds"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 WeightInPounds { get; }
        /// <summary> The Service property of the Entity UpsPackageRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackageRate"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Service { get; }
        /// <summary> The Rate property of the Entity UpsPackageRate<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsPackageRate"."Rate"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal Rate { get; }
        
        
        public IUpsRateTableEntity UpsRateTable { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsPackageRateEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsPackageRateEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUpsPackageRateData(IUpsPackageRateEntity source);
    }
}
