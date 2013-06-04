﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Stores.Platforms.PayPal.WebServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShipWorks.Tests.Stores.PayPal
{
    [TestClass]
    public class ExceptionSerializationTests
    {
        [TestMethod]
        public void SerializeDeserialize()
        {
            GetBalanceResponseType response = new GetBalanceResponseType();
            response.Errors = new ErrorType[] {
                new ErrorType() { ErrorCode = "a", LongMessage = " along message here" },
                new ErrorType() { ErrorCode = "b", LongMessage = "b long message here" },
                new ErrorType() { ErrorCode = "c1", LongMessage = "c1 long message here" }
            };

            PayPalException exception = new PayPalException(response);
            
            // ensure there are 3 errors
            Assert.AreEqual(3, exception.Errors.Count);

            PayPalException exceptionCopy = null;

            // serialize it
            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, exception);
                s.Position = 0; // Reset stream position
                exceptionCopy = (PayPalException) formatter.Deserialize(s);
            }

            // check equality
            Assert.IsNotNull(exceptionCopy);
            Assert.AreEqual(3, exceptionCopy.Errors.Count);

            for (int x = 0; x < exception.Errors.Count; x++)
            {
                Assert.AreEqual(exception.Errors[x].Code, exceptionCopy.Errors[x].Code);
                Assert.AreEqual(exception.Errors[x].Message, exceptionCopy.Errors[x].Message);
            }
        }
    }
}
