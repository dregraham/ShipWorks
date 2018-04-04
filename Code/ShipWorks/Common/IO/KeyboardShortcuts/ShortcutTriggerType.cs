using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum ShortcutTriggerType
    {
        Hotkey = 0,

        Barcode = 1
    }
}
