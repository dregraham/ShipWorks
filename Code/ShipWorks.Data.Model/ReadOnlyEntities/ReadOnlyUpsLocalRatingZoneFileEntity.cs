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
    /// Read-only representation of the entity 'UpsLocalRatingZoneFile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUpsLocalRatingZoneFileEntity : IUpsLocalRatingZoneFileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUpsLocalRatingZoneFileEntity(IUpsLocalRatingZoneFileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ZoneFileID = source.ZoneFileID;
            UploadDate = source.UploadDate;
            FileContent = source.FileContent;
            
            
            
            UpsLocalRatingDeliveryAreaSurcharge = source.UpsLocalRatingDeliveryAreaSurcharge?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IUpsLocalRatingDeliveryAreaSurchargeEntity>();
            UpsLocalRatingZone = source.UpsLocalRatingZone?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IUpsLocalRatingZoneEntity>();

            CopyCustomUpsLocalRatingZoneFileData(source);
        }

        
        /// <summary> The ZoneFileID property of the Entity UpsLocalRatingZoneFile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZoneFile"."ZoneFileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ZoneFileID { get; }
        /// <summary> The UploadDate property of the Entity UpsLocalRatingZoneFile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZoneFile"."UploadDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime2, 7, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime UploadDate { get; }
        /// <summary> The FileContent property of the Entity UpsLocalRatingZoneFile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UpsLocalRatingZoneFile"."FileContent"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] FileContent { get; }
        
        
        
        public IEnumerable<IUpsLocalRatingDeliveryAreaSurchargeEntity> UpsLocalRatingDeliveryAreaSurcharge { get; }
        
        public IEnumerable<IUpsLocalRatingZoneEntity> UpsLocalRatingZone { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLocalRatingZoneFileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUpsLocalRatingZoneFileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUpsLocalRatingZoneFileData(IUpsLocalRatingZoneFileEntity source);
    }
}
