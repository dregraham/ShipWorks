using System;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Interapptive.Shared.ComponentRegistration;
using Newtonsoft.Json;
using ShipWorks.Api.Partner.StreamTech;

namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// Model Binder that uses Json.Net to deserialize requests
    /// </summary>
    [Component(RegistrationType.Self)]
    public class JsonNetModelBinder : IModelBinder
    {
        /// <summary>
        /// bind the given model
        /// </summary>
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            string input = actionContext.Request.Content.ReadAsStringAsync().Result;

            bindingContext.Model = JsonConvert.DeserializeObject(input, bindingContext.ModelType);

            return true;
        }
    }
}
