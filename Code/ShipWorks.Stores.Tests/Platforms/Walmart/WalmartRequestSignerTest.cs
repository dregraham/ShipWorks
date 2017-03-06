using System;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartRequestSignerTest
    {
        private const string Epoch = "1488833232874";
        private const string PrivateKey =
            "MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAKe2qAcodl1QgaZMigrBci1B9G38WBraSoUtoD+nOqFz8Jyzw/p+VOyrizTpODH5a/JftWjDUuWbg5k9GlEv+4ALSOSSRJx5q5ZBoOhtTJNCA3HrOg81dgM0HzIUl4h6YxpKw28B9DzkTn4RfEQxlxJ8eOPFBjkuNmHo0a7859OnAgMBAAECgYB0b//gWFs1FfutNV5xcTSP70aARb31hrBOHgsvpi6ygQgAA16Avsy/M6oGJhT5vS0QrRoJjfIzrvCCp0VqMGHut9FNTcUwCdR+85sLLPF+CogjFnUs9qnvCtWtRaVG4P42K9zl9PkPqQEncYVG/iwWovk6lEtOpUloDs2DuN3aoQJBANnzRh0EphY9mhzHnx1HBTBjOvKlwPUVLFR2XVWOsHyBVKkr/RWPmAM2XclzF8ECsQEMYm21WlxsHhEhM1KV16MCQQDE/i3HOS2Zkyjcng8Lwj8qGXzbxVNtu6l0CL+1GGReRE5F3Uq1U+V7HgUf7U8ULV5b8H3BDwC1nOir7gs8qiQtAkEAt3ytvGRbh0HZav1MIZPW9IO17u5I4owuw/TaYts8DbW8Fqhn6yz2p02v65cvmlivt9g7TW1uY3zKW1V+JbrszQJBAJhadRFFmYzTEaE+5SgU/UEUIUrfnBycLPw+3/Wxfb6iWV8TPPpsfmjv2MrOgIB8biPxJXEwpz3Osux12F78v6kCQCE9PmgcM0KrzWezvvYCKIusn5RdaoV35jFqb2D6sdImXt5GrHLjTRwvBJbCABW8eTrfMcMHWOjyAdUMV0p2TA0=";
        private const string ConsumerID = "8108ba7e-ed95-401b-af51-44b05f4b6044";
        private const string TestConnectionUrl = "https://marketplace.walmartapis.com/v3/feeds";

        [Fact]
        public void Sign_ThrowsWalmartException_WhenPrivateKeyFailsToLoad()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                WalmartRequestSigner testObject = mock.Create<WalmartRequestSigner>();
                HttpRequestSubmitter submitter = new HttpXmlVariableRequestSubmitter();

                Assert.Throws<WalmartException>(() => testObject.Sign(submitter, new WalmartStoreEntity(), Epoch));
            }
        }

        [Fact]
        public void Sign_AddsTimestampToHeader()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(PrivateKey);

                mock.Mock<IEncryptionProviderFactory>()
                    .Setup(x => x.CreateWalmartEncryptionProvider())
                    .Returns(encryptionProvider);

                WalmartRequestSigner testObject = mock.Create<WalmartRequestSigner>();
                HttpRequestSubmitter submitter = new HttpXmlVariableRequestSubmitter()
                {
                    Verb = HttpVerb.Get,
                    Uri = new Uri(TestConnectionUrl)
                };
                WalmartStoreEntity store = new WalmartStoreEntity()
                {
                    PrivateKey = PrivateKey,
                    ConsumerID = ConsumerID
                };

                testObject.Sign(submitter, store, Epoch);

                Assert.Equal(Epoch, submitter.Headers["WM_SEC.TIMESTAMP"]);
            }
        }

        [Fact]
        public void Sign_AddsSignatureToHeader()
        {
            string actualSignature =
                "gzskHYn6gD9jtFuJ/9aLNuItpsgblBpn5RAqH5S9GP6QopcbJkz/7Hc4fVVKSgoGAyBSmgqwsnU1tNbENkyFtUfX8g8UNx070aTIdP09o2FGA2/TLQIXfxDxavx5JkdvxMOZ44n0fDzp9l7Xf9V/ycRBAJjxU6R8uvUt3nfq9eY=";

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var encryptionProvider = mock.Mock<IEncryptionProvider>();
                encryptionProvider.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(PrivateKey);

                mock.Mock<IEncryptionProviderFactory>()
                    .Setup(x => x.CreateWalmartEncryptionProvider())
                    .Returns(encryptionProvider);

                WalmartRequestSigner testObject = mock.Create<WalmartRequestSigner>();
                HttpRequestSubmitter submitter = new HttpXmlVariableRequestSubmitter()
                {
                    Verb = HttpVerb.Get,
                    Uri = new Uri(TestConnectionUrl)
                };
                WalmartStoreEntity store = new WalmartStoreEntity()
                {
                    PrivateKey = PrivateKey,
                    ConsumerID = ConsumerID
                };

                testObject.Sign(submitter, store, Epoch);

                Assert.Equal(actualSignature, submitter.Headers["WM_SEC.AUTH_SIGNATURE"]);
            }
        }
    }
}