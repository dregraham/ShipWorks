namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Represents a single variable to be posted
    /// </summary>
    public class HttpVariable
    {
        string name;
        string value;

        /// <summary>
        /// Default constructor
        /// </summary>
        public HttpVariable()
        {
        }

        /// <summary>
        /// Initializer
        /// </summary>
        public HttpVariable(string name, string value, bool urlEncode)
        {
            this.name = name;
            this.value = value;
            UrlEncode = urlEncode;
        }

        /// <summary>
        /// Initializer
        /// </summary>
        /// <remarks>
        /// Default to URL Encoding
        /// </remarks>
        public HttpVariable(string name, string value) : this(name, value, true)
        {

        }

        /// <summary>
        /// Should this variable be UrlEncoded
        /// </summary>
        public bool UrlEncode { get; set; }

        /// <summary>
        /// The name of the variable to post
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The value of the variable to post
        /// </summary>
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// ToString, for debugging
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0}={1}", name, value);
        }
    }
}
