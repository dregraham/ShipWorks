using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interop;

namespace ShipWorks.UI.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Interaction logic for StringValueListEditorDialog.xaml
    /// </summary>
    [Component]
    public partial class StringValueListEditorDialog : Window, IStringValueListEditorDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StringValueListEditorDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StringValueListEditorDialog(IStringValueListEditorViewModel viewModel) : this()
        {
            this.DataContext = viewModel;
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
