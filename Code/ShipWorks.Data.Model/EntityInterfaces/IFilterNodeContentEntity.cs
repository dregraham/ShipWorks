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
    /// Entity interface which represents the entity 'FilterNodeContent'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFilterNodeContentEntity
    {
        
        /// <summary> The FilterNodeContentID property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."FilterNodeContentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 FilterNodeContentID { get; }
        /// <summary> The RowVersion property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The CountVersion property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."CountVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 CountVersion { get; }
        /// <summary> The Status property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."Status"<br/>
        /// Table field type characteristics (type, precision, scale, length): SmallInt, 5, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int16 Status { get; }
        /// <summary> The InitialCalculation property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."InitialCalculation"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String InitialCalculation { get; }
        /// <summary> The UpdateCalculation property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."UpdateCalculation"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UpdateCalculation { get; }
        /// <summary> The ColumnMask property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."ColumnMask"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] ColumnMask { get; }
        /// <summary> The JoinMask property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."JoinMask"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 JoinMask { get; }
        /// <summary> The Cost property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."Cost"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Cost { get; }
        /// <summary> The Count property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."Count"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Count { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterNodeContentEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFilterNodeContentEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'FilterNodeContent'. <br/><br/>
    /// 
    /// </summary>
    public partial class FilterNodeContentEntity : IFilterNodeContentEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeContentEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFilterNodeContentEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFilterNodeContentEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFilterNodeContentEntity(this, objectMap);
        }
    }
}
