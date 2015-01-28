using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Confirmation of disabling filter. Lists anything the filter migt be associated with.
    /// </summary>
    public partial class DisableLinkedFilterDlg : Form
    {
        private readonly FilterNodeEntity filterNode;

        /// <summary>
        /// Constructor
        /// </summary>
        public DisableLinkedFilterDlg(FilterNodeEntity filterNode)
        {
            InitializeComponent();

            if (filterNode == null)
            {
                throw new ArgumentNullException("filterNode");
            }

            if (filterNode.Filter.IsFolder)
            {
                throw new ArgumentException(@"It is a folder.", "filterNode");
            }

            this.filterNode = filterNode;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            List<string> linkReasons = new FilterNodeReferenceRepository().Find(filterNode);

            if (!linkReasons.Any())
            {
                usages.Visible = false;
                Height -= usages.Height;
            }
            else
            {
                usages.Text = BuildReferenceMessage(linkReasons);
            }
        }

        /// <summary>
        /// Disable Selected
        /// </summary>
        private void OnDisableSelected(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Build a more user friendly message for the filter references
        /// </summary>
        private static string BuildReferenceMessage(List<string> references)
        {
            List<string> messages = new List<string>
            {
                BuildListReferenceMessage(references, "The trigger for action '(?<name>.*)'", "The following actions will no longer run if you continue:"),
                BuildListReferenceMessage(references, "'.*' task for action '(?<name>.*)'", "The following actions include tasks that will not run as expected:"),                
                BuildListReferenceMessage(references, "Print settings '.*' for '(?<name>.*)'", "These providers include printing rules that will no longer apply if you continue:"),                
                BuildShippingRuleMessage(references)
            };

            // If none of the reference messages match our friendly messages, just use the originals
            List<string> filteredMessages = messages.Where(x => !string.IsNullOrEmpty(x)).ToList();

            return (filteredMessages.Any() ? filteredMessages : references)
                           .Aggregate((x, y) => x + Environment.NewLine + Environment.NewLine + y);
        }

        /// <summary>
        /// Build the message for shipping rules
        /// </summary>
        private static string BuildShippingRuleMessage(IEnumerable<string> references)
        {
            List<string> messageLines = new List<string>();

            if (references.ToList().Any(x => string.Equals(x, "Default shipping provider rules", StringComparison.Ordinal)))
            {
                messageLines.Add("Shipping rules for the following carriers will not apply:");
                messageLines.Add("  - General");
            }

            if (!messageLines.Any())
            {
                // There weren't any general shipping rules that were impacted, so we need to 
                // write out the full message
                messageLines.Add(BuildListReferenceMessage(references, "Shipment defaults for '(?<name>.*)'", "Shipping rules for the following carriers will not apply:"));
            }
            else
            {
                // We've already written the header for general shipping rules, so we just need to add
                // the other carriers.
                messageLines.AddRange(GetReferenceNames(references, "Shipment defaults for '(?<name>.*)'"));

            }

            return messageLines.Aggregate((x, y) => x + Environment.NewLine + y);
        }

        /// <summary>
        /// Build a message that contains a list of references
        /// </summary>
        private static string BuildListReferenceMessage(IEnumerable<string> references, string messageMatcher, string sectionTitle)
        {
            List<string> triggerReferences = GetReferenceNames(references, messageMatcher);

            if (triggerReferences.Any())
            {
                return sectionTitle +
                    Environment.NewLine +
                    triggerReferences.Aggregate((x, y) => x + Environment.NewLine + y);
            }

            return string.Empty;
        }

        /// <summary>
        /// A helper method to extract named references from a collection strings that is used when building 
        /// up the list of topics that reference a filter.
        /// </summary>
        private static List<string> GetReferenceNames(IEnumerable<string> references, string messageMatcher)
        {
            Regex triggerRegex = new Regex(messageMatcher);
            List<string> triggerReferences = references.Select(x => triggerRegex.Match(x))
                                                       .Where(x => x.Success)
                                                       .Select(x => "  - " + x.Groups["name"].Value)
                                                       .ToList();
            return triggerReferences;
        }
    }
}