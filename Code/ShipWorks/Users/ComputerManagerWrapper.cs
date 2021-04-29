using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wrapper for the ComputerManager
    /// </summary>
    [Component]
    public class ComputerManagerWrapper : IComputerManager
    {
        /// <summary>
        /// Gets the current computer
        /// </summary>
        public ComputerEntity GetComputer() => ComputerManager.GetComputer();

        /// <summary>
        /// Gets list of computers
        /// </summary>
        public List<ComputerEntity> GetComputers() => ComputerManager.Computers;
    }
}