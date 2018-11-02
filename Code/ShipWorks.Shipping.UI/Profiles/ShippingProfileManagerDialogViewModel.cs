using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Templates.Printing;

namespace ShipWorks.Shipping.UI.Profiles
{
    /// <summary>
    /// ViewModel for ShippingProfileManagerDialog
    /// </summary>
    [Component]
    public class ShippingProfileManagerDialogViewModel : IShippingProfileManagerDialogViewModel, INotifyPropertyChanged
    {
        private readonly IMessageHelper messageHelper;
        private readonly IPrintJobFactory printJobFactory;
        private readonly IWin32Window owner;
        private readonly IShippingProfileService shippingProfileService;
        private readonly PropertyChangedHandler handler;
        private IEditableShippingProfile selectedShippingProfile;
        private readonly Func<IEditableShippingProfile, ShippingProfileEditorDlg> shippingProfileEditorDialogFactory;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDialogViewModel(IShippingProfileService shippingProfileService,
            Func<IEditableShippingProfile, ShippingProfileEditorDlg> shippingProfileEditorDialogFactory,
            IMessageHelper messageHelper,
            IPrintJobFactory printJobFactory,
            IWin32Window owner)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            this.shippingProfileService = shippingProfileService;
            this.shippingProfileEditorDialogFactory = shippingProfileEditorDialogFactory;
            this.messageHelper = messageHelper;
            this.printJobFactory = printJobFactory;
            this.owner = owner;
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, () => SelectedShippingProfile != null);
            DeleteCommand = new RelayCommand(Delete,
                () => SelectedShippingProfile != null && !SelectedShippingProfile.ShippingProfileEntity.ShipmentTypePrimary);

            PrintBarcodesCommand = new RelayCommand(PrintBarcodes);

            ShippingProfiles = new ObservableCollection<IEditableShippingProfile>(shippingProfileService.GetEditableConfiguredShipmentTypeProfiles());
        }

        /// <summary>
        /// Print the barcodes
        /// </summary>
        private void PrintBarcodes() =>
            printJobFactory.CreateBarcodePrintJob().PreviewAsync((Form) owner);

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
        /// Command to print the barcodes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand PrintBarcodesCommand { get; }

        /// <summary>
        /// Collection of profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<IEditableShippingProfile> ShippingProfiles { get; }

        /// <summary>
        /// Currently selected ShippingProfile
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEditableShippingProfile SelectedShippingProfile
        {
            get => selectedShippingProfile;
            set => handler.Set(nameof(SelectedShippingProfile), ref selectedShippingProfile, value);
        }

        /// <summary>
        /// Delete a profile
        /// </summary>
        private void Delete()
        {
            DialogResult dialogResult =
                messageHelper.ShowQuestion($"Delete the profile {SelectedShippingProfile.ShippingProfileEntity.Name}?");
            if (dialogResult == DialogResult.OK)
            {
                // Unset the profile before deleting so it isnt used in logic after the delete
                IEditableShippingProfile profileToDelete = SelectedShippingProfile;
                SelectedShippingProfile = null;
                ShippingProfiles.Remove(profileToDelete);
                shippingProfileService.Delete(profileToDelete);
            }
        }

        /// <summary>
        /// Edit a profile
        /// </summary>
        private void Edit()
        {
            IEditableShippingProfile selected = SelectedShippingProfile;

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
            IEditableShippingProfile profile = shippingProfileService.CreateEmptyShippingProfile();

            if (shippingProfileEditorDialogFactory(profile).ShowDialog() == DialogResult.OK)
            {
                // The dialog saves and refetches, but this profile still points to the old profile,
                // so we need to get the new one.
                profile = shippingProfileService.GetEditable(profile.ShippingProfileEntity.ShippingProfileID);

                ShippingProfiles.Add(profile);
                SelectedShippingProfile = profile;
            }
        }
    }
}