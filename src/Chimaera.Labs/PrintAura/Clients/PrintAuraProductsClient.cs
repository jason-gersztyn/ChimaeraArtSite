using System.Threading.Tasks;
using Chimaera.Labs.PrintAura.Models;
using RestSharp;

namespace Chimaera.Labs.PrintAura
{
    public class PrintAuraProductsClient : PrintAuraClientBase
    {
        public PrintAuraProductsClient() : base() { }

        public ListBrandsResponse ListBrands()
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListBrandsRequest());

            return Execute<ListBrandsResponse>(request);
        }

        public Task<ListBrandsResponse> ListBrandsAsync()
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListBrandsRequest());

            return ExecuteAsync<ListBrandsResponse>(request);
        }

        public ListColorsResponse ListColors()
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListColorsRequest());

            return Execute<ListColorsResponse>(request);
        }

        public Task<ListColorsResponse> ListColorsAsync()
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListColorsRequest());

            return ExecuteAsync<ListColorsResponse>(request);
        }

        public ListProductsResponse ListProducts()
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListProductsRequest());

            return Execute<ListProductsResponse>(request);
        }

        public Task<ListProductsResponse> ListProductsAsync()
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListProductsRequest());

            return ExecuteAsync<ListProductsResponse>(request);
        }

        public ListSizesResponse ListSizes()
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListSizesRequest());

            return Execute<ListSizesResponse>(request);
        }

        public Task<ListSizesResponse> ListSizesAsync()
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListSizesRequest());

            return ExecuteAsync<ListSizesResponse>(request);
        }

        public ViewProductsResponse ViewProducts(ViewProductsRequest viewProductsRequest)
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(viewProductsRequest);

            return Execute<ViewProductsResponse>(request);
        }

        public Task<ViewProductsResponse> ViewProductsAsync(ViewProductsRequest viewProductsRequest)
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(viewProductsRequest);

            return ExecuteAsync<ViewProductsResponse>(request);
        }
    }
}