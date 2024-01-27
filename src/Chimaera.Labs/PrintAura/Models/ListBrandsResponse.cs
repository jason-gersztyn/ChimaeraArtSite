using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chimaera.Labs.PrintAura.Models
{
    public class ListBrandsResponse : Response
    {
        [JsonProperty("results")]
        public List<ListBrand> Brands { get; set; }
    }

    public class ListBrand
    {
        [JsonProperty("brand_id")]
        public int BrandId { get; set; }

        [JsonProperty("brand_name")]
        public string BrandName { get; set; }
    }
}