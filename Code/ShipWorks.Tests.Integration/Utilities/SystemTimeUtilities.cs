using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ShipWorks.Tests.Integration.MSTest.Utilities
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;
    }

    /// <summary>
    /// Utilities for manipulating system time.
    /// </summary>
    public static class SystemTimeUtilities
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetSystemTime(ref SYSTEMTIME st);

        /// <summary>
        /// Set system time to given DateTime
        /// </summary>
        public static void UpdateSystemTime(DateTime dateTime)
        {
            DateTime dateTimeUtc = dateTime.ToUniversalTime();
            SYSTEMTIME st = new SYSTEMTIME();
            st.wYear = (short)dateTimeUtc.Year;
            st.wMonth = (short)dateTimeUtc.Month;
            st.wDay = (short)dateTimeUtc.Day;
            st.wHour = (short)dateTimeUtc.Hour;
            st.wMinute = (short)dateTimeUtc.Minute;
            st.wSecond = (short)dateTimeUtc.Second;

            SetSystemTime(ref st);
        }
    }
}
