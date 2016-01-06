using System;
using System.Xml;
using Autofac.Extras.Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class ActivationResponseTest
    {
        [Fact]
        public void ActivationResponse_WithNull_XmlDocument_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ActivationResponse(null));
        }

        [Fact]
        public void ActivationResponse_SetsKey_FromXmlDocument()
        {
            XmlDocument responseData = new XmlDocument();
            responseData.LoadXml(@"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
                                       <s:Body>
                                          <GetCustomerLicenseInfoResponse xmlns=""http://stamps.com/xml/namespace/2015/09/shipworks/activationv1"">
                                             <GetCustomerLicenseInfoResult xmlns:a=""http://schemas.datacontract.org/2004/07/Sdc.Server.ShipWorksNet.Protocol.CustomerLicenseInfo"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
                                                <a:AssociatedStampsUserName/>
                                                <a:CustomerLicenseKey>38-75-5D-E7-88-FA-E9-DE-D6-72-8D-FC-62-4F-DD-6A-AE-C3-1D-6F-3F-DD-7E-69-62-F2-60-43-91-D8-A2-1B-BA-5E-A7-1E-20-34-9D-E6-01-E4-50-8B-F8-16-D0-00-05-79-89-AE-16-8E-6B-4B-F4-F1-46-8A-C3-AD-58-05-FA-F9-D6-EE-D7-AB-25-9D-2B-5B-FF-78-2C-FC-AB-8B-62-6A-F5-44-BC-E6-A7-AC-0A-39-8B-27-AE-F5-FC-0A-8D-06-42-1E-B8-DE-94-26-E6-37-93-91-51-A3-40-37-E4-35-9F-C3-41-62-9E-B4-9E-2D-B3-A4-66-5E-AB-E6-6B-E4-87-39-CE-F3-B2-C0-30-88-9C-A5-C3-99-29-C5-40-5C-10-DE-57-5F-96-16-0B-12-4D-87-D3-E7-B2-36-1A-7C-9F-9E-94-DA-8B-4E-2B-1B-3C-B9-83-40-E9-4A-4F-D4-4F-89-67-6D-64-6A-9D-6B-73-13-83-18-C9-8D-C4-F6-8E-15-97-02-E8-1F-CA-93-3F-7E-B7-74-68-76-F4-DA-F5-AE-D3-BF-1A-03-12-13-BF-10-0F-41-87-02-35-BB-D8-8B-0E-D7-D0-13-C9-F5-D2-A6-56-55-29-16-4C-31-7C-E5-AE-62-1B-BE-D4-3A-23-E0-78-28-37-B0-00-90-2F-2E-0A-7E-16-1F-98-3B-69-19-4B-D4-2B-0F-BA-B1-0D-4B-CD-76-29-42-9B-A1-8F-65-6A-6A-EC-5D-85-3C-BB-78-6E-26-7B-5A-9C-51-8E-10-31-F1-CB-98-10-D4-65-F8-5F-51-12-83-A4-76-53-C2-F7-5E-CD-9E-9C-F8-63-9D-B0-B4-8C-1C-74-68-48-2C-65-3F-62-2A-B7-4C-56-32-82-92-21-72-F3-70-8E-EA-71-29-AB-26</a:CustomerLicenseKey>
                                                <a:IsLegacyUser>false</a:IsLegacyUser>
                                                <a:StampsUserName>mh_sw_b__0</a:StampsUserName>
                                             </GetCustomerLicenseInfoResult>
                                          </GetCustomerLicenseInfoResponse>
                                       </s:Body>
                                    </s:Envelope>");

            ActivationResponse activationResponse = new ActivationResponse(responseData);

            Assert.Equal("38-75-5D-E7-88-FA-E9-DE-D6-72-8D-FC-62-4F-DD-6A-AE-C3-1D-6F-3F-DD-7E-69-62-F2-60-43-91-D8-A2-1B-BA-5E-A7-1E-20-34-9D-E6-01-E4-50-8B-F8-16-D0-00-05-79-89-AE-16-8E-6B-4B-F4-F1-46-8A-C3-AD-58-05-FA-F9-D6-EE-D7-AB-25-9D-2B-5B-FF-78-2C-FC-AB-8B-62-6A-F5-44-BC-E6-A7-AC-0A-39-8B-27-AE-F5-FC-0A-8D-06-42-1E-B8-DE-94-26-E6-37-93-91-51-A3-40-37-E4-35-9F-C3-41-62-9E-B4-9E-2D-B3-A4-66-5E-AB-E6-6B-E4-87-39-CE-F3-B2-C0-30-88-9C-A5-C3-99-29-C5-40-5C-10-DE-57-5F-96-16-0B-12-4D-87-D3-E7-B2-36-1A-7C-9F-9E-94-DA-8B-4E-2B-1B-3C-B9-83-40-E9-4A-4F-D4-4F-89-67-6D-64-6A-9D-6B-73-13-83-18-C9-8D-C4-F6-8E-15-97-02-E8-1F-CA-93-3F-7E-B7-74-68-76-F4-DA-F5-AE-D3-BF-1A-03-12-13-BF-10-0F-41-87-02-35-BB-D8-8B-0E-D7-D0-13-C9-F5-D2-A6-56-55-29-16-4C-31-7C-E5-AE-62-1B-BE-D4-3A-23-E0-78-28-37-B0-00-90-2F-2E-0A-7E-16-1F-98-3B-69-19-4B-D4-2B-0F-BA-B1-0D-4B-CD-76-29-42-9B-A1-8F-65-6A-6A-EC-5D-85-3C-BB-78-6E-26-7B-5A-9C-51-8E-10-31-F1-CB-98-10-D4-65-F8-5F-51-12-83-A4-76-53-C2-F7-5E-CD-9E-9C-F8-63-9D-B0-B4-8C-1C-74-68-48-2C-65-3F-62-2A-B7-4C-56-32-82-92-21-72-F3-70-8E-EA-71-29-AB-26", activationResponse.Key);
        }

        [Fact]
        public void ActivationResponse_WithInvalidXml_SetsKeyToEmptyString()
        {
            XmlDocument responseData = new XmlDocument();
            responseData.LoadXml(@"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
                                       <s:Body>
                                          <GetCustomerLicenseInfoResponse xmlns=""http://stamps.com/xml/namespace/2015/09/shipworks/activationv1"">
                                             <GetCustomerLicenseInfoResult xmlns:a=""http://schemas.datacontract.org/2004/07/Sdc.Server.ShipWorksNet.Protocol.CustomerLicenseInfo"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
                                                <a:AssociatedStampsUserName/>
                                                <a:IsLegacyUser>false</a:IsLegacyUser>
                                                <a:StampsUserName>mh_sw_b__0</a:StampsUserName>
                                             </GetCustomerLicenseInfoResult>
                                          </GetCustomerLicenseInfoResponse>
                                       </s:Body>
                                    </s:Envelope>");

            ActivationResponse activationResponse = new ActivationResponse(responseData);

            Assert.Equal(string.Empty, activationResponse.Key);
        }
    }
}