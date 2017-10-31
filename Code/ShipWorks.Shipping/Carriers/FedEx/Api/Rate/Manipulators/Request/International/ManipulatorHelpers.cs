﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International
{
    /// <summary>
    /// Helper methods for creating manipulators
    /// </summary>
    public static class ManipulatorHelpers
    {
        /// <summary>
        /// Ensure a property has a value
        /// </summary>
        public static TProp Ensure<T, TProp>(this T obj, Expression<Func<T, TProp>> getter) where TProp : class, new()
        {
            var value = getter.Compile()(obj);

            if (getter.Compile()(obj) == null)
            {
                value = new TProp();
                GetSetter(getter)(obj, value);
            }

            return value;
        }

        /// <summary>
        /// Ensure a property has a value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="obj"></param>
        /// <param name="getter"></param>
        public static void Ensure<T, TProp>(this T obj, Expression<Func<T, TProp[]>> getter)
        {
            if (getter.Compile()(obj) == null)
            {
                GetSetter(getter)(obj, new TProp[0]);
            }
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
