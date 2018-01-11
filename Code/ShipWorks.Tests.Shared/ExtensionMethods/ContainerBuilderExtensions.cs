using Autofac;
using Autofac.Extras.Moq;
using Moq;

namespace ShipWorks.Tests.Shared.ExtensionMethods
{
    /// <summary>
    /// Extension methods on ContainerBuilder
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Register a mocked instance of the given type
        /// </summary>
        public static Mock<T> RegisterMock<T>(this ContainerBuilder builder, AutoMock mock) where T : class
        {
            Mock<T> mocked = mock.CreateMock<T>();
            builder.RegisterInstance(mocked.Object).As<T>();
            return mocked;
        }
    }
}
