namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Launch ShipWorks
    /// </summary>
    public interface IShipWorksLauncher
    {
        /// <summary>
        /// Start ShipWorks for the last active windows user
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to start the shipworks UI as a windows user, this
        /// can be called from the escalator service which is running as local system
        /// </remarks>
        void StartShipWorks();
    }
}