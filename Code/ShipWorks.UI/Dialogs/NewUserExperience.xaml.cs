using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            NativeWindow win32Window = new NativeWindow();
            win32Window.AssignHandle(new WindowInteropHelper(this).Handle);
            viewModel.SetOwnerInteraction(win32Window, Close);

            DataContext = viewModel;
        }

        /// <summary>
        /// Set the owner of this window
        /// </summary>
        public void LoadOwner(System.Windows.Forms.IWin32Window owner)
        {
            WindowInteropHelper interopHelper = new WindowInteropHelper(this);
            interopHelper.Owner = owner.Handle;
        }
    }
}
