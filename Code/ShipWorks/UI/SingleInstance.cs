using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.ApplicationCore;

namespace ShipWorks.UI
{
    /// <summary>
    /// Provides functionality for ShipWorks to only have a single instance open at a time.
    /// </summary>
    static class SingleInstance
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SingleInstance));

        // Private message ID to which the instance should respond
        static int windowsMessageID;

        // Private message for restoring the window
        static int singleInstanceActivateMessageID;

        // The mutex we use to see if more than one instance is running
        static string mutexName;
        static Mutex instanceMutex;

        // Indicates if the application is already running
        static bool isAlreadyRunning;

        /// <summary>
        /// Gets the mutex name, which is based on the instance ID.
        /// </summary>
        private static string MutexName
        {
            get
            {
                return mutexName ?? (mutexName = ShipWorksSession.InstanceID.ToString("B"));
            }
        }

        /// <summary>
        /// Register an application with the given identifier
        /// </summary>
        public static void Register()
        {
            // Register the private window messages
            windowsMessageID = NativeMethods.RegisterWindowMessage(MutexName);
            singleInstanceActivateMessageID = NativeMethods.RegisterWindowMessage(MutexName + "_Restore");

            bool createdNew;
            instanceMutex = new Mutex(false, MutexName, out createdNew);

            isAlreadyRunning = !createdNew;
        }

        /// <summary>
        /// Unregister the application from single instance mode
        /// </summary>
        public static void Unregister()
        {
            instanceMutex?.Dispose();
        }

        /// <summary>
        /// Indicates if the application is already running.
        /// </summary>
        public static bool IsAlreadyRunning
        {
            get
            {
                if (instanceMutex != null)
                {
                    return isAlreadyRunning;
                }

                try
                {
                    using (Mutex.OpenExisting(MutexName))
                    {
                        return true;
                    }
                }
                catch (WaitHandleCannotBeOpenedException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// The private windows message ID that is used for cross-process communication.
        /// </summary>
        public static int WindowsMessageID
        {
            get
            {
                if (instanceMutex == null)
                {
                    throw new InvalidOperationException("SingleInstance.Register has not yet been called.");
                }

                return windowsMessageID;
            }
        }

        /// <summary>
        /// The private windows message ID that is used to notify another process to activate itself as the single instance
        /// </summary>
        public static int SingleInstanceActivateMessageID
        {
            get
            {
                if (instanceMutex == null)
                {
                    throw new InvalidOperationException("SingleInstance.Register has not yet been called.");
                }

                return singleInstanceActivateMessageID;
            }
        }

        /// <summary>
        /// If there is already an instance of this application running, this will bring it to the foreground.
        /// </summary>
        public static bool ActivateRunningInstance()
        {
            if (IsAlreadyRunning)
            {
                log.Info("ShipWorks is already running.");

                // Need it as an object to pass by ref
                object data = IntPtr.Zero;

                // Try to find the running instance
                int result = NativeMethods.EnumWindows(new NativeMethods.EnumWindowsCallback(SingleInstance.WindowSearcher), ref data);

                if (result == 0)
                {
                    log.InfoFormat("EnumWindows GLE: {0}", Marshal.GetLastWin32Error());
                }

                IntPtr hWnd = (IntPtr) (int) data;

                // We found it, bring it to the foreground.
                if (IntPtr.Zero != hWnd)
                {
                    log.DebugFormat("Setting existing window to foreground: '{0}'.", hWnd);

                    NativeMethods.SetForegroundWindow(hWnd);
                    NativeMethods.SendMessage(hWnd, singleInstanceActivateMessageID, IntPtr.Zero, IntPtr.Zero);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Callback to pass to EnumWindows.  Looks for another version of the appication
        /// running, and if it finds one sets the window handle in lParam.
        /// </summary>
        private static bool WindowSearcher(IntPtr hWnd, ref object lParam)
        {
            // Get an integer into an object to send through the marshaler
            IntPtr dwResult = IntPtr.Zero;

            IntPtr lResult = NativeMethods.SendMessageTimeout(
                hWnd,
                windowsMessageID,
                IntPtr.Zero, IntPtr.Zero,
                (NativeMethods.SMTO_BLOCK | NativeMethods.SMTO_ABORTIFHUNG),
                200,
                ref dwResult);

            // Ignore this and continue
            if (lResult == IntPtr.Zero)
            {
                return true;
            }

            // Found it, stop the search
            if ((int) dwResult == windowsMessageID)
            {
                lParam = (object) hWnd;

                // Stop search
                return false;
            }

            // Continue search
            return true;
        }

        /// <summary>
        /// Handles the WndProc of the main window to respond to single instance communication.
        /// </summary>
        internal static bool HandleMainWndProc(ref Message msg)
        {
            // If this is another instance trying to make ask us who we are,
            // respond positively.
            if (msg.Msg == WindowsMessageID)
            {
                msg.Result = (IntPtr) WindowsMessageID;
                return true;
            }

            return false;
        }
    }
}
