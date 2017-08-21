using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// Lighter analog to StackFrame
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    [DebuggerDisplay("{LongHash} : {ToString()}")]
    public struct StackFrameSlim : IEquatable<StackFrameSlim>
    {
        internal const char FirstArrow = (char) 0x21e6;
        internal const char SecondArrow = (char) 0x2190;
        public static readonly StackFrameSlim AsyncBoundary = new StackFrameSlim(new IntPtr(-1L), -1, -1, null, -1, -1, false);
        private static readonly String asyncBoundaryString = ((char) 0x231B).ToString() + " " + string.Join(" ", Enumerable.Range(0, 10).Select(x => (char) 0xB7));

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
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

        /// <summary>
        /// Method
        /// </summary>
        internal MethodBaseSlim Method { get; set; }

        /// <summary>
        /// Handle to the method
        /// </summary>
        public IntPtr MethodHandle { get; }

        /// <summary>
        /// Offset
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// IL Offset
        /// </summary>
        public int ILOffset { get; }

        /// <summary>
        /// Filename
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Line number
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// Column number
        /// </summary>
        public int ColumnNumber { get; }

        /// <summary>
        /// Is the last frame from foreign exception
        /// </summary>
        public bool IsLastFrameFromForeignExceptionStackTrace { get; }

        /// <summary>
        /// To string
        /// </summary>
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

        /// <summary>
        /// Get hash code
        /// </summary>
        public override int GetHashCode()
        {
            var hash = LongHash;

            return (int) (hash ^ (hash >> 32));
        }

        /// <summary>
        /// Equality method
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(StackFrameSlim))
            {
                return false;
            }

            return Equals((StackFrameSlim) obj);
        }

        /// <summary>
        /// Equality method
        /// </summary>
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