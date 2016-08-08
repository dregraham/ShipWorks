using System.ComponentModel;
using System.Reflection;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// File Dialog Type
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]

    public enum FileDialogType
    {
        [Description("Open File")]
        Open = 0,

        [Description("Save File")]
        Save=1
    }
}
