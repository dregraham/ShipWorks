
namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration
{
    /// <summary>
    /// Class for populating the Stamps.com registration type combo box.
    /// </summary>
    public class StampsRegistrationTypeDropdownItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsAccountUsageDropdownItem"/> class.
        /// </summary>
        public StampsRegistrationTypeDropdownItem(PostalAccountRegistrationType registrationType, string displayName)
        {
            RegistrationType = registrationType;
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets the type of the registration.
        /// </summary>
        public PostalAccountRegistrationType RegistrationType { get; private set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
