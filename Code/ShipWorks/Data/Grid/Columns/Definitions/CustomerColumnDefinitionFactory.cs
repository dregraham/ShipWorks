using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Columns.ValueProviders;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class CustomerColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible customer columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                {
                    new GridColumnDefinition("{AF406321-B8B4-463c-B116-C69CA9A095C6}", 
                        new GridTextDisplayType(), "Customer ID", "31845",
                        CustomerFields.CustomerID),

                    new GridColumnDefinition("{1A3FAA08-4727-4102-861E-D18B1667A081}", 
                        new GridTextDisplayType(), "S: First Name", "John",
                        CustomerFields.ShipFirstName),

                    new GridColumnDefinition("{70479878-D70A-498a-B5AB-725977A0FC35}", true, 
                        new GridTextDisplayType(), "S: Last Name", "Smith",
                        CustomerFields.ShipLastName),

                    new GridColumnDefinition("{4E8C8D8D-9B41-4db5-A81F-02DBD33A0070}", 
                        new GridTextDisplayType(), "S: Middle Name", "Edward",
                        CustomerFields.ShipMiddleName),

                    new GridColumnDefinition("{410F3F50-FE2F-4c4f-90B8-8CCC7E2FD19D}", true, 
                        new GridTextDisplayType(), "S: Company", "Interapptive, Inc.",
                        CustomerFields.ShipCompany),

                    new GridColumnDefinition("{CC179597-B426-4c8a-956C-52493E76067B}", 
                        new GridTextDisplayType(), "S: Street1", "14 Main St.",
                        CustomerFields.ShipStreet1),

                    new GridColumnDefinition("{F22F4188-A52F-4d3a-BB87-CCAF41A341B8}", 
                        new GridTextDisplayType(), "S: Street2", "Apt. 1203",
                        CustomerFields.ShipStreet2),

                    new GridColumnDefinition("{54A13D9C-1AF1-41bd-8242-4B147042B3BF}", 
                        new GridTextDisplayType(), "S: Street3", "Attn: Jane Smith",
                        CustomerFields.ShipStreet3),

                    new GridColumnDefinition("{3695389D-45C5-4d80-B7E7-5E2AC135CEE5}", true, 
                        new GridTextDisplayType(), "S: City", "St. Louis",
                        CustomerFields.ShipCity),

                    new GridColumnDefinition("{95BF8D17-A463-444d-8EE4-6DDCD46A396F}", true, 
                        new GridStateDisplayType("Ship"), "S: State", "MO",
                        new GridColumnFunctionValueProvider(e => e),
                        new GridColumnSortProvider(CustomerFields.ShipStateProvCode)),

                    new GridColumnDefinition("{4B22E24F-5481-430d-9663-D55C1D6E9A10}", 
                        new GridTextDisplayType(), "S: Postal Code", "63132",
                        CustomerFields.ShipPostalCode),

                    new GridColumnDefinition("{D5330B7F-F991-49cc-980D-F8C662FA26F0}", true, 
                        new GridCountryDisplayType(), "S: Country", "US",
                        CustomerFields.ShipCountryCode),

                    new GridColumnDefinition("{3402BA73-D8BB-444f-9253-A252C1933A84}", 
                        new GridTextDisplayType(), "S: Phone", "1-314-555-0555",
                        CustomerFields.ShipPhone),

                    new GridColumnDefinition("{125FD515-8BE7-4210-A5F7-B41AA68F1398}", 
                        new GridTextDisplayType(), "S: Fax", "1-314-555-0554",
                        CustomerFields.ShipFax),

                    new GridColumnDefinition("{CD94B27B-8DDB-482f-9509-00CC74188350}", true, 
                        new GridTextDisplayType(), "S: Email", "john.smith@interapptive.com",
                        CustomerFields.ShipEmail),

                    new GridColumnDefinition("{BEAD9518-9E39-4b51-978F-539C389D04B0}", 
                        new GridTextDisplayType(), "S: Website", "www.interapptive.com",
                        CustomerFields.ShipWebsite),

                    new GridColumnDefinition("{2B109A2E-8B5A-4ab7-8932-82B1E422EC6D}", 
                        new GridTextDisplayType(), "B: First Name", "John",
                        CustomerFields.BillFirstName),

                    new GridColumnDefinition("{31EE09C4-C786-48b4-BE06-7411E130B74E}", 
                        new GridTextDisplayType(), "B: Last Name", "Smith",
                        CustomerFields.BillLastName),

                    new GridColumnDefinition("{80A20097-EC07-441b-9514-60DF2A33BB9D}", 
                        new GridTextDisplayType(), "B: Middle Name", "Edward",
                        CustomerFields.BillMiddleName),

                    new GridColumnDefinition("{8FA7180C-9CD6-4fa1-8555-8060215EE07E}", 
                        new GridTextDisplayType(), "B: Company", "Interapptive, Inc.",
                        CustomerFields.BillCompany),

                    new GridColumnDefinition("{BBD304D8-1827-4954-B125-ADBB7A8966CF}", 
                        new GridTextDisplayType(), "B: Street1", "14 Main St.",
                        CustomerFields.BillStreet1),

                    new GridColumnDefinition("{6FAADFCE-4FC6-4738-9707-9AE80859A0A6}", 
                        new GridTextDisplayType(), "B: Street2", "Apt. 1203",
                        CustomerFields.BillStreet2),

                    new GridColumnDefinition("{8403562A-DA56-443b-AE31-14FCA7C34608}", 
                        new GridTextDisplayType(), "B: Street3", "Attn: Jane Smith",
                        CustomerFields.BillStreet3),

                    new GridColumnDefinition("{2B564D7D-99FE-41eb-9863-72443F9104AC}", 
                        new GridTextDisplayType(), "B: City", "St. Louis",
                        CustomerFields.BillCity),

                    new GridColumnDefinition("{0EA673C7-D5D4-4131-A614-D45575100366}", 
                        new GridStateDisplayType("Bill"), "B: State", "MO",
                        new GridColumnFunctionValueProvider(e => e),
                        new GridColumnSortProvider(CustomerFields.BillStateProvCode)),

                    new GridColumnDefinition("{B803C7E2-823A-46c5-AA30-D0C0FC16E5B5}", 
                        new GridTextDisplayType(), "B: Postal Code", "63132",
                        CustomerFields.BillPostalCode),

                    new GridColumnDefinition("{0C44378C-B727-408b-9A97-5236228362B8}", 
                        new GridCountryDisplayType(), "B: Country", "US",
                        CustomerFields.BillCountryCode),

                    new GridColumnDefinition("{333C3851-F7FC-48ec-B834-3BA1D4512CF4}", 
                        new GridTextDisplayType(), "B: Phone", "1-314-555-0555",
                        CustomerFields.BillPhone),

                    new GridColumnDefinition("{D93B0AA6-32BA-4ec3-AAB1-5632EC346D50}", 
                        new GridTextDisplayType(), "B: Fax", "1-314-555-0554",
                        CustomerFields.BillFax),

                    new GridColumnDefinition("{83984FB4-8C47-4421-A74C-77413DCD08F3}", 
                        new GridTextDisplayType(), "B: Email", "john.smith@interapptive.com",
                        CustomerFields.BillEmail),

                    new GridColumnDefinition("{8A9492DA-DF0A-4dcb-9C72-B041A9A56DC0}", 
                        new GridTextDisplayType(), "B: Website", "www.interapptive.com",
                        CustomerFields.BillWebsite),

                    new GridColumnDefinition("{7C038321-5C87-4189-BC4B-841820906C6C}",  true,
                        new GridNoteDisplayType(), "Notes", 2,
                        CustomerFields.RollupNoteCount) { DefaultWidth = 40 },

                    new GridColumnDefinition("{F3D918A1-8C2C-4dd8-AAAF-9D73AF4B75D0}", true, 
                        new GridTextDisplayType(), "Orders", 3,
                        CustomerFields.RollupOrderCount) { DefaultWidth = 60 },  
              
                    new GridColumnDefinition("{0B3C111E-B9B2-4a98-8CEF-135600EF9E3D}", true, 
                        new GridMoneyDisplayType(), "Total", 1024.18m,
                        CustomerFields.RollupOrderTotal)   
                };

            return definitions;
        }
    }
}
