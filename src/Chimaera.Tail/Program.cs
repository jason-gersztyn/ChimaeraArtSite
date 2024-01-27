namespace Chimaera.Tail
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderManager orderManager = new OrderManager();

            orderManager.AddOrders();
            orderManager.UpdateOrders();

            //ProductManager productManager = new ProductManager();
            
            //productManager.AddColors();
            //productManager.AddSizes();
            //productManager.AddProductSizes();
        }
    }
}