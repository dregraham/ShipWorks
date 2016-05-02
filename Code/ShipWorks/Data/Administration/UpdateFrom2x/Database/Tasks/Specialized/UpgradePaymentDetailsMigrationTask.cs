using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using System.Data;
using System.Transactions;
using Interapptive.Shared;
using Interapptive.Shared.Security;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized
{
    /// <summary>
    /// Move PaymentDetails from V2 to OrderPaymentDetail in V3, updating the encrypted
    /// fields where necessary.
    /// </summary>
    public class UpgradePaymentDetailsMigrationTask : MigrationTaskBase
    {
        /// <summary>
        /// Single PaymentDetail row data
        /// </summary>
        class PaymentDetail
        {
            public int OldID { get; set; }
            public long NewID { get; set; }
            public long NewOrderID { get; set; }
            public string Type { get; set; }
            public string Label { get; set; }
            public string Value { get; set; }
        }

        /// <summary>
        /// Identifying task code
        /// </summary>
        public override MigrationTaskTypeCode TaskTypeCode
        {
            get { return MigrationTaskTypeCode.UpgradePaymentDetails; }
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public UpgradePaymentDetailsMigrationTask(UpgradePaymentDetailsMigrationTask toCopy)
            : base (toCopy)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UpgradePaymentDetailsMigrationTask()
            : base(WellKnownMigrationTaskIds.UpgradePaymentDetails, MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.Repeated)
        {

        }

        /// <summary>
        /// Create a copy
        /// </summary>
        public override MigrationTaskBase Clone()
        {
            return new UpgradePaymentDetailsMigrationTask(this);
        }

        /// <summary>
        /// Execute
        /// </summary>
        protected override int Run()
        {
            Progress.Detail = "Upgrading payment details...";

            int count = 0;

            // get a connection to the database
            using (SqlConnection con = MigrationTaskBase.OpenConnectionForTask(this))
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
                {
                    cmd.CommandText = "SELECT TOP 200 PaymentDetailID, OrderID, dbo.v2m_TranslateKey(OrderID, 0) as NewOrderID, [Type], Label, Value FROM dbo.v2m_PaymentDetails";

                    List<PaymentDetail> details = new List<PaymentDetail>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count++;

                            PaymentDetail detail = new PaymentDetail()
                            {
                                OldID = (int)reader["PaymentDetailID"],
                                NewOrderID = (long)reader["NewOrderID"],
                                Type = (string)reader["Type"],
                                Label = (string)reader["Label"],
                                Value = (string)reader["Value"]
                            };

                            details.Add(detail);
                        }
                    }

                    // cycle through, updating the encryption
                    UpdateEncryption(details);

                    SaveToDatabase(con, details);
                }
            }

            // repeat until no rows exist to process in this database
            return count;
        }

        /// <summary>
        /// Save all PaymentDetails to the database, in the ShipWorks 3 table
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void SaveToDatabase(SqlConnection con, List<PaymentDetail> details)
        {
            string masterDatabaseName = (string)MigrationContext.Current.PropertyBag["MasterDatabase"];

            // Create a transaction for the insert, bookkeeping, and deletes
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, SqlCommandProvider.DefaultTimeout))
            {
                using (SqlCommand insertCommand = SqlCommandProvider.Create(con))
                {
                    insertCommand.CommandText = String.Format("INSERT INTO {0}.dbo.OrderPaymentDetail ([OrderID], [Label], [Value]) " +
                                                              " VALUES " +
                                                              " (@orderID, @label, @value); SET @newId = @@IDENTITY", masterDatabaseName);

                    // configure the parameters
                    insertCommand.Parameters.Add("@orderID", SqlDbType.BigInt);
                    insertCommand.Parameters.Add("@label", SqlDbType.NVarChar);
                    insertCommand.Parameters.Add("@value", SqlDbType.NVarChar);

                    // parameter for capturing the new Id
                    SqlParameter idParam = insertCommand.Parameters.Add("@newId", SqlDbType.BigInt);
                    idParam.Direction = ParameterDirection.Output;

                    // Create and configure the command for recording the new key
                    using (SqlCommand recordKeyCommand = SqlCommandProvider.Create(con))
                    {
                        recordKeyCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        recordKeyCommand.CommandText = "v2m_RecordKey";

                        // @oldKey int, @oldKeyType int, @newKey bigint
                        recordKeyCommand.Parameters.Add("@oldKey", SqlDbType.Int);
                        recordKeyCommand.Parameters.Add("@oldKeyType", SqlDbType.Int);
                        recordKeyCommand.Parameters.Add("@newKey", SqlDbType.BigInt);

                        // Create and configure the command for deleting the old row
                        using (SqlCommand deleteCommand = SqlCommandProvider.Create(con))
                        {
                            deleteCommand.CommandText = "DELETE FROM dbo.v2m_PaymentDetails WHERE PaymentDetailID = @paymentDetailID";
                            deleteCommand.Parameters.Add("@paymentDetailID", SqlDbType.Int);

                            // cycle through the provided details, performing inserts and recording the keys
                            foreach (PaymentDetail detail in details)
                            {
                                insertCommand.Parameters["@orderID"].Value = detail.NewOrderID;
                                insertCommand.Parameters["@label"].Value = detail.Label;
                                insertCommand.Parameters["@value"].Value = detail.Value;

                                insertCommand.ExecuteNonQuery();

                                // grab the new PaymentDetailID
                                detail.NewID = (long)idParam.Value;

                                // record the new key
                                recordKeyCommand.Parameters["@oldKey"].Value = detail.OldID;
                                recordKeyCommand.Parameters["@oldKeyType"].Value = 11;
                                recordKeyCommand.Parameters["@newKey"].Value = detail.NewID;
                                recordKeyCommand.ExecuteNonQuery();

                                // delete the old row
                                deleteCommand.Parameters["@paymentDetailID"].Value = detail.OldID;
                                deleteCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                // commit the transaction
                scope.Complete();
            }
        }

        /// <summary>
        /// Encrypt values to be compatible with ShipWorks 3
        /// </summary>
        private static void UpdateEncryption(List<PaymentDetail> details)
        {
            foreach (PaymentDetail detail in details)
            {
                // Re-encrypt values for V3
                if (String.Compare(detail.Type, "cc_number", StringComparison.OrdinalIgnoreCase) == 0 ||
                    String.Compare(detail.Type, "card_num", StringComparison.OrdinalIgnoreCase) == 0 ||
                    String.Compare(detail.Type, "cardnum", StringComparison.OrdinalIgnoreCase) == 0 ||
                    String.Compare(detail.Type, "Card Number", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // decrypt using the v2 method
                    string decryptedValue = SecureText.Decrypt(detail.Value, detail.Type);

                    // re-encrypt with how v3
                    detail.Value = SecureText.Encrypt(decryptedValue, detail.Label);
                }
                else if (detail.Label.StartsWith("CCV", StringComparison.OrdinalIgnoreCase) ||
                         detail.Type.StartsWith("CCV", StringComparison.OrdinalIgnoreCase))
                {
                    // encrypt new values for V3
                    detail.Value = SecureText.Encrypt(detail.Value, detail.Label);
                }
            }
        }

        /// <summary>
        /// Get the estimated number of rows
        /// </summary>
        protected override int RunEstimate(System.Data.SqlClient.SqlConnection con)
        {
            // just count each store as a work unit
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                try
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM PaymentDetails";

                    return (int)SqlCommandProvider.ExecuteScalar(cmd);
                }
                catch (SqlException)
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM v2m_PaymentDetails";

                    return (int)SqlCommandProvider.ExecuteScalar(cmd);
                }
            }
        }
    }
}
