using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Tests.AddressValidation.Predicates
{
    public class FakePredicateExpression : IPredicateExpression
    {
        public bool ContainsPredicate(IPredicate predicate)
        {
            FieldCompareValuePredicate fieldCompare = predicate as FieldCompareValuePredicate;
            if (fieldCompare != null)
            {
                return ContainsPredicate(fieldCompare);
            }

            return false;
        }

        private bool ContainsPredicate(FieldCompareValuePredicate predicate)
        {
            return Predicates.OfType<FieldCompareValuePredicate>().Any(x => ArePredicatesEqual(x, predicate));
        }

        private static bool ArePredicatesEqual(FieldCompareValuePredicate left, FieldCompareValuePredicate right)
        {
            var isName = left.FieldCore.Name == right.FieldCore.Name;
            var isValue = left.Value.Equals(right.Value);
            var isOperator = left.Operator == right.Operator;

            return isName && isValue && isOperator;
        }

        public List<IPredicate> Predicates = new List<IPredicate>();

        public List<object> GetFrameworkElementsInPredicate()
        {
            throw new NotImplementedException();
        }

        public string ToQueryText(ref int uniqueMarker)
        {
            throw new NotImplementedException();
        }

        public string ToQueryText(ref int uniqueMarker, bool inHavingClause)
        {
            throw new NotImplementedException();
        }

        public List<IDataParameter> Parameters { get; private set; }
        public bool Negate { get; set; }
        public IDbSpecificCreator DatabaseSpecificCreator { get; set; }
        public int InstanceType { get; set; }
        public string ObjectAlias { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IPredicateExpression Add(IPredicate predicateToAdd)
        {
            Predicates.Add(predicateToAdd);
            return this;
        }

        public IPredicateExpression AddWithOr(IPredicate predicateToAdd)
        {
            Predicates.Add(predicateToAdd);
            return this;
        }

        public IPredicateExpression AddWithAnd(IPredicate predicateToAdd)
        {
            Predicates.Add(predicateToAdd);
            return this;
        }

        public void Clear()
        {
            Predicates.Clear();
        }

        public IPredicateExpressionElement this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get
            {
                return Predicates.Count;
            }
        }
    }
}