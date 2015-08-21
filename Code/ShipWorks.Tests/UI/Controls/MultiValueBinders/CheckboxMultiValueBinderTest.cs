using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.UI.Controls.MultiValueBinders;

namespace ShipWorks.Tests.UI.Controls.MultiValueBinders
{
    [TestClass]
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

        [TestMethod]
        public void DerivesFromGenericMultiValueBinder_IsTrue()
        {
            CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>> testObject = new CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>>(allDistinctValuesDataSource,
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            // Verify that CheckboxMultiValueBinder is a child of GenericMultiValueBinder
            Assert.IsTrue(testObject is GenericMultiValueBinder<GenericMultiValueBinderDto<bool, string>, bool>);
        }

        [TestMethod]
        public void CheckStateValue_WhenIsMultiValuedIsTrue_ReturnsIndeterminate()
        {
            CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>> testObject = new CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>>(allDifferentValuesDataSource,
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            Assert.AreEqual(testObject.CheckStateValue, CheckState.Indeterminate);
        }

        [TestMethod]
        public void CheckStateValue_WhenIsMultiValuedIsFalse_AndAllValuesAreTrue_ReturnsChecked()
        {
            CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>> testObject = new CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>>(allDistinctValuesDataSource,
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            testObject.PropertyValue = true;

            Assert.AreEqual(CheckState.Checked, testObject.CheckStateValue);
        }

        [TestMethod]
        public void CheckStateValue_WhenIsMultiValuedIsFalse_AndAllValuesAreFalse_ReturnsUnChecked()
        {
            CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>> testObject = new CheckboxMultiValueBinder<GenericMultiValueBinderDto<bool, string>>(allDistinctValuesDataSource,
                s => s.Property1,
                (s, v) => { s.Property1 = v; });

            testObject.PropertyValue = false;

            Assert.AreEqual(CheckState.Unchecked, testObject.CheckStateValue);
        }
    }
}
