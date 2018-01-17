using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Users;

namespace ShipWorks.Tests.Shared.Database
{
    /// <summary>
    /// Fixture that will create a database that can be used to test against, with a reusable context
    /// </summary>
    /// <seealso cref="ShipWorks.Tests.Shared.Database.DatabaseFixture" />
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
        public DataContext GetNewDataContext(Action<AutoMock, ContainerBuilder> addExtraRegistrations, Guid instance, string contextName)
        {
            if (GetExistingContext(contextName) != null)
            {
                throw new InvalidOperationException("Cannot get a new data context when an existing one of the same context name already exists");
            }

            previousContextName = contextName;
            context = CreateReusableDataContext(addExtraRegistrations, instance);

            return context;
        }

        /// <summary>
        /// Creates the reusable data context.
        /// </summary>
        /// <remarks>
        /// Removed initialization of LogSession because it makes getting details on specific test failures difficult.
        /// </remarks>
        private DataContext CreateReusableDataContext(Action<AutoMock, ContainerBuilder> addExtraRegistrations, Guid instance)
        {
            var newContext = CreateDataContext(addExtraRegistrations);

            ShipWorksSession.Initialize(instance);

            UserSession.InitializeForCurrentDatabase();

            return newContext;
        }
    }
}
