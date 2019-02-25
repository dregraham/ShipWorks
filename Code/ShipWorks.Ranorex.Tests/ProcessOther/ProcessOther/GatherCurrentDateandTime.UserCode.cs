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

namespace ProcessOther
{
    public partial class GatherCurrentDateandTime
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        /// 
        public System.DateTime localDate = System.DateTime.Now;
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }

        public void DateandTime()
        {
            // Gather the Date and Time of the computer being used to run the SmokeTest Suite
            

            Report.Log(ReportLevel.Info, "Get Value", "Checking the Date and Time of the computer running the Smoke Test Suite " + localDate);
                       
        }
        
        
        	
    }
}
