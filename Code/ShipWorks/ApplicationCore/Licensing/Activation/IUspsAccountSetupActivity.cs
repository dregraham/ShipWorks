namespace ShipWorks.ApplicationCore.Licensing.Activation
{
    /// <summary>
    /// An interface for carrying out the work required to setup a USPS account
    /// during registration.
    /// </summary>
    public interface IUspsAccountSetupActivity
    {
        /// <summary>
        /// Uses the user name and password provided to populate and save an USPS
        /// account entity.
        /// </summary>
        /// <param name="stampsUserName">The user name of the SDC account.</param>
        /// <param name="password">The password.</param>
        void Execute(string stampsUserName, string password);
    }
}