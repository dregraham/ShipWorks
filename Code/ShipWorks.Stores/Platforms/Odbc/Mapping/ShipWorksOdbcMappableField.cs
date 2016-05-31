using Newtonsoft.Json;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// The ShipWorks half of an OdbcFieldMapEntry
    /// </summary>
    public class ShipWorksOdbcMappableField : IOdbcMappableField
	{
	    private readonly EntityField2 field;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="displayName">The display name.</param>
        [JsonConstructor]
        public ShipWorksOdbcMappableField(EntityField2 field, string displayName) : this(field, displayName, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOdbcMappableField"/> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="isRequired"></param>
        public ShipWorksOdbcMappableField(EntityField2 field, string displayName, bool isRequired)
	    {
	        this.field = field;
            ContainingObjectName = field?.ContainingObjectName;
            Name = field?.Name;
	        DisplayName = displayName;
            IsRequired = isRequired;
	    }

        /// <summary>
        /// The name of the object that contains this field
        /// </summary>
        public string ContainingObjectName { get; set; }

        /// <summary>
        /// The name of the field
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The fields value
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// The fields display name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DisplayName { get; }

        /// <summary>
        /// Gets the qualified name for the field - table.column
        /// </summary>
        public string GetQualifiedName()
        {
            return $"{ContainingObjectName}.{Name}";
        }

        /// <summary>
        /// Is the field required to be mapped.
        /// </summary>
        [JsonIgnore]
        [Obfuscation(Exclude = true)]
        public bool IsRequired { get; set; }
    }
}
