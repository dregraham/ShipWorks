using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Users
{
    /// <summary>
    /// User login workflow
    /// </summary>
    [Component]
    public class UserLoginWorkflow : IUserLoginWorkflow
    {
        readonly IUserSession userSession;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserLoginWorkflow(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        /// <summary>
        /// Get the current user
        /// </summary>
        public UserEntity CurrentUser => userSession.User;

        /// <summary>
        /// Logs the user in with the given UserEntity
        /// </summary>
        /// <remarks>
        /// This method goes through MainForm so that we get the full logon flow
        /// </remarks>
        public void Logon(UserEntity user) => Program.MainForm.InitiateLogon(user);

        /// <summary>
        /// Log off the currently logged on user.
        /// </summary>
        /// <remarks>
        /// This method goes through MainForm so that we get the full logoff flow
        /// </remarks>
        public void Logoff(bool clearRememberMe) => Program.MainForm.InitiateLogoff(clearRememberMe);
    }
}
