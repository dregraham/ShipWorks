using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Utility class for enabling Console output within a Windows Forms application
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Initialize usage of the Console, preserving redirection
        /// </summary>
        public static void AttachToConsole()
        {
            SafeFileHandle hStdOut = NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE);
            SafeFileHandle hStdErr= NativeMethods.GetStdHandle(NativeMethods.STD_ERROR_HANDLE);

            // Get current process handle
            IntPtr hProcess = Process.GetCurrentProcess().Handle;

            // Duplicate Stdout handle to save initial value
            SafeFileHandle hStdOutCopy;
            NativeMethods.DuplicateHandle(hProcess, hStdOut, hProcess, out hStdOutCopy, 0, true, NativeMethods.DUPLICATE_SAME_ACCESS);

            // Duplicate Stderr handle to save initial value
            SafeFileHandle hStdErrCopy;
            NativeMethods.DuplicateHandle(hProcess, hStdErr, hProcess, out hStdErrCopy, 0, true, NativeMethods.DUPLICATE_SAME_ACCESS);

            // Attach to console window – this may modify the standard handles
            NativeMethods.AttachConsole(NativeMethods.ATTACH_PARENT_PROCESS);

            NativeMethods.BY_HANDLE_FILE_INFORMATION bhfi;

            // Adjust the standard handles
            if (NativeMethods.GetFileInformationByHandle(NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE), out bhfi))
            {
                NativeMethods.SetStdHandle(NativeMethods.STD_OUTPUT_HANDLE, hStdOutCopy);
            }
            else
            {
                NativeMethods.SetStdHandle(NativeMethods.STD_OUTPUT_HANDLE, hStdOut);
            }

            // Adjust STDERR if necessary
            if (NativeMethods.GetFileInformationByHandle(NativeMethods.GetStdHandle(NativeMethods.STD_ERROR_HANDLE), out bhfi))
            {
                NativeMethods.SetStdHandle(NativeMethods.STD_ERROR_HANDLE, hStdErrCopy);
            }
            else
            {
                NativeMethods.SetStdHandle(NativeMethods.STD_ERROR_HANDLE, hStdErr);
            }
        }
    }
}
