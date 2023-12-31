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

namespace ShippingCarrierRegression
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The ShipOrders_FromAddress_Canada_UPS recording.
    /// </summary>
    [TestModule("6bbaf34e-c2e6-4a7f-9018-7f061467b8f9", ModuleType.Recording, 1)]
    public partial class ShipOrders_FromAddress_Canada_UPS : ITestModule
    {
        /// <summary>
        /// Holds an instance of the ShippingCarrierRegressionRepository repository.
        /// </summary>
        public static ShippingCarrierRegressionRepository repo = ShippingCarrierRegressionRepository.Instance;

        static ShipOrders_FromAddress_Canada_UPS instance = new ShipOrders_FromAddress_Canada_UPS();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ShipOrders_FromAddress_Canada_UPS()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ShipOrders_FromAddress_Canada_UPS Instance
        {
            get { return instance; }
        }

#region Variables

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "7.0")]
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
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "7.0")]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            // Move to the UPS Account Dropdown
            Report.Log(ReportLevel.Info, "Mouse", "Move to the UPS Account Dropdown\r\nMouse Left Move item 'ShippingDlg.SplitContainer.UpsAccount' at Center.", repo.ShippingDlg.SplitContainer.UpsAccountInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.UpsAccount.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Give Focus to the UPS Account Drowdown
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus to the UPS Account Drowdown\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.UpsAccount'.", repo.ShippingDlg.SplitContainer.UpsAccountInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.UpsAccount.Focus();
            Delay.Milliseconds(0);
            
            // Select the Canadian UPS Account
            Report.Log(ReportLevel.Info, "Set Value", "Select the Canadian UPS Account\r\nSetting attribute SelectedItemText to '268Y23, 236 Wellington Street, K1A0G9' on item 'ShippingDlg.SplitContainer.UpsAccount'.", repo.ShippingDlg.SplitContainer.UpsAccountInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.UpsAccount.Element.SetAttributeValue("SelectedItemText", "268Y23, 236 Wellington Street, K1A0G9");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
