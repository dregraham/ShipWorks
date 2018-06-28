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
    public partial class AllOrdersFilter_SelectFirstRow_ShipOrders2
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }

        
        
        public void CloseShipOrders(RepoItemInfo buttonInfo)
        {
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'buttonInfo' at Center.", buttonInfo);
            
            //	Clicks the Close button if it exists.
            if (buttonInfo.Exists(5000))
            {
            	repo.ShippingDlg.Close.MoveTo();
            	repo.ShippingDlg.Close.Click();
            	
            	
            	          	  // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.Orders' at Center.", repo.ShipWorksSa.OrdersInfo, new RecordItemIndex(0));
            repo.ShipWorksSa.Orders.MoveTo();
            
            // Click the Orders tab in filter pnael in case user is on Customer tab of filters
            Report.Log(ReportLevel.Info, "Mouse", "Click the Orders tab in filter pnael in case user is on Customer tab of filters\r\nMouse Left Click item 'ShipWorksSa.Orders' at Center.", repo.ShipWorksSa.OrdersInfo, new RecordItemIndex(1));
            repo.ShipWorksSa.Orders.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.FilterAllOrders' at Center.", repo.MainForm.FilterAllOrdersInfo, new RecordItemIndex(2));
            repo.MainForm.FilterAllOrders.MoveTo();
            
            // Click the All filter to view all orders
            Report.Log(ReportLevel.Info, "Mouse", "Click the All filter to view all orders\r\nMouse Left Click item 'MainForm.FilterAllOrders' at Center.", repo.MainForm.FilterAllOrdersInfo, new RecordItemIndex(3));
            repo.MainForm.FilterAllOrders.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.MainGridRow2Column1' at Center.", repo.MainForm.MainGridRow2Column1Info, new RecordItemIndex(4));
            repo.MainForm.MainGridRow2Column1.MoveTo();
            
            // Click the 2nd row in the grid (in case multiples are highlighted already)
            Report.Log(ReportLevel.Info, "Mouse", "Click the 2nd row in the grid (in case multiples are highlighted already)\r\nMouse Left Click item 'MainForm.MainGridRow2Column1' at Center.", repo.MainForm.MainGridRow2Column1Info, new RecordItemIndex(5));
            repo.MainForm.MainGridRow2Column1.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Right Move item 'MainForm.MainGridRow2Column1' at Center.", repo.MainForm.MainGridRow2Column1Info, new RecordItemIndex(6));
            repo.MainForm.MainGridRow2Column1.MoveTo();
            
            // Right click the 2nd row in the grid
            Report.Log(ReportLevel.Info, "Mouse", "Right click the 2nd row in the grid\r\nMouse Right Click item 'MainForm.MainGridRow2Column1' at Center.", repo.MainForm.MainGridRow2Column1Info, new RecordItemIndex(7));
            repo.MainForm.MainGridRow2Column1.Click(System.Windows.Forms.MouseButtons.Right);
            }
        }
        public void VerifyShipOrdersExists()
        {
      
            {
            repo.ShippingDlg.Close.MoveTo();
            repo.ShippingDlg.Close.Click();
            	
          	  // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.Orders' at Center.", repo.ShipWorksSa.OrdersInfo, new RecordItemIndex(0));
            repo.ShipWorksSa.Orders.MoveTo();
            
            // Click the Orders tab in filter pnael in case user is on Customer tab of filters
            Report.Log(ReportLevel.Info, "Mouse", "Click the Orders tab in filter pnael in case user is on Customer tab of filters\r\nMouse Left Click item 'ShipWorksSa.Orders' at Center.", repo.ShipWorksSa.OrdersInfo, new RecordItemIndex(1));
            repo.ShipWorksSa.Orders.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.FilterAllOrders' at Center.", repo.MainForm.FilterAllOrdersInfo, new RecordItemIndex(2));
            repo.MainForm.FilterAllOrders.MoveTo();
            
            // Click the All filter to view all orders
            Report.Log(ReportLevel.Info, "Mouse", "Click the All filter to view all orders\r\nMouse Left Click item 'MainForm.FilterAllOrders' at Center.", repo.MainForm.FilterAllOrdersInfo, new RecordItemIndex(3));
            repo.MainForm.FilterAllOrders.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.MainGridRow2Column1' at Center.", repo.MainForm.MainGridRow2Column1Info, new RecordItemIndex(4));
            repo.MainForm.MainGridRow2Column1.MoveTo();
            
            // Click the 2nd row in the grid (in case multiples are highlighted already)
            Report.Log(ReportLevel.Info, "Mouse", "Click the 2nd row in the grid (in case multiples are highlighted already)\r\nMouse Left Click item 'MainForm.MainGridRow2Column1' at Center.", repo.MainForm.MainGridRow2Column1Info, new RecordItemIndex(5));
            repo.MainForm.MainGridRow2Column1.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Right Move item 'MainForm.MainGridRow2Column1' at Center.", repo.MainForm.MainGridRow2Column1Info, new RecordItemIndex(6));
            repo.MainForm.MainGridRow2Column1.MoveTo();
            
            // Right click the 2nd row in the grid
            Report.Log(ReportLevel.Info, "Mouse", "Right click the 2nd row in the grid\r\nMouse Right Click item 'MainForm.MainGridRow2Column1' at Center.", repo.MainForm.MainGridRow2Column1Info, new RecordItemIndex(7));
            repo.MainForm.MainGridRow2Column1.Click(System.Windows.Forms.MouseButtons.Right);
            }

        }

    }
}