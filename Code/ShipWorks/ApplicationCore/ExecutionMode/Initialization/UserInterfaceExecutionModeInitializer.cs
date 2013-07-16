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
using ShipWorks.ApplicationCore.Logging;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Editions;
using ShipWorks.Filters;
using ShipWorks.Filters.Management;
using ShipWorks.UI;
using ShipWorks.UI.Controls;

namespace ShipWorks.ApplicationCore.ExecutionMode.Initialization
{
    /// <summary>
    /// An implementation of the IExecutionModeInitializer interface intended to be used when
    /// ShipWorks is running with a UI.
    /// </summary>
    public class UserInterfaceExecutionModeInitializer : ExecutionModeInitializerBase
    {
        /// <summary>
        /// Intended for settng up/initializing any dependencies for an execution context.
        /// </summary>
        public override void Initialize()
        {
            // Order is important here due to license dependencies of third party components 
            // and other ShipWorks initialization processes.
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            // Check for illegal cross thread calls in any mode - not just when the debugger is attached, which is the default
            Control.CheckForIllegalCrossThreadCalls = true;

            // For Divilements licensing
            Divelements.SandGrid.SandGridBase.ActivateProduct("120|iTixOUJcBvFZeCMW0Zqf8dEUqM0=");
            Divelements.SandRibbon.Ribbon.ActivateProduct("120|wmbyvY12rhj+YHC5nTIyBO33bjE=");
            TD.SandDock.SandDockManager.ActivateProduct("120|cez0Ci0UI1owSCvXUNrMCcZQWik=");
            
            PerformCommonInitialization();
            
            // Initialize window state
            WindowStateSaver.Initialize(Path.Combine(DataPath.WindowsUserSettings, "windows.xml"));
            CollapsibleGroupControl.Initialize(Path.Combine(DataPath.WindowsUserSettings, "collapsiblegroups.xml"));

            // For syntax editor
            SemanticParserService.Start();

            // Register some idle cleanup work.
            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedFilterCounts", FilterContentManager.DeleteAbandonedFilterCounts, "doing maintenance", TimeSpan.FromHours(2));
            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedQuickFilters", QuickFilterHelper.DeleteAbandonedFilters, "doing maintenance", TimeSpan.FromHours(2));
            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedResources", DataResourceManager.DeleteAbandonedResourceData, "cleaning up resources", TimeSpan.FromHours(2));

            // Start idle processing
            IdleWatcher.Initialize();
        }
    }
}
