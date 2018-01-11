using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Extensions for use on all objects
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Ensure a property has a value
        /// </summary>
        public static TProp Ensure<T, TProp>(this T obj, Expression<Func<T, TProp>> getter) where TProp : class, new() =>
            Ensure(obj, getter, () => new TProp());

        /// <summary>
        /// Ensure a property has a value
        /// </summary>
        public static TProp Ensure<T, TProp, TObj>(this T obj, Expression<Func<T, TProp>> getter, Func<TObj> creator)
            where TObj : TProp
        {
            var value = getter.Compile()(obj);

            if (getter.Compile()(obj) == null)
            {
                value = creator();
                GetSetter(getter)(obj, value);
            }

            return value;
        }

        /// <summary>
        /// Ensure an array property is initialized
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="obj"></param>
        /// <param name="getter"></param>
        public static TProp[] Ensure<T, TProp>(this T obj, Expression<Func<T, TProp[]>> getter)
        {
            var value = getter.Compile()(obj);

            if (getter.Compile()(obj) == null)
            {
                value = new TProp[0];
                GetSetter(getter)(obj, value);
            }

            return value;
        }

        /// <summary>
        /// Ensure an array property is initialized with at least one element
        /// </summary>
        /// <returns>
        /// First element of the array, which is a new element if the array was null or empty
        /// </returns>
        public static TProp EnsureAtLeastOne<T, TProp>(this T obj, Expression<Func<T, TProp[]>> getter) where TProp : class, new()
        {
            var array = getter.Compile()(obj);
            if (array?.Any() == true)
            {
                return array.First();
            }

            var value = new TProp();
            GetSetter(getter)(obj, new[] { value });
            return value;
        }

        /// <summary>
        /// Get a setter for a given getter
        /// </summary>
        private static Action<TObject, TValue> GetSetter<TObject, TValue>(Expression<Func<TObject, TValue>> property)
        {
            var memberExp = (MemberExpression) property.Body;
            var propInfo = (PropertyInfo) memberExp.Member;
            MethodInfo setter = propInfo.GetSetMethod();
            Delegate del = Delegate.CreateDelegate(typeof(Action<TObject, TValue>), setter);
            return (Action<TObject, TValue>) del;
        }
    }
}
