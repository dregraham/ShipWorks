using log4net;
using ShipWorks.ApplicationCore.ExecutionMode.Initialization;
using ShipWorks.ApplicationCore.WindowsServices;
using System;
using System.Collections.Generic;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    public class ServiceExecutionMode : IExecutionMode
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceExecutionMode));

        readonly string serviceName;
        readonly IList<string> options;
        readonly IServiceExecutionStrategyFactory strategyFactory;
        readonly IExecutionModeInitializer initializer;

        public ServiceExecutionMode(string serviceName, IList<string> options)
            : this(serviceName, options, null, null) { }

        public ServiceExecutionMode(string serviceName, IList<string> options, IServiceExecutionStrategyFactory strategyFactory, IExecutionModeInitializer initializer)
        {
            if (null == serviceName)
                throw new ArgumentNullException("serviceName");

            this.serviceName = serviceName;
            this.options = options ?? new string[0];
            this.strategyFactory = strategyFactory ?? new DefaultServiceExecutionStrategyFactory();
            this.initializer = initializer ?? new ServiceExecutionModeInitializer();
        }


        public bool IsUserInteractive
        {
            get { return false; }
        }

        public void Execute()
        {
            log.Info("Running as a service.");

            var service = GetServiceForName(serviceName);

            var strategy = strategyFactory.GetStrategyFor(service);

            if (options.Contains("/stop"))
            {
                strategy.Stop();
            }
            else
            {
                initializer.Initialize();
                strategy.Run();
            }
        }

        public void HandleException(Exception exception)
        {
            throw new NotImplementedException();
        }


        static ShipWorksServiceBase GetServiceForName(string serviceName)
        {
            try
            {
                return ShipWorksServiceBase.GetService(serviceName);
            }
            catch (Exception ex)
            {
                throw new ExecutionModeConfigurationException(
                    string.Format("Could not find a service named '{0}'.", serviceName), ex
                );
            }
        }
    }
}
