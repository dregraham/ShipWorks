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
    /// Read-only representation of the entity 'Customer'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyCustomerEntity : ICustomerEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyCustomerEntity(ICustomerEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            CustomerID = source.CustomerID;
            RowVersion = source.RowVersion;
            BillFirstName = source.BillFirstName;
            BillMiddleName = source.BillMiddleName;
            BillLastName = source.BillLastName;
            BillCompany = source.BillCompany;
            BillStreet1 = source.BillStreet1;
            BillStreet2 = source.BillStreet2;
            BillStreet3 = source.BillStreet3;
            BillCity = source.BillCity;
            BillStateProvCode = source.BillStateProvCode;
            BillPostalCode = source.BillPostalCode;
            BillCountryCode = source.BillCountryCode;
            BillPhone = source.BillPhone;
            BillFax = source.BillFax;
            BillEmail = source.BillEmail;
            BillWebsite = source.BillWebsite;
            ShipFirstName = source.ShipFirstName;
            ShipMiddleName = source.ShipMiddleName;
            ShipLastName = source.ShipLastName;
            ShipCompany = source.ShipCompany;
            ShipStreet1 = source.ShipStreet1;
            ShipStreet2 = source.ShipStreet2;
            ShipStreet3 = source.ShipStreet3;
            ShipCity = source.ShipCity;
            ShipStateProvCode = source.ShipStateProvCode;
            ShipPostalCode = source.ShipPostalCode;
            ShipCountryCode = source.ShipCountryCode;
            ShipPhone = source.ShipPhone;
            ShipFax = source.ShipFax;
            ShipEmail = source.ShipEmail;
            ShipWebsite = source.ShipWebsite;
            RollupOrderCount = source.RollupOrderCount;
            RollupOrderTotal = source.RollupOrderTotal;
            RollupNoteCount = source.RollupNoteCount;
            
            
            
            Order = source.Order?.Select(x => x.AsReadOnly(objectMap)).OfType<IOrderEntity>().ToReadOnly() ??
                Enumerable.Empty<IOrderEntity>();

            CopyCustomCustomerData(source);
        }

        
        /// <summary> The CustomerID property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."CustomerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 CustomerID { get; }
        /// <summary> The RowVersion property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The BillFirstName property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillFirstName { get; }
        /// <summary> The BillMiddleName property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillMiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillMiddleName { get; }
        /// <summary> The BillLastName property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillLastName { get; }
        /// <summary> The BillCompany property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillCompany { get; }
        /// <summary> The BillStreet1 property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillStreet1 { get; }
        /// <summary> The BillStreet2 property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillStreet2 { get; }
        /// <summary> The BillStreet3 property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillStreet3 { get; }
        /// <summary> The BillCity property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillCity { get; }
        /// <summary> The BillStateProvCode property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillStateProvCode { get; }
        /// <summary> The BillPostalCode property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillPostalCode { get; }
        /// <summary> The BillCountryCode property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillCountryCode { get; }
        /// <summary> The BillPhone property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillPhone { get; }
        /// <summary> The BillFax property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillFax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillFax { get; }
        /// <summary> The BillEmail property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillEmail { get; }
        /// <summary> The BillWebsite property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."BillWebsite"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BillWebsite { get; }
        /// <summary> The ShipFirstName property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipFirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipFirstName { get; }
        /// <summary> The ShipMiddleName property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipMiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipMiddleName { get; }
        /// <summary> The ShipLastName property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipLastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipLastName { get; }
        /// <summary> The ShipCompany property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipCompany"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipCompany { get; }
        /// <summary> The ShipStreet1 property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipStreet1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStreet1 { get; }
        /// <summary> The ShipStreet2 property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipStreet2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStreet2 { get; }
        /// <summary> The ShipStreet3 property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipStreet3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStreet3 { get; }
        /// <summary> The ShipCity property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipCity"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipCity { get; }
        /// <summary> The ShipStateProvCode property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipStateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipStateProvCode { get; }
        /// <summary> The ShipPostalCode property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipPostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipPostalCode { get; }
        /// <summary> The ShipCountryCode property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipCountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipCountryCode { get; }
        /// <summary> The ShipPhone property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipPhone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipPhone { get; }
        /// <summary> The ShipFax property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipFax"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipFax { get; }
        /// <summary> The ShipEmail property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipEmail { get; }
        /// <summary> The ShipWebsite property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."ShipWebsite"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipWebsite { get; }
        /// <summary> The RollupOrderCount property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."RollupOrderCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RollupOrderCount { get; }
        /// <summary> The RollupOrderTotal property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."RollupOrderTotal"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal RollupOrderTotal { get; }
        /// <summary> The RollupNoteCount property of the Entity Customer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Customer"."RollupNoteCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 RollupNoteCount { get; }
        
        
        
        public IEnumerable<IOrderEntity> Order { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ICustomerEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ICustomerEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomCustomerData(ICustomerEntity source);
    }
}
