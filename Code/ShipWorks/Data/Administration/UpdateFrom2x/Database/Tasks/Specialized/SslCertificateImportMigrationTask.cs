using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using Interapptive.Shared.Net;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Diagnostics;
using Interapptive.Shared;
using ShipWorks.Stores.Platforms.PayPal;
using log4net;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized
{
    /// <summary>
    /// Attempts to import certificates used for store communication which are kept on the local computer in Version 2.
    /// Version 3 stores these in the ShipWorks database.
    /// </summary>
    class SslCertificateImportMigrationTask : MigrationTaskBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SslCertificateImportMigrationTask));
				
        // Converstion result container
        class CertificateBlob
        {
            public long CertificateID { get; set; }
            public long StoreID { get; set; }
            public byte[] CertificateData { get; set; }
        }

        /// <summary>
        /// The task type code
        /// </summary>
        public override MigrationTaskTypeCode TaskTypeCode
        {
            get { return MigrationTaskTypeCode.SslCertificateImportTask; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SslCertificateImportMigrationTask()
            : base(WellKnownMigrationTaskIds.SslCertificateImport, MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.RunOnce)
        {

        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public SslCertificateImportMigrationTask(SslCertificateImportMigrationTask toCopy) 
            : base (toCopy)
        {

        }

        /// <summary>
        /// Creates a clone of the task
        /// </summary>
        public override MigrationTaskBase Clone()
        {
            return new SslCertificateImportMigrationTask(this);
        }

        /// <summary>
        /// Run the certificate import.
        /// </summary>
        protected override int Run()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, SqlCommandProvider.DefaultTimeout))
            {
                using (SqlConnection con = MigrationTaskBase.OpenConnectionForTask(this))
                {
                    UpdatePayPalStores(con);
                    UpdateEbayStores(con);
                    UpdateAmazonStores(con);

                    // done with transaction
                    scope.Complete();
                }
            }

            return base.ActualWorkCount;
        }

        /// <summary>
        /// Run the estimate step 
        /// </summary>
        protected override int RunEstimate(System.Data.SqlClient.SqlConnection con)
        {
            // just count each store as a work unit
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                try
                {
                    // ebay, amazon, paypal
                    cmd.CommandText = "SELECT COUNT(*) FROM Stores WHERE StoreType IN (1, 10, 18)";

                    return (int)SqlCommandProvider.ExecuteScalar(cmd);
                }
                catch (SqlException)
                {
                    // ebay, amazon, paypal
                    cmd.CommandText = "SELECT COUNT(*) FROM Store WHERE TypeCode IN (1, 10, 18)";

                    return (int)SqlCommandProvider.ExecuteScalar(cmd);
                }
            }
        }

        /// <summary>
        /// Converts all of the certificate fields in the amazon store to V3-compatible
        /// </summary>
        [NDependIgnoreLongMethod]
        private void UpdateAmazonStores(SqlConnection con)
        {
            Progress.Detail = "Upgrading Amazon certificates...";

            List<CertificateBlob> convertedCertificates = new List<CertificateBlob>();
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                cmd.CommandText = "SELECT CertificateID, StoreID, CertificateName, PublicKey, PrivateKey FROM dbo.v2m_AmazonCertificateTemp";

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        try
                        {
                            ClientCertificate certificate = new ClientCertificate();

                            string publicKey = (string)reader["PublicKey"];
                            string privateKey = (string)reader["PrivateKey"];

                            certificate.LoadFromPem(publicKey, privateKey);

                            convertedCertificates.Add(new CertificateBlob
                            {
                                CertificateData = certificate.Export(),
                                StoreID = (long)reader["StoreID"],
                                CertificateID = (long)reader["CertificateID"]
                            });
                        }
                        catch (CryptographicException)
                        {                            
                            convertedCertificates.Add(new CertificateBlob { StoreID = (long)reader["StoreID"] });
                        }
                        catch (InvalidOperationException)
                        {
                            // certificate doesn't exist
                            convertedCertificates.Add(new CertificateBlob { StoreID = (long)reader["StoreID"] });
                        }
                        
                        // record progress
                        ReportWorkProgress();
                    }
                }

                // update the AmazonStore records
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE dbo.AmazonStore SET Certificate = @certificateData WHERE StoreID = @storeID";
                cmd.Parameters.Add("@certificateData", SqlDbType.Binary, 2048);
                cmd.Parameters.Add("@storeID", SqlDbType.BigInt);

                convertedCertificates.ForEach(c =>
                {
                    Debug.Assert(c.CertificateData != null && c.CertificateData.Length <= 2048, "Certificate data too large.");

                    if (c.CertificateData != null && c.CertificateData.Length <= 2048)
                    {
                        cmd.Parameters["@certificateData"].Value = c.CertificateData;
                        cmd.Parameters["@storeID"].Value = c.StoreID;

                        SqlCommandProvider.ExecuteNonQuery(cmd);
                    }
                    else
                    {
                        log.InfoFormat("Certificate load for store ID {0} failed, not carrying over to ShipWorks 3", c.StoreID); 
                    }
                });

                // delete the temporary rows
                cmd.Parameters.Clear();
                cmd.CommandText = "DELETE FROM dbo.v2m_AmazonCertificateTemp WHERE CertificateID = @certificateID";
                cmd.Parameters.Add("@certificateID", SqlDbType.BigInt);

                convertedCertificates.ForEach(c =>
                {
                    cmd.Parameters["@certificateID"].Value = c.CertificateID;

                    SqlCommandProvider.ExecuteNonQuery(cmd);
                });
            }
        }

        /// <summary>
        /// Converts all of the certificate fields in the ebay store to V3-compatible
        /// </summary>
        private void UpdateEbayStores(SqlConnection con)
        {
            Progress.Detail = "Upgrading PayPal certificates...";

            // In v2 the PayPalApiPassword was encrypted.  Not so in v3
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                cmd.CommandText = "SELECT StoreID, PayPalApiUserName, PayPalApiPassword FROM dbo.EbayStore";

                using (DataTable table = new DataTable())
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        using (SqlCommand passwordCmd = SqlCommandProvider.Create(con))
                        {
                            passwordCmd.CommandText = "UPDATE dbo.EbayStore SET PayPalApiPassword = @password WHERE StoreID = @storeID";
                            passwordCmd.Parameters.AddWithValue("@password", SecureText.Decrypt((string)row["PayPalApiPassword"], (string)row["PayPalApiUsername"]));
                            passwordCmd.Parameters.AddWithValue("@storeID", (long)row["StoreID"]);

                            passwordCmd.ExecuteNonQuery();

                            ReportWorkProgress();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Converts all of the certificate fields in the paypal store to V3-compatible
        /// </summary>
        private void UpdatePayPalStores(SqlConnection con)
        {
            Progress.Detail = "Upgrading PayPal certificates...";

            // In v2 the ApiPassword was encrypted.  Not so in v3
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                cmd.CommandText = "SELECT StoreID, ApiUserName, ApiPassword FROM dbo.PayPalStore";

                using (DataTable table = new DataTable())
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        using (SqlCommand passwordCmd = SqlCommandProvider.Create(con))
                        {
                            passwordCmd.CommandText = "UPDATE dbo.PayPalStore SET ApiPassword = @password WHERE StoreID = @storeID";
                            passwordCmd.Parameters.AddWithValue("@password", SecureText.Decrypt((string)row["ApiPassword"], (string)row["ApiUsername"]));
                            passwordCmd.Parameters.AddWithValue("@storeID", (long)row["StoreID"]);

                            passwordCmd.ExecuteNonQuery();
                        }

                        ReportWorkProgress();
                    }
                }
            }
        }
    }
}