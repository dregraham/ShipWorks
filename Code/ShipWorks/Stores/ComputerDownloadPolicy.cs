using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Store policy for what computers can be downloaded on for a given store
    /// </summary>
    public class ComputerDownloadPolicy
    {
        // Indiciates if the default is to have downloading on or off
        bool defaultToYes;

        // List of computers that have been explicitly set, and are not just picking up the default
        Dictionary<long, bool> explicitlySet;

        /// <summary>
        /// Constructor
        /// </summary>
        public static ComputerDownloadPolicy Load(StoreEntity store)
        {
            ComputerDownloadPolicy policy = new ComputerDownloadPolicy();

            if (string.IsNullOrWhiteSpace(store.ComputerDownloadPolicy))
            {
                policy.defaultToYes = true;
                policy.explicitlySet = new Dictionary<long, bool>();
            }
            else
            {
                policy.LoadFromXml(store.ComputerDownloadPolicy);
            }

            return policy;
        }

        /// <summary>
        /// Private constructor, use static Load method
        /// </summary>
        private ComputerDownloadPolicy()
        {

        }

        /// <summary>
        /// The default of if a store has not configured yet, if it will be enabled or disabled for downloading
        /// </summary>
        public bool DefaultToYes
        {
            get { return defaultToYes; }
            set { defaultToYes = value; }
        }

        /// <summary>
        /// Indicates if this computer is turned on for downloading
        /// </summary>
        public bool IsThisComputerAllowed
        {
            get
            {
                ComputerDownloadAllowed allowed = GetComputerAllowed(UserSession.Computer.ComputerID);

                switch (allowed)
                {
                    case ComputerDownloadAllowed.Yes: return true;
                    case ComputerDownloadAllowed.No: return false;
                    default:
                        return defaultToYes;
                }
            }
        }

        /// <summary>
        /// Get the status of the given computer.  If it's not configured, null is returned.
        /// </summary>
        public ComputerDownloadAllowed GetComputerAllowed(long computerID)
        {
            bool setting;
            if (explicitlySet.TryGetValue(computerID, out setting))
            {
                return setting ? ComputerDownloadAllowed.Yes : ComputerDownloadAllowed.No;
            }

            return ComputerDownloadAllowed.Default;
        }

        /// <summary>
        /// Set the status of the given computer
        /// </summary>
        public void SetComputerAllowed(long computerID, ComputerDownloadAllowed allowed)
        {
            if (allowed == ComputerDownloadAllowed.Default)
            {
                explicitlySet.Remove(computerID);
            }
            else
            {
                explicitlySet[computerID] = (allowed == ComputerDownloadAllowed.Yes);
            }
        }

        /// <summary>
        /// Serialize the policy to an XML string that can be restored later
        /// </summary>
        public string SerializeToXml()
        {
            XElement policy = new XElement("ComputerDownloadPolicy",
                new XElement("DefaultToYes", defaultToYes),
                new XElement("Computers",
                    explicitlySet.Select(kvp =>
                        new XElement("Computer",
                            new XAttribute("ID", kvp.Key),
                            kvp.Value))));

            return policy.ToString();
        }

        /// <summary>
        /// Load the policy from the given XML
        /// </summary>
        private void LoadFromXml(string policyXml)
        {
            XElement root = XElement.Parse(policyXml);

            if (root.Name != "ComputerDownloadPolicy")
            {
                throw new InvalidOperationException("The policy is not valid computer policy XML.");
            }

            defaultToYes = (bool) root.Element("DefaultToYes");
            explicitlySet = new Dictionary<long, bool>();

            foreach (XElement xComputer in root.XPathSelectElements("Computers/Computer"))
            {
                long computerID = (long) xComputer.Attribute("ID");
                bool setting = (bool) xComputer;

                explicitlySet[computerID] = setting;
            }
        }
    }
}
