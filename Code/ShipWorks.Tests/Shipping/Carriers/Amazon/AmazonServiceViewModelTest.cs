using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon
{
    [TestClass]
    public class AmazonServiceViewModelTest
    {
        [TestMethod]
        public void Load_WithEmptyShipmentList_DoesNotThrow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                testObject.Load(Enumerable.Empty<ShipmentEntity>().ToList());
            }
        }

        [TestMethod]
        public void Load_WithNullShipmentList_ThrowsArgumentNullException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                try
                {
                    testObject.Load(null);
                    Assert.Fail();
                }
                catch (ArgumentNullException)
                {
                    // Success
                }
            }
        }

        #region CarrierWillPickup
        [TestMethod]
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

                Assert.IsFalse(testObject.CarrierWillPickUpIsMultiValued);
            }
        }

        [TestMethod]
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

                Assert.IsTrue(testObject.CarrierWillPickUpIsMultiValued);
            }
        }

        [TestMethod]
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

                Assert.IsTrue(testObject.CarrierWillPickUp);
            }
        }

        [TestMethod]
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

                Assert.IsFalse(testObject.CarrierWillPickUp);
            }
        }

        [TestMethod]
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

                Assert.IsFalse(testObject.CarrierWillPickUp);
            }
        }

        [TestMethod]
        public void CarrierWillPickUp_WithEmptyShipmentList_ReturnsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                testObject.Load(Enumerable.Empty<ShipmentEntity>().ToList());

                Assert.IsFalse(testObject.CarrierWillPickUp);
            }
        }

        [TestMethod]
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

                Assert.IsTrue(testObject.CarrierWillPickUp);

                shipments.ForEach(s => Assert.IsTrue(s.Amazon.CarrierWillPickUp));
            }
        }

        [TestMethod]
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

                Assert.IsFalse(testObject.CarrierWillPickUp);

                shipments.ForEach(s => Assert.IsFalse(s.Amazon.CarrierWillPickUp));
            }
        }

        [TestMethod]
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

                Assert.AreEqual(CheckState.Indeterminate, testObject.CarrierWillPickUpCheckState);
            }
        }

        [TestMethod]
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

                Assert.AreEqual(CheckState.Checked, testObject.CarrierWillPickUpCheckState);
            }
        }

        [TestMethod]
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

                Assert.AreEqual(CheckState.Unchecked, testObject.CarrierWillPickUpCheckState);
            }
        }

#endregion

        #region DeliveryConfirmation

        [TestMethod]
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

                Assert.IsFalse(testObject.DeliveryConfirmationIsMultiValued);
            }
        }

        [TestMethod]
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

                Assert.IsTrue(testObject.DeliveryConfirmationIsMultiValued);
            }
        }

        [TestMethod]
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

                Assert.AreEqual(AmazonDeliveryExperienceType.NoTracking, testObject.DeliveryConfirmation);
            }
        }

        [TestMethod]
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

                Assert.AreEqual(AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature, testObject.DeliveryConfirmation);
            }
        }

        [TestMethod]
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

                Assert.AreEqual(AmazonDeliveryExperienceType.NoTracking, testObject.DeliveryConfirmation);
            }
        }

        [TestMethod]
        public void DeliveryConfirmation_WithEmptyShipmentList_ReturnsDeliveryConfirmation()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                testObject.Load(Enumerable.Empty<ShipmentEntity>().ToList());

                Assert.AreEqual(AmazonDeliveryExperienceType.NoTracking, testObject.DeliveryConfirmation);
            }
        }

        [TestMethod]
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

                testObject.DeliveryConfirmation = AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature;
                testObject.Save(shipments);

                Assert.AreEqual(AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature, testObject.DeliveryConfirmation);

                shipments.ForEach(s => Assert.AreEqual((int)AmazonDeliveryExperienceType.DeliveryConfirmationWithSignature, s.Amazon.DeliveryExperience));
            }
        }

        [TestMethod]
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

                testObject.DeliveryConfirmation = AmazonDeliveryExperienceType.NoTracking;
                testObject.Save(shipments);

                Assert.AreEqual(AmazonDeliveryExperienceType.NoTracking, testObject.DeliveryConfirmation);

                shipments.ForEach(s => Assert.AreEqual((int)AmazonDeliveryExperienceType.NoTracking, s.Amazon.DeliveryExperience));
            }
        }

#endregion

        #region MustArriveByDate

        [TestMethod]
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

                Assert.IsFalse(testObject.MustArriveByDateIsMultiValued);
            }
        }

        [TestMethod]
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

                Assert.IsTrue(testObject.MustArriveByDateIsMultiValued);
            }
        }

        [TestMethod]
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

                Assert.AreEqual(new DateTime(2000, 1, 1), testObject.MustArriveByDate);
            }
        }

        [TestMethod]
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

                Assert.AreEqual(DateTime.Now.AddDays(1), testObject.MustArriveByDate);
            }
        }

        [TestMethod]
        public void MustArriveByDate_WithEmptyShipmentList_ReturnsDefault()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                testObject.Load(Enumerable.Empty<ShipmentEntity>().ToList());

                Assert.AreEqual(DateTime.Now.AddDays(1), testObject.MustArriveByDate);
            }
        }

        [TestMethod]
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

                testObject.MustArriveByDate = new DateTime(2015, 8, 21);
                testObject.Save(shipments);

                Assert.AreEqual(new DateTime(2015, 8, 21), testObject.MustArriveByDate);

                shipments.ForEach(s => Assert.AreEqual((new DateTime(2015, 8, 21)).Date.AddHours(12), s.Amazon.DateMustArriveBy));
            }
        }

        #endregion

        #region ContentWeight

        [TestMethod]
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

                Assert.IsFalse(testObject.ContentWeightIsMultiValued);
            }
        }

        [TestMethod]
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

                Assert.IsTrue(testObject.ContentWeightIsMultiValued);
            }
        }

        [TestMethod]
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

                Assert.AreEqual(1.1, testObject.ContentWeight);
            }
        }

        [TestMethod]
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

                Assert.AreEqual(0, testObject.ContentWeight);
            }
        }

        [TestMethod]
        public void ContentWeight_WithEmptyShipmentList_ReturnsDefault()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonServiceViewModel testObject = mock.Create<AmazonServiceViewModel>();

                testObject.Load(Enumerable.Empty<ShipmentEntity>().ToList());

                Assert.AreEqual(0, testObject.ContentWeight);
            }
        }

        [TestMethod]
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

                Assert.AreEqual(3.33, testObject.ContentWeight);

                shipments.ForEach(s => Assert.AreEqual(3.33, s.ContentWeight));
            }
        }

        #endregion
    }
}
