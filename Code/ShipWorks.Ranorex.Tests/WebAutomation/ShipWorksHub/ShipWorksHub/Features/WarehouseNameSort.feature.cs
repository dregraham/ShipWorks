// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.0.0.0
//      SpecFlow Generator Version:3.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace ShipWorksHub.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class WarehouseNameSortFeature : Xunit.IClassFixture<WarehouseNameSortFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "WarehouseNameSort.feature"
#line hidden
        
        public WarehouseNameSortFeature(WarehouseNameSortFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "WarehouseNameSort", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.TheoryAttribute(DisplayName="The user checks the sorting of the warehouse names on Firefox")]
        [Xunit.TraitAttribute("FeatureTitle", "WarehouseNameSort")]
        [Xunit.TraitAttribute("Description", "The user checks the sorting of the warehouse names on Firefox")]
        [Xunit.TraitAttribute("Category", "Firefox,")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        [Xunit.InlineDataAttribute("Firefox", "user-9998@example.com", "GOOD", new string[0])]
        public virtual void TheUserChecksTheSortingOfTheWarehouseNamesOnFirefox(string browser, string username, string password, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Firefox,",
                    "Smoke"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("The user checks the sorting of the warehouse names on Firefox", null, @__tags);
#line 4
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
 testRunner.Given(string.Format("the following user with \'{0}\' and \'{1}\' wants to navigate to the warehouse page u" +
                        "sing \'{2}\'", username, password, browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.Then("the user gets the list of all the warehouses", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 7
 testRunner.Then("the user clicks the add button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 8
 testRunner.Then("the user adds a warehouse to be sorted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 9
 testRunner.Then("the user clicks the add warehouse button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 10
 testRunner.Then("the user verifies that they are back on the settings page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 11
 testRunner.Then("the user verifies that the warehouses are sorted correctly", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 12
 testRunner.Then("the user closes the warehouse page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="The user checks the sorting of the warehouse names on Chrome")]
        [Xunit.TraitAttribute("FeatureTitle", "WarehouseNameSort")]
        [Xunit.TraitAttribute("Description", "The user checks the sorting of the warehouse names on Chrome")]
        [Xunit.TraitAttribute("Category", "Chrome")]
        [Xunit.InlineDataAttribute("Chrome", "user-9996@example.com", "GOOD", new string[0])]
        public virtual void TheUserChecksTheSortingOfTheWarehouseNamesOnChrome(string browser, string username, string password, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Chrome"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("The user checks the sorting of the warehouse names on Chrome", null, @__tags);
#line 19
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 20
 testRunner.Given(string.Format("the following user with \'{0}\' and \'{1}\' wants to navigate to the warehouse page u" +
                        "sing \'{2}\'", username, password, browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 21
 testRunner.Then("the user gets the list of all the warehouses", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 22
 testRunner.Then("the user clicks the add button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 23
 testRunner.Then("the user adds a warehouse to be sorted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 24
 testRunner.Then("the user clicks the add warehouse button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 25
 testRunner.Then("the user verifies that they are back on the settings page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 26
 testRunner.Then("the user verifies that the warehouses are sorted correctly", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 27
 testRunner.Then("the user closes the warehouse page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="The user checks the sorting of the warehouse names on Edge")]
        [Xunit.TraitAttribute("FeatureTitle", "WarehouseNameSort")]
        [Xunit.TraitAttribute("Description", "The user checks the sorting of the warehouse names on Edge")]
        [Xunit.TraitAttribute("Category", "Edge")]
        [Xunit.InlineDataAttribute("Edge", "user-9994@example.com", "GOOD", new string[0])]
        public virtual void TheUserChecksTheSortingOfTheWarehouseNamesOnEdge(string browser, string username, string password, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Edge"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("The user checks the sorting of the warehouse names on Edge", null, @__tags);
#line 34
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 35
 testRunner.Given(string.Format("the following user with \'{0}\' and \'{1}\' wants to navigate to the warehouse page u" +
                        "sing \'{2}\'", username, password, browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 36
 testRunner.Then("the user gets the list of all the warehouses", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 37
 testRunner.Then("the user clicks the add button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 38
 testRunner.Then("the user adds a warehouse to be sorted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 39
 testRunner.Then("the user clicks the add warehouse button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
 testRunner.Then("the user verifies that they are back on the settings page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 41
 testRunner.Then("the user verifies that the warehouses are sorted correctly", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 42
 testRunner.Then("the user closes the warehouse page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                WarehouseNameSortFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                WarehouseNameSortFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
