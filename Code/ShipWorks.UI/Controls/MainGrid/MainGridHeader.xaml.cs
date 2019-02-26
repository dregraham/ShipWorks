using System.Windows;
using System.Windows.Controls;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;

namespace ShipWorks.UI.Controls.MainGrid
{
    /// <summary>
    /// Interaction logic for MainGridHeader.xaml
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IMainGridHeader))]
    public partial class MainGridHeader : UserControl, IMainGridHeader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainGridHeader()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get the actual control
        /// </summary>
        public UIElement Control => this;

        /// <summary>
        /// View model for the control
        /// </summary>
        public MainGridHeaderViewModel ViewModel
        {
            get => (MainGridHeaderViewModel) DataContext;
            set => DataContext = value;
        }

        /// <summary>
        /// Handle key down of the search box to deal with esc key
        /// </summary>
        private void OnSearchBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                ViewModel?.QuickSearchClearFocus.Execute(null);
            }
        }
    }
}
