using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Repository for ShippingProfiles.
    /// </summary>
    [Component]
    public class ShippingProfileRepository : IShippingProfileRepository
    {
        private readonly IShippingProfileManager profileManager;
        private readonly IShortcutManager shortcutManager;
        private readonly IShippingProfileFactory shippingProfileFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileRepository(IShippingProfileManager profileManager,
            IShortcutManager shortcutManager,
            IShippingProfileFactory shippingProfileFactory,
            Func<Type, ILog> createLogger)
        {
            this.profileManager = profileManager;
            this.shortcutManager = shortcutManager;
            this.shippingProfileFactory = shippingProfileFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Get all of the ShippingProfiles
        /// </summary>
        public IEnumerable<IShippingProfile> GetAll()
        {
            IEnumerable<IShortcutEntity> shortcuts = shortcutManager.ShortcutsReadOnly;

            return profileManager.ProfilesReadOnly
                .Select(x => CreateShippingProfile(x, shortcuts));
        }

        /// <summary>
        /// Get the ShippingProfileEntities corresponding ShippingProfile
        /// </summary>
        public IShippingProfile Get(long shippingProfileEntityId) =>
            Get(profileManager.GetProfileReadOnly(shippingProfileEntityId));

        /// <summary>
        /// Get the ShippingProfileEntities corresponding ShippingProfile
        /// </summary>
        public IShippingProfile Get(IShippingProfileEntity profile) =>
            profile != null ? CreateShippingProfile(profile, shortcutManager.Shortcuts) : null;

        /// <summary>
        /// Given a profile and all the shortcuts, create a ShippingProfile
        /// </summary>
        private IShippingProfile CreateShippingProfile(IShippingProfileEntity shippingProfileEntity, IEnumerable<IShortcutEntity> shortcuts)
        {
            IShortcutEntity shortcutEntity = shortcuts.SingleOrDefault(s => s.RelatedObjectID == shippingProfileEntity.ShippingProfileID);
            if (shortcutEntity == null)
            {
                shortcutEntity = new ShortcutEntity
                {
                    Action = KeyboardShortcutCommand.ApplyProfile,
                    RelatedObjectID = shippingProfileEntity.ShippingProfileID
                };
            }

            return shippingProfileFactory.Create(shippingProfileEntity, shortcutEntity);
        }
    }
}
