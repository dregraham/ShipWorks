using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        private ShippingProfileAndShortcut selectedShippingProfile;
        private ObservableCollection<ShippingProfileAndShortcut> shippingProfilesAndShortcuts;
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
            DeleteCommand = new RelayCommand(Delete, 
                () => SelectedShippingProfile != null && !SelectedShippingProfile.ShippingProfile.ShipmentTypePrimary);
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            ShippingProfiles = new ObservableCollection<ShippingProfileAndShortcut>(shippingProfileManager.Profiles
                                .Where(profile => profile.ShipmentType != ShipmentTypeCode.None)
                                .Select(profile => new ShippingProfileAndShortcut(profile, shortcutManager.Shortcuts)));
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
        public ObservableCollection<ShippingProfileAndShortcut> ShippingProfiles
        {
            get => shippingProfilesAndShortcuts;
            private set => handler.Set(nameof(ShippingProfiles), ref shippingProfilesAndShortcuts, value);
        }

        /// <summary>
        /// Currently selected ShippingProfile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShippingProfileAndShortcut SelectedShippingProfile
        {
            get => selectedShippingProfile;
            set => handler.Set(nameof(SelectedShippingProfile), ref selectedShippingProfile, value);
        }

        /// <summary>
        /// Delete a profile
        /// </summary>
        private void Delete()
        {
            DialogResult dialogResult = messageHelper.ShowQuestion($"Delete the profile {SelectedShippingProfile.ShippingProfile.Name}");
            if (dialogResult == DialogResult.OK)
            {
                // Unset the profile before deleting so it isnt used in logic after the delete
                ShippingProfileAndShortcut profileToDelete = SelectedShippingProfile;
                selectedShippingProfile = null;
                ShippingProfiles.Remove(profileToDelete);
                shippingProfileManager.DeleteProfile(profileToDelete.ShippingProfile);
            }
        }
        
        /// <summary>
        /// Edit a profile
        /// </summary>
        private void Edit()
        {
            ShippingProfileEditorDlg profileEditor = shippingProfileEditorDialogFactory(SelectedShippingProfile.ShippingProfile);

            profileEditor.ShowDialog();
        }

        /// <summary>
        /// Add a profile
        /// </summary>
        private void Add()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                Name = string.Empty,
                ShipmentTypePrimary = false
            };
            
            if (shippingProfileEditorDialogFactory(profile).ShowDialog() == DialogResult.OK)
            {
                ShippingProfileAndShortcut newShortcut = new ShippingProfileAndShortcut(profile, shortcutManager.Shortcuts);
                ShippingProfiles.Add(newShortcut);
                SelectedShippingProfile = newShortcut;
            }
        }
    }
}