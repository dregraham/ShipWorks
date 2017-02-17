using Interapptive.Shared.Business;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Xunit;

namespace Interapptive.Shared.Tests.Business
{
    public class PersonAdapterTest
    {
        [Fact]
        public void ParsedName_GetsValues_AsPersonName()
        {
            var person = new PersonAdapter
            {
                FirstName = "Foo",
                MiddleName = "Baz",
                LastName = "Bar",
                UnparsedName = "Foo Baz Bar",
                NameParseStatus = PersonNameParseStatus.Simple
            };

            PersonName name = person.ParsedName;

            Assert.Equal("Foo", name.First);
            Assert.Equal("Baz", name.Middle);
            Assert.Equal("Bar", name.Last);
            Assert.Equal("Foo Baz Bar", name.UnparsedName);
            Assert.Equal(PersonNameParseStatus.Simple, name.ParseStatus);
        }

        [Fact]
        public void ParsedName_SetsValues_FromPersonName()
        {
            var name = new PersonName
            {
                First = "Foo",
                Middle = "Baz",
                Last = "Bar",
                UnparsedName = "Foo Baz Bar",
                ParseStatus = PersonNameParseStatus.Simple
            };

            PersonAdapter person = new PersonAdapter
            {
                ParsedName = name
            };

            Assert.Equal("Foo", person.FirstName);
            Assert.Equal("Baz", person.MiddleName);
            Assert.Equal("Bar", person.LastName);
            Assert.Equal("Foo Baz Bar", person.UnparsedName);
            Assert.Equal(PersonNameParseStatus.Simple, person.NameParseStatus);
        }

        [Fact]
        public void ParsedName_SetsValuesToEmpty_WhenParsedNameIsNull()
        {
            PersonAdapter person = new PersonAdapter
            {
                ParsedName = null
            };

            Assert.Empty(person.FirstName);
            Assert.Empty(person.MiddleName);
            Assert.Empty(person.LastName);
            Assert.Empty(person.UnparsedName);
            Assert.Equal(PersonNameParseStatus.Unknown, person.NameParseStatus);
        }

        [Fact]
        public void SetField_DoesNotDelegateToSetNewFieldValue_WhenValueHasNotChanged()
        {
            Mock<IEntity2> entity = new Mock<IEntity2>();
            Mock<IEntityField2> field = new Mock<IEntityField2>();

            field.Setup(f => f.CurrentValue).Returns("1");

            entity.Setup(e => e.Fields["FirstName"]).Returns(field.Object);

            PersonAdapter personAdapter = new PersonAdapter(entity.Object, "");

            entity.Reset();
            personAdapter.FirstName = "1";

            entity.Verify(e => e.SetNewFieldValue("FirstName", It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void SetField_DoesDelegateToSetNewFieldValue_WhenValueHasNotChanged()
        {
            Mock<IEntity2> entity = new Mock<IEntity2>();
            Mock<IEntityField2> field = new Mock<IEntityField2>();

            field.Setup(f => f.CurrentValue).Returns("1");

            entity.Setup(e => e.Fields["FirstName"]).Returns(field.Object);

            PersonAdapter personAdapter = new PersonAdapter(entity.Object, "");

            entity.Reset();
            personAdapter.FirstName = "2";

            entity.Verify(e => e.SetNewFieldValue("FirstName", It.IsAny<string>()), Times.Once);
        }
    }
}
