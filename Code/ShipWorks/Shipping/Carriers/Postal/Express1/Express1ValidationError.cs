
namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// A class to encapsulate any Express1 specific validation errors that may be triggered when 
    /// validating information collected from a user.
    /// </summary>
    public class Express1ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Express1ValidationError"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public Express1ValidationError(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}
