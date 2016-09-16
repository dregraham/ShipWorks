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
    /// Read-only representation of the entity 'FedExEndOfDayClose'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFedExEndOfDayCloseEntity : IFedExEndOfDayCloseEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFedExEndOfDayCloseEntity(IFedExEndOfDayCloseEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FedExEndOfDayCloseID = source.FedExEndOfDayCloseID;
            FedExAccountID = source.FedExAccountID;
            AccountNumber = source.AccountNumber;
            CloseDate = source.CloseDate;
            IsSmartPost = source.IsSmartPost;
            
            
            

            CopyCustomFedExEndOfDayCloseData(source);
        }

        
        /// <summary> The FedExEndOfDayCloseID property of the Entity FedExEndOfDayClose<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExEndOfDayClose"."FedExEndOfDayCloseID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 FedExEndOfDayCloseID { get; }
        /// <summary> The FedExAccountID property of the Entity FedExEndOfDayClose<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExEndOfDayClose"."FedExAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FedExAccountID { get; }
        /// <summary> The AccountNumber property of the Entity FedExEndOfDayClose<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExEndOfDayClose"."AccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AccountNumber { get; }
        /// <summary> The CloseDate property of the Entity FedExEndOfDayClose<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExEndOfDayClose"."CloseDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime CloseDate { get; }
        /// <summary> The IsSmartPost property of the Entity FedExEndOfDayClose<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExEndOfDayClose"."IsSmartPost"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsSmartPost { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExEndOfDayCloseEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExEndOfDayCloseEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFedExEndOfDayCloseData(IFedExEndOfDayCloseEntity source);
    }
}
