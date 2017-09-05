using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Email
{
    /// <summary>
    /// Wrapper around the EmailCommunicator class
    /// </summary>
    [Component]
    public class EmailCommunicatorWrapper : IEmailCommunicatorWrapper
    {
        /// <summary>
        /// Initiate the emailing of the given messages.
        /// </summary>
        public void StartEmailingMessages(IEnumerable<EmailOutboundEntity> messages) =>
            EmailCommunicator.StartEmailingMessages(messages);
    }
}
