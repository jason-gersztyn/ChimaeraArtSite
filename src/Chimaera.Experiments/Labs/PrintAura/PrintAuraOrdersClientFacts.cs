using System.Threading.Tasks;
using Chimaera.Beasts.Extensions;
using Chimaera.Labs.PrintAura;
using Xunit;

namespace Chimaera.Experiments.Labs.PrintAura
{
    public class PrintAuraOrdersClientFacts
    {
        [Fact]
        public async Task ListOrders_Should_ReturnSuccess()
        {
            var client = new PrintAuraOrdersClient();

            var response = await client.ListOrdersAsync();

            Assert.True(response.Result == 1);
            Assert.True(response.Message.IsEmpty());
            Assert.True(response.Orders != null && response.Orders.Count > 0);
        }
    }
}