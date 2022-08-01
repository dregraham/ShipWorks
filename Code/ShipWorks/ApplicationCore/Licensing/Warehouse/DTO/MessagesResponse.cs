using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    /// <summary>
    /// DTO for messages retrieved from Hub
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class MessagesResponse
    {
        public List<MessageDTO> Messages { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class MessageDTO
    {
        public string Message { get; set; }

        public bool RequiresConfirmation { get; set; }
    }
}
