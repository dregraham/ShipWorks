using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Administration;

namespace ShipWorks.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for NewUserExperience.xaml
    /// </summary>
    [Component]
    public partial class NewUserExperience : Window, INewUserExperience
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NewUserExperience()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NewUserExperience(INewUserExperienceViewModel viewModel) : this()
        {
            DataContext = viewModel;

            Loaded += (s, e) =>
            {
                NativeWindow win32Window = new NativeWindow();
                win32Window.AssignHandle(new WindowInteropHelper(this).Handle);
                viewModel.SetOwnerInteraction(win32Window, Close);
            };
        }

        /// <summary>
        /// Set the owner of this window
        /// </summary>
        [SuppressMessage("SonarQube", "S1848:Objects should not be created to be dropped immediately without being used",
            Justification = "The interop helper is only used temporarily to set this window's owner")]
        [SuppressMessage("Recommendations", "RECS0026:Objects should not be created to be dropped immediately without being used",
            Justification = "The interop helper is only used temporarily to set this window's owner")]
        public void LoadOwner(System.Windows.Forms.IWin32Window owner)
        {
            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }
    }
}
