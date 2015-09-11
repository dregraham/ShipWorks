﻿using System;
using System.Windows.Forms;
using Interapptive.Shared.Win32;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Stores;
using ShipWorks.Templates;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using log4net;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore;
using ShipWorks.Startup;

namespace ShipWorks.Tests.Integration.MSTest
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
            : this (null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksInitializer"/> class.
        /// </summary>
        /// <param name="executionMode">The instance of execution mode to use when initializing dependencies.</param>
        /// <param name="additionalInitialization">If any additional calls need to be made for initialization, pass them through this Action.</param>
        /// <exception cref="System.Exception">A 'shipworks' account with password 'shipworks' needs to be created.</exception>
        public ShipWorksInitializer(ExecutionMode executionMode, Action additionalInitialization)
        {
            Guid swInstance = GetShipWorksInstance();

            if (ApplicationCore.ShipWorksSession.ComputerID == Guid.Empty)
            {
                ContainerInitializer.Initialize();

                ApplicationCore.ShipWorksSession.Initialize(swInstance);
                SqlSession.Initialize();

                Console.WriteLine(SqlSession.Current.Configuration.DatabaseName);
                Console.WriteLine(SqlSession.Current.Configuration.ServerInstance);

                DataProvider.InitializeForApplication();
                AuditProcessor.InitializeForApplication();

                ShippingSettings.InitializeForCurrentDatabase();
                ShippingProfileManager.InitializeForCurrentSession();
                ShippingDefaultsRuleManager.InitializeForCurrentSession();
                ShippingProviderRuleManager.InitializeForCurrentSession();

                StoreManager.InitializeForCurrentSession();

                UserManager.InitializeForCurrentUser();

                UserSession.InitializeForCurrentDatabase(executionMode);

                if (additionalInitialization != null)
                {
                    additionalInitialization();
                }

                if (!UserSession.Logon("shipworks", "shipworks", true))
                {
                    throw new Exception("A 'shipworks' account with password 'shipworks' needs to be created.");
                }
                ;

                ShippingManager.InitializeForCurrentDatabase();
                LogSession.Initialize();

                TemplateManager.InitializeForCurrentSession();

                ActionManager.InitializeForCurrentSession();
            }
        }

        /// <summary>
        /// Obtains the ShipWorks instance GUID to use when running integration tests.
        /// </summary>
        public static Guid GetShipWorksInstance()
        {
            Guid instance;

            string instanceFromConfig = System.Configuration.ConfigurationManager.AppSettings["ShipWorksInstanceGuid"];
            if (!string.IsNullOrWhiteSpace(instanceFromConfig))
            {
                instance = Guid.Parse(instanceFromConfig);
            }
            else
            {
                // Fall back to the hard-coded values in the case where the instance value is not found in the
                // configuration file
                switch (Environment.MachineName.ToLower())
                {
                    case "tim-pc":
                        instance = Guid.Parse("{2D64FF9F-527F-47EF-BA24-ECBF526431EE}");
                        break;
                    case "john3610-pc":
                        instance = Guid.Parse("{a721d9e4-fb3b-4a64-a612-8579b1251c95}");
                        break;
                    case "kevin-pc":
                        instance = Guid.Parse("{6db3aa02-32bb-430e-95d2-0c59b3b7417a}");
                        break;
                    case "MSTest-vm":
                        instance = Guid.Parse("{3BAE47D1-6903-428B-BD9D-31864E614709}");
                        break;
                    case "benz-pc":
                        instance = Guid.Parse("{a21e0f50-8eb6-469c-8d23-7632c5cdc652}");
                        break;
                    case "mirza-pc":
                        instance = Guid.Parse("{4c5c2b9a-32d3-4314-a4f7-200000000000}");
                        break;
                    default:
                        throw new ApplicationException("Enter your machine and ShipWorks instance GUID in the ShipWorksInitializer");
                }
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