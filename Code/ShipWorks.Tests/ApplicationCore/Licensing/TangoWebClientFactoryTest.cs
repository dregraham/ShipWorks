using Xunit;
using Microsoft.Win32;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class TangoWebClientFactoryTest
    {
        private readonly TangoWebClientFactory testObject;

        private string originalTangoWebClientValue;
        private string originalInternalUserValue;

        public TangoWebClientFactoryTest()
        {
            testObject = new TangoWebClientFactory();
            originalTangoWebClientValue = string.Empty;
            originalInternalUserValue = string.Empty;
        }

        // NOTE: These are highly dependent on the environment since they require reading and writing of 
        // the registry. All tests have been commented out for the build process for this reason.
        
        //public TangoWebClientFactoryTest()
        //{
        //    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Interapptive\ShipWorks\Internal");
        //    if (key != null)
        //    {
        //        originalTangoWebClientValue = (string)key.GetValue("TangoWebClient") ?? string.Empty;
        //        originalInternalUserValue = (string)key.GetValue("Private") ?? string.Empty;
        //    }
        //}

        //[TestCleanup]
        //public void Cleanup()
        //{
        //    // Set the key value back to what it was before
        //    SetTangoWebCientRegistryValue(originalTangoWebClientValue);

        //    // Change the key indicating whether this is an Interapptive user back to its original value
        //    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Interapptive\ShipWorks\Internal");
        //    if (key != null)
        //    {
        //        key.SetValue("Private", originalInternalUserValue);
        //    }
        //}

        ///// <summary>
        ///// Sets the tango web cient registry value.
        ///// </summary>
        ///// <param name="value">The value.</param>
        //private void SetTangoWebCientRegistryValue(string value)
        //{
        //    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Interapptive\ShipWorks\Internal");
        //    key.SetValue("TangoWebClient", value);
        //}

        ///// <summary>
        ///// Configures whther to run as the Interapptive user.
        ///// </summary>
        ///// <param name="isInterapptiveUser">if set to <c>true</c> [is interapptive user].</param>
        //private void ConfigureInterapptiveUser(bool isInterapptiveUser)
        //{
        //    // Change the key indicating whether this is an Interapptive user back to its original value
        //    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Interapptive\ShipWorks\Internal");
        //    if (key != null)
        //    {
        //        key.SetValue("Private", isInterapptiveUser ? "True" : "False");
        //    }
        //}

        //[Fact]
        //public void Create_ReturnsCustomTangoWebClient_WhenRegistryKeyValueIsConfiguredWithExistingType_AndIsInterapptiveUser_Test()
        //{
        //    const string fakeClientTypeName = "ShipWorks.ApplicationCore.Licensing.FakeTangoWebClient";
        //    SetTangoWebCientRegistryValue(fakeClientTypeName);

        //    ConfigureInterapptiveUser(true);

        //    ITangoWebClient client = testObject.CreateWebClient();
        //    Assert.IsAssignableFrom<FakeTangoWebClient>(client);
        //}

        //[Fact]
        //public void Create_ReturnsTangoWebClientWrapper_WhenRegistryKeyValueIsConfiguredWithExisting_AndIsNotInterapptiveUser_Test()
        //{
        //    const string fakeClientTypeName = "ShipWorks.ApplicationCore.Licensing.FakeTangoWebClient";
        //    SetTangoWebCientRegistryValue(fakeClientTypeName);

        //    ConfigureInterapptiveUser(false);

        //    ITangoWebClient client = testObject.CreateWebClient();
        //    Assert.IsAssignableFrom<TangoWebClientWrapper>(client);
        //}

        //[Fact]
        //public void Create_ReturnsTangoWebClientWrapper_WhenRegistryKeyValueIsConfiguredWithNonExistingType_AndIsInterapptiveUser_Test()
        //{
        //    const string typeThatDoesNotExist = "ShipWorks.ApplicationCore.Licensing.SomeTypeThatDoesNotExist";
        //    SetTangoWebCientRegistryValue(typeThatDoesNotExist);

        //    ConfigureInterapptiveUser(false);

        //    ITangoWebClient client = testObject.CreateWebClient();
        //    Assert.IsAssignableFrom<TangoWebClientWrapper>(client);
        //}

        //[Fact]
        //public void Create_ReturnsTangoWebClientWrapper_WhenRegistryKeyValueIsEmpty_AndIsInterapptiveUser_Test()
        //{
        //    const string typeThatDoesNotExist = "ShipWorks.ApplicationCore.Licensing.SomeTypeThatDoesNotExist";
        //    SetTangoWebCientRegistryValue(typeThatDoesNotExist);

        //    ConfigureInterapptiveUser(false);

        //    ITangoWebClient client = testObject.CreateWebClient();
        //    Assert.IsAssignableFrom<TangoWebClientWrapper>(client);
        //}

        //[Fact]
        //public void Create_ReturnsTangoWebClientWrapper_WhenRegistryKeyValueDoesNotExist_AndIsInterapptiveUser_Test()
        //{
        //    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Interapptive\ShipWorks\Internal");
        //    key.DeleteValue("TangoWebClient");

        //    ConfigureInterapptiveUser(false);

        //    ITangoWebClient client = testObject.CreateWebClient();
        //    Assert.IsAssignableFrom<TangoWebClientWrapper>(client);
        //}
    }
}
