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
    /// Read-only representation of the entity 'FilterNodeContent'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFilterNodeContentEntity : IFilterNodeContentEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFilterNodeContentEntity(IFilterNodeContentEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FilterNodeContentID = source.FilterNodeContentID;
            RowVersion = source.RowVersion;
            CountVersion = source.CountVersion;
            Status = source.Status;
            InitialCalculation = source.InitialCalculation;
            UpdateCalculation = source.UpdateCalculation;
            ColumnMask = source.ColumnMask;
            JoinMask = source.JoinMask;
            Cost = source.Cost;
            Count = source.Count;
            
            
            

            CopyCustomFilterNodeContentData(source);
        }

        
        /// <summary> The FilterNodeContentID property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."FilterNodeContentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 FilterNodeContentID { get; }
        /// <summary> The RowVersion property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The CountVersion property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."CountVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 CountVersion { get; }
        /// <summary> The Status property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."Status"<br/>
        /// Table field type characteristics (type, precision, scale, length): SmallInt, 5, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int16 Status { get; }
        /// <summary> The InitialCalculation property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."InitialCalculation"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InitialCalculation { get; }
        /// <summary> The UpdateCalculation property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."UpdateCalculation"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String UpdateCalculation { get; }
        /// <summary> The ColumnMask property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."ColumnMask"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] ColumnMask { get; }
        /// <summary> The JoinMask property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."JoinMask"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 JoinMask { get; }
        /// <summary> The Cost property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."Cost"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Cost { get; }
        /// <summary> The Count property of the Entity FilterNodeContent<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FilterNodeContent"."Count"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Count { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeContentEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFilterNodeContentEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFilterNodeContentData(IFilterNodeContentEntity source);
    }
}
