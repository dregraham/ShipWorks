using System;
using System.Collections.Generic;
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

        #region CarrierWillPickup
        [Fact]
        public void CarrierWillPickUpIsMultiValued_WithDistinctValues_ReturnsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = true}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = true}}
                };

                testObject.Load(shipments);

                Assert.False(testObject.CarrierWillPickUpIsMultiValued);
            }
        }

        [Fact]
        public void CarrierWillPickUpIsMultiValued_WithDifferentValues_ReturnsTrue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = true}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = false}}
                };


                testObject.Load(shipments);

                Assert.True(testObject.CarrierWillPickUpIsMultiValued);
            }
        }

        [Fact]
        public void CarrierWillPickUp_WithAllValuesTrue_ReturnsTrue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = true}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = true}}
                };

                testObject.Load(shipments);

                Assert.True(testObject.CarrierWillPickUp);
            }
        }

        [Fact]
        public void CarrierWillPickUp_WithAllValuesFalse_ReturnsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = false}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = false}}
                };

                testObject.Load(shipments);

                Assert.False(testObject.CarrierWillPickUp);
            }
        }

        [Fact]
        public void CarrierWillPickUp_WithMixedValues_ReturnsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = true}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = false}}
                };

                testObject.Load(shipments);

                Assert.False(testObject.CarrierWillPickUp);
            }
        }

        [Fact]
        public void CarrierWillPickUp_WithEmptyShipmentList_ReturnsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                testObject.Load(Enumerable.Empty<ShipmentEntity>().ToList());

                Assert.False(testObject.CarrierWillPickUp);
            }
        }

        [Fact]
        public void CarrierWillPickUpSaves_True_WithMixedValues()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = true}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = false}}
                };

                testObject.Load(shipments);

                testObject.CarrierWillPickUp = true;
                testObject.Save(shipments);

                Assert.True(testObject.CarrierWillPickUp);

                shipments.ForEach(s => Assert.True(s.Amazon.CarrierWillPickUp));
            }
        }

        [Fact]
        public void CarrierWillPickUpSaves_False_WithMixedValues()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = true}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = false}}
                };

                testObject.Load(shipments);

                testObject.CarrierWillPickUp = false;
                testObject.Save(shipments);

                Assert.False(testObject.CarrierWillPickUp);

                shipments.ForEach(s => Assert.False(s.Amazon.CarrierWillPickUp));
            }
        }

        [Fact]
        public void CarrierWillPickUpCheckState_IsIndeterminate_WithMixedValues()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = true}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = false}}
                };

                testObject.Load(shipments);

                Assert.Equal(CheckState.Indeterminate, testObject.CarrierWillPickUpCheckState);
            }
        }

        [Fact]
        public void CarrierWillPickUpCheckState_IsChecked_WithOnlyTrueValues()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = true}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = true}}
                };

                testObject.Load(shipments);

                Assert.Equal(CheckState.Checked, testObject.CarrierWillPickUpCheckState);
            }
        }

        [Fact]
        public void CarrierWillPickUpCheckState_IsUnChecked_WithOnlyFalseValues()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = false}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {CarrierWillPickUp = false}}
                };

                testObject.Load(shipments);

                Assert.Equal(CheckState.Unchecked, testObject.CarrierWillPickUpCheckState);
            }
        }

#endregion

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

                Assert.False(testObject.DeliveryConfirmationIsMultiValued);
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
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.NoTracking}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithAdultSignature}}
                };


                testObject.Load(shipments);

                Assert.True(testObject.DeliveryConfirmationIsMultiValued);
            }
        }

        [Fact]
        public void DeliveryConfirmation_WithAllValuesNoTracking_ReturnsNoTracking()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.NoTracking}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.NoTracking}}
                };

                testObject.Load(shipments);

                Assert.Equal(AmazonDeliveryExperienceType.NoTracking, testObject.DeliveryExperience);
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

                Assert.Equal(AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature, testObject.DeliveryExperience);
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
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.NoTracking}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithAdultSignature}}
                };

                testObject.Load(shipments);

                Assert.Equal(AmazonDeliveryExperienceType.NoTracking, testObject.DeliveryExperience);
            }
        }

        [Fact]
        public void DeliveryConfirmation_WithEmptyShipmentList_ReturnsDeliveryConfirmation()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                testObject.Load(Enumerable.Empty<ShipmentEntity>().ToList());

                Assert.Equal(AmazonDeliveryExperienceType.NoTracking, testObject.DeliveryExperience);
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
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.NoTracking}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithAdultSignature}}
                };

                testObject.Load(shipments);

                testObject.DeliveryExperience = AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature;
                testObject.Save(shipments);

                Assert.Equal(AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature, testObject.DeliveryExperience);

                shipments.ForEach(s => Assert.Equal((int)AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature, s.Amazon.DeliveryExperience));
            }
        }

        [Fact]
        public void DeliveryConfirmationSaves_NoTracking_WithMixedValues()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.NoTracking}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DeliveryExperience = (int)AmazonDeliveryExperienceType.DeliveryConfirmationWithAdultSignature}}
                };

                testObject.Load(shipments);

                testObject.DeliveryExperience = AmazonDeliveryExperienceType.NoTracking;
                testObject.Save(shipments);

                Assert.Equal(AmazonDeliveryExperienceType.NoTracking, testObject.DeliveryExperience);

                shipments.ForEach(s => Assert.Equal((int)AmazonDeliveryExperienceType.NoTracking, s.Amazon.DeliveryExperience));
            }
        }

#endregion

        #region DateMustArriveBy

        [Fact]
        public void MustArriveByDateIsMultiValued_WithDistinctValues_ReturnsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DateMustArriveBy = new DateTime(2000, 1, 1)}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DateMustArriveBy = new DateTime(2000, 1, 1)}}
                };

                testObject.Load(shipments);

                Assert.False(testObject.DateMustArriveByIsMultiValued);
            }
        }

        [Fact]
        public void MustArriveByDateIsMultiValued_WithDifferentValues_ReturnsTrue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DateMustArriveBy = new DateTime(2000, 1, 1)}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DateMustArriveBy = new DateTime(2100, 12, 12)}}
                };


                testObject.Load(shipments);

                Assert.True(testObject.DateMustArriveByIsMultiValued);
            }
        }

        [Fact]
        public void MustArriveByDate_WithAllSameValues_ReturnsSameValue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DateMustArriveBy = new DateTime(2000, 1, 1)}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DateMustArriveBy = new DateTime(2000, 1, 1)}}
                };

                testObject.Load(shipments);

                Assert.Equal(new DateTime(2000, 1, 1), testObject.DateMustArriveBy);
            }
        }

        [Fact]
        public void MustArriveByDate_WithMixedValues_ReturnsDefault()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DateMustArriveBy = new DateTime(2000, 1, 1)}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DateMustArriveBy = new DateTime(2100, 12, 12)}}
                };

                testObject.Load(shipments);

                Assert.Equal(DateTime.Now.AddDays(1), testObject.DateMustArriveBy);
            }
        }

        [Fact]
        public void MustArriveByDate_WithEmptyShipmentList_ReturnsDefault()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                testObject.Load(Enumerable.Empty<ShipmentEntity>().ToList());

                Assert.Equal(DateTime.Now.AddDays(1), testObject.DateMustArriveBy);
            }
        }

        [Fact]
        public void MustArriveByDateSaves_WithMixedValues()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                List<ShipmentEntity> shipments = new List<ShipmentEntity>()
                {
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DateMustArriveBy = new DateTime(2000, 1, 1)}},
                    new ShipmentEntity() {Amazon = new AmazonShipmentEntity() {DateMustArriveBy = new DateTime(2100, 12, 12)}}
                };

                testObject.Load(shipments);

                testObject.DateMustArriveBy = new DateTime(2015, 8, 21);
                testObject.Save(shipments);

                Assert.Equal(new DateTime(2015, 8, 21), testObject.DateMustArriveBy);

                shipments.ForEach(s => Assert.Equal((new DateTime(2015, 8, 21)).Date.AddHours(12), s.Amazon.DateMustArriveBy));
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
