using System.Threading.Tasks;
using Chimaera.Beasts.Extensions;
using Chimaera.Labs.PrintAura;
using Xunit;

namespace Chimaera.Experiments.Labs.PrintAura
{
    public class PrintAuraProductsClientFacts
    {
        [Fact]
        public async Task ListColors_Should_ContainColors()
        {
            var client = new PrintAuraProductsClient();

            var response = await client.ListColorsAsync();

            Assert.True(response.Result == 1);
            Assert.True(response.Message.IsEmpty());
            Assert.True(response.Colors != null && response.Colors.Count > 0);
        }

        [Fact]
        public async Task ListDesigns_Should_ContainDesigns()
        {
            var client = new PrintAuraImagesClient();

            var response = await client.ListMyImagesAsync();

            Assert.True(response.Result == 1);
        }

        [Fact]
        public async Task ListProducts_Should_ListProducts()
        {
            var client = new PrintAuraProductsClient();

            var response = await client.ListProductsAsync();

            Assert.True(response.Result == 1);
        }
    }
}