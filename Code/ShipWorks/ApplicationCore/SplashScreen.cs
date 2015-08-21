using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Interapptive.Shared.Utility;
using System.Reflection;
using ShipWorks.Properties;
using ShipWorks.Common.Threading;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Splash screen shown during program startup
    /// </summary>
    partial class SplashScreen : Form
    {
        static SplashScreen splash;

        // Ensures we don't return from the show function until the splash screen is ready
        static ManualResetEvent createdEvent = new ManualResetEvent(false);

        // For sync
        static object splashLock = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        public SplashScreen()
        {
            InitializeComponent();

            Region = new Region(new Rectangle(1, 1, Width - 2, Height - 2));

            if (DateTime.Today == new DateTime(2014, 4, 1))
            {
                BackgroundImage = Resources.splash_april;
                labelReleaseInfo.ForeColor = Color.White;
                status.ForeColor = Color.White;
            }
        }

        /// <summary>
        /// Show the splash screen
        /// </summary>
        public static void ShowSplash()
        {
            if (splash != null)
            {
                throw new InvalidOperationException("An instance of the splash screen has already been created.");
            }

            Thread thread = new Thread(ExceptionMonitor.WrapThread(SplashThread));
            thread.Name = "Splash";
            thread.IsBackground = true;
            thread.Start();
           
            // Don't return from this function until the splash screen is ready.
            createdEvent.WaitOne();
        }

        /// <summary>
        /// The thread on which the splash screen will run
        /// </summary>
        private static void SplashThread()
        {
            // Create the SplashScreen and get it going
            splash = new SplashScreen();

            // Show it
            Application.Run(splash);
        }

        /// <summary>
        /// Called when the splash screen is visible to the user
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoad);

            createdEvent.Set();
        }

        /// <summary>
        /// Ensure the splash screen is closed.  No error if its not currently open.
        /// </summary>
        public static void CloseSplash()
        {
            lock (splashLock)
            {
                if (splash != null)
                {
                    SplashScreen tempSplash = splash;
                    splash = null;

                    tempSplash.BeginInvoke(new MethodInvoker(tempSplash.Close));
                }
            }
        }

        /// <summary>
        /// Special code for altering custom text displayed, such as alpha\beta warnings
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            labelReleaseInfo.Text = string.Format("ShipWorks {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        /// <summary>
        /// The form is being closed
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            lock (splashLock)
            {
                if (splash != null)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Splash is closing, do some cleanup
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            AppDomain.CurrentDomain.AssemblyLoad -= new AssemblyLoadEventHandler(OnAssemblyLoad);
            createdEvent = null;
        }

        /// <summary>
        /// An assembly has been loaded into the AppDomain
        /// </summary>
        void OnAssemblyLoad(object sender, AssemblyLoadEventArgs e)
        {
            Status = string.Format("Loading {0}.dll...", e.LoadedAssembly.GetName().Name);
        }

        /// <summary>
        /// The status text displayed in the 
        /// </summary>
        public static string Status
        {
            get
            {
                lock (splashLock)
                {
                    if (splash == null)
                    {
                        return "";
                    }

                    return splash.status.Text;
                }
            }
            set
            {
                lock (splashLock)
                {
                    if (splash != null)
                    {
                        // Get it on the right thread
                        splash.Invoke(new MethodInvoker(delegate { splash.status.Text = value; }));
                    }
                }
            }
        }
    }
}