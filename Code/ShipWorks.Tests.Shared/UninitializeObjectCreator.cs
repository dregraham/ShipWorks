using System.Runtime.Serialization;

namespace ShipWorks.Tests.Shared
{
    public static class UninitializeObjectCreator
    {
        /// <summary>
        /// Creates an Uninitialized object of type T.
        /// </summary>
        public static T Create<T>() where T : class
        {
            return FormatterServices.GetUninitializedObject(typeof(T)) as T;
        }
    }
}
