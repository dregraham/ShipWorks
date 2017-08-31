using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.Ebay.CoreExtensions.Actions
{
    /// <summary>
    /// Task for sending shipment notification email to the buyer's eBay inbox.
    /// </summary>
    [ActionTask("Send eBay message to buyer", "EbayMessageBuyer", ActionTaskCategory.UpdateOnline)]
    public class EbayMessageBuyerTask : StoreTypeTaskBase
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(EbayMessageBuyerTask));
        private readonly IEbayOnlineUpdater updater;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayMessageBuyerTask(IEbayOnlineUpdater updater)
        {
            this.updater = updater;
        }

        /// <summary>
        /// Type of message to send to the eBay member.
        /// This gets translated down to "QuestionType" in
        /// eBay api parlance.
        /// </summary>
        public EbaySendMessageType MessageType { get; set; }

        /// <summary>
        /// Directs eBay to also send a copy of the message
        /// to the user's eBay inbox.
        /// </summary>
        public bool CopyMe { get; set; }

        /// <summary>
        /// Message Subject
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// eBay message body
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// Contextual hint label
        /// </summary>
        public override string InputLabel => "Message using: ";

        /// <summary>
        /// Specify the entity type supported by the task
        /// </summary>
        public override EntityType? InputEntityType => EntityType.OrderItemEntity;

        /// <summary>
        /// Only applies to eBay stores
        /// </summary>
        public override bool SupportsType(StoreType storeType) => storeType is EbayStoreType;

        /// <summary>
        /// Creates the task editor for configuring the task
        /// </summary>
        public override ActionTaskEditor CreateEditor() => new EbayMessageBuyerTaskEditor(this);

        /// <summary>
        /// This task should be run asynchronously
        /// </summary>
        public override bool IsAsync => true;

        /// <summary>
        /// Execute the eBay message sending
        /// </summary>
        protected override async Task RunAsync(List<long> inputKeys)
        {
            try
            {
                foreach (long entityId in inputKeys)
                {
                    // Perform token processing on the message to be sent
                    string processedSubject = TemplateTokenProcessor.ProcessTokens(Subject, entityId);
                    string processedMessage = TemplateTokenProcessor.ProcessTokens(Body, entityId);

                    EbayStoreEntity ebayStore = StoreManager.GetRelatedStore(entityId) as EbayStoreEntity;

                    if (ebayStore != null)
                    {
                        await updater.SendMessage(ebayStore, entityId, MessageType, processedSubject, processedMessage, CopyMe).ConfigureAwait(false);
                    }
                    else
                    {
                        log.InfoFormat("Unable to send eBay message for entity {0} because it is not related to an eBay store.", entityId);
                    }
                }
            }
            catch (EbayException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }
    }
}
