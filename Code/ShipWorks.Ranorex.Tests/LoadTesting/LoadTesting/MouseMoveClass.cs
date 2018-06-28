/*
 * Created by Ranorex
 * User: jeman
 * Date: 6/9/2017
 * Time: 10:29 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace LoadTesting
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class MoveMouseClass
	{
        public void MoveToName(RepoItemInfo rawtextInfoJon, RepoItemInfo rawtextInfoBrett, RepoItemInfo rawtextInfoKatie, RepoItemInfo rawtestInfoMirza)
        {
        	
        	switch(Environment.MachineName)
        	{
        	
        		case "QATesting-PC":
        		
            		Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'rawtextInfo' at Center.", rawtextInfo);
            		rawtextInfoJon.FindAdapter<RawText>().MoveTo();
                    break;
            
                case "QATesting-PC2":
                    
            		Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'rawtextInfo' at Center.", rawtextInfo);
            		rawtextInfoBrett.FindAdapter<RawText>().MoveTo();
            		break;
            
            	case "QATesting-PC3":
            		
           			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'rawtextInfo' at Center.", rawtextInfo);
           			rawtextInfoKatie.FindAdapter<RawText>().MoveTo();
           			break;
           			
           		default:
           			
           			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'rawtextInfo' at Center.", rawtextInfo);
           			rawtextInfoMirza.FindAdapter<RawText>().MoveTo();
           			break;           
            
            
        	}
        }
	}
}
