using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shared.Database
{
    /// <summary>
    /// Context that is used when running a data driven test
    /// </summary>
    public class DataContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataContext(UserEntity user, ComputerEntity computer)
        {
            User = user;
            Computer = computer;
        }

        /// <summary>
        /// Current user entity
        /// </summary>
        public UserEntity User { get; }

        /// <summary>
        /// Current computer entity
        /// </summary>
        public ComputerEntity Computer { get; }
    }
}
