using System;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Users.Security;

namespace ShipWorks.Archiving
{
    /// <summary>
    /// Orchestrate the order archiving process
    /// </summary>
    [Component]
    public class OrderArchiveOrchestrator : IOrderArchiveOrchestrator
    {
        private readonly IOrderArchiver archiver;
        private readonly ISecurityContext userSecurity;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IAsyncMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderArchiveOrchestrator(IOrderArchiver archiver, ISecurityContext userSecurity, IDateTimeProvider dateTimeProvider, IAsyncMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.dateTimeProvider = dateTimeProvider;
            this.userSecurity = userSecurity;
            this.archiver = archiver;
        }

        /// <summary>
        /// Start the archiving process
        /// </summary>
        public Task<Unit> Archive()
        {
            return userSecurity.RequestPermission(PermissionType.DatabaseArchive, null)
                .Bind(() => archiver.Archive(dateTimeProvider.Now.Subtract(TimeSpan.FromDays(90))))
                .Do(
                    x => messageHelper.ShowMessage("Archive finished"),
                    ex => messageHelper.ShowError(ex.Message));
        }
    }
}
