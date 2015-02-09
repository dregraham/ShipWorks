using System;
using System.Diagnostics;
using System.Reflection;
using System.Web.Services.Protocols;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29;

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

        /// <summary>
        /// Checks the necessary code is information place.
        /// This method will check that code for shipworks is in place. For instance, RewriteScanFromMessage is a required
        /// attribute on ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.CreateScanForm but is generated code
        /// and might get lost, hence the check.
        /// </summary>
        [Conditional("DEBUG")]
        public static void CheckNecessaryCodeIsInPlace()
        {
            Type stampsWebServiceType = typeof(SwsimV29);
            MethodInfo createScanFormMethod = stampsWebServiceType.GetMethod("CreateScanForm");

            Type attributeType = typeof(RewriteScanFormMessageAttribute);

            RewriteScanFormMessageAttribute[] rewriteScanFormMessageAttributes = (RewriteScanFormMessageAttribute[])createScanFormMethod.GetCustomAttributes(attributeType, false);

            if (rewriteScanFormMessageAttributes.Length == 0)
            {
                throw new Exception(@"CreateScanForm in \ShipWorks\Web References\Shipping.Carriers.Postal.Stamps.WebServices\Reference.cs needs the attribute RewriteScanFormMessageAttribute.");
            }
        }
    }
}