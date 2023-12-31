﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'FedExEndOfDayClose'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFedExEndOfDayCloseEntity
    {
        
        /// <summary> The FedExEndOfDayCloseID property of the Entity FedExEndOfDayClose<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExEndOfDayClose"."FedExEndOfDayCloseID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 FedExEndOfDayCloseID { get; }
        /// <summary> The FedExAccountID property of the Entity FedExEndOfDayClose<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExEndOfDayClose"."FedExAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FedExAccountID { get; }
        /// <summary> The AccountNumber property of the Entity FedExEndOfDayClose<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExEndOfDayClose"."AccountNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AccountNumber { get; }
        /// <summary> The CloseDate property of the Entity FedExEndOfDayClose<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExEndOfDayClose"."CloseDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CloseDate { get; }
        /// <summary> The IsSmartPost property of the Entity FedExEndOfDayClose<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FedExEndOfDayClose"."IsSmartPost"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsSmartPost { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFedExEndOfDayCloseEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFedExEndOfDayCloseEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'FedExEndOfDayClose'. <br/><br/>
    /// 
    /// </summary>
    public partial class FedExEndOfDayCloseEntity : IFedExEndOfDayCloseEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFedExEndOfDayCloseEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFedExEndOfDayCloseEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFedExEndOfDayCloseEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFedExEndOfDayCloseEntity(this, objectMap);
        }

        
    }
}
