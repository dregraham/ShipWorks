using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ActiproSoftware.SyntaxEditor;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Editions;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Filters;
using ShipWorks.Filters.Management;
using ShipWorks.UI;
using ShipWorks.UI.Controls;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// An implementation of the IExectionMode interface intended to be used when running ShipWorks with a UI.
    /// </summary>
    public class UserInterfaceExecutionMode : IExecutionMode
    {
        private readonly ILog log;
        private readonly MainForm mainForm;


        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceExecutionMode"/> class.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        public UserInterfaceExecutionMode(MainForm mainForm)
            : this(mainForm, LogManager.GetLogger(typeof (UserInterfaceExecutionMode)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceExecutionMode" /> class.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        /// <param name="log">The log.</param>
        public UserInterfaceExecutionMode(MainForm mainForm, ILog log)
        {
            this.mainForm = mainForm;
            this.log = log;
        }

        /// <summary>
        /// Determines whether the exection mode supports interacting with the user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> ShipWorks is running in a mode that interacts with the user; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserInteractive()
        {
            return true;
        }

        /// <summary>
        /// Runs the ShipWorks UI.
        /// </summary>
        public void Execute()
        {
            // Now we are ready to show the splash
            SplashScreen.ShowSplash();

            // For syntax editor
            SemanticParserService.Start();

            // Register some idle cleanup work.
            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedFilterCounts", FilterContentManager.DeleteAbandonedFilterCounts, "doing maintenance", TimeSpan.FromHours(2));
            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedQuickFilters", QuickFilterHelper.DeleteAbandonedFilters, "doing maintenance", TimeSpan.FromHours(2));
            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedResources", DataResourceManager.DeleteAbandonedResourceData, "cleaning up resources", TimeSpan.FromHours(2));

            mainForm.Load += new EventHandler(OnMainFormLoaded);

            SplashScreen.Status = "Loading ShipWorks...";
            Application.Run(mainForm);
        }

        /// <summary>
        /// When the main form has loaded we wait for the splash minimum time to be up.
        /// </summary>
        private void OnMainFormLoaded(object sender, EventArgs e)
        {
            mainForm.Activate();
            SplashScreen.CloseSplash();

            // Start idle processing
            IdleWatcher.Initialize();

            log.InfoFormat("Application activated.");
        }

        public void HandleException(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
