using System.Threading.Tasks;
using Chimaera.Labs.PrintAura.Models;
using RestSharp;

namespace Chimaera.Labs.PrintAura
{
    public class PrintAuraOrdersClient : PrintAuraClientBase
    {
        public PrintAuraOrdersClient() : base() { }

        public AddOrderResponse AddOrder(AddOrderRequest addOrderRequest)
        {
            var request = new PrintAuraRestRequest
            {
                Method = Method.POST
            };

            request.AddJsonBody(addOrderRequest);

            return Execute<AddOrderResponse>(request);
        }

        public Task<AddOrderResponse> AddOrderAsync(AddOrderRequest addOrderRequest)
        {
            var request = new PrintAuraRestRequest
            {
                Method = Method.POST
            };

            request.AddJsonBody(addOrderRequest);

            return ExecuteAsync<AddOrderResponse>(request);
        }

        public ListOrdersResponse ListOrders()
        {
            var request = new PrintAuraRestRequest
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListOrdersRequest());

            return Execute<ListOrdersResponse>(request);
        }

        public Task<ListOrdersResponse> ListOrdersAsync()
        {
            var request = new PrintAuraRestRequest
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListOrdersRequest());

            return ExecuteAsync<ListOrdersResponse>(request);
        }
    }
}