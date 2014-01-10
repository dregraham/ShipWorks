using System;

namespace ShipWorks.Data.Administration.Versioning
{
    [Serializable]
    public class FindVersionUpgradePathException:Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindVersionUpgradePathException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FindVersionUpgradePathException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindVersionUpgradePathException"/> class.
        /// </summary>
        public FindVersionUpgradePathException(string message, Exception ex):base(message, ex)
        {
        }
    }
}
