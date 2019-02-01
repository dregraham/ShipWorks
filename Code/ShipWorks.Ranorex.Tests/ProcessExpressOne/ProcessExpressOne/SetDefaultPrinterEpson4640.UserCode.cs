﻿///////////////////////////////////////////////////////////////////////////////
//
// This file was automatically generated by RANOREX.
// Your custom recording code should go in this file.
// The designer will only add methods to this file, so your custom code won't be overwritten.
// http://www.ranorex.com
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Repository;
using Ranorex.Core.Testing;

namespace ProcessExpressOne
{
    public partial class SetDefaultPrinterEpson4640
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }

        public void SetDefaultPrinter()
        {
            string setDefaultPrinterBATPath = @"\BatchFile\SetDefaultPrinterEpson4640.bat";
        	string currentDir = System.IO.Directory.GetCurrentDirectory();
        	string smokeTestPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(currentDir,@"..\..\"));
        	
        	//	Runs the batch file in the folder BatchFiles. This batch file set the default printer to 'Epson4640'.
        	System.Diagnostics.Process.Start(string.Concat(smokeTestPath,setDefaultPrinterBATPath));
        }

    }
}
