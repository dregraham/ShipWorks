using System;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Orders.Archive.Errors;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Orchestrate the order archiving process
    /// </summary>
    [Component]
    public class OrderArchiveOrchestrator : IOrderArchiveOrchestrator
    {
        private readonly IOrderArchiver archiver;
        private readonly ISecurityContext userSecurity;
        private readonly IAsyncMessageHelper messageHelper;
        private readonly IOrderArchiveViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderArchiveOrchestrator(
            IOrderArchiver archiver,
            ISecurityContext userSecurity,
            IAsyncMessageHelper messageHelper,
            IOrderArchiveViewModel viewModel)
        {
            this.messageHelper = messageHelper;
            this.viewModel = viewModel;
            this.userSecurity = userSecurity;
            this.archiver = archiver;
        }

        /// <summary>
        /// Start the archiving process
        /// </summary>
        public Task<Unit> Archive()
        {
            return userSecurity.RequestPermission(PermissionType.DatabaseArchive, null)
                .Bind(viewModel.GetArchiveDateFromUser)
                .Bind(archiver.Archive)
                .Do(DisplaySuccess, DisplayError);
        }

        /// <summary>
        /// Display a success message
        /// </summary>
        private Task DisplaySuccess(Unit _) =>
            messageHelper.ShowMessage("Archive finished");

        /// <summary>
        /// Display an error
        /// </summary>
        private Task DisplayError(Exception ex) =>
            ex == Error.Canceled ?
                Task.CompletedTask :
                messageHelper.ShowError(ex.Message);
    }
}
