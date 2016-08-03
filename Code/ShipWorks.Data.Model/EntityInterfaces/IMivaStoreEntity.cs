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
    /// Entity interface which represents the entity 'MivaStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IMivaStoreEntity: IGenericModuleStoreEntity
    {
        
        /// <summary> The EncryptionPassphrase property of the Entity MivaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaStore"."EncryptionPassphrase"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EncryptionPassphrase { get; }
        /// <summary> The LiveManualOrderNumbers property of the Entity MivaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaStore"."LiveManualOrderNumbers"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean LiveManualOrderNumbers { get; }
        /// <summary> The SebenzaCheckoutDataEnabled property of the Entity MivaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaStore"."SebenzaCheckoutDataEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean SebenzaCheckoutDataEnabled { get; }
        /// <summary> The OnlineUpdateStrategy property of the Entity MivaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaStore"."OnlineUpdateStrategy"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 OnlineUpdateStrategy { get; }
        /// <summary> The OnlineUpdateStatusChangeEmail property of the Entity MivaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaStore"."OnlineUpdateStatusChangeEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean OnlineUpdateStatusChangeEmail { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IMivaStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IMivaStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'MivaStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class MivaStoreEntity : IMivaStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IMivaStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IMivaStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IMivaStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyMivaStoreEntity(this, objectMap);
        }
    }
}
