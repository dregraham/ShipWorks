using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Tokens;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Utility class for executing user requests for updating status changes
    /// </summary>
    public static class StatusPresetCommands
    {
        /// <summary>
        /// Create the list of MenuCommand objects that currently apply.  The status' will be updated with the builtin live database update executor.
        /// </summary>
        public static List<MenuCommand> CreateMenuCommands(StatusPresetTarget target, List<long> storeKeys)
        {
            return CreateMenuCommands(target, storeKeys, false);
        }

        /// <summary>
        /// Create the list of MenuCommand objects that currently apply.  The status' will be updated with the builtin live database update executor.
        /// </summary>
        public static List<MenuCommand> CreateMenuCommands(StatusPresetTarget target, List<long> storeKeys, bool showEditPresets)
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            MenuCommandExecutor executor = GetCommandHandler(target);

            // Add the blank one at the top, its always there
            MenuCommand command = new MenuCommand("", new MenuCommandExecutor(executor));
            command.Tag = new StatusPresetEntity { StatusText = "", StoreID = null };
            commands.Add(command);

            commands.AddRange(CreateMenuCommands(StatusPresetManager.GetGlobalPresets(target), executor));

            if (commands.Count > 0)
            {
                commands[commands.Count - 1].BreakAfter = true;
            }

            List<StoreEntity> stores = StoreManager.GetAllStores().Where(s => storeKeys.Contains(s.StoreID)).ToList();

            // Now do for each specific store
            foreach (StoreEntity store in stores)
            {
                ICollection<StatusPresetEntity> presets = StatusPresetManager.GetStorePresets(store, target);
                if (presets.Count > 0)
                {
                    if (stores.Count > 1)
                    {
                        MenuCommand storeRoot = new MenuCommand(store.StoreName);
                        commands.Add(storeRoot);

                        storeRoot.ChildCommands.AddRange(CreateMenuCommands(presets, executor));
                    }
                    else
                    {
                        commands.AddRange(CreateMenuCommands(presets, executor));
                    }
                }
            }

            // Create the commands for editing the status presets.  Editing the presets is considered management of the store,
            // so they have to be clear to do that.
            if (showEditPresets && UserSession.Security.HasPermission(PermissionType.ManageStores))
            {
                CreateEditPresetsCommands(commands, stores);
            }

            return commands;
        }

        /// <summary>
        /// Create the commands for editing the status presets
        /// </summary>
        private static void CreateEditPresetsCommands(List<MenuCommand> commands, List<StoreEntity> stores)
        {
            // If more than one store, the edit presets menu goes in a sub-menu
            if (stores.Count > 1)
            {
                MenuCommand editPresetsRoot = new MenuCommand("Edit presets for...");
                editPresetsRoot.BreakBefore = true;

                commands.Add(editPresetsRoot);

                foreach (StoreEntity store in stores)
                {
                    MenuCommand editPresets = new MenuCommand(store.StoreName, new MenuCommandExecutor(OnEditPresets));
                    editPresets.Tag = store;

                    editPresetsRoot.ChildCommands.Add(editPresets);
                }
            }
            else
            {
                MenuCommand editPresets = new MenuCommand("Edit presets...", new MenuCommandExecutor(OnEditPresets));
                editPresets.BreakBefore = true;
                editPresets.Tag = stores[0];

                commands.Add(editPresets);
            }
        }

        /// <summary>
        /// Create the menu commands for the given presets
        /// </summary>
        private static List<MenuCommand> CreateMenuCommands(ICollection<StatusPresetEntity> presets, MenuCommandExecutor executor)
        {
            List<MenuCommand> commands = new List<MenuCommand>();

            // First add each global preset
            foreach (StatusPresetEntity preset in presets)
            {
                MenuCommand command = new MenuCommand(preset.StatusText.Replace("&", "&&"), executor);
                command.Tag = preset;

                commands.Add(command);
            }

            return commands;
        }

        /// <summary>
        /// Get the handler to use for setting status of the given target
        /// </summary>
        private static MenuCommandExecutor GetCommandHandler(StatusPresetTarget target)
        {
            switch (target)
            {
                case StatusPresetTarget.Order:
                    return OnSetOrderStatus;

                case StatusPresetTarget.OrderItem:
                    return OnSetItemStatus;
            }

            throw new InvalidOperationException("Unhandled target in GetCommandHandler: " + target);
        }

        /// <summary>
        /// Edit store status presets
        /// </summary>
        private static void OnEditPresets(MenuCommandExecutionContext context)
        {
            StoreEntity store = (StoreEntity) context.MenuCommand.Tag;

            using (StoreSettingsDlg dlg = new StoreSettingsDlg(store, StoreSettingsDlg.Section.StatusPresets))
            {
                dlg.ShowDialog(context.Owner);
            }
        }

        /// <summary>
        /// Set the order status
        /// </summary>
        private static void OnSetOrderStatus(MenuCommandExecutionContext context)
        {
            BeginSetStatus(SetOrderStatusCallback, context);
        }

        /// <summary>
        /// Set the item status
        /// </summary>
        private static void OnSetItemStatus(MenuCommandExecutionContext context)
        {
            BeginSetStatus(SetItemStatusCallback, context);
        }

        /// <summary>
        /// Begin setting the status with the given executor
        /// </summary>
        private static void BeginSetStatus(BackgroundExecutorCallback<long> statusSetter, MenuCommandExecutionContext context)
        {
            BackgroundExecutor<long> executor = new BackgroundExecutor<long>(context.Owner,
                "Set Status",
                "ShipWorks is setting the local status.",
                "Updating order {0} of {1}...");

            // Code to execute when all items are done
            executor.ExecuteCompleted += (sender, e) =>
            {
                if (e.Issues.Count == 0)
                {
                    context.Complete();
                }
                // Issues are added for insufficient permissions.  If there are any, issue a warning.
                else
                {
                    context.Complete(MenuCommandResult.Warning, "The status of some orders were not set due to insufficient permission.");
                }
            };

            executor.ExecuteAsync(

                // Code to execute for each key
                statusSetter,

                // The keys to execute for
                context.SelectedKeys,

                // User state
                context

                );
        }

        /// <summary>
        /// The worker thread function that does the actual status setting
        /// </summary>
        private static void SetOrderStatusCallback(long orderID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            MenuCommandExecutionContext context = (MenuCommandExecutionContext) userState;

            // Validate permissions
            if (!UserSession.Security.HasPermission(PermissionType.OrdersEditStatus, orderID))
            {
                issueAdder.Add(orderID);
                return;
            }

            IMenuCommand command = context.MenuCommand;
            StatusPresetEntity preset = (StatusPresetEntity) command.Tag;

            OrderEntity prototype = new OrderEntity(orderID) { IsNew = false };
            prototype.LocalStatus = TemplateTokenProcessor.ProcessTokens(preset.StatusText, orderID);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveEntity(prototype);
            }
        }

        /// <summary>
        /// The worker thread function that does the actual status setting
        /// </summary>
        private static void SetItemStatusCallback(long orderItemID, object userState, BackgroundIssueAdder<long> issueAdder)
        {
            MenuCommandExecutionContext context = (MenuCommandExecutionContext) userState;

            // Validate permissions
            if (!UserSession.Security.HasPermission(PermissionType.OrdersEditStatus, orderItemID))
            {
                issueAdder.Add(orderItemID);
                return;
            }

            IMenuCommand command = context.MenuCommand;
            StatusPresetEntity preset = (StatusPresetEntity) command.Tag;

            OrderItemEntity prototype = new OrderItemEntity(orderItemID) { IsNew = false };
            prototype.LocalStatus = TemplateTokenProcessor.ProcessTokens(preset.StatusText, orderItemID);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveEntity(prototype);
            }
        }
    }
}
