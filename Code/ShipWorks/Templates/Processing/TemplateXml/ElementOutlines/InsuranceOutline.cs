using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Insurance;
using Interapptive.Shared.Utility;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Represents a single package for insurance display purposes
    /// </summary>
    public class InsuranceOutline : ElementOutline
    {
        IInsuranceChoice insuranceChoice;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("Provider", () => GetInsuranceProviderName(insuranceChoice));
            AddElement("InsuredValue", () => 
                (insuranceChoice.InsurancePennyOne == false && insuranceChoice.InsuranceProvider == InsuranceProvider.ShipWorks) 
                    ? Math.Max(0, insuranceChoice.InsuranceValue - 100) 
                    : insuranceChoice.InsuranceValue);
        }

        /// <summary>
        /// Get the name of the selected insurance provider
        /// </summary>
        private static string GetInsuranceProviderName(IInsuranceChoice insuranceChoice)
        {
            if (!insuranceChoice.Insured)
            {
                return "None";
            }
            else
            {
                return EnumHelper.GetDescription(insuranceChoice.InsuranceProvider);
            }
        }

        /// <summary>
        /// Bind a new cloned instance to the specified data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new InsuranceOutline(Context) { insuranceChoice = (IInsuranceChoice) data };
        }
    }
}
