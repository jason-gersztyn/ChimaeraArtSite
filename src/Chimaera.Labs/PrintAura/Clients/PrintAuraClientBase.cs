using System;
using System.Threading.Tasks;
using Chimaera.Labs.PrintAura.Models;
using RestSharp;

namespace Chimaera.Labs.PrintAura
{
    public abstract class PrintAuraClientBase
    {
        private const string BASE_URL = "http://www.api.printaura.com/api.php";

        private readonly RestClient _restClient;

        public PrintAuraClientBase()
        {
            _restClient = new RestClient(BASE_URL);
            _restClient.AddDefaultHeader("Content-Type", "application/json");
            _restClient.AddHandler("application/json", new PrintAuraJsonSerializer());
        }
        
        internal TResponse Execute<TResponse>(IRestRequest request) where TResponse : Response, new()
        {
            var restResponse = _restClient.Execute<TResponse>(request);
            var response = restResponse.Data ?? Activator.CreateInstance<TResponse>();

            return response;
        }

        internal async Task<TResponse> ExecuteAsync<TResponse>(IRestRequest request) where TResponse : Response
        {
            var restResponse = await _restClient.ExecuteTaskAsync<TResponse>(request);
            var response = restResponse.Data ?? Activator.CreateInstance<TResponse>();

            return response;
        }
    }
}