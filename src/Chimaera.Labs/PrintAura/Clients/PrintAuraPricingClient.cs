using Chimaera.Labs.PrintAura.Models;
using RestSharp;

namespace Chimaera.Labs.PrintAura
{
    public class PrintAuraPricingClient : PrintAuraClientBase
    {
        public PrintAuraPricingClient() : base() { }

        public GetAllPricingResponse GetAllPricing(GetAllPricingRequest getAllPricingRequest)
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(getAllPricingRequest);

            return Execute<GetAllPricingResponse>(request);
        }
    }
}