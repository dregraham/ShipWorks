using System;
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

namespace ShipWorks.Tests.Integration.MSTest
{
    /// <summary>
    /// Base class for tests to initialize ShipWorks
    /// </summary>
    public abstract class ShipWorksInitializer
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksInitializer));

        /// <summary>
        /// Constructor
        /// </summary>
        protected ShipWorksInitializer()
        {
            Guid swInstance;
            switch (Environment.MachineName.ToLower())
            {
                case "tim-pc":
                    swInstance = Guid.Parse("{2D64FF9F-527F-47EF-BA24-ECBF526431EE}");
                    break;
                case "john-pc":
                    swInstance = Guid.Parse("{00000000-143F-4C2B-A80F-5CF0E121A909}");
                    break;
                case "kevin-pc":
                    swInstance = Guid.Parse("{0BDCFB64-15FC-4BA3-84BC-83E8A6D0455A}");
                    break;
                case "MSTest-vm":
                    swInstance = Guid.Parse("{3BAE47D1-6903-428B-BD9D-31864E614709}");
                    break;
                default:
                    throw new ApplicationException("Enter your machine and ShipWorks instance guid in iParcelPrototypeFixture()");
            }

            if (ApplicationCore.ShipWorksSession.ComputerID == Guid.Empty)
            {

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

                UserSession.InitializeForCurrentDatabase();

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
