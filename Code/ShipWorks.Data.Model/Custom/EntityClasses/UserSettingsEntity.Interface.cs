using ShipWorks.Shared.Users;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Custom code for the UserSettingsEntity
    /// </summary>
    public partial interface IUserSettingsEntity
    {
        /// <summary>
        /// Get an object for the DialogSettings XML
        /// </summary>
        DialogSettings DialogSettingsObject { get; }
    }
}
