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
    /// Read-only representation of the entity 'ProductListItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProductListItemEntity : IProductListItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProductListItemEntity(IProductListItemEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ProductVariantID = source.ProductVariantID;
            SKU = source.SKU;
            Name = source.Name;
            Length = source.Length;
            Width = source.Width;
            Height = source.Height;
            Weight = source.Weight;
            BinLocation = source.BinLocation;
            ImageUrl = source.ImageUrl;
            IsActive = source.IsActive;
            
            
            

            CopyCustomProductListItemData(source);
        }

        
        /// <summary> The ProductVariantID property of the Entity ProductListItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProductListView"."ProductVariantID"<br/>
        /// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ProductVariantID { get; }
        /// <summary> The SKU property of the Entity ProductListItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProductListView"."SKU"<br/>
        /// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SKU { get; }
        /// <summary> The Name property of the Entity ProductListItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProductListView"."Name"<br/>
        /// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Name { get; }
        /// <summary> The Length property of the Entity ProductListItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProductListView"."Length"<br/>
        /// View field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> Length { get; }
        /// <summary> The Width property of the Entity ProductListItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProductListView"."Width"<br/>
        /// View field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> Width { get; }
        /// <summary> The Height property of the Entity ProductListItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProductListView"."Height"<br/>
        /// View field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> Height { get; }
        /// <summary> The Weight property of the Entity ProductListItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProductListView"."Weight"<br/>
        /// View field type characteristics (type, precision, scale, length): Decimal, 29, 9, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Decimal> Weight { get; }
        /// <summary> The BinLocation property of the Entity ProductListItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProductListView"."BinLocation"<br/>
        /// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String BinLocation { get; }
        /// <summary> The ImageUrl property of the Entity ProductListItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProductListView"."ImageUrl"<br/>
        /// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ImageUrl { get; }
        /// <summary> The IsActive property of the Entity ProductListItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on view field: "ProductListView"."IsActive"<br/>
        /// View field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsActive { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductListItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductListItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProductListItemData(IProductListItemEntity source);
    }
}
