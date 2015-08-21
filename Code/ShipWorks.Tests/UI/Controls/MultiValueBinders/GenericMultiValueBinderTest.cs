using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.UI.Controls.MultiValueBinders;

namespace ShipWorks.Tests.UI.Controls.MultiValueBinders
{
    public class GenericMultiValueBinderDto<TProperty1, TProperty2>
    {
        public TProperty1 Property1;
        public TProperty2 Property2;

        public GenericMultiValueBinderDto(TProperty1 p1, TProperty2 p2)
        {
            Property1 = p1;
            Property2 = p2;
        }
    }

    [TestClass]
    public class GenericMultiValueBinderTest
    {
        protected List<GenericMultiValueBinderDto<int, string>> allDistinctValuesDataSource = new List<GenericMultiValueBinderDto<int, string>>()
            {
                new GenericMultiValueBinderDto<int, string>(1, "one"),
                new GenericMultiValueBinderDto<int, string>(1, "one"),
                new GenericMultiValueBinderDto<int, string>(1, "one")
            };
        protected List<GenericMultiValueBinderDto<int, string>> allDifferentValuesDataSource = new List<GenericMultiValueBinderDto<int, string>>()
            {
                new GenericMultiValueBinderDto<int, string>(1, "one"),
                new GenericMultiValueBinderDto<int, string>(2, "one"),
                new GenericMultiValueBinderDto<int, string>(3, "one")
            };
        protected List<GenericMultiValueBinderDto<int, string>> mixedWithSomeSameValuesDataSource = new List<GenericMultiValueBinderDto<int, string>>()
            {
                new GenericMultiValueBinderDto<int, string>(1, "one"),
                new GenericMultiValueBinderDto<int, string>(2, "one"),
                new GenericMultiValueBinderDto<int, string>(1, "one")
            };

        [TestMethod]
        public void IsMultiValued_WithOnePropertyValue_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDistinctValuesDataSource,
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.AreEqual(false, testObject.IsMultiValued);
        }

        [TestMethod]
        public void IsMultiValued_WithAllDifferentPropertyValues_ReturnsTrue()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDifferentValuesDataSource,
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.AreEqual(true, testObject.IsMultiValued);
        }

        [TestMethod]
        public void IsMultiValued_WithSomeSamePropertyValues_ReturnsTrue()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(mixedWithSomeSameValuesDataSource,
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.AreEqual(true, testObject.IsMultiValued);
        }

        [TestMethod]
        public void IsMultiValued_WithEmptyDataSource_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(Enumerable.Empty<GenericMultiValueBinderDto<int, string>>(),
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.AreEqual(false, testObject.IsMultiValued);
        }

        [TestMethod]
        public void IsMultiValued_WithSingleItemDataSource_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(mixedWithSomeSameValuesDataSource.GetRange(1, 1),
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.AreEqual(false, testObject.IsMultiValued);
        }

        [TestMethod]
        public void PropertyValue_ForObjectPropertyType_WithEmptyDataSource_ReturnsNull()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<string, string>, string> testObject2 = new GenericMultiValueBinder<GenericMultiValueBinderDto<string, string>, string>(Enumerable.Empty<GenericMultiValueBinderDto<string, string>>(),
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.IsNull(testObject2.PropertyValue);
        }

        [TestMethod]
        public void SetPropertyValueWithSameDataSourceValue_AndAllDistinctDataSource_IsMultiValued_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDistinctValuesDataSource,
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            testObject.PropertyValue = allDistinctValuesDataSource.First().Property1;

            Assert.IsFalse(testObject.IsMultiValued);
        }

        [TestMethod]
        public void SetPropertyValueWithDifferentDataSourceValue_AndAllDifferentDataSource_IsMultiValued_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDifferentValuesDataSource,
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            testObject.PropertyValue = 99999992;

            Assert.IsFalse(testObject.IsMultiValued);
        }

        [TestMethod]
        public void SetPropertyValueWithDifferentDataSourceValue_AndMixedDataSource_IsMultiValued_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(mixedWithSomeSameValuesDataSource,
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            testObject.PropertyValue = 99999992;

            Assert.IsFalse(testObject.IsMultiValued);
        }

        [TestMethod]
        public void SetPropertyValue_WithSingleItemDataSource_IsMultiValued_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(mixedWithSomeSameValuesDataSource.GetRange(1, 1),
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            testObject.PropertyValue = 99999992;

            Assert.AreEqual(false, testObject.IsMultiValued);
        }

        [TestMethod]
        public void Save_AndAllDistinctDataSource_IsMultiValued_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDistinctValuesDataSource,
                s => s.Property1,
                (s, v) => s.Property1 = v);

            testObject.Save();

            Assert.IsFalse(testObject.IsMultiValued);
        }

        [TestMethod]
        public void Save_AndAllDifferentDataSource_IsMultiValued_ReturnsTrue()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDifferentValuesDataSource,
                s => s.Property1,
                (s, v) => s.Property1 = v);

            testObject.Save();

            Assert.IsTrue(testObject.IsMultiValued);
        }

        [TestMethod]
        public void SaveWithNewPropertyValue_AndAllDistinctDataSource_PropertyValueIsNewValue()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDistinctValuesDataSource,
                s => s.Property1,
                (s, v) => s.Property1 = v);

            testObject.PropertyValue = 3;
            testObject.Save();

            Assert.AreEqual(3, testObject.PropertyValue);
        }
    }
}
