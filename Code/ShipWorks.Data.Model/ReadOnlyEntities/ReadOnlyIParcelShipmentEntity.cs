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
    /// Read-only representation of the entity 'IParcelShipment'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyIParcelShipmentEntity : IIParcelShipmentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyIParcelShipmentEntity(IIParcelShipmentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShipmentID = source.ShipmentID;
            IParcelAccountID = source.IParcelAccountID;
            Service = source.Service;
            Reference = source.Reference;
            TrackByEmail = source.TrackByEmail;
            TrackBySMS = source.TrackBySMS;
            IsDeliveryDutyPaid = source.IsDeliveryDutyPaid;
            RequestedLabelFormat = source.RequestedLabelFormat;
            
            Shipment = (IShipmentEntity) source.Shipment?.AsReadOnly(objectMap);
            
            
            Packages = source.Packages?.Select(x => x.AsReadOnly(objectMap)).OfType<IIParcelPackageEntity>().ToReadOnly() ??
                Enumerable.Empty<IIParcelPackageEntity>();

            CopyCustomIParcelShipmentData(source);
        }

        
        /// <summary> The ShipmentID property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."ShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShipmentID { get; }
        /// <summary> The IParcelAccountID property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."iParcelAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 IParcelAccountID { get; }
        /// <summary> The Service property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Service { get; }
        /// <summary> The Reference property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."Reference"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Reference { get; }
        /// <summary> The TrackByEmail property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."TrackByEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean TrackByEmail { get; }
        /// <summary> The TrackBySMS property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."TrackBySMS"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean TrackBySMS { get; }
        /// <summary> The IsDeliveryDutyPaid property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."IsDeliveryDutyPaid"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsDeliveryDutyPaid { get; }
        /// <summary> The RequestedLabelFormat property of the Entity IParcelShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelShipment"."RequestedLabelFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RequestedLabelFormat { get; }
        
        public IShipmentEntity Shipment { get; }
        
        
        
        public IEnumerable<IIParcelPackageEntity> Packages { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelShipmentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelShipmentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomIParcelShipmentData(IIParcelShipmentEntity source);
    }
}
