using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Shared code required to process a set of shipments
    /// </summary>
    [Component]
    public class ShipmentProcessor : IShipmentProcessor
    {
        private readonly IShippingErrorManager errorManager;
        private readonly Func<Control> ownerRetriever;
        private readonly ILifetimeScope lifetimeScope;
        private readonly ILicenseService licenseService;
        private readonly IProcessShipmentsWorkflowFactory workflowFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IShippingSettings shippingSettings;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IActionDispatcher actionDispatcher;
        private Control owner;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public ShipmentProcessor(Func<Control> ownerRetriever, IShippingErrorManager errorManager,
            ILifetimeScope lifetimeScope, ILicenseService licenseService, IMessageHelper messageHelper,
            IProcessShipmentsWorkflowFactory workflowFactory, IShippingSettings shippingSettings,
            ISqlAdapterFactory sqlAdapterFactory, IActionDispatcher actionDispatcher)
        {
            this.actionDispatcher = actionDispatcher;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.shippingSettings = shippingSettings;
            this.messageHelper = messageHelper;
            this.workflowFactory = workflowFactory;
            this.errorManager = errorManager;
            this.ownerRetriever = ownerRetriever;
            this.lifetimeScope = lifetimeScope;
            this.licenseService = licenseService;
        }

        /// <summary>
        /// Process the list of shipments
        /// </summary>
        /// <param name="shipmentsToProcess">The shipments to process.</param>
        /// <param name="shipmentRefresher">The CarrierConfigurationShipmentRefresher</param>
        /// <param name="chosenRateResult">Rate that was chosen to use, if there was any</param>
        /// <param name="counterRateCarrierConfiguredWhileProcessingAction">Execute after a counter rate carrier was configured</param>
        [NDependIgnoreLongMethod]
        public async Task<IEnumerable<ProcessShipmentResult>> Process(object sender, IEnumerable<ShipmentEntity> shipmentsToProcess,
            ICarrierConfigurationShipmentRefresher shipmentRefresher,
            RateResult chosenRateResult, Action counterRateCarrierConfiguredWhileProcessingAction)
        {
            DateTime startingTime = DateTime.UtcNow;

            owner = ownerRetriever();

            // Filter out the ones we know to be already processed, or are not ready
            IEnumerable<ShipmentEntity> shipmentEntities = shipmentsToProcess as IList<ShipmentEntity> ?? shipmentsToProcess.ToList();
            IEnumerable<ShipmentEntity> filteredShipments = shipmentEntities.Where(s => !s.Processed && s.ShipmentType != (int) ShipmentTypeCode.None);

            licenseService.GetLicenses().FirstOrDefault()?.EnforceCapabilities(EnforcementContext.CreateLabel, owner);

            // Create clones to be processed - that way any changes made don't have race conditions with the UI trying to paint with them
            List<ShipmentEntity> clonedShipments = EntityUtility.CloneEntityCollection(filteredShipments);
            int shipmentCount = clonedShipments.Count;
            shipmentRefresher.ProcessingShipments(clonedShipments);

            if (!clonedShipments.Any())
            {
                messageHelper.ShowMessage("There are no shipments to process.");

                return Enumerable.Empty<ProcessShipmentResult>();
            }

            // Check restriction
            if (!licenseService.HandleRestriction(EditionFeature.SelectionLimit, clonedShipments.Count, owner))
            {
                return Enumerable.Empty<ProcessShipmentResult>();
            }

            // Check for shipment type process shipment nudges
            ShowShipmentTypeProcessingNudges(clonedShipments);

            IProcessShipmentsWorkflowResult result;
            IProcessShipmentsWorkflow workflow = workflowFactory.Create(clonedShipments.Count);

            using (CancellationTokenSource cancellationSource = new CancellationTokenSource())
            {
                IProgressProvider progressProvider = new CancellationTokenProgressProvider(cancellationSource);

                // Progress Item
                IProgressReporter workProgress = new ProgressItem("Processing Shipments");
                progressProvider.ProgressItems.Add(workProgress);

                using (messageHelper.ShowProgressDialog("Processing Shipments",
                    "ShipWorks is processing the shipments.", progressProvider, TimeSpan.Zero))
                {
                    result = await workflow.Process(clonedShipments, chosenRateResult, workProgress,
                        cancellationSource, counterRateCarrierConfiguredWhileProcessingAction);
                }
            }

            HandleProcessingException(result);

            // See if we are supposed to open WorldShip
            if (result.WorldshipExported && shippingSettings.FetchReadOnly().WorldShipLaunch)
            {
                WorldShipUtility.LaunchWorldShip(owner);
            }

            RefreshShipSenseStatusForUnprocessedShipments(shipmentRefresher, shipmentEntities, result.OrderHashes);

            shipmentRefresher.FinishProcessing();

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                actionDispatcher.DispatchProcessingBatchFinished(adapter,
                    FinishProcessingBatchTask.CreateExtraData(startingTime, shipmentCount, errorManager.ShipmentCount(), workflow.Name, workflow.ConcurrencyCount));
            }

            ShowPostProcessingMessage(clonedShipments);

            IEnumerable<ProcessShipmentResult> results = clonedShipments
                .Select(CreateResultFromShipment)
                .ToList();

            IMessenger messenger = lifetimeScope.Resolve<IMessenger>();
            messenger.Send(new ShipmentsProcessedMessage(sender, results));

            return results;
        }

        /// <summary>
        /// Shows the post processing message
        /// </summary>
        private void ShowPostProcessingMessage(IEnumerable<ShipmentEntity> processedShipments)
        {
            MethodConditions.EnsureArgumentIsNotNull(processedShipments, nameof(processedShipments));

            bool hasGlobalPost = processedShipments.Any(IsProcessedGlobalPost);

            if (hasGlobalPost)
            {
                IGlobalPostLabelNotification globalPostLabelNotification = lifetimeScope.Resolve<IGlobalPostLabelNotification>();
                if (globalPostLabelNotification.AppliesToCurrentUser())
                {
                    globalPostLabelNotification.Show();
                }
            }
        }

        /// <summary>
        /// Determines whether the shipment is a Processed GlobalPost shipment.
        /// </summary>
        private static bool IsProcessedGlobalPost(ShipmentEntity shipment)
        {
            if (shipment.Processed &&
                shipment.ShipmentType == (int) ShipmentTypeCode.Usps &&
                shipment.Postal != null)
            {
                // We have a processed USPS shipment. Now check for the GlobalPost service type
                return PostalUtility.IsGlobalPost((PostalServiceType) shipment.Postal.Service);
            }

            return false;
        }

        /// <summary>
        /// Refresh/update the ShipSense status of any unprocessed shipments that are outside of the current context
        /// </summary>
        /// <remarks>This was initially intended to apply ShipSense to all unprocessed shipments outside of
        /// the shipping dialog, but now that we have the shipping panel it applies to more.</remarks>
        private void RefreshShipSenseStatusForUnprocessedShipments(ICarrierConfigurationShipmentRefresher shipmentRefresher,
            IEnumerable<ShipmentEntity> shipmentsToProcess, IEnumerable<string> orderHashes)
        {
            // Exclude shipments that are in the current context but are not being processed,
            // like the list of shipments in the shipping dialog
            IEnumerable<long> otherShipments = shipmentRefresher.RetrieveShipments?.Invoke().Select(s => s.ShipmentID) ??
                Enumerable.Empty<long>();

            // Exclude the shipments that are currently being processed, as well
            List<long> shipmentIdsToIgnore = otherShipments
                .Union(shipmentsToProcess.Select(x => x.ShipmentID))
                .ToList();

            Knowledgebase knowledgebase = new Knowledgebase();
            foreach (string hash in orderHashes.Distinct())
            {
                knowledgebase.RefreshShipSenseStatus(hash, shipmentIdsToIgnore);
            }
        }

        /// <summary>
        /// Create a process shipment result from the given shipment
        /// </summary>
        private ProcessShipmentResult CreateResultFromShipment(ShipmentEntity shipment) =>
            new ProcessShipmentResult(shipment, errorManager.GetErrorForShipment(shipment.ShipmentID));

        /// <summary>
        /// Handle an exception raised during processing, if possible
        /// </summary>
        private void HandleProcessingException(IProcessShipmentsWorkflowResult executionState)
        {
            // If any accounts were out of funds we show that instead of the errors
            if (executionState.OutOfFundsException != null)
            {
                DialogResult answer = messageHelper.ShowQuestion(
                    $"You do not have sufficient funds in {executionState.OutOfFundsException.Provider} account {executionState.OutOfFundsException.AccountIdentifier} to continue shipping.\n\n" +
                    "Would you like to purchase more now?");

                if (answer == DialogResult.OK)
                {
                    using (Form dlg = executionState.OutOfFundsException.CreatePostageDialog(lifetimeScope))
                    {
                        dlg.ShowDialog(owner);
                    }
                }
            }
            else if (executionState.TermsAndConditionsException != null)
            {
                messageHelper.ShowError(executionState.NewErrors.FirstOrDefault());
                executionState.TermsAndConditionsException.OpenTermsAndConditionsDlg(lifetimeScope);
            }
            else
            {
                if (!executionState.NewErrors.Any())
                {
                    return;
                }

                string message = executionState.NewErrors.Take(3)
                    .Aggregate("Some errors occurred during processing.", (x, y) => x + "\n\n" + y);

                if (executionState.NewErrors.Count > 3)
                {
                    message += "\n\nSee the shipment list for all errors.";
                }

                messageHelper.ShowError(message);
            }
        }

        /// <summary>
        /// Checks for any Process Shipment nudges that might pertain to processing the referenced list of shipments.
        /// </summary>
        private void ShowShipmentTypeProcessingNudges(IEnumerable<ShipmentEntity> shipments)
        {
            // If there is an Endicia shipment in the list, check for ProcessEndicia nudges
            if (shipments.Any(s => s.ShipmentTypeCode == ShipmentTypeCode.Endicia))
            {
                NudgeManager.ShowNudge(owner, NudgeManager.GetFirstNudgeOfType(NudgeType.ProcessEndicia));
            }
        }
    }
}
