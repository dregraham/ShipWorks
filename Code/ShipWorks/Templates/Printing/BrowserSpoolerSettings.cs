using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace ShipWorks.Templates.Printing
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class BrowserSpoolerSettings
    {
        string jobName;
        int copies;
        bool collate;

        /// <summary>
        /// Controls how 
        /// </summary>
        public BrowserSpoolerSettings(string jobName, int copies, bool collate)
        {
            this.jobName = jobName;
            this.copies = copies;
            this.collate = collate;
        }

        [Obfuscation(Exclude = true)]
        public string JobName
        {
            get { return jobName; }
        }

        [Obfuscation(Exclude = true)]
        public int Copies
        {
            get { return copies; }
        }

        [Obfuscation(Exclude = true)]
        public bool Collate
        {
            get { return collate; }
        }
    }
}
