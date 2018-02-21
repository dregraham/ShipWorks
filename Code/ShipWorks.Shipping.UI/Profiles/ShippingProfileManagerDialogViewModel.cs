using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
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
        private IEnumerable<ShippingProfileEntity> shippingProfiles;
        private readonly Func<ShippingProfileEntity, ShippingProfileEditorDlg> shippingProfileEditorDialogFactory;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDialogViewModel(IMessageHelper messageHelper,
            IShippingProfileManager shippingProfileManager,
            Func<ShippingProfileEntity, ShippingProfileEditorDlg> shippingProfileEditorDialogFactory)
        {
            this.messageHelper = messageHelper;
            this.shippingProfileManager = shippingProfileManager;
            this.shippingProfileEditorDialogFactory = shippingProfileEditorDialogFactory;
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, () => SelectedShippingProfile != null);
            DeleteCommand = new RelayCommand(Delete, () => SelectedShippingProfile != null && !SelectedShippingProfile.ShipmentTypePrimary);
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
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
        public IEnumerable<ShippingProfileEntity> ShippingProfiles
        {
            get => shippingProfiles;
            private set => handler.Set(nameof(ShippingProfiles), ref shippingProfiles, value);
        }

        /// <summary>
        /// Currently selected ShippingProfile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShippingProfileEntity SelectedShippingProfile
        {
            get => selectedShippingProfile;
            private set => handler.Set(nameof(SelectedShippingProfile), ref selectedShippingProfile, value);
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

                ShippingProfiles = shippingProfileManager.Profiles;
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
                ShippingProfiles = shippingProfileManager.Profiles;
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
                ShippingProfiles = shippingProfileManager.Profiles;
                SelectedShippingProfile = profile;
            }
        }
    }
}