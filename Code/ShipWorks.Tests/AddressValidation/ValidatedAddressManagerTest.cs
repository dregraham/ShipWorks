﻿using System.Collections.Generic;
using Interapptive.Shared.Business;
using Xunit;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.AddressValidation
{
    public class ValidatedAddressManagerTest
    {
        private OrderEntity testOrder;
        private AddressAdapter originalAddress;
        private AddressAdapter newAddress;
        private Mock<IAddressValidationDataAccess> dataAccess;
        private ShipmentEntity shipmentFromOtherOrder;
        private ShipmentEntity shipmentIsProcessed;
        private ShipmentEntity shipmentWithOtherAddress;
        private ShipmentEntity shipmentWithOrderAddress;
        private ValidatedAddressEntity addressForOtherOrder;
        private ValidatedAddressEntity addressForOrder;

        public ValidatedAddressManagerTest()
        {
            originalAddress = new AddressAdapter
            {
                Street1 = "123 Main"
            };

            newAddress = new AddressAdapter
            {
                Street1 = "456 Main"
            };

            testOrder = new OrderEntity
            {
                OrderID = 6,
                ShipStreet1 = originalAddress.Street1
            };

            shipmentWithOtherAddress = new ShipmentEntity
            {
                ShipmentID = 1,
                OrderID = testOrder.OrderID,
                ShipStreet1 = "999 Main"
            };

            shipmentWithOrderAddress = new ShipmentEntity
            {
                ShipmentID = 2,
                OrderID = testOrder.OrderID,
                ShipStreet1 = originalAddress.Street1
            };

            shipmentFromOtherOrder = new ShipmentEntity
            {
                ShipmentID = 3,
                OrderID = testOrder.OrderID + 1,
                ShipStreet1 = originalAddress.Street1
            };

            shipmentIsProcessed = new ShipmentEntity
            {
                ShipmentID = 4,
                OrderID = testOrder.OrderID,
                Processed = true,
                ShipStreet1 = originalAddress.Street1
            };

            addressForOrder = new ValidatedAddressEntity
            {
                ConsumerID = testOrder.OrderID,
                AddressPrefix = "Ship"
            };

            addressForOtherOrder = new ValidatedAddressEntity
            {
                ConsumerID = testOrder.OrderID + 1,
                AddressPrefix = "Ship"
            };
            
            dataAccess = new Mock<IAddressValidationDataAccess>();
            dataAccess.Setup(x => x.GetUnprocessedShipmentsForOrder(testOrder.OrderID))
                .Returns(new List<ShipmentEntity> { shipmentWithOrderAddress });

            dataAccess.Setup(x => x.GetValidatedAddressesByConsumerAndPrefix(testOrder.OrderID, "Ship"))
                .Returns(new List<ValidatedAddressEntity> {addressForOrder});
        }

        [Fact]
        public void PropogateAddressChangesToShipments_DoesNotQueryShipments_WhenAddressHasNotChanged()
        {
            ValidatedAddressManager.PropagateAddressChangesToShipments(dataAccess.Object, testOrder.OrderID, originalAddress, originalAddress);
            
            dataAccess.Verify(x => x.GetUnprocessedShipmentsForOrder(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void PropogateAddressChangesToShipments_UpdatesAddress_OnlyOnShipmentWithIdenticalAddress()
        {
            ValidatedAddressManager.PropagateAddressChangesToShipments(dataAccess.Object, testOrder.OrderID, originalAddress, newAddress);

            Assert.Equal(newAddress.Street1, shipmentWithOrderAddress.ShipStreet1);
            Assert.NotEqual(newAddress.Street1, shipmentWithOtherAddress.ShipStreet1);
            Assert.NotEqual(newAddress.Street1, shipmentFromOtherOrder.ShipStreet1);
            Assert.NotEqual(newAddress.Street1, shipmentIsProcessed.ShipStreet1);
        }

        [Fact]
        public void PropogateAddressChangesToShipments_UpdatesValidationProperties_OnlyOnShipmentWithIdenticalAddress()
        {
            ValidatedAddressManager.PropagateAddressChangesToShipments(dataAccess.Object, testOrder.OrderID, originalAddress, newAddress);

            Assert.Equal(newAddress.AddressValidationStatus, shipmentWithOrderAddress.ShipAddressValidationStatus);
            Assert.Equal(newAddress.AddressValidationError, shipmentWithOrderAddress.ShipAddressValidationError);
            Assert.Equal(newAddress.AddressValidationSuggestionCount, shipmentWithOrderAddress.ShipAddressValidationSuggestionCount);
        }

        [Fact]
        public void PropogateAddressChangesToShipments_CallsSave_OnlyOnShipmentWithIdenticalAddress()
        {
            ValidatedAddressManager.PropagateAddressChangesToShipments(dataAccess.Object, testOrder.OrderID, originalAddress, newAddress);

            dataAccess.Verify(x => x.SaveEntity(shipmentWithOrderAddress), Times.Once);
            dataAccess.Verify(x => x.SaveEntity(shipmentWithOtherAddress), Times.Never);
            dataAccess.Verify(x => x.SaveEntity(shipmentFromOtherOrder), Times.Never);
            dataAccess.Verify(x => x.SaveEntity(shipmentIsProcessed), Times.Never);
        }

        [Fact]
        public void PropagateAddressChangesToShipments_ClonesOrderValidatedAddresses()
        {
            List<ValidatedAddressEntity> savedAddresses = new List<ValidatedAddressEntity>();
            dataAccess.Setup(x => x.SaveEntity(It.IsAny<ValidatedAddressEntity>()))
                .Callback<IEntity2>(x => savedAddresses.Add(x as ValidatedAddressEntity));

            ValidatedAddressManager.PropagateAddressChangesToShipments(dataAccess.Object, testOrder.OrderID, originalAddress, newAddress);

            Assert.True(new AddressAdapter(addressForOrder, string.Empty) == new AddressAdapter(savedAddresses[0], string.Empty));
            Assert.Equal(shipmentWithOrderAddress.ShipmentID, savedAddresses[0].ConsumerID);
        }

        [Fact]
        public void DeleteExistingAddresses_CallsDelete_OnlyOnAddressesAssociatedWithOrder()
        {
            ValidatedAddressManager.DeleteExistingAddresses(dataAccess.Object, testOrder.OrderID, "Ship");

            dataAccess.Verify(x => x.DeleteEntity(addressForOrder), Times.Once);
            dataAccess.Verify(x => x.DeleteEntity(addressForOtherOrder), Times.Never);
        }

        [Fact]
        public void SaveValidatedOrder_CallsDelete_OnlyOnAddressesAssociatedWithOrder()
        {
            ValidatedOrderShipAddress validatedOrderAddress = new ValidatedOrderShipAddress(testOrder, null, new List<ValidatedAddressEntity>(), new AddressAdapter());
            ValidatedAddressManager.SaveValidatedOrderShipAddress(dataAccess.Object, validatedOrderAddress);

            dataAccess.Verify(x => x.DeleteEntity(addressForOrder), Times.Once);
            dataAccess.Verify(x => x.DeleteEntity(addressForOtherOrder), Times.Never);
        }

        [Fact]
        public void SaveValidatedOrder_CallsSaveOnEnteredAddress()
        {
            ValidatedAddressEntity orderAddress = new ValidatedAddressEntity { IsOriginal = true };
            List<ValidatedAddressEntity> savedAddresses = new List<ValidatedAddressEntity>();
            dataAccess.Setup(x => x.SaveEntity(It.IsAny<ValidatedAddressEntity>()))
                .Callback<IEntity2>(x => savedAddresses.Add(x as ValidatedAddressEntity));

            ValidatedOrderShipAddress validatedOrderAddress = new ValidatedOrderShipAddress(testOrder, orderAddress, new List<ValidatedAddressEntity>(), originalAddress);
            ValidatedAddressManager.SaveValidatedOrderShipAddress(dataAccess.Object, validatedOrderAddress);

            AssertSavedAddress(savedAddresses, orderAddress, true);
        }

        [Fact]
        public void SaveValidatedOrder_CallsSaveOnAddressSuggestions()
        {
            ValidatedAddressEntity orderAddress = new ValidatedAddressEntity();
            ValidatedAddressEntity suggestion1 = new ValidatedAddressEntity();
            ValidatedAddressEntity suggestion2 = new ValidatedAddressEntity();

            List<ValidatedAddressEntity> savedAddresses = new List<ValidatedAddressEntity>();
            dataAccess.Setup(x => x.SaveEntity(It.IsAny<ValidatedAddressEntity>()))
                .Callback<IEntity2>(x => savedAddresses.Add(x as ValidatedAddressEntity));


            ValidatedOrderShipAddress validatedOrderAddress = new ValidatedOrderShipAddress(testOrder, orderAddress, new List<ValidatedAddressEntity> { suggestion1, suggestion2 }, originalAddress);
            ValidatedAddressManager.SaveValidatedOrderShipAddress(dataAccess.Object, validatedOrderAddress);

            AssertSavedAddress(savedAddresses, suggestion1, false);
            AssertSavedAddress(savedAddresses, suggestion2, false);
        }

        [Fact]
        public void SaveValidatedOrder_SavesOrder()
        {
            ValidatedOrderShipAddress validatedOrderAddress = new ValidatedOrderShipAddress(testOrder, null, new List<ValidatedAddressEntity>(), originalAddress);
            ValidatedAddressManager.SaveValidatedOrderShipAddress(dataAccess.Object, validatedOrderAddress);

            dataAccess.Verify(x => x.SaveEntity(testOrder));
        }

        [Fact]
        public void SaveValidatedOrder_PropagatesAddressChanges_OnlyOnShipmentWithIdenticalAddress()
        {
            testOrder.ShipStreet1 = "999 Main";

            ValidatedOrderShipAddress validatedOrderAddress = new ValidatedOrderShipAddress(testOrder, null, new List<ValidatedAddressEntity>(), new AddressAdapter { Street1 = shipmentWithOrderAddress.ShipStreet1 });
            ValidatedAddressManager.SaveValidatedOrderShipAddress(dataAccess.Object, validatedOrderAddress);
            
            dataAccess.Verify(x => x.SaveEntity(shipmentWithOrderAddress), Times.Once);
            dataAccess.Verify(x => x.SaveEntity(shipmentWithOtherAddress), Times.Never);
            dataAccess.Verify(x => x.SaveEntity(shipmentFromOtherOrder), Times.Never);
            dataAccess.Verify(x => x.SaveEntity(shipmentIsProcessed), Times.Never);
        }

        [Fact]
        public void SaveOrderAddress_DoesNotCallSave_WhenAddressIsNull()
        {
            ValidatedAddressManager.SaveEntityAddress(dataAccess.Object, testOrder.OrderID, null);
            dataAccess.Verify(x => x.SaveEntity(It.IsAny<ValidatedAddressEntity>()), Times.Never);
        }

        [Fact]
        public void SaveOrderAddress_CallsSave_WhenAddressIsNotNull()
        {
            ValidatedAddressEntity testAddress = new ValidatedAddressEntity();
            ValidatedAddressEntity savedEntity = null;

            dataAccess.Setup(x => x.SaveEntity(It.IsAny<ValidatedAddressEntity>()))
                .Callback<IEntity2>(x => savedEntity = x as ValidatedAddressEntity);
            ValidatedAddressManager.SaveEntityAddress(dataAccess.Object, testOrder.OrderID, testAddress);
            
            Assert.Equal(testOrder.OrderID, savedEntity.ConsumerID);
            Assert.Equal(testAddress, savedEntity);
        }

        private void AssertSavedAddress(IEnumerable<ValidatedAddressEntity> savedAddresses, ValidatedAddressEntity orderAddress, bool isOriginalExpected)
        {
            bool wasFound = false;

            foreach (var validatedAddress in savedAddresses)
            {
                if (validatedAddress == orderAddress)
                {
                    Assert.Equal(testOrder.OrderID, validatedAddress.ConsumerID);
                    Assert.Equal(isOriginalExpected, validatedAddress.IsOriginal);
                    wasFound = true;
                }
            }

            Assert.True(wasFound);
        }
    }
}
