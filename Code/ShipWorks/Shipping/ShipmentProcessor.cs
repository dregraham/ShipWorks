using Autofac;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Setup;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Shared code required to process a set of shipments
    /// </summary>
    public class ShipmentProcessor : IShipmentProcessor
    {
        private bool showBestRateCounterRateSetupWizard;
        private bool cancelProcessing;
        private readonly IShippingErrorManager errorManager;
        private readonly IShippingManager shippingManager;
        private readonly Func<Control> ownerRetriever;
        private readonly ILifetimeScope lifetimeScope;
        private RateResult chosenRate;
        private int shipmentCount;
        private Action counterRateCarrierConfiguredWhileProcessing;
        Control owner;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentProcessor(Func<Control> ownerRetriever, IShippingErrorManager errorManager, 
            ILifetimeScope lifetimeScope, IShippingManager shippingManager)
        {
            this.shippingManager = shippingManager;
            this.errorManager = errorManager;
            this.ownerRetriever = ownerRetriever;
            this.lifetimeScope = lifetimeScope;
        }
        
        /// <summary>
        /// Filtered rates that should be displayed after shipping
        /// </summary>
        public RateGroup FilteredRates { get; private set; }

        /// <summary>
        /// Process the list of shipments
        /// </summary>
        /// <param name="shipments">List of shipments to process</param>
        /// <param name="chosenRate">Rate that was chosen to use, if there was any</param>
        /// <param name="counterRateCarrierConfiguredWhileProcessing">Execute after a counter rate carrier was configured</param>
        /// <returns></returns>
        public Task<IEnumerable<ShipmentEntity>> Process(IEnumerable<ShipmentEntity> shipments, ICarrierConfigurationShipmentRefresher shipmentRefresher, RateResult chosenRate, Action counterRateCarrierConfiguredWhileProcessing)
        {
            owner = ownerRetriever();

            // Filter out the ones we know to be already processed, or are not ready
            shipments = shipments.Where(s => !s.Processed && s.ShipmentType != (int)ShipmentTypeCode.None);

            this.counterRateCarrierConfiguredWhileProcessing = counterRateCarrierConfiguredWhileProcessing;
            this.chosenRate = chosenRate;

            cancelProcessing = false;

            // Create clones to be processed - that way any changes made don't have race conditions with the UI trying to paint with them
            shipments = EntityUtility.CloneEntityCollection(shipments);
            shipmentCount = shipments.Count();
            shipmentRefresher.ProcessingShipments(shipments);

            if (!shipments.Any())
            {
                MessageHelper.ShowMessage(owner, "There are no shipments to process.");

                return TaskEx.FromResult(Enumerable.Empty<ShipmentEntity>());
            }

            // Check restriction
            if (!EditionManager.HandleRestrictionIssue(owner, EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.SelectionLimit, shipments.Count())))
            {
                return TaskEx.FromResult(Enumerable.Empty<ShipmentEntity>());
            }

            // Check for shipment type process shipment nudges
            ShowShipmentTypeProcessingNudges(shipments);

            BackgroundExecutor<ShipmentEntity> executor = new BackgroundExecutor<ShipmentEntity>(owner,
                "Processing Shipments",
                "ShipWorks is processing the shipments.",
                "Shipment {0} of {1}",
                false);

            List<string> newErrors = new List<string>();
            IDictionary<ShipmentEntity, Exception> concurrencyErrors = new Dictionary<ShipmentEntity, Exception>();

            // Special cases - I didn't think we needed to abstract these.  If it gets out of hand we should refactor.
            bool worldshipExported = false;
            IInsufficientFunds outOfFundsException = null;

            // Maps storeID's to license exceptions, so we only have to check a store once per processing batch
            Dictionary<long, Exception> licenseCheckResults = new Dictionary<long, Exception>();

            List<string> orderHashes = new List<string>();
            
            TaskCompletionSource<IEnumerable<ShipmentEntity>> completionSource = new TaskCompletionSource<IEnumerable<ShipmentEntity>>();

            // What to do before it gets started (but is on the background thread)
            executor.ExecuteStarting += (object s, EventArgs args) =>
            {
                // Force the shipments to save - this weeds out any shipments early that have been edited by another user on another computer.
                concurrencyErrors = shippingManager.SaveShipmentsToDatabase(shipments, ValidatedAddressScope.Current, true);

                // Reset to true, so that we show the counter rate setup wizard for this batch.
                showBestRateCounterRateSetupWizard = true;
            };
            
            // Code to execute once background load is complete
            executor.ExecuteCompleted += (sender, e) =>
            {
                HandleProcessingException(outOfFundsException, newErrors);

                // See if we are supposed to open WorldShip
                if (worldshipExported && ShippingSettings.Fetch().WorldShipLaunch)
                {
                    WorldShipUtility.LaunchWorldShip(owner);
                }
                
                // Refresh/update the ShipSense status of any unprocessed shipments that are outside of the shipping dialog
                Knowledgebase knowledgebase = new Knowledgebase();
                foreach (string hash in orderHashes.Distinct())
                {
                    knowledgebase.RefreshShipSenseStatus(hash, shipmentRefresher.RetrieveShipments?.Invoke().Select(s => s.ShipmentID) ?? Enumerable.Empty<long>());
                }

                shipmentRefresher.FinishProcessing();

                completionSource.SetResult(shipments);
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

                    Func<CounterRatesProcessingArgs, DialogResult> ratesProcessing = shipment.ShipmentType == (int)ShipmentTypeCode.BestRate ?
                        (Func<CounterRatesProcessingArgs, DialogResult>)BestRateCounterRatesProcessing :
                        CounterRatesProcessing;
                    
                    ShippingManager.ProcessShipment(shipmentID, licenseCheckResults, 
                        x => (DialogResult)owner.Invoke(ratesProcessing, x), 
                        selectedRate, lifetimeScope);

                    // Clear any previous errors
                    errorManager.Remove(shipmentID);

                    // Special case - could refactor to abstract if necessary
                    worldshipExported |= shipment.ShipmentType == (int)ShipmentTypeCode.UpsWorldShip;
                }
                catch (ORMConcurrencyException ex)
                {
                    errorMessage = errorManager.SetShipmentErrorMessage(shipmentID, ex, "processed");
                }
                catch (ObjectDeletedException ex)
                {
                    errorMessage = errorManager.SetShipmentErrorMessage(shipmentID, ex, "processed");
                }
                catch (SqlForeignKeyException ex)
                {
                    errorMessage = errorManager.SetShipmentErrorMessage(shipmentID, ex, "processed");
                }
                catch (ShippingException ex)
                {
                    errorMessage = errorManager.SetShipmentErrorMessage(shipmentID, ex);

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
                        errorManager.SetShipmentErrorMessage(shipmentID, new ShippingException(errorMessage, ex));
                    }
                }

                if (errorMessage != null)
                {
                    newErrors.Add("Order " + shipment.Order.OrderNumberComplete + ": " + errorMessage);
                }
            }, shipments, chosenRate); // Each shipment to execute the code for
            
            return completionSource.Task;
        }

        /// <summary>
        /// Handle an exception raised during processing, if possible
        /// </summary>
        private void HandleProcessingException(IInsufficientFunds outOfFundsException, List<string> newErrors)
        {
            // If any accounts were out of funds we show that instead of the errors
            if (outOfFundsException != null)
            {
                DialogResult answer = MessageHelper.ShowQuestion(owner,
                    $"You do not have sufficient funds in {outOfFundsException.Provider} account {outOfFundsException.AccountIdentifier} to continue shipping.\n\n" +
                    "Would you like to purchase more now?");

                if (answer == DialogResult.OK)
                {
                    using (Form dlg = outOfFundsException.CreatePostageDialog())
                    {
                        dlg.ShowDialog(owner);
                    }
                }
            }
            else
            {
                if (!newErrors.Any())
                {
                    return;
                }

                string message = "Some errors occurred during processing.\n\n";

                foreach (string error in newErrors.Take(3))
                {
                    message += error + "\n\n";
                }

                if (newErrors.Count > 3)
                {
                    message += "See the shipment list for all errors.";
                }

                MessageHelper.ShowError(owner, message);
            }
        }

        /// <summary>
        /// Checks for any Process Shipment nudges that might pertain to processing the referenced list of shipments.
        /// </summary>
        private void ShowShipmentTypeProcessingNudges(IEnumerable<ShipmentEntity> shipments)
        {
            // Get a distinct list of shipment types from the list of shipments to process
            List<ShipmentTypeCode> shipmentTypeCodes = shipments.Select(s => (ShipmentTypeCode)s.ShipmentType).Distinct().ToList();

            // If there is an Endicia shipment in the list, check for ProcessEndicia nudges
            if (shipmentTypeCodes.Contains(ShipmentTypeCode.Endicia))
            {
                NudgeManager.ShowNudge(owner, NudgeManager.GetFirstNudgeOfType(NudgeType.ProcessEndicia));
            }
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

            if (cancelProcessing)
            {
                return DialogResult.Cancel;
            }

            // If this shipment type is not allowed to have new registrations, cancel out.
            if (!shipmentType.IsAccountRegistrationAllowed)
            {
                MessageHelper.ShowWarning(owner, $"Account registration is disabled for {EnumHelper.GetDescription(shipmentType.ShipmentTypeCode)}");
                return DialogResult.Cancel;
            }
            
            using (ShipmentTypeSetupWizardForm setupWizard = shipmentType.CreateSetupWizard(lifetimeScope))
            {
                result = setupWizard.ShowDialog(owner);

                if (result == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(shipmentType.ShipmentTypeCode);

                    // Make sure we've got the latest data for the shipment since the requested label format may have changed
                    ShippingManager.RefreshShipment(counterRatesProcessingArgs.Shipment);

                    ShippingManager.EnsureShipmentLoaded(counterRatesProcessingArgs.Shipment);

                    counterRateCarrierConfiguredWhileProcessing?.Invoke();
                }
                else
                {
                    // User canceled out of the setup wizard for this batch, so don't show
                    // any setup wizard for the rest of this batch
                    cancelProcessing = true;
                }

                return result;
            }
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
            
            RateResult selectedRate = chosenRate ?? counterRatesProcessingArgs.FilteredRates.Rates.First();

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

            if (setupWizardDialogResult != DialogResult.OK)
            {
                cancelProcessing = true;
                showBestRateCounterRateSetupWizard = false;

                FilteredRates = counterRatesProcessingArgs.FilteredRates;
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

            using (CounterRateProcessingSetupWizard rateProcessingSetupWizard = new CounterRateProcessingSetupWizard(counterRatesProcessingArgs.FilteredRates, selectedRate, shipmentCount))
            {
                setupWizardDialogResult = rateProcessingSetupWizard.ShowDialog(owner);
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
    }
}
