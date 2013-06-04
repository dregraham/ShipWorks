using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Drawing;
using ShipWorks.Properties;
using System.Net;
using log4net;
using System.IO;
using ShipWorks.Users;
using ShipWorks.Data.Connection;
using System.Xml.Linq;
using System.Xml;
using System.Diagnostics;
using Interapptive.Shared.Net;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Represents a messages recieved from the server
    /// </summary>
    class DashboardServerMessageItem : DashboardMessageItem
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DashboardServerMessageItem));

        ServerMessageEntity serverMessage;

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardServerMessageItem(ServerMessageEntity serverMessage)
            : base(serverMessage.Published)
        {
            this.serverMessage = serverMessage;
        }

        /// <summary>
        /// The server message this item represents
        /// </summary>
        public ServerMessageEntity ServerMessage
        {
            get { return serverMessage; }
        }

        /// <summary>
        /// Initialize the dashboard item
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            DashboardBar.CanUserDismiss = serverMessage.Dismissable;

            DashboardBar.Image = DetermineImage();
            DashboardBar.PrimaryText = serverMessage.PrimaryText;
            DashboardBar.SecondaryText = FormatSecondaryText(serverMessage.SecondaryText);

            LoadActions();
        }

        /// <summary>
        /// Load any actions specified by the server message
        /// </summary>
        private void LoadActions()
        {
            if (!string.IsNullOrEmpty(serverMessage.Actions))
            {
                try
                {
                    List<DashboardAction> actions = new List<DashboardAction>();

                    XElement xElement = XElement.Parse(serverMessage.Actions);

                    foreach (XElement xAction in xElement.Descendants("Action"))
                    {
                        string text = (string) xAction.Element("Text");
                        string target = (string) xAction.Element("Target");

                        if (!string.IsNullOrEmpty(text))
                        {
                            Uri uri = null;

                            if (!string.IsNullOrEmpty(target))
                            {
                                if (!Uri.TryCreate(target, UriKind.Absolute, out uri))
                                {
                                    Debug.Fail("Found invalid action target in message");
                                    log.ErrorFormat("Found invalid action target in message {0}", serverMessage.Number);
                                }
                            }

                            actions.Add(new DashboardActionUrl(text, uri));
                        }
                        else
                        {
                            Debug.Fail("Found empty action text in message");
                            log.ErrorFormat("Found empty action text in message {0}", serverMessage.Number);
                        }
                    }

                    DashboardBar.ApplyActions(actions);
                }
                catch (XmlException ex)
                {
                    Debug.Fail(ex.Message, ex.ToString());
                    log.Error(string.Format("Failed to load actions for message {0}", serverMessage.Number), ex);
                }
            }
        }

        /// <summary>
        /// Determine the image to use based on the server message
        /// </summary>
        private Image DetermineImage()
        {
            // If its an integer, then map it to our enum
            int enumValue;
            if (int.TryParse(serverMessage.Image, out enumValue))
            {
                return GetImage((DashboardMessageImageType) enumValue);
            }

            // At this point we try to assume its a URL
            try
            {
                WebRequest request = WebRequest.Create(serverMessage.Image);

                // It's just an image - don't hang on it
                request.Timeout = (int) TimeSpan.FromSeconds(5).TotalMilliseconds;

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        return Image.FromStream(stream);
                    }
                }
            }
            catch (UriFormatException ex)
            {
                log.Error(string.Format("Failed to load message image for message {0}. ({1})", 
                    serverMessage.Number, serverMessage.Image), 
                    ex);
            }
            catch (ArgumentException ex)
            {
                log.Error(string.Format("Failed to load message image for message {0}. ({1})",
                    serverMessage.Number, serverMessage.Image),
                    ex);
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    log.Error(string.Format("Failed to load message image for message {0}. ({1})",
                        serverMessage.Number, serverMessage.Image),
                        ex);
                }
                else
                {
                    throw;
                }
            }

            // If all else failed, fall back to information
            return Resources.exclamation16;
        }

        /// <summary>
        /// Item is being dismissed.  This needs to be persisted to the database.
        /// </summary>
        public override void Dismiss()
        {
            base.Dismiss();

            ServerMessageSignoffEntity signoff = new ServerMessageSignoffEntity();
            signoff.ServerMessageID = serverMessage.ServerMessageID;
            signoff.UserID = UserSession.User.UserID;
            signoff.ComputerID = UserSession.Computer.ComputerID;
            signoff.Dismissed = DateTime.UtcNow;

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveEntity(signoff);
            }
        }
    }
}
