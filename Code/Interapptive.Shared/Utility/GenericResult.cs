namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Generic Class that can be used to return an object or message
    /// </summary>
    /// <remarks>
    /// Used to return an object or message
    /// </remarks>
    public class GenericResult<T>
    {
        /// <summary>
        /// Constructor that contains the result object
        /// </summary>
        public GenericResult(T context)
        {
            Context = context;
        }

        /// <summary>
        /// The object being returned
        /// </summary>
        public T Context { get; set; }

        /// <summary>
        /// Message accompanying the object
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Whether or not the operation was a success
        /// </summary>
        public bool Success { get; set; }
    }
}
