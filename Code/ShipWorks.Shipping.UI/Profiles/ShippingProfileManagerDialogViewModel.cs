using System;
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
        private IShippingProfile selectedShippingProfile;
        private ObservableCollection<IShippingProfile> shippingProfiles = new ObservableCollection<IShippingProfile>();
        private readonly Func<IShippingProfile, ShippingProfileEditorDlg> shippingProfileEditorDialogFactory;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDialogViewModel(IShippingProfileService shippingProfileService,
            Func<IShippingProfile, ShippingProfileEditorDlg> shippingProfileEditorDialogFactory,
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

            ShippingProfiles = new ObservableCollection<IShippingProfile>(shippingProfileService.GetAll()
                .Where(IncludeProfileInGrid));
        }

        /// <summary>
        /// Returns true if should show in grid
        /// </summary>
        private bool IncludeProfileInGrid(IShippingProfile shippingProfile)
        {
            ShipmentTypeCode? shipmentType = shippingProfile.ShippingProfileEntity.ShipmentType;
            
            // Return true if glbal profile or the shipment type is configured
            return !shipmentType.HasValue || shipmentTypeManager.ConfiguredShipmentTypes.Contains(shipmentType.Value);
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
        public ObservableCollection<IShippingProfile> ShippingProfiles
        {
            get => shippingProfiles;
            private set => handler.Set(nameof(ShippingProfiles), ref shippingProfiles, value);
        }

        /// <summary>
        /// Currently selected ShippingProfile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IShippingProfile SelectedShippingProfile
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
                IShippingProfile profileToDelete = SelectedShippingProfile;
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
            IShippingProfile selected = SelectedShippingProfile;

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
            IShippingProfile profile = shippingProfileService.CreateEmptyShippingProfile();

            if (shippingProfileEditorDialogFactory(profile).ShowDialog() == DialogResult.OK)
            {
                // The dialog saves and refetches, but this profile still points to the old profile,
                // so we need to get the new one.
                profile = shippingProfileService.Get(profile.ShippingProfileEntity.ShippingProfileID);
                
                ShippingProfiles.Add(profile);
                SelectedShippingProfile = profile;
            }
        }
    }
}