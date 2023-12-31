﻿///////////////////////////////////////////////////////////////////////////////
//
// This file was automatically generated by RANOREX.
// DO NOT MODIFY THIS FILE! It is regenerated by the designer.
// All your modifications will be lost!
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
using Ranorex.Core.Testing;
using Ranorex.Core.Repository;

namespace UIResponsiveness
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The SelectAllOrders_AccessShipOrdersDialog recording.
    /// </summary>
    [TestModule("4a582960-001b-4749-8c50-a184241b269d", ModuleType.Recording, 1)]
    public partial class SelectAllOrders_AccessShipOrdersDialog : ITestModule
    {
        /// <summary>
        /// Holds an instance of the UIResponsivenessRepository repository.
        /// </summary>
        public static UIResponsivenessRepository repo = UIResponsivenessRepository.Instance;

        static SelectAllOrders_AccessShipOrdersDialog instance = new SelectAllOrders_AccessShipOrdersDialog();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SelectAllOrders_AccessShipOrdersDialog()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SelectAllOrders_AccessShipOrdersDialog Instance
        {
            get { return instance; }
        }

#region Variables

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "5.4.5")]
        public static void Start()
        {
            TestModuleRunner.Run(Instance);
        }

        /// <summary>
        /// Performs the playback of actions in this recording.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "5.4.5")]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;

            Init();

            // Click: 2nd Order in Main Grid
            Report.Log(ReportLevel.Info, "Mouse", "Click: 2nd Order in Main Grid\r\nMouse Left Click item 'MainForm.MainGridRow2Column1' at Center.", repo.MainForm.MainGridRow2Column1Info, new RecordItemIndex(0));
            repo.MainForm.MainGridRow2Column1.Click();
            Delay.Milliseconds(200);
            
            // CTRL+A to select all orders
            Report.Log(ReportLevel.Info, "Keyboard", "CTRL+A to select all orders\r\nKey 'Ctrl+A' Press with focus on 'MainForm.MainGridRow2Column1'.", repo.MainForm.MainGridRow2Column1Info, new RecordItemIndex(1));
            Keyboard.PrepareFocus(repo.MainForm.MainGridRow2Column1);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Click: Ship Ordrs button (Shipping Panel)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Ship Ordrs button (Shipping Panel)\r\nMouse Left Click item 'ShipWorks1.ShipOrders_ShippingPanel' at Center.", repo.ShipWorks1.ShipOrders_ShippingPanelInfo, new RecordItemIndex(2));
            repo.ShipWorks1.ShipOrders_ShippingPanel.Click();
            Delay.Milliseconds(200);
            
            // Click: Close (Close Ship Orders dialog)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Close (Close Ship Orders dialog)\r\nMouse Left Click item 'ShippingDlg.Close' at Center.", repo.ShippingDlg.CloseInfo, new RecordItemIndex(3));
            repo.ShippingDlg.Close.Click();
            Delay.Milliseconds(200);
            
            // Click the Home sub-ribbon
            Report.Log(ReportLevel.Info, "Mouse", "Click the Home sub-ribbon\r\nMouse Left Click item 'ShipWorksSa.Home' at Center.", repo.ShipWorksSa.HomeInfo, new RecordItemIndex(4));
            repo.ShipWorksSa.Home.Click();
            Delay.Milliseconds(200);
            
            // Click: Ship Orders
            Report.Log(ReportLevel.Info, "Mouse", "Click: Ship Orders\r\nMouse Left Click item 'ShipWorks1.ShipOrders' at Center.", repo.ShipWorks1.ShipOrdersInfo, new RecordItemIndex(5));
            repo.ShipWorks1.ShipOrders.Click();
            Delay.Milliseconds(200);
            
            // Click: Close (Close Ship Orders dialog)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Close (Close Ship Orders dialog)\r\nMouse Left Click item 'ShippingDlg.Close' at Center.", repo.ShippingDlg.CloseInfo, new RecordItemIndex(6));
            repo.ShippingDlg.Close.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
