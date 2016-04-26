using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ShipWorks.Stores.Platforms.Odbc
{
    public static class Odbc32
    {
        // API return values from functions (from sql.h)
        public const int SQL_SUCCESS = 0;
        public const int SQL_SUCCESS_WITH_INFO = 1;
        public const int SQL_NO_DATA = 100;
        public const int SQL_ERROR = -1;
        public const int SQL_INVALID_HANDLE = -2;
        public const int SQL_STILL_EXECUTING = 2;
        public const int SQL_NEED_DATA = 99;

        // test for SQL_SUCCESS or SQL_SUCCESS_WITH_INFO
        public static bool SQL_SUCCEEDED(int rc)
        {
            return (rc & (~1)) == 0;
        }

        public enum Direction : short
        {
            SQL_FETCH_NEXT = 1,
            SQL_FETCH_FIRST = 2,
            SQL_FETCH_LAST = 3,
            SQL_FETCH_PRIOR = 4,
            SQL_FETCH_ABSOLUTE = 5,
            SQL_FETCH_RELATIVE = 6,
        }

        public enum HandleType
        {
            SQL_HANDLE_ENV = 1,
            SQL_HANDLE_DBC = 2,
            SQL_HANDLE_STMT = 3,
            SQL_HANDLE_DESC = 4,
        }

        [DllImport("odbc32.dll", CharSet = CharSet.Auto)]
        public extern static short SQLAllocHandle(HandleType handleType, int inputHandle, out IntPtr outputHandle);

        [DllImport("odbc32.dll", CharSet = CharSet.Auto)]
        public static extern short SQLDataSources(
           IntPtr environmentHandle,
           Direction direction,
           StringBuilder serverName,
           short bufferLength1,
           ref short nameLength1Ptr,
           StringBuilder description,
           short bufferLength2,
           ref short nameLength2Ptr);

        [DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
        public static extern short SQLFreeHandle(HandleType handleType, IntPtr inputHandle);

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
