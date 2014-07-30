using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.LimeLightCRM
{
    /// <summary>
    /// Web client for Lime Light CRM
    /// </summary>
    public class LimeLightCRMWebClient : GenericStoreWebClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LimeLightCRMWebClient(GenericModuleStoreEntity store) : 
            base(store)
        {

        }

        /// <summary>
        /// Catch authentication errors, since they are not returned as XML
        /// </summary>
        protected override string TransformResponse(string resultXml, string action)
        {
            // Try to parse the XML before the real parse to handle the authentication error.
            // Lime Light CRM does not give us auth errors as valid XML
            try
            {
                XmlDocument xmlResponse = new XmlDocument
                {
                    XmlResolver = null
                };

                xmlResponse.LoadXml(resultXml);
            }
            catch (XmlException)
            {
                if (resultXml.Contains("not authenticated"))
                {
                    throw new GenericStoreException("Invalid username or password.");
                }
            }

            return base.TransformResponse(resultXml, action);
        }
    }
}
