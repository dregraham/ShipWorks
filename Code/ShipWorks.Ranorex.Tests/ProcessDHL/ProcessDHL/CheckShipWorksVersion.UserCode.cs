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
using System.Net.Mail;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Repository;
using Ranorex.Core.Testing;

namespace ProcessDHL
{
	
	public static class SWVersion
	{
		public static string SWVersionNumber;
	}
	
    public partial class CheckShipWorksVersion
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }

        public void Get_Value_Version()
        {
           
        }

        public void GrabVersion()
        {
            //Output the version number to the report
            SWVersion.SWVersionNumber = ShipWorksVersion;
             Report.Log(ReportLevel.Info, "Get Value", "The version number is " + SWVersion.SWVersionNumber, repo.ShipWorksSa.VersionInfo, new RecordItemIndex(6));
        }



    }
}
