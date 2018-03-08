﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
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
        private readonly IShippingProfileApplicationStrategyFactory strategyFactory;
        private readonly ILog log;

        public ShippingProfileService(IShippingProfileManager profileManager,
            IShortcutManager shortcutManager,
            ISqlAdapterFactory sqlAdapterFactory,
            IShippingProfileLoader profileLoader,
            Func<Type, ILog> createLogger,
            IShippingProfileApplicationStrategyFactory strategyFactory)
        {
            this.profileManager = profileManager;
            this.shortcutManager = shortcutManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.profileLoader = profileLoader;
            this.strategyFactory = strategyFactory;
            this.log = createLogger(GetType());
        }

        /// <summary>
        /// Get all of the ShippingProfiles
        /// </summary>
        public IEnumerable<IShippingProfile> GetAll()
        {
            IEnumerable<ShortcutEntity> shortcuts = shortcutManager.Shortcuts;
            IEnumerable<ShippingProfileEntity> profiles = profileManager.Profiles;

            return profiles.ForEach(p => profileLoader.LoadProfileData(p, true))
                .Select(p => CreateShippingProfile(p, shortcuts));
        }

        /// <summary>
        /// Given a profile and all the shortcuts, create a ShippingProfile
        /// </summary>
        private IShippingProfile CreateShippingProfile(ShippingProfileEntity shippingProfileEntity, IEnumerable<ShortcutEntity> shortcuts)
        {
            ShortcutEntity shortcutEntity = shortcuts.SingleOrDefault(s => s.RelatedObjectID == shippingProfileEntity.ShippingProfileID);
            if (shortcutEntity == null)
            {
                shortcutEntity = new ShortcutEntity
                {
                    Action = (int) KeyboardShortcutCommand.ApplyProfile,
                    RelatedObjectID = shippingProfileEntity.ShippingProfileID
                };
            }

            return CreateShippingProfile(shippingProfileEntity, shortcutEntity);
        }

        /// <summary>
        /// Given a profile and its associated shortcut, create a Shipping Profile
        /// </summary>
        private IShippingProfile CreateShippingProfile(ShippingProfileEntity shippingProfileEntity, ShortcutEntity shortcutEntity)
        {
            return new ShippingProfile(shippingProfileEntity, shortcutEntity, profileManager, shortcutManager, profileLoader, strategyFactory);
        }

        /// <summary>
        /// Get the ShippingProfileEntities corrisponding ShippingProfile
        /// </summary>
        public IShippingProfile Get(long shippingProfileEntityId)
        {
            IShippingProfile fetchedShippingProfile = null;

            ShippingProfileEntity profile = profileManager.Profiles.SingleOrDefault(p => p.ShippingProfileID == shippingProfileEntityId);

            if (profile != null)
            {
                profileLoader.LoadProfileData(profile, true);
                ShortcutEntity shortcut = shortcutManager.Shortcuts.SingleOrDefault(s => s.RelatedObjectID == shippingProfileEntityId) ??
                    new ShortcutEntity
                    {
                        Action = (int) KeyboardShortcutCommand.ApplyProfile,
                        RelatedObjectID = shippingProfileEntityId
                    };

                fetchedShippingProfile = CreateShippingProfile(profile, shortcut);
            }

            return fetchedShippingProfile;
        }
        
        /// <summary>
        /// Create an empty ShippingProfile
        /// </summary>
        public IShippingProfile CreateEmptyShippingProfile()
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
            return CreateShippingProfile(profile, shortcutEntity);
        }

        /// <summary>
        /// Save the ShippingProfile and its children 
        /// </summary>
        public Result Save(IShippingProfile shippingProfile)
        {
            Result result = shippingProfile.Validate();
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
                    result = Result.FromError("Your changes cannot be saved because another use has deleted the profile.");
                    log.Error("Error saving shippingProfile", ex);
                }
                catch (ORMQueryExecutionException ex)
                {
                    result = Result.FromError("An error ocurred saving your profile.");
                    log.Error("Error saving shippingProfile", ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Delete the ShippingProfile and its children
        /// </summary>
        public Result Delete(IShippingProfile shippingProfile)
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
                return Result.FromError("An error occured when deleting the profile.");
            }
        }

        /// <summary>
        /// Get the available hotkeys for the given ShippingProfile
        /// </summary>
        public IEnumerable<Hotkey> GetAvailableHotkeys(IShippingProfile shippingProfile)
        {
            List<Hotkey> availableHotkeys = shortcutManager.GetAvailableHotkeys();
            if (shippingProfile.Shortcut?.Hotkey.HasValue ?? false)
            {
                availableHotkeys.Add(shippingProfile.Shortcut.Hotkey.Value);
            }

            return availableHotkeys;
        }
    }
}
