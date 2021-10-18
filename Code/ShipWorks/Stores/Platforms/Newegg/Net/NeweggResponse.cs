using System;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response;


namespace ShipWorks.Stores.Platforms.Newegg.Net
{
    /// <summary>
    /// Class containing the general response of a request submitted to Newegg including the actual
    /// result that was returned by Newegg and any errors that may have occurred.
    /// </summary>
    public class NeweggResponse
    {
        private INeweggSerializer serializer;
        private string rawResponseData;
        private List<Error> responseErrors;
        private object result;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggResponse"/> class.
        /// </summary>
        /// <param name="responseData">The response data.</param>
        /// <param name="serializer">The serializer.</param>
        public NeweggResponse(string responseData, INeweggSerializer serializer)
        {
            this.rawResponseData = responseData;
            this.serializer = serializer;
            this.responseErrors = new List<Error>();

            // We will attempt to extract an object from the response data using
            // the serializer provided now rather than waiting for the Result property
            // to be called since attempting to get a handle on the object will also
            // load/initialize any response errors. In doing so, the client does not
            // have to "know" to attempt to get the result prior to checking for any
            // response errors.
            this.result = GetResult();
        }


        /// <summary>
        /// Gets the result.
        /// </summary>
        public object Result
        {
            get { return this.result; }
        }

        /// <summary>
        /// Gets the response errors.
        /// </summary>
        public IEnumerable<Error> ResponseErrors
        {
            get { return this.responseErrors; }
        }


        /// <summary>
        /// Attempts to extract an object from the raw response data using the 
        /// serializer that was provided in the constructor.
        /// </summary>
        /// <returns>An object representation of the response data.</returns>
        private object GetResult()
        {
            object result = null;

            try
            {
                result = serializer.Deserialize(this.rawResponseData);
            }
            catch (InvalidOperationException)
            {
                // An invalid operation exception is the result of attempting to deserialize
                // the raw response data into an object that does not match the response. This 
                // usually indicates an error response was returned from the Newegg API, so
                // we'll try to deserialize the response into an ErrorResult. 
                Errors.ErrorResponseSerializer errorSerializer = new Errors.ErrorResponseSerializer();
                ErrorResult errorResult = (ErrorResult) errorSerializer.Deserialize(this.rawResponseData);

                // Now just add the errors to our response errors
                this.responseErrors.AddRange(errorResult.Errors);
            }

            return result;            
        }
    }
}
