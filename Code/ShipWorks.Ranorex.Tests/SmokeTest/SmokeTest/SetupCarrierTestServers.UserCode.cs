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
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Repository;
using Ranorex.Core.Testing;

namespace SmokeTest
{
    public partial class SetupCarrierTestServers
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }

        public void RunTestServerCMD()
        {
            // TODO: Replace the following line with your code implementation.

        	string smokeTestPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),@"..\..\"));
        	string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        	
        	
        	if(Environ == "Production")
        	{
            Report.Log(ReportLevel.Info, "Application", "Run TestServers.cmd to change test server settings in registry");
            Host.Local.RunApplication(smokeTestPath + @"\ZipFiles\TestServers.cmd", "", "", false);
        	}
            
            else
            {
            Report.Log(ReportLevel.Info, "Application", "Run TestServers2.cmd to change test server settings in registry");
            Host.Local.RunApplication(smokeTestPath + @"\ZipFiles\TestServers2.cmd", "", "", false);
            }
        }


    }
}