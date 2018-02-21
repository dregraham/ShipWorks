using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.UI.Profiles
{
    /// <summary>
    /// ViewModel for ShippingProfileManagerDialog
    /// </summary>
    [Component]
    public class ShippingProfileManagerDialogViewModel : IShippingProfileManagerDialogViewModel, INotifyPropertyChanged
    {
        private readonly IMessageHelper messageHelper;
        private readonly IShippingProfileManager shippingProfileManager;
        private readonly PropertyChangedHandler handler;
        private ShippingProfileEntity selectedShippingProfile;
        private IEnumerable<ShippingProfileAndShortcut> shippingProfilesAndShortcuts;
        private readonly Func<ShippingProfileEntity, ShippingProfileEditorDlg> shippingProfileEditorDialogFactory;
        private readonly IShortcutManager shortcutManager;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDialogViewModel(IMessageHelper messageHelper,
            IShippingProfileManager shippingProfileManager,
            Func<ShippingProfileEntity, ShippingProfileEditorDlg> shippingProfileEditorDialogFactory,
            IShortcutManager shortcutManager)
        {
            this.messageHelper = messageHelper;
            this.shippingProfileManager = shippingProfileManager;
            this.shippingProfileEditorDialogFactory = shippingProfileEditorDialogFactory;
            this.shortcutManager = shortcutManager;
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, () => SelectedShippingProfile != null);
            DeleteCommand = new RelayCommand(Delete, () => SelectedShippingProfile != null && !SelectedShippingProfile.ShipmentTypePrimary);
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            LoadShippingProfilesAndShortcuts();
        }

        /// <summary>
        /// Command to add a new profile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand AddCommand { get; }

        /// <summary>
        /// Command to edit an existing profile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand EditCommand { get; }

        /// <summary>
        /// Command to delete a profile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Collection of profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<ShippingProfileAndShortcut> ShippingProfiles
        {
            get => shippingProfilesAndShortcuts;
            private set => handler.Set(nameof(ShippingProfiles), ref shippingProfilesAndShortcuts, value);
        }

        /// <summary>
        /// Currently selected ShippingProfile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShippingProfileEntity SelectedShippingProfile
        {
            get => selectedShippingProfile;
            set => handler.Set(nameof(SelectedShippingProfile), ref selectedShippingProfile, value);
        }

        /// <summary>
        /// Delete a profile
        /// </summary>
        private void Delete()
        {
            DialogResult dialogResult = messageHelper.ShowQuestion($"Delete the profile {SelectedShippingProfile.Name}");
            if (dialogResult == DialogResult.Yes)
            {
                shippingProfileManager.DeleteProfile(SelectedShippingProfile);

                LoadShippingProfilesAndShortcuts();
            }
        }
        
        /// <summary>
        /// Edit a profile
        /// </summary>
        private void Edit()
        {
            ShippingProfileEditorDlg profileEditor = shippingProfileEditorDialogFactory(SelectedShippingProfile);

            if (profileEditor.ShowDialog() == DialogResult.OK)
            {
                LoadShippingProfilesAndShortcuts();
            }
        }

        /// <summary>
        /// Add a profile
        /// </summary>
        private void Add()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                Name = "",
                ShipmentTypePrimary = false
            };

            ShippingProfileEditorDlg profileEditor = shippingProfileEditorDialogFactory(profile);

            if (profileEditor.ShowDialog() == DialogResult.OK)
            {
                LoadShippingProfilesAndShortcuts();
                SelectedShippingProfile = profile;
            }
        }

        /// <summary>
        /// LoadsShippingProfilesAndShortcuts
        /// </summary>
        private void LoadShippingProfilesAndShortcuts()
        {
            IEnumerable<ShortcutEntity> shortcuts = shortcutManager.Shortcuts;
            ShippingProfiles = shippingProfileManager.Profiles.Select(profile => CreateShippingProfileAndShortcut(profile, shortcuts));
        }
        
        /// <summary>
        /// Given a profile, create a DTO with its associated hotkey text.
        /// </summary>
        private ShippingProfileAndShortcut CreateShippingProfileAndShortcut(ShippingProfileEntity profile, IEnumerable<ShortcutEntity> shortcuts)
        {
            ShortcutEntity shortcut = shortcuts.FirstOrDefault(s => s.RelatedObjectID == profile.ShippingProfileID);
            string shortcutText = string.Empty;
            if (shortcut?.Hotkey != null)
            {
                shortcutText = EnumHelper.GetDescription(shortcut.Hotkey);
            }

            return new ShippingProfileAndShortcut(profile, shortcutText);
        }
    }
}