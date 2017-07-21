using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Implemented by OrderEntities that could be Amazon Orders (Orders from Amazon or CA maybe others in the future)
    /// </summary>
    public partial class ChannelAdvisorStoreEntity : IAmazonCredentials
    {
        /// <summary>
        /// Amazon auth token
        /// </summary>
        string IAmazonCredentials.AuthToken
        {
            get { return AmazonAuthToken; }
        }

        /// <summary>
        /// Amazon merchant ID
        /// </summary>
        string IAmazonCredentials.MerchantID
        {
            get { return AmazonMerchantID; }
        }

        /// <summary>
        /// Amazon store region
        /// </summary>
        string IAmazonCredentials.Region
        {
            get { return AmazonApiRegion; }
        }

        /// <summary>
        /// Returns a list of attributes user has selected to download
        /// </summary>
        public IEnumerable<string> ParsedAttributesToDownload
        {
            get
            {
                XDocument attributesToDownload = XDocument.Parse(AttributesToDownload);
                return attributesToDownload.Descendants("Attribute").Select(a => a.Value);
            }
        }
    }
}
