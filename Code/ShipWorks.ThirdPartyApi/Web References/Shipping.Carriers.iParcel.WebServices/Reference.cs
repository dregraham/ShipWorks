﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18052.
// 
#pragma warning disable 1591

namespace ShipWorks.Shipping.Carriers.iParcel.WebServices {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="XMLSOAPSoap", Namespace="http://www.i-parcel.com/soap/")]
    public partial class XMLSOAP : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback UploadXMLFileStringOperationCompleted;
        
        private System.Threading.SendOrPostCallback UploadXMLFileOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public XMLSOAP() {
            this.Url = "http://www.i-parcel.com/soap/xmlsoap.asmx";
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
        public event UploadXMLFileStringCompletedEventHandler UploadXMLFileStringCompleted;
        
        /// <remarks/>
        public event UploadXMLFileCompletedEventHandler UploadXMLFileCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.i-parcel.com/soap/UploadXMLFileString", RequestNamespace="http://www.i-parcel.com/soap/", ResponseNamespace="http://www.i-parcel.com/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UploadXMLFileString(string requestFor, string xmlFile) {
            object[] results = this.Invoke("UploadXMLFileString", new object[] {
                        requestFor,
                        xmlFile});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void UploadXMLFileStringAsync(string requestFor, string xmlFile) {
            this.UploadXMLFileStringAsync(requestFor, xmlFile, null);
        }
        
        /// <remarks/>
        public void UploadXMLFileStringAsync(string requestFor, string xmlFile, object userState) {
            if ((this.UploadXMLFileStringOperationCompleted == null)) {
                this.UploadXMLFileStringOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadXMLFileStringOperationCompleted);
            }
            this.InvokeAsync("UploadXMLFileString", new object[] {
                        requestFor,
                        xmlFile}, this.UploadXMLFileStringOperationCompleted, userState);
        }
        
        private void OnUploadXMLFileStringOperationCompleted(object arg) {
            if ((this.UploadXMLFileStringCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadXMLFileStringCompleted(this, new UploadXMLFileStringCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.i-parcel.com/soap/UploadXMLFile", RequestNamespace="http://www.i-parcel.com/soap/", ResponseNamespace="http://www.i-parcel.com/soap/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet UploadXMLFile(string requestFor, string xmlFile) {
            object[] results = this.Invoke("UploadXMLFile", new object[] {
                        requestFor,
                        xmlFile});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void UploadXMLFileAsync(string requestFor, string xmlFile) {
            this.UploadXMLFileAsync(requestFor, xmlFile, null);
        }
        
        /// <remarks/>
        public void UploadXMLFileAsync(string requestFor, string xmlFile, object userState) {
            if ((this.UploadXMLFileOperationCompleted == null)) {
                this.UploadXMLFileOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadXMLFileOperationCompleted);
            }
            this.InvokeAsync("UploadXMLFile", new object[] {
                        requestFor,
                        xmlFile}, this.UploadXMLFileOperationCompleted, userState);
        }
        
        private void OnUploadXMLFileOperationCompleted(object arg) {
            if ((this.UploadXMLFileCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UploadXMLFileCompleted(this, new UploadXMLFileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void UploadXMLFileStringCompletedEventHandler(object sender, UploadXMLFileStringCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadXMLFileStringCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadXMLFileStringCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void UploadXMLFileCompletedEventHandler(object sender, UploadXMLFileCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UploadXMLFileCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal UploadXMLFileCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591