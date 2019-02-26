using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// View model for the main grid header
    /// </summary>
    public class MainGridHeaderViewModel : ViewModelBase
    {
        private bool isAdvancedSearchOpen;
        private string title;
        private string searchText;
        private string watermarkText;
        private string filterName;
        private Image headerImage;
        private Image endSearchImage;
        private bool isSearching;
        private bool isSearchActive;

        public event EventHandler SearchEndClicked;
        public event EventHandler FilterSaveClicked;
        public event EventHandler QuickSearchFocusCleared;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainGridHeaderViewModel()
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                Title = "All";
                WatermarkText = "Search all orders...";
                HeaderImage = ShipWorks.Properties.Resources.view;
                isAdvancedSearchOpen = true;
            }

            EndSearchImage = ShipWorks.Properties.Resources.buttonEndSearchDisabled;
            EndSearch = new RelayCommand(EndSearchAction);
            ToggleAdvancedSearch = new RelayCommand(ToggleAdvancedSearchAction);
            SaveFilter = new RelayCommand(SaveFilterAction);
            QuickSearchClearFocus = new RelayCommand(QuickSearchClearFocusAction);
        }

        /// <summary>
        /// Command to end the search
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand EndSearch { get; }

        /// <summary>
        /// Command to toggle the advanced search
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ToggleAdvancedSearch { get; }

        /// <summary>
        /// Command to save the filter
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand SaveFilter { get; }

        /// <summary>
        /// Clear the quick search focus
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand QuickSearchClearFocus { get; }

        /// <summary>
        /// Is the advanced search open
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsAdvancedSearchOpen
        {
            get => isAdvancedSearchOpen;
            set
            {
                if (Set(ref isAdvancedSearchOpen, value) && value)
                {
                    IsSearchActive = true;
                }
            }
        }

        /// <summary>
        /// Title of the header
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        /// <summary>
        /// Header image
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Image HeaderImage
        {
            get => headerImage;
            set => Set(ref headerImage, value);
        }

        /// <summary>
        /// Is there a search in progress
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsSearching
        {
            get => isSearching;
            set => Set(ref isSearching, value);
        }

        /// <summary>
        /// Is the search active
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsSearchActive
        {
            get => isSearchActive;
            set
            {
                if (Set(ref isSearchActive, value) && !value)
                {
                    EndSearchImage = ShipWorks.Properties.Resources.buttonEndSearchDisabled;
                }
            }
        }

        /// <summary>
        /// Text that's being searched
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SearchText
        {
            get => searchText;
            set => Set(ref searchText, value);
        }

        /// <summary>
        /// Text to show in the watermark
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string WatermarkText
        {
            get => watermarkText;
            set => Set(ref watermarkText, value);
        }

        /// <summary>
        /// Name of the filter being edited
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string FilterName
        {
            get => filterName;
            set => Set(ref filterName, value);
        }

        /// <summary>
        /// Image to use for the end search button
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Image EndSearchImage
        {
            get => endSearchImage;
            set => Set(ref endSearchImage, value);
        }

        /// <summary>
        /// End the current search
        /// </summary>
        private void EndSearchAction() => SearchEndClicked?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Toggle the advanced search
        /// </summary>
        private void ToggleAdvancedSearchAction() =>
            IsAdvancedSearchOpen = !IsAdvancedSearchOpen;

        /// <summary>
        /// Save the advanced search
        /// </summary>
        private void SaveFilterAction() =>
            FilterSaveClicked?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Clear the quick search focus
        /// </summary>
        private void QuickSearchClearFocusAction() =>
            QuickSearchFocusCleared?.Invoke(this, EventArgs.Empty);
    }
}
