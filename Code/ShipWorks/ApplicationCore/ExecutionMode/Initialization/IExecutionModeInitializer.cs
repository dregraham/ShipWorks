namespace ShipWorks.ApplicationCore.ExecutionMode.Initialization
{
    public interface IExecutionModeInitializer
    {
        /// <summary>
        /// Intended for setting up/initializing any dependencies for an execution context.
        /// </summary>
        void Initialize();
    }
}
