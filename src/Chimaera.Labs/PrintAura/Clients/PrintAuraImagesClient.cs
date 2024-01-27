using System.Threading.Tasks;
using Chimaera.Labs.PrintAura.Models;
using RestSharp;

namespace Chimaera.Labs.PrintAura
{
    public class PrintAuraImagesClient : PrintAuraClientBase
    {
        public PrintAuraImagesClient() : base() { }

        public ListMyImagesResponse ListMyImages()
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListMyImagesRequest());

            return Execute<ListMyImagesResponse>(request);
        }

        public Task<ListMyImagesResponse> ListMyImagesAsync()
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(new ListMyImagesRequest());

            return ExecuteAsync<ListMyImagesResponse>(request);
        }

        public UploadImageFromUrlResponse UploadImageFromUrl(UploadImageFromUrlRequest uploadImageFromUrlRequest)
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(uploadImageFromUrlRequest);

            return Execute<UploadImageFromUrlResponse>(request);
        }

        public Task<UploadImageFromUrlResponse> UploadImageFromUrlAsync(UploadImageFromUrlRequest uploadImageFromUrlRequest)
        {
            var request = new PrintAuraRestRequest()
            {
                Method = Method.POST
            };

            request.AddJsonBody(uploadImageFromUrlRequest);

            return ExecuteAsync<UploadImageFromUrlResponse>(request);
        }
    }
}