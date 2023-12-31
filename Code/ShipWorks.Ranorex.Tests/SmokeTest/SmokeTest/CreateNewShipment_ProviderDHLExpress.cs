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
    ///The CreateNewShipment_ProviderDHLExpress recording.
    /// </summary>
    [TestModule("991c6364-22a7-4dd1-8a8c-c301919984d1", ModuleType.Recording, 1)]
    public partial class CreateNewShipment_ProviderDHLExpress : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static CreateNewShipment_ProviderDHLExpress instance = new CreateNewShipment_ProviderDHLExpress();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CreateNewShipment_ProviderDHLExpress()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static CreateNewShipment_ProviderDHLExpress Instance
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

            // Move mouse to avoid click inconsistency.
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to avoid click inconsistency.\r\nMouse Left Move item 'ShippingDlg.SplitContainer.CreateNew' at Center.", repo.ShippingDlg.SplitContainer.CreateNewInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.CreateNew.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Create New to create new shipment
            Report.Log(ReportLevel.Info, "Mouse", "Click Create New to create new shipment\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CreateNew' at Center.", repo.ShippingDlg.SplitContainer.CreateNewInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.CreateNew.Click();
            Delay.Milliseconds(200);
            
            // Move mouse to avoid click inconsistency.
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to avoid click inconsistency.\r\nMouse Left Move item 'ShippingDlg.SplitContainer.ComboShipmentType' at Center.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.ComboShipmentType.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.SplitContainer.ComboShipmentType' at Center.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(3));
            repo.ShippingDlg.SplitContainer.ComboShipmentType.Click();
            Delay.Milliseconds(200);
            
            // Set Provider: DHL Express
            //Report.Log(ReportLevel.Info, "Set value", "Set Provider: DHL Express\r\nSetting attribute SelectedItemText to 'DHL Express' on item 'ShippingDlg.SplitContainer.ComboShipmentType'.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(4));
            //repo.ShippingDlg.SplitContainer.ComboShipmentType.Element.SetAttributeValue("SelectedItemText", "DHL Express");
            //Delay.Milliseconds(0);
            
            // USPS (Express1)
            Report.Log(ReportLevel.Info, "Keyboard", "USPS (Express1)\r\nKey sequence '{Down}'.", new RecordItemIndex(5));
            Keyboard.Press("{Down}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 5s.", new RecordItemIndex(6));
            Delay.Duration(5000, false);
            
            // FedEx
            Report.Log(ReportLevel.Info, "Keyboard", "FedEx\r\nKey sequence '{Down}'.", new RecordItemIndex(7));
            Keyboard.Press("{Down}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 5s.", new RecordItemIndex(8));
            Delay.Duration(5000, false);
            
            // UPS
            Report.Log(ReportLevel.Info, "Keyboard", "UPS\r\nKey sequence '{Down}'.", new RecordItemIndex(9));
            Keyboard.Press("{Down}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 5s.", new RecordItemIndex(10));
            Delay.Duration(5000, false);
            
            // UPS (WorldShip)
            Report.Log(ReportLevel.Info, "Keyboard", "UPS (WorldShip)\r\nKey sequence '{Down}'.", new RecordItemIndex(11));
            Keyboard.Press("{Down}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 5s.", new RecordItemIndex(12));
            Delay.Duration(5000, false);
            
            // USPS (Endiciaj)
            Report.Log(ReportLevel.Info, "Keyboard", "USPS (Endiciaj)\r\nKey sequence '{Down}'.", new RecordItemIndex(13));
            Keyboard.Press("{Down}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 5s.", new RecordItemIndex(14));
            Delay.Duration(5000, false);
            
            // USPS (Express1 for Endicia)
            Report.Log(ReportLevel.Info, "Keyboard", "USPS (Express1 for Endicia)\r\nKey sequence '{Down}'.", new RecordItemIndex(15));
            Keyboard.Press("{Down}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 5s.", new RecordItemIndex(16));
            Delay.Duration(5000, false);
            
            // OnTrac
            Report.Log(ReportLevel.Info, "Keyboard", "OnTrac\r\nKey sequence '{Down}'.", new RecordItemIndex(17));
            Keyboard.Press("{Down}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 5s.", new RecordItemIndex(18));
            Delay.Duration(5000, false);
            
            // DHL Express
            Report.Log(ReportLevel.Info, "Keyboard", "DHL Express\r\nKey sequence '{Down}'.", new RecordItemIndex(19));
            Keyboard.Press("{Down}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Return}'.", new RecordItemIndex(20));
            Keyboard.Press("{Return}");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
