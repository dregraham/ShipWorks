using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    [TestClass]
    public class UspsUtilityTest
    {
        PostalShipmentEntity postalShipment = new PostalShipmentEntity();

        [TestInitialize]
        public void Initialize()
        {
            postalShipment.Memo1 = "memo1";
            postalShipment.Memo2 = "memo2";
            postalShipment.Memo3 = "memo3";
        }

        [TestMethod]
        public void BuildMemoField_StartsWithMultilineChar_WhenOneMemoFieldHasContent_Test()
        {
            postalShipment.Memo1 = "";
            postalShipment.Memo2 = "";
            string apiMemoText = UspsUtility.BuildMemoField(postalShipment);

            Debug.Assert(Regex.Matches(apiMemoText, "\x09").Count == 1);
        }

        [TestMethod]
        public void BuildMemoField_StartsWithMultilineChar_WhenTwoMemoFieldsHaveContent_Test()
        {
            postalShipment.Memo1 = "";
            string apiMemoText = UspsUtility.BuildMemoField(postalShipment);

            Debug.Assert(Regex.Matches(apiMemoText, "\x09").Count == 1);
        }

        [TestMethod]
        public void BuildMemoField_StartsWithMultilineChar_WhenThreeMemoFieldsHaveContent_Test()
        {
            string apiMemoText = UspsUtility.BuildMemoField(postalShipment);

            Debug.Assert(Regex.Matches(apiMemoText, "\x09").Count == 1);
        }

        [TestMethod]
        public void BuildMemoField_ReturnsTwoWrapChars_WhenThreeMemoFieldsHaveContent_Test()
        {
            string apiMemoText = UspsUtility.BuildMemoField(postalShipment);

            Debug.Assert(Regex.Matches(apiMemoText, "\r\n").Count == 2);
        }

        [TestMethod]
        public void BuildMemoField_ReturnsTwoWrapChars_WhenOnlyMemo3FieldHasContent_Test()
        {
            postalShipment.Memo1 = "";
            postalShipment.Memo2 = "";
            string apiMemoText = UspsUtility.BuildMemoField(postalShipment);

            Debug.Assert(Regex.Matches(apiMemoText, "\r\n").Count == 2);
        }

        [TestMethod]
        public void BuildMemoField_ReturnsOnlyUpTo200Chars_WhenMemoFieldsGreaterThan200CharsEach_Test()
        {
            postalShipment.Memo1 = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789_____";
            postalShipment.Memo2 = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789_____";
            postalShipment.Memo3 = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789_____";
            string apiMemoText = UspsUtility.BuildMemoField(postalShipment);

            Debug.Assert(apiMemoText.Length <= 200);
        }
    }
}
