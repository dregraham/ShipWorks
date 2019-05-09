﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.4.0.0
//      SpecFlow Generator Version:2.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace XunitSpecflow.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class RemoveWarehouseFeature : Xunit.IClassFixture<RemoveWarehouseFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "RemoveWarehouse.feature"
#line hidden
        
        public RemoveWarehouseFeature(RemoveWarehouseFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "RemoveWarehouse", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [Xunit.TheoryAttribute(DisplayName="User removes a warehouse for Chrome")]
        [Xunit.TraitAttribute("FeatureTitle", "RemoveWarehouse")]
        [Xunit.TraitAttribute("Description", "User removes a warehouse for Chrome")]
        [Xunit.TraitAttribute("Category", "Chrome")]
        [Xunit.InlineDataAttribute("Chrome", "user-0801@example.com", "GOOD", new string[0])]
        public virtual void UserRemovesAWarehouseForChrome(string browser, string username, string password, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Chrome"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User removes a warehouse for Chrome", null, @__tags);
#line 4
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
 testRunner.Given(string.Format("the following user with \'{0}\' and \'{1}\' wants to navigate to the warehouse page u" +
                        "sing \'{2}\'", username, password, browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.Then("the user clicks the remove button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 7
 testRunner.Then("the user accepts the remove warehouse confirmation", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="User removes a warehouse for Firefox")]
        [Xunit.TraitAttribute("FeatureTitle", "RemoveWarehouse")]
        [Xunit.TraitAttribute("Description", "User removes a warehouse for Firefox")]
        [Xunit.TraitAttribute("Category", "Firefox,")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        [Xunit.InlineDataAttribute("Firefox", "user-0801@example.com", "GOOD", new string[0])]
        public virtual void UserRemovesAWarehouseForFirefox(string browser, string username, string password, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Firefox,",
                    "Smoke"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User removes a warehouse for Firefox", null, @__tags);
#line 16
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 17
 testRunner.Given(string.Format("the following user with \'{0}\' and \'{1}\' wants to navigate to the warehouse page u" +
                        "sing \'{2}\'", username, password, browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 18
 testRunner.Then("the user clicks the remove button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 19
 testRunner.Then("the user accepts the remove warehouse confirmation", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="User removes a warehouse for Edge")]
        [Xunit.TraitAttribute("FeatureTitle", "RemoveWarehouse")]
        [Xunit.TraitAttribute("Description", "User removes a warehouse for Edge")]
        [Xunit.TraitAttribute("Category", "Edge")]
        [Xunit.InlineDataAttribute("Edge", "user-0801@example.com", "GOOD", new string[0])]
        public virtual void UserRemovesAWarehouseForEdge(string browser, string username, string password, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Edge"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User removes a warehouse for Edge", null, @__tags);
#line 27
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 28
 testRunner.Given(string.Format("the following user with \'{0}\' and \'{1}\' wants to navigate to the warehouse page u" +
                        "sing \'{2}\'", username, password, browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 29
 testRunner.Then("the user clicks the remove button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 30
 testRunner.Then("the user accepts the remove warehouse confirmation", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                RemoveWarehouseFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                RemoveWarehouseFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion

