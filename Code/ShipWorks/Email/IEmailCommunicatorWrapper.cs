using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Email
{
    /// <summary>
    /// Wrapper around the EmailCommunicator class
    /// </summary>
    public interface IEmailCommunicatorWrapper
    {
        /// <summary>
        /// Initiate the emailing of the given messages.
        /// </summary>
        void StartEmailingMessages(IEnumerable<EmailOutboundEntity> messages);
    }
}