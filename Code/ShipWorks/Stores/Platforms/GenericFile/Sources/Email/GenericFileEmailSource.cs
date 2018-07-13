using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using Rebex.Net;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Utility;
using Rebex.Mail;
using System.Text.RegularExpressions;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.Email
{
    /// <summary>
    /// Implementatino of the GenericFileSource for email
    /// </summary>
    public class GenericFileEmailSource : GenericFileSource
    {
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileEmailSource));

        GenericFileStoreEntity store;

        // The open imap connection
        Imap imap = null;

        // The current list of messages we are processing
        List<string> messages = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileEmailSource(GenericFileStoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;
        }

        /// <summary>
        /// Dispose of used resources
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (imap != null)
                {
                    try
                    {
                        imap.Disconnect();
                        imap.Dispose();
                    }
                    catch (Exception ex)
                    {
                        // Eat it, what else could we do?
                        log.Error("Error logging out of IMAP", ex);
                    }

                    imap = null;
                }
            }
        }

        /// <summary>
        /// Load the message list of unique ID's that need to be processed
        /// </summary>
        private void LoadMessageList()
        {
            if (imap != null)
            {
                throw new InvalidOperationException("Shouldn't be here if imap was already initialized.");
            }

            try
            {
                imap = EmailUtility.LogonToImap(EmailAccountManager.GetAccount(store.EmailAccountID.Value));
                imap.SelectFolder(store.EmailIncomingFolder);

                // We don't want to set the Seen (Read) flag just by getting messages
                imap.Settings.UsePeekForGetMessage = true;

                // Havn't done a download for this folder yet
                if (store.EmailFolderValidityID == 0)
                {
                    store.EmailFolderValidityID = imap.CurrentFolder.ValidityId;
                    SqlAdapter.Default.SaveAndRefetch(store);
                }
                else
                {
                    if (store.EmailFolderValidityID != imap.CurrentFolder.ValidityId)
                    {
                        throw new GenericFileLoadException("The ValidityID of the IMAP folder has changed.  Please contact Interapptive at 1-800-95-APPTIVE for assistance.");
                    }
                }

                long lastUid = store.EmailFolderLastMessageID;

                if (InterapptiveOnly.MagicKeysDown)
                {
                    lastUid = 0;
                }

                // Only look for messages newer (a bigger ID) than the last we know of
                ImapMessageSet messageSet = new ImapMessageSet();
                messageSet.AddRange(
                    ImapMessageSet.BuildUniqueId(store.EmailFolderValidityID, lastUid + 1),
                    ImapMessageSet.BuildUniqueId(store.EmailFolderValidityID, Int32.MaxValue));

                ImapMessageCollection messageList;

                if (store.EmailOnlyUnread)
                {
                    messageList = imap.Search(messageSet, ImapListFields.UniqueId, ImapSearchParameter.Unread);
                }
                else
                {
                    messageList = imap.GetMessageList(messageSet, ImapListFields.UniqueId);
                }

                // We have to make sure they are sorted in ascending UniqueID order. Not sure if they always will be or not, but our download algorithm depends on it,
                // so we do it.
                messages = messageList
                    .Select(m => m.UniqueId)
                    .OrderBy(rebexID => GetUnqiueID(rebexID))
                    .Distinct()
                    .Where(rebexID => GetUnqiueID(rebexID) > lastUid)
                    .ToList();

                log.InfoFormat("Found {0} messages from mailbox to consider", messages.Count);
            }
            catch (EmailLogonException ex)
            {
                throw new GenericFileLoadException("There was an error logging in to the IMAP server:\n\n" + ex.Message, ex);
            }
            catch (ImapException ex)
            {
                throw new GenericFileLoadException("There was an error reading email from the IMAP server:\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get the next email to import data from based on store configuration
        /// </summary>
        public override GenericFileInstance GetNextFile()
        {
            if (messages == null)
            {
                LoadMessageList();
            }

            if (messages.Count == 0)
            {
                return null;
            }

            try
            {
                // In case of subject pattern matching, we may have to keep looping till we find one that's ok
                while (messages.Count > 0)
                {
                    string rebexID = messages[0];
                    messages.RemoveAt(0);

                    MailMessage message = imap.GetMailMessage(rebexID);

                    // See if the subject must match a given pattern
                    if ((!string.IsNullOrWhiteSpace(store.NamePatternMatch) && !Regex.Match(message.Subject, store.NamePatternMatch).Success) ||
                        (!string.IsNullOrWhiteSpace(store.NamePatternSkip) && Regex.Match(message.Subject, store.NamePatternSkip).Success))
                    {
                        UpdateStoreLastMessageID(rebexID);
                    }
                    else
                    {
                        return new GenericFileEmailInstance(rebexID, message);
                    }
                }

                // Looped through them all - none of the subjects must have matched the rules
                return null;
            }
            catch (ImapException ex)
            {
                throw new GenericFileLoadException("There was an error reading email from the IMAP server:\n\n" + ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new GenericFileLoadException("There was an filtering incoming email by Subject:\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Handle the succesful import of data from an email message
        /// </summary>
        public override void HandleSuccess(GenericFileInstance file)
        {
            GenericFileEmailInstance email = (GenericFileEmailInstance) file;
            GenericFileSuccessAction successAction = (GenericFileSuccessAction) store.SuccessAction;

            try
            {
                switch (successAction)
                {
                    // Move the message and mark it as unread
                    case GenericFileSuccessAction.Move:
                        {
                            // For success we mark it as read - i think that makes sense, since they are done with it
                            imap.SetMessageFlags(email.RebexID, ImapFlagAction.Add, ImapMessageFlags.Seen);
                            imap.CopyMessage(email.RebexID, store.SuccessMoveFolder);
                            imap.DeleteMessage(email.RebexID);
                            imap.Purge();

                            break;
                        }

                    // Delete the message completely
                    case GenericFileSuccessAction.Delete:
                        {
                            imap.DeleteMessage(email.RebexID);
                            imap.Purge();

                            break;
                        }

                    // Mark as read
                    case GenericFileSuccessAction.MarkAsRead:
                        {
                            imap.SetMessageFlags(email.RebexID, ImapFlagAction.Add, ImapMessageFlags.Seen);

                            break;
                        }
                    
                    default:
                        throw new InvalidOperationException("Unhandled success action: " + successAction);
                }

                // Update the store so we start after this message next time
                UpdateStoreLastMessageID(email.RebexID);
            }
            catch (ImapException ex)
            {
                throw new GenericFileStoreException(string.Format("Failed to perform success action on message '{0}':\n\n{1}",
                    email.Name,
                    ex.Message),
                    ex);
            }
        }

        /// <summary>
        /// Handle the successful import of data from 
        /// </summary>
        public override void HandleError(GenericFileInstance file, GenericFileStoreException ex)
        {
            GenericFileEmailInstance email = (GenericFileEmailInstance) file;
            GenericFileErrorAction errorAction = (GenericFileErrorAction) store.ErrorAction;

            try
            {
                switch (errorAction)
                {
                    // Stop and display the error
                    case GenericFileErrorAction.Stop:
                        {
                            throw new GenericFileStoreException(string.Format("There was an error reading email '{0}':\n\n{1}", email.Name, ex.Message), ex);
                        }

                    // Move and mark unread
                    case GenericFileErrorAction.Move:
                        {
                            // For error's we make sure its unread, so it shows up
                            imap.SetMessageFlags(email.RebexID, ImapFlagAction.Remove, ImapMessageFlags.Seen);
                            imap.CopyMessage(email.RebexID, store.ErrorMoveFolder);
                            imap.DeleteMessage(email.RebexID);
                            imap.Purge();

                            break;
                        }

                    // Just mark unread
                    case GenericFileErrorAction.MarkAsRead:
                        {
                            imap.SetMessageFlags(email.RebexID, ImapFlagAction.Add, ImapMessageFlags.Seen);

                            break;
                        }

                    default:
                        throw new InvalidOperationException("Unhandled error action: " + errorAction);
                }

                // Update the store so we start after this message next time
                UpdateStoreLastMessageID(email.RebexID);
            }
            catch (ImapException imapEx)
            {
                throw new GenericFileStoreException(string.Format("Failed to perform error action on message '{0}':\n\n{1}",
                    email.Name,
                    imapEx.Message),
                    imapEx);
            }
        }

        /// <summary>
        /// Update the last message ID we have recorded for the store
        /// </summary>
        private void UpdateStoreLastMessageID(string rebexID)
        {
            store.EmailFolderLastMessageID = GetUnqiueID(rebexID);
            SqlAdapter.Default.SaveAndRefetch(store);
        }

        /// <summary>
        /// Get the actual integral uniqueID given the base64 rebexID
        /// </summary>
        private static long GetUnqiueID(string rebexID)
        {
            long unused;
            long uniqueID;
            ImapMessageSet.ParseUniqueId(rebexID, out unused, out uniqueID);

            return uniqueID;
        }
    }
}
