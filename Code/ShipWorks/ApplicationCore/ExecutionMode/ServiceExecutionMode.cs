using log4net;
using ShipWorks.ApplicationCore.ExecutionMode.Initialization;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using System;
using System.Collections.Generic;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    public class ServiceExecutionMode : IExecutionMode
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceExecutionMode));

        readonly Lazy<IServiceHost> host;
        readonly IList<string> options;
        readonly IExecutionModeInitializer initializer;

        public ServiceExecutionMode(string serviceName, IList<string> options)
            : this(serviceName, options, null, null) { }

        public ServiceExecutionMode(string serviceName, IList<string> options, IServiceHostFactory hostFactory, IExecutionModeInitializer initializer)
        {
            if (null == serviceName)
                throw new ArgumentNullException("serviceName");
            if (null == hostFactory)
                hostFactory = new DefaultServiceHostFactory();

            this.host = new Lazy<IServiceHost>(() =>
                hostFactory.GetHostFor(GetServiceForName(serviceName))
            );

            this.options = options ?? new string[0];
            this.initializer = initializer ?? new ServiceExecutionModeInitializer();
        }

        IServiceHost Host
        {
            get { return host.Value; }
        }

        public bool IsUserInteractive
        {
            get { return false; }
        }

        public void Execute()
        {
            log.Info("Running as a service.");

            if (options.Contains("/stop"))
            {
                Host.Stop();
            }
            else
            {
                initializer.Initialize();
                Host.Run();
            }
        }

        public void HandleException(Exception exception)
        {
            if (UserSession.IsLoggedOn)
            {
                UserSession.Logoff(false);
            }
            UserSession.Reset();

            if (ConnectionMonitor.HandleTerminatedConnection(exception))
            {
                log.Info("Terminating due to unrecoverable connection.", exception);
            }
            else
            {
                log.Fatal("Application Crashed", exception);
            }

            Host.OnUnhandledException(exception);
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
