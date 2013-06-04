﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Base class for all exceptions that we want to handle from MarketplaceAdvisor
    /// </summary>
    class MarketplaceAdvisorException : Exception
    {
        public MarketplaceAdvisorException()
        {

        }

        public MarketplaceAdvisorException(string message)
            : this(message, null)
        {

        }

        public MarketplaceAdvisorException(string message, Exception inner)
            : base(ProcessMessage(message), inner)
        {

        }

        /// <summary>
        /// Process the given message text since MarketplaceAdvisor returns sometimes stupidly long errors
        /// </summary>
        private static string ProcessMessage(string message)
        {
            int length;

            do
            {
                length = message.Length;

                // Remove exception names
                message = Regex.Replace(message, @"[a-zA-Z\.]+Exception:[ ]", "");
            }

            // Keep going as long as there were changes
            while (length != message.Length);

            int traceIndex = message.IndexOf(" at Market");
            if (traceIndex != -1)
            {
                message = message.Substring(0, traceIndex);
            }

            message = message.Trim();

            return message;
        }
    }
}
