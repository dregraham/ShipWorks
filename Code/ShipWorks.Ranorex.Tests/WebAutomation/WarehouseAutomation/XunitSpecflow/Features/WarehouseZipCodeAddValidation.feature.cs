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
    public partial class WarehouseZipCodeAddValidationFeature : Xunit.IClassFixture<WarehouseZipCodeAddValidationFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "WarehouseZipCodeAddValidation.feature"
#line hidden
        
        public WarehouseZipCodeAddValidationFeature(WarehouseZipCodeAddValidationFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "WarehouseZipCodeAddValidation", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [Xunit.TheoryAttribute(DisplayName="User validates add zip code on Firefox")]
        [Xunit.TraitAttribute("FeatureTitle", "WarehouseZipCodeAddValidation")]
        [Xunit.TraitAttribute("Description", "User validates add zip code on Firefox")]
        [Xunit.TraitAttribute("Category", "Firefox,")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        [Xunit.InlineDataAttribute("Firefox", "Garrett", "Code 3", "1 Memorial Drive", "St. Louis", "MO", "63102", "user-0801@example.com", "GOOD", new string[0])]
        [Xunit.InlineDataAttribute("Firefox", "Garrett", "Code 3", "1 Memorial Drive", "St. Louis", "MO", "63102-3410", "user-0801@example.com", "GOOD", new string[0])]
        [Xunit.InlineDataAttribute("Firefox", "Garrett", "Code 3", "1 Memorial Drive", "St. Louis", "MO", "631023410", "user-0801@example.com", "GOOD", new string[0])]
        public virtual void UserValidatesAddZipCodeOnFirefox(string browser, string name, string code, string street, string city, string state, string zip, string username, string password, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Firefox,",
                    "Smoke"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User validates add zip code on Firefox", null, @__tags);
#line 4
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
 testRunner.Given(string.Format("the following user with \'{0}\' and \'{1}\' wants to navigate to the warehouse page u" +
                        "sing \'{2}\'", username, password, browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.Then("the user clicks the add button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 7
 testRunner.Then(string.Format("the user adds the following Warehouse details \'{0}\' \'{1}\' \'{2}\' \'{3}\' \'{4}\' \'{5}\'" +
                        "", name, code, street, city, state, zip), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 8
 testRunner.Then("the user clicks the add warehouse button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 9
 testRunner.Then("the user closes the warehouse page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="User validates add zip code on Chrome")]
        [Xunit.TraitAttribute("FeatureTitle", "WarehouseZipCodeAddValidation")]
        [Xunit.TraitAttribute("Description", "User validates add zip code on Chrome")]
        [Xunit.TraitAttribute("Category", "Chrome")]
        [Xunit.InlineDataAttribute("Chrome", "Garrett", "Code 3", "1 Memorial Drive", "St. Louis", "MO", "63102", "user-0801@example.com", "GOOD", new string[0])]
        [Xunit.InlineDataAttribute("Chrome", "Garrett", "Code 3", "1 Memorial Drive", "St. Louis", "MO", "63102-3410", "user-0801@example.com", "GOOD", new string[0])]
        [Xunit.InlineDataAttribute("Chrome", "Garrett", "Code 3", "1 Memorial Drive", "St. Louis", "MO", "631023410", "user-0801@example.com", "GOOD", new string[0])]
        public virtual void UserValidatesAddZipCodeOnChrome(string browser, string name, string code, string street, string city, string state, string zip, string username, string password, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Chrome"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User validates add zip code on Chrome", null, @__tags);
#line 18
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 19
 testRunner.Given(string.Format("the following user with \'{0}\' and \'{1}\' wants to navigate to the warehouse page u" +
                        "sing \'{2}\'", username, password, browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 20
 testRunner.Then("the user clicks the add button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 21
 testRunner.Then(string.Format("the user adds the following Warehouse details \'{0}\' \'{1}\' \'{2}\' \'{3}\' \'{4}\' \'{5}\'" +
                        "", name, code, street, city, state, zip), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 22
 testRunner.Then("the user clicks the add warehouse button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 23
 testRunner.Then("the user closes the warehouse page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="User validates add zip code on Edge")]
        [Xunit.TraitAttribute("FeatureTitle", "WarehouseZipCodeAddValidation")]
        [Xunit.TraitAttribute("Description", "User validates add zip code on Edge")]
        [Xunit.TraitAttribute("Category", "Edge")]
        [Xunit.InlineDataAttribute("Edge", "Garrett", "Code 3", "1 Memorial Drive", "St. Louis", "MO", "63102", "user-0801@example.com", "GOOD", new string[0])]
        [Xunit.InlineDataAttribute("Edge", "Garrett", "Code 3", "1 Memorial Drive", "St. Louis", "MO", "63102-3410", "user-0801@example.com", "GOOD", new string[0])]
        [Xunit.InlineDataAttribute("Edge", "Garrett", "Code 3", "1 Memorial Drive", "St. Louis", "MO", "631023410", "user-0801@example.com", "GOOD", new string[0])]
        public virtual void UserValidatesAddZipCodeOnEdge(string browser, string name, string code, string street, string city, string state, string zip, string username, string password, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Edge"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User validates add zip code on Edge", null, @__tags);
#line 32
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 33
 testRunner.Given(string.Format("the following user with \'{0}\' and \'{1}\' wants to navigate to the warehouse page u" +
                        "sing \'{2}\'", username, password, browser), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 34
 testRunner.Then("the user clicks the add button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 35
 testRunner.Then(string.Format("the user adds the following Warehouse details \'{0}\' \'{1}\' \'{2}\' \'{3}\' \'{4}\' \'{5}\'" +
                        "", name, code, street, city, state, zip), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 36
 testRunner.Then("the user clicks the add warehouse button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 37
 testRunner.Then("the user closes the warehouse page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                WarehouseZipCodeAddValidationFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                WarehouseZipCodeAddValidationFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion

