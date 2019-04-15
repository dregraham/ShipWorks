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

namespace OrderCreation
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The LogInCA recording.
    /// </summary>
    [TestModule("12befaf9-7613-40af-a113-1d5337b62b49", ModuleType.Recording, 1)]
    public partial class LogInCA : ITestModule
    {
        /// <summary>
        /// Holds an instance of the OrderCreationRepository repository.
        /// </summary>
        public static OrderCreationRepository repo = OrderCreationRepository.Instance;

        static LogInCA instance = new LogInCA();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public LogInCA()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static LogInCA Instance
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

            Report.Log(ReportLevel.Info, "Website", "Opening web site 'https://complete.channeladvisor.com/Orders/AllOrders.mvc/List?apid=12000997&filter=w9OrH7Z8cgHDKdgBcTyMw2IaNeg' with browser 'firefox' in normal mode.", new RecordItemIndex(0));
            Host.Current.OpenBrowser("https://complete.channeladvisor.com/Orders/AllOrders.mvc/List?apid=12000997&filter=w9OrH7Z8cgHDKdgBcTyMw2IaNeg", "firefox", "", false, false, false, false, false);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'LogInMozillaFirefox.Grouping.Email' at 95;16.", repo.LogInMozillaFirefox.Grouping.EmailInfo, new RecordItemIndex(1));
            repo.LogInMozillaFirefox.Grouping.Email.Click("95;16");
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'richard{LShiftKey down}@{LShiftKey up}shipworks.com' with focus on 'LogInMozillaFirefox.Grouping.Email'.", repo.LogInMozillaFirefox.Grouping.EmailInfo, new RecordItemIndex(2));
            repo.LogInMozillaFirefox.Grouping.Email.PressKeys("richard{LShiftKey down}@{LShiftKey up}shipworks.com");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Tab}Channel7458-4' with focus on 'LogInMozillaFirefox.Grouping.Email'.", repo.LogInMozillaFirefox.Grouping.EmailInfo, new RecordItemIndex(3));
            repo.LogInMozillaFirefox.Grouping.Email.PressKeys("{Tab}Channel7458-4");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'LogInMozillaFirefox.Grouping.LogIn' at 111;23.", repo.LogInMozillaFirefox.Grouping.LogInInfo, new RecordItemIndex(4));
            repo.LogInMozillaFirefox.Grouping.LogIn.Click("111;23");
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Escape}'.", new RecordItemIndex(5));
            Keyboard.Press("{Escape}");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
