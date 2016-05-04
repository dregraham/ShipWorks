using Interapptive.Shared.Security;
using System;

namespace ShipWorks.Stores.Platforms.Sears
{
    public class SearsAesParams : IAesParams
    {
        public byte[] InitializationVector { get; } = {84, 104, 101, 68, 111, 111, 115, 107, 101, 114, 110, 111, 111, 100, 108, 101};

        public byte[] Key { get; } = new Guid("{A2FC95D9-F255-4D23-B86C-756889A51C6A}").ToByteArray();

        public string EmptyValue { get; }
    }
}