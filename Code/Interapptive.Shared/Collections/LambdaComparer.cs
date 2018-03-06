﻿using System;
using System.Collections.Generic;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Helper class for comparing lists
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LambdaComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _expression;

        /// <summary>
        /// Constructor
        /// </summary>
        public LambdaComparer(Func<T, T, bool> lambda)
        {
            _expression = lambda;
        }

        /// <summary>
        /// Equals
        /// </summary>
        public bool Equals(T x, T y)
        {
            return _expression(x, y);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        public int GetHashCode(T obj)
        {
            /*
             If you just return 0 for the hash the Equals comparer will kick in. 
             The underlying evaluation checks the hash and then short circuits the evaluation if it is false.
             Otherwise, it checks the Equals. If you force the hash to be true (by assuming 0 for both objects), 
             you will always fall through to the Equals check which is what we are always going for.
            */
            return 0;
        }
    }
}
