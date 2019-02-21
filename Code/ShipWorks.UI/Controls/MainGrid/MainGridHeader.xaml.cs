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
    }
}
