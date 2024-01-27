using System;
using System.Collections.Generic;
using System.Linq;
using Chimaera.Beasts.Model;
using Chimaera.Beasts.Service;
using Chimaera.Labs.PrintAura;
using Chimaera.Labs.PrintAura.Models;
using AutoMapper;
using Chimaera.Beasts.Extensions;
using Chimaera.Beasts.Utils;
using Chimaera.Beasts.Integration;
using Chimaera.Tail.Mappings;

namespace Chimaera.Tail
{
    public class OrderManager
    {
        private PrintAuraOrdersClient client;

        public OrderManager()
        {
            client = new PrintAuraOrdersClient();
            OrderMappings.Configure();
        }

        public void AddOrders()
        {
            List<Order> pendingOrders = OrderService.GetOrders(StatusID: (int)Beasts.Model.Status.Pending).ToList();
            foreach (Order order in pendingOrders)
            {
                try
                {
                    if (PayPal.CheckSale(order.PaypalSaleID))
                    {
                        AddOrderRequest addOrderRequest = Mapper.Map<AddOrderRequest>(order);

                        AddOrderResponse addOrderResponse = client.AddOrder(addOrderRequest);

                        if (addOrderResponse.Result == 1 && addOrderResponse.Order != null)
                        {
                            order.PrintAuraID = addOrderResponse.Order.OrderId;
                            order.Status = Beasts.Model.Status.Processing;
                            OrderService.UpdateOrder(order);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionService.CreateException(ex);
                }
            }
        }

        public void UpdateOrders()
        {
            ListOrdersResponse listOrdersResponse = client.ListOrders();
            if (listOrdersResponse.Result == 1 && listOrdersResponse.Orders.Count > 0)
            {
                List<Order> unshippedOrders = OrderService.GetOrders(StatusID: (int)Beasts.Model.Status.Processing).ToList();
                foreach (Order order in unshippedOrders)
                {
                    try
                    {
                        ListOrder listOrder = listOrdersResponse.Orders.Where(x => x.OrderId == order.PrintAuraID).FirstOrDefault();

                        if (listOrder != null)
                        {
                            if (listOrder.Status == Labs.PrintAura.Models.Status.Shipped)
                            {
                                var item = listOrder.Items.First();

                                Shipment shipment = new Shipment();
                                shipment.Tracking = item.Tracking;
                                shipment.ShipDate = item.ShippedDate.ToDateTime();
                                shipment.Service = "USPS";
                                shipment.OrderID = order.OrderID;

                                ShipmentService.CreateShipment(shipment);

                                order.Status = Beasts.Model.Status.Shipped;

                                OrderService.UpdateOrder(order);

                                Email.SendTracking(OrderService.GetOrders(order.OrderID).FirstOrDefault());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionService.CreateException(ex);
                    }
                }
            }
        }
    }
}