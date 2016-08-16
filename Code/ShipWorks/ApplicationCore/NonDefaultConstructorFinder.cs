using System;
using System.Reflection;
using Autofac.Core.Activators.Reflection;
using System.Linq;

namespace ShipWorks.Core.ApplicationCode
{
    /// <summary>
    /// Instruct Autofac to ignore constructors with no parameters
    /// </summary>
    /// <remarks>This should be used for classes that have a default constructor but that we don't want Autofac
    /// to ever use.  This causes a very useful Autofac exception when trying to resolve a dependency
    /// instead of null reference exceptions in locations that don't seem related</remarks>
    public class NonDefaultConstructorFinder : IConstructorFinder
    {
        /// <summary>
        /// Find constructors that have at least one parameter
        /// </summary>
        public ConstructorInfo[] FindConstructors(Type targetType) =>
            new DefaultConstructorFinder().FindConstructors(targetType).Where(x => x.GetParameters().Any()).ToArray();
    }
}