using System;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Network Exception
    /// </summary>
    public class NetworkException : Exception
    {
        public NetworkException(string message)
            : base(message)
        {}
    }
}