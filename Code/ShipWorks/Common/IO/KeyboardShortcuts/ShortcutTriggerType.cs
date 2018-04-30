using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// The types of shortcut triggers
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum ShortcutTriggerType
    {
        [Description("Hotkey")]
        Hotkey = 0,

        [Description("Barcode")]
        Barcode = 1
    }
}
