using System.Collections.Generic;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    /// <summary>
    /// DTO for messages retrieved from Hub
    /// </summary>
    public class MessagesResponse
    {
        public List<MessageDTO> Messages { get; set; }
    }

    public class MessageDTO
    {
        public string Message { get; set; }

        public bool RequiresConfirmation { get; set; }
    }
}
