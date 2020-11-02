using System;
using System.Threading.Tasks;
using System.Windows.Input;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using ShipWorks.Installer.Extensions;

namespace ShipWorks.Installer.Utilities
{
    /// <summary>
    /// Async Command
    /// </summary>
    public class AsyncCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged;

        private bool isExecuting;
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;
        private readonly IErrorHandler errorHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        public AsyncCommand(
            Func<Task> execute,
            Func<bool> canExecute = null,
            ILog log = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            errorHandler = new ErrorHandler(LogManager.GetLogger(typeof(AsyncCommand)));
        }

        /// <summary>
        /// Can execute?
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            return !isExecuting && (canExecute?.Invoke() ?? true);
        }

        /// <summary>
        /// Execute async
        /// </summary>
        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    isExecuting = true;
                    await execute();
                }
                finally
                {
                    isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Raise can execute changed
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Can execute?
        /// </summary>
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <summary>
        /// Execute
        /// </summary>
        void ICommand.Execute(object parameter)
        {
            ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }
    }
}
