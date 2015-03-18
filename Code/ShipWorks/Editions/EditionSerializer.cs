using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Interapptive.Shared.Utility;
using ShipWorks.Editions.Freemium;
using ShipWorks.Editions.Brown;
using System.Xml.Linq;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Editions
{
    /// <summary>
    /// Responsibile for saving and restoring the state of Editions
    /// </summary>
    public static class EditionSerializer
    {
        /// <summary>
        /// Serialize the given edition
        /// </summary>
        public static string Serialize(Edition edition)
        {            
            XElement root = new XElement("Edition");
            root.Add(new XAttribute("identifier", GetIdentifier(edition)));

            root.Add(CreateMemento(edition));

            return SecureText.Encrypt(root.ToString(), "edition");
        }

        /// <summary>
        /// Restore the concrete edition implementation from the given state
        /// </summary>
        public static Edition Restore(StoreEntity store)
        {
            string state = store.Edition;

            if (string.IsNullOrWhiteSpace(state))
            {
                return new Edition(store);
            }

            if (InterapptiveOnly.IsInterapptiveUser)
            {
                if (state == "ups")
                {
                    return new BrownSubsidizedEdition(store, new string[] { "6Y3F12" }, BrownPostalAvailability.ApoFpoPobox);
                }

                if (state == "endicia")
                {
                    return new FreemiumFreeEdition(store, "", FreemiumAccountType.None);
                }
            }

            XElement root = XElement.Parse(SecureText.Decrypt(state, "edition"));
            string identifier = (string) root.Attribute("identifier");

            IEnumerable<XElement> xMemento = root.Elements() ?? new List<XElement>();

            Edition edition = InstantiateEdition(store, identifier, xMemento);

            ApplySharedOptions(edition, xMemento);

            edition.ShipmentTypeFunctionality = ShipmentTypeFunctionality.Deserialize(store.StoreID, root);

            return edition;
        }


        /// <summary>
        /// Insantiate an edition based on the given identifier and memento data
        /// </summary>
        private static Edition InstantiateEdition(StoreEntity store, string identifier, IEnumerable<XElement> xMemento)
        {
            switch (identifier)
            {
                case "standard": return new Edition(store);
                case "freemiumFree": return RestoreFreemiumFree(xMemento, store);
                case "freemiumPaid": return new FreemiumPaidEdition(store);
                case "upsDiscounted": return RestoreBrownDiscounted(xMemento, store);
                case "upsSubsidized": return RestoreBrownSubsidized(xMemento, store);
                case "upsCtp2014": return RestoreBrownCtp2014(xMemento, store);
                case "srendicia": return new ShipRushEndiciaEdition(store);
            }

            throw new InvalidOperationException("Unhandled Edition identifier: " + identifier);
        }
        
        /// <summary>
        /// Apply all the options that are shared accross editions
        /// </summary>
        private static void ApplySharedOptions(Edition edition, IEnumerable<XElement> xMemento)
        {
            XElement xOptions = xMemento.SingleOrDefault(x => x.Name == "SharedOptions");

            if (xOptions != null)
            {
                bool dhlEnabled = (bool) xOptions.Element("EndiciaDhl");
                edition.SharedOptions.EndiciaDhlEnabled = dhlEnabled;

                bool endiciaInsuranceEnabled = (bool) xOptions.Element("EndiciaInsurance");
                edition.SharedOptions.EndiciaInsuranceEnabled = endiciaInsuranceEnabled;
                
                bool upsSurePostEnabled = xOptions.Descendants("UpsSurePost").Any() && (bool) xOptions.Element("UpsSurePost");
                edition.SharedOptions.UpsSurePostEnabled = upsSurePostEnabled;

                bool endiciaConsolidator = xOptions.Descendants("EndiciaConsolidator").Any() && (bool) xOptions.Element("EndiciaConsolidator");
                edition.SharedOptions.EndiciaConsolidatorEnabled = endiciaConsolidator;

                bool endiciaScanBasedReturns = xOptions.Descendants("EndiciaScanBasedReturns").Any() && (bool)xOptions.Element("EndiciaScanBasedReturns");
                edition.SharedOptions.EndiciaScanBasedReturnEnabled = endiciaScanBasedReturns;

                bool stampsInsuranceEnabled = (bool)xOptions.Element("StampsInsurance");
                edition.SharedOptions.StampsInsuranceEnabled = stampsInsuranceEnabled;
            }
        }
        
        /// <summary>
        /// Get a type string to use to serialize the given edition
        /// </summary>
        private static string GetIdentifier(Edition edition)
        {
            Type type = edition.GetType();

            if (type == typeof(Edition))                return "standard";
            if (type == typeof(FreemiumFreeEdition))    return "freemiumFree";
            if (type == typeof(FreemiumPaidEdition))    return "freemiumPaid";
            if (type == typeof(BrownDiscountedEdition)) return "upsDiscounted";
            if (type == typeof(BrownSubsidizedEdition)) return "upsSubsidized";
            if (type == typeof(BrownCtp2014Edition))    return "upsCtp2014";
            if (type == typeof(ShipRushEndiciaEdition)) return "srendicia";

            throw new InvalidOperationException("Unhandled Edition type: " + type);
        }

        /// <summary>
        /// Create a memento to use for the given edition
        /// </summary>
        private static IEnumerable<XElement> CreateMemento(Edition edition)
        {
            List<XElement> elements = new List<XElement>();

            switch (GetIdentifier(edition))
            {
                case "freemiumFree":
                    FreemiumFreeEdition freemium = (FreemiumFreeEdition) edition;

                    elements.Add(new XElement("FreemiumAccount",
                        new XAttribute("type", (int) freemium.AccountType),
                        freemium.AccountNumber));

                    break;

                case "upsSubsidized":
                case "upsDiscounted":
                case "upsCtp2014":

                    BrownSubsidizedEdition subsidized = edition as BrownSubsidizedEdition;
                    if (subsidized != null)
                    {
                        elements.Add(new XElement("UpsAccounts",
                            subsidized.UpsAccounts.Select(a => new XElement("AccountNumber", a))));
                    }

                    elements.Add(new XElement("PostalAvailability", (int) ((BrownEdition) edition).PostalAvailability));

                    break;
            }

            elements.Add(new XElement("SharedOptions",
                new XElement("EndiciaDhl", edition.SharedOptions.EndiciaDhlEnabled),
                new XElement("EndiciaInsurance", edition.SharedOptions.EndiciaInsuranceEnabled),
                new XElement("UpsSurePost", edition.SharedOptions.UpsSurePostEnabled),
                new XElement("EndiciaConsolidator", edition.SharedOptions.EndiciaConsolidatorEnabled),
                new XElement("EndiciaScanBasedReturns", edition.SharedOptions.EndiciaScanBasedReturnEnabled),
                new XElement("StampsInsurance", edition.SharedOptions.StampsInsuranceEnabled)));

            if (edition.ShipmentTypeFunctionality != null)
            {
                elements.Add(edition.ShipmentTypeFunctionality.ToXElement());
            }

            return elements;
        }

        /// <summary>
        /// Restore the BrownSubsidized edition from the given memento
        /// </summary>
        private static Edition RestoreBrownSubsidized(IEnumerable<XElement> memento, StoreEntity store)
        {
            var upsAccounts = memento.Single(x => x.Name == "UpsAccounts").Elements().Select(e => (string) e);

            BrownPostalAvailability postalAvailability = BrownPostalAvailability.ApoFpoPobox;

            XElement xPostal = memento.SingleOrDefault(x => x.Name == "PostalAvailability");
            if (xPostal != null)
            {
                postalAvailability = (BrownPostalAvailability) (int) xPostal;
            }

            return new BrownSubsidizedEdition(store, upsAccounts, postalAvailability);
        }

        /// <summary>
        /// Restore the BrownDiscounted edition from the given memento
        /// </summary>
        private static Edition RestoreBrownDiscounted(IEnumerable<XElement> memento, StoreEntity store)
        {
            BrownPostalAvailability postalAvailability = BrownPostalAvailability.ApoFpoPobox;

            XElement xPostal = memento.SingleOrDefault(x => x.Name == "PostalAvailability");
            if (xPostal != null)
            {
                postalAvailability = (BrownPostalAvailability) (int) xPostal;
            }

            return new BrownDiscountedEdition(store, postalAvailability);
        }

        /// <summary>
        /// Restore the BrownCtp2014 edition from the given memento
        /// </summary>
        private static Edition RestoreBrownCtp2014(IEnumerable<XElement> memento, StoreEntity store)
        {
            BrownPostalAvailability postalAvailability = BrownPostalAvailability.ApoFpoPobox;

            XElement xPostal = memento.SingleOrDefault(x => x.Name == "PostalAvailability");
            if (xPostal != null)
            {
                postalAvailability = (BrownPostalAvailability) (int) xPostal;
            }

            return new BrownCtp2014Edition(store, postalAvailability);
        }

        /// <summary>
        /// Restore the freemium free edition from the given memento
        /// </summary>
        private static Edition RestoreFreemiumFree(IEnumerable<XElement> memento, StoreEntity store)
        {
            XElement xAccount = memento.First();

            return new FreemiumFreeEdition(store, (string) xAccount, (FreemiumAccountType) (int) xAccount.Attribute("type"));
        }
    }
}
