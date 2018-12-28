using System;
using ShipWorks.Data.Grid;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Interface that represents the MainGridControl
    /// </summary>
    public interface IMainGridControl
    {
        /// <summary>
        /// Action for adding Search box Text change event
        /// </summary>
        Action<EventHandler> SearchTextChangedAdd { get; }

        /// <summary>
        /// Action for removing Search box Text change event
        /// </summary>
        Action<EventHandler> SearchTextChangedRemove { get; }

        /// <summary>
        /// Action for adding filter editor definition edited event
        /// </summary>
        Action<EventHandler> FilterEditorDefinitionEditedAdd { get; }

        /// <summary>
        /// Action for removing filter editor definition edited event
        /// </summary>
        Action<EventHandler> FilterEditorDefinitionEditedRemove { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control and all its child controls
        /// are displayed.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets a value indicating whether the control can receive focus.
        /// </summary>
        bool CanFocus { get; }

        /// <summary>
        /// The current grid selection
        /// </summary>
        IGridSelection Selection { get; }

        /// <summary>
        /// Get the normalized text of the basic search box.
        /// </summary>
        string GetBasicSearchText();

        /// <summary>
        /// Performs the search.
        /// </summary>
        void PerformManualSearch();

        /// <summary>
        /// Perform a barcode search
        /// </summary>
        void PerformBarcodeSearch(string barcode);

        /// <summary>
        /// Executes the specified delegate asynchronously with the specified arguments,
        ///  on the thread that the control's underlying handle was created on.
        /// </summary>
        IAsyncResult BeginInvoke(Delegate method, params object[] args);

        /// <summary>
        /// Focus the searchbox
        /// </summary>
        void FocusSearch();

        /// <summary>
        /// Clear the searchbox text
        /// </summary>
        void ClearSearch();
    }
}
