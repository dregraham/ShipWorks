using System.Drawing;
using Divelements.SandGrid.Rendering;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.BestRate.Setup;
using log4net;
using RestSharp.Extensions;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Filters;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores;
using ShipWorks.Templates;
using ShipWorks.Templates.Media;
using ShipWorks.Templates.Printing;
using ShipWorks.Templates.Processing;
using ShipWorks.UI.Utility;
using ShipWorks.UI.Wizard;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Window from which all shipments are created
    /// </summary>
    partial class ShippingDlg : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShippingDlg));

        // The singleton list of the current set of shipping errors.
        static Dictionary<long, Exception> processingErrors = new Dictionary<long, Exception>();

        // We load shipments asyncronously.  This flag lets us know if that's what we're currently doing, so we don't try to do
        // it reentrantly.
        bool loadingSelectedShipments = false;

        // Usually when changing selected shipments if the service control type doesn't change we don't recreate it, we just load the new data into it.
        // But after changing shipping settings we need to make sure a new one gets initialized to pick up any new settings.
        bool forceRecreateServiceControl = false;

        // Indicates if the tracking control will be initially displayed
        bool initialDisplayTracking = false;

        // The list of shipments that are the ones currently shown in the UI, not necessarily the current selection dependong on async loading stuff
        List<ShipmentEntity> uiDisplayedShipments = new List<ShipmentEntity>();

        // List of shipment types that were "Activated" at the time the UI was loaded.  This is important b\c if it got activated after we loaded the UI from another machine,
        // and we used the "live" value, then we'd think a shipment was currently loaded when really it wasn't.
        List<ShipmentTypeCode> uiActivatedShipmentTypes = new List<ShipmentTypeCode>();

        // Maps shipment ID's to the list of rates for the shipment
        Dictionary<long, Dictionary<ShipmentTypeCode, RateGroup>> shipmentRateMap = new Dictionary<long, Dictionary<ShipmentTypeCode, RateGroup>>();

        // If the user is processing with best rate and counter rates, they have the option to ignore signing up for
        // counter rates during the current batch.  This variable will be for tracking that flag.
        bool showCounterRateSetupWizard = true;

        private List<ShipmentEntity> loadedShipmentEntities;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDlg(List<ShipmentEntity> shipments)
            : this(shipments, false)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDlg(List<ShipmentEntity> shipments, bool trackingPage)
        {
            InitializeComponent();

            if (shipments == null)
            {
                throw new ArgumentNullException("shipments");
            }

            ThemedBorderProvider.Apply(rateControlArea);

            // Load all the shipments into the grid
            shipmentControl.AddShipments(shipments);

            shipmentControl.ErrorProvider = new ShipmentGridRowProcessingErrorProvider();

            // Security
            panelSettingsButtons.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);

            this.initialDisplayTracking = trackingPage;

            rateControl.Initialize(new FootnoteParameters(GetRates, GetStoreForCurrentShipment));

            // Default the minimum size of the tab control panel to be 2/3rds the hight of the shipping dialog.
            ratesSplitContainer.Panel1MinSize = 2*(Size.Height/3);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // Manage the window positioning
            WindowStateSaver windowSaver = new WindowStateSaver(this);
            windowSaver.ManageSplitter(splitContainer, "Splitter");
            windowSaver.ManageSplitter(ratesSplitContainer, "RateSplitter");

            labelInternal.Visible = InterapptiveOnly.IsInterapptiveUser;
            unprocess.Visible = InterapptiveOnly.IsInterapptiveUser;

            shipmentControl.Initialize();

            comboShipmentType.DisplayMember = "Key";
            comboShipmentType.ValueMember = "Value";

            LoadShipmentTypeCombo();

            if (initialDisplayTracking)
            {
                tabControl.SelectedTab = tabPageTracking;
            }

            ShipmentGridRow firstRow = shipmentControl.AllRows.FirstOrDefault();
            if (firstRow != null)
            {
                firstRow.Selected = true;
            }
            else
            {
                LoadSelectedShipments(false);
            }

            UpdateEditControlsSecurity();
        }

        /// <summary>
        /// Window has become visible
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            // Without this the window looks half-initialized while its working hard in the background to load the shipment data
            Refresh();
        }

        /// <summary>
        /// Load the shipment type combo with the available shipment types
        /// </summary>
        private void LoadShipmentTypeCombo()
        {
            ShipmentTypeCode? selected = null;
            bool multiValued = comboShipmentType.MultiValued;

            if (!multiValued && comboShipmentType.SelectedIndex >= 0)
            {
                selected = (ShipmentTypeCode) comboShipmentType.SelectedValue;
            }

            List<KeyValuePair<string, ShipmentTypeCode>> enabledTypes = ShipmentTypeManager.ShipmentTypes
                .Where(t => ShippingManager.IsShipmentTypeEnabled(t.ShipmentTypeCode))
                .Select(t => new KeyValuePair<string, ShipmentTypeCode>(t.ShipmentTypeName, t.ShipmentTypeCode)).ToList();

            if (selected != null && !enabledTypes.Any(p => p.Value == selected.Value))
            {
                enabledTypes.Add(new KeyValuePair<string, ShipmentTypeCode>(ShipmentTypeManager.GetType(selected.Value).ShipmentTypeName, selected.Value));
                SortShipmentTypes(enabledTypes);
            }

            comboShipmentType.SelectedIndexChanged -= this.OnChangeShipmentType;
            comboShipmentType.DataSource = enabledTypes;

            if (multiValued)
            {
                comboShipmentType.MultiValued = true;
            }
            else if (selected != null)
            {
                comboShipmentType.SelectedValue = selected.Value;
            }

            comboShipmentType.SelectedIndexChanged += this.OnChangeShipmentType;
        }

        /// <summary>
        /// Sort the list of shipment types for display in the combobox
        /// </summary>
        private void SortShipmentTypes(List<KeyValuePair<string, ShipmentTypeCode>> enabledTypes)
        {
            enabledTypes.Sort(new Comparison<KeyValuePair<string, ShipmentTypeCode>>((left, right) =>
                {
                    string lValue = left.Key == "None" ? "z" : left.Key;
                    string rValue = right.Key == "None" ? "z" : right.Key;

                    return lValue.CompareTo(rValue);
                }));
        }

        /// <summary>
        /// The singleton instance of the processing errors from the last time shipments were processed using the shipping window.  This works
        /// because the shipping window can only be open at most once on the screen.
        /// </summary>
        public static Dictionary<long, Exception> ProcessingErrors
        {
            get { return processingErrors; }
        }

        /// <summary>
        /// The splitter has moved.
        /// </summary>
        private void OnSplitterMoved(object sender, SplitterEventArgs e)
        {
            // For some reason the tab control looks goofy and doesnt redraw after the splitter moves, so we force it
            Refresh();
        }

        /// <summary>
        /// The set of selected shipments has changed
        /// </summary>
        private void OnChangeSelectedShipments(object sender, ShipmentSelectionChangedEventArgs e)
        {
            // If its not already in the process of loading selected shipments, save and load.  If it is already in the process, the
            // completion handler for the loader will recall itself.  It's possible to be already loading if your using the up-arrow
            // in the shipment grid real fast.
            if (!loadingSelectedShipments)
            {
                // Save all changes from the UI to the previous entity selection
                SaveUIDisplayedShipments();

                // Load the newly selected shipments
                LoadSelectedShipments(false);
            }
        }

        /// <summary>
        /// The selected shipment type has changed
        /// </summary>
        private void OnChangeShipmentType(object sender, EventArgs e)
        {
            // Shouldnt be able to be in the middle of loading and get that combo to change at the same time.
            Debug.Assert(!loadingSelectedShipments);

            // Save all changes from the UI to the previous entity selection
            SaveUIDisplayedShipments();

            // Reload the displayed shipments so that they show the new shipment type UI
            LoadSelectedShipments(true);
        }

        /// <summary>
        /// Gets the service control that is currently displayed
        /// </summary>
        private ServiceControlBase ServiceControl
        {
            get
            {
                if (serviceControlArea.Controls.Count == 0)
                {
                    return null;
                }

                return serviceControlArea.Controls[0] as ServiceControlBase;
            }
        }

        /// <summary>
        /// Gets the customs control that is currently displayed
        /// </summary>
        private CustomsControlBase CustomsControl
        {
            get
            {
                if (customsControlArea.Controls.Count == 0)
                {
                    return null;
                }

                return (CustomsControlBase) customsControlArea.Controls[0];
            }
        }

        /// <summary>
        /// Update the shipment details section to display the data currently in the selected shipments
        /// </summary>
        private void LoadSelectedShipments(bool resortWhenDone)
        {
            // If we're already in the process of loading shipments, there is no need to process this
            if (loadingSelectedShipments)
            {
                return;
            }

            // We are now in the process of loading shipments
            loadingSelectedShipments = true;

            List<ShipmentEntity> deleted = new List<ShipmentEntity>();
            List<ShipmentEntity> loaded = new List<ShipmentEntity>();

            // Get the list of setup shipment types up front - so in case it changes from another ShipWorks in the middle of loading,
            // all shipments of the same type are loaded in the same way.
            uiActivatedShipmentTypes = ShippingSettings.Fetch().ActivatedTypes.Select(v => (ShipmentTypeCode) v).ToList();
            uiActivatedShipmentTypes.Add(ShipmentTypeCode.None);

            BackgroundExecutor<ShipmentEntity> executor = new BackgroundExecutor<ShipmentEntity>(this,
                "Preparing Shipments",
                "ShipWorks is preparing the shipments.",
                "Shipment {0} of {1}");

            // Code to execute once background load is complete
            executor.ExecuteCompleted += LoadSelectedShipmentsCompleted;

            // User state that will get passed to the completed method
            var userState = new Dictionary<string, object>();
            userState["loaded"] = loaded;
            userState["deleted"] = deleted;
            userState["resortWhenDone"] = resortWhenDone;

            // We need to load a copy of the shipments, since its going to be on the background thread.  Otherwise the UI could try to draw at the exact same
            // time it was being loaded on the background, and crashes could occur.
            IEnumerable<ShipmentEntity> shipmentsToLoad = shipmentControl.SelectedRows.Select(r => EntityUtility.CloneEntity(r.Shipment));

            // Code to execute for each shipment
            executor.ExecuteAsync((shipment, state, issueAdder) =>
                {
                    // If we already know its deleted, don't bother
                    if (shipment.DeletedFromDatabase)
                    {
                        deleted.Add(shipment);
                    }
                    else
                    {
                        try
                        {
                            // Only load the specific data if the shipment type is setup, b\c if its not, 
                            // then we don't have proper defaults to load from.
                            if (IsShipmentTypeActivatedUI((ShipmentTypeCode)shipment.ShipmentType))
                            {
                                ShippingManager.EnsureShipmentLoaded(shipment);
                            }
                            // Even without the type being setup, we can still load the customs stuff.  Normally EnsureShipmentLoaded would do that for us.
                            else
                            {
                                CustomsManager.LoadCustomsItems(shipment, false);
                            }

                            loaded.Add(shipment);
                        }
                        catch (ObjectDeletedException)
                        {
                            deleted.Add(shipment);
                        }
                        catch (SqlForeignKeyException)
                        {
                            deleted.Add(shipment);
                        }
                        catch (InvalidOperationException ex)
                        {
                            // If a processed shipment has been deleted by another SW instance and the user tries to load it here,
                            // the New Shipment creation procedure can be initiated.  This can result in 
                            // an InvalidOperationException.  It can only mean the base shipment entity has been deleted.
                            if (ex.Data.Contains("UpdateDynamicData"))
                            {
                                deleted.Add(shipment);
                            }
                            else
                            {
                                // we don't want any other causes of InvalidOperationException to get eaten
                                throw;
                            }
                        }
                    }
                },

                // Each shipment to execute the code for
                shipmentsToLoad,
                
                // User state
                userState
            );
        }

        /// <summary>
        /// Save what's in the UI right now to what was loaded into it.  Returns true if it succeeds and there are no errors.  Return false
        /// if there were concurrency or deleted errors detected.  Upon return of false, depending on when\where this is called, the grid should
        /// be reloaded following.
        /// </summary>
        private bool SaveUIDisplayedShipments()
        {
            // Save all changes from the UI to the entities loaded into the UI
            SaveChangesToUIDisplayedShipments();

            // Save changes to the database for those entities that have been completely removed from the grid.  If we didn't do this now, then
            // the save would never happen, b\c we wouldnt have a reference to it when we closed.
            var removedNeedsSaved = uiDisplayedShipments.Where(s => !s.DeletedFromDatabase && shipmentControl.FindRow(s.ShipmentID) == null).ToList();

            // For the one's that have been removed, ignore any concurrency\deleted errors
            SaveShipmentsToDatabase(removedNeedsSaved, false);

            // We also need to save changes to any whose state\country has changed, since that affects customs items requirements
            var destinationChangeNeedsSaved = uiDisplayedShipments.Except(removedNeedsSaved)
                .Where(s => s.Fields[ShipmentFields.ShipStateProvCode.FieldIndex].IsChanged || s.Fields[ShipmentFields.ShipCountryCode.FieldIndex].IsChanged).ToList();

            // We need to show the user if anything went wrong while doing that
            Dictionary<ShipmentEntity, Exception> errors = SaveShipmentsToDatabase(destinationChangeNeedsSaved, false);

            // When the destination address is changed we save the affected shipments to the database right away to ensure that any concurrency errors that could
            // occur while generating customs items are caught right away.  This is why we do the Load here - if the customs items already exist, then this will do
            // nothing.  Which means that this will do nothing unless the shipment changed from being a domestic to an international.
            foreach (ShipmentEntity shipment in destinationChangeNeedsSaved.Except(errors.Keys))
            {
                try
                {
                    CustomsManager.LoadCustomsItems(shipment, false);
                }
                catch (SqlForeignKeyException ex)
                {
                    errors[shipment] = ex;
                }
                catch (ORMConcurrencyException ex)
                {
                    errors[shipment] = ex;
                }
            }

            // Go through each one not known to be deleted and try to get it completely refreshed so we can update it's UI display
            foreach (var pair in errors)
            {
                ShipmentEntity shipment = pair.Key;

                // It we don't think its deleted, and it had a concurrency exception, try to refresh it
                if (!shipment.DeletedFromDatabase)
                {
                    try
                    {
                        ShippingManager.RefreshShipment(shipment);
                    }
                    catch (ObjectDeletedException)
                    {
                        // We can ignore this for now - we'll deal with the deleted list in a minute
                    }
                }
            }

            // See if we have to tell the user about the errors
            if (errors.Count > 0)
            {
                // See how many are in error from being deleted.
                int deletedCount = errors.Count(p => p.Key.DeletedFromDatabase);

                if (deletedCount == errors.Count)
                {
                    // We actually don't need to show the error message here, b\c when the grid reloads it detects the deleted\removed case and shows the message.
                }
                else
                {
                    MessageHelper.ShowError(this,
                        "The selected shipments were edited or deleted by another ShipWorks user and your changes could not be saved.\n\n" +
                        "The shipments will be refreshed to reflect the recent changes.");
                }
            }

            // Return true if it succeeded completely
            return errors.Count == 0;
        }

        /// <summary>
        /// The second part of the load selected shipments routine, after we have assurred all carrier specific data has been loaded
        /// </summary>
        void LoadSelectedShipmentsCompleted(object sender, BackgroundExecutorCompletedEventArgs<ShipmentEntity> e)
        {
            loadingSelectedShipments = false;

            // Extract userState
            var userState = (Dictionary<string, object>) e.UserState;
            this.loadedShipmentEntities = (List<ShipmentEntity>)userState["loaded"];
            List<ShipmentEntity> deleted = (List<ShipmentEntity>) userState["deleted"];
            bool resortWhenDone = (bool) userState["resortWhenDone"];

            // Go thread each shipment that we loaded and update the corresponding row in the grid with the latest shipment data
            ApplyShipmentsToGridRows(loadedShipmentEntities);

            // We also have to apply deleted information to the grid, so the grid knows which rows are marked for deletion and will need
            // removed on the next RefreshAndResort
            ApplyShipmentsToGridRows(deleted);

            // If they didn't cancel - but the selection is different from what's loaded - then the selection has changed since we started
            // this background loading, and we need to do it again.
            //
            // If they had cancelled, then the selection will be bigger than what was loaded obviously - and the rest of this function
            // will take care of unselecting it.
            if (!e.Canceled && HasSelectionChanged(loadedShipmentEntities))
            {
                LoadSelectedShipments(resortWhenDone);
                return;
            }

            // Turn off selection processing
            shipmentControl.SelectionChanged -= this.OnChangeSelectedShipments;

            // If we cancelled reate a list of all rows that are selected that weren't loaded and unselect them.  There could be selected and notloaded rows for rows
            // that had been deleted too, but those will get wiped out when we do the RefreshAndRestor.
            if (e.Canceled)
            {
                var notLoaded = shipmentControl.SelectedRows.Where(r => !loadedShipmentEntities.Any(s => s.ShipmentID == r.Shipment.ShipmentID)).ToList();

                if (notLoaded.Count > 0)
                {
                    resortWhenDone = true;

                    // Unselected all the rows we ended up not loading
                    foreach (ShipmentGridRow row in notLoaded)
                    {
                        row.Selected = false;
                    }
                }
            }

            // We'll need to resort and refresh if some got deleted or not loaded
            if (deleted.Count > 0)
            {
                resortWhenDone = true;
            }

            // Turn selection processing back on
            shipmentControl.SelectionChanged += this.OnChangeSelectedShipments;

            ShipmentType shipmentType = null;

            bool enableEditing = !loadedShipmentEntities.Any(s => s.Processed);
            
            // To have editing enabled, its also necesary to have permissions for all of them
            if (enableEditing)
            {
                enableEditing = loadedShipmentEntities.All(s => UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, s.OrderID));
            }

            bool enableShippingAddress = enableEditing;

            if (enableEditing && loadedShipmentEntities.Count > 0)
            {
                // Check with the store to see if the shipping address should be editable
                OrderEntity order = DataProvider.GetEntity(loadedShipmentEntities.FirstOrDefault().OrderID) as OrderEntity;
                StoreType storeType = StoreTypeManager.GetType(StoreManager.GetStore(order.StoreID));

                enableShippingAddress = loadedShipmentEntities.All(s => storeType.IsShippingAddressEditable(s));
            }

            comboShipmentType.SelectedIndexChanged -= this.OnChangeShipmentType;

            // See if anything is selected
            if (loadedShipmentEntities.Count == 0)
            {
                comboShipmentType.Enabled = false;
                comboShipmentType.SelectedIndex = -1;
            }
            else
            {
                shipmentType = GetShipmentType(loadedShipmentEntities);

                // Update the shipment type combo
                comboShipmentType.Enabled = enableEditing;

                if (shipmentType != null)
                {
                    // If the selected type is one that's not currently enabled, add it back in so it can be selected
                    List<KeyValuePair<string, ShipmentTypeCode>> enabledTypes = (List<KeyValuePair<string, ShipmentTypeCode>>) comboShipmentType.DataSource;
                    if (!enabledTypes.Any(p => p.Value == shipmentType.ShipmentTypeCode))
                    {
                        enabledTypes.Add(new KeyValuePair<string, ShipmentTypeCode>(shipmentType.ShipmentTypeName, shipmentType.ShipmentTypeCode));
                        SortShipmentTypes(enabledTypes);
                        comboShipmentType.DataSource = enabledTypes.ToList();
                    }

                    comboShipmentType.SelectedValue = shipmentType.ShipmentTypeCode;
                }
                else
                {
                    comboShipmentType.MultiValued = true;
                }
            }

            comboShipmentType.SelectedIndexChanged += this.OnChangeShipmentType;

            // Update our list of shipments that are displayed in the UI
            uiDisplayedShipments = loadedShipmentEntities.ToList();

            // Some of the shipment data is "dynamic".  Like the origin address, if it pulled from the Store Address,
            // and the store address has since changed, we want to update to reflect that.
            foreach (ShipmentEntity shipment in loadedShipmentEntities)
            {
                if (!shipment.Processed && IsShipmentTypeActivatedUI(shipment))
                {
                    ShipmentTypeManager.GetType(shipment).UpdateDynamicShipmentData(shipment);
                }
            }

            // Load the service control with the UI displayed shipments
            LoadServiceControl(loadedShipmentEntities, shipmentType, enableEditing, enableShippingAddress);

            // Load the tracking control
            LoadTrackingDisplay();

            // If there was a setup control, remove it
            ClearPreviousSetupControl();

            // Show the setup control if setup is required.  Dont go by the current value in ShippingManager.IsShipmentTypeSetup - go by the value we used when loading.
            if (shipmentType != null && !IsShipmentTypeActivatedUI(shipmentType.ShipmentTypeCode))
            {
                ShipmentTypeSetupControl setupControl = new ShipmentTypeSetupControl(shipmentType);
                setupControl.SetupComplete += new EventHandler(OnShipmentTypeSetupComplete);

                serviceControlArea.Controls.Add(setupControl);
            }

            // Update the processing \ settings buttons
            UpdateSelectionDependentUI();

            if (deleted.Count > 0)
            {
                MessageHelper.ShowInformation(this, "Some of the shipments you selected were deleted by another user and have been removed from the list.");
            }

            if (resortWhenDone)
            {
                shipmentControl.RefreshAndResort();
            }
        }

        /// <summary>
        /// Indicates if the shipment type was considered "Activated" at the time the last ui selection was loaded.
        /// </summary>
        private bool IsShipmentTypeActivatedUI(ShipmentTypeCode shipmentTypeCode)
        {
            return uiActivatedShipmentTypes.Contains(shipmentTypeCode);
        }

        /// <summary>
        /// Indicates if the shipment type was considered "Activated" at the time the last ui selection was loaded.
        /// </summary>
        private bool IsShipmentTypeActivatedUI(ShipmentEntity shipment)
        {
            return IsShipmentTypeActivatedUI((ShipmentTypeCode) shipment.ShipmentType);
        }

        /// <summary>
        /// Apply the given shipments to their associated rows in the grid.  This is used after cloned shipments were altered somehow in the background thread
        /// and the changes need to be propagated ot the UI.
        /// </summary>
        private void ApplyShipmentsToGridRows(IEnumerable<ShipmentEntity> shipments)
        {
            // Go thread each shipment that we loaded and update the corresponding row in the grid with the latest shipment data
            foreach (ShipmentEntity shipment in shipments)
            {
                ShipmentGridRow row = shipmentControl.FindRow(shipment.ShipmentID);

                // I dont _think_ this should ever be null
                Debug.Assert(row != null, "Could not find shipment row for shipment when applying settings to grid row.");

                if (row != null)
                {
                    // Update the row with the copy of the shipment that was updated in the background
                    row.Shipment = shipment;
                }
            }
        }

        /// <summary>
        /// Determines if the selection has changed from what we loaded
        /// </summary>
        private bool HasSelectionChanged(List<ShipmentEntity> loaded)
        {
            // Determine the current seelction - don't consider deleted, or based on where this function is called we'd end up
            // in a stack overflow.  The deleted ones will get unselected by the processing following this function call in LoadSelectedShipmentsCompleted.
            var selected = shipmentControl.SelectedShipments.Where(s => !s.DeletedFromDatabase).ToDictionary(s => s.ShipmentID, s => true);

            if (selected.Count != loaded.Count)
            {
                return true;
            }

            foreach (ShipmentEntity shipment in loaded)
            {
                if (!selected.ContainsKey(shipment.ShipmentID))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Clear the previous shipment type setup control, if there was one.
        /// </summary>
        private void ClearPreviousSetupControl()
        {
            ShipmentTypeSetupControl setupControl = serviceControlArea.Controls.OfType<ShipmentTypeSetupControl>().SingleOrDefault();
            if (setupControl != null)
            {
                setupControl.SetupComplete -= this.OnShipmentTypeSetupComplete;
                setupControl.Dispose();
            }
        }

        /// <summary>
        /// Setup of the selected shipment type has completed, we can reload the UI
        /// </summary>
        void OnShipmentTypeSetupComplete(object sender, EventArgs e)
        {
            LoadSelectedShipments(false);
        }

        /// <summary>
        /// Update the service control to reflect the currently selected shipment type
        /// </summary>
        private void LoadServiceControl(IEnumerable<ShipmentEntity> shipments, ShipmentType shipmentType, bool enableEditing, bool enableShippingAddress)
        {
            ServiceControlBase newServiceControl = null;

            if (shipments.Any())
            {
                // If its null, its multi select
                if (shipmentType == null)
                {
                    newServiceControl = new MultiSelectServiceControl();
                }

                else if (IsShipmentTypeActivatedUI(shipmentType.ShipmentTypeCode))
                {
                    newServiceControl = shipmentType.CreateServiceControl();
                }

                if (newServiceControl != null)
                {
                    // If the type we need didn't change, then don't change it
                    if (!forceRecreateServiceControl && ServiceControl != null && 
                        ServiceControl.GetType() == newServiceControl.GetType() && 
                        ServiceControl.ShipmentTypeCode == newServiceControl.ShipmentTypeCode)
                    {
                        // Get rid of the one we just created and use the old one
                        newServiceControl.Dispose();
                        newServiceControl = ServiceControl;
                    }
                    else
                    {
                        newServiceControl.Initialize();
                    }
                }
            }

            forceRecreateServiceControl = false;

            // If there is a service control, load the data into it before making it visible
            if (newServiceControl != null)
            {
                newServiceControl.LoadShipments(shipments, enableEditing, enableShippingAddress);
            }

            // See if the service control has changed
            if (ServiceControl != newServiceControl)
            {
                ServiceControlBase oldServiceControl = ServiceControl;
                Control reduceFlash = null;

                // If there was not an old service control, create a blank panel that will cover our new service control
                // while it's controls are being positioned
                if (oldServiceControl == null)
                {
                    reduceFlash = new Panel();
                    reduceFlash.Dock = DockStyle.Fill;
                    serviceControlArea.Controls.Add(reduceFlash);
                }

                // If there was a setup control, remove it
                ClearPreviousSetupControl();

                // If there is a new service control, add it to our controls under either the old one, or the blank panel we created.
                if (newServiceControl != null)
                {
                    newServiceControl.RecipientDestinationChanged += this.OnRecipientDestinationChanged;
                    newServiceControl.ShipmentServiceChanged += this.OnShipmentServiceChanged;
                    newServiceControl.RateCriteriaChanged += this.OnRateCriteriaChanged;
                    newServiceControl.ShipmentsAdded += this.OnServiceControlShipmentsAdded;
                    newServiceControl.ShipmentTypeChanged += this.OnShipmentTypeChanged;
                    newServiceControl.ClearRatesAction = ClearRates;
                    rateControl.RateSelected += newServiceControl.OnRateSelected;

                    newServiceControl.Dock = DockStyle.Fill;
                    serviceControlArea.Controls.Add(newServiceControl);
                }

                // Finally, remove the old service control, or the blank panel we created
                if (oldServiceControl != null)
                {
                    oldServiceControl.RecipientDestinationChanged -= this.OnRecipientDestinationChanged;
                    oldServiceControl.ShipmentServiceChanged -= this.OnShipmentServiceChanged;
                    oldServiceControl.RateCriteriaChanged -= this.OnRateCriteriaChanged;
                    oldServiceControl.ShipmentsAdded -= this.OnServiceControlShipmentsAdded;
                    oldServiceControl.ShipmentTypeChanged -= OnShipmentTypeChanged;
                    oldServiceControl.ClearRatesAction = x => { };
                    rateControl.RateSelected -= oldServiceControl.OnRateSelected;

                    oldServiceControl.Dispose();
                }
                else
                {
                    reduceFlash.Dispose();
                }
            }

            // Update the custom's control
            LoadCustomsControl(shipments, shipmentType, enableEditing);

            // Update the displayed rates
            LoadDisplayedRates();
        }

        /// <summary>
        /// Clear the rates from the grid
        /// </summary>
        private void ClearRates(string reason)
        {
            rateControl.ClearRates(reason);
            shipmentRateMap.Clear();
        }

        /// <summary>
        /// When BestRate Rate is selected, the BestRateServiceControl raises this event.
        /// </summary>
        private void OnShipmentTypeChanged(object sender, EventArgs e)
        {
            ShipmentEntity shipment = shipmentControl.SelectedShipments.First();
            ShipmentTypeCode shipmentTypeCode = ((ShipmentTypeCode)shipment.ShipmentType);

            comboShipmentType.SelectedValue = shipmentTypeCode;
        }

        /// <summary>
        /// Load the tracking data from the selected shipments into the control
        /// </summary>
        private void LoadTrackingDisplay()
        {
            trackingControl.Clear();
            trackingControl.Visible = false;

            if (uiDisplayedShipments.Count == 0)
            {
                panelTrackingData.Visible = false;
                panelTrackingMessage.Visible = false;
            }
            else if (uiDisplayedShipments.Count > 1)
            {
                panelTrackingData.Visible = false;
                panelTrackingMessage.Visible = true;

                labelTrackingMessage.Text = "Multiple shipments are selected.  Select a single shipment to view tracking information.";
            }
            else
            {
                ShipmentEntity shipment = uiDisplayedShipments[0];

                if (!shipment.Processed)
                {
                    panelTrackingData.Visible = false;
                    panelTrackingMessage.Visible = true;

                    labelTrackingMessage.Text = "The selected shipment has not been processed.";
                }
                else
                {
                    panelTrackingData.Visible = true;
                    panelTrackingMessage.Visible = false;

                    trackingProcessedDate.Text = shipment.ProcessedDate.Value.ToLocalTime().ToString("M/dd/yyy h:mm tt");
                    trackingCost.Text = shipment.ShipmentCost.ToString("c");

                    ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
                    List<string> trackingList = shipmentType.GetTrackingNumbers(shipment);

                    trackingNumbers.Height = 20 + (trackingList.Count - 1) * 14;
                    trackingNumbers.Lines = trackingList.ToArray();

                    trackingControl.Top = trackingNumbers.Bottom + 5;
                    trackingControl.Height = panelTrackingData.Height - 5 - trackingControl.Top;

                    if (initialDisplayTracking)
                    {
                        OnTrack(track, EventArgs.Empty);
                    }
                }
            }

            initialDisplayTracking = false;
        }

        /// <summary>
        /// Load the rates displayed in the Rates section
        /// </summary>
        private void LoadDisplayedRates()
        {
            if (ServiceControl != null)
            {
                RateGroup rateGroup = null;

                if (uiDisplayedShipments.Count == 1)
                {
                    ShipmentEntity uiShipment = uiDisplayedShipments[0];
                    rateGroup = GetCachedRates(uiShipment);
                }

                this.LoadRates(rateGroup);
            }
        }

        /// <summary>
        /// Loads the rates.
        /// </summary>
        /// <param name="rateGroup">The rate group.</param>
        public void LoadRates(RateGroup rateGroup)
        {
            if (this.loadedShipmentEntities.Count > 1)
            {
                rateControl.ClearRates("(Multiple)");
            }
            else
            {
                if (rateGroup == null)
                {
                    if (this.loadedShipmentEntities.Count == 1 && this.loadedShipmentEntities[0].Processed)
                    {
                        rateControl.ClearRates("The shipment has already been processed.");
                    }
                    else
                    {
                        rateControl.ClearRates("Click 'Get Rates'.");
                    }
                }
                else if (rateGroup.Rates.Count == 0)
                {
                    rateControl.ClearRates("No rates are available for the shipment.", rateGroup);
                }
                else
                {
                    rateControl.LoadRates(rateGroup);
                }
            }
        }

        /// <summary>
        /// The selected shipment(s) has been edited in a such a way that the rates are no longer valid.
        /// </summary>
        void OnRateCriteriaChanged(object sender, EventArgs e)
        {
            // Since this could change if customs needs generated, we need to save right now.
            SaveChangesToUIDisplayedShipments();

            // Shipping rate changes will also affect insurace rates
            UpdateInsuranceDisplay();

            foreach (ShipmentEntity shipment in uiDisplayedShipments)
            {
                Dictionary<ShipmentTypeCode, RateGroup> rateMap;
                if (shipmentRateMap.TryGetValue(shipment.ShipmentID, out rateMap))
                {
                    foreach (RateGroup group in rateMap.Values)
                    {
                        group.OutOfDate = true;
                    }
                }
            }

            LoadDisplayedRates();
        }

        /// <summary>
        /// A full reload of rates is required
        /// </summary>
        void OnRateReloadRequired(object sender, EventArgs e)
        {
            // Clear out all of the rates, whether they are selected or not, since we know they need reloaded
            shipmentRateMap.Clear();

            // Then go ahead and do a Get Rates and whatever is selected
            if (uiDisplayedShipments.Count > 0)
            {
                BeginInvoke(new MethodInvoker<object, EventArgs>(OnGetRates), this, e);
            }
        }

        /// <summary>
        /// A rate was selected on the rate grid
        /// </summary>
        void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            
        }

        /// <summary>
        /// Update the display of our insurance rates for the given shipments
        /// </summary>
        private void UpdateInsuranceDisplay()
        {
            if (ServiceControl != null)
            {
                ServiceControl.UpdateInsuranceDisplay();
            }
        }

        /// <summary>
        /// Update the custom's control with the given shipments, controlling if it can be edited or not
        /// </summary>
        private void LoadCustomsControl(IEnumerable<ShipmentEntity> shipments, ShipmentType shipmentType, bool enableEditing)
        {
            CustomsControlBase newCustomsControl = null;

            if (shipments.Any())
            {
                // If its null, its multi select
                if (shipmentType == null || !IsShipmentTypeActivatedUI(shipmentType.ShipmentTypeCode))
                {
                    newCustomsControl = new CustomsControlBase();
                }

                else
                {
                    newCustomsControl = shipmentType.CreateCustomsControl();
                }

                // If the type we need didn't change, then don't change it
                if (CustomsControl != null && CustomsControl.GetType() == newCustomsControl.GetType())
                {
                    newCustomsControl = CustomsControl;
                }
                else
                {
                    newCustomsControl.Initialize();
                }
            }

            // If there is a service control, load the data into it before making it visible
            if (newCustomsControl != null)
            {
                newCustomsControl.LoadShipments(shipments, enableEditing);
            }

            // See if the customs control has changed
            if (CustomsControl != newCustomsControl)
            {
                CustomsControlBase oldCustomsControl = CustomsControl;
                Control reduceFlash = null;

                // If there was not an old service control, create a blank panel that will cover our new service control
                // while it's controls are being positioned
                if (oldCustomsControl == null)
                {
                    reduceFlash = new Panel();
                    reduceFlash.Dock = DockStyle.Fill;
                    customsControlArea.Controls.Add(reduceFlash);
                }

                // If there is a new service control, add it to our controls under either the old one, or the blank panel we created.
                if (newCustomsControl != null)
                {
                    newCustomsControl.Dock = DockStyle.Fill;
                    customsControlArea.Controls.Add(newCustomsControl);
                }

                // Finally, remove the old service control, or the blank panel we created
                if (oldCustomsControl != null)
                {
                    oldCustomsControl.Dispose();
                }
                else
                {
                    reduceFlash.Dispose();
                }
            }

            UpdateCustomsDisplay(shipments);
        }

        /// <summary>
        /// Update the custom's control display
        /// </summary>
        private void UpdateCustomsDisplay(IEnumerable<ShipmentEntity> shipments)
        {
            bool anyNeedCustoms = shipments.Any(s => CustomsManager.IsCustomsRequired(s));

            if (!anyNeedCustoms && tabControl.Contains(tabPageCustoms))
            {
                tabControl.TabPages.Remove(tabPageCustoms);
            }

            if (anyNeedCustoms && !tabControl.Contains(tabPageCustoms))
            {
                // Insert after the main tab, but before Tracking
                tabControl.TabPages.Insert(1, tabPageCustoms);
            }
        }

        /// <summary>
        /// The user has changed the selected destination country\state changed
        /// </summary>
        void OnRecipientDestinationChanged(object sender, EventArgs e)
        {
            // Since this could change if customs needs generated, we need to save right now.
            bool success = SaveUIDisplayedShipments();

            if (success)
            {
                // Update the customs display
                UpdateCustomsDisplay(uiDisplayedShipments);

                // If the customs tab is now (or is still) visible, load the shipments into it
                if (tabControl.TabPages.Contains(tabPageCustoms) && CustomsControl != null)
                {
                    CustomsControl.LoadShipments(uiDisplayedShipments, true);
                }
            }
            else
            {
                // If we didn't successfully save, we need to reload the grid completely instead of just updating the customs display.
                // Don't want to do it directly, as we're in the middle of a change event - and reloading the UI out from undering could 
                // relaly goof that up.
                BeginInvoke(new MethodInvoker(() => LoadSelectedShipments(true)));                
            }
        }

        /// <summary>
        /// Called when [shipment service changed].
        /// </summary>
        private void OnShipmentServiceChanged(object sender, EventArgs e)
        {
            if (tabControl.Contains(tabPageCustoms))
            {
                UpdateCustomsDisplay(uiDisplayedShipments);

                CustomsControl.LoadShipments(uiDisplayedShipments, true);
            }
        }

        /// <summary>
        /// A tab is about to be selected
        /// </summary>
        private void OnTabDeselecting(object sender, TabControlCancelEventArgs e)
        {
            // If switching from the services tab, we want to save the weight from the services tab so that if the weight changes
            // on the customs tab, we are able to know.
            if (tabControl.SelectedTab == tabPageService)
            {
                if (ServiceControl != null)
                {
                    ServiceControl.SaveToShipments();
                }
            }

            // If switching from the customs tab, we have to save the customs stuff and load the service stuff, since
            // changing customs weights can alter the shipment weight
            if (tabControl.SelectedTab == tabPageCustoms)
            {
                if (CustomsControl != null)
                {
                    CustomsControl.SaveToShipments();

                    if (ServiceControl != null)
                    {
                        ServiceControl.RefreshContentWeight();
                    }
                }
            }
        }

        /// <summary>
        /// Save any changes to the current set of shipment.  In memory only.
        /// </summary>
        private void SaveChangesToUIDisplayedShipments()
        {
            List<ShipmentEntity> shipments = uiDisplayedShipments.Where(s => !s.DeletedFromDatabase).ToList();

            if (shipments.Count == 0)
            {
                return;
            }

            if (CustomsControl != null)
            {
                CustomsControl.SaveToShipments();

                if (tabControl.SelectedTab == tabPageCustoms)
                {
                    if (ServiceControl != null)
                    {
                        ServiceControl.RefreshContentWeight();
                    }
                }
            }

            if (ServiceControl != null)
            {
                ServiceControl.SaveToShipments();
            }

            foreach (ShipmentEntity shipment in shipments)
            {
                if (!shipment.Processed)
                {
                    // Update the total shipment weight and any dynamic data changes
                    if (IsShipmentTypeActivatedUI(shipment))
                    {
                        ShipmentTypeManager.GetType(shipment).UpdateDynamicShipmentData(shipment);
                        ShipmentTypeManager.GetType(shipment).UpdateTotalWeight(shipment);
                    }

                    // If the there is a specific shiment type selected, set it
                    if (!comboShipmentType.MultiValued)
                    {
                        shipment.ShipmentType = (int) (ShipmentTypeCode) comboShipmentType.SelectedValue;
                    }
                }
            }
        }

        /// <summary>
        /// Persist each dirty shipment in the list to the database.  If any concurrency errors occur, the offending shipments are returned.  The rest are
        /// still saved.
        /// </summary>
        private Dictionary<ShipmentEntity, Exception> SaveShipmentsToDatabase(IEnumerable<ShipmentEntity> shipments, bool forceSave)
        {
            if (shipments.Any())
            {
                Cursor.Current = Cursors.WaitCursor;
            }

            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();

            foreach (ShipmentEntity shipment in shipments)
            {
                try
                {
                    if (forceSave)
                    {
                        // Force the shipment to look dirty to its forced to save.  This is to make sure that if any other
                        // changes had been made by other users we pick up the concurrency violation.
                        if (!shipment.IsDirty)
                        {
                            shipment.Fields[(int) ShipmentFieldIndex.ShipmentType].IsChanged = true;
                            shipment.Fields.IsDirty = true;
                        }
                    }

                    ShippingManager.SaveShipment(shipment);
                }
                catch (ObjectDeletedException ex)
                {
                    errors[shipment] = ex;
                }
                catch (SqlForeignKeyException ex)
                {
                    errors[shipment] = ex;
                }
                catch (ORMConcurrencyException ex)
                {
                    errors[shipment] = ex;
                }
            }

            return errors;
        }

        /// <summary>
        /// Open the profiles manager
        /// </summary>
        private void OnProfiles(object sender, EventArgs e)
        {
            using (ShippingProfileManagerDlg dlg = new ShippingProfileManagerDlg(null))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Opening the profiles menu
        /// </summary>
        private void OnOpeningProfilesMenu(object sender, CancelEventArgs e)
        {
            contextMenuProfiles.Items.Clear();

            ShipmentTypeCode? shipmentTypeCode = comboShipmentType.MultiValued ? (ShipmentTypeCode?) null :
                (ShipmentTypeCode) comboShipmentType.SelectedValue;

            // Add each relevant profile
            if (shipmentTypeCode != null)
            {
                foreach (ShippingProfileEntity profile in ShippingProfileManager.Profiles.OrderBy(p => p.ShipmentTypePrimary ? "zzzzz" : p.Name))
                {
                    if (profile.ShipmentType != (int) ShipmentTypeCode.None &&
                        profile.ShipmentType == (int) shipmentTypeCode.Value)
                    {
                        ToolStripMenuItem menuItem = new ToolStripMenuItem(profile.Name);
                        menuItem.Tag = profile;
                        menuItem.Click += new EventHandler(OnApplyProfile);

                        if (profile.ShipmentTypePrimary && contextMenuProfiles.Items.Count > 0)
                        {
                            contextMenuProfiles.Items.Add(new ToolStripSeparator());
                        }

                        contextMenuProfiles.Items.Add(menuItem);
                    }
                }
            }

            if (contextMenuProfiles.Items.Count == 0)
            {
                contextMenuProfiles.Items.Add(new ToolStripMenuItem { Text = "(None)", Enabled = false });
            }

            // Add a menu item to open the shipping profile manager dialog
            contextMenuProfiles.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem manageProfilesMenuItem = new ToolStripMenuItem("Manage Profiles...");
            manageProfilesMenuItem.Click += OnProfiles;
            contextMenuProfiles.Items.Add(manageProfilesMenuItem);

        }

        /// <summary>
        /// User has selected to apply a profile to the selected shipments
        /// </summary>
        void OnApplyProfile(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
            ShippingProfileEntity profile = (ShippingProfileEntity) menuItem.Tag;

            // Save any changes that have been made thus far, so the profile changes can be made on top of that
            SaveChangesToUIDisplayedShipments();

            // Apply the profile to each ui displayed shipmetn
            foreach (ShipmentEntity shipment in uiDisplayedShipments)
            {
                if (!shipment.Processed && IsShipmentTypeActivatedUI(shipment))
                {
                    ShippingProfileManager.ApplyProfile(shipment, profile);
                }
            }

            // Reload the UI to show the changes
            LoadSelectedShipments(true);
        }

        /// <summary>
        /// Update the UI components that depend on the current selection
        /// </summary>
        private void UpdateSelectionDependentUI()
        {
            bool canApplyProfile = false;
            bool canGetRates = false;
            bool canPrint = false;
            bool canVoid = false;

            // Determine if there is security allowed for all of them
            bool securityCreateEditProcess = uiDisplayedShipments.All(s => UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, s.Order.OrderID));
            bool securityVoidDelete = uiDisplayedShipments.All(s => UserSession.Security.HasPermission(PermissionType.ShipmentsVoidDelete, s.Order.OrderID));


            // Check each shipment
            foreach (ShipmentEntity shipment in uiDisplayedShipments)
            {
                if (IsShipmentTypeActivatedUI(shipment) && shipment.ShipmentType != (int) ShipmentTypeCode.None)
                {
                    if (!shipment.Processed && securityCreateEditProcess)
                    {
                        canApplyProfile = true;
                    }

                    if (!shipment.Processed && ShipmentTypeManager.GetType(shipment).SupportsGetRates)
                    {
                        canGetRates = true;
                    }

                    if (shipment.Processed && !shipment.Voided)
                    {
                        canPrint = true;
                    }

                    if (shipment.Processed && !shipment.Voided && securityVoidDelete)
                    {
                        canVoid = true;
                    }
                }
            }

            // Update enable state
            processDropDownButton.Enabled = securityCreateEditProcess;
            applyProfile.Enabled = canApplyProfile;
            getRates.Enabled = canGetRates;
            ratesSplitContainer.Panel2Collapsed = !canGetRates;
            print.Enabled = canPrint;
            voidSelected.Enabled = canVoid;

            Update();
        }

        /// <summary>
        /// Update the security availability of the edit controls
        /// </summary>
        private void UpdateEditControlsSecurity()
        {
            bool canEdit;
            bool canVoid;

            if (shipmentControl.AllRows.Any())
            {
                canEdit = shipmentControl.AllRows.Any(r => UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, r.EntityID));
                canVoid = shipmentControl.AllRows.Any(r => UserSession.Security.HasPermission(PermissionType.ShipmentsVoidDelete, r.EntityID));
            }
            else
            {
                canEdit = StoreManager.GetAllStores().Any(s => UserSession.Security.HasPermission(PermissionType.ShipmentsCreateEditProcess, s.StoreID));
                canVoid = StoreManager.GetAllStores().Any(s => UserSession.Security.HasPermission(PermissionType.ShipmentsVoidDelete, s.StoreID));
            }

            voidSelected.Visible = canVoid;
            panelEditButtons.Visible = canEdit;

            if (canEdit)
            {
                splitContainer.Width = panelEditButtons.Left - splitContainer.Left;
            }
            else
            {
                splitContainer.Width = panelEditButtons.Right - splitContainer.Left;
            }

            splitContainer.Refresh();
        }

        /// <summary>
        /// Shipments were created in the service control and need to be loaded into the grid
        /// </summary>
        private void OnServiceControlShipmentsAdded(object sender, ShipmentsAddedRemovedEventArgs e)
        {
            shipmentControl.AddShipments(e.Shipments);
            shipmentControl.SelectShipments(e.Shipments);
        }

        /// <summary>
        /// Shipments have been added to the grid
        /// </summary>
        private void OnShipmentsAdded(object sender, EventArgs e)
        {
            UpdateEditControlsSecurity();
        }

        /// <summary>
        /// Shipments have been removed from the grid
        /// </summary>
        private void OnShipmentsRemoved(object sender, EventArgs e)
        {
            UpdateEditControlsSecurity();
        }

        /// <summary>
        /// Get the selected ShipmentType, or null if multipe
        /// </summary>
        private ShipmentType GetShipmentType(IEnumerable<ShipmentEntity> shipments)
        {
            int typeCode = -1;

            foreach (ShipmentEntity shipment in shipments)
            {
                // First one
                if (typeCode == -1)
                {
                    typeCode = shipment.ShipmentType;
                }
                else
                {
                    if (typeCode != shipment.ShipmentType)
                    {
                        return null;
                    }
                }
            }

            if (typeCode == -1)
            {
                return null;
            }

            return ShipmentTypeManager.GetType((ShipmentTypeCode) typeCode);
        }

        /// <summary>
        /// Print labels for the selected shipments
        /// </summary>
        private void OnPrint(object sender, EventArgs e)
        {
            BackgroundExecutor<ShipmentEntity> executor = new BackgroundExecutor<ShipmentEntity>(this,
                "Print Shipments",
                "ShipWorks is printing labels.",
                "Printing {0} of {1}");

            // We are prepared for exceptions
            executor.PropagateException = true;

            // What to do before it gets started (but is on the background thread)
            executor.ExecuteStarting += (object s, EventArgs args) =>
                {
                    // Make sure all filters are up to date since the determination of what to print is based on filters
                    FilterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));
                };

            // Some of the printing will be delayed b\c we are waiting for label sheets to fill up
            Dictionary<TemplateEntity, List<long>> delayedPrints = new Dictionary<TemplateEntity, List<long>>(TemplateHelper.TemplateEqualityComparer);

            // Executes right after things finish - but still on the background thread
            executor.ExecuteCompleting += (object s, EventArgs args) =>
                {
                    foreach (KeyValuePair<TemplateEntity, List<long>> pair in delayedPrints)
                    {
                        PrintJob printJob = PrintJob.Create(pair.Key, pair.Value);
                        printJob.Print();
                    }
                };

            // Executes after everyting is totally done, and is on the UI thread
            executor.ExecuteCompleted += (object s, BackgroundExecutorCompletedEventArgs<ShipmentEntity> args) =>
                {
                    if (args.ErrorException != null)
                    {
                        if (args.ErrorException is PrintingException)
                        {
                            if (!(args.ErrorException is PrintingNoTemplateOutputException))
                            {
                                MessageHelper.ShowError(this, args.ErrorException.Message);
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException(args.ErrorException.Message, args.ErrorException);
                        }
                    }
                };

            executor.ExecuteAsync(

                // What to do for each shipment
                (ShipmentEntity shipment, object state, BackgroundIssueAdder<ShipmentEntity> issueAdder) =>
                {
                    List<TemplateEntity> templates = ShipmentPrintHelper.DetermineTemplatesToPrint(shipment);

                    // Print with each template
                    foreach (TemplateEntity template in templates)
                    {
                        // If it's standard or thermal we can print it right away
                        if (template.Type == (int) TemplateType.Standard || template.Type == (int) TemplateType.Thermal)
                        {
                            PrintJob printJob = PrintJob.Create(template, new List<long> { shipment.ShipmentID });
                            printJob.Print();
                        }
                        else
                        {
                            // Get the list of keys that have been delayed so far for this template
                            List<long> delayedKeys;
                            if (!delayedPrints.TryGetValue(template, out delayedKeys))
                            {
                                delayedKeys = new List<long>();
                                delayedPrints[template] = delayedKeys;
                            }

                            // Add this as a delayed key
                            delayedKeys.Add(shipment.ShipmentID);

                            // If its a report just queue it up until the end
                            if (template.Type == (int) TemplateType.Report)
                            {

                            }

                            // It must be a label template
                            if (template.Type == (int) TemplateType.Label)
                            {
                                LabelSheetEntity labelSheet = LabelSheetManager.GetLabelSheet(template.LabelSheetID);
                                if (labelSheet != null)
                                {
                                    int cells = labelSheet.Rows * labelSheet.Columns;

                                    // To know how many cell's we'll use, we have to translate
                                    int inputs = TemplateContextTranslator.Translate(delayedKeys, template).Count;

                                    // If we have enough to fill a sheet, print now
                                    if (inputs % cells == 0)
                                    {
                                        PrintJob printJob = PrintJob.Create(template, delayedKeys);
                                        printJob.Print();

                                        delayedPrints.Remove(template);
                                    }
                                }
                                else
                                {
                                    delayedPrints.Remove(template);
                                }
                            }
                        }
                    }
                },

                // Each shipment to print.  Send in a cloned collection so changes on other threads don't affect it.
                EntityUtility.CloneEntityCollection(shipmentControl.SelectedShipments.Where(s => !s.DeletedFromDatabase && s.Processed && !s.Voided))
            
            );
        }

        /// <summary>
        /// Get rates for the selected shipments
        /// </summary>
        private void OnGetRates(object sender, EventArgs e)
        {
            GetRates();
        }

        /// <summary>
        /// Gets rates for the selected shipments
        /// </summary>
        private void GetRates()
        {
            // Save changes to the current selection (NOT the ones we are processing) before we process it
            SaveChangesToUIDisplayedShipments();

            BackgroundExecutor<ShipmentEntity> executor = new BackgroundExecutor<ShipmentEntity>(this,
                "Get Rates",
                "ShipWorks is getting shipping rates.",
                "Getting {0} of {1}");

            List<string> newErrors = new List<string>();
            bool noRates = false;
            bool anyAttempted = false;

            // The list of shipments to get rates for.  A cloned collection to changes in the background don't have thread issues with the foreground
            List<ShipmentEntity> shipments = EntityUtility.CloneEntityCollection(uiDisplayedShipments);

            // What to do when done.  Runs on the UI thread.
            executor.ExecuteCompleted += (_sender, _e) =>
            {
                if (anyAttempted)
                {
                    // This is not necessary since we reload completely anyway, but it reduces the percieved load time by getting these displayed ASAP
                    LoadDisplayedRates();

                    // Apply any changes made during processing to the grid
                    ApplyShipmentsToGridRows(shipments);
                }

                if (newErrors.Count > 0)
                {
                    string message = "Some errors occurred while getting rates.\n\n";

                    foreach (string error in newErrors.Take(3))
                    {
                        message += error + "\n\n";
                    }

                    if (newErrors.Count > 3)
                    {
                        message += "See the shipment list for all errors.";
                    }

                    MessageHelper.ShowError(this, message);
                }
                else if (noRates)
                {
                    MessageHelper.ShowInformation(this,
                        "Rates could not be obtained for every shipment.\n\nThis could be because the shipment is already processed or " +
                        "getting rates for the selected carrier is not supported.");
                }

                if (anyAttempted)
                {
                    LoadSelectedShipments(true);
                }
            };

            // What to do for each shipment
            executor.ExecuteAsync((ShipmentEntity shipment, object state, BackgroundIssueAdder<ShipmentEntity> issueAdder) =>
            {
                ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

                // Don't attempt to use cached rates if just one is selected.  If multi are selected, then we'll only get them if we need to for effeciency.  But
                // if you are looking at a specific order, we'll get them every time you ask.  I think this is a good way to do it.  If better logic presents itself
                // for a different way, im open.
                if (shipments.Count > 1)
                {
                    RateGroup cachedRates = GetCachedRates(shipment);
                    if (cachedRates != null && !cachedRates.OutOfDate)
                    {
                        return;
                    }
                }

                if (shipment.Processed || !shipmentType.SupportsGetRates || !IsShipmentTypeActivatedUI(shipment))
                {
                    noRates = true;
                    return;
                }

                try
                {
                    anyAttempted = true;

                    Dictionary<ShipmentTypeCode, RateGroup> rateMap;
                    if (!shipmentRateMap.TryGetValue(shipment.ShipmentID, out rateMap))
                    {
                        rateMap = new Dictionary<ShipmentTypeCode, RateGroup>();
                        shipmentRateMap[shipment.ShipmentID] = rateMap;
                    }

                    RateGroup rateResults = ShippingManager.GetRates(shipment);

                    rateMap[shipmentType.ShipmentTypeCode] = rateResults;

                    // Just in case it used to have an error remove it
                    processingErrors.Remove(shipment.ShipmentID);
                }
                catch (ShippingException ex)
                {
                    newErrors.Add("Order " + shipment.Order.OrderNumberComplete + ": " + ex.Message);
                    processingErrors[shipment.ShipmentID] = ex;
                }
            },

                // The shipments to get rates for.
                shipments);
        }

        /// <summary>
        /// Get the cached rates for the shipment, or null if there are none
        /// </summary>
        private RateGroup GetCachedRates(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
            if (shipmentType.SupportsGetRates)
            {
                Dictionary<ShipmentTypeCode, RateGroup> rateMap;
                if (shipmentRateMap.TryGetValue(shipment.ShipmentID, out rateMap))
                {
                    RateGroup rateGroup;
                    if (rateMap.TryGetValue(shipmentType.ShipmentTypeCode, out rateGroup))
                    {
                        return rateGroup;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Void the selected shipments that are processed, and have not yet been already voided.
        /// </summary>
        private void OnVoid(object sender, EventArgs e)
        {
            // Confirm they want to void
            using (ShipmentVoidConfirmDlg dlg = new ShipmentVoidConfirmDlg())
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
            }

            // Save changes to the current selection in memory.  We save to the database later on a per-shipment basis in the background thread.
            SaveChangesToUIDisplayedShipments();

            // Filter out the ones we know to be already processed, or are not ready. We need to process a copy of the shipments, since its going 
            // to be on the background thread.  Otherwise the UI could try to draw at the exact same time a shipment was being edited or saved.
            var shipments = EntityUtility.CloneEntityCollection(shipmentControl.SelectedShipments.Where(s => s.Processed && !s.Voided));

            // Clear all errors before starting the voiding process.  We'll just show the errors that happen during voiding.
            processingErrors.Clear();

            BackgroundExecutor<ShipmentEntity> executor = new BackgroundExecutor<ShipmentEntity>(this,
                "Voiding Shipments",
                "ShipWorks is voiding the shipments.",
                "Shipment {0} of {1}");

            List<string> newErrors = new List<string>();

            // Code to execute once background load is complete
            executor.ExecuteCompleted += (s, args) =>
            {
                // Apply the edits that were made in the background to the grid rows
                ApplyShipmentsToGridRows(shipments);

                // This clears out all the deleted ones
                shipmentControl.SelectionChanged -= this.OnChangeSelectedShipments;
                shipmentControl.RefreshAndResort();
                shipmentControl.SelectionChanged += this.OnChangeSelectedShipments;

                if (newErrors.Count > 0)
                {
                    string message = "Some errors occurred during voiding.\n\n";

                    foreach (string error in newErrors.Take(3))
                    {
                        message += error + "\n\n";
                    }

                    if (newErrors.Count > 3)
                    {
                        message += "See the shipment list for all errors.";
                    }

                    MessageHelper.ShowError(this, message);
                }

                LoadSelectedShipments(true);
            };


            // Code to execute for each shipment
            executor.ExecuteAsync((ShipmentEntity shipment, object state, BackgroundIssueAdder<ShipmentEntity> issueAdder) =>
            {
                long shipmentID = shipment.ShipmentID;
                string errorMessage = null;

                try
                {
                    // Process it
                    ShippingManager.VoidShipment(shipmentID);

                    // Clear any previous errors
                    processingErrors.Remove(shipmentID);
                }
                catch (ORMConcurrencyException ex)
                {
                    errorMessage = "Another user had recently made changes, so the shipment was not voided.";
                    processingErrors[shipmentID] = new ShippingException(errorMessage, ex);
                }
                catch (ObjectDeletedException ex)
                {
                    errorMessage = "The shipment has been deleted.";
                    processingErrors[shipmentID] = new ShippingException(errorMessage, ex);
                }
                catch (SqlForeignKeyException ex)
                {
                    errorMessage = "The shipment has been deleted.";
                    processingErrors[shipmentID] = new ShippingException(errorMessage, ex);
                }
                catch (ShippingException ex)
                {
                    errorMessage = ex.Message;
                    processingErrors[shipmentID] = ex;
                }

                try
                {
                    // Reload it so we can show the most recent data when the grid redisplays
                    ShippingManager.RefreshShipment(shipment);
                }
                catch (ObjectDeletedException ex)
                {
                    // If there wasn't already a different error, set this as the error
                    if (errorMessage == null)
                    {
                        errorMessage = "The shipment has been deleted.";
                        processingErrors[shipmentID] = new ShippingException(errorMessage, ex);
                    }
                }

                if (errorMessage != null)
                {
                    newErrors.Add("Order " + shipment.Order.OrderNumberComplete + ": " + errorMessage);
                }
            },

            // Each shipment to execute the code for
            shipments);
        }

        /// <summary>
        /// Process selected shipments
        /// </summary>
        private void OnProcessSelected(object sender, EventArgs e)
        {
            if (shipmentControl.SelectedShipments.Any())
            {
                Process(shipmentControl.SelectedShipments);
            }
            else
            {
                MessageHelper.ShowError(this, "At least one shipment must be selected.");
            }
        }

        /// <summary>
        /// Process all shipments
        /// </summary>
        private void OnProcessAll(object sender, EventArgs e)
        {
            Process(shipmentControl.AllRows.Select(r => r.Shipment));
        }

        /// <summary>
        /// Process the given list of shipments
        /// </summary>
        private void Process(IEnumerable<ShipmentEntity> shipments)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Save changes to the current selection in memory.  We save to the database later on a per-shipment basis in the background thread.
            SaveChangesToUIDisplayedShipments();

            // Clear errors on ones that are already marked as processed. Like if they got the AlreadyProcessed error, and then hit process again,
            // the error shouldnt stay
            foreach (ShipmentEntity shipment in shipments.Where(s => s.Processed))
            {
                processingErrors.Remove(shipment.ShipmentID);
            }

            // Filter out the ones we know to be already processed, or are not ready
            shipments = shipments.Where(s => !s.Processed && IsShipmentTypeActivatedUI(s) && s.ShipmentType != (int) ShipmentTypeCode.None);

            // Create clones to be processsed - that way any changes made don't have race conditions with the UI trying to paint with them
            shipments = EntityUtility.CloneEntityCollection(shipments);

            if (!shipments.Any())
            {
                MessageHelper.ShowMessage(this, "There are no shipments to process.");
                return;
            }

            // Check restriction
            if (!EditionManager.HandleRestrictionIssue(this, EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.SelectionLimit, shipments.Count())))
            {
                return;
            }

            BackgroundExecutor<ShipmentEntity> executor = new BackgroundExecutor<ShipmentEntity>(this,
                "Processing Shipments",
                "ShipWorks is processing the shipments.",
                "Shipment {0} of {1}");

            List<string> newErrors = new List<string>();
            Dictionary<ShipmentEntity, Exception> concurrencyErrors = new Dictionary<ShipmentEntity, Exception>();

            // Special cases - I didn't think we needed to abstract these.  If it gets out of hand we should refactor.
            bool worldshipExported = false;
            EndiciaInsufficientFundsException endiciaOutOfFundsEx = null;

            // Maps storeID's to license exceptions, so we only have to check a store once per processing batch
            Dictionary<long, Exception> licenseCheckResults = new Dictionary<long, Exception>();

            // What to do before it gets started (but is on the background thread)
            executor.ExecuteStarting += (object s, EventArgs args) =>
            {
                // Force the shipments to save - this weeds out any shipments early that have been edited by another user on another computer.
                concurrencyErrors = SaveShipmentsToDatabase(shipments, true);

                // Reset to true, so that we show the counter rate setup wizard for this batch.
                showCounterRateSetupWizard = true;
            };

            // Code to execute once background load is complete
            executor.ExecuteCompleted += (sender, e) =>
            {
                // Apply any changes made during processing to the grid
                ApplyShipmentsToGridRows(shipments);

                // This clears out all the deleted ones
                shipmentControl.SelectionChanged -= this.OnChangeSelectedShipments;
                shipmentControl.RefreshAndResort();
                shipmentControl.SelectionChanged += this.OnChangeSelectedShipments;

                // If any accounts were out of funds we show that instead of the errors
                if (endiciaOutOfFundsEx != null)
                {
                    string provider = (endiciaOutOfFundsEx.Account.EndiciaReseller == (int) EndiciaReseller.None) ? "Endicia" : "Express1";

                    DialogResult answer = MessageHelper.ShowQuestion(this,
                        string.Format("You do not have sufficient funds in {0} account #{1} to continue shipping.\n\n" +
                        "Would you like to purchase more now?", provider, endiciaOutOfFundsEx.Account.AccountNumber));

                    if (answer == DialogResult.OK)
                    {
                        using (EndiciaBuyPostageDlg dlg = new EndiciaBuyPostageDlg(endiciaOutOfFundsEx.Account))
                        {
                            dlg.ShowDialog(this);
                        }
                    }
                }
                else
                {
                    if (newErrors.Count > 0)
                    {
                        string message = "Some errors occurred during processing.\n\n";

                        foreach (string error in newErrors.Take(3))
                        {
                            message += error + "\n\n";
                        }

                        if (newErrors.Count > 3)
                        {
                            message += "See the shipment list for all errors.";
                        }

                        MessageHelper.ShowError(this, message);
                    }
                }

                // See if we are supposed to open WorldShip
                if (worldshipExported && ShippingSettings.Fetch().WorldShipLaunch)
                {
                    WorldShipUtility.LaunchWorldShip(this);
                }

                LoadSelectedShipments(true);
            };

            // Code to execute for each shipment
            executor.ExecuteAsync((ShipmentEntity shipment, object state, BackgroundIssueAdder<ShipmentEntity> issueAdder) =>
            {
                long shipmentID = shipment.ShipmentID;
                string errorMessage = null;

                try
                {
                    Exception concurrencyEx;
                    if (concurrencyErrors.TryGetValue(shipment, out concurrencyEx))
                    {
                        throw concurrencyEx;
                    }

                    // Process it
                    ShippingManager.ProcessShipment(shipmentID, licenseCheckResults, CounterRatesProcessing);

                    // Clear any previous errors
                    processingErrors.Remove(shipmentID);

                    // Special case - could refactor to abstract if necessary
                    worldshipExported |= shipment.ShipmentType == (int) ShipmentTypeCode.UpsWorldShip;
                }
                catch (ORMConcurrencyException ex)
                {
                    errorMessage = "Another user had recently made changes, so the shipment was not processed.";
                    processingErrors[shipmentID] = new ShippingException(errorMessage, ex);
                }
                catch (ObjectDeletedException ex)
                {
                    errorMessage = "The shipment has been deleted.";
                    processingErrors[shipmentID] = new ShippingException(errorMessage, ex);
                }
                catch (SqlForeignKeyException ex)
                {
                    errorMessage = "The shipment has been deleted.";
                    processingErrors[shipmentID] = new ShippingException(errorMessage, ex);
                }
                catch (ShippingException ex)
                {
                    errorMessage = ex.Message;
                    processingErrors[shipmentID] = ex;

                    // Special case
                    endiciaOutOfFundsEx = endiciaOutOfFundsEx ?? (ex.InnerException as EndiciaInsufficientFundsException);
                }

                try
                {
                    // Reload it so we can show the most recent data when the grid redisplays
                    ShippingManager.RefreshShipment(shipment);
                }
                catch (ObjectDeletedException ex)
                {
                    // If there wasn't already a different error, set this as the error
                    if (errorMessage == null)
                    {
                        errorMessage = "The shipment has been deleted.";
                        processingErrors[shipmentID] = new ShippingException(errorMessage, ex);
                    }
                }

                if (errorMessage != null)
                {
                    newErrors.Add("Order " + shipment.Order.OrderNumberComplete + ": " + errorMessage);
                }
            },

            // Each shipment to execute the code for
            shipments);
        }

        /// <summary>
        /// Method used when processing a best rate shipment whose best rate is a counter rate, and we need
        /// to provide the user with a way to sign up for the counter carrier or chose to use the best available rate.
        /// </summary>
        private DialogResult CounterRatesProcessing(CounterRatesProcessingArgs counterRatesProcessingArgs)
        {
            // If the user has opted to not see counter rate setup wizard for this batch, just return.
            if (!showCounterRateSetupWizard)
            {
                RateResult rateResult = counterRatesProcessingArgs.FilteredRates.Rates.FirstOrDefault(rr => !rr.IsCounterRate);
                if (rateResult == null)
                {
                    throw new ShippingException("No rate was found for any of your accounts, or you have not setup any accounts yet.");
                }

                counterRatesProcessingArgs.SelectedShipmentType = ShipmentTypeManager.GetType(rateResult.ShipmentType);
                return DialogResult.OK;
            }

            DialogResult setupWizardDialogResult = DialogResult.Cancel;

            this.Invoke((MethodInvoker)delegate
            {
                setupWizardDialogResult = ShowCounterRateSetupWizard(counterRatesProcessingArgs);
            });

            if (setupWizardDialogResult != DialogResult.OK)
            {
                showCounterRateSetupWizard = false;

                this.Invoke((MethodInvoker)delegate
                {
                    // When processing, we do not cache the rates, so we need to cache them now so they will get displayed.
                    // (after we call LoadRates on the ratecontrol below, later on the rates try to be loaded from cache)
                    Dictionary<ShipmentTypeCode, RateGroup> rateMap;
                    if (!shipmentRateMap.TryGetValue(counterRatesProcessingArgs.ShipmentID, out rateMap))
                    {
                        rateMap = new Dictionary<ShipmentTypeCode, RateGroup>();
                        rateMap[ShipmentTypeCode.BestRate] = counterRatesProcessingArgs.FilteredRates;
                        shipmentRateMap[counterRatesProcessingArgs.ShipmentID] = rateMap;
                    }

                    // Now we can load the rates and they will display
                    rateControl.LoadRates(counterRatesProcessingArgs.FilteredRates);
                });
                
            }

            return setupWizardDialogResult;
        }

        /// <summary>
        /// Shows the counter rate carrier setup wizard, and handles the result of the wizard.
        /// </summary>
        private DialogResult ShowCounterRateSetupWizard(CounterRatesProcessingArgs counterRatesProcessingArgs)
        {
            DialogResult setupWizardDialogResult;

            using (CounterRateProcessingSetupWizard rateProcessingSetupWizard =
                new CounterRateProcessingSetupWizard(counterRatesProcessingArgs, shipmentControl.SelectedShipments))
            {
                setupWizardDialogResult = rateProcessingSetupWizard.ShowDialog(this);

                if (setupWizardDialogResult == DialogResult.OK)
                {
                    showCounterRateSetupWizard = !rateProcessingSetupWizard.IgnoreAllCounterRates;
                    counterRatesProcessingArgs.SelectedShipmentType = rateProcessingSetupWizard.SelectedShipmentType;
                    ShippingSettings.MarkAsConfigured(rateProcessingSetupWizard.SelectedShipmentType.ShipmentTypeCode);
                }
            }

            return setupWizardDialogResult;
        }

        /// <summary>
        /// Unprocess just marks processed shipments as not processed and not voided. This is for internal
        /// use only for certification.
        /// </summary>
        private void OnUnprocess(object sender, EventArgs e)
        {
            SaveChangesToUIDisplayedShipments();

            foreach (ShipmentEntity shipment in shipmentControl.SelectedShipments)
            {
                if (shipment.Processed)
                {
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        shipment.Processed = false;
                        shipment.ProcessedDate = null;
                        shipment.Voided = false;
                        shipment.VoidedDate = null;
                        shipment.TrackingNumber = "";
                        shipment.ShipmentCost = 0;

                        adapter.SaveAndRefetch(shipment);
                    }
                }
            }

            LoadSelectedShipments(true);
        }

        /// <summary>
        /// Open the settings window
        /// </summary>
        private void OnShippingSettings(object sender, EventArgs e)
        {
            // Save changes to the current selection since we reload after getting out of the settings
            SaveChangesToUIDisplayedShipments();

            using (ShippingSettingsDlg dlg = new ShippingSettingsDlg())
            {
                dlg.ShowDialog(this);
            }

            LoadShipmentTypeCombo();

            // Reload the selected shipments in case the settings affected their values
            forceRecreateServiceControl = true;
            LoadSelectedShipments(false);
        }

        /// <summary>
        /// Track the selected shipment
        /// </summary>
        private void OnTrack(object sender, EventArgs e)
        {
            if (uiDisplayedShipments.Count != 1)
            {
                Debug.Fail("No shipments selected for tracking.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            trackingControl.TrackShipment(uiDisplayedShipments[0]);
            trackingControl.Visible = true;
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Save changes to the current selection to in memory.  Anything not selected
            // will already be saved in memory.
            SaveChangesToUIDisplayedShipments();

            // Save them to the database
            if (SaveShipmentsToDatabase(shipmentControl.AllRows.Select(r => r.Shipment), false).Count > 0)
            {
                MessageHelper.ShowWarning(this,
                    "Some of the shipments you edited had already been edited or deleted by other users.\n\n" +
                    "Your changes to those shipments were not saved.");
            }

            processingErrors.Clear();
        }

        /// <summary>
        /// Gets the store for the current shipment, if only one is selected
        /// </summary>
        private StoreEntity GetStoreForCurrentShipment()
        {
            if (uiDisplayedShipments.Count == 1)
            {
                ShipmentEntity shipment = uiDisplayedShipments[0];
                OrderEntity order = DataProvider.GetEntity(shipment.OrderID) as OrderEntity;

                StoreEntity store = StoreManager.GetStore(order.StoreID);
                shipment.Order.Store = store;

                return store;
            }

            return null;
        }

        private void OnTabSelecting(object sender, TabControlCancelEventArgs e)
        {
            // Hide the rates panel if we aren't on the service tab.
            ratesSplitContainer.Panel2Collapsed = tabControl.SelectedTab != tabPageService;
        }
    }
}
