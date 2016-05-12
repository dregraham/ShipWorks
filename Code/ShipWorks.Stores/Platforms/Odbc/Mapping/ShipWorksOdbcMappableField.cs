using System.Reflection;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

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
        public ShipWorksOdbcMappableField(EntityField2 field, string displayName)
	    {
	        this.field = field;
	        DisplayName = displayName;
	    }

        /// <summary>
        /// Gets the qualified name for the field - table.column
        /// </summary>
        public string GetQualifiedName()
	    {
            string table = field.ContainingObjectName.Replace("Entity", string.Empty);

            return $"{table}.{field.Name}";
	    }

        /// <summary>
        /// The fields value
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// The fields display name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DisplayName { get; }
	}
}
