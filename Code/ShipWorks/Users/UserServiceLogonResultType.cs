using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Users
{
    /// <summary>
    /// The result of logging in using the UserService.
    /// 
    /// Represents success or the reason why the login failed.
    /// </summary>
    public enum UserServiceLogonResultType
    {
        InvalidCredentials,
        
        TangoAccountDisabled,

        Success
    }
}
