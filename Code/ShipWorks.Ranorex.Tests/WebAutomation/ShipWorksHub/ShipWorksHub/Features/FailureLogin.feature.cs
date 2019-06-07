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
    public partial class FailureLoginFeature : Xunit.IClassFixture<FailureLoginFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "FailureLogin.feature"
#line hidden
        
        public FailureLoginFeature(FailureLoginFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "FailureLogin", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [Xunit.TheoryAttribute(DisplayName="Login with invalid credentials on Firefox")]
        [Xunit.TraitAttribute("FeatureTitle", "FailureLogin")]
        [Xunit.TraitAttribute("Description", "Login with invalid credentials on Firefox")]
        [Xunit.TraitAttribute("Category", "Firefox,")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        [Xunit.InlineDataAttribute("Firefox", new string[0])]
        public virtual void LoginWithInvalidCredentialsOnFirefox(string browser, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Firefox,",
                    "Smoke"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Login with invalid credentials on Firefox", null, @__tags);
#line 4
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
 testRunner.Given(string.Format("the user is on login page on \'{0}\'", browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.Given("the user enters invalid username and password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
 testRunner.Then("the user sees the error message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 8
 testRunner.Then("the user closes the browser", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="Login with invalid credentials on Chrome")]
        [Xunit.TraitAttribute("FeatureTitle", "FailureLogin")]
        [Xunit.TraitAttribute("Description", "Login with invalid credentials on Chrome")]
        [Xunit.TraitAttribute("Category", "Chrome")]
        [Xunit.InlineDataAttribute("Chrome", new string[0])]
        public virtual void LoginWithInvalidCredentialsOnChrome(string browser, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Chrome"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Login with invalid credentials on Chrome", null, @__tags);
#line 15
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 16
 testRunner.Given(string.Format("the user is on login page on \'{0}\'", browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 17
 testRunner.Given("the user enters invalid username and password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 18
 testRunner.Then("the user sees the error message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 19
 testRunner.Then("the user closes the browser", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="Login with invalid credentials on Edge")]
        [Xunit.TraitAttribute("FeatureTitle", "FailureLogin")]
        [Xunit.TraitAttribute("Description", "Login with invalid credentials on Edge")]
        [Xunit.TraitAttribute("Category", "Edge")]
        [Xunit.InlineDataAttribute("Edge", new string[0])]
        public virtual void LoginWithInvalidCredentialsOnEdge(string browser, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Edge"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Login with invalid credentials on Edge", null, @__tags);
#line 26
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 27
 testRunner.Given(string.Format("the user is on login page on \'{0}\'", browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 28
 testRunner.Given("the user enters invalid username and password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 29
 testRunner.Then("the user sees the error message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 30
 testRunner.Then("the user closes the browser", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                FailureLoginFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                FailureLoginFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
