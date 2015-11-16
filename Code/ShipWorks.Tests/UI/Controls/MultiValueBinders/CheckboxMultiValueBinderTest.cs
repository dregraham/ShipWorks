using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.UI.Controls.MultiValueBinders;
using Xunit;

namespace ShipWorks.Tests.UI.Controls.MultiValueBinders
{
    public class CheckboxMultiValueBinderTest 
    {
        private List<GenericMultiValueBinderDto<bool, string>> allDistinctValuesDataSource = new List<GenericMultiValueBinderDto<bool, string>>()
            {
                new GenericMultiValueBinderDto<bool, string>(true, "one"),
                new GenericMultiValueBinderDto<bool, string>(true, "one"),
                new GenericMultiValueBinderDto<bool, string>(true, "one")
            };
        private List<GenericMultiValueBinderDto<bool, string>> allDifferentValuesDataSource = new List<GenericMultiValueBinderDto<bool, string>>()
            {
                new GenericMultiValueBinderDto<bool, string>(true, "one"),
                new GenericMultiValueBinderDto<bool, string>(false, "one"),
                new GenericMultiValueBinderDto<bool, string>(true, "one")
            };

        [Fact]
        public void DerivesFromGenericMultiValueBinder_IsTrue()
        {
            CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>> testObject = new CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>>(allDistinctValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; },
                s => false);

            // Verify that CheckboxMultiValueBinder is a child of GenericMultiValueBinder
            Assert.True(testObject is GenericMultiValueBinder<GenericMultiValueBinderDto<bool, string>, bool>);
        }

        [Fact]
        public void CheckStateValue_WhenIsMultiValuedIsTrue_ReturnsIndeterminate()
        {
            CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>> testObject = new CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>>(allDifferentValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; },
                s => false);

            Assert.Equal(testObject.CheckStateValue, CheckState.Indeterminate);
        }

        [Fact]
        public void CheckStateValue_WhenIsMultiValuedIsFalse_AndAllValuesAreTrue_ReturnsChecked()
        {
            CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>> testObject = new CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>>(allDistinctValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; },
                s => false);

            testObject.PropertyValue = true;

            Assert.Equal(CheckState.Checked, testObject.CheckStateValue);
        }

        [Fact]
        public void CheckStateValue_WhenIsMultiValuedIsFalse_AndAllValuesAreFalse_ReturnsUnChecked()
        {
            CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>> testObject = new CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>>(allDistinctValuesDataSource,
                "Property1",
                s => s.Property1,
                (s, v) => { s.Property1 = v; },
                s => false);

            testObject.PropertyValue = false;

            Assert.Equal(CheckState.Unchecked, testObject.CheckStateValue);
        }
    }
}
