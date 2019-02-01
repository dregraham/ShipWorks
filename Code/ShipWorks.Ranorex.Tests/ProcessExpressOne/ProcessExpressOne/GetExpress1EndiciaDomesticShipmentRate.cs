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

namespace ProcessExpressOne
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The GetExpress1EndiciaDomesticShipmentRate recording.
    /// </summary>
    [TestModule("66d3a392-6a35-4843-a7d4-889989d9a7e5", ModuleType.Recording, 1)]
    public partial class GetExpress1EndiciaDomesticShipmentRate : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static GetExpress1EndiciaDomesticShipmentRate instance = new GetExpress1EndiciaDomesticShipmentRate();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public GetExpress1EndiciaDomesticShipmentRate()
        {
            ExpOneEndiciaAvailablePostage = "50";
            ExpOneEndiciaShipmentRate = "50";
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static GetExpress1EndiciaDomesticShipmentRate Instance
        {
            get { return instance; }
        }

#region Variables

        string _ExpOneEndiciaAvailablePostage;

        /// <summary>
        /// Gets or sets the value of variable ExpOneEndiciaAvailablePostage.
        /// </summary>
        [TestVariable("bb13a931-b930-4e05-975d-da66d318d1c7")]
        public string ExpOneEndiciaAvailablePostage
        {
            get { return _ExpOneEndiciaAvailablePostage; }
            set { _ExpOneEndiciaAvailablePostage = value; }
        }

        string _ExpOneEndiciaShipmentRate;

        /// <summary>
        /// Gets or sets the value of variable ExpOneEndiciaShipmentRate.
        /// </summary>
        [TestVariable("10629c5e-93fc-4f4c-9fbf-16142af698fd")]
        public string ExpOneEndiciaShipmentRate
        {
            get { return _ExpOneEndiciaShipmentRate; }
            set { _ExpOneEndiciaShipmentRate = value; }
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
            
            // Move mouse to Express 1 Endicia
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to Express 1 Endicia\r\nMouse Left Move item 'ShippingSettingsDlg.USPSExpress1ForEndicia1' at Center.", repo.ShippingSettingsDlg.USPSExpress1ForEndicia1Info, new RecordItemIndex(2));
            repo.ShippingSettingsDlg.USPSExpress1ForEndicia1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click on Express 1 Endicia
            Report.Log(ReportLevel.Info, "Mouse", "Click on Express 1 Endicia\r\nMouse Left Click item 'ShippingSettingsDlg.USPSExpress1ForEndicia1' at Center.", repo.ShippingSettingsDlg.USPSExpress1ForEndicia1Info, new RecordItemIndex(3));
            repo.ShippingSettingsDlg.USPSExpress1ForEndicia1.Click();
            Delay.Milliseconds(200);
            
            // Move mouse to check postage
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to check postage\r\nMouse Left Move item 'ShippingSettingsDlg.Express1EndiciaPostageBalance50' at 25;8.", repo.ShippingSettingsDlg.Express1EndiciaPostageBalance50Info, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.Express1EndiciaPostageBalance50.MoveTo("25;8");
            Delay.Milliseconds(200);
            
            // Get the postage value and store it in a variable
            Report.Log(ReportLevel.Info, "Get Value", "Get the postage value and store it in a variable\r\nGetting attribute 'RawText' from item 'ShippingSettingsDlg.Express1EndiciaPostageBalance50' and assigning its value to variable 'ExpOneEndiciaAvailablePostage'.", repo.ShippingSettingsDlg.Express1EndiciaPostageBalance50Info, new RecordItemIndex(5));
            ExpOneEndiciaAvailablePostage = repo.ShippingSettingsDlg.Express1EndiciaPostageBalance50.Element.GetAttributeValueText("RawText");
            Delay.Milliseconds(0);
            
            // Move mouse to close button
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to close button\r\nMouse Left Move item 'ShippingSettingsDlg.Close1' at Center.", repo.ShippingSettingsDlg.Close1Info, new RecordItemIndex(6));
            repo.ShippingSettingsDlg.Close1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the close button
            Report.Log(ReportLevel.Info, "Mouse", "Click the close button\r\nMouse Left Click item 'ShippingSettingsDlg.Close1' at Center.", repo.ShippingSettingsDlg.Close1Info, new RecordItemIndex(7));
            repo.ShippingSettingsDlg.Close1.Click();
            Delay.Milliseconds(200);
            
            CheckRates();
            Delay.Milliseconds(0);
            
            // Move mouse to check the shipment rate
            //Report.Log(ReportLevel.Info, "Mouse", "Move mouse to check the shipment rate\r\nMouse Left Move item 'ShipOrders1.SplitContainer.DomesticShipmentRate' at 18;7.", repo.ShipOrders1.SplitContainer.DomesticShipmentRateInfo, new RecordItemIndex(9));
            //repo.ShipOrders1.SplitContainer.DomesticShipmentRate.MoveTo("18;7");
            //Delay.Milliseconds(200);
            
            // Get the shipment rate and store it in a variable
            //Report.Log(ReportLevel.Info, "Get Value", "Get the shipment rate and store it in a variable\r\nGetting attribute 'RawText' from item 'ShipOrders1.SplitContainer.DomesticShipmentRate' and assigning its value to variable 'ExpOneEndiciaShipmentRate'.", repo.ShipOrders1.SplitContainer.DomesticShipmentRateInfo, new RecordItemIndex(10));
            //ExpOneEndiciaShipmentRate = repo.ShipOrders1.SplitContainer.DomesticShipmentRate.Element.GetAttributeValueText("RawText");
            //Delay.Milliseconds(0);
            
            // Compare the available postage with the shipment rate and, if there is enough postage available, process the shipment.
            //CheckPostageBalance();
            //Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
