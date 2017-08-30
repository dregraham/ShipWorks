using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms;
using ShipWorks.Data.Model;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Tokens;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using log4net;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using System.Threading.Tasks;

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

        EbaySendMessageType messageType;
        bool copyMe = false;
        string subject = "";
        string body = "";

        /// <summary>
        /// Type of message to send to the eBay member.
        /// This gets translated down to "QuestionType" in 
        /// eBay api parlance.
        /// </summary>
        public EbaySendMessageType MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        /// <summary>
        /// Directs eBay to also send a copy of the message
        /// to the user's eBay inbox.
        /// </summary>
        public bool CopyMe
        {
            get { return copyMe; }
            set { copyMe = value; }
        }

        /// <summary>
        /// Message Subject
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        /// <summary>
        /// eBay message body
        /// </summary>
        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        /// <summary>
        /// Contextual hint label
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Message using: ";
            }
        }

        /// <summary>
        /// Specify the entity type supported by the task
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.OrderItemEntity;
            }
        }

        /// <summary>
        /// Only applies to eBay stores
        /// </summary>
        public override bool SupportsType(StoreType storeType)
        {
            return storeType is EbayStoreType;
        }

        /// <summary>
        /// Creates the task editor for configuring the task
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new EbayMessageBuyerTaskEditor(this);
        }

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
                    string processedSubject = TemplateTokenProcessor.ProcessTokens(subject, entityId);
                    string processedMessage = TemplateTokenProcessor.ProcessTokens(body, entityId);

                    EbayStoreEntity ebayStore = StoreManager.GetRelatedStore(entityId) as EbayStoreEntity;

                    if (ebayStore != null)
                    {
                        EbayOnlineUpdater updater = new EbayOnlineUpdater(ebayStore);
                        await updater.SendMessage(entityId, messageType, processedSubject, processedMessage, copyMe).ConfigureAwait(false);
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
