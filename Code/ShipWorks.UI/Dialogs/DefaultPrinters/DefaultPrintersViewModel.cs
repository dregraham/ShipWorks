using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShipWorks.UI.Dialogs.DefaultPrinters
{
    public class DefaultPrintersViewModel
    {
        [Obfuscation]
        public ICommand SetDefaults { get; }

        public bool OverrideExistingPrinters { get; set; }

        public string ThermalPrinterName { get; set; }

        public string ThermalPaperSource { get; set; }

        public string StandardPrinterName { get; set; }

        public string StandardPaperSource { get; set; }
    }
}
