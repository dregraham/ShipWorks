using System.Reflection;

namespace ShipWorks.OrderLookup.Controls.EmailNotifications
{
    /// <summary>
    /// View model for the email notification controls
    /// </summary>
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public interface IEmailNotificationsViewModel : IOrderLookupViewModel
    {
    }
}