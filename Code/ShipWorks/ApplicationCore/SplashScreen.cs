using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using ShipWorks.Common.Threading;
using System.Reflection;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Splash screen shown during program startup
    /// </summary>
    partial class SplashScreen : Form
    {
        static SplashScreen splash;

        /// <summary>
        /// Constructor
        /// </summary>
        public SplashScreen()
        {
            InitializeComponent();

            Region = new Region(new Rectangle(1, 1, Width - 2, Height - 2));
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
        }

        /// <summary>
        /// The thread on which the splash screen will run
        /// </summary>
        private static void SplashThread()
        {
            splash = new SplashScreen();

            Application.Run(splash);
        }

        /// <summary>
        /// Called when the splash screen is visible to the user
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
        }

        /// <summary>
        /// Ensure the splash screen is closed.  No error if its not currently open.
        /// </summary>
        public static void CloseSplash()
        {
            InvokeOnSplashThread(x =>
            {
                splash = null;
                x.Close();
            });
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
            if (splash != null)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Splash is closing, do some cleanup
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            AppDomain.CurrentDomain.AssemblyLoad -= OnAssemblyLoad;
        }

        /// <summary>
        /// An assembly has been loaded into the AppDomain
        /// </summary>
        static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs e)
        {
            Status = string.Format("Loading {0}.dll...", e.LoadedAssembly.GetName().Name);
        }

        /// <summary>
        /// The status text displayed in the 
        /// </summary>
        public static string Status
        {
            set
            {
                InvokeOnSplashThread(x => x.status.Text = value);
            }
        }

        /// <summary>
        /// Invoke the specified method on the splash screen UI thread
        /// </summary>
        private static void InvokeOnSplashThread(Action<SplashScreen> methodToInvoke)
        {
            SplashScreen tempSplash = splash;
            if (tempSplash != null && tempSplash.IsHandleCreated)
            {
                tempSplash.Invoke(methodToInvoke, tempSplash);
            }
        }
    }
}