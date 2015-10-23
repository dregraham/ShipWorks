using System;

namespace ShipWorks.Stores.Platforms.Yahoo
{
    /// <summary>
    /// Base for exceptions thrown and known by yahoo
    /// </summary>
    public class YahooException : Exception
    {
        public YahooException()
        {

        }

        public YahooException(string message)
            : base(message)
        {

        }

        public YahooException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
