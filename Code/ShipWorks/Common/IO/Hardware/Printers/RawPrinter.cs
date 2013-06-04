using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using ShipWorks.Templates.Printing;
using System.Drawing.Printing;
using System.ComponentModel;

namespace ShipWorks.Common.IO.Hardware.Printers
{
    /// <summary>
    /// Enables one to send raw data directly to a named printer.
    /// </summary>
    public sealed class RawPrinter
    {
        string printerName;

        /// <summary>
        /// Constructor.  Takes the name of the printer we want to print to
        /// </summary>
        public RawPrinter(string printerName)
        {
            this.printerName = printerName;
        }

        /// <summary>
        /// When the function is given an unmanaged array
        /// of bytes, the function sends those bytes to the print queue.
        /// Returns true on success, false on failure.
        /// </summary>
        private void SendBytesToPrinter(string jobName, IntPtr unmanagedBytes, int length)
        {
            // The selected printer could not be found
            if (!PrintUtility.InstalledPrinters.Contains(printerName))
            {
                throw new PrintingException(
                    "The thermal printer \"" + printerName + "\" could not be found.");
            }

            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA docInfo = new DOCINFOA();
            bool success = false;

            docInfo.pDocName = jobName;
            docInfo.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(printerName, out hPrinter, 0))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, docInfo))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        int written = 0;
                        success = WritePrinter(hPrinter, unmanagedBytes, length, out written);

                        // End the page
                        EndPagePrinter(hPrinter);
                    }

                    // End the document
                    EndDocPrinter(hPrinter);
                }

                // Cose the printer
                ClosePrinter(hPrinter);
            }

            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                Win32Exception winEx = new Win32Exception(error);

                throw new PrintingException(string.Format(
                    "An error occurred while printing.\n\n" +
                    "Message: {0}\n" +
                    "Code: {1}", winEx.Message, error));
            }
        }

        /// <summary>
        /// When the function is given an array
        /// of bytes, the function sends those bytes to the print queue.
        /// Returns true on success, false on failure.
        /// </summary>
        public void SendBytesToPrinter(string jobName, byte[] bytes)
        {
            // Convert to unmanaged data
            int length = (int) bytes.Length;
            IntPtr unmanagedBytes = Marshal.AllocCoTaskMem(length);

            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, unmanagedBytes, length);

            try
            {
                SendBytesToPrinter(jobName, unmanagedBytes, length);
            }
            finally
            {
                // Free the unmanaged memory that you allocated earlier.
                Marshal.FreeCoTaskMem(unmanagedBytes);
            }
        }

        /// <summary>
        /// Send the bytes from the given file directly to the printer
        /// </summary>
        public void SendFileToPrinter(string jobName, string fileName)
        {
            byte[] bytes = null;

            // Open the file.
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                // Create a BinaryReader on the file.
                BinaryReader reader = new BinaryReader(stream);

                // Read the contents of the file into the array.
                bytes = reader.ReadBytes((int) stream.Length);
            }

            // Send the bytes to the printer.
            SendBytesToPrinter(jobName, bytes);
        }

        /// <summary>
        /// Send the bytes from the given string to the printer
        /// </summary>
        public void SendStringToPrinter(string jobName, string data)
        {
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            IntPtr unmanagedBytes = Marshal.StringToCoTaskMemAnsi(data);

            // Send the converted ANSI string to the printer.
            try
            {
                SendBytesToPrinter(jobName, unmanagedBytes, data.Length);
            }
            finally
            {
                Marshal.FreeCoTaskMem(unmanagedBytes);
            }
        }

        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName = null;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile = null;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType = null;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, long pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);
    }
}
