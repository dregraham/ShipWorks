﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users;

namespace ShipWorks.Tests.Shared.Database
{
    public class DatabaseFixtureWithReusableContext : DatabaseFixture
    {
        private DataContext context;
        private string previousContextName = string.Empty;

        /// <summary>
        /// Gets the existing context based on contextName - null if none exists
        /// </summary>
        public DataContext GetExistingContext(string contextName)
        {
            return context == null || previousContextName != contextName ? null : context;
        }

        /// <summary>
        /// Returns the datacontext. A new one is created if contextName differs from previous contextName or context hasn't been resolved.
        /// </summary>
        public DataContext GetNewDataContext(Action<IContainer> initializeContainer, Guid instance, string contextName)
        {
            if (GetExistingContext(contextName) != null)
            {
                throw new InvalidOperationException("Cannot get a new data context when an existing one of the same context name already exists");
            }

            previousContextName = contextName;
            context = CreateReusableDataContext(initializeContainer, instance);

            return context;
        }

        /// <summary>
        /// Creates the reusable data context.
        /// </summary>
        private DataContext CreateReusableDataContext(Action<IContainer> initializeContainer, Guid instance)
        {
            var newContext = CreateDataContext(initializeContainer);

            newContext.Mock.Provide<Control>(new Control());
            newContext.Mock.Provide<Func<Control>>(() => new Control());
            newContext.Mock.Override<ITangoWebClient>();
            newContext.Mock.Override<IMessageHelper>();

            ShipWorksSession.Initialize(instance);
            LogSession.Initialize();
            UserSession.InitializeForCurrentDatabase();

            return newContext;
        }
    }
}
