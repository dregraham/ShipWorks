using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Users.Audit;
using Xunit;

namespace ShipWorks.Tests.Users.Audit
{
    public class ImmutableStackContainerTest
    {
        [Fact]
        public void Push_AddsItemToStack()
        {
            var testObject = new ImmutableStackContainer<int>();
            testObject.Push(3);
            Assert.Equal(3, testObject.Peek());
        }

        [Fact]
        public void Push_AddsTwoItemsToStack()
        {
            var testObject = new ImmutableStackContainer<int>();
            testObject.Push(3);
            testObject.Push(5);
            Assert.Equal(5, testObject.Peek());
            testObject.Pop();
            Assert.Equal(3, testObject.Peek());
        }

        [Fact]
        public void Enumerate_ReturnsInitialValues_WhenPushingDuringEnumeration()
        {
            var testObject = new ImmutableStackContainer<int>();
            testObject.Push(3);
            testObject.Push(5);

            var enumerator = testObject.GetEnumerator();
            testObject.Push(6);

            var items = new List<int>();
            while (enumerator.MoveNext())
            {
                items.Add(enumerator.Current);
            }

            Assert.Equal(new [] { 5, 3 }, items);
        }
    }
}
