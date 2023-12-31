﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace ShipWorks.ApplicationCore.Licensing.Activation.WebServices
{


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="BasicHttpBinding_IActivationV1", Namespace="http://stamps.com/xml/namespace/2015/09/shipworks/activationv1")]
    public partial class Activation : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetCustomerLicenseInfoOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Activation() {
            this.Url = "http://shpwrk003.qasc.stamps.com/ShipWorksNet/ActivationV1.svc";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetCustomerLicenseInfoCompletedEventHandler GetCustomerLicenseInfoCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://stamps.com/xml/namespace/2015/09/shipworks/activationv1/IActivationV1/GetC" +
            "ustomerLicenseInfo", RequestNamespace="http://stamps.com/xml/namespace/2015/09/shipworks/activationv1", ResponseNamespace="http://stamps.com/xml/namespace/2015/09/shipworks/activationv1", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public CustomerLicenseInfoV1 GetCustomerLicenseInfo([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string email, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string password) {
            object[] results = this.Invoke("GetCustomerLicenseInfo", new object[] {
                        email,
                        password});
            return ((CustomerLicenseInfoV1)(results[0]));
        }
        
        /// <remarks/>
        public void GetCustomerLicenseInfoAsync(string email, string password) {
            this.GetCustomerLicenseInfoAsync(email, password, null);
        }
        
        /// <remarks/>
        public void GetCustomerLicenseInfoAsync(string email, string password, object userState) {
            if ((this.GetCustomerLicenseInfoOperationCompleted == null)) {
                this.GetCustomerLicenseInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCustomerLicenseInfoOperationCompleted);
            }
            this.InvokeAsync("GetCustomerLicenseInfo", new object[] {
                        email,
                        password}, this.GetCustomerLicenseInfoOperationCompleted, userState);
        }
        
        private void OnGetCustomerLicenseInfoOperationCompleted(object arg) {
            if ((this.GetCustomerLicenseInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCustomerLicenseInfoCompleted(this, new GetCustomerLicenseInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/Sdc.Server.ShipWorksNet.Protocol.Customer" +
        "LicenseInfo")]
    public partial class CustomerLicenseInfoV1 {
        
        private string associatedStampsUserNameField;
        
        private string customerLicenseKeyField;
        
        private string legacyCustomerLicenseKeyField;
        
        private bool isLegacyUserField;
        
        private string stampsUserNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string AssociatedStampsUserName {
            get {
                return this.associatedStampsUserNameField;
            }
            set {
                this.associatedStampsUserNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string CustomerLicenseKey {
            get {
                return this.customerLicenseKeyField;
            }
            set {
                this.customerLicenseKeyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string LegacyCustomerLicenseKey {
            get {
                return this.legacyCustomerLicenseKeyField;
            }
            set {
                this.legacyCustomerLicenseKeyField = value;
            }
        }
        
        /// <remarks/>
        public bool IsLegacyUser {
            get {
                return this.isLegacyUserField;
            }
            set {
                this.isLegacyUserField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string StampsUserName {
            get {
                return this.stampsUserNameField;
            }
            set {
                this.stampsUserNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void GetCustomerLicenseInfoCompletedEventHandler(object sender, GetCustomerLicenseInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCustomerLicenseInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetCustomerLicenseInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public CustomerLicenseInfoV1 Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((CustomerLicenseInfoV1)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591