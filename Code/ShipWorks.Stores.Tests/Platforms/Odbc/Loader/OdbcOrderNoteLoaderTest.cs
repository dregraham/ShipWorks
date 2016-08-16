using System;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Loader
{

    public class OdbcOrderNoteLoaderTest
    {
        [Fact]
        public void Load_NoteFieldsDelegatesFindingEntriesToFieldMapWithCorrectParameters()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();

                var testObject = mock.Create<OdbcOrderNoteLoader>();

                testObject.Load(fieldMap.Object, new OrderEntity());

                fieldMap.Verify(
                    m =>
                        m.FindEntriesBy(
                            It.Is<EntityField2>(
                                f =>
                                    f.Name == NoteFields.Text.Name &&
                                    f.ContainingObjectName == NoteFields.Text.ContainingObjectName),
                            It.Is<bool>(b => !b)));
            }
        }

        [Fact]
        public void Test_PublicNoteLoaded_WhenMapNameContainsPublic()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOrderNote> note = mock.Mock<IOrderNote>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.SetupGet(f => f.DisplayName).Returns("this is a public note");
                shipworksField.SetupGet(f => f.Value).Returns("this is the value of the note");

                var fieldMapEntry = mock.Mock<IOdbcFieldMapEntry>();
                fieldMapEntry.SetupGet(e => e.ShipWorksField).Returns(shipworksField.Object);


                var fieldMap = mock.Mock<IOdbcFieldMap>();
                fieldMap.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), It.IsAny<bool>()))
                    .Returns(new[] {fieldMapEntry.Object});

                var testObject = mock.Create<OdbcOrderNoteLoader>();

                testObject.Load(fieldMap.Object, new OrderEntity());

                note.Verify(
                    n =>
                        n.Add(It.IsAny<OrderEntity>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                            It.Is<NoteVisibility>(v => v == NoteVisibility.Public)), Times.Once);
            }
        }

        [Fact]
        public void Test_InternalNoteLoaded_WhenMapNameContainsPrivate()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOrderNote> note = mock.Mock<IOrderNote>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.SetupGet(f => f.DisplayName).Returns("this is a private note");
                shipworksField.SetupGet(f => f.Value).Returns("this is the value of the note");

                var fieldMapEntry = mock.Mock<IOdbcFieldMapEntry>();
                fieldMapEntry.SetupGet(e => e.ShipWorksField).Returns(shipworksField.Object);


                var fieldMap = mock.Mock<IOdbcFieldMap>();
                fieldMap.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), It.IsAny<bool>()))
                    .Returns(new[] { fieldMapEntry.Object });

                var testObject = mock.Create<OdbcOrderNoteLoader>();

                testObject.Load(fieldMap.Object, new OrderEntity());

                note.Verify(
                    n =>
                        n.Add(It.IsAny<OrderEntity>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                            It.Is<NoteVisibility>(v => v == NoteVisibility.Internal)), Times.Once);
            }
        }
    }
}
