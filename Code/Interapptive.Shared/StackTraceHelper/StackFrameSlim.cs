using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WindowsFormsApp1.StackTraceHelper
{
    /// <summary>
    /// Lighter analog to StackFrame
    /// </summary>
    [DebuggerDisplay("{LongHash} : {ToString()}")]
    public struct StackFrameSlim : IEquatable<StackFrameSlim>
    {
        internal const char FirstArrow = (char) 0x21e6;
        internal const char SecondArrow = (char) 0x2190;

        public static readonly StackFrameSlim AsyncBoundary = new StackFrameSlim(new IntPtr(-1L), -1, -1, null, -1, -1, false);

        private static readonly String asyncBoundaryString = ((char) 0x231B).ToString() + " " + string.Join(" ", Enumerable.Range(0, 10).Select(x => (char) 0xB7));


        public StackFrameSlim(IntPtr rgMethodHandle, int rgiOffset, int rgiILOffset,
            string rgFilename, int rgiLineNumber, int rgiColumnNumber, bool rgiLastFrameFromForeignExceptionStackTrace)
        {
            Method = default(MethodBaseSlim);

            MethodHandle = rgMethodHandle;
            Offset = rgiOffset;

            ILOffset = rgiILOffset;

            FileName = rgFilename;

            LineNumber = rgiLineNumber;

            ColumnNumber = rgiColumnNumber;

            IsLastFrameFromForeignExceptionStackTrace = rgiLastFrameFromForeignExceptionStackTrace;

        }


        internal MethodBaseSlim Method { get; set; }


        public IntPtr MethodHandle { get; }
        public int Offset { get; }

        public int ILOffset { get; }
        public string FileName { get; }

        public int LineNumber { get; }
        public int ColumnNumber { get; }

        public bool IsLastFrameFromForeignExceptionStackTrace { get; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (Equals(AsyncBoundary))
            {
                builder.Append(asyncBoundaryString);
            }
            else
            {
                builder.Append(Method.StringValue ?? "<UnknownFrame>");

                if (!String.IsNullOrEmpty(FileName) || LineNumber != 0 || ColumnNumber != 0)
                {
                    builder.AppendFormat(" {3} {0}:line {1}:column {2}", FileName, LineNumber, ColumnNumber, FirstArrow);

                }

            }

            return builder.ToString();
        }

        /// <summary>
        /// Hashes are long to reduce collisions when relying on hash comparisons between stack trace segments
        /// </summary>
        public long LongHash
        {
            get
            {
                return MethodHandle.ToInt64() ^
                    (ILOffset * 76319L) ^
                    (Offset * 1743920413L) ^
                    (FileName == null ? 0 : (FileName.GetHashCode() * 9432173L)) ^
                    (LineNumber * 18472183L) ^
                    (ColumnNumber * 29428193823821L) ^
                    (IsLastFrameFromForeignExceptionStackTrace ? 0x400000 : 0);
            }
        }



        public override int GetHashCode()
        {
            var hash = LongHash;

            return (int) (hash ^ (hash >> 32));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(StackFrameSlim))
            {
                return false;
            }

            return Equals((StackFrameSlim) obj);
        }

        public bool Equals(StackFrameSlim other)
        {
            return MethodHandle == other.MethodHandle &&
                Offset == other.Offset &&
                ILOffset == other.ILOffset &&
                FileName == other.FileName &&
                LineNumber == other.LineNumber &&
                ColumnNumber == other.ColumnNumber &&
                IsLastFrameFromForeignExceptionStackTrace == other.IsLastFrameFromForeignExceptionStackTrace;
        }

        /// <summary>
        /// Checks if the frame should be visible against justMyCode filter
        /// </summary>
        /// <param name="justMyCode">Hide framework boilerplate</param>
        /// <returns>Whether frame needs to be hidden</returns>
        public bool IsHidden(bool justMyCode)
        {
            return (!Equals(AsyncBoundary)) && (Method.IsEventInfrastructure || (justMyCode && Method.IsExternalCode));
        }
    }
}