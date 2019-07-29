using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users
{
    public interface IUserManager
    {
        /// <summary>
        /// Get the user entity with the given ID, or null if it does not exist
        /// </summary>
        UserEntity GetUser(long userID);
    }
}
