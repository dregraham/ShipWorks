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

namespace DownloadManyStores
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The SetupCarrierTestServers recording.
    /// </summary>
    [TestModule("5099df02-3625-482e-b38d-f1ff20194745", ModuleType.Recording, 1)]
    public partial class SetupCarrierTestServers : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static SetupCarrierTestServers instance = new SetupCarrierTestServers();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SetupCarrierTestServers()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SetupCarrierTestServers Instance
        {
            get { return instance; }
        }

#region Variables

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.1")]
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
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.1")]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            // KeyPress: Magic Key Combo (Left Windows+CTRL+SHIFT)
            //Report.Log(ReportLevel.Info, "Keyboard", "KeyPress: Magic Key Combo (Left Windows+CTRL+SHIFT)\r\nKey sequence '{LControlKey down}{LShiftKey down}{LWin down}'.", new RecordItemIndex(0));
            //Keyboard.Press("{LControlKey down}{LShiftKey down}{LWin down}");
            //Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(1));
            repo.MainForm.Manage.MoveTo();
            Delay.Milliseconds(200);
            
            // Click: Manage
            Report.Log(ReportLevel.Info, "Mouse", "Click: Manage\r\nMouse Left Click item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(2));
            repo.MainForm.Manage.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.Options' at Center.", repo.MainForm.OptionsInfo, new RecordItemIndex(3));
            repo.MainForm.Options.MoveTo();
            Delay.Milliseconds(200);
            
            // Click: Options
            Report.Log(ReportLevel.Info, "Mouse", "Click: Options\r\nMouse Left Click item 'MainForm.Options' at Center.", repo.MainForm.OptionsInfo, new RecordItemIndex(4));
            repo.MainForm.Options.Click();
            Delay.Milliseconds(200);
            
            // Release KeyPress: Magic Key Combo
            Report.Log(ReportLevel.Info, "Keyboard", "Release KeyPress: Magic Key Combo\r\nKey sequence '{LShiftKey up}{LWin up}{LControlKey up}'.", new RecordItemIndex(5));
            Keyboard.Press("{LShiftKey up}{LWin up}{LControlKey up}");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.Interapptive' at Center.", repo.ShipWorksSa.InterapptiveInfo, new RecordItemIndex(6));
            repo.ShipWorksSa.Interapptive.MoveTo();
            Delay.Milliseconds(200);
            
            // Click: Interapptive Menu
            Report.Log(ReportLevel.Info, "Mouse", "Click: Interapptive Menu\r\nMouse Left Click item 'ShipWorksSa.Interapptive' at Center.", repo.ShipWorksSa.InterapptiveInfo, new RecordItemIndex(7));
            repo.ShipWorksSa.Interapptive.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.SectionContainer.PostalWebTestServer' at Center.", repo.ShipWorksSa.SectionContainer.PostalWebTestServerInfo, new RecordItemIndex(8));
            repo.ShipWorksSa.SectionContainer.PostalWebTestServer.MoveTo();
            Delay.Milliseconds(200);
            
            // Check: USPS w/o Postage
            Report.Log(ReportLevel.Info, "Invoke action", "Check: USPS w/o Postage\r\nInvoking Check() on item 'ShipWorksSa.SectionContainer.PostalWebTestServer'.", repo.ShipWorksSa.SectionContainer.PostalWebTestServerInfo, new RecordItemIndex(9));
            repo.ShipWorksSa.SectionContainer.PostalWebTestServer.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.SectionContainer.UspsTestServer' at Center.", repo.ShipWorksSa.SectionContainer.UspsTestServerInfo, new RecordItemIndex(10));
            repo.ShipWorksSa.SectionContainer.UspsTestServer.MoveTo();
            Delay.Milliseconds(200);
            
            // Check: USPS
            Report.Log(ReportLevel.Info, "Invoke action", "Check: USPS\r\nInvoking Check() on item 'ShipWorksSa.SectionContainer.UspsTestServer'.", repo.ShipWorksSa.SectionContainer.UspsTestServerInfo, new RecordItemIndex(11));
            repo.ShipWorksSa.SectionContainer.UspsTestServer.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.SectionContainer.EndiciaTestServer' at Center.", repo.ShipWorksSa.SectionContainer.EndiciaTestServerInfo, new RecordItemIndex(12));
            repo.ShipWorksSa.SectionContainer.EndiciaTestServer.MoveTo();
            Delay.Milliseconds(200);
            
            // Check: USPS (Endicia)
            Report.Log(ReportLevel.Info, "Invoke action", "Check: USPS (Endicia)\r\nInvoking Check() on item 'ShipWorksSa.SectionContainer.EndiciaTestServer'.", repo.ShipWorksSa.SectionContainer.EndiciaTestServerInfo, new RecordItemIndex(13));
            repo.ShipWorksSa.SectionContainer.EndiciaTestServer.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.SectionContainer.Express1EndiciaTestServer' at Center.", repo.ShipWorksSa.SectionContainer.Express1EndiciaTestServerInfo, new RecordItemIndex(14));
            repo.ShipWorksSa.SectionContainer.Express1EndiciaTestServer.MoveTo();
            Delay.Milliseconds(200);
            
            // Check: USPS (Express1 - Endicia)
            Report.Log(ReportLevel.Info, "Invoke action", "Check: USPS (Express1 - Endicia)\r\nInvoking Uncheck() on item 'ShipWorksSa.SectionContainer.Express1EndiciaTestServer'.", repo.ShipWorksSa.SectionContainer.Express1EndiciaTestServerInfo, new RecordItemIndex(15));
            repo.ShipWorksSa.SectionContainer.Express1EndiciaTestServer.Uncheck();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.SectionContainer.Express1UspsTestServer' at Center.", repo.ShipWorksSa.SectionContainer.Express1UspsTestServerInfo, new RecordItemIndex(16));
            repo.ShipWorksSa.SectionContainer.Express1UspsTestServer.MoveTo();
            Delay.Milliseconds(200);
            
            // Check: USPS (Express1 - USPS)
            Report.Log(ReportLevel.Info, "Invoke action", "Check: USPS (Express1 - USPS)\r\nInvoking Uncheck() on item 'ShipWorksSa.SectionContainer.Express1UspsTestServer'.", repo.ShipWorksSa.SectionContainer.Express1UspsTestServerInfo, new RecordItemIndex(17));
            repo.ShipWorksSa.SectionContainer.Express1UspsTestServer.Uncheck();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.SectionContainer.FedexTestServer' at Center.", repo.ShipWorksSa.SectionContainer.FedexTestServerInfo, new RecordItemIndex(18));
            repo.ShipWorksSa.SectionContainer.FedexTestServer.MoveTo();
            Delay.Milliseconds(200);
            
            // Check: FedEx
            Report.Log(ReportLevel.Info, "Invoke action", "Check: FedEx\r\nInvoking Check() on item 'ShipWorksSa.SectionContainer.FedexTestServer'.", repo.ShipWorksSa.SectionContainer.FedexTestServerInfo, new RecordItemIndex(19));
            repo.ShipWorksSa.SectionContainer.FedexTestServer.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.SectionContainer.UpsOnLineTools' at Center.", repo.ShipWorksSa.SectionContainer.UpsOnLineToolsInfo, new RecordItemIndex(20));
            repo.ShipWorksSa.SectionContainer.UpsOnLineTools.MoveTo();
            Delay.Milliseconds(200);
            
            // UnCheck: UPS
            Report.Log(ReportLevel.Info, "Invoke action", "UnCheck: UPS\r\nInvoking Check() on item 'ShipWorksSa.SectionContainer.UpsOnLineTools'.", repo.ShipWorksSa.SectionContainer.UpsOnLineToolsInfo, new RecordItemIndex(21));
            repo.ShipWorksSa.SectionContainer.UpsOnLineTools.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.SectionContainer.OnTracTestServer' at Center.", repo.ShipWorksSa.SectionContainer.OnTracTestServerInfo, new RecordItemIndex(22));
            repo.ShipWorksSa.SectionContainer.OnTracTestServer.MoveTo();
            Delay.Milliseconds(200);
            
            // Check: OnTrac
            Report.Log(ReportLevel.Info, "Invoke action", "Check: OnTrac\r\nInvoking Check() on item 'ShipWorksSa.SectionContainer.OnTracTestServer'.", repo.ShipWorksSa.SectionContainer.OnTracTestServerInfo, new RecordItemIndex(23));
            repo.ShipWorksSa.SectionContainer.OnTracTestServer.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.SectionContainer.FedexListRates' at Center.", repo.ShipWorksSa.SectionContainer.FedexListRatesInfo, new RecordItemIndex(24));
            repo.ShipWorksSa.SectionContainer.FedexListRates.MoveTo();
            Delay.Milliseconds(200);
            
            // Check: Use FedEx LIST rates
            Report.Log(ReportLevel.Info, "Invoke action", "Check: Use FedEx LIST rates\r\nInvoking Check() on item 'ShipWorksSa.SectionContainer.FedexListRates'.", repo.ShipWorksSa.SectionContainer.FedexListRatesInfo, new RecordItemIndex(25));
            repo.ShipWorksSa.SectionContainer.FedexListRates.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.SectionContainer.UseInsureShipTestServer' at Center.", repo.ShipWorksSa.SectionContainer.UseInsureShipTestServerInfo, new RecordItemIndex(26));
            repo.ShipWorksSa.SectionContainer.UseInsureShipTestServer.MoveTo();
            Delay.Milliseconds(200);
            
            // Check: Use InsureShip Test Server
            Report.Log(ReportLevel.Info, "Invoke action", "Check: Use InsureShip Test Server\r\nInvoking Check() on item 'ShipWorksSa.SectionContainer.UseInsureShipTestServer'.", repo.ShipWorksSa.SectionContainer.UseInsureShipTestServerInfo, new RecordItemIndex(27));
            repo.ShipWorksSa.SectionContainer.UseInsureShipTestServer.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.SectionContainer.Ebay' at Center.", repo.ShipWorksSa.SectionContainer.EbayInfo, new RecordItemIndex(28));
            repo.ShipWorksSa.SectionContainer.Ebay.MoveTo();
            Delay.Milliseconds(200);
            
            // Check: Use eBay Test Server
            Report.Log(ReportLevel.Info, "Invoke action", "Check: Use eBay Test Server\r\nInvoking Check() on item 'ShipWorksSa.SectionContainer.Ebay'.", repo.ShipWorksSa.SectionContainer.EbayInfo, new RecordItemIndex(29));
            repo.ShipWorksSa.SectionContainer.Ebay.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorksSa.ButtonOk' at Center.", repo.ShipWorksSa.ButtonOkInfo, new RecordItemIndex(30));
            repo.ShipWorksSa.ButtonOk.MoveTo();
            Delay.Milliseconds(200);
            
            // Click: OK
            Report.Log(ReportLevel.Info, "Mouse", "Click: OK\r\nMouse Left Click item 'ShipWorksSa.ButtonOk' at Center.", repo.ShipWorksSa.ButtonOkInfo, new RecordItemIndex(31));
            repo.ShipWorksSa.ButtonOk.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
