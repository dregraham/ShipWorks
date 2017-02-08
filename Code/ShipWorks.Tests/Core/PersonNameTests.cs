using Interapptive.Shared.Business;
using Xunit;

namespace ShipWorks.Tests.Core
{
    public class PersonNameTests
    {
        [Theory]
        [InlineData("", "", "", "", PersonNameParseStatus.Unknown)]
        [InlineData("Small Arms Shop", "Small", "Arms", "Shop", PersonNameParseStatus.Simple)]
        [InlineData("John Randolph D'oe", "John", "Randolph", "D'oe", PersonNameParseStatus.Simple)]
        [InlineData("Joe Smith", "Joe", "", "Smith", PersonNameParseStatus.Simple)]
        [InlineData("John D'oe", "John", "", "D'oe", PersonNameParseStatus.Simple)]

        public void Constructor_WithFirstMiddleLast_ParsedValuesAreCorrect(string fullName, string first, string middle, string last, PersonNameParseStatus nameParseStatus)
        {
            PersonName name = new PersonName(first, middle, last);

            Assert.Equal(first, name.First);
            Assert.Equal(middle, name.Middle);
            Assert.Equal(last, name.Last);
            Assert.Equal(fullName, name.FullName);
            Assert.Equal(fullName, name.UnparsedName);
            Assert.Equal(nameParseStatus, name.ParseStatus);
        }
        [Theory]
        [InlineData("", "", "", "", PersonNameParseStatus.Unknown)]
        [InlineData("Small Arms Shop", "Small", "Arms", "Shop", PersonNameParseStatus.Simple)]
        [InlineData("John Randolph D'oe", "John", "Randolph", "D'oe", PersonNameParseStatus.Simple)]
        [InlineData("Joe Smith", "Joe", "", "Smith", PersonNameParseStatus.Simple)]
        [InlineData("John D'oe", "John", "", "D'oe", PersonNameParseStatus.Simple)]

        public void Constructor_WithPersonAdapter_ParsedValuesAreCorrect(string fullName, string first, string middle, string last, PersonNameParseStatus nameParseStatus)
        {
            PersonAdapter personAdapter = new PersonAdapter()
            {
                FirstName = first,
                MiddleName = middle,
                LastName = last
            };

            PersonName name = new PersonName(personAdapter);

            Assert.Equal(first, name.First);
            Assert.Equal(middle, name.Middle);
            Assert.Equal(last, name.Last);
            Assert.Equal(fullName, name.FullName);
            Assert.Equal(fullName, name.UnparsedName);
            Assert.Equal(nameParseStatus, name.ParseStatus);
        }

        [Fact]
        public void SmallArmsShopTest()
        {
            PersonName name = PersonName.Parse("Small Arms Shop");

            Assert.Equal("Small", name.First);
            Assert.Equal("Arms", name.Middle);
            Assert.Equal("Shop", name.Last);
        }

        [Fact]
        public void SeargentName()
        {
            PersonName name = PersonName.Parse("Sgt John Doe");

            Assert.Equal("John", name.First);
            Assert.Equal("Doe", name.Last);
            Assert.Equal("Sgt John Doe", name.FullName);
        }

        [Fact]
        public void DrFullName()
        {
            PersonName name = PersonName.Parse("Dr. John Smith");

            Assert.Equal("John", name.First);
            Assert.Equal("Smith", name.Last);
            Assert.Equal("Dr. John Smith", name.FullName);
        }

        [Fact]
        public void DrWhenSetAsPrefix()
        {
            PersonName name = PersonName.Parse("John Smith");
            name.Prefix = "Dr.";

            Assert.Equal("Dr.", name.Prefix);
            Assert.Equal("John", name.First);
            Assert.Equal("Smith", name.Last);
            Assert.Equal("Dr. John Smith", name.FullName);
        }

        [Fact]
        public void DisplayFirstLast()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Last = "Smith";

            Assert.Equal("Joe Smith", name.FullName);
        }

        [Fact]
        public void DisplayFirstLastMiddle()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Middle = "Wonderball";
            name.Last = "Smith";

            Assert.Equal("Joe Wonderball Smith", name.FullName);
        }

        [Fact]
        public void DisplayFirstLastSuffix()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Last = "Smith";
            name.Suffix = "Jr.";

            Assert.Equal("Joe Smith Jr.", name.FullName);
        }

        [Fact]
        public void DisplayFirstLastMiddleSuffix()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Middle = "Marco";
            name.Last = "Smith";
            name.Suffix = "Jr.";

            Assert.Equal("Joe Marco Smith Jr.", name.FullName);
        }

        [Fact]
        public void ParseFirstLast()
        {
            PersonName name = PersonName.Parse("John D'oe");

            Assert.Equal(string.Empty, name.Prefix);
            Assert.Equal("John", name.First);
            Assert.Equal(string.Empty, name.Middle);
            Assert.Equal("D'oe", name.Last);
            Assert.Equal(string.Empty, name.Suffix);
            Assert.Equal(PersonNameParseStatus.Simple, name.ParseStatus);
        }

        [Fact]
        public void ParseFirstLastMiddle()
        {
            PersonName name = PersonName.Parse("John Randolph D'oe");

            Assert.Equal(string.Empty, name.Prefix);
            Assert.Equal("John", name.First);
            Assert.Equal("Randolph", name.Middle);
            Assert.Equal("D'oe", name.Last);
            Assert.Equal(string.Empty, name.Suffix);
            Assert.Equal(PersonNameParseStatus.Simple, name.ParseStatus);
        }

        [Fact]
        public void ParseFirstLastMiddleSuffix()
        {
            PersonName name = PersonName.Parse("John Randolph D'oe III");

            Assert.Equal(string.Empty, name.Prefix);
            Assert.Equal("John", name.First);
            Assert.Equal("Randolph", name.Middle);
            Assert.Equal("D'oe", name.Last);
            Assert.Equal("III", name.Suffix);
            Assert.Equal(PersonNameParseStatus.Simple, name.ParseStatus);
        }

        [Fact]
        public void ParseFirstLastSuffix()
        {
            PersonName name = PersonName.Parse("John D'oe Jr");

            Assert.Equal(string.Empty, name.Prefix);
            Assert.Equal("John", name.First);
            Assert.Equal(string.Empty, name.Middle);
            Assert.Equal("D'oe", name.Last);
            Assert.Equal("Jr", name.Suffix);
            Assert.Equal(PersonNameParseStatus.Simple, name.ParseStatus);
        }

        [Fact]
        public void ParsePrefixFirstLast()
        {
            PersonName name = PersonName.Parse("Mr. John D'oe");

            Assert.Equal("Mr.", name.Prefix);
            Assert.Equal("John", name.First);
            Assert.Equal(string.Empty, name.Middle);
            Assert.Equal("D'oe", name.Last);
            Assert.Equal(string.Empty, name.Suffix);
            Assert.Equal(PersonNameParseStatus.PrefixFound, name.ParseStatus);
        }

        [Fact]
        public void ParsePrefixFirstLastMiddle()
        {
            PersonName name = PersonName.Parse("dr John Randolph D'oe");

            Assert.Equal("dr", name.Prefix);
            Assert.Equal("John", name.First);
            Assert.Equal("Randolph", name.Middle);
            Assert.Equal("D'oe", name.Last);
            Assert.Equal(string.Empty, name.Suffix);
            Assert.Equal(PersonNameParseStatus.PrefixFound, name.ParseStatus);
        }

        [Fact]
        public void ParsePrefixFirstLastMiddleSuffix()
        {
            PersonName name = PersonName.Parse("Miss John Randolph D'oe, ii");

            Assert.Equal("Miss", name.Prefix);
            Assert.Equal("John", name.First);
            Assert.Equal("Randolph", name.Middle);
            Assert.Equal("D'oe", name.Last);
            Assert.Equal("ii", name.Suffix);
            Assert.Equal(PersonNameParseStatus.PrefixFound, name.ParseStatus);
        }

        [Fact]
        public void ParsePrefixFirstLastMiddleAndSuffixWithTwoWords()
        {
            PersonName name = PersonName.Parse("Miss John Randolph D'oe, The ii");

            Assert.Equal("", name.Prefix);
            Assert.Equal("Miss John Randolph D'oe", name.First);
            Assert.Equal("", name.Middle);
            Assert.Equal("The ii", name.Last);
            Assert.Equal("", name.Suffix);
            Assert.Equal(PersonNameParseStatus.Unparsed, name.ParseStatus);
        }

        [Fact]
        public void ToString_Matches_Fullname()
        {
            PersonName name = PersonName.Parse("Miss John Randolph D'oe, The ii");

            Assert.Equal("Miss John Randolph D'oe, The ii", name.ToString());
        }

        [Fact]
        public void ParseFirstLastMiddleAndBlankPrefix()
        {
            PersonName name = PersonName.Parse(", The ii");

            Assert.Equal("", name.Prefix);
            Assert.Equal("The", name.First);
            Assert.Equal("", name.Middle);
            Assert.Equal("", name.Last);
            Assert.Equal("ii", name.Suffix);
            Assert.Equal(PersonNameParseStatus.Simple, name.ParseStatus);
        }

        [Fact]
        public void ParseWithMultipleCommas()
        {
            PersonName name = PersonName.Parse("Miss John, Randolph D'oe, The ii");

            Assert.Equal("", name.Prefix);
            Assert.Equal("Miss John", name.First);
            Assert.Equal("", name.Middle);
            Assert.Equal("Randolph D'oe, The ii", name.Last);
            Assert.Equal("", name.Suffix);
            Assert.Equal(PersonNameParseStatus.Unparsed, name.ParseStatus);
        }

        [Fact]
        public void ParsePrefixFirstLastSuffix()
        {
            PersonName name = PersonName.Parse("Ms. John D'oe - M.D.");

            Assert.Equal("Ms.", name.Prefix);
            Assert.Equal("John", name.First);
            Assert.Equal(string.Empty, name.Middle);
            Assert.Equal("D'oe", name.Last);
            Assert.Equal("M.D.", name.Suffix);
            Assert.Equal(PersonNameParseStatus.PrefixFound, name.ParseStatus);
        }


        [Fact]
        public void SmallArmsShop_WithCompany()
        {
            PersonName name = PersonName.Parse("Small Arms Shop Co.");

            Assert.Equal("", name.First);
            Assert.Equal("", name.Middle);
            Assert.Equal("Small Arms Shop Co.", name.Last);
            Assert.Equal(PersonNameParseStatus.CompanyFound, name.ParseStatus);
        }
    }
}
