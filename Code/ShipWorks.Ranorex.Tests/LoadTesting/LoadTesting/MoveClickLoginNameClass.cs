/*
 * Created by Ranorex
 * User: jeman
 * Date: 6/9/2017
 * Time: 10:38 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
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
	/// <summary>
	/// Description of MoveMovingClass.
	/// </summary>
	public class MoveClickLoginNameClass
	{
		public MoveClickLoginNameClass()
		{
		}
		
		public void MoveToName(RepoItemInfo rawtextInfoJon, RepoItemInfo rawtextInfoBrett, RepoItemInfo rawtextInfoKatie, RepoItemInfo rawtextInfoMirza)
        {
        	
        	switch(Environment.MachineName)
        	{
        	
        		case "QATesting-PC":
        		
            		Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move and click item 'rawtextInfo' at Center.", rawtextInfoJon);
            		rawtextInfoJon.FindAdapter<RawText>().MoveTo();
            		rawtextInfoJon.FindAdapter<RawText>().Click();
                    break;
            
                case "QATesting-PC2":
                    
            		Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'rawtextInfo' at Center.", rawtextInfoBrett);
            		rawtextInfoBrett.FindAdapter<RawText>().MoveTo();
            		rawtextInfoBrett.FindAdapter<RawText>().Click();
            		break;
            
            	case "QATesting-PC3":
            		
           			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'rawtextInfo' at Center.", rawtextInfoKatie);
           			rawtextInfoKatie.FindAdapter<RawText>().MoveTo();
           			rawtextInfoKatie.FindAdapter<RawText>().Click();
           			break;
           			
           		default:
           			
           			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'rawtextInfo' at Center.", rawtextInfoMirza);
           			rawtextInfoMirza.FindAdapter<RawText>().MoveTo();
           			rawtextInfoMirza.FindAdapter<RawText>().Click();
           			break;           
            
            
        	}
        }
	}
}
