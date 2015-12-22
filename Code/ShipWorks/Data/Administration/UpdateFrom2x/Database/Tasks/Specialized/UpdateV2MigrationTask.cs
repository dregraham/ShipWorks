using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using ShipWorks.ApplicationCore;
using System.IO;
using System.Reflection;
using Interapptive.Shared.Utility;
using Interapptive.Shared.IO.Zip;
using System.Data;
using System.Transactions;
using Interapptive.Shared;
using log4net;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized
{
    /// <summary>
    /// Performs necessary schema updates to get a V2 database up to the latest version.
    /// 
    /// Instancing: OncePerDatabase
    /// RunPattern: RunOnce
    /// </summary>
    public class UpdateV2MigrationTask : MigrationTaskBase
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(UpdateV2MigrationTask));
						  
        /// <summary>
        /// A script container for the scripts required to move to Version from the previous.
        /// </summary>
        class V2UpdateBatch
        {
            public Version Version { get; private set; }
            public List<string> Scripts { get; private set; }
            public bool SetQuotedIdentifiers { get; set; }

            /// <summary>
            /// Create a V2 database update script batch
            /// </summary>
            public V2UpdateBatch(Version version, params string[] scriptResources)
            {
                SetQuotedIdentifiers = true;
                Version = version;
                Scripts = new List<string>(scriptResources);
            }
        }

        /// <summary>
        /// Get the type code for this task
        /// </summary>
        public override MigrationTaskTypeCode TaskTypeCode
        {
            get { return MigrationTaskTypeCode.UpgradeV2MigrationTask; }
        }

        #region instantiation

        // script batches to perform up updates
        List<V2UpdateBatch> updateBatches = new List<V2UpdateBatch>();

        // temporary location for where the v2 scripts will be unpacked to
        string baseScriptPath = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdateV2MigrationTask() 
            : base(WellKnownMigrationTaskIds.UpdateV2Schema, MigrationTaskInstancing.MainDatabaseAndArchives, MigrationTaskRunPattern.RunOnce)
        {
            PrepareUpdateBatches();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        protected UpdateV2MigrationTask(UpdateV2MigrationTask copy) 
            : base(copy)
        {
            PrepareUpdateBatches();
        }

        /// <summary>
        /// Clone
        /// </summary>
        public override MigrationTaskBase Clone()
        {
            return new UpdateV2MigrationTask(this);
        }

        #endregion


        /// <summary>
        /// Unpack the scripts 
        /// </summary>
        public override void Initialize()
        {
            baseScriptPath = ExtractScripts2x();
        }

        /// <summary>
        /// Extract all the script files that we need to run
        /// </summary>
        private static string ExtractScripts2x()
        {
            string basePath = DataPath.CreateUniqueTempPath();

            string zipFile = Path.Combine(basePath, "scripts.zip");

            // First thing we have to do is write out the zip with the scripts
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"ShipWorks.Data.Administration.UpdateFrom2x.Database.Scripts.ShipWorks2x.zip"))
            {
                StreamUtility.WriteToFile(stream, zipFile);
            }

            // Then we need to extract all the files in the zip
            using (ZipReader reader = new ZipReader(zipFile))
            {
                foreach (ZipReaderItem item in reader.ReadItems())
                {
                    item.Extract(Path.Combine(basePath, item.Name));
                }
            }

            return Path.Combine(basePath, "Scripts");
        }

        /// <summary>
        /// Adds a script batch to the collection
        /// </summary>
        private V2UpdateBatch RegisterScripts(string version, params string[] scripts)
        {
            V2UpdateBatch batch = new V2UpdateBatch(new Version(version), scripts);
            updateBatches.Add(batch);

            return batch;
        }

        /// <summary>
        /// All V2 upgrade scripts need to be registered here.  
        /// 
        /// These were upgrade definitions copied from v2 and changed into RegisterScripts() calls.
        /// New v2 update scripts go at the end.
        /// </summary>
        [NDependIgnoreLongMethod]
        private void PrepareUpdateBatches()
        {
            #region V2 Scripts
            // Upgrade from 1.0 to 1.0.1
            RegisterScripts("1.0.0.0",
                @"Upgrade_10_to_101\ClientStoreSettings",
                @"Upgrade_10_to_101\Shipments",
                @"Upgrade_10_to_101\Common").SetQuotedIdentifiers = false;

            // Upgrade from 1.0.1 to 1.0.2
            RegisterScripts("1.0.1.0",
                @"Upgrade_101_to_102\MivaBatches",
                @"Upgrade_101_to_102\Common");

            // Upgrade from 1.0.2 to 1.0.3
            RegisterScripts("1.0.2.0",
                @"Upgrade_102_to_103\Orders",
                @"Upgrade_102_to_103\OrderItems",
                @"Upgrade_102_to_103\OrderItemAttributes",
                @"Upgrade_102_to_103\Shipments",
                @"Upgrade_102_to_103\ClientStoreSettings",
                @"Upgrade_102_to_103\Common").SetQuotedIdentifiers = false;

            // Upgrade from 1.0.3 to 1.0.4
            RegisterScripts("1.0.3.0",
                @"Upgrade_103_to_104\Orders",
                @"Upgrade_103_to_104\OrderItems",
                @"Upgrade_103_to_104\OrderItemAttributes",
                @"Upgrade_103_to_104\Shipments",
                @"Upgrade_103_to_104\Common").SetQuotedIdentifiers = false;

            // Upgrade from 1.0.4 to 1.0.5
            RegisterScripts("1.0.4.0",
                @"Upgrade_104_to_105\MivaSebenzaMsgs",
                @"Upgrade_104_to_105\OrderCharges",
                @"Upgrade_104_to_105\OrderItemAttributes",
                @"Upgrade_104_to_105\PaymentDetails",
                @"Upgrade_104_to_105\UpsPackages",
                @"Upgrade_104_to_105\UpsShipments",
                @"Upgrade_104_to_105\UspsShipments",
                @"Upgrade_104_to_105\Common").SetQuotedIdentifiers = false;

            // Upgrade from 1.0.5 to 1.0.6
            RegisterScripts("1.0.5.0",
                @"Upgrade_105_to_106\ClientStoreSettings",
                @"Upgrade_105_to_106\Orders",
                @"Upgrade_105_to_106\OrderItems",
                @"Upgrade_105_to_106\Shipments",
                @"Upgrade_105_to_106\Stores",
                @"Upgrade_105_to_106\EmailLog",
                @"Upgrade_105_to_106\Filters",
                @"Upgrade_105_to_106\TableChanges",
                @"Upgrade_105_to_106\Common").SetQuotedIdentifiers = false;

            // Upgrade from 1.0.6 to 1.1.0
            RegisterScripts("1.0.6.0",
                @"Upgrade_106_to_110\ClientStoreSettings",
                @"Upgrade_106_to_110\Downloaded",
                @"Upgrade_106_to_110\UpsPackages",
                @"Upgrade_106_to_110\Stores",
                @"Upgrade_106_to_110\Orders",
                @"Upgrade_106_to_110\OrderItems",
                @"Upgrade_106_to_110\Common").SetQuotedIdentifiers = false;

            // From here on out, we leave this on, and do embbedded double apostrophe's the right way
            //qouteCmd.CommandText = "SET QUOTED_IDENTIFIER ON";
            //qouteCmd.ExecuteNonQuery();

            // Upgrade from 1.1.0 to 1.1.1
            RegisterScripts("1.1.0.0",
                @"Upgrade_110_to_111\TableChanges",
                @"Upgrade_110_to_111\Stores",
                @"Upgrade_110_to_111\Customers",
                @"Upgrade_110_to_111\Shipments",
                @"Upgrade_110_to_111\Orders",
                @"Upgrade_110_to_111\OrderItems",
                @"Upgrade_110_to_111\OrderItemAttributes",
                @"Upgrade_110_to_111\PaymentDetails",
                @"Upgrade_110_to_111\Common");

            // Upgrade from 1.1.1 to 1.1.2
            RegisterScripts("1.1.1.0",
                @"Upgrade_111_to_112\UpsShipments",
                @"Upgrade_111_to_112\UspsShipments",
                @"Upgrade_111_to_112\Customers",
                @"Upgrade_111_to_112\Common");

            // Upgrade from 1.1.2 to 1.1.3
            RegisterScripts("1.1.2.0",
                @"Upgrade_112_to_113\ClientStoreSettings",
                @"Upgrade_112_to_113\EmailLog",
                @"Upgrade_112_to_113\Shipments",
                @"Upgrade_112_to_113\UpsShipments",
                @"Upgrade_112_to_113\UspsShipments",
                @"Upgrade_112_to_113\UpsPreferences",
                @"Upgrade_112_to_113\Stores",
                @"Upgrade_112_to_113\Orders",
                @"Upgrade_112_to_113\Customers",
                @"Upgrade_112_to_113\Filters",
                @"Upgrade_112_to_113\Common");

            // Upgrade from 1.1.3 to 1.1.4
            RegisterScripts("1.1.3.0",
                @"Upgrade_113_to_114\EmailAccounts",
                @"Upgrade_113_to_114\Common");

            // Upgrade from 1.1.4 to 1.1.5
            RegisterScripts("1.1.4.0",
                @"Upgrade_114_to_115\UpsPackages",
                @"Upgrade_114_to_115\Shipments",
                @"Upgrade_114_to_115\Common");

            // Upgrade 1.1.5 to 1.1.6 (ShipWorks v2.2.1+)
            RegisterScripts("1.1.5.0",
                @"Upgrade_115_to_116\Stores",
                @"Upgrade_115_to_116\Customers",
                @"Upgrade_115_to_116\Orders",
                @"Upgrade_115_to_116\Actions",
                @"Upgrade_115_to_116\Notifications",
                @"Upgrade_115_to_116\ClientStoreSettings",
                @"Upgrade_115_to_116\FedexShippers",
                @"Upgrade_115_to_116\FedexPreferences",
                @"Upgrade_115_to_116\FedexShipments",
                @"Upgrade_115_to_116\FedexPackages",
                @"Upgrade_115_to_116\Common");

            // Upgrade 1.1.6 to 1.1.6.1 (ShipWorks v2.3.4+)
            RegisterScripts("1.1.6.0",
                @"Upgrade_116_to_1161\ClientStoreSettings",
                @"Upgrade_116_to_1161\OrderItems",
                @"Upgrade_116_to_1161\Common");

            // Upgrade 1.1.6.1 to 1.1.6.2 (ShipWorks v2.3.6+)
            RegisterScripts("1.1.6.1",
                @"Upgrade_1161_to_1162\OrderItems",
                @"Upgrade_1161_to_1162\Common");

            // Upgrade 1.1.6.2 to 1.1.6.3 (ShipWorks v2.3.6+)
            RegisterScripts("1.1.6.2",
                @"Upgrade_1162_to_1163\Stores",
                @"Upgrade_1162_to_1163\Common");

            // Upgrade 1.1.6.3 to 1.1.6.4 (ShipWorks v2.3.6+)
            RegisterScripts("1.1.6.3",
                @"Upgrade_1163_to_1164\Stores",
                @"Upgrade_1163_to_1164\Common");

            // Upgrade 1.1.6.4 to 1.1.6.5 (ShipWorks v2.3.13+)
            RegisterScripts("1.1.6.4",
                @"Upgrade_1164_to_1165\Shipments",
                @"Upgrade_1164_to_1165\ShipmentCommodities",
                @"Upgrade_1164_to_1165\UspsShipments",
                @"Upgrade_1164_to_1165\ClientStoreSettings",
                @"Upgrade_1164_to_1165\Orders",
                @"Upgrade_1164_to_1165\Customers",
                @"Upgrade_1164_to_1165\Common");

            // Upgrade 1.1.6.5 to 1.1.6.6 (ShipWorks v2.3.16+)
            RegisterScripts("1.1.6.5",
                @"Upgrade_1165_to_1166\Orders",
                @"Upgrade_1165_to_1166\Common");

            // Upgrade 1.1.6.6 to 1.1.6.7 (ShipWorks v2.3.18+)
            RegisterScripts("1.1.6.6",
                @"Upgrade_1166_to_1167\Shipments",
                @"Upgrade_1166_to_1167\Common");

            // Upgrade 1.1.6.7 to 1.2.0 (ShipWorks 2.4+)
            RegisterScripts("1.1.6.7",
                @"Upgrade_1167_to_120\Cleanup",
                @"Upgrade_1167_to_120\FedexPreferences",
                @"Upgrade_1167_to_120\FedexShippers",
                @"Upgrade_1167_to_120\FedexShipments",
                @"Upgrade_1167_to_120\FedexPackages",
                @"Upgrade_1167_to_120\FedexClosings",
                @"Upgrade_1167_to_120\Shipments",
                @"Upgrade_1167_to_120\ShipmentCommodities",
                @"Upgrade_1167_to_120\ClientStoreSettings",
                @"Upgrade_1167_to_120\UpsPreferences",
                @"Upgrade_1167_to_120\Customers",
                @"Upgrade_1167_to_120\Orders",
                @"Upgrade_1167_to_120\Stores",
                @"Upgrade_1167_to_120\Users",
                @"Upgrade_1167_to_120\Common");

            // Upgrade 1.2.0 to 1.2.0.1 (ShipWorks 2.4.4+)
            RegisterScripts("1.2.0.0",
                @"Upgrade_120_to_1201\Orders",
                @"Upgrade_120_to_1201\Users",
                @"Upgrade_120_to_1201\Common");

            // Upgrade 1.2.0.1 to 1.2.0.2 (ShipWorks 2.4.5+)
            RegisterScripts("1.2.0.1",
                @"Upgrade_1201_to_1202\Orders",
                @"Upgrade_1201_to_1202\Customers",
                @"Upgrade_1201_to_1202\OrderItems",
                @"Upgrade_1201_to_1202\OrderItemAttributes",
                @"Upgrade_1201_to_1202\Shipments",
                @"Upgrade_1201_to_1202\Common");

            // Upgrade 1.2.0.2 to 1.2.0.3 (ShipWorks 2.4.7+)
            RegisterScripts("1.2.0.2",
                @"Upgrade_1202_to_1203\Orders",
                @"Upgrade_1202_to_1203\Common");

            // Upgrade 1.2.0.3 to 1.2.0.4 (ShipWorks 2.4.9+)
            RegisterScripts("1.2.0.3",
                @"Upgrade_1203_to_1204\FedexPreferences",
                @"Upgrade_1203_to_1204\FedexShipments",
                @"Upgrade_1203_to_1204\FedexShippers",
                @"Upgrade_1203_to_1204\Orders",
                @"Upgrade_1203_to_1204\OrderItems",
                @"Upgrade_1203_to_1204\Stores",
                @"Upgrade_1203_to_1204\ClientStatus",
                @"Upgrade_1203_to_1204\Common");

            // Upgrade 1.2.0.4 to 1.2.0.5 (ShipWorks 2.5.3+)
            RegisterScripts("1.2.0.4",
                @"Upgrade_1204_to_1205\Stores",
                @"Upgrade_1204_to_1205\Customers",
                @"Upgrade_1204_to_1205\Orders",
                @"Upgrade_1204_to_1205\Common");

            // Upgrade 1.2.0.5 to 1.2.0.6 (ShipWorks 2.5.4+)
            RegisterScripts("1.2.0.5",
                @"Upgrade_1205_to_1206\ClientStatus",
                @"Upgrade_1205_to_1206\Shipments",
                @"Upgrade_1205_to_1206\FedexShipments",
                @"Upgrade_1205_to_1206\UpsShipments",
                @"Upgrade_1205_to_1206\FedexPreferences",
                @"Upgrade_1205_to_1206\UpsPreferences",
                @"Upgrade_1205_to_1206\EndiciaPreferences",
                @"Upgrade_1205_to_1206\ClientStoreSettings",
                @"Upgrade_1205_to_1206\Common");

            // Upgrade 1.2.0.6 to 1.3.0.0 (ShipWorks 2.5.5+)
            RegisterScripts("1.2.0.6",
                @"Upgrade_1206_to_1300\Stores",
                @"Upgrade_1206_to_1300\Common");

            // Upgrade 1.3.0.0 to 1.3.0.1 (ShipWorks 2.5.6+)
            RegisterScripts("1.3.0.0",
                @"Upgrade_1300_to_1301\FedexPackages",
                @"Upgrade_1300_to_1301\Orders",
                @"Upgrade_1300_to_1301\Common");

            // Upgrade 1.3.0.1 to 1.3.0.2 (ShipWorks 2.5.7+)
            RegisterScripts("1.3.0.1",
                @"Upgrade_1301_to_1302\Stores",
                @"Upgrade_1301_to_1302\Common");

            // Upgrade 1.3.0.2 to 1.3.0.3 (ShipWorks 2.5.8+)
            RegisterScripts("1.3.0.2",
                @"Upgrade_1302_to_1303\Customers",
                @"Upgrade_1302_to_1303\Orders",
                @"Upgrade_1302_to_1303\Common");

            // Upgrade 1.3.0.3 to 1.3.0.4 (ShipWorks 2.5.15+)
            RegisterScripts("1.3.0.3",
                @"Upgrade_1303_to_1304\Stores",
                @"Upgrade_1303_to_1304\Common");

            // Upgrade 1.3.0.4 to 1.3.0.5 (ShipWorks 2.5.16+)
            RegisterScripts("1.3.0.4",
                @"Upgrade_1304_to_1305\Stores",
                @"Upgrade_1304_to_1305\Common");

            // Upgrade 1.3.0.5 to 1.3.0.6 (ShipWorks 2.5.17+)
            RegisterScripts("1.3.0.5",
                @"Upgrade_1305_to_1306\Stores",
                @"Upgrade_1305_to_1306\Common");

            // Upgrade 1.3.0.6 to 1.3.0.7 (ShipWorks 2.5.18+)
            RegisterScripts("1.3.0.6",
                @"Upgrade_1306_to_1307\Filters",
                @"Upgrade_1306_to_1307\Orders",
                @"Upgrade_1306_to_1307\OrderItems",
                @"Upgrade_1306_to_1307\Stores",
                @"Upgrade_1306_to_1307\Common");

            // Upgrade 1.3.0.7 to 1.3.0.8 (ShipWorks 2.5.19+)
            RegisterScripts("1.3.0.7",
                @"Upgrade_1307_to_1308\Stores",
                @"Upgrade_1307_to_1308\Common");

            // Upgrade 1.3.0.8 to 1.3.0.9 (ShipWorks 2.5.20+)
            RegisterScripts("1.3.0.8",
                @"Upgrade_1308_to_1309\Filters",
                @"Upgrade_1308_to_1309\Stores",
                @"Upgrade_1308_to_1309\Common");

            // Upgrade 1.3.0.9 to 1.3.1.0 (ShipWorks 2.6.1+)
            RegisterScripts("1.3.0.9",
                @"Upgrade_1309_to_1310\ClientStoreSettings",
                @"Upgrade_1309_to_1310\Shipments",
                @"Upgrade_1309_to_1310\Common");

            // Upgrade 1.3.1.0 to 1.4.0.0 (ShipWorks 2.6.2+)
            RegisterScripts("1.3.1.0",
                @"Upgrade_1310_to_1400\Orders",
                @"Upgrade_1310_to_1400\Common");

            // Upgrade 1.4.0.0 to 1.4.0.1 (ShipWorks 2.6.4+)
            RegisterScripts("1.4.0.0",
                @"Upgrade_1400_to_1401\Stores",
                @"Upgrade_1400_to_1401\Clients",
                @"Upgrade_1400_to_1401\Users",
                @"Upgrade_1400_to_1401\ClientStoreSettings",
                @"Upgrade_1400_to_1401\Customers",
                @"Upgrade_1400_to_1401\Orders",
                @"Upgrade_1400_to_1401\MivaBatches",
                @"Upgrade_1400_to_1401\OrderCharges",
                @"Upgrade_1400_to_1401\OrderItems",
                @"Upgrade_1400_to_1401\OrderItemAttributes",
                @"Upgrade_1400_to_1401\DownloadLog",
                @"Upgrade_1400_to_1401\Downloaded",
                @"Upgrade_1400_to_1401\Filters",
                @"Upgrade_1400_to_1401\FeedbackPresets",
                @"Upgrade_1400_to_1401\PaymentDetails",
                @"Upgrade_1400_to_1401\MivaSebenzaMsgs",
                @"Upgrade_1400_to_1401\Shipments",
                @"Upgrade_1400_to_1401\ShipmentCommodities",
                @"Upgrade_1400_to_1401\EndiciaPreferences",
                @"Upgrade_1400_to_1401\UspsShipments",
                @"Upgrade_1400_to_1401\UpsShippers",
                @"Upgrade_1400_to_1401\UpsShipments",
                @"Upgrade_1400_to_1401\UpsPackages",
                @"Upgrade_1400_to_1401\UpsPreferences",
                @"Upgrade_1400_to_1401\Actions",
                @"Upgrade_1400_to_1401\Notifications",
                @"Upgrade_1400_to_1401\FedexShippers",
                @"Upgrade_1400_to_1401\FedexPreferences",
                @"Upgrade_1400_to_1401\FedexShipments",
                @"Upgrade_1400_to_1401\FedexPackages",
                @"Upgrade_1400_to_1401\FedexClosings",
                @"Upgrade_1400_to_1401\CustomLabelSheets",
                @"Upgrade_1400_to_1401\EmailAccounts",
                @"Upgrade_1400_to_1401\EmailLog",
                @"Upgrade_1400_to_1401\TableChanges",
                @"Upgrade_1400_to_1401\ClientStatus",
                @"Upgrade_1400_to_1401\Common");

            // Upgrade 1.4.0.1 to 1.4.0.2 (ShipWorks 2.6.5+)
            RegisterScripts("1.4.0.1",
                @"Upgrade_1401_to_1402\Stores",
                @"Upgrade_1401_to_1402\Common");

            // Upgrade 1.4.0.2 to 2.6.5.0
            RegisterScripts("1.4.0.2",
                @"Upgrade_1402_to_265\OrderItems",
                @"Upgrade_1402_to_265\Customers",
                @"Upgrade_1402_to_265\Orders",
                @"Upgrade_1402_to_265\Common");

            // Upgrade 2.6.5.0 to 2.6.6.0
            RegisterScripts("2.6.5.0",
                @"Upgrade_265_to_266\Stores",
                @"Upgrade_265_to_266\Orders",
                @"Upgrade_265_to_266\Common");

            // Upgrade 2.6.6.0 to 2.6.7.0
            RegisterScripts("2.6.6.0",
                @"Upgrade_266_to_267\FedexPreferences",
                @"Upgrade_266_to_267\EndiciaPreferences",
                @"Upgrade_266_to_267\UspsShipments",
                @"Upgrade_266_to_267\Common");
            

            // Upgrade 2.6.7.0 to 2.6.8.0
            RegisterScripts("2.6.7.0",
                @"Upgrade_267_to_268\FedexPreferences",
                @"Upgrade_267_to_268\UpsShipments",
                @"Upgrade_267_to_268\Common");
            

            // Upgrade 2.6.8.0 to 2.7.0.0
            RegisterScripts("2.6.8.0",
                @"Upgrade_268_to_270\Shipments",
                @"Upgrade_268_to_270\UpsPreferences",
                @"Upgrade_268_to_270\Common");
            

            // Upgrade 2.7.0.0 to 2.8.0.0
            RegisterScripts("2.7.0.0",
                @"Upgrade_270_to_280\AmazonInventory",
                @"Upgrade_270_to_280\Stores",
                @"Upgrade_270_to_280\Orders",
                @"Upgrade_270_to_280\OrderItems",
                @"Upgrade_270_to_280\Common");
            

            // Upgrade 2.8.0.0 to 2.8.1.0
            RegisterScripts("2.8.0.0",
                @"Upgrade_280_to_281\AmazonInventory",
                @"Upgrade_280_to_281\UspsShipments",
                @"Upgrade_280_to_281\Stores",
                @"Upgrade_280_to_281\Common");
            

            // Upgrade 2.8.1.0 to 2.8.4.0
            RegisterScripts("2.8.1.0",
                @"Upgrade_281_to_284\EndiciaPreferences",
                @"Upgrade_281_to_284\UspsShipments",
                @"Upgrade_281_to_284\Common");
            

            // Upgrade 2.8.4.0 to 2.8.5.0
            RegisterScripts("2.8.4.0",
                @"Upgrade_284_to_285\Common",
                @"Upgrade_284_to_285\Stores",
                @"Upgrade_284_to_285\Orders",
                @"Upgrade_284_to_285\UpsShipments");
            

            // Upgrade 2.8.5.0 to 2.8.15.0
            RegisterScripts("2.8.5.0",
                @"Upgrade_285_to_2815\Common",
                @"Upgrade_285_to_2815\UspsShipments");
            

            // Upgrade 2.8.15.0 to 2.8.16.0
            RegisterScripts("2.8.15.0",
                @"Upgrade_2815_to_2816\Common",
                @"Upgrade_2815_to_2816\Stores",
                @"Upgrade_2815_to_2816\Orders");
            

            // Upgrade 2.8.16.0 to 2.8.25.0
            RegisterScripts("2.8.16.0",
                @"Upgrade_2816_to_2825\Common",
                @"Upgrade_2816_to_2825\Stores");
            

            // Upgrade 2.8.25.0 to 2.8.26.0
            RegisterScripts("2.8.25.0",
                @"Upgrade_2825_to_2826\Common",
                @"Upgrade_2825_to_2826\DhlShippers",
                @"Upgrade_2825_to_2826\DhlPreferences",
                @"Upgrade_2825_to_2826\DhlShipments",
                @"Upgrade_2825_to_2826\Orders",
                @"Upgrade_2825_to_2826\OrderItems",
                @"Upgrade_2825_to_2826\ShipmentCommodities",
                @"Upgrade_2825_to_2826\Shipments",
                @"Upgrade_2825_to_2826\FedexPreferences",
                @"Upgrade_2825_to_2826\UpsPreferences",
                @"Upgrade_2825_to_2826\EndiciaPreferences");
            

            // Upgrade 2.8.26.0 to 2.8.28.0
            RegisterScripts("2.8.26.0",
                @"Upgrade_2826_to_2828\Common",
                @"Upgrade_2826_to_2828\DhlShipments",
                @"Upgrade_2826_to_2828\ShipmentCommodities");
            

            // Upgrade 2.8.28.0 to 2.9.0.1
            RegisterScripts("2.8.28.0",
                @"Upgrade_2828_to_2901\Common",
                @"Upgrade_2828_to_2901\DhlShipments");
            

            // Upgrade 2.9.0.1 to 2.9.0.2
            RegisterScripts("2.9.0.1",
                @"Upgrade_2901_to_2902\Common",
                @"Upgrade_2901_to_2902\DhlPreferences",
                @"Upgrade_2901_to_2902\DhlShipments");
            

            // Upgrade 2.9.0.2 to 2.9.5.0
            RegisterScripts("2.9.0.2",
                @"Upgrade_2902_to_295\Common",
                @"Upgrade_2902_to_295\UspsShipments",
                @"Upgrade_2902_to_295\EndiciaPreferences",
                @"Upgrade_2902_to_295\EndiciaShippers");
            

            // Upgrade 2.9.5.0 to 2.9.8.0
            RegisterScripts("2.9.5.0",
                @"Upgrade_295_to_298\Common",
                @"Upgrade_295_to_298\Orders",
                @"Upgrade_295_to_298\YahooInventory",
                @"Upgrade_295_to_298\Stores",
                @"Upgrade_295_to_298\ClientStoreSettings",
                @"Upgrade_295_to_298\UpsPreferences");
            


            // Upgrade 2.9.8.0 to 2.9.9.0
            RegisterScripts("2.9.8.0",
                @"Upgrade_298_to_299\Common",
                @"Upgrade_298_to_299\Stores");
            

            // Upgrade 2.9.9.0 to 2.9.10.0
            RegisterScripts("2.9.9.0",
                @"Upgrade_299_to_2910\Common",
                @"Upgrade_299_to_2910\Filters",
                @"Upgrade_299_to_2910\ArchiveSet",
                @"Upgrade_299_to_2910\ArchiveLog",
                @"Upgrade_299_to_2910\Clients",
                @"Upgrade_299_to_2910\Stores",
                @"Upgrade_299_to_2910\Orders",
                @"Upgrade_299_to_2910\Shipments",
                @"Upgrade_299_to_2910\EmailLog",
                @"Upgrade_299_to_2910\Customers",
                @"Upgrade_299_to_2910\DhlPackages",
                @"Upgrade_299_to_2910\DhlShipments",
                @"Upgrade_299_to_2910\FedexPackages",
                @"Upgrade_299_to_2910\FedexShipments",
                @"Upgrade_299_to_2910\MivaSebenzaMsgs",
                @"Upgrade_299_to_2910\OrderCharges",
                @"Upgrade_299_to_2910\OrderItemAttributes",
                @"Upgrade_299_to_2910\OrderItems",
                @"Upgrade_299_to_2910\PaymentDetails",
                @"Upgrade_299_to_2910\ShipmentCommodities",
                @"Upgrade_299_to_2910\Shipments",
                @"Upgrade_299_to_2910\UpsPackages",
                @"Upgrade_299_to_2910\UpsShipments",
                @"Upgrade_299_to_2910\UspsShipments");
            

            // Upgrade 2.9.10.0 to 2.9.11.0
            RegisterScripts("2.9.10.0",
                @"Upgrade_2910_to_2911\Common",
                @"Upgrade_2910_to_2911\Stores",
                @"Upgrade_2910_to_2911\Orders");
            

            // Upgrade 2.9.11.0 to 2.9.12.0
            RegisterScripts("2.9.11.0",
                @"Upgrade_2911_to_2912\Common",
                @"Upgrade_2911_to_2912\UpsShippers",
                @"Upgrade_2911_to_2912\UpsShipments");
            

            // Upgrade 2.9.12.0 to 2.9.13.0
            RegisterScripts("2.9.12.0",
                @"Upgrade_2912_to_2913\Common",
                @"Upgrade_2912_to_2913\FedexPreferences");
            

            // Upgrade from 2.9.13.0 to 2.9.14.0
            RegisterScripts("2.9.13.0",
                @"Upgrade_2913_to_2914\Common",
                @"Upgrade_2913_to_2914\Stores",
                @"Upgrade_2913_to_2914\Orders",
                @"Upgrade_2913_to_2914\ArchiveSet");
            

            // Upgrade from 2.9.14.0 to 2.9.15.0
            RegisterScripts("2.9.14.0",
                @"Upgrade_2914_to_2915\Common",
                @"Upgrade_2914_to_2915\Orders",
                @"Upgrade_2914_to_2915\Stores");
            

            // Upgrade from 2.9.15.0 to 2.9.16.0
            RegisterScripts("2.9.15.0",
                @"Upgrade_2915_to_2916\Common",
                @"Upgrade_2915_to_2916\Stores",
                @"Upgrade_2915_to_2916\Orders");
            

            // Upgrade from 2.9.16.0 to 2.9.19.0
            RegisterScripts("2.9.16.0",
                @"Upgrade_2916_to_2919\Common",
                @"Upgrade_2916_to_2919\Stores");
            

            // Upgrade from 2.9.19.0 to 2.9.21.0
            RegisterScripts("2.9.19.0",
                @"Upgrade_2919_to_2921\Common",
                @"Upgrade_2919_to_2921\Stores");
            

            // Upgrade from 2.9.21.0 to 2.9.22.0
            RegisterScripts("2.9.21.0",
                @"Upgrade_2921_to_2922\Common",
                @"Upgrade_2921_to_2922\Stores",
                @"Upgrade_2921_to_2922\Orders");
            

            // Upgrade from 2.9.22.0 to 2.9.24.0
            RegisterScripts("2.9.22.0",
                @"Upgrade_2922_to_2924\Common",
                @"Upgrade_2922_to_2924\Orders");
            

            // Upgrade from 2.9.24.0 to 2.9.27.0
            RegisterScripts("2.9.24.0",
                @"Upgrade_2924_to_2927\Common",
                @"Upgrade_2924_to_2927\FedexPreferences",
                @"Upgrade_2924_to_2927\FedexShipments");
            

            // Upgrade from 2.9.27.0 to 2.9.28.0
            RegisterScripts("2.9.27.0",
                @"Upgrade_2927_to_2928\Common",
                @"Upgrade_2927_to_2928\Stores",
                @"Upgrade_2927_to_2928\Customers",
                @"Upgrade_2927_to_2928\Orders");
            

            // Upgrade from 2.9.28.0 to 2.9.30.0
            RegisterScripts("2.9.28.0",
                @"Upgrade_2928_to_2930\Common",
                @"Upgrade_2928_to_2930\Orders");
            

            // Upgrade from 2.9.30.0 to 2.9.33.0
            RegisterScripts("2.9.30.0",
                @"Upgrade_2930_to_2933\Common",
                @"Upgrade_2930_to_2933\Stores",
                @"Upgrade_2930_to_2933\Orders");
            

            // Upgrade from 2.9.33.0 to 2.9.34.0
            RegisterScripts("2.9.33.0",
                @"Upgrade_2933_to_2934\Common",
                @"Upgrade_2933_to_2934\EmailLog",
                @"Upgrade_2933_to_2934\Orders",
                @"Upgrade_2933_to_2934\Stores",
                @"Upgrade_2933_to_2934\OrderItems");
            

            // Upgrade from 2.9.34.0 to 2.9.37.0
            RegisterScripts("2.9.34.0",
                @"Upgrade_2934_to_2937\Common",
                @"Upgrade_2934_to_2937\Stores",
                @"Upgrade_2934_to_2937\Orders",
                @"Upgrade_2934_to_2937\OrderItems");
            

            // Upgrade form 2.9.37.0 to 2.9.39.0
            RegisterScripts("2.9.37.0",
                @"Upgrade_2937_to_2939\Common",
                @"Upgrade_2937_to_2939\Stores",
                @"Upgrade_2937_to_2939\Orders");
            

            // Upgrade from 2.9.39.0 to 2.9.40.0
            RegisterScripts("2.9.39.0",
                @"Upgrade_2939_to_2940\Common",
                @"Upgrade_2939_to_2940\Stores");
            

            // Upgrade from 2.9.40.0 to 2.9.43.0
            RegisterScripts("2.9.40.0",
                @"Upgrade_2940_to_2943\Common",
                @"Upgrade_2940_to_2943\Stores");
            

            // Upgrade from 2.9.43.0 to 2.9.44.0
            RegisterScripts("2.9.43.0",
                @"Upgrade_2943_to_2944\Common",
                @"Upgrade_2943_to_2944\Orders");


            // Upgrade from 2.9.44.0 to 2.9.52
            RegisterScripts("2.9.44.0",
                @"Upgrade_2944_to_2952\Common",
                @"Upgrade_2944_to_2952\Orders");

            // Upgrade from 2.9.52.0 to 2.9.54.0
            RegisterScripts("2.9.52.0",
                @"Upgrade_2952_to_2954\Common",
                @"Upgrade_2952_to_2954\EndiciaShippers", 
                @"Upgrade_2952_to_2954\EndiciaPreferences",
                @"Upgrade_2952_to_2954\UspsShipments",
                @"Upgrade_2952_to_2954\UspsPackages",
                @"Upgrade_2952_to_2954\Shipments",
                @"Upgrade_2952_to_2954\Stores",
                @"Upgrade_2952_to_2954\Orders",
                @"Upgrade_2952_to_2954\Customers",
                @"Upgrade_2952_to_2954\ArchiveLog", 
                @"Upgrade_2952_to_2954\EndiciaScanForms");

            // Upgrade from 2.9.54.0 to 2.9.57.0
            RegisterScripts("2.9.54.0",
                @"Upgrade_2954_to_2957\Common",
                @"Upgrade_2954_to_2957\EndiciaPreferences");

            // Upgrade from 2.9.57 to 2.9.58
            RegisterScripts("2.9.57.0",
                @"Upgrade_2957_to_2958\Common",
                @"Upgrade_2957_to_2958\EmailAccounts");

            // Upgrade from 2.9.58 to 2.9.62
            RegisterScripts("2.9.57.0",
                @"Upgrade_2958_to_2962\Common",
                @"Upgrade_2958_to_2962\EndiciaShippers");

            // Upgrade from 2.9.62.0 to 2.9.65.0
            RegisterScripts("2.9.62.0",
                @"Upgrade_2962_to_2965\Common",
                @"Upgrade_2962_to_2965\Stores");

            #endregion
        }

        /// <summary>
        /// Perform the work unit estimate.
        /// </summary>
        protected override int RunEstimate(SqlConnection con)
        {
            Version installedVersion = SqlSchemaUpdater.GetInstalledSchemaVersion(con);

            // only allow upgrading from V2 versions we are aware of.
            if (installedVersion > new Version("2.9.65.0"))
            {
                throw new MigrationException("Your ShipWorks 2 database is newer than this version of ShipWorks 3 supports upgrading.\n\nPlease update to the latest version of ShipWorks 3 to upgrade your ShipWorks 2 database.");
            }

            int i = 0;
            foreach (V2UpdateBatch batch in updateBatches)
            {
                i++;
                if (installedVersion <= batch.Version)
                {
                    return updateBatches.Count - i;
                }
            }

            return 0;
        }

        /// <summary>
        /// Execute
        /// </summary>
        protected override int Run()
        {
            Version installedVersion;
            using (SqlConnection con = MigrationTaskBase.OpenConnectionForTask(this))
            {
                // capture the current database version
                installedVersion = SqlSchemaUpdater.GetInstalledSchemaVersion(con);

                // The scripts must run in SQL 2000 mode, as that's what they were generated for.  Most databases will already be at that level unless
                // they were created in a manually installed instance of 05 or higher.
                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandText = string.Format("ALTER DATABASE {0} SET COMPATIBILITY_LEVEL = 80", con.Database);
                cmd.ExecuteNonQuery();
            }

            // need to use a non-default script loader since these scripts are coming from disk
            MigrationSqlScriptLoader loader = new MigrationSqlScriptLoader(new DirectoryInfo(baseScriptPath));

            int i = 0;
            foreach (V2UpdateBatch batch in updateBatches)
            {
                if (installedVersion <= batch.Version)
                {
                    // set the progress detail text to show what's going on
                    if (i < updateBatches.Count - 1)
                    {
                        V2UpdateBatch nextBatch = updateBatches[i + 1];
                        Progress.Detail = String.Format("Updating {0}to {1}...", IsArchiveDatabase ? Database + " " : "", nextBatch.Version.ToString());
                    }
                    else
                    {
                        Progress.Detail = String.Format("Updating {0}...", Database);
                    }

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, SqlCommandProvider.DefaultTimeout))
                    {
                        using (SqlConnection con = MigrationTaskBase.OpenConnectionForTask(this))
                        {
                            // be sure to turn on SW 2 Connection options
                            SqlCommandProvider.ExecuteNonQuery(con, "SET ARITHABORT ON");
                            SqlCommandProvider.ExecuteNonQuery(con, String.Format("SET QUOTED_IDENTIFIER {0}", batch.SetQuotedIdentifiers ? "ON" : "OFF"));

                            log.InfoFormat("Executing V2 scripts on {0} for version {1}", Database, batch.Version);

                            // run this batch's scripts
                            foreach (string script in batch.Scripts)
                            {
                                loader[script].Execute(con);
                            }

                            // commit
                            scope.Complete();
                        
                            ReportWorkProgress();
                        }
                    }
                }

                // next batch counter
                i++;
            }

            return 0;
        }
    }
}
