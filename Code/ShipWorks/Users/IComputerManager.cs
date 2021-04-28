using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users
{
    /// <summary>
    /// Manage computers
    /// </summary>
    public interface IComputerManager
    {
        /// <summary>
        /// Returns this computer
        /// </summary>
        ComputerEntity GetComputer();
    }
}