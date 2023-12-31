﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.225.
// 
#pragma warning disable 1591

namespace ShipWorks.Stores.Platforms.Magento.WebServices {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="Mage_Api_Model_Server_HandlerBinding", Namespace="urn:Magento")]
    public partial class MagentoService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback callOperationCompleted;
        
        private System.Threading.SendOrPostCallback multiCallOperationCompleted;
        
        private System.Threading.SendOrPostCallback endSessionOperationCompleted;
        
        private System.Threading.SendOrPostCallback loginOperationCompleted;
        
        private System.Threading.SendOrPostCallback startSessionOperationCompleted;
        
        private System.Threading.SendOrPostCallback resourcesOperationCompleted;
        
        private System.Threading.SendOrPostCallback globalFaultsOperationCompleted;
        
        private System.Threading.SendOrPostCallback resourceFaultsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MagentoService() {
            this.Url = "http://magento151.interapptive.local/index.php/api/soap/index/";
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
        public event callCompletedEventHandler callCompleted;
        
        /// <remarks/>
        public event multiCallCompletedEventHandler multiCallCompleted;
        
        /// <remarks/>
        public event endSessionCompletedEventHandler endSessionCompleted;
        
        /// <remarks/>
        public event loginCompletedEventHandler loginCompleted;
        
        /// <remarks/>
        public event startSessionCompletedEventHandler startSessionCompleted;
        
        /// <remarks/>
        public event resourcesCompletedEventHandler resourcesCompleted;
        
        /// <remarks/>
        public event globalFaultsCompletedEventHandler globalFaultsCompleted;
        
        /// <remarks/>
        public event resourceFaultsCompletedEventHandler resourceFaultsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:Mage_Api_Model_Server_HandlerAction", RequestNamespace="urn:Magento", ResponseNamespace="urn:Magento")]
        [return: System.Xml.Serialization.SoapElementAttribute("callReturn")]
        public object call(string sessionId, string resourcePath, object args) {
            object[] results = this.Invoke("call", new object[] {
                        sessionId,
                        resourcePath,
                        args});
            return ((object)(results[0]));
        }
        
        /// <remarks/>
        public void callAsync(string sessionId, string resourcePath, object args) {
            this.callAsync(sessionId, resourcePath, args, null);
        }
        
        /// <remarks/>
        public void callAsync(string sessionId, string resourcePath, object args, object userState) {
            if ((this.callOperationCompleted == null)) {
                this.callOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallOperationCompleted);
            }
            this.InvokeAsync("call", new object[] {
                        sessionId,
                        resourcePath,
                        args}, this.callOperationCompleted, userState);
        }
        
        private void OncallOperationCompleted(object arg) {
            if ((this.callCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callCompleted(this, new callCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:Mage_Api_Model_Server_HandlerAction", RequestNamespace="urn:Magento", ResponseNamespace="urn:Magento")]
        [return: System.Xml.Serialization.SoapElementAttribute("multiCallReturn")]
        public object[] multiCall(string sessionId, object[] calls, object options) {
            object[] results = this.Invoke("multiCall", new object[] {
                        sessionId,
                        calls,
                        options});
            return ((object[])(results[0]));
        }
        
        /// <remarks/>
        public void multiCallAsync(string sessionId, object[] calls, object options) {
            this.multiCallAsync(sessionId, calls, options, null);
        }
        
        /// <remarks/>
        public void multiCallAsync(string sessionId, object[] calls, object options, object userState) {
            if ((this.multiCallOperationCompleted == null)) {
                this.multiCallOperationCompleted = new System.Threading.SendOrPostCallback(this.OnmultiCallOperationCompleted);
            }
            this.InvokeAsync("multiCall", new object[] {
                        sessionId,
                        calls,
                        options}, this.multiCallOperationCompleted, userState);
        }
        
        private void OnmultiCallOperationCompleted(object arg) {
            if ((this.multiCallCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.multiCallCompleted(this, new multiCallCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:Mage_Api_Model_Server_HandlerAction", RequestNamespace="urn:Magento", ResponseNamespace="urn:Magento")]
        [return: System.Xml.Serialization.SoapElementAttribute("endSessionReturn")]
        public bool endSession(string sessionId) {
            object[] results = this.Invoke("endSession", new object[] {
                        sessionId});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void endSessionAsync(string sessionId) {
            this.endSessionAsync(sessionId, null);
        }
        
        /// <remarks/>
        public void endSessionAsync(string sessionId, object userState) {
            if ((this.endSessionOperationCompleted == null)) {
                this.endSessionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnendSessionOperationCompleted);
            }
            this.InvokeAsync("endSession", new object[] {
                        sessionId}, this.endSessionOperationCompleted, userState);
        }
        
        private void OnendSessionOperationCompleted(object arg) {
            if ((this.endSessionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.endSessionCompleted(this, new endSessionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:Mage_Api_Model_Server_HandlerAction", RequestNamespace="urn:Magento", ResponseNamespace="urn:Magento")]
        [return: System.Xml.Serialization.SoapElementAttribute("loginReturn")]
        public string login(string username, string apiKey) {
            object[] results = this.Invoke("login", new object[] {
                        username,
                        apiKey});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void loginAsync(string username, string apiKey) {
            this.loginAsync(username, apiKey, null);
        }
        
        /// <remarks/>
        public void loginAsync(string username, string apiKey, object userState) {
            if ((this.loginOperationCompleted == null)) {
                this.loginOperationCompleted = new System.Threading.SendOrPostCallback(this.OnloginOperationCompleted);
            }
            this.InvokeAsync("login", new object[] {
                        username,
                        apiKey}, this.loginOperationCompleted, userState);
        }
        
        private void OnloginOperationCompleted(object arg) {
            if ((this.loginCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.loginCompleted(this, new loginCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:Mage_Api_Model_Server_HandlerAction", RequestNamespace="urn:Magento", ResponseNamespace="urn:Magento")]
        [return: System.Xml.Serialization.SoapElementAttribute("startSessionReturn")]
        public string startSession() {
            object[] results = this.Invoke("startSession", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void startSessionAsync() {
            this.startSessionAsync(null);
        }
        
        /// <remarks/>
        public void startSessionAsync(object userState) {
            if ((this.startSessionOperationCompleted == null)) {
                this.startSessionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnstartSessionOperationCompleted);
            }
            this.InvokeAsync("startSession", new object[0], this.startSessionOperationCompleted, userState);
        }
        
        private void OnstartSessionOperationCompleted(object arg) {
            if ((this.startSessionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.startSessionCompleted(this, new startSessionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:Mage_Api_Model_Server_HandlerAction", RequestNamespace="urn:Magento", ResponseNamespace="urn:Magento")]
        [return: System.Xml.Serialization.SoapElementAttribute("resourcesReturn")]
        public object[] resources(string sessionId) {
            object[] results = this.Invoke("resources", new object[] {
                        sessionId});
            return ((object[])(results[0]));
        }
        
        /// <remarks/>
        public void resourcesAsync(string sessionId) {
            this.resourcesAsync(sessionId, null);
        }
        
        /// <remarks/>
        public void resourcesAsync(string sessionId, object userState) {
            if ((this.resourcesOperationCompleted == null)) {
                this.resourcesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnresourcesOperationCompleted);
            }
            this.InvokeAsync("resources", new object[] {
                        sessionId}, this.resourcesOperationCompleted, userState);
        }
        
        private void OnresourcesOperationCompleted(object arg) {
            if ((this.resourcesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.resourcesCompleted(this, new resourcesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:Mage_Api_Model_Server_HandlerAction", RequestNamespace="urn:Magento", ResponseNamespace="urn:Magento")]
        [return: System.Xml.Serialization.SoapElementAttribute("globalFaultsReturn")]
        public object[] globalFaults(string sessionId) {
            object[] results = this.Invoke("globalFaults", new object[] {
                        sessionId});
            return ((object[])(results[0]));
        }
        
        /// <remarks/>
        public void globalFaultsAsync(string sessionId) {
            this.globalFaultsAsync(sessionId, null);
        }
        
        /// <remarks/>
        public void globalFaultsAsync(string sessionId, object userState) {
            if ((this.globalFaultsOperationCompleted == null)) {
                this.globalFaultsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnglobalFaultsOperationCompleted);
            }
            this.InvokeAsync("globalFaults", new object[] {
                        sessionId}, this.globalFaultsOperationCompleted, userState);
        }
        
        private void OnglobalFaultsOperationCompleted(object arg) {
            if ((this.globalFaultsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.globalFaultsCompleted(this, new globalFaultsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:Mage_Api_Model_Server_HandlerAction", RequestNamespace="urn:Magento", ResponseNamespace="urn:Magento")]
        [return: System.Xml.Serialization.SoapElementAttribute("resourceFaultsReturn")]
        public object[] resourceFaults(string resourceName, string sessionId) {
            object[] results = this.Invoke("resourceFaults", new object[] {
                        resourceName,
                        sessionId});
            return ((object[])(results[0]));
        }
        
        /// <remarks/>
        public void resourceFaultsAsync(string resourceName, string sessionId) {
            this.resourceFaultsAsync(resourceName, sessionId, null);
        }
        
        /// <remarks/>
        public void resourceFaultsAsync(string resourceName, string sessionId, object userState) {
            if ((this.resourceFaultsOperationCompleted == null)) {
                this.resourceFaultsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnresourceFaultsOperationCompleted);
            }
            this.InvokeAsync("resourceFaults", new object[] {
                        resourceName,
                        sessionId}, this.resourceFaultsOperationCompleted, userState);
        }
        
        private void OnresourceFaultsOperationCompleted(object arg) {
            if ((this.resourceFaultsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.resourceFaultsCompleted(this, new resourceFaultsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void callCompletedEventHandler(object sender, callCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal callCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void multiCallCompletedEventHandler(object sender, multiCallCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class multiCallCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal multiCallCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void endSessionCompletedEventHandler(object sender, endSessionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class endSessionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal endSessionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void loginCompletedEventHandler(object sender, loginCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class loginCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal loginCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void startSessionCompletedEventHandler(object sender, startSessionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class startSessionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal startSessionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void resourcesCompletedEventHandler(object sender, resourcesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class resourcesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal resourcesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void globalFaultsCompletedEventHandler(object sender, globalFaultsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class globalFaultsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal globalFaultsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void resourceFaultsCompletedEventHandler(object sender, resourceFaultsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class resourceFaultsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal resourceFaultsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591