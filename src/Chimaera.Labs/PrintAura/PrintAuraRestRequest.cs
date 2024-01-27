using System;
using RestSharp;

namespace Chimaera.Labs.PrintAura
{
    public class PrintAuraRestRequest : RestRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintAuraRestRequest"/> class.
        /// </summary>
        public PrintAuraRestRequest()
        {
            IntializeJsonSerializer();
            OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
        }

        /// <summary>
        /// Sets Method property to value of method
        /// 
        /// </summary>
        /// <param name="method">Method to use for this request</param>
        public PrintAuraRestRequest(Method method) : base(method)
        {
            IntializeJsonSerializer();
        }

        /// <summary>
        /// Sets Resource property
        /// 
        /// </summary>
        /// <param name="resource">Resource to use for this request</param>
        public PrintAuraRestRequest(string resource) : base(resource)
        {
            IntializeJsonSerializer();
        }

        /// <summary>
        /// Sets Resource and Method properties
        /// 
        /// </summary>
        /// <param name="resource">Resource to use for this request</param><param name="method">Method to use for this request</param>
        public PrintAuraRestRequest(string resource, Method method) : base(resource, method)
        {
            IntializeJsonSerializer();
        }

        /// <summary>
        /// Sets Resource property
        /// 
        /// </summary>
        /// <param name="resource">Resource to use for this request</param>
        public PrintAuraRestRequest(Uri resource) : base(resource)
        {
            IntializeJsonSerializer();
        }

        /// <summary>
        /// Sets Resource and Method properties
        /// 
        /// </summary>
        /// <param name="resource">Resource to use for this request</param><param name="method">Method to use for this request</param>
        public PrintAuraRestRequest(Uri resource, Method method) : base(resource, method)
        {
            IntializeJsonSerializer();
        }

        /// <summary>
        /// Intializes the serializer.
        /// </summary>
        protected virtual void IntializeJsonSerializer()
        {
            JsonSerializer = new PrintAuraJsonSerializer();
        }
    }
}