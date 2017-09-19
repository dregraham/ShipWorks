using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.NetworkSolutions.WebServices;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Manages available status codes for NetworkSolutions store
    /// </summary>
    [Component(RegistrationType.Self)]
    public class NetworkSolutionsStatusCodeProvider : OnlineStatusCodeProvider<long>
    {
        /// <summary>
        /// Maps a status code to it's possible "next steps" as defined by the store's configuration
        /// </summary>
        Dictionary<long, List<long>> statusWorkflow = new Dictionary<long, List<long>>();

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsStatusCodeProvider(NetworkSolutionsStoreEntity store)
            : base(store, NetworkSolutionsStoreFields.StatusCodes)
        {

        }

        /// <summary>
        /// Retrieves the status codes from NetworkSolutions
        /// </summary>
        protected override Dictionary<long, string> GetCodeMapFromOnline()
        {
            // get the status codes
            NetworkSolutionsWebClient client = new NetworkSolutionsWebClient();
            List<OrderStatusType> statuses = client.GetStatusCodes((NetworkSolutionsStoreEntity) Store);

            // status id -> name mapping
            Dictionary<long, string> statusCodeMap = new Dictionary<long, string>();

            foreach (OrderStatusType status in statuses)
            {
                // populate the name mapping
                statusCodeMap[status.OrderStatusId] = status.Name;

                // populate the workflow mapping
                List<long> nextSteps = new List<long>();

                if (status.NextStatusList != null)
                {
                    foreach (OrderStatusType nextStatus in status.NextStatusList)
                    {
                        nextSteps.Add(nextStatus.OrderStatusId);
                    }
                }

                statusWorkflow[status.OrderStatusId] = nextSteps;
            }

            return statusCodeMap;
        }

        /// <summary>
        /// Read the serialized workflow information back from the xml stored in the database
        /// </summary>
        protected override void ReadExtendedStatusCodeXml(long code, XPathNavigator xpath)
        {
            string nextStatuses = XPathUtility.Evaluate(xpath, "NextStatuses", "");
            if (nextStatuses.Length > 0)
            {
                List<long> nextStatusList = new List<long>();

                // turn the CSV into a list of Ids
                string[] nextIds = nextStatuses.Split(',');
                foreach (string nextId in nextIds)
                {
                    try
                    {
                        nextStatusList.Add(Convert.ToInt64(nextId));
                    }
                    catch (FormatException)
                    {
                        // bad value somehow got in, ignore it
                    }
                }

                statusWorkflow[code] = nextStatusList;
            }
        }

        /// <summary>
        /// Serialize the worklfow information to the xml stored in the database
        /// </summary>
        protected override void WriteExtendedStatusCodeXml(KeyValuePair<long, string> pair, XmlTextWriter xmlWriter)
        {
            // if extra data exists to save
            if (statusWorkflow.ContainsKey(pair.Key))
            {
                List<long> nextStatusIds = statusWorkflow[pair.Key];

                // make sure a valid list was mapped
                if (nextStatusIds != null)
                {
                    List<string> stringIds = nextStatusIds.ConvertAll(i => i.ToString());
                    string workflowString = String.Join(",", stringIds.ToArray());

                    // write the workflow string to the xml
                    xmlWriter.WriteElementString("NextStatuses", workflowString);
                }
            }
        }

        /// <summary>
        /// Returns the collection of Network Solusions "Next Status Codes" in their allowed status flow
        /// </summary>
        public List<long> GetNextOrderStatusCodes(long orderStatusCode)
        {
            if (statusWorkflow.ContainsKey(orderStatusCode))
            {
                return statusWorkflow[orderStatusCode];
            }
            else
            {
                return new List<long>();
            }
        }
    }
}
