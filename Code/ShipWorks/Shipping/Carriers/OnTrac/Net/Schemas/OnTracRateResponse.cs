﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.18020.
// 
namespace ShipWorks.Shipping.Carriers.OnTrac.Schemas.Rate {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.18020")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("OnTracRateResponse", Namespace="", IsNullable=true)]
    public partial class RateShipmentList {
        
        private RateShipment[] shipmentsField;
        
        private string errorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Shipment")]
        public RateShipment[] Shipments {
            get {
                return this.shipmentsField;
            }
            set {
                this.shipmentsField = value;
            }
        }
        
        /// <remarks/>
        public string Error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.18020")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RateShipment {
        
        private RateQuote[] ratesField;
        
        private string uIDField;
        
        private string delzipField;
        
        private string pUZipField;
        
        private double declaredField;
        
        private bool residentialField;
        
        private double cODField;
        
        private bool saturdayDelField;
        
        private double weightField;
        
        private Dim dIMField;
        
        private string errorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Rate")]
        public RateQuote[] Rates {
            get {
                return this.ratesField;
            }
            set {
                this.ratesField = value;
            }
        }
        
        /// <remarks/>
        public string UID {
            get {
                return this.uIDField;
            }
            set {
                this.uIDField = value;
            }
        }
        
        /// <remarks/>
        public string Delzip {
            get {
                return this.delzipField;
            }
            set {
                this.delzipField = value;
            }
        }
        
        /// <remarks/>
        public string PUZip {
            get {
                return this.pUZipField;
            }
            set {
                this.pUZipField = value;
            }
        }
        
        /// <remarks/>
        public double Declared {
            get {
                return this.declaredField;
            }
            set {
                this.declaredField = value;
            }
        }
        
        /// <remarks/>
        public bool Residential {
            get {
                return this.residentialField;
            }
            set {
                this.residentialField = value;
            }
        }
        
        /// <remarks/>
        public double COD {
            get {
                return this.cODField;
            }
            set {
                this.cODField = value;
            }
        }
        
        /// <remarks/>
        public bool SaturdayDel {
            get {
                return this.saturdayDelField;
            }
            set {
                this.saturdayDelField = value;
            }
        }
        
        /// <remarks/>
        public double Weight {
            get {
                return this.weightField;
            }
            set {
                this.weightField = value;
            }
        }
        
        /// <remarks/>
        public Dim DIM {
            get {
                return this.dIMField;
            }
            set {
                this.dIMField = value;
            }
        }
        
        /// <remarks/>
        public string Error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.18020")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RateQuote {
        
        private string serviceField;
        
        private double serviceChargeField;
        
        private ServiceChargeDetails serviceChargeDetailsField;
        
        private double fuelChargeField;
        
        private double totalChargeField;
        
        private decimal billedWeightField;
        
        private int transitDaysField;
        
        private string expectedDeliveryDateField;
        
        private string commitTimeField;
        
        private int rateZoneField;
        
        private double globalRateField;
        
        /// <remarks/>
        public string Service {
            get {
                return this.serviceField;
            }
            set {
                this.serviceField = value;
            }
        }
        
        /// <remarks/>
        public double ServiceCharge {
            get {
                return this.serviceChargeField;
            }
            set {
                this.serviceChargeField = value;
            }
        }
        
        /// <remarks/>
        public ServiceChargeDetails ServiceChargeDetails {
            get {
                return this.serviceChargeDetailsField;
            }
            set {
                this.serviceChargeDetailsField = value;
            }
        }
        
        /// <remarks/>
        public double FuelCharge {
            get {
                return this.fuelChargeField;
            }
            set {
                this.fuelChargeField = value;
            }
        }
        
        /// <remarks/>
        public double TotalCharge {
            get {
                return this.totalChargeField;
            }
            set {
                this.totalChargeField = value;
            }
        }
        
        /// <remarks/>
        public decimal BilledWeight {
            get {
                return this.billedWeightField;
            }
            set {
                this.billedWeightField = value;
            }
        }
        
        /// <remarks/>
        public int TransitDays {
            get {
                return this.transitDaysField;
            }
            set {
                this.transitDaysField = value;
            }
        }
        
        /// <remarks/>
        public string ExpectedDeliveryDate {
            get {
                return this.expectedDeliveryDateField;
            }
            set {
                this.expectedDeliveryDateField = value;
            }
        }
        
        /// <remarks/>
        public string CommitTime {
            get {
                return this.commitTimeField;
            }
            set {
                this.commitTimeField = value;
            }
        }
        
        /// <remarks/>
        public int RateZone {
            get {
                return this.rateZoneField;
            }
            set {
                this.rateZoneField = value;
            }
        }
        
        /// <remarks/>
        public double GlobalRate {
            get {
                return this.globalRateField;
            }
            set {
                this.globalRateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.18020")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ServiceChargeDetails {
        
        private double baseChargeField;
        
        private double cODChargeField;
        
        private double declaredChargeField;
        
        private double additionalChargesField;
        
        private double saturdayChargeField;
        
        /// <remarks/>
        public double BaseCharge {
            get {
                return this.baseChargeField;
            }
            set {
                this.baseChargeField = value;
            }
        }
        
        /// <remarks/>
        public double CODCharge {
            get {
                return this.cODChargeField;
            }
            set {
                this.cODChargeField = value;
            }
        }
        
        /// <remarks/>
        public double DeclaredCharge {
            get {
                return this.declaredChargeField;
            }
            set {
                this.declaredChargeField = value;
            }
        }
        
        /// <remarks/>
        public double AdditionalCharges {
            get {
                return this.additionalChargesField;
            }
            set {
                this.additionalChargesField = value;
            }
        }
        
        /// <remarks/>
        public double SaturdayCharge {
            get {
                return this.saturdayChargeField;
            }
            set {
                this.saturdayChargeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.18020")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Dim {
        
        private double lengthField;
        
        private double widthField;
        
        private double heightField;
        
        /// <remarks/>
        public double Length {
            get {
                return this.lengthField;
            }
            set {
                this.lengthField = value;
            }
        }
        
        /// <remarks/>
        public double Width {
            get {
                return this.widthField;
            }
            set {
                this.widthField = value;
            }
        }
        
        /// <remarks/>
        public double Height {
            get {
                return this.heightField;
            }
            set {
                this.heightField = value;
            }
        }
    }
}
