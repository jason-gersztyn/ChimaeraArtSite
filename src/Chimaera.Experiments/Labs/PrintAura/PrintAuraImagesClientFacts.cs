using System.Threading.Tasks;
using Chimaera.Beasts.Extensions;
using Chimaera.Labs.PrintAura;
using Xunit;

namespace Chimaera.Experiments.Labs.PrintAura
{
    public class PrintAuraImagesClientFacts
    {
        [Fact]
        public async Task ListMyImages_Should_ContainImages()
        {
            var client = new PrintAuraImagesClient();

            var response = await client.ListMyImagesAsync();

            Assert.True(response.Result == 1);
            Assert.True(response.Message.IsEmpty());
            Assert.True(response.Images != null && response.Images.Count > 0);
        }
    }
}