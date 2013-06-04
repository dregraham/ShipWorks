﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Newegg.Net.Errors;
using ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response;

namespace ShipWorks.Tests.Stores.Newegg
{
    [TestClass]
    public class ErrorResponseSerializerTest
    {
        private string errorXml;
        private string nonErrorXml;

        private ErrorResponseSerializer serializer;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new ErrorResponseSerializer();

            errorXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Errors xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Error>
    <Code>AccessDenied</Code>
    <Message>The specified seller id is invalid or you have not yet got the authorization from this seller.</Message>
  </Error>
</Errors>";

            nonErrorXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<NeweggAPIResponse>
  <IsSuccess>true</IsSuccess>
  <OperationType>GetServiceStatus</OperationType>
  <SellerID>A09V</SellerID>
  <ResponseBody>
    <Status>1</Status>
    <Timestamp>06/21/2012 06:09:33</Timestamp>
  </ResponseBody>
</NeweggAPIResponse>";

        }


        [TestMethod]
        public void Deserialize_ReturnsErrorResult_WhenDeserializingErrorXml_Test()
        {
            object result = serializer.Deserialize(errorXml);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ErrorResult));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Deserialize_ThrowsInvalidOperationException_WhenDeserializingNonErrorXml_Test()
        {
            object result = serializer.Deserialize(nonErrorXml);
        }

    }
}
