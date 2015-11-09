using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using Xunit;
using System.Windows.Forms;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon
{
    [Trait("Carrier", "Amazon")]
    public class AmazonServiceViewModelTest
    {
        [Fact]
        public void Load_WithEmptyShipmentList_DoesNotThrow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                testObject.Load(Enumerable.Empty<ShipmentEntity>().ToList());
            }
        }

        [Fact]
        public void Load_WithNullShipmentList_ThrowsArgumentNullException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                Assert.Throws<ArgumentNullException>(() => testObject.Load(null));
            }
        }

        #region DeliveryExperience

        [Fact]
        public void DeliveryConfirmationIsMultiValued_WithDistinctValues_ReturnsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithoutSignature}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithoutSignature}}
                };

                testObject.Load(shipments);

                Assert.False(testObject.DeliveryExperience.IsMultiValued);
            }
        }

        [Fact]
        public void DeliveryConfirmationIsMultiValued_WithDifferentValues_ReturnsTrue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithAdultSignature}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature}}
                };
                
                testObject.Load(shipments);

                Assert.True(testObject.DeliveryExperience.IsMultiValued);
            }
        }

        [Fact]
        public void DeliveryConfirmation_WithAllValuesDeliveryConfirmationWithSignature_ReturnsDeliveryConfirmationWithSignature()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature}}
                };

                testObject.Load(shipments);

                Assert.Equal(AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature, testObject.DeliveryExperience.PropertyValue);
            }
        }

        [Fact]
        public void DeliveryConfirmation_WithMixedValues_ReturnsNoTracking()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithAdultSignature}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature}}
                };

                testObject.Load(shipments);

                Assert.Equal(null, testObject.DeliveryExperience.PropertyValue);
            }
        }

        [Fact]
        public void DeliveryConfirmation_WithEmptyShipmentList_ReturnsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                testObject.Load(Enumerable.Empty<ShipmentEntity>().ToList());

                Assert.Equal(null, testObject.DeliveryExperience.PropertyValue);
            }
        }

        [Fact]
        public void DeliveryConfirmationSaves_DeliveryConfirmationWithSignature_WithMixedValues()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithAdultSignature}}
                };

                testObject.Load(shipments);

                testObject.DeliveryExperience.PropertyValue = AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature;
                testObject.Save(shipments);

                Assert.Equal(AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature, testObject.DeliveryExperience.PropertyValue);

                shipments.ForEach(s => Assert.Equal((int)AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature, s.Amazon.DeliveryExperience));
            }
        }

#endregion
        
        #region ContentWeight

        [Fact]
        public void ContentWeightIsMultiValued_WithDistinctValues_ReturnsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() { ContentWeight = 1.1, Amazon = new AmazonShipmentEntity()},
                    new ShipmentEntity() { ContentWeight = 1.1, Amazon = new AmazonShipmentEntity()},
                };

                testObject.Load(shipments);

                Assert.False(testObject.ContentWeightIsMultiValued);
            }
        }

        [Fact]
        public void ContentWeightIsMultiValued_WithDifferentValues_ReturnsTrue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() { ContentWeight = 1.1, Amazon = new AmazonShipmentEntity()},
                    new ShipmentEntity() { ContentWeight = 20.2, Amazon = new AmazonShipmentEntity()},
                };


                testObject.Load(shipments);

                Assert.True(testObject.ContentWeightIsMultiValued);
            }
        }

        [Fact]
        public void ContentWeight_WithAllSameValues_ReturnsSameValue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() { ContentWeight = 1.1, Amazon = new AmazonShipmentEntity()},
                    new ShipmentEntity() { ContentWeight = 1.1, Amazon = new AmazonShipmentEntity()},
                };

                testObject.Load(shipments);

                Assert.Equal(1.1, testObject.ContentWeight);
            }
        }

        [Fact]
        public void ContentWeight_WithMixedValues_ReturnsDefault()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() { ContentWeight = 1.1, Amazon = new AmazonShipmentEntity()},
                    new ShipmentEntity() { ContentWeight = 20.2, Amazon = new AmazonShipmentEntity()},
                };

                testObject.Load(shipments);

                Assert.Equal(0, testObject.ContentWeight);
            }
        }

        [Fact]
        public void ContentWeight_WithEmptyShipmentList_ReturnsDefault()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                testObject.Load(Enumerable.Empty<ShipmentEntity>().ToList());

                Assert.Equal(0, testObject.ContentWeight);
            }
        }

        [Fact]
        public void ContentWeightSaves_WithMixedValues()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() { ContentWeight = 1.1, Amazon = new AmazonShipmentEntity()},
                    new ShipmentEntity() { ContentWeight = 20.2, Amazon = new AmazonShipmentEntity()},
                };

                testObject.Load(shipments);

                testObject.ContentWeight = 3.33;
                testObject.Save(shipments);

                Assert.Equal(3.33, testObject.ContentWeight);

                shipments.ForEach(s => Assert.Equal(3.33, s.ContentWeight));
            }
        }

        #endregion
    }
}
