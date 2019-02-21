using System.Drawing;
using System.Reflection;
using GalaSoft.MvvmLight;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// View model for the main grid header
    /// </summary>
    public class MainGridHeaderViewModel : ViewModelBase
    {
        private bool isAdvancedSearchOpen;
        private string text;
        private Image headerImage;
        private bool isSearching;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainGridHeaderViewModel()
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                Text = "All";
                HeaderImage = ShipWorks.Properties.Resources.view;
            }
        }

        [Obfuscation]
        public bool IsAdvancedSearchOpen
        {
            get => isAdvancedSearchOpen;
            set => Set(ref isAdvancedSearchOpen, value);
        }

        [Obfuscation]
        public string Text
        {
            get => text;
            set => Set(ref text, value);
        }

        [Obfuscation]
        public Image HeaderImage
        {
            get => headerImage;
            set => Set(ref headerImage, value);
        }

        [Obfuscation]
        public bool IsSearching
        {
            get => isSearching;
            set => Set(ref isSearching, value);
        }
    }
}
