﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Startup;
using ShipWorks.Templates;
using ShipWorks.Users;
using ShipWorks.Users.Audit;

namespace ShipWorks.Tests.Integration.Shared
{
    /// <summary>
    /// Base class for tests to initialize ShipWorks
    /// </summary>
    public class ShipWorksInitializer
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksInitializer));

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksInitializer"/> class.
        /// </summary>
        public ShipWorksInitializer()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksInitializer"/> class.
        /// </summary>
        /// <param name="executionMode">The instance of execution mode to use when initializing dependencies.</param>
        /// <param name="additionalInitialization">If any additional calls need to be made for initialization, pass them through this Action.</param>
        /// <exception cref="System.Exception">A 'ShipWorks' account with password 'ShipWorks' needs to be created.</exception>
        public ShipWorksInitializer(ExecutionMode executionMode, Action additionalInitialization)
        {
            Guid swInstance = GetShipWorksInstance();

            if (ShipWorksSession.ComputerID == Guid.Empty)
            {
                ContainerInitializer.Initialize();

                ShipWorksSession.Initialize(swInstance);
                SqlSession.Initialize();

                Console.WriteLine(SqlSession.Current.Configuration.DatabaseName);
                Console.WriteLine(SqlSession.Current.Configuration.ServerInstance);

                DataProvider.InitializeForApplication();
                AuditProcessor.InitializeForApplication();

                ShippingProfileManager.InitializeForCurrentSession();
                ShippingDefaultsRuleManager.InitializeForCurrentSession();

                foreach (IInitializeForCurrentDatabase service in IoC.UnsafeGlobalLifetimeScope.Resolve<IEnumerable<IInitializeForCurrentDatabase>>())
                {
                    service.InitializeForCurrentDatabase(executionMode);
                }

                UserManager.InitializeForCurrentUser();
                LogSession.Initialize();
                UserSession.InitializeForCurrentDatabase(executionMode);
                UspsAccountManager.InitializeForCurrentSession();

                TemplateManager.InitializeForCurrentSession();

                IEnumerable<IInitializeForCurrentSession> sessionInitializers = IoC.UnsafeGlobalLifetimeScope.Resolve<IEnumerable<IInitializeForCurrentSession>>();
                foreach (IInitializeForCurrentSession service in sessionInitializers)
                {
                    service.InitializeForCurrentSession();
                }

                if (additionalInitialization != null)
                {
                    additionalInitialization();
                }

                if (!UserSession.Logon("shipworks", "shipworks", true))
                {
                    throw new Exception("A 'shipworks' account with password 'shipworks' needs to be created.");
                }

                ShippingManager.InitializeForCurrentDatabase();
                LogSession.Initialize();
            }
        }

        /// <summary>
        /// Obtains the ShipWorks instance GUID to use when running integration tests.
        /// </summary>
        public static Guid GetShipWorksInstance()
        {
            Guid instance;
            switch (Environment.MachineName.ToLower())
            {
                case "tim-pc":
                    instance = Guid.Parse("{2D64FF9F-527F-47EF-BA24-ECBF526431EE}");
                    break;
                case "tim-pc2":
                    instance = Guid.Parse("{a6c0a1b0-4757-4655-b74b-dbb9195b82ff}");
                    break;
                case "john3610-pc":
                    instance = Guid.Parse("{a721d9e4-fb3b-4a64-a612-8579b1251c95}");
                    break;
                case "kevin":
                    instance = Guid.Parse("{10000000-0000-0000-0000-000000000000}");
                    break;
                case "MSTest-vm":
                    instance = Guid.Parse("{3BAE47D1-6903-428B-BD9D-31864E614709}");
                    break;
                case "benz-pc3":
                    instance = Guid.Parse("{A74AED9C-0AB8-4649-B233-8DFBE774D9F8}");
                    break;
                case "mirza-pc2":
                    instance = Guid.Parse("{1231F4A9-640C-4E08-A52A-AE3B2C2FB864}");
                    break;
                case "berger-pc":
                    instance = Guid.Parse("{AABB7285-a889-46af-87b8-69c10cdbAABB}");
                    break;
                case "hicks-pc":
                    instance = Guid.Parse("{2599E819-28C0-4CA1-8810-7A8D3F1FC865}");
                    break;
                default:
                    // If instance not specified for pc, look for instance guid from AppSettings.
                    string instanceFromConfig = System.Configuration.ConfigurationManager.AppSettings["ShipWorksInstanceGuid"];
                    if (!string.IsNullOrWhiteSpace(instanceFromConfig))
                    {
                        instance = Guid.Parse(instanceFromConfig);
                    }
                    else
                    {
                        throw new ApplicationException("Enter your machine and ShipWorks instance GUID in the ShipWorksInitializer");
                    }
                    break;
            }

            return instance;
        }

        /// <summary>
        /// Determines if a special key combination is active.  Can be used
        /// for enabling "hidden" (but not secure!) functionality.
        /// </summary>
        public static bool MagicKeysDown
        {
            get
            {
                return Control.ModifierKeys == (Keys.Control | Keys.Shift) &&
                    (NativeMethods.GetAsyncKeyState(Keys.LWin) & 0x8000) != 0;
            }
        }

        /// <summary>
        /// Determines if a special key combination is active.  Can be used
        /// for enabling "hidden" (but not secure!) functionality.
        /// </summary>
        public static bool DebugKeysDown
        {
            get
            {
                return Control.ModifierKeys == (Keys.Control | Keys.Shift | Keys.Alt) &&
                    (NativeMethods.GetAsyncKeyState(Keys.LWin) & 0x8000) != 0;
            }
        }

        /// <summary>
        /// Gets the next.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns></returns>
        private DateTime GetNext(DateTime from, DayOfWeek dayOfWeek)
        {
            DateTime date = new DateTime(from.Ticks);

            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1);
            }

            return date;
        }
    }
}
