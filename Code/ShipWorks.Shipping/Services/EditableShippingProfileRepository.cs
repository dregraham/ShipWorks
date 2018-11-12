using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{

    /// <summary>
    /// Repository for ShippingProfiles.
    /// </summary>
    [Component(SingleInstance = true)]
    public class EditableShippingProfileRepository : IEditableShippingProfileRepository
    {
        private readonly IShippingProfileManager profileManager;
        private readonly IShortcutManager shortcutManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IShippingProfileFactory shippingProfileFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public EditableShippingProfileRepository(IShippingProfileManager profileManager,
            IShortcutManager shortcutManager,
            ISqlAdapterFactory sqlAdapterFactory,
            IShippingProfileFactory shippingProfileFactory,
            Func<Type, ILog> createLogger)
        {
            this.profileManager = profileManager;
            this.shortcutManager = shortcutManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.shippingProfileFactory = shippingProfileFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Get all of the ShippingProfiles
        /// </summary>
        public IEnumerable<IEditableShippingProfile> GetAll()
        {
            IEnumerable<ShortcutEntity> shortcuts = shortcutManager.Shortcuts.ToList();
            IEnumerable<ShippingProfileEntity> profiles = profileManager.Profiles;

            List<IEditableShippingProfile> shippingProfiles = new List<IEditableShippingProfile>();

            foreach (ShippingProfileEntity profile in profiles)
            {
                IEditableShippingProfile shippingProfile = CreateShippingProfile(profile, shortcuts);
                shippingProfiles.Add(shippingProfile);
            }

            return shippingProfiles;
        }

        /// <summary>
        /// Get the ShippingProfileEntities corresponding ShippingProfile
        /// </summary>
        public IEditableShippingProfile Get(long shippingProfileEntityId)
        {
            IEditableShippingProfile fetchedShippingProfile = null;

            ShippingProfileEntity profile = profileManager.Profiles.SingleOrDefault(p => p.ShippingProfileID == shippingProfileEntityId);

            if (profile != null)
            {
                fetchedShippingProfile = CreateShippingProfile(profile, shortcutManager.Shortcuts);
            }

            return fetchedShippingProfile;
        }

        /// <summary>
        /// Save the ShippingProfile and its children
        /// </summary>
        public Result Save(IEditableShippingProfile shippingProfile)
        {
            Result result = shippingProfile.Validate(profileManager, shortcutManager);
            if (result.Success)
            {
                try
                {
                    using (ISqlAdapter sqlAdapter = sqlAdapterFactory.CreateTransacted())
                    {
                        profileManager.SaveProfile(shippingProfile.ShippingProfileEntity, sqlAdapter);

                        shippingProfile.Shortcut.RelatedObjectID = shippingProfile.ShippingProfileEntity.ShippingProfileID;
                        shortcutManager.Save(shippingProfile.Shortcut, sqlAdapter);

                        sqlAdapter.Commit();
                    }
                }
                catch (ORMConcurrencyException ex)
                {
                    profileManager.InitializeForCurrentSession();
                    result = Result.FromError("Your changes cannot be saved because another user has deleted the profile.");
                    log.Error("Error saving shippingProfile", ex);
                }
                catch (ORMQueryExecutionException ex)
                {
                    result = Result.FromError("An error occurred saving your profile.");
                    log.Error("Error saving shippingProfile", ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Given a profile and all the shortcuts, create a ShippingProfile
        /// </summary>
        private IEditableShippingProfile CreateShippingProfile(ShippingProfileEntity shippingProfileEntity, IEnumerable<ShortcutEntity> shortcuts)
        {
            ShortcutEntity shortcutEntity = shortcuts.SingleOrDefault(s => s.RelatedObjectID == shippingProfileEntity.ShippingProfileID);
            if (shortcutEntity == null)
            {
                shortcutEntity = new ShortcutEntity
                {
                    Action = KeyboardShortcutCommand.ApplyProfile,
                    RelatedObjectID = shippingProfileEntity.ShippingProfileID
                };
            }

            return shippingProfileFactory.CreateEditable(shippingProfileEntity, shortcutEntity);
        }

        /// <summary>
        /// Delete the ShippingProfile and its children
        /// </summary>
        public Result Delete(IEditableShippingProfile shippingProfile)
        {
            try
            {
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.CreateTransacted())
                {
                    profileManager.DeleteProfile(shippingProfile.ShippingProfileEntity, sqlAdapter);
                    shortcutManager.Delete(shippingProfile.Shortcut, sqlAdapter);

                    sqlAdapter.Commit();
                }

                return Result.FromSuccess();
            }
            catch (ORMException ex)
            {
                log.Error("Error deleting shipping profile", ex);
                return Result.FromError("An error occurred when deleting the profile.");
            }
        }

        /// <summary>
        /// Load the shipping profile
        /// </summary>
        public void Load(IEditableShippingProfile profile, bool refreshIfPresent)
        {
            profileManager.LoadProfileData(profile.ShippingProfileEntity, refreshIfPresent);
        }
    }
}
