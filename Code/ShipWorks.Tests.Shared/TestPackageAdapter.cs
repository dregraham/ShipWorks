using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Tests.Shared
{
    public class TestPackageAdapter : IPackageAdapter
    {
        [SuppressMessage("CSharp", "CS0067: ",Justification = "For now, we're fine with fire and forget here")]
        public event PropertyChangedEventHandler PropertyChanged;
        public int Index { get; set; }
        public double Weight { get; set; }
        public double AdditionalWeight { get; set; }
        public bool ApplyAdditionalWeight { get; set; }
        public double DimsLength { get; set; }
        public double DimsWidth { get; set; }
        public double DimsHeight { get; set; }
        public long DimsProfileID { get; set; }
        public PackageTypeBinding PackagingType { get; set; }
        public IInsuranceChoice InsuranceChoice { get; set; }
        public string HashCode()
        {
            return this.HashCode();
        }
    }
}
