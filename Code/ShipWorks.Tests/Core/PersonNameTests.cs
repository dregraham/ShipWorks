using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Business;

namespace ShipWorks.Tests.Core
{
    [TestClass]
    public class PersonNameTests
    {
        [TestMethod]
        public void SmallArmsShopTest()
        {
            PersonName name = PersonName.Parse("Small Arms Shop");

            Assert.AreEqual("Small", name.First);
            Assert.AreEqual("Arms", name.Middle);
            Assert.AreEqual("Shop", name.Last);
        }

        [TestMethod]
        public void SeargentName()
        {
            PersonName name = PersonName.Parse("Sgt John Doe");

            Assert.AreEqual(name.First, "John");
            Assert.AreEqual(name.Last, "Doe");
            Assert.AreEqual(name.FullName, "Sgt John Doe");
        }

        [TestMethod]
        public void DrFullName()
        {
            PersonName name = PersonName.Parse("Dr. John Smith");

            Assert.AreEqual(name.First, "John");
            Assert.AreEqual(name.Last, "Smith");
            Assert.AreEqual(name.FullName, "Dr. John Smith");
        }

        [TestMethod]
        public void DisplayFirstLast()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Last = "Smith";

            Assert.AreEqual(name.FullName, "Joe Smith");
        }

        [TestMethod]
        public void DisplayFirstLastMiddle()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Middle = "Wonderball";
            name.Last = "Smith";

            Assert.AreEqual(name.FullName, "Joe Wonderball Smith");
        }

        [TestMethod]
        public void DisplayFirstLastSuffix()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Last = "Smith";
            name.Suffix = "Jr.";

           Assert.AreEqual(name.FullName, "Joe Smith Jr.");
        }

        [TestMethod]
        public void DisplayFirstLastMiddleSuffix()
        {
            PersonName name = new PersonName();
            name.First = "Joe";
            name.Middle = "Marco";
            name.Last = "Smith";
            name.Suffix = "Jr.";

            Assert.AreEqual(name.FullName, "Joe Marco Smith Jr.");
        }

        [TestMethod]
        public void ParseFirstLast()
        {
            PersonName name = PersonName.Parse("John D'oe");

            Assert.AreEqual(name.Prefix, string.Empty);
            Assert.AreEqual(name.First, "John");
            Assert.AreEqual(name.Middle, string.Empty);
            Assert.AreEqual(name.Last, "D'oe");
            Assert.AreEqual(name.Suffix, string.Empty);
        }

        [TestMethod]
        public void ParseFirstLastMiddle()
        {
            PersonName name = PersonName.Parse("John Randolph D'oe");

            Assert.AreEqual(name.Prefix, string.Empty);
            Assert.AreEqual(name.First, "John");
            Assert.AreEqual(name.Middle, "Randolph");
            Assert.AreEqual(name.Last, "D'oe");
            Assert.AreEqual(name.Suffix, string.Empty);
        }

        [TestMethod]
        public void ParseFirstLastMiddleSuffix()
        {
            PersonName name = PersonName.Parse("John Randolph D'oe III");

            Assert.AreEqual(name.Prefix, string.Empty);
            Assert.AreEqual(name.First, "John");
            Assert.AreEqual(name.Middle, "Randolph");
            Assert.AreEqual(name.Last, "D'oe");
            Assert.AreEqual(name.Suffix, "III");
        }

        [TestMethod]
        public void ParseFirstLastSuffix()
        {
            PersonName name = PersonName.Parse("John D'oe Jr");

            Assert.AreEqual(name.Prefix, string.Empty);
            Assert.AreEqual(name.First, "John");
            Assert.AreEqual(name.Middle, string.Empty);
            Assert.AreEqual(name.Last, "D'oe");
            Assert.AreEqual(name.Suffix, "Jr");
        }

        [TestMethod]
        public void ParsePrefixFirstLast()
        {
            PersonName name = PersonName.Parse("Mr. John D'oe");

            Assert.AreEqual(name.Prefix, "Mr.");
            Assert.AreEqual(name.First, "John");
            Assert.AreEqual(name.Middle, string.Empty);
            Assert.AreEqual(name.Last, "D'oe");
            Assert.AreEqual(name.Suffix, string.Empty);
        }

        [TestMethod]
        public void ParsePrefixFirstLastMiddle()
        {
            PersonName name = PersonName.Parse("dr John Randolph D'oe");

            Assert.AreEqual(name.Prefix, "dr");
            Assert.AreEqual(name.First, "John");
            Assert.AreEqual(name.Middle, "Randolph");
            Assert.AreEqual(name.Last, "D'oe");
            Assert.AreEqual(name.Suffix, string.Empty);
        }

        [TestMethod]
        public void ParsePrefixFirstLastMiddleSuffix()
        {
            PersonName name = PersonName.Parse("Miss John Randolph D'oe, ii");

            Assert.AreEqual(name.Prefix, "Miss");
            Assert.AreEqual(name.First, "John");
            Assert.AreEqual(name.Middle, "Randolph");
            Assert.AreEqual(name.Last, "D'oe");
            Assert.AreEqual(name.Suffix, "ii");
        }

        [TestMethod]
        public void ParsePrefixFirstLastSuffix()
        {
            PersonName name = PersonName.Parse("Ms. John D'oe - M.D.");

            Assert.AreEqual(name.Prefix, "Ms.");
            Assert.AreEqual(name.First, "John");
            Assert.AreEqual(name.Middle, string.Empty);
            Assert.AreEqual(name.Last, "D'oe");
            Assert.AreEqual(name.Suffix, "M.D.");
        }
    }
}
