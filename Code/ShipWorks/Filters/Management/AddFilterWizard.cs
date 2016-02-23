using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using ShipWorks.Filters.Controls;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.ApplicationCore.Appearance;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Filters.Grid;
using ShipWorks.Editions;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Wizard for creating a new filter or folder
    /// </summary>
    public partial class AddFilterWizard : WizardForm
    {
        // Filter being created
        FilterEntity filter;

        // Nodes created when filter is successfully creatd
        List<FilterNodeEntity> createdNodes;

        // Fake FilterNode used until we create the real one
        FilterNodeEntity fakeFilterNode;

        // The folder selected to start off with
        FolderExpansionState initialState;
        FilterNodeEntity browserInitialParent;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddFilterWizard(bool isFolder, FolderExpansionState initialState, FilterNodeEntity browserInitialParent)
        {
            InitializeComponent();

            if (browserInitialParent == null)
            {
                browserInitialParent = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Orders).FilterNode;
            }

            this.initialState = initialState;
            this.browserInitialParent = browserInitialParent;

            filter = new FilterEntity();
            filter.Name = "";
            filter.IsFolder = isFolder;
            filter.FilterTarget = browserInitialParent.Filter.FilterTarget;

            if (isFolder)
            {
                // Update all text for filter\folder
                Control[] filterFolderTextControls = new Control[]
                    {
                        this,
                        labelLocation,
                        labelName
                    };

                foreach (Control control in filterFolderTextControls)
                {
                    control.Text = control.Text.Replace("ilter", "older");
                }

                foreach (WizardPage page in Pages)
                {
                    page.Title = page.Title.Replace("ilter", "older");
                    page.Description = page.Description.Replace("ilter", "older");
                }

                pictureBox.Image = Resources.folder_add;
            }

            // Limit to my filters if permissions not enabled
            if (!UserSession.Security.HasPermission(PermissionType.ManageFilters))
            {
                filterTree.FilterScope = FilterScope.MyFilters;
            }
        }

        /// <summary>
        /// The filter being created
        /// </summary>
        public FilterEntity Filter
        {
            get
            {
                return filter;
            }
        }

        /// <summary>
        /// The filter node selected to be the parent node.
        /// </summary>
        public FilterNodeEntity ParentFilterNode
        {
            get
            {
                return filterTree.SelectedFilterNode;
            }
        }

        /// <summary>
        /// If the result is OK, this is the list of nodes that were created
        /// </summary>
        public List<FilterNodeEntity> CreatedNodes
        {
            get
            {
                return createdNodes;
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // Load the tree
            filterTree.AlwaysShowMyFilters = true;
            filterTree.LoadLayouts((FilterTarget) browserInitialParent.Filter.FilterTarget);
            filterTree.ApplyFolderState(initialState);

            // Select the initial
            filterTree.SelectedFilterNode = browserInitialParent;

            // Load the editor
            conditionControl.LoadFilter(filter);
        }

        /// <summary>
        /// Stepping next from the name and location page
        /// </summary>
        private void OnStepNextNameAndLocation(object sender, WizardStepEventArgs e)
        {
            string filterName = name.Text.Trim();
            if (filterName.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please enter a name.");
                e.NextPage = CurrentPage;
                return;
            }

            if (filterTree.SelectedFilterNode == null)
            {
                MessageHelper.ShowMessage(this, "Please select a folder.");
                e.NextPage = CurrentPage;
                return;
            }

            // Edition restriction check on My Filters
            if (FilterHelper.IsMyFilter(filterTree.SelectedFilterNode))
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    ILicenseService licenseService = scope.Resolve<ILicenseService>();
                    if (!licenseService?
                        .GetLicenses()?
                        .FirstOrDefault()?
                        .HandleRestriction(EditionFeature.MyFilters, null, this) ?? false)
                    {
                        e.NextPage = CurrentPage;
                        return;
                    }
                }
            }

            filter.Name = filterName;

            if (fakeFilterNode == null || !ReferenceEquals(fakeFilterNode.ParentNode, filterTree.SelectedFilterNode))
            {
                CreateFakeFilterNode();

                // Go ahead and load the grid column editor
                FilterNodeColumnSettings gridLayout = new FilterNodeColumnSettings(
                    fakeFilterNode, 
                    null);

                gridColumns.LoadSettings(gridLayout);
            }
        }

        /// <summary>
        /// Stepping next from the condition page
        /// </summary>
        private void OnStepNextCondition(object sender, WizardStepEventArgs e)
        {
            if (!conditionControl.SaveDefinitionToFilter())
            {
                MessageHelper.ShowInformation(this, "Some of the values entered in the condition are not valid.");

                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Create the fake filter node object
        /// </summary>
        private void CreateFakeFilterNode()
        {
            CleanupFakeFilterNode();

            // We need a FilterNodeEntity to hold the Filter, since the editor uses a node.  We will end up throwing it away - 
            // its just a placeholder to be able to open the editor.
            FilterSequenceEntity sequence = new FilterSequenceEntity();
            sequence.Filter = filter;

            fakeFilterNode = new FilterNodeEntity();
            fakeFilterNode.FilterSequence = sequence;
            fakeFilterNode.Filter.State = (int)FilterState.Enabled;

            // Have to set this for GridLayout defaults to work
            fakeFilterNode.ParentNode = filterTree.SelectedFilterNode;
        }

        /// <summary>
        /// Cleanup the fake filter node
        /// </summary>
        private void CleanupFakeFilterNode()
        {
            if (fakeFilterNode != null)
            {
                fakeFilterNode.ParentNode = null;
                fakeFilterNode.FilterSequence.Filter = null;
                fakeFilterNode.FilterSequence = null;
                fakeFilterNode = null;                
            }
        }

        /// <summary>
        /// Wizard has closed
        /// </summary>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            CleanupFakeFilterNode();
        }

        /// <summary>
        /// Finishing
        /// </summary>
        private void OnStepNextGridColumns(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                CleanupFakeFilterNode();

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Add the filter
                    createdNodes = FilterLayoutContext.Current.AddFilter(filter, filterTree.SelectedFilterNode, 0, adapter);

                    // Get the grid layout
                    FilterNodeColumnSettings columnSettings = gridColumns.Settings;

                    // We have to save the default layout for each of the created nodes
                    foreach (FilterNodeEntity node in createdNodes)
                    {
                        columnSettings.SaveAs(node, adapter);
                    }

                    adapter.Commit();
                }
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                DialogResult = DialogResult.Abort;
            }
        }
    }
}
