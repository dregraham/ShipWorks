namespace ShipWorks.ApplicationCore.ExecutionMode.Initialization
{
    public interface IExecutionModeInitializer
    {
        /// <summary>
        /// Intended for setting up/initializing any dependencies for an execution mode/context.
        /// </summary>
        void Initialize();
    }
}
