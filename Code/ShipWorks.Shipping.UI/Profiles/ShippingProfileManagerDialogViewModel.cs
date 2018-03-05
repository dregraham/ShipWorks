﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Profiles
{
    /// <summary>
    /// ViewModel for ShippingProfileManagerDialog
    /// </summary>
    [Component]
    public class ShippingProfileManagerDialogViewModel : IShippingProfileManagerDialogViewModel, INotifyPropertyChanged
    {
        private readonly IMessageHelper messageHelper;
        private readonly IShippingSettings shippingSettings;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IShippingProfileService shippingProfileService;
        private readonly PropertyChangedHandler handler;
        private ShippingProfile selectedShippingProfile;
        private ObservableCollection<ShippingProfile> shippingProfiles = new ObservableCollection<ShippingProfile>();
        private readonly Func<ShippingProfile, ShippingProfileEditorDlg> shippingProfileEditorDialogFactory;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDialogViewModel(IShippingProfileService shippingProfileService,
            Func<ShippingProfile, ShippingProfileEditorDlg> shippingProfileEditorDialogFactory,
            IMessageHelper messageHelper,
            IShippingSettings shippingSettings,
            IShipmentTypeManager shipmentTypeManager)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            this.shippingProfileService = shippingProfileService;
            this.shippingProfileEditorDialogFactory = shippingProfileEditorDialogFactory;
            this.messageHelper = messageHelper;
            this.shippingSettings = shippingSettings;
            this.shipmentTypeManager = shipmentTypeManager;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, () => SelectedShippingProfile != null);
            DeleteCommand = new RelayCommand(Delete,
                () => SelectedShippingProfile != null && !SelectedShippingProfile.ShippingProfileEntity.ShipmentTypePrimary);

            ShippingProfiles = new ObservableCollection<ShippingProfile>(shippingProfileService.GetAll()
                .Where(IncludeProfileInGrid));
        }

        /// <summary>
        /// Returns true if should show in grid
        /// </summary>
        private bool IncludeProfileInGrid(ShippingProfile shippingProfile)
        {
            ShipmentTypeCode? shipmentType = shippingProfile.ShippingProfileEntity.ShipmentType;
            // Global shipment types should be included
            if (!shipmentType.HasValue)
            {
                return true;
            }

            // None shipment type should always be excluded
            if (shipmentType.Value == ShipmentTypeCode.None)
            {
                return false;
            }
            
            // Best rate never gets configured, so we include it if it is allowed
            if (shipmentType.Value == ShipmentTypeCode.BestRate && shipmentTypeManager.ShipmentTypeCodes.Contains(ShipmentTypeCode.BestRate))
            {
                return true;
            }

            // For all other types, include if configured
            return shippingSettings.IsConfigured(shipmentType.Value);
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
        public ObservableCollection<ShippingProfile> ShippingProfiles
        {
            get => shippingProfiles;
            private set => handler.Set(nameof(ShippingProfiles), ref shippingProfiles, value);
        }

        /// <summary>
        /// Currently selected ShippingProfile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShippingProfile SelectedShippingProfile
        {
            get => selectedShippingProfile;
            set => handler.Set(nameof(SelectedShippingProfile), ref selectedShippingProfile, value);
        }

        /// <summary>
        /// Delete a profile
        /// </summary>
        private void Delete()
        {
            DialogResult dialogResult = messageHelper.ShowQuestion($"Delete the profile {SelectedShippingProfile.ShippingProfileEntity.Name}?");
            if (dialogResult == DialogResult.OK)
            {
                // Unset the profile before deleting so it isnt used in logic after the delete
                ShippingProfile profileToDelete = SelectedShippingProfile;
                selectedShippingProfile = null;
                ShippingProfiles.Remove(profileToDelete);
                shippingProfileService.Delete(profileToDelete);
            }
        }
        
        /// <summary>
        /// Edit a profile
        /// </summary>
        private void Edit()
        {
            ShippingProfile selected = SelectedShippingProfile;

            ShippingProfileEditorDlg profileEditor = shippingProfileEditorDialogFactory(selected);

            profileEditor.ShowDialog();

            ShippingProfiles.Remove(selected);
            ShippingProfiles.Add(selected);
            SelectedShippingProfile = selected;
        }

        /// <summary>
        /// Add a profile
        /// </summary>
        private void Add()
        {
            ShippingProfile profile = shippingProfileService.CreateEmptyShippingProfile();

            if (shippingProfileEditorDialogFactory(profile).ShowDialog() == DialogResult.OK)
            {
                ShippingProfiles.Add(profile);
                SelectedShippingProfile = profile;
            }
        }
    }
}