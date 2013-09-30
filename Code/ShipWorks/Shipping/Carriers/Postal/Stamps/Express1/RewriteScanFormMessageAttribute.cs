using System;
using System.Web.Services.Protocols;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Attribute that marks a method as needing to have its scanform data replaced
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RewriteScanFormMessageAttribute : SoapExtensionAttribute
    {
        /// <summary>
        /// Gets and sets the priority of the extension
        /// </summary>
        public override int Priority { get; set; }

        /// <summary>
        /// Gets the type of extension to which this attribute applies
        /// </summary>
        public override Type ExtensionType
        {
            get { return typeof(RewriteScanFormMessageSoapExtension); }
        }
    }
}