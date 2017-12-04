using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Filters.Content.SqlGeneration
{
    /// <summary>
    /// Class for generating the SQL that will be used to execute a filter definition
    /// </summary>
    public interface ISqlGenerationContext
    {
        /// <summary>
        /// Add a column to the ColumnsUsed collection
        /// </summary>
        void AddColumnUsed(EntityField2 field);

        /// <summary>
        /// Register a parameter
        /// </summary>
        /// <param name="value">Value of the parameter</param>
        /// <returns>Name of the created parameter</returns>
        string RegisterParameter(object value);
    }
}