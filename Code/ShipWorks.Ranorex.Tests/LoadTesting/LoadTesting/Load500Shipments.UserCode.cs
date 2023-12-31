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

namespace LoadTesting
{
    public partial class Load500Shipments
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }

        public void SelectNamedFilter()
        {
          switch(Environment.MachineName)
        	{
        	
        		case "QATesting-PC":
        		
            		Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move and click item 'Jon' at Center.", new RecordItemIndex(1));
            		repo.MainForm.DockableWindowOrderFilters.Jon.MoveTo();
            		repo.MainForm.DockableWindowOrderFilters.Jon.Click();
            		break;
            
                case "QATesting-PC2":
                    
            		//Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'rawtextInfo' at Center.", repo.MainForm.DockableWindowOrderFilters.Brett);
            		repo.MainForm.DockableWindowOrderFilters.Brett.Click();
            		repo.MainForm.DockableWindowOrderFilters.Brett.MoveTo();
            		break;
            
            	case "QATesting-PC3":
            		
           			//Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'rawtextInfo' at Center.", repo.MainForm.DockableWindowOrderFilters.Katie);
            		repo.MainForm.DockableWindowOrderFilters.Katie.Click();
            		repo.MainForm.DockableWindowOrderFilters.Katie.MoveTo();
           			break;
           			
           		default:
           			
           			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move and click item 'Mirza' at Center.",  new RecordItemIndex(1));
            		repo.MainForm.DockableWindowOrderFilters.Mirza.Click();
            		repo.MainForm.DockableWindowOrderFilters.Mirza.MoveTo();
           			break; 
             }
            
        }

    }
}
