using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Class used for holding shipment rating fields; shipment and package fields and helper utilities.
    /// </summary>
    public class RatingFields
    {
        private Dictionary<string, EntityField2> shipmentFields;

        /// <summary>
        /// Constructor
        /// </summary>
        public RatingFields()
        {
            shipmentFields = new Dictionary<string, EntityField2>();
            PackageFields = new List<EntityField2>();
        }

        /// <summary>
        /// List of fields based on package entities like FedExPackageFields.
        /// </summary>
        public List<EntityField2> PackageFields { get; }

        /// <summary>
        /// Returns true if specified field name is in the shipment fields or package fields.
        /// </summary>
        public bool FieldsContainName(string fieldName)
        {
            return shipmentFields.ContainsKey(fieldName) || PackageFields.Any(f => f.Name.Contains(fieldName));
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
            IEqualityComparer<EntityField2> fieldComparer =
                new GenericPropertyEqualityComparer<EntityField2, string>(x => x.Name);

            // Get the field values for shipment fields.
            foreach (EntityField2 field in shipmentFields.Values.Distinct(fieldComparer))
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

        /// <summary>
        /// Add a shipment field for rating
        /// </summary>
        public void AddShipmentField(EntityField2 field) =>
            AddShipmentField(field, field.Name);

        /// <summary>
        /// Add shipment field that does not correspond exactly to LLBLgen entity fields
        /// </summary>
        public void AddShipmentField(EntityField2 field, params string[] customFieldNames)
        {
            AddShipmentFieldToDictionary(field, field.Name);

            foreach (string fieldName in customFieldNames)
            {
                AddShipmentFieldToDictionary(field, fieldName);
            }
        }

        /// <summary>
        /// Add the field to the dictionary if it doesn't already exist
        /// </summary>
        private void AddShipmentFieldToDictionary(EntityField2 field, string fieldName)
        {
            if (!shipmentFields.ContainsKey(fieldName))
            {
                shipmentFields.Add(fieldName, field);
            }
        }
    }
}
