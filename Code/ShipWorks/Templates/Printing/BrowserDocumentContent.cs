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
    public class BrowserDocumentContent
    {
        string contentSource = "document";

        int templateResultsTotal;
        int templateResultsDisplayed;

        bool isLabelTemplate = false;
        int labelSheetsRequired = 1;

        /// <summary>
        /// The source of the displayed html content
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ContentSource
        {
            get { return contentSource; }
            internal set { contentSource = value; }
        }

        /// <summary>
        /// Indicates if the content are labels
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsLabelSheet
        {
            get { return isLabelTemplate; }
            internal set { isLabelTemplate = value; }
        }

        /// <summary>
        /// Indicates how many total label sheets are required.  Due to margins used for offetting for printer calibration, and how IE
        /// detects that as overflow, we'd end up with extra pages if we didnt manually cap the page count to this number.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int LabelSheetsRequired
        {
            get { return labelSheetsRequired; }
            internal set { labelSheetsRequired = value; }
        }

        /// <summary>
        /// Total number of template results that were selected for printing
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int TemplateResultsTotal
        {
            get { return templateResultsTotal; }
            internal set { templateResultsTotal = value; }
        }

        /// <summary>
        /// The number of template results that are contained in the preview document content
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int TemplateResultsDisplayed
        {
            get { return templateResultsDisplayed; }
            internal set { templateResultsDisplayed = value; }
        }
    }
}
