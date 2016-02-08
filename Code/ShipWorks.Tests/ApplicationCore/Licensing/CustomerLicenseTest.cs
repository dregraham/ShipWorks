using System.Xml;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using Xunit;
using log4net;
using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class CustomerLicenseTest
    {
        [Fact]
        public void ActivateThrows_ShipWorksLicenseException_IfTangoActivationFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ITangoWebClient>().Setup(w => w.ActivateLicense(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new GenericResult<ActivationResponse>(null)
                    {
                        Success = false,
                        Message = "something went wrong",
                        Context = null
                    });

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "someKey"));

                ShipWorksLicenseException ex =
                    Assert.Throws<ShipWorksLicenseException>(
                        () => customerLicense.Activate("some@email.com", "randompassword"));
                Assert.Equal("something went wrong", ex.Message);
            }
        }

        [Fact]
        public void ActivateCalls_ITangoWebClient_WithUsernamePassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ITangoWebClient> tangoWebClient = mock.Mock<ITangoWebClient>();

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

                tangoWebClient.Setup(w => w.ActivateLicense(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new GenericResult<ActivationResponse>(null)
                    {
                        Context = activationResponse,
                        Success = true,
                    });

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "someKey"));
                customerLicense.Activate("some@email.com", "randompassword");

                tangoWebClient.Verify(w => w.ActivateLicense("some@email.com", "randompassword"), Times.Once);
            }
        }

        [Fact]
        public void ActivateCalls_LicenseWriterSave_WithCustomerLicense()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ITangoWebClient> tangoWebClient = mock.Mock<ITangoWebClient>();

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

                tangoWebClient.Setup(w => w.ActivateLicense(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new GenericResult<ActivationResponse>(null)
                    {
                        Context = activationResponse,
                        Success = true,
                    });

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "someKey"));

                Mock<ICustomerLicenseWriter> licenseWriter = mock.Mock<ICustomerLicenseWriter>();

                customerLicense.Activate("some@email.com", "randompassword");

                licenseWriter.Verify(w => w.Write(customerLicense), Times.Once);
            }
        }

        [Fact]
        public void Activate_SetsKey_ActivationResponse()
        {
            using (var mock = AutoMock.GetLoose())
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

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.ActivateLicense(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new GenericResult<ActivationResponse>(null)
                    {
                        Success = true,
                        Context = activationResponse
                    });

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "someKey"));

                customerLicense.Activate("foo@bar.com", "baz");

                Assert.Equal(
                    "38-75-5D-E7-88-FA-E9-DE-D6-72-8D-FC-62-4F-DD-6A-AE-C3-1D-6F-3F-DD-7E-69-62-F2-60-43-91-D8-A2-1B-BA-5E-A7-1E-20-34-9D-E6-01-E4-50-8B-F8-16-D0-00-05-79-89-AE-16-8E-6B-4B-F4-F1-46-8A-C3-AD-58-05-FA-F9-D6-EE-D7-AB-25-9D-2B-5B-FF-78-2C-FC-AB-8B-62-6A-F5-44-BC-E6-A7-AC-0A-39-8B-27-AE-F5-FC-0A-8D-06-42-1E-B8-DE-94-26-E6-37-93-91-51-A3-40-37-E4-35-9F-C3-41-62-9E-B4-9E-2D-B3-A4-66-5E-AB-E6-6B-E4-87-39-CE-F3-B2-C0-30-88-9C-A5-C3-99-29-C5-40-5C-10-DE-57-5F-96-16-0B-12-4D-87-D3-E7-B2-36-1A-7C-9F-9E-94-DA-8B-4E-2B-1B-3C-B9-83-40-E9-4A-4F-D4-4F-89-67-6D-64-6A-9D-6B-73-13-83-18-C9-8D-C4-F6-8E-15-97-02-E8-1F-CA-93-3F-7E-B7-74-68-76-F4-DA-F5-AE-D3-BF-1A-03-12-13-BF-10-0F-41-87-02-35-BB-D8-8B-0E-D7-D0-13-C9-F5-D2-A6-56-55-29-16-4C-31-7C-E5-AE-62-1B-BE-D4-3A-23-E0-78-28-37-B0-00-90-2F-2E-0A-7E-16-1F-98-3B-69-19-4B-D4-2B-0F-BA-B1-0D-4B-CD-76-29-42-9B-A1-8F-65-6A-6A-EC-5D-85-3C-BB-78-6E-26-7B-5A-9C-51-8E-10-31-F1-CB-98-10-D4-65-F8-5F-51-12-83-A4-76-53-C2-F7-5E-CD-9E-9C-F8-63-9D-B0-B4-8C-1C-74-68-48-2C-65-3F-62-2A-B7-4C-56-32-82-92-21-72-F3-70-8E-EA-71-29-AB-26",
                    customerLicense.Key);
            }
        }

        [Fact]
        public void Refresh_SetsLicenseCapabilities_GetLicenseCapabilitiesResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LicenseCapabilities licenseResponse = new LicenseCapabilities(new XmlDocument());

                var tangoWebClient =
                    mock.Mock<ITangoWebClient>();

                tangoWebClient.Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseResponse);

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                customerLicense.Refresh();

                tangoWebClient.Verify(wc => wc.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()), Times.Once);
            }
        }

        [Fact]
        public void Refresh_SetsDisabledReason_GetLicenseCapabilitiesResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Throws(new TangoException("Disabled for some reason"));

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                customerLicense.Refresh();

                Assert.Equal(customerLicense.DisabledReason, "Disabled for some reason");
            }
        }

        [Fact]
        public void Refresh_LogsDisabledReason_ActivationResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var log = mock.Mock<ILog>();

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Throws(new TangoException("Disabled for some reason"));

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                customerLicense.Refresh();

                log.Verify(l => l.Warn(It.IsAny<TangoException>()), Times.Once);
            }
        }

        [Fact]
        public void Save_RethrowsException_FromLicenseWriter()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var writer = mock.Mock<ICustomerLicenseWriter>();

                writer.Setup(w => w.Write(It.IsAny<ICustomerLicense>())).Throws(new Exception("some random exception"));

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var ex = Assert.Throws<Exception>(() => customerLicense.Save());
                Assert.Equal("some random exception", ex.Message);
            }
        }

        [Fact]
        public void IsOverChannelLimit_ReturnsTrue_WhenActiveChannelsExceedChannelLimitAndNotInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(false);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();

                Assert.True(testObject.IsOverChannelLimit);
            }
        }

        [Fact]
        public void IsOverChannelLimit_ReturnsFalse_WhenActiveChannelsExceedChannelLimitAndInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(true);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();

                Assert.False(testObject.IsOverChannelLimit);
            }
        }

        [Fact]
        public void IsOverChannelLimit_ReturnsFalse_WhenActiveChannelsLessThanChannelLimitAndNotInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.ChannelLimit)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(false);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();
                Assert.False(testObject.IsOverChannelLimit);
            }
        }

        [Fact]
        public void NumberOfChannelsOverLimit_Returns0_WhenActiveChannelsLessThanChannelLimitAndNotInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.ChannelLimit)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(false);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();
                Assert.Equal(0, testObject.NumberOfChannelsOverLimit);
            }
        }

        [Fact]
        public void NumberOfChannelsOverLimit_Returns0_WhenActiveChannelsMoreThanChannelLimitAndInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(10);

                licenseCapabilities
                    .Setup(lc => lc.ChannelLimit)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(true);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();
                Assert.Equal(0, testObject.NumberOfChannelsOverLimit);
            }
        }

        [Fact]
        public void NumberOfChannelsOverLimit_ReturnsNumberOverLimit_WhenActiveChannelsMoreThanChannelLimitAndNotInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(10);

                licenseCapabilities
                    .Setup(lc => lc.ChannelLimit)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(false);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();
                Assert.Equal(5, testObject.NumberOfChannelsOverLimit);
            }
        }

        [Fact]
        public void EnforceChannelLimit_DialogShown_WhenLicenseOverChannelLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                Mock<IChannelLimitDlg> channelLimitDlg = mock.Mock<IChannelLimitDlg>();

                mock.Mock<IChannelLimitDlgFactory>()
                    .Setup(c => c.GetChannelLimitDlg(It.IsAny<ICustomerLicense>()))
                    .Returns(channelLimitDlg.Object);
                
                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceChannelLimit();

                channelLimitDlg.Verify(d => d.ShowDialog(), Times.Once);
            }
        }

        [Fact]
        public void EnforceChannelLimit_DialogNotShown_WhenUnderChannelLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(0);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                var channelLimitDlg = mock.Mock<IChannelLimitDlg>();

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceChannelLimit();

                channelLimitDlg.Verify(d => d.ShowDialog(), Times.Never);
            }
        }

        [Fact]
        public void EnforceChannelLimit_RefreshIsCalled_WhenOverChannelLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                Mock<ITangoWebClient> webClient = MockWebClientToReturnCapabilities(mock, licenseCapabilities);

                Mock<IChannelLimitDlg> channelLimitDlg = mock.Mock<IChannelLimitDlg>();

                mock.Mock<IChannelLimitDlgFactory>()
                    .Setup(c => c.GetChannelLimitDlg(It.IsAny<ICustomerLicense>()))
                    .Returns(channelLimitDlg.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceChannelLimit();

                // The only way to verify that Refresh is called is by making sure
                // GetLicenseCapabilities is called because that is all refresh does.
                webClient.Verify(d => d.GetLicenseCapabilities(testObject), Times.Once);
            }
        }

        private static Mock<ITangoWebClient> MockWebClientToReturnCapabilities(AutoMock mock, Mock<ILicenseCapabilities> licenseCapabilities)
        {
            var webClient = mock.Mock<ITangoWebClient>();
            webClient
                .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                .Returns(licenseCapabilities.Object);
            return webClient;
        }

        [Fact]
        public void IsShipmentLimitReached_IsTrue_WhenOverShipmentLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(c => c.ProcessedShipments)
                    .Returns(101);

                licenseCapabilities.Setup(c => c.ShipmentLimit)
                    .Returns(100);

                MockWebClientToReturnCapabilities(mock, licenseCapabilities);
                
                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();

                Assert.True(testObject.IsShipmentLimitReached);
            }
        }

        [Fact]
        public void IsShipmentLimitReached_IsTrue_WhenAtShipmentLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(c => c.ProcessedShipments)
                    .Returns(100);

                licenseCapabilities.Setup(c => c.ShipmentLimit)
                    .Returns(100);

                MockWebClientToReturnCapabilities(mock, licenseCapabilities);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();

                Assert.True(testObject.IsShipmentLimitReached);
            }
        }

        [Fact]
        public void IsShipmentLimitReached_IsFalse_WhenAtShipmentLimitAndInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(c => c.ProcessedShipments)
                    .Returns(100);

                licenseCapabilities.Setup(c => c.ShipmentLimit)
                    .Returns(100);

                licenseCapabilities.Setup(c => c.IsInTrial)
                    .Returns(true);

                MockWebClientToReturnCapabilities(mock, licenseCapabilities);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();

                Assert.False(testObject.IsShipmentLimitReached);
            }
        }

        [Fact]
        public void EnforceShipmentLimit_CallsShowDialog_WhenShipmentLimitReached()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                MockWebClientToReturnCapabilities(mock, licenseCapabilities);

                var dlg = mock.Mock<IDialog>();

                mock.Mock<IUpgradePlanDlgFactory>()
                    .Setup(f => f.Create(It.IsAny<string>()))
                    .Returns(dlg.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                
                testObject.EnforceShipmentLimit();

                dlg.Verify(d=>d.ShowDialog(), Times.Once);
            }
        }

        [Fact]
        public void EnforceShipmentLimit_DoesNotCallShowDialog_WhenShipmentLimitNotReached()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                
                licenseCapabilities.Setup(c => c.ShipmentLimit)
                    .Returns(100);

                MockWebClientToReturnCapabilities(mock, licenseCapabilities);

                var dlg = mock.Mock<IDialog>();

                mock.Mock<IUpgradePlanDlgFactory>()
                    .Setup(f => f.Create(It.IsAny<string>()))
                    .Returns(dlg.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceShipmentLimit();

                dlg.Verify(d => d.ShowDialog(), Times.Never);
            }
        }

        [Fact]
        public void Activate_ReturnsActive_WhenTangoReturnsSuccess()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var response = mock.Mock<IAddStoreResponse>();
                response.SetupGet(r => r.Success)
                    .Returns(true);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.AddStore(It.IsAny<CustomerLicense>(), It.IsAny<StoreEntity>()))
                    .Returns(response.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var result = testObject.Activate(new StoreEntity());

                Assert.Equal(LicenseActivationState.Active, result.Value);
            }
        }

        [Fact]
        public void Activate_ReturnsInvalid_WhenTangoReturnsFailedResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var response = mock.Mock<IAddStoreResponse>();
                response.SetupGet(r => r.Success)
                    .Returns(false);

                response.SetupGet(r => r.Error)
                    .Returns("blah");

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.AddStore(It.IsAny<CustomerLicense>(), It.IsAny<StoreEntity>()))
                    .Returns(response.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var result = testObject.Activate(new StoreEntity());

                Assert.Equal(LicenseActivationState.Invalid, result.Value);
            }
        }

        [Fact]
        public void Activate_ReturnsOverChannelLimit_WhenTangoReturnsOverChannelLimitResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var response = mock.Mock<IAddStoreResponse>();
                response.SetupGet(r => r.Success)
                    .Returns(false);

                response.SetupGet(r => r.Error)
                    .Returns("OverChannelLimit");

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.AddStore(It.IsAny<CustomerLicense>(), It.IsAny<StoreEntity>()))
                    .Returns(response.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var result = testObject.Activate(new StoreEntity());

                Assert.Equal(LicenseActivationState.OverChannelLimit, result.Value);
            }
        }

    }
}