using Interapptive.Shared.Utility;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Edit a list of string values
    /// </summary>
    public interface IStringValueListEditorViewModel
    {
        /// <summary>
        /// Edit the given list
        /// </summary>
        GenericResult<IEnumerable<string>> EditList(IWin32Window owner, IEnumerable<string> values);
    }
}