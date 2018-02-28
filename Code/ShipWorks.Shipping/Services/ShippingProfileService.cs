﻿using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shared.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Service for managing ShippingProfiles and their shortcuts
    /// </summary>
    [Component]
    public class ShippingProfileService : IShippingProfileService
    {
        private readonly IShippingProfileManager profileManager;
        private readonly IShortcutManager shortcutManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IShippingProfileLoader profileLoader;

        public ShippingProfileService(IShippingProfileManager profileManager,
            IShortcutManager shortcutManager,
            ISqlAdapterFactory sqlAdapterFactory,
            IShippingProfileLoader profileLoader)
        {
            this.profileManager = profileManager;
            this.shortcutManager = shortcutManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.profileLoader = profileLoader;
        }

        /// <summary>
        /// Get all of the ShippingProfiles
        /// </summary>
        public IEnumerable<ShippingProfile> GetAll()
        {
            IEnumerable<ShortcutEntity> shortcuts = shortcutManager.Shortcuts;
            IEnumerable<ShippingProfileEntity> profiles = profileManager.Profiles;

            return profiles.ForEach(p => profileLoader.LoadProfileData(p, true))
                .Select(p => new ShippingProfile(p,
                    shortcuts.SingleOrDefault(s => s.RelatedObjectID == p.ShippingProfileID) ?? new ShortcutEntity()
                    {
                        Action = (int) KeyboardShortcutCommand.ApplyProfile,
                        RelatedObjectID = p.ShippingProfileID
                    }));
        }

        /// <summary>
        /// Get the ShippingProfileEntities corrisponding ShippingProfile
        /// </summary>
        public ShippingProfile Get(long shippingProfileEntityId)
        {
            ShippingProfile fetchedShippingProfile = null;

            ShippingProfileEntity profile = profileManager.Profiles.SingleOrDefault(p => p.ShippingProfileID == shippingProfileEntityId);

            if (profile!=null)
            {
                profileLoader.LoadProfileData(profile, true);
                ShortcutEntity shortcut = shortcutManager.Shortcuts.SingleOrDefault(s => s.RelatedObjectID == shippingProfileEntityId) ?? new ShortcutEntity();
                fetchedShippingProfile = new ShippingProfile(profile, shortcut);
            }

            return fetchedShippingProfile;
        }

        /// <summary>
        /// Create an empty ShippingProfile
        /// </summary>
        public ShippingProfile Create()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                Name = string.Empty,
                ShipmentTypePrimary = false
            };

            ShortcutEntity shortcutEntity = new ShortcutEntity
            {
                Action = (int) KeyboardShortcutCommand.ApplyProfile
            };

            profileLoader.LoadProfileData(profile, false);
            return new ShippingProfile(profile, shortcutEntity);
        }

        /// <summary>
        /// Save the ShippingProfile and its children 
        /// </summary>
        public Result Save(ShippingProfile shippingProfile)
        {
            Result result = Validate(shippingProfile);
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
                catch (ORMConcurrencyException)
                {
                    profileManager.InitializeForCurrentSession();
                    result = Result.FromError("Your changes cannot be saved because another use has deleted the profile.");
                }
                catch (ORMQueryExecutionException)
                {
                    result = Result.FromError(
                        "Your changes cannot be saved because another use has saved a profile with your selected HotKey.");
                }
            }

            return result;
        }

        /// <summary>
        /// Validate that the shippintProfile can be saved
        /// </summary>
        private Result Validate(ShippingProfile shippingProfile)
        {
            Result result = Result.FromSuccess();

            if (string.IsNullOrWhiteSpace(shippingProfile.ShippingProfileEntity.Name))
            {
                result = Result.FromError("Enter a name for the profile.");
            }
            else if (profileManager.Profiles.Any(profile =>
                profile.ShippingProfileID != shippingProfile.ShippingProfileEntity.ShippingProfileID &&
                profile.Name == shippingProfile.ShippingProfileEntity.Name))
            {
                result = Result.FromError("A profile with the chosen name already exists.");
            }
            else if (shortcutManager.Shortcuts.Any(s =>
                s.ShortcutID != shippingProfile.Shortcut.ShortcutID && s.Barcode == shippingProfile.Shortcut.Barcode))
            {
                result = Result.FromError($"The barcode \"{shippingProfile.Shortcut.Barcode}\" is already in use.");
            }

            return result;
        }
        
        /// <summary>
        /// Delete the ShippingProfile and its children
        /// </summary>
        public Result Delete(ShippingProfile shippingProfile)
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
            catch (ORMException)
            {
                return Result.FromError("An error occured when deleting the profile.");
            }
        }

        /// <summary>
        /// Get the available hotkeys for the given ShippingProfile
        /// </summary>
        public IEnumerable<Hotkey> GetAvailableHotkeys(ShippingProfile shippingProfile)
        {
            List<Hotkey> availableHotkeys = shortcutManager.GetAvailableHotkeys();
            if (shippingProfile.Shortcut?.Hotkey.HasValue ?? false)
            {
                availableHotkeys.Add(shippingProfile.Shortcut.Hotkey.Value);
            }

            return availableHotkeys;
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        public void LoadProfileData(ShippingProfile profile, bool refreshIfPresent)
        {
            profileLoader.LoadProfileData(profile.ShippingProfileEntity, refreshIfPresent);
        }
    }
}
