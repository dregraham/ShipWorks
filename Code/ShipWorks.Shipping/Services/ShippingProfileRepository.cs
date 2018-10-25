using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Win32.Native;
using log4net;
using ShipWorks.Common.IO.KeyboardShortcuts;
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
            IShortcutEntity shortcutEntity = shortcuts.SingleOrDefault(s => s.RelatedObjectID == shippingProfileEntity.ShippingProfileID) ??
                new ProfileShortcutSkeleton(shippingProfileEntity.ShippingProfileID);

            return shippingProfileFactory.Create(shippingProfileEntity, shortcutEntity);
        }

        /// <summary>
        /// Skeleton implementation of Shortcut when a profile doesn't actually have any shortcuts
        /// </summary>
        /// <remarks>
        /// We used to create a new instance of the ShortcutEntity and set the two values implemented
        /// in this skeleton, but for a large number of profiles, doing that took a non-trivial amount
        /// of time because of the LLBLgen machinery involved. Since we don't need any of that in this
        /// case, we can use a fake - but fast - version instead.
        /// </remarks>
        private class ProfileShortcutSkeleton : IShortcutEntity
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ProfileShortcutSkeleton(long relatedObjectID)
            {
                RelatedObjectID = relatedObjectID;
            }

            /// <summary>
            /// Shortcut ID
            /// </summary>
            public long ShortcutID => 0;

            /// <summary>
            /// Row version
            /// </summary>
            public byte[] RowVersion => new byte[0];

            /// <summary>
            /// Modifier keys
            /// </summary>
            public KeyboardShortcutModifiers? ModifierKeys => null;

            /// <summary>
            /// Virtual key
            /// </summary>
            public VirtualKeys? VirtualKey => null;

            /// <summary>
            /// Barcode
            /// </summary>
            public string Barcode => null;

            /// <summary>
            /// Action
            /// </summary>
            public KeyboardShortcutCommand Action => KeyboardShortcutCommand.ApplyProfile;

            /// <summary>
            /// Related object id
            /// </summary>
            public long? RelatedObjectID { get; }

            /// <summary>
            /// AsReadOnly
            /// </summary>
            public IShortcutEntity AsReadOnly() => this;

            /// <summary>
            /// AsReadOnly
            /// </summary>
            public IShortcutEntity AsReadOnly(IDictionary<object, object> objectMap) => this;
        }
    }
}
