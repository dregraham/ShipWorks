using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Class used for holding shipment rating fields; shipment and package fields and helper utilities.
    /// </summary>
    public class RatingFields
    {
        private readonly List<EntityField2> shipmentFields;
        private readonly List<EntityField2> packageFields;

        /// <summary>
        /// Constructor
        /// </summary>
        public RatingFields()
        {
            shipmentFields = new List<EntityField2>();
            packageFields = new List<EntityField2>();
        }

        /// <summary>
        /// List of fields based on Shipment entities.  You can include ShipmentFields and any derived shipment fields like FedExFields.
        /// </summary>
        public List<EntityField2> ShipmentFields
        {
            get { return shipmentFields; }
        }

        /// <summary>
        /// List of fields based on package entities like FedExPackageFields.
        /// </summary>
        public List<EntityField2> PackageFields
        {
            get { return packageFields; }
        }

        /// <summary>
        /// The union of shipment and package fields.
        /// </summary>
        private List<EntityField2> AllFields
        {
            get
            {
                return shipmentFields.Union(packageFields).ToList();
            }
        }

        /// <summary>
        /// The names of AllFields
        /// </summary>
        private List<string> AllFieldNames
        {
            get
            {
                return AllFields.Select(f => f.Name).ToList();
            }
        }

        /// <summary>
        /// Returns true if specified field name is in the shipment fields or package fields.
        /// </summary>
        public bool FieldsContainName(string fieldName)
        {
            return AllFieldNames.Any(f => f.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the rating hash based on the shipment's configuration.  Package fields will NOT be used.  If package fields are needed, use the overloaded constructor.
        /// </summary>
        public virtual string GetRatingHash<TShipmentEntity>(TShipmentEntity shipment) where TShipmentEntity : EntityBase2
        {
            return GetRatingHash(shipment, null as EntityCollection<EntityBase2>);
        }

        /// <summary>
        /// Gets the rating hash based on the shipment's configuration, including package fields.
        /// </summary>
        public virtual string GetRatingHash<TShipmentEntity, TPackageEntity>(TShipmentEntity shipment, EntityCollection<TPackageEntity> packages)
            where TShipmentEntity : EntityBase2
            where TPackageEntity : EntityBase2
        {
            StringBuilder valueToBeHashed = new StringBuilder();
            
            // Get the field values for shipment fields.
            foreach (EntityField2 field in ShipmentFields)
            {
                object currentValue = EntityUtility.GetFieldValue(shipment, field, true);
                valueToBeHashed.Append(currentValue ?? string.Empty);
            }

            if (packages != null)
            {
                // Get the field values for each package's fields.
                foreach (TPackageEntity package in packages)
                {
                    foreach (EntityField2 field in PackageFields)
                    {
                        object currentValue = EntityUtility.GetFieldValue(package, field, true);
                        valueToBeHashed.Append(currentValue ?? string.Empty);
                    }
                }
            }

            // Hash the value 
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(valueToBeHashed.ToString()));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
