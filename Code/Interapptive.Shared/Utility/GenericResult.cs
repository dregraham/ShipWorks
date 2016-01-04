namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Generic Class that can be used to return an object or message
    /// </summary>
    /// <remarks>
    /// Used to return an object or message to display to the user
    /// keeps classes from having to access the message helper directly
    /// </remarks>
    public class GenericResult<T>
    {
        /// <summary>
        /// Constructor that contains the result object
        /// </summary>
        /// <remarks>
        /// Checks to see if the result object is not null and sets the Success to yes
        /// </remarks>
        public GenericResult(T context)
        {
            Context = context;
        }

        /// <summary>
        /// Constructor that takes every avaialble parameter
        /// </summary>
        /// <param name="context">The result object</param>
        /// <param name="message">Any message that might need to occompany the object</param>
        /// <param name="success">Bool if the result was a success</param>
        public GenericResult(T context, string message, bool success)
        {
            Context = context;
            Message = message;
            Success = success;
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
