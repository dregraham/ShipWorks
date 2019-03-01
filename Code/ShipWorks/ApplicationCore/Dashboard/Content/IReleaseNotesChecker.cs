using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Check whether the release notes dashboard item should be displayed
    /// </summary>
    public interface IReleaseNotesChecker
    {
        /// <summary>
        /// Show the release notes dashboard item, if necessary
        /// </summary>
        Task<Unit> ShowReleaseNotesIfNecessary(ISynchronizeInvoke invoker, IUserEntity user);
    }
}