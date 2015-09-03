using System.Collections.Generic;
using System.Linq;
using ShipWorks.UI.Controls.MultiValueBinders;
using Xunit;

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

        [Fact]
        public void IsMultiValued_WithOnePropertyValue_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDistinctValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.Equal(false, testObject.IsMultiValued);
        }

        [Fact]
        public void IsMultiValued_WithAllDifferentPropertyValues_ReturnsTrue()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDifferentValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.Equal(true, testObject.IsMultiValued);
        }

        [Fact]
        public void IsMultiValued_WithSomeSamePropertyValues_ReturnsTrue()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(mixedWithSomeSameValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.Equal(true, testObject.IsMultiValued);
        }

        [Fact]
        public void IsMultiValued_WithEmptyDataSource_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(Enumerable.Empty<GenericMultiValueBinderDto<int, string>>(),
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.Equal(false, testObject.IsMultiValued);
        }

        [Fact]
        public void IsMultiValued_WithSingleItemDataSource_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(mixedWithSomeSameValuesDataSource.GetRange(1, 1),
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.Equal(false, testObject.IsMultiValued);
        }

        [Fact]
        public void PropertyValue_ForObjectPropertyType_WithEmptyDataSource_ReturnsNull()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<string, string>, string> testObject2 = new GenericMultiValueBinder<GenericMultiValueBinderDto<string, string>, string>(Enumerable.Empty<GenericMultiValueBinderDto<string, string>>(),
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.Null(testObject2.PropertyValue);
        }

        [Fact]
        public void SetPropertyValueWithSameDataSourceValue_AndAllDistinctDataSource_IsMultiValued_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDistinctValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            testObject.PropertyValue = allDistinctValuesDataSource.First().Property1;

            Assert.False(testObject.IsMultiValued);
        }

        [Fact]
        public void SetPropertyValueWithDifferentDataSourceValue_AndAllDifferentDataSource_IsMultiValued_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDifferentValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            testObject.PropertyValue = 99999992;

            Assert.False(testObject.IsMultiValued);
        }

        [Fact]
        public void SetPropertyValueWithDifferentDataSourceValue_AndMixedDataSource_IsMultiValued_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(mixedWithSomeSameValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            testObject.PropertyValue = 99999992;

            Assert.False(testObject.IsMultiValued);
        }

        [Fact]
        public void SetPropertyValue_WithSingleItemDataSource_IsMultiValued_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(mixedWithSomeSameValuesDataSource.GetRange(1, 1),
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            testObject.PropertyValue = 99999992;

            Assert.Equal(false, testObject.IsMultiValued);
        }

        [Fact]
        public void Save_AndAllDistinctDataSource_IsMultiValued_ReturnsFalse()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDistinctValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => s.Property1 = v);

            testObject.Save();

            Assert.False(testObject.IsMultiValued);
        }

        [Fact]
        public void Save_AndAllDifferentDataSource_IsMultiValued_ReturnsTrue()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDifferentValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => s.Property1 = v);

            testObject.Save();

            Assert.True(testObject.IsMultiValued);
        }

        [Fact]
        public void SaveWithNewPropertyValue_AndAllDistinctDataSource_PropertyValueIsNewValue()
        {
            GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int> testObject = new GenericMultiValueBinder<GenericMultiValueBinderDto<int, string>, int>(allDistinctValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => s.Property1 = v);

            testObject.PropertyValue = 3;
            testObject.Save();

            Assert.Equal(3, testObject.PropertyValue);
        }
    }
}
