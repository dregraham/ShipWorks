using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using Moq;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Requests.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests.System;
using ShipWorks.Stores.Platforms.Ebay.Requests.Download;
using ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment;

namespace ShipWorks.Tests.Stores.eBay
{
    /// <summary>
    /// Summary description for EbayWebClientTest
    /// </summary>
    [TestClass]
    public class EbayWebClientTest
    {
        EbayWebClient testObject;
        
        Mock<IRequestFactory> mockedRequestFactory;        
        List<TransactionType> validTransactionsToCombine; 

        [TestInitialize]
        public void Initialize()
        {
            mockedRequestFactory = new Mock<IRequestFactory>();
            testObject = new EbayWebClient(mockedRequestFactory.Object);

            validTransactionsToCombine = new List<TransactionType>()
            {
                new TransactionType(),
                new TransactionType()
            };
        }

        [TestMethod]
        public void GetTangoAuthorization_DelegatesToRequestFactory_Test()
        {
            // Create a mocked tango request and configure the factory to create our mocked tango request
            Mock<ITangoAuthorizationRequest> tangoRequest = new Mock<ITangoAuthorizationRequest>();            
            mockedRequestFactory.Setup(f => f.CreateTangoAuthorizationRequest(It.IsAny<string>())).Returns(tangoRequest.Object);

            
            testObject.GetTangoAuthorization();

            // Verify the method to create the tango request was called exactly once
            mockedRequestFactory.Verify(f => f.CreateTangoAuthorizationRequest(It.IsAny<string>()), Times.Once());
        }
        
        [TestMethod]
        public void GetTangoAuthorization_DelegatesToTangoAuthorizationRequest_Test()
        {
            // We're just testing that work is delegated to the the tango request (we don't care about what it returns) 
            // so create a mocked tango request and configure it to return an XML document 
            Mock<ITangoAuthorizationRequest> tangoRequest = new Mock<ITangoAuthorizationRequest>();
            tangoRequest.Setup(t => t.Authorize()).Returns(new System.Xml.XmlDocument());

            // Setup the factory to create our mocked tango request
            mockedRequestFactory.Setup(f => f.CreateTangoAuthorizationRequest(It.IsAny<string>())).Returns(tangoRequest.Object);

            
            testObject.GetTangoAuthorization();

            // Verify the Authorize method was called
            tangoRequest.Verify(t => t.Authorize(), Times.Once());
        }


        [TestMethod]
        public void GetUserInfo_DelegatesToRequestFactory_Test()
        {
            // Setup our mocked request factory to just return a mocked IUserInfoRequest
            mockedRequestFactory.Setup(f => f.CreateUserInfoRequest(It.IsAny<TokenData>())).Returns(new Mock<IUserInfoRequest>().Object);

            // Use the mocked request factory to test our object
            
            testObject.GetUserInfo("my token");

            // Verify that the create method was called exactly once
            mockedRequestFactory.Verify(f => f.CreateUserInfoRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetUserInfo_DelegatesToUserInfoRequest_Test()
        {
            // Setup a mocked user info request so we can verify it was called
            Mock<IUserInfoRequest> mockedRequest = new Mock<IUserInfoRequest>();
            mockedRequest.Setup(r => r.GetUserInfo()).Returns(new GetUserResponseType());

            // Setup the factory to create our mocked request
            mockedRequestFactory.Setup(f => f.CreateUserInfoRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Use the mocked request factory to test our object
            
            testObject.GetUserInfo("my token");

            mockedRequest.Verify(r => r.GetUserInfo(), Times.Once());
        }


        [TestMethod]
        public void GetServerTimeInUtc_DelegatesToRequestFactory_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateTimeRequest(It.IsAny<TokenData>())).Returns(new Mock<ITimeRequest>().Object);

            
            testObject.GetServerTimeInUtc("my token");

            mockedRequestFactory.Verify(f => f.CreateTimeRequest(It.IsAny<TokenData>()), Times.Once());
        }
        
        [TestMethod]
        public void GetServerTimeInUtc_DelegatesToTimeRequest_Test()
        {
            Mock<ITimeRequest> mockedRequest = new Mock<ITimeRequest>();
            mockedRequest.Setup(r => r.GetServerTimeInUtc()).Returns(DateTime.Now);

            mockedRequestFactory.Setup(f => f.CreateTimeRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetServerTimeInUtc("my token");

            mockedRequest.Verify(r => r.GetServerTimeInUtc(), Times.Once());
        }

        #region Get Transaction Summary Tests

        [TestMethod]
        public void GetTransactionSummary_DelegatesToRequestFactory_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateTransactionRequest(It.IsAny<TokenData>())).Returns(new Mock<ITransactionRequest>().Object);

            
            testObject.GetTransactionSummary("my token", DateTime.Now, DateTime.Now.AddDays(1));

            mockedRequestFactory.Verify(f => f.CreateTransactionRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetTransactionSummary_DelegatesToTransactionRequest_Test()
        {
            // Arrange
            Mock<ITransactionRequest> mockedRequest = new Mock<ITransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactionSummary(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            testObject.GetTransactionSummary("my token", DateTime.Now, DateTime.Now.AddDays(1));

            // Assert - Check that the transaction summary method was invoked once
            mockedRequest.Verify(r => r.GetTransactionSummary(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetTransactionSummary_ThrowsEbayException_WhenStartDateIsLaterThanEndDate_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateTransactionRequest(It.IsAny<TokenData>())).Returns(new Mock<ITransactionRequest>().Object);

            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(-1);

            
            testObject.GetTransactionSummary("my token", startDate, endDate);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetTransactionSummary_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateTransactionRequest(It.IsAny<TokenData>())).Returns(new Mock<ITransactionRequest>().Object); 
            
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);

            
            testObject.GetTransactionSummary(string.Empty, startDate, endDate);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetTransactionSummary_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateTransactionRequest(It.IsAny<TokenData>())).Returns(new Mock<ITransactionRequest>().Object);

            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);

            
            testObject.GetTransactionSummary(null, startDate, endDate);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetTransactionSummary_ThrowsEbayException_WhenDateRangeExceedsThirtyDays_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateTransactionRequest(It.IsAny<TokenData>())).Returns(new Mock<ITransactionRequest>().Object);

            
            testObject.GetTransactionSummary("my token", DateTime.Now.AddDays(-31), DateTime.Now);
        }

        #endregion Get Transaction Count Tests


        #region Download Transactions Tests

        [TestMethod]
        public void DownloadTransactions_DelegatesToRequestFactory_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateTransactionRequest(It.IsAny<TokenData>())).Returns(new Mock<ITransactionRequest>().Object);

            
            testObject.DownloadTransactions("my token", DateTime.Now, DateTime.Now.AddDays(1), 1);

            mockedRequestFactory.Verify(f => f.CreateTransactionRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void DownloadTransactions_DelegatesToTransactionRequest_Test()
        {
            // Arrange
            Mock<ITransactionRequest> mockedRequest = new Mock<ITransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactions(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new GetSellerTransactionsResponseType());

            mockedRequestFactory.Setup(f => f.CreateTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            testObject.DownloadTransactions("my token", DateTime.Now, DateTime.Now.AddDays(1), 1);

            // Assert 
            mockedRequest.Verify(r => r.GetTransactions(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void DownloadTransactions_ThrowsEbayException_WhenDateRangeExceedsThirtyDays_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateTransactionRequest(It.IsAny<TokenData>())).Returns(new Mock<ITransactionRequest>().Object);

            
            testObject.DownloadTransactions("my token", DateTime.Now.AddDays(-31), DateTime.Now, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void DownloadTransactions_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);

            mockedRequestFactory.Setup(f => f.CreateTransactionRequest(It.IsAny<TokenData>())).Returns(new Mock<ITransactionRequest>().Object);

            
            testObject.DownloadTransactions(string.Empty, startDate, endDate, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void DownloadTransactions_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);

            mockedRequestFactory.Setup(f => f.CreateTransactionRequest(It.IsAny<TokenData>())).Returns(new Mock<ITransactionRequest>().Object);

            
            testObject.DownloadTransactions(null, startDate, endDate, 1);
        }

        #endregion Download Transactions Tests


        #region Combined Payments Tests

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetPaymentSummary_ThrowsException_WhenDateRangeExceedsThirtyDays_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);

            
            testObject.GetPaymentSummary("my token", OrderStatusCodeType.All, DateTime.Now.AddDays(-31), DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetPaymentSummary_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);

            
            testObject.GetPaymentSummary(string.Empty, OrderStatusCodeType.All, startDate, endDate);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetPaymentSummary_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);

            
            testObject.GetPaymentSummary(null, OrderStatusCodeType.All, startDate, endDate);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetPaymentSummary_WithoutSpecificStatus_ThrowsException_WhenDateRangeExceedsThirtyDays_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);


            testObject.GetPaymentSummary("my token", DateTime.Now.AddDays(-31), DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetPaymentSummar_WithoutSpecificStatusy_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);


            testObject.GetPaymentSummary(string.Empty, startDate, endDate);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetPaymentSummary_WithoutSpecificStatus_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);


            testObject.GetPaymentSummary(null, startDate, endDate);
        }

        [TestMethod]
        public void GetPaymentSummary_DelegatesToRequestFactory_Test()
        {
            // Arrange
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);

            // Act
            
            testObject.GetPaymentSummary("my token", OrderStatusCodeType.All, DateTime.Now, DateTime.Now.AddDays(1));

            // Assert
            mockedRequestFactory.Verify(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetPaymentSummary_DelegatesToCombinedPaymentRequest_Test()
        {
            // Arrange
            Mock<ICombinedPaymentRequest> mockedRequest = new Mock<ICombinedPaymentRequest>();
            mockedRequest.Setup(r => r.GetPaymentSummary(It.IsAny<OrderStatusCodeType>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            testObject.GetPaymentSummary("my token", OrderStatusCodeType.All, DateTime.Now, DateTime.Now.AddDays(1));

            // Assert
            mockedRequest.Verify(r => r.GetPaymentSummary(It.IsAny<OrderStatusCodeType>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }
        
        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetActivePayments_ThrowsException_WhenDateRangeExceedsThirtyDays_Test()
        {
            // Arrange
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);

            // Act
            
            testObject.GetActivePayments("my token", DateTime.Now.AddDays(-31), DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetActivePayments_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            // Arrange
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);            
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);

            // Act
            
            testObject.GetActivePayments(string.Empty, startDate, endDate);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetActivePayments_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            // Arrange
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);

            // Act
            
            testObject.GetActivePayments(null, startDate, endDate);
        }

        [TestMethod]
        public void GetActivePayments_DelegatesToRequestFactory_Test()
        {
            // Arrange - need to define our mocked request to return a GetOrdersResponseType with an order array
            Mock<ICombinedPaymentRequest> mockedRequest = new Mock<ICombinedPaymentRequest>();
            mockedRequest.Setup(r => r.GetActivePayments(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new GetOrdersResponseType { OrderArray = new OrderType[] { } });

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            testObject.GetActivePayments("my token", DateTime.Now, DateTime.Now.AddDays(1));

            // Assert
            mockedRequestFactory.Verify(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetActivePayments_DelegatesToCombinedPaymentRequest_Test()
        {
            // Arrange - need to define our mocked request to return a GetOrdersResponseType with an order array
            Mock<ICombinedPaymentRequest> mockedRequest = new Mock<ICombinedPaymentRequest>();
            mockedRequest.Setup(r => r.GetActivePayments(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new GetOrdersResponseType { OrderArray = new OrderType[] { } });

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            testObject.GetActivePayments("my token", DateTime.Now, DateTime.Now.AddDays(1));

            // Assert
            mockedRequest.Verify(r => r.GetActivePayments(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetActivePayments_ReturnsEmptyOrderList_WhenOrderArrayIsNull_Test()
        {
            // Arrange - Setup our mocked request factory to use an ICombinedPaymentRequest that returns a response 
            // containing a null order array
            Mock<ICombinedPaymentRequest> mockedRequest = new Mock<ICombinedPaymentRequest>();
            mockedRequest.Setup(r => r.GetActivePayments(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new GetOrdersResponseType { OrderArray = null });

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            List<OrderType> orders = testObject.GetActivePayments("my token", DateTime.Now, DateTime.Now.AddDays(1));
            
            // Assert
            Assert.IsNotNull(orders);
            Assert.AreEqual(0, orders.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetCompletedPayments_ThrowsException_WhenDateRangeExceedsThirtyDays_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);

            
            testObject.GetCompletedPayments("my token", DateTime.Now.AddDays(-31), DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetCompletedPayments_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);

            
            testObject.GetCompletedPayments(string.Empty, startDate, endDate);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetCompletedPayments_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);

            
            testObject.GetCompletedPayments(null, startDate, endDate);
        }

        [TestMethod]
        public void GetCompletedPayments_DelegatesToRequestFactory_Test()
        {
            // Arrange
            Mock<ICombinedPaymentRequest> mockedRequest = new Mock<ICombinedPaymentRequest>();
            mockedRequest.Setup(r => r.GetCompletedPayments(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new GetOrdersResponseType { OrderArray = null });

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            testObject.GetCompletedPayments("my token", DateTime.Now, DateTime.Now.AddDays(1));

            // Assert
            mockedRequestFactory.Verify(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetCompletedPayments_DelegatesToCombinedPaymentRequest_Test()
        {
            // Arrange
            Mock<ICombinedPaymentRequest> mockedRequest = new Mock<ICombinedPaymentRequest>();
            mockedRequest.Setup(r => r.GetCompletedPayments(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new GetOrdersResponseType { OrderArray = null });

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            testObject.GetCompletedPayments("my token", DateTime.Now, DateTime.Now.AddDays(1));

            // Assert
            mockedRequest.Verify(r => r.GetCompletedPayments(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetCompletedPayments_ReturnsEmptyOrderList_WhenOrderArrayIsNull_Test()
        {
            // Setup our request factory to use an ICombinedPaymentRequest that returns a response 
            // containing a null order array
            Mock<ICombinedPaymentRequest> mockedRequest = new Mock<ICombinedPaymentRequest>();
            mockedRequest.Setup(r => r.GetCompletedPayments(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new GetOrdersResponseType { OrderArray = null });

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            List<OrderType> orders = testObject.GetCompletedPayments("my token", DateTime.Now, DateTime.Now.AddDays(1));

            Assert.IsNotNull(orders);
            Assert.AreEqual(0, orders.Count);
        }




        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetAllPayments_ThrowsException_WhenDateRangeExceedsThirtyDays_Test()
        {
            Mock<ICombinedPaymentRequest> mockedRequest = new Mock<ICombinedPaymentRequest>();
            mockedRequest.Setup(r => r.GetAllPayments(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new GetOrdersResponseType { OrderArray = null });

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetAllPayments("my token", DateTime.Now.AddDays(-31), DateTime.Now, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetAllPayments_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);
            
            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);


            testObject.GetAllPayments(string.Empty, startDate, endDate, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetAllPayments_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddMinutes(1);

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(new Mock<ICombinedPaymentRequest>().Object);


            testObject.GetAllPayments(null, startDate, endDate, 1);
        }

        [TestMethod]
        public void GetAllPayments_DelegatesToRequestFactory_Test()
        {
            Mock<ICombinedPaymentRequest> mockedRequest = new Mock<ICombinedPaymentRequest>();
            mockedRequest.Setup(r => r.GetAllPayments(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new GetOrdersResponseType { OrderArray = null });

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.GetAllPayments("my token", DateTime.Now, DateTime.Now.AddDays(1), 1);

            mockedRequestFactory.Verify(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetAllPayments_DelegatesToCombinedPaymentRequest_Test()
        {
            Mock<ICombinedPaymentRequest> mockedRequest = new Mock<ICombinedPaymentRequest>();
            mockedRequest.Setup(r => r.GetAllPayments(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new GetOrdersResponseType { OrderArray = null });

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetAllPayments("my token", DateTime.Now, DateTime.Now.AddDays(1), 1);

            mockedRequest.Verify(r => r.GetAllPayments(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetAllPayments_ReturnsEmptyOrderList_WhenOrderArrayIsNull_Test()
        {
            // Setup our request factory to use an ICombinedPaymentRequest that returns a response containing a null order array
            Mock<ICombinedPaymentRequest> mockedRequest = new Mock<ICombinedPaymentRequest>();
            mockedRequest.Setup(r => r.GetAllPayments(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new GetOrdersResponseType { OrderArray = null });

            mockedRequestFactory.Setup(f => f.CreateCombinedPaymentRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            List<OrderType> orders = testObject.GetAllPayments("my token", DateTime.Now, DateTime.Now.AddDays(1), 1);

            // Assert 
            Assert.IsNotNull(orders);
            Assert.AreEqual(0, orders.Count);
        }

        #endregion Combined Payments Tests


        #region Get Sold Items Tests

        [TestMethod]
        public void GetSoldItemsSummary_DelegatesToRequestFactory_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItemsSummary("my token", 30);

            mockedRequestFactory.Verify(f => f.CreateSellingRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetSoldItemsSummary_DelegatesToSellingRequest_AndCallsGetSoldItemsSummary_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItemsSummary(It.IsAny<int>())).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItemsSummary("my token", 30);

            // Cast this to get the mock request so we can see if it was called
            mockedRequest.Verify(r => r.GetSoldItemsSummary(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetSoldItemsSummary_ThrowsEbayException_WhenDurationInDaysExceedsMaximum_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItemsSummary(It.IsAny<int>())).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItemsSummary("my token", mockedRequest.Object.MaximumDurationInDays + 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetSoldItemsSummary_ThrowsEbayException_WhenDurationInDaysIsLessThanZero_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItemsSummary(It.IsAny<int>())).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItemsSummary("my token", -1);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetSoldItemsSummary_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItemsSummary(It.IsAny<int>())).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItemsSummary(string.Empty, 30);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetSoldItemsSummary_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItemsSummary(It.IsAny<int>())).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItemsSummary(null, 30);
        }




        [TestMethod]
        public void GetSoldItems_DelegatesToRequestFactory_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItems(It.IsAny<int>(), It.IsAny<int>())).Returns(new GetMyeBaySellingResponseType());

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItems("my token", 30, 1);

            mockedRequestFactory.Verify(f => f.CreateSellingRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetSoldItems_DelegatesToSellingRequest_AndCallsGetSoldItems_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItems(It.IsAny<int>(), It.IsAny<int>())).Returns(new GetMyeBaySellingResponseType());

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItems("my token", 30, 1);
                        
            mockedRequest.Verify(r => r.GetSoldItems(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetSoldItems_ReturnsEmptyList_WhenEbaySoldListIsNull_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItems(It.IsAny<int>(), It.IsAny<int>())).Returns(new GetMyeBaySellingResponseType { SoldList = null } );

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            List<OrderTransactionType> orderTransactions = testObject.GetSoldItems("my token", 30, 1);

            Assert.IsNotNull(orderTransactions);
            Assert.AreEqual(0, orderTransactions.Count);
        }

        [TestMethod]
        public void GetSoldItems_ReturnsEmptyList_WhenEbayOrderTransactionArrayInSoldListIsNull_Test()
        {
            // Arrange
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItems(It.IsAny<int>(), It.IsAny<int>()))
                         .Returns(new GetMyeBaySellingResponseType
                                    {
                                        SoldList = new PaginatedOrderTransactionArrayType
                                        {
                                            OrderTransactionArray = null
                                        }
                                    }
                                  );

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            List<OrderTransactionType> orderTransactions = testObject.GetSoldItems("my token", 30, 1);

            // Assert
            Assert.IsNotNull(orderTransactions);
            Assert.AreEqual(0, orderTransactions.Count);
        }

        [TestMethod]
        public void GetSoldItems_ReturnsPopulatedList_WhenEbayOrderTransactionsAreAvailable_Test()
        {
            // Arrange
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItems(It.IsAny<int>(), It.IsAny<int>()))
                         .Returns(new GetMyeBaySellingResponseType
                                    {
                                        SoldList = new PaginatedOrderTransactionArrayType
                                        {
                                            OrderTransactionArray = new OrderTransactionType[] { new OrderTransactionType(), new OrderTransactionType() }
                                        }
                                    }
                         );

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            List<OrderTransactionType> orderTransactions = testObject.GetSoldItems("my token", 30, 1);

            // Assert - we're using our mock selling request, so we know there should 2 transactions present
            Assert.IsNotNull(orderTransactions);
            Assert.AreEqual(2, orderTransactions.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetSoldItems_ThrowsEbayException_WhenDurationInDaysExceedsMaximum_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItemsSummary(It.IsAny<int>())).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItems("my token", mockedRequest.Object.MaximumDurationInDays + 1, 1);            
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetSoldItems_ThrowsEbayException_WhenDurationInDaysIsLessThanZero_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItemsSummary(It.IsAny<int>())).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItems("my token", -1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetSoldItems_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItemsSummary(It.IsAny<int>())).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItems(string.Empty, 30, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetSoldItems_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            Mock<ISellingRequest> mockedRequest = new Mock<ISellingRequest>();
            mockedRequest.Setup(r => r.MaximumDurationInDays).Returns(60);
            mockedRequest.Setup(r => r.GetSoldItemsSummary(It.IsAny<int>())).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateSellingRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetSoldItems(null, 30, 1);
        }

        #endregion Get Sold Items Tests


        #region Feedback Tests


        [TestMethod]
        public void GetFeedbackSummary_DelegatesToRequestFactory_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(new Mock<IFeedbackRequest>().Object);

            
            testObject.GetFeedbackSummary("my token");

            mockedRequestFactory.Verify(f => f.CreateFeedbackRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetFeedbackSummary_DelegatesToFeedbackRequest_AndCallsGetFeedbackSummary_Test()
        {
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDownloadSummary()).Returns(new EbayDownloadSummary(1, 1, DateTime.Now, DateTime.Now));

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetFeedbackSummary("my token");

            mockedRequest.Verify(r => r.GetFeedbackDownloadSummary(), Times.Once());
        }

        [TestMethod]
        public void GetFeedbackSummary_ReturnsNonNullSummary_WhenRequestReturnsNullSummary_Test()
        {
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDownloadSummary()).Returns(null as EbayDownloadSummary);

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            EbayDownloadSummary summary = testObject.GetFeedbackSummary("my token");

            Assert.IsNotNull(summary);
        }

        [TestMethod]
        public void GetFeedbackSummary_ReturnsSummaryWithZeroTransactions_WhenRequestReturnsNullSummary_Test()
        {
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDownloadSummary()).Returns(null as EbayDownloadSummary);

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            EbayDownloadSummary summary = testObject.GetFeedbackSummary("my token");

            Assert.AreEqual(0, summary.NumberOfTransactions);
        }

        [TestMethod]
        public void GetFeedbackSummary_ReturnsSummaryWithZeroPages_WhenRequestReturnsNullSummary_Test()
        {
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDownloadSummary()).Returns(null as EbayDownloadSummary);

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            EbayDownloadSummary summary = testObject.GetFeedbackSummary("my token");

            Assert.AreEqual(0, summary.NumberOfPages);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetFeedbackSummary_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(new Mock<IFeedbackRequest>().Object);

            
            testObject.GetFeedbackSummary(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetFeedbackSummary_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(new Mock<IFeedbackRequest>().Object);

            
            testObject.GetFeedbackSummary(null);
        }




        [TestMethod]
        public void GetFeedbackDetails_DelegatesToRequestFactory_Test()
        {
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDetails(It.IsAny<int>())).Returns(new GetFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetFeedbackDetails("my token", 1);

            mockedRequestFactory.Verify(f => f.CreateFeedbackRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetFeedbackDetails_DelegatesToFeedbackRequest_AndCallsGetFeedbackDetails_Test()
        {
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDetails(It.IsAny<int>())).Returns(new GetFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetFeedbackDetails("my token", 1);

            mockedRequest.Verify(r => r.GetFeedbackDetails(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetFeedbackDetails_ReturnsListWithSameCount_AsFeedbackDetailArrayInEbayResponse_Test()
        {
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDetails(It.IsAny<int>()))
                         .Returns(new GetFeedbackResponseType
                                {
                                    FeedbackDetailArray = new FeedbackDetailType[]
                                    {
                                    new FeedbackDetailType(),
                                    new FeedbackDetailType(),
                                    new FeedbackDetailType()
                                    }
                                }
                         );

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            List<FeedbackDetailType> feedbackDetails = testObject.GetFeedbackDetails("my token", 1);
            
            // We know there should be 3 items because we're using the mock feedback request
            Assert.AreEqual(3, feedbackDetails.Count);
        }

        [TestMethod]
        public void GetFeedbackDetails_ReturnsEmptyList_WhenEbayResponseContainsNullFeedbackDetailArray_Test()
        {
            // Arrange
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDetails(It.IsAny<int>()))
                         .Returns(new GetFeedbackResponseType
                                {
                                    FeedbackDetailArray = null
                                }
                         );

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            List<FeedbackDetailType> feedbackDetails = testObject.GetFeedbackDetails("my token", 1);

            // Assert
            Assert.AreEqual(0, feedbackDetails.Count);
        }

        [TestMethod]
        public void GetFeedbackDetails_ReturnsNonNullList_WhenEbayResponseContainsNullFeedbackDetailArray_Test()
        {
            // Arrange
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDetails(It.IsAny<int>()))
                         .Returns(new GetFeedbackResponseType
                                 {
                                     FeedbackDetailArray = null
                                 }
                         );

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            // Act
            
            List<FeedbackDetailType> feedbackDetails = testObject.GetFeedbackDetails("my token", 1);

            // Assert
            Assert.IsNotNull(feedbackDetails);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetFeedbackDetails_ThrowsEbayException_PageNumberIsZero_Test()
        {
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDetails(It.IsAny<int>())).Returns(new GetFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetFeedbackDetails(string.Empty, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetFeedbackDetails_ThrowsEbayException_PageNumberIsNegative_Test()
        {
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDetails(It.IsAny<int>())).Returns(new GetFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetFeedbackDetails(string.Empty, -5);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetFeedbackDetails_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDetails(It.IsAny<int>())).Returns(new GetFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetFeedbackDetails(string.Empty, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetFeedbackDetails_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            Mock<IFeedbackRequest> mockedRequest = new Mock<IFeedbackRequest>();
            mockedRequest.Setup(r => r.GetFeedbackDetails(It.IsAny<int>())).Returns(new GetFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetFeedbackDetails(null, 1);
        }
        #endregion Feedback Tests


        #region Item Tests

        [TestMethod]
        public void GetItemDetails_DelegatesToRequestFactory_Test()
        {
            Mock<IItemRequest> mockedRequest = new Mock<IItemRequest>();
            mockedRequest.Setup(r => r.GetItemDetails(It.IsAny<string>())).Returns(new GetItemResponseType { Item = new ItemType() });

            mockedRequestFactory.Setup(f => f.CreateItemRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetItemDetails("my token", "my item ID");

            mockedRequestFactory.Verify(f => f.CreateItemRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetItemDetails_DelegatesToItemRequest_AndCallsGetItemDetails_Test()
        {
            Mock<IItemRequest> mockedRequest = new Mock<IItemRequest>();
            mockedRequest.Setup(r => r.GetItemDetails(It.IsAny<string>())).Returns(new GetItemResponseType { Item = new ItemType() });

            mockedRequestFactory.Setup(f => f.CreateItemRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetItemDetails("my token", "my item ID");

            mockedRequest.Verify(r => r.GetItemDetails(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetItemDetails_ThrowsEbayException_WhenResponseContainsNullItem_Test()
        {
            Mock<IItemRequest> mockedRequest = new Mock<IItemRequest>();
            mockedRequest.Setup(r => r.GetItemDetails(It.IsAny<string>())).Returns(new GetItemResponseType { Item = null });

            mockedRequestFactory.Setup(f => f.CreateItemRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetItemDetails("my token", string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetItemDetails_ThrowsEbayException_WhenGivenAnEmptyItemId_Test()
        {
            Mock<IItemRequest> mockedRequest = new Mock<IItemRequest>();
            mockedRequest.Setup(r => r.GetItemDetails(It.IsAny<string>())).Returns(new GetItemResponseType { Item = new ItemType() });

            mockedRequestFactory.Setup(f => f.CreateItemRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetItemDetails("my token", string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetItemDetails_ThrowsEbayException_WhenGivenNullItemId_Test()
        {
            Mock<IItemRequest> mockedRequest = new Mock<IItemRequest>();
            mockedRequest.Setup(r => r.GetItemDetails(It.IsAny<string>())).Returns(new GetItemResponseType { Item = new ItemType() });

            mockedRequestFactory.Setup(f => f.CreateItemRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetItemDetails("my token", null);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetItemDetails_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            Mock<IItemRequest> mockedRequest = new Mock<IItemRequest>();
            mockedRequest.Setup(r => r.GetItemDetails(It.IsAny<string>())).Returns(new GetItemResponseType { Item = new ItemType() });

            mockedRequestFactory.Setup(f => f.CreateItemRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetItemDetails(string.Empty, "my item ID");
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetItemDetails_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            Mock<IItemRequest> mockedRequest = new Mock<IItemRequest>();
            mockedRequest.Setup(r => r.GetItemDetails(It.IsAny<string>())).Returns(new GetItemResponseType { Item = new ItemType() });

            mockedRequestFactory.Setup(f => f.CreateItemRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetItemDetails(null, "my item ID");
        }

        #endregion Item Tests


        #region Item Transaction Details Tests

        [TestMethod]
        public void GetTransactionDetails_DelegatesToRequestFactory_Test()
        {
            Mock<IItemTransactionRequest> mockedRequest = new Mock<IItemTransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactionDetail(It.IsAny<string>(), It.IsAny<string>())).Returns(new GetItemTransactionsResponseType());

            mockedRequestFactory.Setup(f => f.CreateItemTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetTransactionDetails("my token", "my item ID", "my transaction ID");

            mockedRequestFactory.Verify(f => f.CreateItemTransactionRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void GetTransactionDetails_DelegatesToItemTransactionRequest_AndCallsGetTransactionDetails_Test()
        {
            Mock<IItemTransactionRequest> mockedRequest = new Mock<IItemTransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactionDetail(It.IsAny<string>(), It.IsAny<string>())).Returns(new GetItemTransactionsResponseType());

            mockedRequestFactory.Setup(f => f.CreateItemTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetTransactionDetails("my token", "my item ID", "my transaction ID");

            mockedRequest.Verify(r => r.GetTransactionDetail(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTransaction_ReturnsNonNullTransaction_WhenResponseContainsTransaction_Test()
        {
            Mock<IItemTransactionRequest> mockedRequest = new Mock<IItemTransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactionDetail(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new GetItemTransactionsResponseType 
                        { 
                            TransactionArray = new TransactionType[] { new TransactionType() } 
                        });

            mockedRequestFactory.Setup(f => f.CreateItemTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            TransactionType transaction = testObject.GetTransactionDetails("my token", "my item ID", "my transaction ID");

            Assert.IsNotNull(transaction);
        }

        [TestMethod]
        public void GetTransaction_ReturnsNullTransaction_WhenResponseContainsNullTransaction_Test()
        {
            Mock<IItemTransactionRequest> mockedRequest = new Mock<IItemTransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactionDetail(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new GetItemTransactionsResponseType
                {
                    TransactionArray = null
                });

            mockedRequestFactory.Setup(f => f.CreateItemTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            TransactionType transaction = testObject.GetTransactionDetails("my token", "my item ID", "my transaction ID");

            Assert.IsNull(transaction);
        }
        
        [ExpectedException(typeof(EbayException))]
        public void GetTransactionDetails_ThrowsEbayException_WhenGivenAnEmptyTransactionId_Test()
        {
            Mock<IItemTransactionRequest> mockedRequest = new Mock<IItemTransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactionDetail(It.IsAny<string>(), It.IsAny<string>())).Returns(new GetItemTransactionsResponseType());

            mockedRequestFactory.Setup(f => f.CreateItemTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetTransactionDetails("my token", "my item ID", string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetTransactionDetails_ThrowsEbayException_WhenGivenNullTransactionId_Test()
        {
            Mock<IItemTransactionRequest> mockedRequest = new Mock<IItemTransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactionDetail(It.IsAny<string>(), It.IsAny<string>())).Returns(new GetItemTransactionsResponseType());

            mockedRequestFactory.Setup(f => f.CreateItemTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetTransactionDetails("my token", "my item ID", null);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetTransactionDetails_ThrowsEbayException_WhenGivenAnEmptyItemId_Test()
        {
            Mock<IItemTransactionRequest> mockedRequest = new Mock<IItemTransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactionDetail(It.IsAny<string>(), It.IsAny<string>())).Returns(new GetItemTransactionsResponseType());

            mockedRequestFactory.Setup(f => f.CreateItemTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetTransactionDetails("my token", string.Empty, "my transaction ID");
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetTransactionDetails_ThrowsEbayException_WhenGivenNullItemId_Test()
        {
            Mock<IItemTransactionRequest> mockedRequest = new Mock<IItemTransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactionDetail(It.IsAny<string>(), It.IsAny<string>())).Returns(new GetItemTransactionsResponseType());

            mockedRequestFactory.Setup(f => f.CreateItemTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetTransactionDetails("my token", null, "my transaction ID");
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetTransactionDetails_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            Mock<IItemTransactionRequest> mockedRequest = new Mock<IItemTransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactionDetail(It.IsAny<string>(), It.IsAny<string>())).Returns(new GetItemTransactionsResponseType());

            mockedRequestFactory.Setup(f => f.CreateItemTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetTransactionDetails(string.Empty, "my item ID", "my transaction ID");
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void GetTransactionDetails_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            Mock<IItemTransactionRequest> mockedRequest = new Mock<IItemTransactionRequest>();
            mockedRequest.Setup(r => r.GetTransactionDetail(It.IsAny<string>(), It.IsAny<string>())).Returns(new GetItemTransactionsResponseType());

            mockedRequestFactory.Setup(f => f.CreateItemTransactionRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.GetTransactionDetails(null, "my item ID", "my transaction ID");
        }


        #endregion Item Transaction Details Tests


        #region Leave Feedback Tests

        [TestMethod]
        public void LeaveFeedback_DelegatesToRequestFactory_Test()
        {
            Mock<ILeaveFeedbackRequest> mockedRequest = new Mock<ILeaveFeedbackRequest>();
            mockedRequest.Setup(r => r.LeaveFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>())).Returns(new LeaveFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateLeaveFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.LeaveFeedback("my token", "my item ID", "my transaction ID", "userId", CommentTypeCodeType.Positive, string.Empty);

            mockedRequestFactory.Verify(f => f.CreateLeaveFeedbackRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void LeaveFeedback_DelegatesToLeaveFeedbackRequest_AndCallsLeaveFeedback_Test()
        {
            Mock<ILeaveFeedbackRequest> mockedRequest = new Mock<ILeaveFeedbackRequest>();
            mockedRequest.Setup(r => r.LeaveFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>())).Returns(new LeaveFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateLeaveFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.LeaveFeedback("my token", "my item ID", "my transaction ID", "userId", CommentTypeCodeType.Positive, string.Empty);

            mockedRequest.Verify(r => r.LeaveFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>()), Times.Once());
        }


        [ExpectedException(typeof(EbayException))]
        public void LeaveFeedback_ThrowsEbayException_WhenGivenAnEmptyUserId_Test()
        {
            Mock<ILeaveFeedbackRequest> mockedRequest = new Mock<ILeaveFeedbackRequest>();
            mockedRequest.Setup(r => r.LeaveFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>())).Returns(new LeaveFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateLeaveFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.LeaveFeedback("my token", "my item ID", "my transaction ID", string.Empty, CommentTypeCodeType.Positive, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void LeaveFeedback_ThrowsEbayException_WhenGivenNullUserId_Test()
        {
            Mock<ILeaveFeedbackRequest> mockedRequest = new Mock<ILeaveFeedbackRequest>();
            mockedRequest.Setup(r => r.LeaveFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>())).Returns(new LeaveFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateLeaveFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.LeaveFeedback("my token", "my item ID", "my transaction ID", string.Empty, CommentTypeCodeType.Positive, string.Empty);
        }

        [ExpectedException(typeof(EbayException))]
        public void LeaveFeedback_ThrowsEbayException_WhenGivenAnEmptyTransactionId_Test()
        {
            Mock<ILeaveFeedbackRequest> mockedRequest = new Mock<ILeaveFeedbackRequest>();
            mockedRequest.Setup(r => r.LeaveFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>())).Returns(new LeaveFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateLeaveFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.LeaveFeedback("my token", "my item ID", string.Empty, "userId", CommentTypeCodeType.Positive, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void LeaveFeedback_ThrowsEbayException_WhenGivenNullTransactionId_Test()
        {
            Mock<ILeaveFeedbackRequest> mockedRequest = new Mock<ILeaveFeedbackRequest>();
            mockedRequest.Setup(r => r.LeaveFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>())).Returns(new LeaveFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateLeaveFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.LeaveFeedback("my token", "my item ID", null, "userId", CommentTypeCodeType.Positive, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void LeaveFeedback_ThrowsEbayException_WhenGivenAnEmptyItemId_Test()
        {
            Mock<ILeaveFeedbackRequest> mockedRequest = new Mock<ILeaveFeedbackRequest>();
            mockedRequest.Setup(r => r.LeaveFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>())).Returns(new LeaveFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateLeaveFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.LeaveFeedback("my token", string.Empty, "my transaction ID", "userId", CommentTypeCodeType.Positive, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void LeaveFeedback_ThrowsEbayException_WhenGivenNullItemId_Test()
        {
            Mock<ILeaveFeedbackRequest> mockedRequest = new Mock<ILeaveFeedbackRequest>();
            mockedRequest.Setup(r => r.LeaveFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>())).Returns(new LeaveFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateLeaveFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.LeaveFeedback("my token", null, "my transaction ID", "userId", CommentTypeCodeType.Positive, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void LeaveFeedback_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            Mock<ILeaveFeedbackRequest> mockedRequest = new Mock<ILeaveFeedbackRequest>();
            mockedRequest.Setup(r => r.LeaveFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>())).Returns(new LeaveFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateLeaveFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.LeaveFeedback(string.Empty, "my item ID", "my transaction ID", "userId", CommentTypeCodeType.Positive, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void LeaveFeedback_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            Mock<ILeaveFeedbackRequest> mockedRequest = new Mock<ILeaveFeedbackRequest>();
            mockedRequest.Setup(r => r.LeaveFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>())).Returns(new LeaveFeedbackResponseType());

            mockedRequestFactory.Setup(f => f.CreateLeaveFeedbackRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.LeaveFeedback(null, "my item ID", "my transaction ID", "userId", CommentTypeCodeType.Positive, string.Empty);
        }

        #endregion Leave Feedback Tests


        #region Send Message To Partner Tests


        [TestMethod]
        public void SendMessageToPartner_DelegatesToRequestFactory_Test()
        {
            Mock<ISendPartnerMessageRequest> mockedRequest = new Mock<ISendPartnerMessageRequest>();
            mockedRequest.Setup(r => r.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QuestionTypeCodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new AddMemberMessageAAQToPartnerResponseType());

            mockedRequestFactory.Setup(f => f.CreateSendPartnerMessageRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.SendMessageToPartner("my token", "my item ID", "userId", QuestionTypeCodeType.General, string.Empty, string.Empty, true);

            mockedRequestFactory.Verify(f => f.CreateSendPartnerMessageRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void SendMessageToPartner_DelegatesToSendMessageToPartnerRequest_AndCallsSendMessageToPartner_Test()
        {
            Mock<ISendPartnerMessageRequest> mockedRequest = new Mock<ISendPartnerMessageRequest>();
            mockedRequest.Setup(r => r.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QuestionTypeCodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new AddMemberMessageAAQToPartnerResponseType());

            mockedRequestFactory.Setup(f => f.CreateSendPartnerMessageRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.SendMessageToPartner("my token", "my item ID", "userId", QuestionTypeCodeType.General, string.Empty, string.Empty, true);

            mockedRequest.Verify(r => r.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QuestionTypeCodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        }


        [ExpectedException(typeof(EbayException))]
        public void SendMessageToPartner_ThrowsEbayException_WhenGivenAnEmptyUserId_Test()
        {
            Mock<ISendPartnerMessageRequest> mockedRequest = new Mock<ISendPartnerMessageRequest>();
            mockedRequest.Setup(r => r.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QuestionTypeCodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new AddMemberMessageAAQToPartnerResponseType());

            mockedRequestFactory.Setup(f => f.CreateSendPartnerMessageRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.SendMessageToPartner("my token", "my item ID", string.Empty, QuestionTypeCodeType.General, string.Empty, string.Empty, true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void SendMessageToPartner_ThrowsEbayException_WhenGivenNullUserId_Test()
        {
            Mock<ISendPartnerMessageRequest> mockedRequest = new Mock<ISendPartnerMessageRequest>();
            mockedRequest.Setup(r => r.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QuestionTypeCodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new AddMemberMessageAAQToPartnerResponseType());

            mockedRequestFactory.Setup(f => f.CreateSendPartnerMessageRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.SendMessageToPartner("my token", "my item ID", null, QuestionTypeCodeType.General, string.Empty, string.Empty, true);
        }
        
        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void SendMessageToPartner_ThrowsEbayException_WhenGivenAnEmptyItemId_Test()
        {
            Mock<ISendPartnerMessageRequest> mockedRequest = new Mock<ISendPartnerMessageRequest>();
            mockedRequest.Setup(r => r.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QuestionTypeCodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new AddMemberMessageAAQToPartnerResponseType());

            mockedRequestFactory.Setup(f => f.CreateSendPartnerMessageRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.SendMessageToPartner("my token", string.Empty, "userId", QuestionTypeCodeType.General, string.Empty, string.Empty, true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void SendMessageToPartner_ThrowsEbayException_WhenGivenNullItemId_Test()
        {
            Mock<ISendPartnerMessageRequest> mockedRequest = new Mock<ISendPartnerMessageRequest>();
            mockedRequest.Setup(r => r.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QuestionTypeCodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new AddMemberMessageAAQToPartnerResponseType());

            mockedRequestFactory.Setup(f => f.CreateSendPartnerMessageRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.SendMessageToPartner("my token", null, "userId", QuestionTypeCodeType.General, string.Empty, string.Empty, true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void SendMessageToPartner_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            Mock<ISendPartnerMessageRequest> mockedRequest = new Mock<ISendPartnerMessageRequest>();
            mockedRequest.Setup(r => r.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QuestionTypeCodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new AddMemberMessageAAQToPartnerResponseType());

            mockedRequestFactory.Setup(f => f.CreateSendPartnerMessageRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.SendMessageToPartner(string.Empty, "my item ID", "userId", QuestionTypeCodeType.General, string.Empty, string.Empty, true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void SendMessageToPartner_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            Mock<ISendPartnerMessageRequest> mockedRequest = new Mock<ISendPartnerMessageRequest>();
            mockedRequest.Setup(r => r.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QuestionTypeCodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new AddMemberMessageAAQToPartnerResponseType());

            mockedRequestFactory.Setup(f => f.CreateSendPartnerMessageRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.SendMessageToPartner(null, "my item ID", "userId", QuestionTypeCodeType.General, string.Empty, string.Empty, true);
        }


        #endregion Send Message To Partner Tests


        #region Complete Sale Tests

        [TestMethod]
        public void CompleteSale_DelegatesToRequestFactory_Test()
        {
            Mock<ICompleteSaleRequest> mockedRequest = new Mock<ICompleteSaleRequest>();
            mockedRequest.Setup(r => r.CompleteSale(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new CompleteSaleResponseType());

            mockedRequestFactory.Setup(f => f.CreateCompleteSaleRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            
            testObject.CompleteSale("my token", "my item ID", "my transaction ID", true, true, "123", ShippingCarrierCodeType.UPS.ToString());

            mockedRequestFactory.Verify(f => f.CreateCompleteSaleRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void CompleteSale_DelegatesToCompleteSaleRequest_AndCallsCompleteSale_Test()
        {
            Mock<ICompleteSaleRequest> mockedRequest = new Mock<ICompleteSaleRequest>();
            mockedRequest.Setup(r => r.CompleteSale(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new CompleteSaleResponseType());

            mockedRequestFactory.Setup(f => f.CreateCompleteSaleRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.CompleteSale("my token", "my item ID", "my transaction ID", true, true, "123", ShippingCarrierCodeType.UPS.ToString());

            mockedRequest.Verify(r => r.CompleteSale(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [ExpectedException(typeof(EbayException))]
        public void CompleteSale_ThrowsEbayException_WhenGivenAnEmptyTransactionId_Test()
        {
            Mock<ICompleteSaleRequest> mockedRequest = new Mock<ICompleteSaleRequest>();
            mockedRequest.Setup(r => r.CompleteSale(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new CompleteSaleResponseType());

            mockedRequestFactory.Setup(f => f.CreateCompleteSaleRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.CompleteSale("my token", "my item ID", string.Empty, true, true, "123", ShippingCarrierCodeType.UPS.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CompleteSale_ThrowsEbayException_WhenGivenNullTransactionId_Test()
        {
            Mock<ICompleteSaleRequest> mockedRequest = new Mock<ICompleteSaleRequest>();
            mockedRequest.Setup(r => r.CompleteSale(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new CompleteSaleResponseType());

            mockedRequestFactory.Setup(f => f.CreateCompleteSaleRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.CompleteSale("my token", "my item ID", null, true, true, "123", ShippingCarrierCodeType.UPS.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CompleteSale_ThrowsEbayException_WhenGivenAnEmptyItemId_Test()
        {
            Mock<ICompleteSaleRequest> mockedRequest = new Mock<ICompleteSaleRequest>();
            mockedRequest.Setup(r => r.CompleteSale(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new CompleteSaleResponseType());

            mockedRequestFactory.Setup(f => f.CreateCompleteSaleRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.CompleteSale("my token", string.Empty, "my transaction ID", true, true, "123", ShippingCarrierCodeType.UPS.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CompleteSale_ThrowsEbayException_WhenGivenNullItemId_Test()
        {
            Mock<ICompleteSaleRequest> mockedRequest = new Mock<ICompleteSaleRequest>();
            mockedRequest.Setup(r => r.CompleteSale(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new CompleteSaleResponseType());

            mockedRequestFactory.Setup(f => f.CreateCompleteSaleRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.CompleteSale("my token", null, "my transaction ID", true, true, "123", ShippingCarrierCodeType.UPS.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CompleteSale_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            Mock<ICompleteSaleRequest> mockedRequest = new Mock<ICompleteSaleRequest>();
            mockedRequest.Setup(r => r.CompleteSale(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new CompleteSaleResponseType());

            mockedRequestFactory.Setup(f => f.CreateCompleteSaleRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.CompleteSale(string.Empty, "my item ID", "my transaction ID", true, true, "123", ShippingCarrierCodeType.UPS.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CompleteSale_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            Mock<ICompleteSaleRequest> mockedRequest = new Mock<ICompleteSaleRequest>();
            mockedRequest.Setup(r => r.CompleteSale(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new CompleteSaleResponseType());

            mockedRequestFactory.Setup(f => f.CreateCompleteSaleRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.CompleteSale(null, "my item ID", "my transaction ID", true, true, "123", ShippingCarrierCodeType.UPS.ToString());
        }

        #endregion Complete Sale Tests


        #region Save Note Tests

        [TestMethod]
        public void SaveNote_DelegatesToRequestFactory_Test()
        {
            Mock<IUserNotesRequest> mockedRequest = new Mock<IUserNotesRequest>();
            mockedRequest.Setup(r => r.SaveNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new SetUserNotesResponseType());

            mockedRequestFactory.Setup(f => f.CreateUserNotesRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.SaveNote("my token", "my item ID", "my transaction ID", "the note text");

            mockedRequestFactory.Verify(f => f.CreateUserNotesRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void SaveNote_DelegatesToSaveNoteRequest_AndCallsSaveNote_Test()
        {
            Mock<IUserNotesRequest> mockedRequest = new Mock<IUserNotesRequest>();
            mockedRequest.Setup(r => r.SaveNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new SetUserNotesResponseType());

            mockedRequestFactory.Setup(f => f.CreateUserNotesRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.SaveNote("my token", "my item ID", "my transaction ID", "the note text");

            mockedRequest.Verify(r => r.SaveNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void SaveNote_DoesNot_ThrowException_WhenGivenAnEmptyTransactionId_Test()
        {
            Mock<IUserNotesRequest> mockedRequest = new Mock<IUserNotesRequest>();
            mockedRequest.Setup(r => r.SaveNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new SetUserNotesResponseType());

            mockedRequestFactory.Setup(f => f.CreateUserNotesRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.SaveNote("my token", "my item ID", string.Empty, "the note text");

            // We just want to make sure the save note method did not throw a validation 
            // exception because of the empty transaction ID and that the request.SaveNote method was invoked
            mockedRequest.Verify(r => r.SaveNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [TestMethod]
        public void SaveNote_DoesNot_ThrowException_WhenGivenNullTransactionId_Test()
        {
            Mock<IUserNotesRequest> mockedRequest = new Mock<IUserNotesRequest>();
            mockedRequest.Setup(r => r.SaveNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new SetUserNotesResponseType());

            mockedRequestFactory.Setup(f => f.CreateUserNotesRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);


            testObject.SaveNote("my token", "my item ID", null, "the note text");

            // We just want to make sure the save note method did not throw a validation 
            // exception because of the null transaction ID and that the request.SaveNote method was invoked
            mockedRequest.Verify(r => r.SaveNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void SaveNote_ThrowsEbayException_WhenGivenAnEmptyItemId_Test()
        {
            Mock<IUserNotesRequest> mockedRequest = new Mock<IUserNotesRequest>();
            mockedRequest.Setup(r => r.SaveNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new SetUserNotesResponseType());

            mockedRequestFactory.Setup(f => f.CreateUserNotesRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.SaveNote("my token", string.Empty, "my transaction ID", "the note text");
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void SaveNote_ThrowsEbayException_WhenGivenNullItemId_Test()
        {
            Mock<IUserNotesRequest> mockedRequest = new Mock<IUserNotesRequest>();
            mockedRequest.Setup(r => r.SaveNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new SetUserNotesResponseType());

            mockedRequestFactory.Setup(f => f.CreateUserNotesRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.SaveNote("my token", null, "my transaction ID", "the note text");
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void SaveNote_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            Mock<IUserNotesRequest> mockedRequest = new Mock<IUserNotesRequest>();
            mockedRequest.Setup(r => r.SaveNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new SetUserNotesResponseType());

            mockedRequestFactory.Setup(f => f.CreateUserNotesRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.SaveNote(string.Empty, "my item ID", "my transaction ID", "the note text");
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void SaveNote_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            Mock<IUserNotesRequest> mockedRequest = new Mock<IUserNotesRequest>();
            mockedRequest.Setup(r => r.SaveNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new SetUserNotesResponseType());

            mockedRequestFactory.Setup(f => f.CreateUserNotesRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.SaveNote(null, "my item ID", "my transaction ID", "the note text");
        }

        #endregion Save Note Tests


        #region Combine Order Tests

        /// <summary>
        /// Creates a ICombinedOrdersRequest that returns a valid response
        /// </summary>
        private Mock<ICombineOrdersRequest> CreateMockedCombinedOrdersRequest()
        {
            Mock<ICombineOrdersRequest> mockedRequest = new Mock<ICombineOrdersRequest>();
            mockedRequest.Setup(r => r.CombineOrders(It.IsAny<IEnumerable<TransactionType>>(), It.IsAny<double>(), It.IsAny<IEnumerable<BuyerPaymentMethodCodeType>>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new AddOrderResponseType { OrderID = "123456" } );

            return mockedRequest;
        }

        
        /// <summary>
        /// Creates a ICombinedOrdersRequest that returns a response with a null Order ID
        /// </summary>
        private Mock<ICombineOrdersRequest> CreateMockedCombinedOrdersRequestWithNullOrderId()
        {
            Mock<ICombineOrdersRequest> mockedRequest = new Mock<ICombineOrdersRequest>();
            mockedRequest.Setup(r => r.CombineOrders(It.IsAny<IEnumerable<TransactionType>>(), It.IsAny<double>(), It.IsAny<IEnumerable<BuyerPaymentMethodCodeType>>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new AddOrderResponseType { OrderID = null });

            return mockedRequest;
        }

        /// <summary>
        /// Creates a ICombinedOrdersRequest that returns an empty string for the Order ID
        /// </summary>
        private Mock<ICombineOrdersRequest> CreateMockedCombinedOrdersRequestWithEmptyOrderId()
        {
            Mock<ICombineOrdersRequest> mockedRequest = new Mock<ICombineOrdersRequest>();
            mockedRequest.Setup(r => r.CombineOrders(It.IsAny<IEnumerable<TransactionType>>(), It.IsAny<double>(), It.IsAny<IEnumerable<BuyerPaymentMethodCodeType>>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new AddOrderResponseType { OrderID = string.Empty });

            return mockedRequest;
        }

        /// <summary>
        /// Creates a ICombinedOrdersRequest that returns a response with an order ID of zero
        /// </summary>
        private Mock<ICombineOrdersRequest> CreateMockedCombinedOrdersRequestWithZeroOrderId()
        {
            Mock<ICombineOrdersRequest> mockedRequest = new Mock<ICombineOrdersRequest>();
            mockedRequest.Setup(r => r.CombineOrders(It.IsAny<IEnumerable<TransactionType>>(), It.IsAny<double>(), It.IsAny<IEnumerable<BuyerPaymentMethodCodeType>>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new AddOrderResponseType { OrderID = "0" });

            return mockedRequest;
        }

        [TestMethod]
        public void CombineOrders_DelegatesToRequestFactory_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequest();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders("my token", validTransactionsToCombine, 0.0d, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", 0.0m, "MO", true);

            mockedRequestFactory.Verify(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>()), Times.Once());
        }

        [TestMethod]
        public void CombineOrders_DelegatesToCombineOrdersRequest_AndCallsCombineOrders_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequest();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders("my token", validTransactionsToCombine, 0.0d, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", 0.0m, "MO", true);

            mockedRequest.Verify(r => r.CombineOrders(It.IsAny<IEnumerable<TransactionType>>(), It.IsAny<double>(), It.IsAny<IEnumerable<BuyerPaymentMethodCodeType>>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        }


        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CombineOrders_ThrowsEbayException_WhenGivenAnEmptyToken_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequest();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders(string.Empty, validTransactionsToCombine, 0.0d, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", 0.0m, "MO", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CombineOrders_ThrowsEbayException_WhenGivenNullToken_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequest();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders(null, new List<TransactionType>(), 0.0d, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", 0.0m, "MO", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CombineOrders_ThrowsEbayException_WhenGivenNullTransactions_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequest();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders("my token", null, 0.0d, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", 0.0m, "MO", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CombineOrders_ThrowsEbayException_WhenGivenZeroTransactions_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequest();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders("my token", new List<TransactionType>(), 0.0d, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", 0.0m, "MO", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CombineOrders_ThrowsEbayException_WhenGivenOneTransactions_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequest();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            List<TransactionType> singleTransactionList = new List<TransactionType>() { new TransactionType() };


            testObject.CombineOrders("my token", singleTransactionList, 0.0d, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", 0.0m, "MO", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CombineOrders_ThrowsEbayException_WhenResponse_ContainsNullOrderId_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequestWithNullOrderId();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders("my token", validTransactionsToCombine, 0.0d, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", 0.0m, "MO", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CombineOrders_ThrowsEbayException_WhenResponse_ContainsEmptyOrderId_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequestWithEmptyOrderId();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders("my token", validTransactionsToCombine, 0.0d, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", 0.0m, "MO", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CombineOrders_ThrowsEbayException_WhenResponse_ContainsZeroOrderId_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequestWithZeroOrderId();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders("my token", validTransactionsToCombine, 0.0d, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", 0.0m, "MO", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CombineOrders_ThrowsEbayException_WhenGivenNegativeOrderTotal_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequest();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders("my token", validTransactionsToCombine, -0.01, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", 0.0m, "MO", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CombineOrders_ThrowsEbayException_WhenGivenNegativeSalesTax_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequest();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders("my token", validTransactionsToCombine, 0.0d, new List<BuyerPaymentMethodCodeType>(), 0.0m, "US", "shipping service", -.01m, "MO", true);
        }

        [TestMethod]
        [ExpectedException(typeof(EbayException))]
        public void CombineOrders_ThrowsEbayException_WhenGivenNegativeShippingCost_Test()
        {
            Mock<ICombineOrdersRequest> mockedRequest = CreateMockedCombinedOrdersRequest();
            mockedRequestFactory.Setup(f => f.CreateCombineOrdersRequest(It.IsAny<TokenData>())).Returns(mockedRequest.Object);

            testObject.CombineOrders("my token", validTransactionsToCombine, 0.0d, new List<BuyerPaymentMethodCodeType>(), -0.01m, "US", "shipping service", 0.0m, "MO", true);
        }

        #endregion Combine Order Tests

    }
}

