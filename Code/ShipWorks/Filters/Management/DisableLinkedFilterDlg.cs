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
            return new[] 
                {
                    BuildListReferenceMessage(references, "The trigger for action '(?<name>.*)'", "The following actions will no longer run if you continue:"),
                    BuildListReferenceMessage(references, "'.*' task for action '(?<name>.*)'", "The following actions include tasks that will not run as expected:"),
                    BuildShippingProviderRuleMessage(references),
                    BuildListReferenceMessage(references, "Print settings '.*' for '(?<name>.*)'", "These providers include printing rules that will no longer apply if you continue:"),
                }
                .Where(x => !string.IsNullOrEmpty(x))
                .Aggregate((x, y) => x + Environment.NewLine + Environment.NewLine + y);
        }

        /// <summary>
        /// Build the message for shipping provider rules
        /// </summary>
        private static string BuildShippingProviderRuleMessage(IEnumerable<string> references)
        {
            return references.Any(x => string.Equals(x, "Default shipping provider rules", StringComparison.Ordinal)) ?
                "There are shipping rules that will no longer apply if request is completed." :
                string.Empty;
        }

        /// <summary>
        /// Build a message that contains a list of references
        /// </summary>
        private static string BuildListReferenceMessage(IEnumerable<string> references, string messageMatcher, string sectionTitle)
        {
            Regex triggerRegex = new Regex(messageMatcher);
            List<string> triggerReferences = references.Select(x => triggerRegex.Match(x))
                .Where(x => x.Success)
                .Select(x => "  - " + x.Groups["name"].Value)
                .ToList();

            if (triggerReferences.Any())
            {
                return sectionTitle +
                    Environment.NewLine +
                    triggerReferences.Aggregate((x, y) => x + Environment.NewLine + y);
            }

            return string.Empty;
        }
    }
}