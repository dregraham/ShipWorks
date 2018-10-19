using System.Collections.Generic;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class CommonEntityBaseTest
    {
        [Theory]
        [InlineData("Processed", true)]
        [InlineData("ShipFirstName", "Foo")]
        [InlineData("TotalWeight", 4.5)]
        public void PropertyChangeEventFires_WhenValueChanges(string fieldName, object value)
        {
            var changedField = string.Empty;

            var shipment = Create.Shipment().Build();
            shipment.PropertyChanged += (s, e) => changedField = e.PropertyName;

            shipment.SetNewFieldValue(fieldName, value);

            Assert.Equal(fieldName, changedField);
        }

        [Theory]
        [InlineData("Processed", false)]
        [InlineData("ShipFirstName", "")]
        [InlineData("TotalWeight", 0.0)]
        public void PropertyChangeEventDoesNotFire_WhenValueDidNotChange(string fieldName, object value)
        {
            var changedField = string.Empty;

            var shipment = Create.Shipment().Build();
            shipment.PropertyChanged += (s, e) => changedField = e.PropertyName;

            shipment.SetNewFieldValue(fieldName, value);

            Assert.Empty(changedField);
        }

        [Fact]
        public void PropertyChangeEventDoesNotFire_WhenDoubleValuesAreSimilar()
        {
            var shipment = Create.Shipment().Set(x => x.TotalWeight, 1.000000000002).Build();
            shipment.PropertyChanged += (s, e) => Assert.False(true, "A property changed that shouldn't have");

            shipment.TotalWeight = 1;
        }

        [Fact]
        public void SurpressPropertyChangeNotifications_DoesNotFireNotification_WhenInBlock()
        {
            var shipment = Create.Shipment().Build();
            shipment.PropertyChanged += (s, e) => Assert.False(true, "A property changed that shouldn't have");

            using (shipment.SurpressPropertyChangeNotifications())
            {
                shipment.Processed = true;
            }
        }

        [Fact]
        public void SurpressPropertyChangeNotifications_FiresNotification_WhenChangeIsAfterBlock()
        {
            var changedField = string.Empty;

            var shipment = Create.Shipment().Build();
            shipment.PropertyChanged += (s, e) => changedField = e.PropertyName;

            using (shipment.SurpressPropertyChangeNotifications())
            {
                shipment.Processed = true;
            }

            shipment.Voided = true;

            Assert.Equal("Voided", changedField);
        }

        [Fact]
        public void SurpressPropertyChangeNotifications_DoesNotFireNotification_WhenNestedSurpressIsDone()
        {
            var shipment = Create.Shipment().Build();
            shipment.PropertyChanged += (s, e) => Assert.False(true, "A property changed that shouldn't have");

            using (shipment.SurpressPropertyChangeNotifications())
            {
                using (shipment.SurpressPropertyChangeNotifications())
                {
                    shipment.Voided = true;
                }

                shipment.Processed = true;
            }
        }

        [Fact]
        public void BatchPropertyChangeNotifications_FiresNotification_AtTheEndOfBlock()
        {
            var changedField = string.Empty;

            var shipment = Create.Shipment().Build();
            shipment.PropertyChanged += (s, e) => changedField = e.PropertyName;

            using (shipment.BatchPropertyChangeNotifications())
            {
                shipment.Processed = true;

                changedField = string.Empty;
            }

            Assert.Equal("Processed", changedField);
        }

        [Fact]
        public void BatchPropertyChangeNotifications_FiresNotification_WhenChangeIsAfterBlock()
        {
            var changedField = string.Empty;

            var shipment = Create.Shipment().Build();
            shipment.PropertyChanged += (s, e) => changedField = e.PropertyName;

            using (shipment.BatchPropertyChangeNotifications())
            {
                shipment.Processed = true;
            }

            shipment.Voided = true;

            Assert.Equal("Voided", changedField);
        }

        [Fact]
        public void BatchPropertyChangeNotifications_DoesNotFireNotification_WhenNestedBatchIsDone()
        {
            var changedField = string.Empty;

            var shipment = Create.Shipment().Build();
            shipment.PropertyChanged += (s, e) => changedField = e.PropertyName;

            using (shipment.BatchPropertyChangeNotifications())
            {
                using (shipment.BatchPropertyChangeNotifications())
                {
                    shipment.Voided = true;
                }

                Assert.Equal("Voided", changedField);

                shipment.Processed = true;
            }

            Assert.Equal("Processed", changedField);
        }

        [Fact]
        public void BatchPropertyChangeNotifications_FiresMultipleNotifications_WhenMultiplePropertiesAreChanged()
        {
            var changedFields = new List<string>();

            var shipment = Create.Shipment().Build();
            shipment.PropertyChanged += (s, e) => changedFields.Add(e.PropertyName);

            using (shipment.BatchPropertyChangeNotifications())
            {
                shipment.Voided = true;
                shipment.Processed = true;
            }

            Assert.Equal("Voided", changedFields[0]);
            Assert.Equal("Processed", changedFields[1]);
        }

        [Fact]
        public void BatchPropertyChangeNotifications_FiresNotificationsCorrectly_WhenDisposeCausesFieldsToChange()
        {
            var changedFields = new List<string>();

            var shipment = Create.Shipment().Build();
            shipment.PropertyChanged += (s, e) =>
            {
                changedFields.Add(e.PropertyName);

                using (shipment.BatchPropertyChangeNotifications())
                {
                    shipment.Voided = true;
                }
            };

            using (shipment.BatchPropertyChangeNotifications())
            {
                shipment.Processed = true;
            }

            Assert.Equal("Processed", changedFields[0]);
            Assert.Equal("Voided", changedFields[1]);
        }
    }
}
