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
    /// Entity interface which represents the entity 'Product'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProductEntity
    {
        
        /// <summary> The ProductID property of the Entity Product<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Product"."ProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ProductID { get; }
        /// <summary> The CreatedDate property of the Entity Product<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Product"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime CreatedDate { get; }
        /// <summary> The Name property of the Entity Product<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Product"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        /// <summary> The IsActive property of the Entity Product<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Product"."IsActive"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsActive { get; }
        /// <summary> The IsBundle property of the Entity Product<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Product"."IsBundle"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsBundle { get; }
        
        
        
        IEnumerable<IProductBundleEntity> Bundles { get; }
        IEnumerable<IProductVariantEntity> Variants { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Product'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProductEntity : IProductEntity
    {
        
        
        IEnumerable<IProductBundleEntity> IProductEntity.Bundles => Bundles;
        IEnumerable<IProductVariantEntity> IProductEntity.Variants => Variants;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IProductEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProductEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProductEntity(this, objectMap);
        }

        
    }
}
