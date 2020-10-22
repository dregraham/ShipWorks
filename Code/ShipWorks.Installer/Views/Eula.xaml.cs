using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.Installer.Views
{
    /// <summary>
    /// Interaction logic for Eula.xaml
    /// </summary>
    public partial class Eula : Page
    {
        public Eula()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ShipWorks.Installer.License.rtf";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                eulaTextBox.Selection.Load(stream, DataFormats.Rtf);
                eulaTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
        }
    }
}
