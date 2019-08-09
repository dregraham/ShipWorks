using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users
{
    /// <summary>
    /// Wraps the user manager for better IOC.
    /// </summary>
    [Component]
    public class UserManagerWrapper : IUserManager
    {
        /// <summary>
        /// Get the user entity with the given ID, or null if it does not exist
        /// </summary>
        public UserEntity GetUser(long userID) => 
            UserManager.GetUser(userID);
    }
}
