using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Filters.Content.SqlGeneration
{
    /// <summary>
    /// Class for generating the SQL that will be used to execute a filter definition
    /// </summary>
    public interface ISqlGenerationBuilder
    {
        /// <summary>
        /// Register a parameter
        /// </summary>
        /// <param name="field">Field for which the parameter will apply</param>
        /// <param name="value">Value of the parameter</param>
        /// <returns>Name of the created parameter</returns>
        string RegisterParameter(EntityField2 field, object value);
    }
}