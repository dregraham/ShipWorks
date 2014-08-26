namespace Interapptive.Shared.IO.Text.Csv
{
    /// <summary>
    /// Specifies the action to take when a duplicate field is encountered
    /// </summary>
    public enum DuplicateFieldAction
    {
        /// <summary>
        /// Ignore the field
        /// </summary>
        Ignore,

        /// <summary>
        /// Throw an argument exception
        /// </summary>
        Throw
    }
}