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

namespace SmokeTest
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The GetExpress1DomesticShipmentRate recording.
    /// </summary>
    [TestModule("a3b4fdce-ac31-41ad-adec-cdf21a303e24", ModuleType.Recording, 1)]
    public partial class GetExpress1DomesticShipmentRate : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static GetExpress1DomesticShipmentRate instance = new GetExpress1DomesticShipmentRate();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public GetExpress1DomesticShipmentRate()
        {
            ExpOneAvailablePostage = "50";
            ExpOneShipmentRate = "50";
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static GetExpress1DomesticShipmentRate Instance
        {
            get { return instance; }
        }

#region Variables

        string _ExpOneAvailablePostage;

        /// <summary>
        /// Gets or sets the value of variable ExpOneAvailablePostage.
        /// </summary>
        [TestVariable("f2c6b442-bbd8-4dca-8fa2-95674c673f9f")]
        public string ExpOneAvailablePostage
        {
            get { return _ExpOneAvailablePostage; }
            set { _ExpOneAvailablePostage = value; }
        }

        string _ExpOneShipmentRate;

        /// <summary>
        /// Gets or sets the value of variable ExpOneShipmentRate.
        /// </summary>
        [TestVariable("befb0f22-9c6f-41d6-8d15-2dc192b98f73")]
        public string ExpOneShipmentRate
        {
            get { return _ExpOneShipmentRate; }
            set { _ExpOneShipmentRate = value; }
        }

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

            // Move mouse to shipping settings
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to shipping settings\r\nMouse Left Move item 'ShippingDlg.ShippingSettings' at Center.", repo.ShippingDlg.ShippingSettingsInfo, new RecordItemIndex(0));
            repo.ShippingDlg.ShippingSettings.MoveTo();
            Delay.Milliseconds(200);
            
            // Click shipping settings
            Report.Log(ReportLevel.Info, "Mouse", "Click shipping settings\r\nMouse Left Click item 'ShippingDlg.ShippingSettings' at Center.", repo.ShippingDlg.ShippingSettingsInfo, new RecordItemIndex(1));
            repo.ShippingDlg.ShippingSettings.Click();
            Delay.Milliseconds(200);
            
            // Move mouse to Express 1
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to Express 1\r\nMouse Left Move item 'ShippingSettingsDlg.USPSExpress11' at Center.", repo.ShippingSettingsDlg.USPSExpress11Info, new RecordItemIndex(2));
            repo.ShippingSettingsDlg.USPSExpress11.MoveTo();
            Delay.Milliseconds(200);
            
            // Click on Express 1
            Report.Log(ReportLevel.Info, "Mouse", "Click on Express 1\r\nMouse Left Click item 'ShippingSettingsDlg.USPSExpress11' at Center.", repo.ShippingSettingsDlg.USPSExpress11Info, new RecordItemIndex(3));
            repo.ShippingSettingsDlg.USPSExpress11.Click();
            Delay.Milliseconds(200);
            
            // Move mouse to check postage
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to check postage\r\nMouse Left Move item 'ShippingSettingsDlg.Express1PostageBalance50' at 25;8.", repo.ShippingSettingsDlg.Express1PostageBalance50Info, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.Express1PostageBalance50.MoveTo("25;8");
            Delay.Milliseconds(200);
            
            // Get the postage value and store it in a variable
            Report.Log(ReportLevel.Info, "Get Value", "Get the postage value and store it in a variable\r\nGetting attribute 'RawText' from item 'ShippingSettingsDlg.Express1PostageBalance50' and assigning its value to variable 'ExpOneAvailablePostage'.", repo.ShippingSettingsDlg.Express1PostageBalance50Info, new RecordItemIndex(5));
            ExpOneAvailablePostage = repo.ShippingSettingsDlg.Express1PostageBalance50.Element.GetAttributeValueText("RawText");
            Delay.Milliseconds(0);
            
            // Move mouse to close button
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to close button\r\nMouse Left Move item 'ShippingSettingsDlg.Close1' at Center.", repo.ShippingSettingsDlg.Close1Info, new RecordItemIndex(6));
            repo.ShippingSettingsDlg.Close1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the close button
            Report.Log(ReportLevel.Info, "Mouse", "Click the close button\r\nMouse Left Click item 'ShippingSettingsDlg.Close1' at Center.", repo.ShippingSettingsDlg.Close1Info, new RecordItemIndex(7));
            repo.ShippingSettingsDlg.Close1.Click();
            Delay.Milliseconds(200);
            
            // Move mouse to check the shipment rate
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to check the shipment rate\r\nMouse Left Move item 'ShipOrders1.SplitContainer.DomesticShipmentRate' at Center.", repo.ShipOrders1.SplitContainer.DomesticShipmentRateInfo, new RecordItemIndex(8));
            repo.ShipOrders1.SplitContainer.DomesticShipmentRate.MoveTo();
            Delay.Milliseconds(200);
            
            // Get the shipment rate and store it in a variable
            Report.Log(ReportLevel.Info, "Get Value", "Get the shipment rate and store it in a variable\r\nGetting attribute 'RawText' from item 'ShipOrders1.SplitContainer.DomesticShipmentRate' and assigning its value to variable 'ExpOneShipmentRate'.", repo.ShipOrders1.SplitContainer.DomesticShipmentRateInfo, new RecordItemIndex(9));
            ExpOneShipmentRate = repo.ShipOrders1.SplitContainer.DomesticShipmentRate.Element.GetAttributeValueText("RawText");
            Delay.Milliseconds(0);
            
            // Compare the available postage with the shipment rate and, if there is enough postage available, process the shipment.
            CheckPostageBalance();
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
