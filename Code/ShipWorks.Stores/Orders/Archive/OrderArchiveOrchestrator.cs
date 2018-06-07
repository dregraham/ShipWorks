using System;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
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
                .Bind(d => archiver.Archive(d, true))
                .Do(DisplaySuccess, DisplayError)
                .Map(_ => Unit.Default);
        }

        /// <summary>
        /// Display a success message
        /// </summary>
        private Task DisplaySuccess(IResult _) =>
            Task.CompletedTask;

        /// <summary>
        /// Display an error
        /// </summary>
        private Task DisplayError(Exception ex) =>
            ex == Error.Canceled ?
                Task.CompletedTask :
                messageHelper.ShowError(ex.Message);
    }
}
