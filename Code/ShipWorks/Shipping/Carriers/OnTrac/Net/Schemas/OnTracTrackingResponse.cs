﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 
namespace ShipWorks.Shipping.Carriers.OnTrac.Schemas.Tracking {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("OnTracTrackingResult", Namespace="", IsNullable=true)]
    public partial class TrackingShipmentList {
        
        private TrackingShipment[] shipmentsField;
        
        private string noteField;
        
        private string logoField;
        
        private string errorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Shipment")]
        public TrackingShipment[] Shipments {
            get {
                return this.shipmentsField;
            }
            set {
                this.shipmentsField = value;
            }
        }
        
        /// <remarks/>
        public string Note {
            get {
                return this.noteField;
            }
            set {
                this.noteField = value;
            }
        }
        
        /// <remarks/>
        public string Logo {
            get {
                return this.logoField;
            }
            set {
                this.logoField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TrackingShipment {
        
        private Event[] eventsField;
        
        private string trackingField;
        
        private System.DateTime exp_Del_DateField;
        
        private System.DateTime shipDateField;
        
        private bool deliveredField;
        
        private string nameField;
        
        private string contactField;
        
        private string addr1Field;
        
        private string addr2Field;
        
        private string addr3Field;
        
        private string cityField;
        
        private string stateField;
        
        private string zipField;
        
        private string serviceField;
        
        private string pODField;
        
        private string errorField;
        
        private string referenceField;
        
        private string reference2Field;
        
        private string reference3Field;
        
        private double serviceChargeField;
        
        private double fuelChargeField;
        
        private double totalChrgField;
        
        private bool residentialField;
        
        private double weightField;
        
        private string signatureField;
        
        /// <remarks/>
        public Event[] Events {
            get {
                return this.eventsField;
            }
            set {
                this.eventsField = value;
            }
        }
        
        /// <remarks/>
        public string Tracking {
            get {
                return this.trackingField;
            }
            set {
                this.trackingField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime Exp_Del_Date {
            get {
                return this.exp_Del_DateField;
            }
            set {
                this.exp_Del_DateField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime ShipDate {
            get {
                return this.shipDateField;
            }
            set {
                this.shipDateField = value;
            }
        }
        
        /// <remarks/>
        public bool Delivered {
            get {
                return this.deliveredField;
            }
            set {
                this.deliveredField = value;
            }
        }
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string Contact {
            get {
                return this.contactField;
            }
            set {
                this.contactField = value;
            }
        }
        
        /// <remarks/>
        public string Addr1 {
            get {
                return this.addr1Field;
            }
            set {
                this.addr1Field = value;
            }
        }
        
        /// <remarks/>
        public string Addr2 {
            get {
                return this.addr2Field;
            }
            set {
                this.addr2Field = value;
            }
        }
        
        /// <remarks/>
        public string Addr3 {
            get {
                return this.addr3Field;
            }
            set {
                this.addr3Field = value;
            }
        }
        
        /// <remarks/>
        public string City {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
            }
        }
        
        /// <remarks/>
        public string State {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
            }
        }
        
        /// <remarks/>
        public string Zip {
            get {
                return this.zipField;
            }
            set {
                this.zipField = value;
            }
        }
        
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
        public string POD {
            get {
                return this.pODField;
            }
            set {
                this.pODField = value;
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
        
        /// <remarks/>
        public string Reference {
            get {
                return this.referenceField;
            }
            set {
                this.referenceField = value;
            }
        }
        
        /// <remarks/>
        public string Reference2 {
            get {
                return this.reference2Field;
            }
            set {
                this.reference2Field = value;
            }
        }
        
        /// <remarks/>
        public string Reference3 {
            get {
                return this.reference3Field;
            }
            set {
                this.reference3Field = value;
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
        public double FuelCharge {
            get {
                return this.fuelChargeField;
            }
            set {
                this.fuelChargeField = value;
            }
        }
        
        /// <remarks/>
        public double TotalChrg {
            get {
                return this.totalChrgField;
            }
            set {
                this.totalChrgField = value;
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
        public double Weight {
            get {
                return this.weightField;
            }
            set {
                this.weightField = value;
            }
        }
        
        /// <remarks/>
        public string Signature {
            get {
                return this.signatureField;
            }
            set {
                this.signatureField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Event {
        
        private string statusField;
        
        private string descriptionField;
        
        private System.DateTime eventTimeField;
        
        private string facilityField;
        
        private string cityField;
        
        private string stateField;
        
        private string zipField;
        
        /// <remarks/>
        public string Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime EventTime {
            get {
                return this.eventTimeField;
            }
            set {
                this.eventTimeField = value;
            }
        }
        
        /// <remarks/>
        public string Facility {
            get {
                return this.facilityField;
            }
            set {
                this.facilityField = value;
            }
        }
        
        /// <remarks/>
        public string City {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
            }
        }
        
        /// <remarks/>
        public string State {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
            }
        }
        
        /// <remarks/>
        public string Zip {
            get {
                return this.zipField;
            }
            set {
                this.zipField = value;
            }
        }
    }
}
