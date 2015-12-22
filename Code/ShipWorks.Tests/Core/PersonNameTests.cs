using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Business;

namespace ShipWorks.Tests.Core
{
    public class PersonNameTests
    {
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

            Assert.Equal(name.First, "John");
            Assert.Equal(name.Last, "Doe");
            Assert.Equal(name.FullName, "Sgt John Doe");
        }

        [Fact]
        public void DrFullName()
        {
            PersonName name = PersonName.Parse("Dr. John Smith");

            Assert.Equal(name.First, "John");
            Assert.Equal(name.Last, "Smith");
            Assert.Equal(name.FullName, "Dr. John Smith");
        }

        [Fact]
        public void DisplayFirstLast()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Last = "Smith";

            Assert.Equal(name.FullName, "Joe Smith");
        }

        [Fact]
        public void DisplayFirstLastMiddle()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Middle = "Wonderball";
            name.Last = "Smith";

            Assert.Equal(name.FullName, "Joe Wonderball Smith");
        }

        [Fact]
        public void DisplayFirstLastSuffix()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Last = "Smith";
            name.Suffix = "Jr.";

           Assert.Equal(name.FullName, "Joe Smith Jr.");
        }

        [Fact]
        public void DisplayFirstLastMiddleSuffix()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Middle = "Marco";
            name.Last = "Smith";
            name.Suffix = "Jr.";

            Assert.Equal(name.FullName, "Joe Marco Smith Jr.");
        }

        [Fact]
        public void ParseFirstLast()
        {
            PersonName name = PersonName.Parse("John D'oe");

            Assert.Equal(name.Prefix, string.Empty);
            Assert.Equal(name.First, "John");
            Assert.Equal(name.Middle, string.Empty);
            Assert.Equal(name.Last, "D'oe");
            Assert.Equal(name.Suffix, string.Empty);
        }

        [Fact]
        public void ParseFirstLastMiddle()
        {
            PersonName name = PersonName.Parse("John Randolph D'oe");

            Assert.Equal(name.Prefix, string.Empty);
            Assert.Equal(name.First, "John");
            Assert.Equal(name.Middle, "Randolph");
            Assert.Equal(name.Last, "D'oe");
            Assert.Equal(name.Suffix, string.Empty);
        }

        [Fact]
        public void ParseFirstLastMiddleSuffix()
        {
            PersonName name = PersonName.Parse("John Randolph D'oe III");

            Assert.Equal(name.Prefix, string.Empty);
            Assert.Equal(name.First, "John");
            Assert.Equal(name.Middle, "Randolph");
            Assert.Equal(name.Last, "D'oe");
            Assert.Equal(name.Suffix, "III");
        }

        [Fact]
        public void ParseFirstLastSuffix()
        {
            PersonName name = PersonName.Parse("John D'oe Jr");

            Assert.Equal(name.Prefix, string.Empty);
            Assert.Equal(name.First, "John");
            Assert.Equal(name.Middle, string.Empty);
            Assert.Equal(name.Last, "D'oe");
            Assert.Equal(name.Suffix, "Jr");
        }

        [Fact]
        public void ParsePrefixFirstLast()
        {
            PersonName name = PersonName.Parse("Mr. John D'oe");

            Assert.Equal(name.Prefix, "Mr.");
            Assert.Equal(name.First, "John");
            Assert.Equal(name.Middle, string.Empty);
            Assert.Equal(name.Last, "D'oe");
            Assert.Equal(name.Suffix, string.Empty);
        }

        [Fact]
        public void ParsePrefixFirstLastMiddle()
        {
            PersonName name = PersonName.Parse("dr John Randolph D'oe");

            Assert.Equal(name.Prefix, "dr");
            Assert.Equal(name.First, "John");
            Assert.Equal(name.Middle, "Randolph");
            Assert.Equal(name.Last, "D'oe");
            Assert.Equal(name.Suffix, string.Empty);
        }

        [Fact]
        public void ParsePrefixFirstLastMiddleSuffix()
        {
            PersonName name = PersonName.Parse("Miss John Randolph D'oe, ii");

            Assert.Equal(name.Prefix, "Miss");
            Assert.Equal(name.First, "John");
            Assert.Equal(name.Middle, "Randolph");
            Assert.Equal(name.Last, "D'oe");
            Assert.Equal(name.Suffix, "ii");
        }

        [Fact]
        public void ParsePrefixFirstLastSuffix()
        {
            PersonName name = PersonName.Parse("Ms. John D'oe - M.D.");

            Assert.Equal(name.Prefix, "Ms.");
            Assert.Equal(name.First, "John");
            Assert.Equal(name.Middle, string.Empty);
            Assert.Equal(name.Last, "D'oe");
            Assert.Equal(name.Suffix, "M.D.");
        }
    }
}
