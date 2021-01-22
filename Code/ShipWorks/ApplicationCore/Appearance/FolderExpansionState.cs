using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.ApplicationCore.Appearance
{
    /// <summary>
    /// Class for persisting the expand\collapse state of folders
    /// </summary>
    public class FolderExpansionState
    {
        Dictionary<long, bool> folderState = new Dictionary<long, bool>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public FolderExpansionState()
        {

        }

        /// <summary>
        /// Load a folder state instance from saved xml state
        /// </summary>
        public FolderExpansionState(string xmlState)
        {
            LoadState(xmlState);
        }

        /// <summary>
        /// Returns true if the sate
        /// </summary>
        public bool IsExpanded(EntityBase2 entity)
        {
            return IsExpanded(entity, true);
        }

        /// <summary>
        /// Gets the expanded state.  If no state exists, returns the specified default.
        /// </summary>
        public bool IsExpanded(EntityBase2 entity, bool defaultExpanded)
        {
            bool expanded;
            if (folderState.TryGetValue((long) entity.Fields.PrimaryKeyFields[0].CurrentValue, out expanded))
            {
                return expanded;
            }

            return defaultExpanded;
        }

        /// <summary>
        /// Set the expansion state of the given entity
        /// </summary>
        public void SetExpanded(EntityBase2 entity, bool expanded)
        {
            SetExpanded((long) entity.Fields.PrimaryKeyFields[0].CurrentValue, expanded);
        }

        /// <summary>
        /// Set the expansion state of the given entity id
        /// </summary>
        public void SetExpanded(long entityID, bool expanded)
        {
            folderState[entityID] = expanded;
        }

        /// <summary>
        /// Get the XML representation of the state.
        /// </summary>
        public string GetState()
        {
            StringBuilder state = new StringBuilder();

            state.Append("<FolderState>\n");

            // Add in each value
            foreach (KeyValuePair<long, bool> pair in folderState)
            {
                state.AppendFormat("   <Folder node='{0}' expanded='{1}' />\n", pair.Key, pair.Value);
            }

            state.Append("</FolderState>");

            return state.ToString();
        }

        /// <summary>
        /// Load the state from its previously saved xml.  If null, the stae is cleared.
        /// </summary>
        public void LoadState(string xml)
        {
            if (xml == null)
            {
                folderState.Clear();
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            XmlNodeList folderNodes = xmlDocument.SelectNodes("//Folder");

            foreach (XmlNode node in folderNodes)
            {
                folderState[Convert.ToInt64(node.Attributes["node"].Value)] = Convert.ToBoolean(node.Attributes["expanded"].Value);
            }
        }
    }
}
