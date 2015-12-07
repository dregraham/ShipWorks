using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Adapter.Custom;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Connection;
using log4net;
using ShipWorks.Data;
using ShipWorks.Users;
using Interapptive.Shared.Net;
using System.Net;
using System.Diagnostics;
using System.Xml.Linq;
using ShipWorks.Stores;
using ShipWorks.Shipping;
using System.Xml;
using Interapptive.Shared;
using Interapptive.Shared.Data;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Responsible for checking messages published from on the server for display in the dashboard.
    /// </summary>
    public static class ServerMessageManager
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ServerMessageManager));

        static List<ServerMessageEntity> activeMessages;

        // Max row versions we have retrieved so far
        static byte[] messageRowVersion;
        static byte[] signoffRowVersion;

        static string feedEndpoint = "http://www.interapptive.com/shipworks/messagefeed.php";

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            // If there's no UI, we don't need it
            if (!Program.ExecutionMode.IsUISupported)
            {
                return;
            }

            activeMessages = new List<ServerMessageEntity>();

            messageRowVersion = new byte[8];
            signoffRowVersion = new byte[8];

            // The initial database load is different than the incremental check
            InitialLoadFromDatabase();
        }

        /// <summary>
        /// Check tango for the latest server messsages.
        /// </summary>
        public static void CheckLatestServerMessages()
        {
            int maxNumber = 0;

            // First we have to determine what the highest known message is
            object maxQuery = SqlAdapter.Default.GetScalar(ServerMessageFields.Number, AggregateFunction.Max);
            if (!(maxQuery is DBNull))
            {
                maxNumber = Convert.ToInt32(maxQuery);
            }

            try
            {
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                request.Uri = new Uri(feedEndpoint);
                request.Variables.Add("start", maxNumber.ToString());

                using (IHttpResponseReader response = request.GetResponse())
                {
                    ProcessServerMessageFeed(response.ReadResult());
                }
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    Debug.Fail(ex.Message, ex.ToString());
                    log.Error("Failed to connect to server message feed.", ex);
                }
                else
                {
                    throw;
                }
            }

            // We also need to make sure that we expire any messages that were set to auto expire
            using (SqlAdapter adapter = new SqlAdapter())
            {
                ServerMessageEntity prototype = new ServerMessageEntity();
                prototype.Active = false;

                adapter.UpdateEntitiesDirectly(prototype, new RelationPredicateBucket(
                    ServerMessageFields.Active == true &
                    ServerMessageFields.Expires < DateTime.UtcNow));
            }
        }

        /// <summary>
        /// Read and process the given server message feed content.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void ProcessServerMessageFeed(string feedXml)
        {
            try
            {
                XDocument xDocument = XDocument.Parse(feedXml);

                // Have to process low numbers to high numbers
                foreach (XElement xMessage in xDocument.Descendants("Message").OrderBy(x => (int) x.Element("Number")))
                {
                    int number = (int) xMessage.Element("Number");
                    DateTime published = (DateTime) xMessage.Element("Published");
                    bool dismissable = (bool) xMessage.Element("Dismissable");

                    DateTime? expires = (DateTime?) xMessage.Element("Expires");
                    int? responseTo = (int?) xMessage.Element("ResponseTo");
                    int? responseAction = (int?) xMessage.Element("ResponseAction");
                    int? editTo = (int?) xMessage.Element("EditTo");

                    string image = (string) xMessage.Element("Image");
                    string primaryText = (string) xMessage.Element("PrimaryText");
                    string secondaryText = (string) xMessage.Element("SecondaryText");

                    if (image == null)
                    {
                        throw new NullReferenceException("'Image' cannot be null.");
                    }

                    if (primaryText == null)
                    {
                        throw new NullReferenceException("'PrimaryText' cannot be null.");
                    }

                    if (secondaryText == null)
                    {
                        throw new NullReferenceException("'SecondaryText' cannot be null.");
                    }

                    string actions = xMessage.Element("Actions").ToString(SaveOptions.DisableFormatting);
                    string stores = xMessage.Element("Stores").ToString(SaveOptions.DisableFormatting);
                    string shippers = xMessage.Element("Shippers").ToString(SaveOptions.DisableFormatting);

                    ServerMessageEntity message = new ServerMessageEntity();
                    message.Number = number;
                    message.Published = published;
                    message.Dismissable = dismissable;
                    message.Expires = expires;
                    message.ResponseTo = responseTo;
                    message.ResponseAction = responseAction;
                    message.EditTo = editTo;
                    message.Image = image;
                    message.PrimaryText = primaryText;
                    message.SecondaryText = secondaryText;
                    message.Actions = actions;
                    message.Stores = stores;
                    message.Shippers = shippers;

                    ProcessIncomingServerMessage(message);
                }
            }
            // Invalid XML document
            catch (XmlException ex)
            {
                throw new ServerMessageFeedException(ex.Message, ex);
            }
            // Could not cast\convert property
            catch (FormatException ex)
            {
                throw new ServerMessageFeedException(ex.Message, ex);
            }
            // Missing XML element
            catch (NullReferenceException ex)
            {
                throw new ServerMessageFeedException(ex.Message, ex);
            }
            // Missing XML element
            catch (ArgumentNullException ex)
            {
                throw new ServerMessageFeedException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Process the incoming newly recieved and not yet saved server message
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void ProcessIncomingServerMessage(ServerMessageEntity message)
        {
            // Start out assuming its not active
            message.Active = false;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // If the message is not in response to anything, its just a standard incoming message
                if (message.ResponseTo == null)
                {
                    // So far its active
                    message.Active = true;
                }
                else
                {
                    int responseToNumber = message.ResponseTo.Value;
                    ServerMessageResponseAction responseAction = (ServerMessageResponseAction) message.ResponseAction;

                    // A standard response means this message can only be active if the original message exists
                    if (responseAction == ServerMessageResponseAction.FollowUp)
                    {
                        // its active state depends on if the original exists
                        message.Active = IsOriginalMessageExistant(adapter, responseToNumber);

                        if (!message.Active)
                        {
                            log.InfoFormat("Recieved message {0} from server but original message not found.", message.Number);
                        }
                    }

                    // A dismiss response means just try to find the original message, and dismiss it if it exists
                    if (responseAction == ServerMessageResponseAction.Dismiss)
                    {
                        // This type is never active - its only sent to dismiss an existing one
                        message.Active = false;

                        ServerMessageCollection messages = ServerMessageCollection.Fetch(adapter, ServerMessageFields.Number == responseToNumber);
                        if (messages.Count == 1)
                        {
                            log.InfoFormat("Dismissing message {0} due to message {1}.", responseToNumber, message.Number);

                            ServerMessageEntity originalMessage = messages[0];
                            originalMessage.Active = false;
                            adapter.SaveEntity(originalMessage);
                        }
                        else
                        {
                            log.InfoFormat("Message {1} did not find message {0} to dismiss.", responseToNumber, message.Number);
                        }
                    }
                }

                // If its an edit, try to make the one we are editing go away
                if (message.EditTo != null)
                {
                    ServerMessageCollection messages = ServerMessageCollection.Fetch(adapter, ServerMessageFields.Number == message.EditTo.Value);
                    if (messages.Count == 1)
                    {
                        log.InfoFormat("Deactivating message {0} due to edit message {1}.", message.EditTo, message.Number);

                        ServerMessageEntity originalMessage = messages[0];
                        originalMessage.Active = false;
                        adapter.SaveEntity(originalMessage);
                    }
                }

                // If its still active after coming this far, we have to run it through the affected shippers and store types
                if (message.Active)
                {
                    message.Active = IsAnyAffectedShipperInUse(message.Shippers) && IsAnyAffectedStoreInUse(message.Stores);
                }

                // Save the message to the database
                try
                {
                    adapter.SaveEntity(message);
                }
                catch (ORMQueryExecutionException ex)
                {
                    // If the message already exists then just let it be - it could be that either Tango is screwing up
                    // and not respecting our start number, or that another copy of sw was running and we had a race condition
                    // and were both downloading at once.
                    if (ex.Message.Contains("IX_ServerMessage_Number"))
                    {
                        log.WarnFormat("Message {0} already found in database.", message.Number);
                    }
                    else
                    {
                        throw;
                    }
                }

                adapter.Commit();
            }
        }

        /// <summary>
        /// Indicates if the given message - or the message it was originally a follow up to - is present in the database.
        /// </summary>
        private static bool IsOriginalMessageExistant(SqlAdapter adapter, int number)
        {
            ServerMessageCollection messages = ServerMessageCollection.Fetch(adapter, ServerMessageFields.Number == number);
            if (messages.Count == 0)
            {
                return false;
            }

            ServerMessageEntity message = messages[0];

            // If this message was a follow-up to another, see if its original exists
            if (message.ResponseTo != null && message.ResponseAction == (int) ServerMessageResponseAction.FollowUp)
            {
                return IsOriginalMessageExistant(adapter, message.ResponseTo.Value);
            }

            return true;
        }

        /// <summary>
        /// Determines if the current database is affected by any of the stores contained in the given XML.
        /// </summary>
        private static bool IsAnyAffectedStoreInUse(string storesXml)
        {
            // If none are specified, assume its affected
            if (string.IsNullOrEmpty(storesXml))
            {
                return true;
            }

            XElement xElement = XElement.Parse(storesXml);
            List<string> affectedCodes = xElement.Descendants("StoreCode").Select(c => (string) c).ToList();

            // If no store codes were specified, then assume they are all affected
            if (affectedCodes.Count == 0)
            {
                return true;
            }

            // Get all the store codes for stores being used
            List<string> existingCodes = StoreManager.GetUniqueStoreTypes().Select(t => t.TangoCode).ToList();

            // If there is any interesction, there is an affected store.
            bool affected = existingCodes.Intersect(affectedCodes).Count() > 0;

            if (!affected)
            {
                log.InfoFormat("Message being deactivated due to unaffected stores.");
            }

            return affected;
        }

        /// <summary>
        /// Determines if any shipments in the database match any of the shippers in the given XML.
        /// </summary>
        private static bool IsAnyAffectedShipperInUse(string shippersXml)
        {
            // If none are specified, assume its affected
            if (string.IsNullOrEmpty(shippersXml))
            {
                return true;
            }

            XElement xElement = XElement.Parse(shippersXml);
            List<ShipmentTypeCode> affectedTypes = xElement.Descendants("Shipper").Select(s => (ShipmentTypeCode) (int) s).ToList();

            // If no types were specified, then assume they are all affected
            if (affectedTypes.Count == 0)
            {
                return true;
            }

            ExcludeIncludeFieldsList includeFields = new ExcludeIncludeFieldsList();
            includeFields.ExcludeContainedFields = false;
            includeFields.Add(ShipmentFields.ShipmentID);

            ShipmentCollection shipments = new ShipmentCollection();
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(shipments,
                    new RelationPredicateBucket(ShipmentFields.ShipmentType == affectedTypes.Select(c => (int) c).ToArray() & ShipmentFields.Processed == true), 
                    1,
                    null,
                    null,
                    includeFields);

                if (shipments.Count == 0)
                {
                    log.InfoFormat("Message being deactivated due to unaffected shipments.");
                }

                return shipments.Count != 0;
            }
        }

        /// <summary>
        /// Do the initial load of server messages from the database
        /// </summary>
        private static void InitialLoadFromDatabase()
        {
            // Get our max message timestamp value
            object objMessageTimestamp = SqlAdapter.Default.GetScalar(ServerMessageFields.RowVersion, AggregateFunction.Max);
            if (!(objMessageTimestamp is DBNull))
            {
                messageRowVersion = (byte[]) objMessageTimestamp;
                if (messageRowVersion.Length == 0)
                {
                    messageRowVersion = new byte[8];
                }
            }

            // Get our max signoff timestamp value
            object objSignoffTimestamp = SqlAdapter.Default.GetScalar(ServerMessageSignoffFields.RowVersion, AggregateFunction.Max);
            if (!(objSignoffTimestamp is DBNull))
            {
                signoffRowVersion = (byte[]) objSignoffTimestamp;
                if (signoffRowVersion.Length == 0)
                {
                    signoffRowVersion = new byte[8];
                }
            }

            // They have to be active
            RelationPredicateBucket bucket = new RelationPredicateBucket(ServerMessageFields.Active == true);

            // NOT IN (SELECT ServerMessageID FROM SererMEssageSignoff WHERE ComputerID = @ AND UserID = 2
            FieldCompareSetPredicate notInSignoff = new FieldCompareSetPredicate(
                ServerMessageFields.ServerMessageID, null, ServerMessageSignoffFields.ServerMessageID, null, SetOperator.In,
                ServerMessageSignoffFields.ComputerID == UserSession.Computer.ComputerID &
                ServerMessageSignoffFields.UserID == UserSession.User.UserID);
            notInSignoff.Negate = true;

            // Add the subquery
            bucket.PredicateExpression.AddWithAnd(notInSignoff);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                ServerMessageCollection messages = new ServerMessageCollection();
                adapter.FetchEntityCollection(messages, bucket);

                // These are all active messages
                activeMessages.AddRange(messages);

                log.InfoFormat("Loaded {0} initial active server messsages.", activeMessages.Count);
            }
        }

        /// <summary>
        /// Check the database for any changes to server messages or signoffs
        /// </summary>
        public static void CheckDatabaseForChanges()
        {
            // Get all server messages from the database that have changed since last check
            ServerMessageCollection changedMessages = GetChangedServerMessages();

            // Go through each changed message.  We are checking for:
            // 1) New messages that are active
            // 2) Messages that are not active, that we need to remove from our active list.
            foreach (ServerMessageEntity message in changedMessages)
            {
                // See if this message exists in our active list
                ServerMessageEntity existing = activeMessages.Where(a => a.ServerMessageID == message.ServerMessageID).SingleOrDefault();

                // If it does not exist in our list, and it is active, then we need to add it to our active list
                if (existing == null && message.Active)
                {
                    log.InfoFormat("Adding active message {0}", message.Number);
                    activeMessages.Add(message);
                }

                // If it does exist in our active list, and it is not active, we need to remove it
                if (existing != null && !message.Active)
                {
                    log.InfoFormat("Removing inactive message {0}", message.Number);
                    activeMessages.Remove(existing);
                }
            }

            // Last we need to remove any active messages, but that have been signed off by this particular logged in user.
            RemoveSignedOffMessages();
        }

        /// <summary>
        /// Get any server messages that are new or changed since the last time we checked
        /// </summary>
        private static ServerMessageCollection GetChangedServerMessages()
        {
            ServerMessageCollection messages = new ServerMessageCollection();

            RelationPredicateBucket bucket = new RelationPredicateBucket(
                new FieldCompareValuePredicate(ServerMessageFields.RowVersion, null,
                ComparisonOperator.GreaterThan,
                messageRowVersion));

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(messages, bucket);
            }

            if (messages.Count > 0)
            {
                log.InfoFormat("Found {0} changed server messages in database.", messages.Count);
            }

            // Remember the larget timestamp for next time
            foreach (ServerMessageEntity message in messages)
            {
                if (SqlUtility.GetTimestampValue(message.RowVersion) > SqlUtility.GetTimestampValue(messageRowVersion))
                {
                    messageRowVersion = message.RowVersion;
                }
            }

            return messages;
        }

        /// <summary>
        /// Checks the active message list and removes any that have been signed off by the currently logged in user on the running computer.
        /// </summary>
        private static void RemoveSignedOffMessages()
        {
            ServerMessageSignoffCollection signoffs = new ServerMessageSignoffCollection();

            RelationPredicateBucket bucket = new RelationPredicateBucket(
                new FieldCompareValuePredicate(ServerMessageSignoffFields.RowVersion, null,
                ComparisonOperator.GreaterThan,
                signoffRowVersion));

            // Also restrict it to this user on this computer
            bucket.PredicateExpression.AddWithAnd(
                ServerMessageSignoffFields.UserID == UserSession.User.UserID &
                ServerMessageSignoffFields.ComputerID == UserSession.Computer.ComputerID);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(signoffs, bucket);
            }

            if (signoffs.Count > 0)
            {
                log.InfoFormat("Found {0} new signoffs in database.", signoffs.Count);
            }

            // Go through each new signoff and see if there are any in our active list that need removed
            foreach (ServerMessageSignoffEntity signoff in signoffs)
            {
                ServerMessageEntity message = activeMessages.Where(a => a.ServerMessageID == signoff.ServerMessageID).SingleOrDefault();

                // It's signed off now, remove it from the active list
                if (message != null)
                {
                    log.InfoFormat("Removing signed off message {0}", message.Number);
                    activeMessages.Remove(message);
                }

                // Remember the largest timestamp we have seen for next time
                if (SqlUtility.GetTimestampValue(signoff.RowVersion) > SqlUtility.GetTimestampValue(signoffRowVersion))
                {
                    signoffRowVersion = signoff.RowVersion;
                }
            }
        }

        /// <summary>
        /// The current list of active server messages.  This list is specific to the logged in user on the current computer.
        /// </summary>
        public static ICollection<ServerMessageEntity> ActiveMessages
        {
            get
            {
                return activeMessages;
            }
        }

    }
}
