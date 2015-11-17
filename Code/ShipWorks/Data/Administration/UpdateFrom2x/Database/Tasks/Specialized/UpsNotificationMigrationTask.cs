using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using System.Transactions;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using System.Data;
using Interapptive.Shared;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized
{
    /// <summary>
    /// Converts the Serialized Dataset column for Ups shipment notification configuration.
    /// </summary>
    class UpsNotificationUpgradeMigrationTask : MigrationTaskBase
    {
        enum NotificationTarget
        {
            None = 0,
            Recipient = 1,
            Other = 2,
            Sender = 3
        }

        class WorkItem
        {
            public long ShipmentID { get; set; }
            public string SerializedDataset { get; set; }

            public int NotifySender { get; set; }
            public int NotifyRecipient { get; set; }
            public int NotifyOther { get; set; }
            public string OtherAddress { get; set; }
        }

        // work chunk
        const int batchSize = 1000;

        /// <summary>
        /// Type code for instantiation
        /// </summary>
        public override MigrationTaskTypeCode TaskTypeCode
        {
            get { return MigrationTaskTypeCode.UpsNotificationUpgradeTask; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UpsNotificationUpgradeMigrationTask()
            : base(WellKnownMigrationTaskIds.UpsNotificationUpgrade, MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.Repeated)
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public UpsNotificationUpgradeMigrationTask(UpsNotificationUpgradeMigrationTask toCopy) 
            : base (toCopy)
        {

        }

        /// <summary>
        /// Cloning
        /// </summary>
        public override MigrationTaskBase Clone()
        {
            return new UpsNotificationUpgradeMigrationTask(this);
        }

        /// <summary>
        /// Execute the conversion
        /// </summary>
        [NDependIgnoreLongMethod]
        protected override int Run()
        {
            Progress.Detail = "Updating UPS Shipments...";

            // the ids to be updated
            List<WorkItem> toUpgrade  = new List<WorkItem>();

            // start a transaction 
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, SqlCommandProvider.DefaultTimeout))
            {
                // new connection
                using (SqlConnection con = MigrationTaskBase.OpenConnectionForTask(this))
                {
                    // find rows yet to be updated
                    string query = String.Format("SELECT TOP {0} ShipmentID, NotificationEmailRecipients FROM dbo.v2m_UpsShipmentNotify", batchSize);
                    using (SqlCommand cmd = SqlCommandProvider.Create(con, query))
                    {
                        using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                toUpgrade.Add(new WorkItem
                                {
                                    ShipmentID = (long)reader["ShipmentID"],
                                    SerializedDataset = (string)reader["NotificationEmailRecipients"]
                                });
                            }
                        }

                        // convert
                        toUpgrade.ForEach(i => { ConvertXml(i); ReportWorkProgress(); });

                        // save
                        using (SqlCommand deleteCmd = SqlCommandProvider.Create(con))
                        {
                            cmd.CommandText = "UPDATE dbo.UpsShipment SET EmailNotifySender = @NotifySender, EmailNotifyRecipient = @NotifyRecipient, " +
                                                " EmailNotifyOther = @NotifyOther, EmailNotifyOtherAddress = @NotifyOtherAddress" +
                                                " WHERE ShipmentID = @ShipmentID";

                            cmd.Parameters.Add("@NotifySender", SqlDbType.Int);
                            cmd.Parameters.Add("@NotifyRecipient", SqlDbType.Int);
                            cmd.Parameters.Add("@NotifyOther", SqlDbType.Int);
                            cmd.Parameters.Add("@NotifyOtherAddress", SqlDbType.NVarChar, 50);
                            cmd.Parameters.Add("@ShipmentID", SqlDbType.BigInt);

                            deleteCmd.CommandText = "DELETE FROM dbo.v2m_UpsShipmentNotify WHERE ShipmentID = @ShipmentID";
                            deleteCmd.Parameters.Add("@ShipmentID", SqlDbType.BigInt);

                            toUpgrade.ForEach(i =>
                            {
                                // perform the update
                                cmd.Parameters["@NotifySender"].Value = i.NotifySender;
                                cmd.Parameters["@NotifyRecipient"].Value = i.NotifyRecipient;
                                cmd.Parameters["@NotifyOther"].Value = i.NotifyOther;
                                cmd.Parameters["@NotifyOtherAddress"].Value = i.OtherAddress == null ? "" : i.OtherAddress;
                                cmd.Parameters["@ShipmentID"].Value = i.ShipmentID;
                                SqlCommandProvider.ExecuteNonQuery(cmd);

                                // perform the delete
                                deleteCmd.Parameters["@ShipmentID"].Value = i.ShipmentID;
                                SqlCommandProvider.ExecuteNonQuery(deleteCmd);

                                // increment progress
                                ReportWorkProgress();
                            });
                        }
                    }
                }

                // commit the transaction
                scope.Complete();
            }

            // return the number of rows processed
            return toUpgrade.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void ConvertXml(WorkItem item)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(item.SerializedDataset);
                XPathNavigator xpath = doc.CreateNavigator();

                NotificationTarget target = NotificationTarget.None;
                foreach (XPathNavigator recipient in xpath.Select("//UpsEmailRecipients"))
                {
                    string emailAddress = XPathUtility.Evaluate(recipient, "EmailAddress", "");
                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        target++;
                        bool shipNotify = false;
                        bool exceptionNotify = false;
                        bool deliveryNotify = false;

                        // I've come across both formats: true/false and 0/1.  Parse all *Notify bools.
                        try
                        {
                            XPathUtility.Evaluate(recipient, "ShipNotify", false);
                        }
                        catch (FormatException)
                        {
                            shipNotify = String.Compare(XPathUtility.Evaluate(recipient, "ShipNotify", ""), "1") == 0;
                        }

                        try
                        {
                            exceptionNotify = XPathUtility.Evaluate(recipient, "ExceptionNotify", false);
                        }
                        catch (FormatException)
                        {
                            exceptionNotify = String.Compare(XPathUtility.Evaluate(recipient, "ExceptionNotify", ""), "1") == 0;
                        }

                        try
                        {
                            deliveryNotify = XPathUtility.Evaluate(recipient, "DeliveryNotify", false);
                        }
                        catch (FormatException)
                        {
                            deliveryNotify = String.Compare(XPathUtility.Evaluate(recipient, "DeliveryNotify", ""), "1") == 0;
                        }

                        // convert the data
                        int value = Convert.ToInt32(shipNotify) * 1 + 
                                    Convert.ToInt32(exceptionNotify) * 2 +
                                    Convert.ToInt32(deliveryNotify) * 4;

                        // decide which field this is for
                        switch (target)
                        {
                            case NotificationTarget.Other:
                                item.NotifyOther = value;
                                item.OtherAddress = emailAddress;
                                break;

                            case NotificationTarget.Recipient:
                                item.NotifyRecipient = value;
                                break;

                            case NotificationTarget.Sender:
                                item.NotifySender = value;
                                break;

                            default:
                                // nothing
                                break;
                        }
                    }
                }

            }
            catch (XmlException ex)
            {
                throw new MigrationException("Failure loading UPS Notification Xml", ex);
            }
        }

        /// <summary>
        /// Determine the estimated number of rows impacted
        /// </summary>
        protected override int RunEstimate(System.Data.SqlClient.SqlConnection con)
        {
            // just count each store as a work unit
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                try
                {
                    try
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM dbo.UpsShipments";

                        // multiply becuase converting is a step and saving is a step
                        return (int)SqlCommandProvider.ExecuteScalar(cmd) * 2;
                    }
                    catch (SqlException)
                    {
                        // must be mid-conversion, so get the number of ebay items remaining to be updated
                        cmd.CommandText = "SELECT COUNT(*) FROM dbo.v2m_UpsShipments";

                        // multiply becuase converting is a step and saving is a step
                        return (int)SqlCommandProvider.ExecuteScalar(cmd) * 2;
                    }
                }
                catch (SqlException)
                {
                    // just throw a number out and continue
                    return 1;
                }
            }
        }
    }
}