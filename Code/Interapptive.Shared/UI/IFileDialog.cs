using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    public interface IFileDialog
    {
        DialogResult ShowOpenFile(string filter);

        DialogResult ShowSaveFile(string filter, string defaultExtension, string initialFileName);

        string FileName { get; }
    }
}
