using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters;
using System.Collections;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using System.Threading.Tasks;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Interface that is implemented by controls that are within docking panels on the main form
    /// </summary>
    public interface IDockingPanelContent
    {
        /// <summary>
        /// Load the state of the panel.
        /// </summary>
        void LoadState();

        /// <summary>
        /// Save the state of the panel.
        /// </summary>
        void SaveState();

        /// <summary>
        /// The EntityType displayed by the panel grid
        /// </summary>
        EntityType EntityType { get; }

        /// <summary>
        /// The supported filter targets that the panel can display for.
        /// </summary>
        FilterTarget[] SupportedTargets { get; }

        /// <summary>
        /// Indicates if the panel can handle multiple selected items at one time.
        /// </summary>
        bool SupportsMultiSelect { get; }

        /// <summary>
        /// Change the content of the panel based on the given keys.
        /// </summary>
        Task ChangeContent(IGridSelection selection);

        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row 
        /// list with up-to-date displayed entity content.
        /// </summary>
        Task ReloadContent();

        /// <summary>
        /// Refresh the existing displayed content.  Does not try to reset or look for new\deleted rows - just refreshes
        /// the known existing rows and their known corresponding entities.
        /// </summary>
        Task UpdateContent();

        /// <summary>
        /// Update the content to reflect changes to the loaded stores
        /// </summary>
        void UpdateStoreDependentUI();
    }
}
