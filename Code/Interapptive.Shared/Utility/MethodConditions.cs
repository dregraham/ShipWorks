using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Conditions for valid method calls
    /// </summary>
    public static class MethodConditions
    {
        /// <summary>
        /// Throw an ArgumentNullException if the specified object is null
        /// </summary>
        public static T EnsureArgumentIsNotNull<T>(T testObject, string parameterName) where T : class
        {
            if (testObject == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return testObject;
        }
    }
}
