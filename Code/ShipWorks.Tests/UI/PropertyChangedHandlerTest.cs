using ShipWorks.Core.UI;
using System.ComponentModel;
using Xunit;

namespace ShipWorks.Tests.UI
{
    public class PropertyChangedHandlerTest
    {
        private string testField;
        public event PropertyChangedEventHandler TestEventHandler;

        [Fact]
        public void Set_RaisesEvent_WhenPropertyChangesAndEventIsRegisteredPriorToConstructor()
        {
            bool wasCalled = false;
            TestEventHandler += (s, e) => wasCalled = true;

            var handler = new PropertyChangedHandler(() => TestEventHandler);

            handler.Set("Foo", ref testField, "Value");

            Assert.True(wasCalled);
        }

        [Fact]
        public void Set_RaisesEvent_WhenPropertyChangesAndEventIsRegisteredAfterConstructor()
        {
            bool wasCalled = false;
            var handler = new PropertyChangedHandler(() => TestEventHandler);

            TestEventHandler += (s, e) => wasCalled = true;

            handler.Set("Foo", ref testField, "Value");

            Assert.True(wasCalled);
        }

        [Fact]
        public void Set_DoesNotRaiseEvent_WhenPropertyDoesNotChange()
        {
            bool wasCalled = false;
            testField = "Value";

            var handler = new PropertyChangedHandler(() => TestEventHandler);

            TestEventHandler += (s, e) => wasCalled = true;

            handler.Set("Foo", ref testField, "Value");

            Assert.False(wasCalled);
        }

        [Fact]
        public void Set_DoesNotThrow_WhenNoEventHandlerIsRegistered()
        {
            var handler = new PropertyChangedHandler(() => TestEventHandler);
            
            handler.Set("Foo", ref testField, "Value");
        }

        [Fact]
        public void Set_ChangesFieldValue()
        {
            var handler = new PropertyChangedHandler(() => TestEventHandler);

            handler.Set("Foo", ref testField, "Bar");

            Assert.Equal("Bar", testField);
        }

        [Fact]
        public void Set_ReturnsTrue_WhenFieldIsChanged()
        {
            var handler = new PropertyChangedHandler(() => TestEventHandler);

            bool result = handler.Set("Foo", ref testField, "Bar");

            Assert.True(result);
        }

        [Fact]
        public void Set_ReturnsFalse_WhenFieldIsNotChanged()
        {
            testField = "Bar";
            var handler = new PropertyChangedHandler(() => TestEventHandler);

            bool result = handler.Set("Foo", ref testField, "Bar");

            Assert.False(result);
        }
    }
}
