using System;
using System.IO;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class OdbcFieldMapTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcFieldMapTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Save_DelegatesToOdbcFieldMapIOFactoryForOdbcFieldMapWriter()
        {
            Mock<IOdbcFieldMapWriter> odbcWriter = mock.Mock<IOdbcFieldMapWriter>();
            Mock<IOdbcFieldMapIOFactory> ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();
            ioFactory.Setup(f => f.CreateWriter()).Returns(odbcWriter.Object);
            OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();

            testObject.Save(new MemoryStream());

            ioFactory.Verify(f => f.CreateWriter());
        }

        [Fact]
        public void Save_DelegatesToOdbcFieldMapWriter()
        {
            Mock<IOdbcFieldMapWriter> odbcWriter = mock.Mock<IOdbcFieldMapWriter>();
            Mock<IOdbcFieldMapIOFactory> ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();
            ioFactory.Setup(f => f.CreateWriter()).Returns(odbcWriter.Object);
            OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();

            MemoryStream memoryStream = new MemoryStream();

            testObject.Save(memoryStream);

            odbcWriter.Verify(w => w.Write(testObject, memoryStream));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}