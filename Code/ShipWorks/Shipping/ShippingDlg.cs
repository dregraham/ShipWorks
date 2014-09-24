﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore;
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
using ShipWorks.Shipping.Carriers.BestRate.Setup;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Templates;
using ShipWorks.Templates.Media;
using ShipWorks.Templates.Printing;
using ShipWorks.Templates.Processing;
using ShipWorks.UI.Utility;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using log4net;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Window from which all shipments are created
    /// </summary>
    partial class ShippingDlg : Form
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ShippingDlg));

        // The singleton list of the current set of shipping errors.
        private static Dictionary<long, Exception> processingErrors = new Dictionary<long, Exception>();

        // We load shipments asynchronously.  This flag lets us know if that's what we're currently doing, so we don't try to do
        // it reentrantly.
        private bool loadingSelectedShipments = false;

        // Usually when changing selected shipments if the service control type doesn't change we don't recreate it, we just load the new data into it.
        // But after changing shipping settings we need to make sure a new one gets initialized to pick up any new settings.
        private bool forceRecreateServiceControl = false;

        // Indicates which tab in the dialog will be initially displayed
        private InitialShippingTabDisplay initialDisplay = InitialShippingTabDisplay.Shipping;

        // The list of shipments that are the ones currently shown in the UI, not necessarily the current selection depending on async loading stuff
        private List<ShipmentEntity> uiDisplayedShipments = new List<ShipmentEntity>();

        // List of shipment types that were "Activated" at the time the UI was loaded.  This is important b\c if it got activated after we loaded the UI from another machine,
        // and we used the "live" value, then we'd think a shipment was currently loaded when really it wasn't.
        private List<ShipmentTypeCode> uiActivatedShipmentTypes = new List<ShipmentTypeCode>();
        
        // If the user is processing with best rate and counter rates, they have the option to ignore signing up for
        // counter rates during the current batch.  This variable will be for tracking that flag.
        private bool showBestRateCounterRateSetupWizard = true;

        private List<ShipmentEntity> loadedShipmentEntities;
        private bool cancelProcessing;

        private readonly System.Windows.Forms.Timer getRatesTimer = new System.Windows.Forms.Timer();
        private BackgroundWorker getRatesBackgroundWorker;
        private const int getRatesDebounceTime = 500;

        private ShipmentEntity clonedShipmentEntityForRates;
        
        private RateSelectedEventArgs preSelectedRateEventArgs;
        private ValidatedAddressScope validatedAddressScope;

        private readonly ShipSenseSynchronizer shipSenseSynchronizer;

        private readonly System.Windows.Forms.Timer shipSenseChangedTimer = new System.Windows.Forms.Timer();
        private const int shipSenseChangedDebounceTime = 500;
        private bool shipSenseNeedsUpdated = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDlg(List<ShipmentEntity> shipments)
            : this(shipments, InitialShippingTabDisplay.Shipping)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingDlg"/> class. This is intended
        /// to be used when a rate has been selected outside of the shipping dialog.
        /// </summary>
        public ShippingDlg(ShipmentEntity shipment, RateSelectedEventArgs preSelectedRateEventArgs)
            : this(new List<ShipmentEntity> { shipment }, InitialShippingTabDisplay.Shipping)
        {
            this.preSelectedRateEventArgs = preSelectedRateEventArgs;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDlg(List<ShipmentEntity> shipments, InitialShippingTabDisplay initialDisplay)
        {
            InitializeComponent();

            if (shipments == null)
            {
                throw new ArgumentNullException("shipments");
            }
            
            // Manage the window positioning
            WindowStateSaver windowSaver = new WindowStateSaver(this, WindowStateSaverOptions.Size | WindowStateSaverOptions.InitialMaximize);
            windowSaver.ManageSplitter(splitContainer, "Splitter");
            windowSaver.ManageSplitter(ratesSplitContainer, "RateSplitter");

            getRatesTimer.Tick += OnGetRatesTimerTick;
            getRatesTimer.Interval = getRatesDebounceTime;

            shipSenseChangedTimer.Tick += OnShipSenseChangedTimerTick;
            shipSenseChangedTimer.Interval = shipSenseChangedDebounceTime;

            // Load all the shipments into the grid
            shipmentControl.AddShipments(shipments);

            shipmentControl.ErrorProvider = new ShipmentGridRowProcessingErrorProvider();

            // Security
            panelSettingsButtons.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);

            this.initialDisplay = initialDisplay;

            rateControl.Initialize(new FootnoteParameters(()=> LoadSelectedShipments(false) , GetStoreForCurrentShipment));

            // Ensure that the rate control cannot take up more than 1/3rd the height of the dialog, even after resizing
            SetServiceControlMinimumHeight();

            ResizeBegin += (sender, args) => ratesSplitContainer.Panel1MinSize = 0;
            ResizeEnd += (sender, args) => SetServiceControlMinimumHeight();

            //TODO: Delete this line in the next story, use the hash that's stored on the shipment so that we don't have to populate the order!!!
            shipments.ForEach(OrderUtility.PopulateOrderDetails);
            shipments.ForEach(ShippingManager.EnsureShipmentLoaded);
            shipSenseSynchronizer = new ShipSenseSynchronizer(shipments);
        }

        /// <summary>
        /// Ensure that the service control is always at least 2/3rds the height of the shipping dialog
        /// </summary>
        private void SetServiceControlMinimumHeight()
        {
            ratesSplitContainer.Panel1MinSize = 2 * (ratesSplitContainer.Height / 3);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            validatedAddressScope = new ValidatedAddressScope();

            labelInternal.Visible = InterapptiveOnly.IsInterapptiveUser;
            unprocess.Visible = InterapptiveOnly.IsInterapptiveUser;

            shipmentControl.Initialize();

            comboShipmentType.DisplayMember = "Key";
            comboShipmentType.ValueMember = "Value";

            LoadShipmentTypeCombo();

            if (initialDisplay == InitialShippingTabDisplay.Tracking)
            {
                tabControl.SelectedTab = tabPageTracking;
            }
            else if (initialDisplay == InitialShippingTabDisplay.Insurance)
            {
                tabControl.SelectedTab = tabPageInsurance;
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
                selected = (ShipmentTypeCode)comboShipmentType.SelectedValue;
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

            comboShipmentType.SelectedIndexChanged += OnChangeShipmentType;
        }

        /// <summary>
        /// Sort the list of shipment types for display in the combo box
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
            // For some reason the tab control looks goofy and doesn't redraw after the splitter moves, so we force it
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
                // Detach from the ShipSense events, so we aren't handling events as the shipments get loaded
                if (ServiceControl != null)
                {
                    ServiceControl.ShipSenseFieldChanged -= OnShipSenseFieldChanged;
                }

                if (CustomsControl != null)
                {
                    CustomsControl.ShipSenseFieldChanged -= OnShipSenseFieldChanged;
                }

                // Save all changes from the UI to the previous entity selection
                SaveUIDisplayedShipments();

                // The user could have changed something and clicked a different shipment before the timer fired, so apply any ShipSense
                // if needed.
                SynchronizeWithShipSense();

                UpdateSelectedShipmentCount();

                // Load the newly selected shipments
                LoadSelectedShipments(false);

                // Re-attach to the ShipSense changed event now that the shipments have been loaded
                if (ServiceControl != null)
                {
                    ServiceControl.ShipSenseFieldChanged += OnShipSenseFieldChanged;
                }

                if (CustomsControl != null)
                {
                    CustomsControl.ShipSenseFieldChanged += OnShipSenseFieldChanged;
                }

                ClearRates(string.Empty);
                GetRates();
            }
        }

        /// <summary>
        /// Updates the selected shipment count.
        /// </summary>
        private void UpdateSelectedShipmentCount()
        {
            int selectedShipmentCount = shipmentControl.SelectedShipments.Count();
            
            labelProcessing.Text = string.Format("Shipments ({0} selected)", selectedShipmentCount);
            menuProcessSelected.Text = string.Format("Create Label ({0} shipment{1})", selectedShipmentCount, selectedShipmentCount > 1 ? "s" : "");
            
            string plural = selectedShipmentCount > 1 ? "s" : "";

            processDropDownButton.Text = "Create Label" + plural;
            voidSelected.Text = "Void Label" + plural;
            print.Text = "Reprint Label" + plural;
        }

        /// <summary>
        /// The selected shipment type has changed
        /// </summary>
        private void OnChangeShipmentType(object sender, EventArgs e)
        {
            // Shouldn't be able to be in the middle of loading and get that combo to change at the same time.
            Debug.Assert(!loadingSelectedShipments);

            // Save all changes from the UI to the previous entity selection
            SaveUIDisplayedShipments();

            // Synchronize to make sure the status is up to date in the case where dimensions
            // have been manually altered across shipment types
            shipSenseNeedsUpdated = true;
            SynchronizeWithShipSense();
            
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
            LoadSelectedShipments(resortWhenDone, true);
        }

        /// <summary>
        /// Update the shipment details section to display the data currently in the selected shipments
        /// </summary>
        private void LoadSelectedShipments(bool resortWhenDone, bool getRatesWhenDone)
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

            BackgroundExecutor<ShipmentEntity> executor = new BackgroundExecutor<ShipmentEntity>(this, "Preparing Shipments", "ShipWorks is preparing the shipments.", "Shipment {0} of {1}");

            // Code to execute once background load is complete
            executor.ExecuteCompleted += LoadSelectedShipmentsCompleted;

            // User state that will get passed to the completed method
            Dictionary<string, object> userState = new Dictionary<string, object>();
            userState["loaded"] = loaded;
            userState["deleted"] = deleted;
            userState["resortWhenDone"] = resortWhenDone;
            userState["getRatesWhenDone"] = getRatesWhenDone;

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
                        ShippingManager.EnsureShipmentLoaded(shipment);
                        
                        // Even without the type being setup, we can still load the customs stuff.  Normally EnsureShipmentLoaded would do that for us.
                        CustomsManager.LoadCustomsItems(shipment, false);

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
            }, shipmentsToLoad, userState); // Execute the code for each shipment
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
            // the save would never happen, b\c we wouldn't have a reference to it when we closed.
            List<ShipmentEntity> removedNeedsSaved = uiDisplayedShipments.Where(s => !s.DeletedFromDatabase && shipmentControl.FindRow(s.ShipmentID) == null).ToList();

            // For the one's that have been removed, ignore any concurrency\deleted errors
            SaveShipmentsToDatabase(removedNeedsSaved, false);

            // We also need to save changes to any whose state\country has changed, since that affects customs items requirements
            List<ShipmentEntity> destinationChangeNeedsSaved = uiDisplayedShipments.Except(removedNeedsSaved)
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
            foreach (KeyValuePair<ShipmentEntity, Exception> pair in errors)
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
        /// The second part of the load selected shipments routine, after we have assured all carrier specific data has been loaded
        /// </summary>
        private void LoadSelectedShipmentsCompleted(object sender, BackgroundExecutorCompletedEventArgs<ShipmentEntity> e)
        {
            loadingSelectedShipments = false;

            // Extract userState
            Dictionary<string, object> userState = (Dictionary<string, object>)e.UserState;
            loadedShipmentEntities = (List<ShipmentEntity>)userState["loaded"];
            List<ShipmentEntity> deleted = (List<ShipmentEntity>)userState["deleted"];
            bool resortWhenDone = (bool)userState["resortWhenDone"];
            bool getRatesWhenDone = (bool)userState["getRatesWhenDone"];

            // Go thread each shipment that we loaded and update the corresponding row in the grid with the latest shipment data
            ApplyShipmentsToGridRows(loadedShipmentEntities);

            // We also have to apply deleted information to the grid, so the grid knows which rows are marked for deletion and will need
            // removed on the next RefreshAndResort
            ApplyShipmentsToGridRows(deleted);

            // If they didn't cancel - but the selection is different from what's loaded - then the selection has changed since we started
            // this background loading, and we need to do it again.
            //
            // If they had canceled, then the selection will be bigger than what was loaded obviously - and the rest of this function
            // will take care of unselecting it.
            if (!e.Canceled && HasSelectionChanged(loadedShipmentEntities))
            {
                LoadSelectedShipments(resortWhenDone);
                return;
            }

            // Turn off selection processing
            shipmentControl.SelectionChanged -= this.OnChangeSelectedShipments;

            // If we canceled rate a list of all rows that are selected that weren't loaded and un-select them.  There could be selected and not loaded rows for rows
            // that had been deleted too, but those will get wiped out when we do the RefreshAndRestor.
            if (e.Canceled)
            {
                List<ShipmentGridRow> notLoaded = shipmentControl.SelectedRows.Where(r => !loadedShipmentEntities.Any(s => s.ShipmentID == r.Shipment.ShipmentID)).ToList();

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
            shipmentControl.SelectionChanged += OnChangeSelectedShipments;

            ShipmentType shipmentType = null;

            bool enableEditing = !loadedShipmentEntities.Any(s => s.Processed);

            // To have editing enabled, its also necessary to have permissions for all of them
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
                    List<KeyValuePair<string, ShipmentTypeCode>> enabledTypes = (List<KeyValuePair<string, ShipmentTypeCode>>)comboShipmentType.DataSource;
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
                if (!shipment.Processed)
                {
                    ShipmentTypeManager.GetType(shipment).UpdateDynamicShipmentData(shipment);
                }
            }

            // Load the service control with the UI displayed shipments
            LoadServiceControl(loadedShipmentEntities, shipmentType, enableEditing, enableShippingAddress);

            // Load the tracking control
            LoadTrackingDisplay();

            // Load the insurance control
            LoadInsuranceDisplay();

            // If there was a setup control, remove it
            ClearPreviousSetupControl();

            // Show the setup control if setup is required.  Don't go by the current value in ShippingManager.IsShipmentTypeSetup - go by the value we used when loading.
            if (shipmentType != null)
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

            if (resortWhenDone && !getRatesWhenDone)
            {
                shipmentControl.RefreshAndResort();
            }
            
            if (preSelectedRateEventArgs != null)
            {
                // A rate has been preselected outside of this dialog, so trigger the 
                // service control event handler to load the correct service type
                ServiceControl.OnConfigureRateClick(this, preSelectedRateEventArgs);

                // We only want to do this once for the initial load
                preSelectedRateEventArgs = null;
            }

            if (getRatesWhenDone)
            {
                ClearRates(string.Empty);
                GetRates();
            }

            shipSenseSynchronizer.Add(loadedShipmentEntities);
        }
        
        /// <summary>
        /// Apply the given shipments to their associated rows in the grid.  This is used after cloned shipments were altered somehow in the background thread
        /// and the changes need to be propagated to the UI.
        /// </summary>
        private void ApplyShipmentsToGridRows(IEnumerable<ShipmentEntity> shipments)
        {
            // Go thread each shipment that we loaded and update the corresponding row in the grid with the latest shipment data
            foreach (ShipmentEntity shipment in shipments)
            {
                ApplyShipmentToGridRow(shipment);
            }
        }

        /// <summary>
        /// Apply the given shipment to the associated row in the grid.  This is used after cloned shipments were altered somehow in the background thread
        /// and the changes need to be propagated to the UI.
        /// </summary>
        private void ApplyShipmentToGridRow(ShipmentEntity shipment)
        {
            ShipmentGridRow row = shipmentControl.FindRow(shipment.ShipmentID);

            // I don't _think_ this should ever be null
            Debug.Assert(row != null, "Could not find shipment row for shipment when applying settings to grid row.");

            if (row != null)
            {
                // Update the row with the copy of the shipment that was updated in the background
                row.Shipment = shipment;
            }
        }

        /// <summary>
        /// Determines if the selection has changed from what we loaded
        /// </summary>
        private bool HasSelectionChanged(List<ShipmentEntity> loaded)
        {
            // Determine the current selection - don't consider deleted, or based on where this function is called we'd end up
            // in a stack overflow.  The deleted ones will get unselected by the processing following this function call in LoadSelectedShipmentsCompleted.
            Dictionary<long, bool> selected = shipmentControl.SelectedShipments.Where(s => !s.DeletedFromDatabase).ToDictionary(s => s.ShipmentID, s => true);

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
        private void OnShipmentTypeSetupComplete(object sender, EventArgs e)
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
                    newServiceControl = new MultiSelectServiceControl(rateControl);
                }
                else
                {
                    newServiceControl = shipmentType.CreateServiceControl(rateControl);
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
                    newServiceControl.RecipientDestinationChanged += OnRecipientDestinationChanged;
                    newServiceControl.ShipmentServiceChanged += OnShipmentServiceChanged;
                    newServiceControl.RateCriteriaChanged += OnRateCriteriaChanged;
                    newServiceControl.ShipSenseFieldChanged += OnShipSenseFieldChanged;
                    newServiceControl.ShipmentsAdded += OnServiceControlShipmentsAdded;
                    newServiceControl.ShipmentTypeChanged += OnShipmentTypeChanged;
                    newServiceControl.ClearRatesAction = ClearRates;
                    rateControl.RateSelected += newServiceControl.OnRateSelected;
                    rateControl.ActionLinkClicked += newServiceControl.OnConfigureRateClick;

                    newServiceControl.Dock = DockStyle.Fill;
                    serviceControlArea.Controls.Add(newServiceControl);
                }

                // Finally, remove the old service control, or the blank panel we created
                if (oldServiceControl != null)
                {
                    oldServiceControl.RecipientDestinationChanged -= OnRecipientDestinationChanged;
                    oldServiceControl.ShipmentServiceChanged -= OnShipmentServiceChanged;
                    oldServiceControl.RateCriteriaChanged -= OnRateCriteriaChanged;
                    oldServiceControl.ShipSenseFieldChanged -= OnShipSenseFieldChanged;
                    oldServiceControl.ShipmentsAdded -= OnServiceControlShipmentsAdded;
                    oldServiceControl.ShipmentTypeChanged -= OnShipmentTypeChanged;
                    oldServiceControl.ClearRatesAction = x => { };
                    rateControl.RateSelected -= oldServiceControl.OnRateSelected;
                    rateControl.ActionLinkClicked -= oldServiceControl.OnConfigureRateClick;

                    oldServiceControl.Dispose();
                }
                else
                {
                    reduceFlash.Dispose();
                }
            }

            // Update the custom's control
            LoadCustomsControl(shipments, shipmentType, enableEditing);

            // Update the provider in the shipment grid
            shipmentControl.Refresh();
        }

        /// <summary>
        /// Clear the rates from the grid
        /// </summary>
        private void ClearRates(string reason)
        {
            rateControl.ClearRates(reason);
        }

        /// <summary>
        /// When BestRate Rate is selected, the BestRateServiceControl raises this event.
        /// </summary>
        private void OnShipmentTypeChanged(object sender, EventArgs e)
        {
            ShipmentEntity shipment = shipmentControl.SelectedShipments.First();
            ShipmentTypeCode shipmentTypeCode = ((ShipmentTypeCode)shipment.ShipmentType);

            // Check that the combo box has the shipment type in it
            List<KeyValuePair<string, ShipmentTypeCode>> dataSource = (List<KeyValuePair<string, ShipmentTypeCode>>)comboShipmentType.DataSource;
            if (dataSource.Select(d => d.Value).All(v => v != shipmentTypeCode))
            {
                // The combo box does not have the shipment type in it, so we need to reload 
                // it; this is probably due to an account being created via best rate that
                // was previously a globally excluded shipment type
                LoadShipmentTypeCombo();
            }

            comboShipmentType.SelectedValue = shipmentTypeCode;

            ClearRates(string.Empty);
            GetRates();
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

                labelTrackingMessage.Text = "Multiple shipments are selected. Select a single shipment to view tracking information.";
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

                    if (initialDisplay == InitialShippingTabDisplay.Tracking)
                    {
                        OnTrack(track, EventArgs.Empty);
                    }
                }
            }

            initialDisplay = InitialShippingTabDisplay.Shipping;
        }

        /// <summary>
        /// Load the insurance data from the selected shipments into the control
        /// </summary>
        private void LoadInsuranceDisplay()
        {
            insuranceTabControl.LoadClaim(uiDisplayedShipments);
            initialDisplay = InitialShippingTabDisplay.Shipping;
        }

        /// <summary>
        /// Load the rates displayed in the Rates section
        /// </summary>
        private void LoadDisplayedRates(RateGroup rateGroup)
        {
            if (ServiceControl != null)
            {
                if (uiDisplayedShipments.Count != 1)
                {
                    rateGroup = null;
                }

                LoadRates(rateGroup);
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
                ClearRates("(Multiple)");
            }
            else
            {
                if (rateGroup == null)
                {
                    if (this.loadedShipmentEntities.Count == 1 && this.loadedShipmentEntities[0].Processed)
                    {
                        ClearRates("The shipment has already been processed.");
                    }
                    else
                    {
                        ClearRates(string.Empty);
                    }
                }
                else if (rateGroup.Rates.Count == 0)
                {
                    rateControl.ClearRates(rateGroup.FootnoteFactories.Count() == 0 ? "No rates are available for the shipment." : "", rateGroup);
                }
                else
                {
                    // Only show the configure link for the best rate shipment type
                    rateControl.ActionLinkVisible = rateGroup.Carrier == ShipmentTypeCode.BestRate;
                    rateControl.LoadRates(rateGroup);
                    
                    ServiceControl.SyncSelectedRate();
                }
            }
        }

        /// <summary>
        /// The selected shipment(s) has been edited in a such a way that the rates are no longer valid.
        /// </summary>
        private void OnRateCriteriaChanged(object sender, EventArgs e)
        {
            // Since this could change if customs needs generated, we need to save right now.
            SaveChangesToUIDisplayedShipments();

            // Shipping rate changes will also affect insurance rates
            UpdateInsuranceDisplay();

            // Update the grid so that changes to the destination appear immediately (especially validation status)
            shipmentControl.RefreshAndResort();

            GetRates();
        }

        /// <summary>
        /// A full reload of rates is required
        /// </summary>
        private void OnRateReloadRequired(object sender, EventArgs e)
        {
            // Then go ahead and do a Get Rates and whatever is selected
            if (uiDisplayedShipments.Count == 1)
            {
                // Because this is coming from the rate control, and the only thing that causes rate changes from the rate control
                // is the Express1 promo footer, we need to remove the shipment from the cache before we get rates
                ShippingManager.RemoveShipmentFromRatesCache(uiDisplayedShipments.FirstOrDefault());

                GetRates();
            }
        }

        /// <summary>
        /// The selected shipment(s) has been edited in a such a way that ShipSense values may no longer valid.
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e)
        {
            // Stop the timer
            shipSenseChangedTimer.Stop();

            // Make a note that something changed and we still need to apply ShipSense.  This is for the race condition with the user
            // making changes and clicking a different shipment before the timer fires.
            shipSenseNeedsUpdated = true;

            // Restart the timer
            shipSenseChangedTimer.Start();
        }

        /// <summary>
        /// Synchronize the shipment with other matching shipments
        /// </summary>
        private void SynchronizeWithShipSense()
        {
            if (uiDisplayedShipments.Count > 0 && shipSenseNeedsUpdated)
            {
                // Check for null in case the ShipSense timer fires as the window is closing and the customs
                // control is no longer available
                if (CustomsControl != null)
                {
                    // The UI hasn't updated the shipment properties, so we need to force an update to the entities
                    CustomsControl.SaveToShipments();
                }

                ShipmentEntity shipment = uiDisplayedShipments.FirstOrDefault(s => !s.Processed);

                if (shipment != null)
                {
                    //TODO: Delete this line in the next story, use the hash that's stored on the shipment so that we don't have to populate the order!!!
                    OrderUtility.PopulateOrderDetails(shipment);

                    shipSenseSynchronizer.Add(loadedShipmentEntities);
                    shipSenseSynchronizer.SynchronizeWith(shipment);

                    // Set shipSenseNeedsUpdated to false, so that we don't get in an infinite refresh loop
                    shipSenseNeedsUpdated = false;

                    // Check for the handle, so we don't crash if the shipping dialog is closed
                    // in the middle of the synchronization
                    if (IsHandleCreated)
                    {
                        // Refresh the shipment control, so any status changes are reflected
                        shipmentControl.RefreshAndResort();
                        
                        if (CustomsControl != null)
                        {
                            // Refresh the items in the customs control after synching with ShipSense. This
                            // retains any customs items that were selected prior to the sync.
                            CustomsControl.RefreshItems();
                        }
                    }
                }
            }
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
                if (shipmentType == null)
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
                newCustomsControl.ShipSenseFieldChanged += OnShipSenseFieldChanged;
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
                    oldCustomsControl.ShipSenseFieldChanged -= OnShipSenseFieldChanged;
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
        private void OnRecipientDestinationChanged(object sender, EventArgs e)
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
                // really goof that up.
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

            // Save the insurance data if we're switching away from the insurance tab
            if (tabControl.SelectedTab == tabPageInsurance)
            {
                insuranceTabControl.SaveToShipments();
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

            insuranceTabControl.SaveToShipments();

            foreach (ShipmentEntity shipment in shipments)
            {
                if (!shipment.Processed)
                {
                    ShipmentTypeManager.GetType(shipment).UpdateDynamicShipmentData(shipment);
                    ShipmentTypeManager.GetType(shipment).UpdateTotalWeight(shipment);

                    // If the there is a specific shipment type selected, set it
                    if (!comboShipmentType.MultiValued)
                    {
                        shipment.ShipmentType = (int)(ShipmentTypeCode)comboShipmentType.SelectedValue;
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

                    using (SqlAdapter sqlAdapter = new SqlAdapter(true))
                    {
                        validatedAddressScope.FlushAddressesToDatabase(sqlAdapter, shipment.ShipmentID, "Ship");
                        sqlAdapter.Commit();
                    }
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
        private void OnApplyProfile(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
            ShippingProfileEntity profile = (ShippingProfileEntity) menuItem.Tag;

            // Save any changes that have been made thus far, so the profile changes can be made on top of that
            SaveChangesToUIDisplayedShipments();

            // Apply the profile to each ui displayed shipment
            foreach (ShipmentEntity shipment in uiDisplayedShipments)
            {
                if (!shipment.Processed)
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
                if (shipment.ShipmentType != (int)ShipmentTypeCode.None)
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

            ShipmentGridShipmentsChangedEventArgs eventArgs = (ShipmentGridShipmentsChangedEventArgs) e;
            foreach (ShipmentEntity shipment in eventArgs.ShipmentsAdded)
            {
                //TODO: Delete this line in the next story, use the hash that's stored on the shipment so that we don't have to populate the order!!!
                OrderUtility.PopulateOrderDetails(shipment);
                ShippingManager.EnsureShipmentLoaded(shipment);
                
                shipSenseSynchronizer.Add(shipment);
            }
        }

        /// <summary>
        /// Shipments have been removed from the grid
        /// </summary>
        private void OnShipmentsRemoved(object sender, EventArgs e)
        {
            UpdateEditControlsSecurity();

            ShipmentGridShipmentsChangedEventArgs eventArgs = (ShipmentGridShipmentsChangedEventArgs)e;
            foreach (ShipmentEntity shipment in eventArgs.ShipmentsRemoved)
            {
                shipSenseSynchronizer.Remove(shipment);
            }
        }

        /// <summary>
        /// Get the selected ShipmentType, or null if multiple
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
                FilterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

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

            // Executes after everything is totally done, and is on the UI thread
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
        /// Gets rates for the selected shipments
        /// </summary>
        private void GetRates()
        {
            GetRates(true);
        }

        /// <summary>
        /// Gets rates for the selected shipments
        /// </summary>
        /// <param name="cloneShipment">Indicates whether the currently selected shipment should be cloned. This should only
        /// be true on calls raised because the shipment changed. It should be false on calls from the debounce logic.</param>
        private void GetRates(bool cloneShipment)
        {
            getRatesTimer.Stop();

            // There's no need to bother doing anything if we don't have exactly one shipment because
            // the grid does not show rates otherwise
            if (uiDisplayedShipments.Count > 1)
            {
                ClearRates("(Multiple)");
                return;
            }
            if (uiDisplayedShipments.Count == 0)
            {
                ClearRates(string.Empty);
                return;
            }

            // Don't save and clone the current shipment if this is being executed from the debounce logic,
            // since it causes concurrency issues
            if (cloneShipment)
            {
                // Save changes to the current selection (NOT the ones we are processing) before we process it
                SaveChangesToUIDisplayedShipments();

                // The list of shipments to get rates for.  A cloned collection to changes in the background don't have thread issues with the foreground
                ShipmentEntity uiShipment = uiDisplayedShipments.First();
                clonedShipmentEntityForRates = EntityUtility.CloneEntity(uiShipment);   
            }

            getRatesTimer.Start();
        }

        /// <summary>
        /// Actually get the rates when the debounce timer has elapsed
        /// </summary>
        private void OnGetRatesTimerTick(object sender, EventArgs e)
        {
            ShipmentEntity clonedShipment = clonedShipmentEntityForRates;
            ShipmentType uiShipmentType = ShipmentTypeManager.GetType(clonedShipment);

            getRatesTimer.Stop();

            if (getRatesBackgroundWorker != null && getRatesBackgroundWorker.IsBusy)
            {
                GetRates(false);
                return;
            }

            bool anyAttempted = false;

            if (!uiShipmentType.SupportsGetRates || clonedShipment.Processed)
            {
                return;
            }

            if (getRatesBackgroundWorker != null)
            {
                getRatesBackgroundWorker.Dispose();
            }

            getRatesBackgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = true
            };

            // What to do when done.  Runs on the UI thread.
            getRatesBackgroundWorker.RunWorkerCompleted += (_sender, _e) =>
            {
                if (anyAttempted && !getRatesTimer.Enabled)
                {
                    rateControl.ShowSpinner = false;

                    // This is not necessary since we reload completely anyway, but it reduces the perceived load time by getting these displayed ASAP
                    LoadDisplayedRates(_e.Result as RateGroup);
                }
            };

            // What to do for each shipment
            getRatesBackgroundWorker.DoWork += (_sender, _e) =>
            {
                ShipmentEntity shipment = (ShipmentEntity)_e.Argument;
                _e.Result = _e.Argument;

                try
                {
                    anyAttempted = true;
                    _e.Result = ShippingManager.GetRates(shipment);

                    // Just in case it used to have an error remove it
                    processingErrors.Remove(shipment.ShipmentID);
                }
                catch (InvalidRateGroupShippingException ex)
                {
                    log.Error("Shipping exception encountered while getting rates", ex);
                    _e.Result = ex.InvalidRates;
                }
                catch (ShippingException ex)
                {
                    log.Error("Shipping exception encountered while getting rates", ex);
                }
            };

            rateControl.ShowSpinner = true;
            getRatesBackgroundWorker.RunWorkerAsync(clonedShipment);
        }

        /// <summary>
        /// Actually makes the ShipSense changed call when the debounce timer has elapsed
        /// </summary>
        private void OnShipSenseChangedTimerTick(object sender, EventArgs e)
        {
            shipSenseChangedTimer.Stop();
            SynchronizeWithShipSense();
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
            List<ShipmentEntity> shipments = EntityUtility.CloneEntityCollection(shipmentControl.SelectedShipments.Where(s => s.Processed && !s.Voided));

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
            }, shipments); // Each shipment to execute the code for
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
            cancelProcessing = false;

            // Save changes to the current selection in memory.  We save to the database later on a per-shipment basis in the background thread.
            SaveChangesToUIDisplayedShipments();

            // Clear errors on ones that are already marked as processed. Like if they got the AlreadyProcessed error, and then hit process again,
            // the error shouldn't stay
            foreach (ShipmentEntity shipment in shipments.Where(s => s.Processed))
            {
                processingErrors.Remove(shipment.ShipmentID);
            }

            // Filter out the ones we know to be already processed, or are not ready
            shipments = shipments.Where(s => !s.Processed && s.ShipmentType != (int) ShipmentTypeCode.None);

            // Create clones to be processed - that way any changes made don't have race conditions with the UI trying to paint with them
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
                "Shipment {0} of {1}",
                false);

            List<string> newErrors = new List<string>();
            Dictionary<ShipmentEntity, Exception> concurrencyErrors = new Dictionary<ShipmentEntity, Exception>();

            // Special cases - I didn't think we needed to abstract these.  If it gets out of hand we should refactor.
            bool worldshipExported = false;
            IInsufficientFunds outOfFundsException = null;

            // Maps storeID's to license exceptions, so we only have to check a store once per processing batch
            Dictionary<long, Exception> licenseCheckResults = new Dictionary<long, Exception>();


            List<string> orderHashes = new List<string>();
            IEnumerable<ShipmentEntity> exludedShipmentsFromShipSenseRefresh = shipmentControl.AllRows.Select(r => r.Shipment);

            // What to do before it gets started (but is on the background thread)
            executor.ExecuteStarting += (object s, EventArgs args) =>
            {
                // Force the shipments to save - this weeds out any shipments early that have been edited by another user on another computer.
                concurrencyErrors = SaveShipmentsToDatabase(shipments, true);

                // Reset to true, so that we show the counter rate setup wizard for this batch.
                showBestRateCounterRateSetupWizard = true;
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
                if (outOfFundsException != null)
                {
                    DialogResult answer = MessageHelper.ShowQuestion(this,
                        string.Format("You do not have sufficient funds in {0} account {1} to continue shipping.\n\n" +
                        "Would you like to purchase more now?", outOfFundsException.Provider, outOfFundsException.AccountIdentifier));

                    if (answer == DialogResult.OK)
                    {
                        using (Form dlg = outOfFundsException.CreatePostageDialog())
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

                // We want to update the synchronizer with the KB entry of the latest processed
                // shipment, so the status of any remaining unprocessed shipments are reflected correctly
                shipSenseSynchronizer.RefreshKnowledgebaseEntries();
                shipSenseSynchronizer.MonitoredShipments.ToList().ForEach(shipSenseSynchronizer.SynchronizeWith);

                // Refresh/update the ShipSense status of any unprocessed shipments that are outside of the shipping dialog
                Knowledgebase knowledgebase = new Knowledgebase();
                foreach (string hash in orderHashes.Distinct())
                {
                    knowledgebase.RefreshShipSenseStatus(hash, exludedShipmentsFromShipSenseRefresh.Select(s => s.ShipmentID));
                }
            };

            // Code to execute for each shipment
            executor.ExecuteAsync((ShipmentEntity shipment, object state, BackgroundIssueAdder<ShipmentEntity> issueAdder) =>
            {
                // Processing was canceled by the best rate processing dialog
                if (cancelProcessing)
                {
                    return;
                }

                RateResult selectedRate = state as RateResult;

                long shipmentID = shipment.ShipmentID;
                string errorMessage = null;

                try
                {
                    Exception concurrencyEx;
                    if (concurrencyErrors.TryGetValue(shipment, out concurrencyEx))
                    {
                        throw concurrencyEx;
                    }

                    orderHashes.Add(shipment.Order.ShipSenseHashKey);

                    // Process it       
                    if (shipment.ShipmentType == (int)ShipmentTypeCode.BestRate)
                    {
                        // Process the shipment with the counter rates processing method for best rate
                        ShippingManager.ProcessShipment(shipmentID, licenseCheckResults, BestRateCounterRatesProcessing, selectedRate);
                    }
                    else
                    {
                        // This is a shipment for a "real" shipping provider, so use the callback that will
                        // launch the wizard for a specific shipment type.
                        ShippingManager.ProcessShipment(shipmentID, licenseCheckResults, CounterRatesProcessing, selectedRate);
                    }

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

                    // Special case for out of funds
                    if (ex.InnerException != null)
                    {
                        outOfFundsException = outOfFundsException ?? (ex.InnerException.InnerException as IInsufficientFunds);
                    }
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
            }, shipments, rateControl.SelectedRate); // Each shipment to execute the code for
        }

        /// <summary>
        /// Method used when processing a (non-best rate) shipment for a provider that does not have any
        /// accounts setup, and we need to provide the user with a way to sign up for the carrier.
        /// </summary>
        /// <param name="counterRatesProcessingArgs">The counter rates processing arguments.</param>
        /// <returns></returns>
        private DialogResult CounterRatesProcessing(CounterRatesProcessingArgs counterRatesProcessingArgs)
        {
            // This is for a specific shipment type, so we're always going to need to show the wizard 
            // since the user explicitly chose to process with this provider
            ShipmentType shipmentType = ShipmentTypeManager.GetType(counterRatesProcessingArgs.Shipment);

            DialogResult result = DialogResult.Cancel;

            if (!cancelProcessing)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    using (ShipmentTypeSetupWizardForm setupWizard = shipmentType.CreateSetupWizard())
                    {
                        result = setupWizard.ShowDialog(this);

                        if (result == DialogResult.OK)
                        {
                            ShippingSettings.MarkAsConfigured(shipmentType.ShipmentTypeCode);

                            ShippingManager.EnsureShipmentLoaded(counterRatesProcessingArgs.Shipment);
                            ServiceControl.SaveToShipments();
                            ServiceControl.LoadAccounts();
                        }
                        else
                        {
                            // User canceled out of the setup wizard for this batch, so don't show
                            // any setup wizard for the rest of this batch
                            cancelProcessing = true;
                        }
                    }
                });
            }

            return result;
        }

        /// <summary>
        /// Method used when processing a best rate shipment whose best rate is a counter rate, and we need
        /// to provide the user with a way to sign up for the counter carrier or chose to use the best available rate.
        /// </summary>
        private DialogResult BestRateCounterRatesProcessing(CounterRatesProcessingArgs counterRatesProcessingArgs)
        {
            // If the user has opted to not see counter rate setup wizard for this batch, just return.
            if (!showBestRateCounterRateSetupWizard)
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
                RateResult selectedRate = rateControl.SelectedRate ??
                                          counterRatesProcessingArgs.FilteredRates.Rates.First();

                if (selectedRate.IsCounterRate)
                {
                    setupWizardDialogResult = ShowBestRateCounterRateSetupWizard(counterRatesProcessingArgs, selectedRate);
                }
                else
                {
                    //The selected rate is not a counter rate, so we just set the shipment type and rate based on the rate grid
                    counterRatesProcessingArgs.SelectedShipmentType = ShipmentTypeManager.GetType(selectedRate.ShipmentType);
                    counterRatesProcessingArgs.SelectedRate = selectedRate;

                    setupWizardDialogResult = DialogResult.OK;
                }
            });

            if (setupWizardDialogResult != DialogResult.OK)
            {
                cancelProcessing = true;
                showBestRateCounterRateSetupWizard = false;

                this.Invoke((MethodInvoker)delegate
                {
                    // The user canceled out of the dialog, so just show the filtered rates
                    rateControl.LoadRates(counterRatesProcessingArgs.FilteredRates);
                });
            }

            return setupWizardDialogResult;
        }

        /// <summary>
        /// Shows the counter rate carrier setup wizard, and handles the result of the wizard.
        /// </summary>
        /// <param name="counterRatesProcessingArgs">Arguments passed from the counter rate event</param>
        /// <param name="selectedRate">Rate that has been selected</param>
        /// <remarks>The selected rate is passed into the method instead of checking the selected rate from the rate control
        /// because there is no selected rate when processing multiple shipments.  Also, it is simply for a user to deselect a rate,
        /// in which case, we should just use the cheapest.</remarks>
        private DialogResult ShowBestRateCounterRateSetupWizard(CounterRatesProcessingArgs counterRatesProcessingArgs, RateResult selectedRate)
        {
            DialogResult setupWizardDialogResult;

            using (CounterRateProcessingSetupWizard rateProcessingSetupWizard = new CounterRateProcessingSetupWizard(counterRatesProcessingArgs.FilteredRates, selectedRate, shipmentControl.SelectedShipments))
            {
                setupWizardDialogResult = rateProcessingSetupWizard.ShowDialog(this);
                rateProcessingSetupWizard.BringToFront();

                if (setupWizardDialogResult == DialogResult.OK)
                {
                    showBestRateCounterRateSetupWizard = !rateProcessingSetupWizard.IgnoreAllCounterRates;
                    
                    counterRatesProcessingArgs.SelectedShipmentType = rateProcessingSetupWizard.SelectedShipmentType;
                    counterRatesProcessingArgs.SelectedRate = rateProcessingSetupWizard.SelectedRate;

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
            // Disable the ShipSense timer and make one final call to synchronize to make sure we have
            // everything matching that should be matching; otherwise changing a value and closing
            // the dialog before the timer kicks would result in shipments being out of sync
            shipSenseChangedTimer.Enabled = false;
            SynchronizeWithShipSense();

            Cursor.Current = Cursors.WaitCursor;

            if (ServiceControl != null)
            {
                ServiceControl.SuspendRateCriteriaChangeEvent();
            }

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

            validatedAddressScope.Dispose();
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

        /// <summary>
        /// Called when [tab selecting].
        /// </summary>
        private void OnTabSelecting(object sender, TabControlCancelEventArgs e)
        {
            // Hide the rates panel if we aren't on the service tab.
            ratesSplitContainer.Panel2Collapsed = tabControl.SelectedTab != tabPageService;
        }
    }
}