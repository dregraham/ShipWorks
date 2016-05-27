using Interapptive.Shared;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Class for interacting with the odbc32.dll
    /// </summary>
    public static class Odbc32
    {
        // API return values from functions (from sql.h)
        public const int SqlSuccess = 0;
        public const int SqlSuccessWithInfo = 1;
        public const int SqlNoData = 100;
        public const int SqlError = -1;
        public const int SqlInvalidHandle = -2;
        public const int SqlStillExecuting = 2;
        public const int SqlNeedData = 99;

        // test for SqlSuccess or SqlSuccessWithInfo
        public static bool SqlSucceeded(int rc)
        {
            return (rc & (~1)) == 0;
        }

        /// <summary>
        /// Sql fetch direction
        /// </summary>
        public enum Direction : short
        {
            SqlFetchNext = 1,
            SqlFetchFirst = 2,
            SqlFetchLast = 3,
            SqlFetchPrior = 4,
            SqlFetchAbsolute = 5,
            SqlFetchRelative = 6,
        }

        /// <summary>
        /// Sql handle type
        /// </summary>
        public enum HandleType
        {
            SqlHandleEnv = 1,
            SqlHandleDbc = 2,
            SqlHandleStmt = 3,
            SqlHandleDesc = 4,
        }

        /// <summary>
        /// The Sql alloc handle.
        /// </summary>
        [DllImport("odbc32.dll", CharSet = CharSet.Auto)]
        public extern static short SQLAllocHandle(HandleType handleType, int inputHandle, out IntPtr outputHandle);

        /// <summary>
        /// Sql data sources
        /// </summary>
        [DllImport("odbc32.dll", CharSet = CharSet.Auto)]
        [NDependIgnoreTooManyParams]
#pragma warning disable S107 // Methods should not have too many parameters
        public static extern short SQLDataSources(
           IntPtr environmentHandle,
           Direction direction,
           StringBuilder serverName,
           short bufferLength1,
           ref short nameLength1Ptr,
           StringBuilder description,
           short bufferLength2,
           ref short nameLength2Ptr);
#pragma warning restore S107 // Methods should not have too many parameters

        /// <summary>
        /// Free SQL handle
        /// </summary>
        [DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
        public static extern short SQLFreeHandle(HandleType handleType, IntPtr inputHandle);

        /// <summary>
        /// Sets SQL env attr
        /// </summary>
        [DllImport("odbc32.dll", SetLastError = true)]
        public static extern short SQLSetEnvAttr(
            IntPtr envHandle,
#pragma warning disable CS3001 // Argument type is not CLS-compliant
            // Needed for pInvoke
            ushort attribute,
#pragma warning restore CS3001 // Argument type is not CLS-compliant
            IntPtr attrValue,
            int stringLength);
    }
}
