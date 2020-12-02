using System.Windows.Input;
using System.Threading.Tasks;

namespace ShipWorks.Installer.Utilities
{
    /// <summary>
    /// Interface for async commands
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The Task to execute</returns>
        Task ExecuteAsync();

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        void RaiseCanExecuteChanged();
	}
}
