using AutoMapper;
using Chimaera.Beasts.Extensions;
using Chimaera.Beasts.Model;
using Chimaera.Labs.PrintAura.Models;
using System.Web.Script.Serialization;
using Chimaera.Labs.PrintAura;

namespace Chimaera.Tail.Mappings
{
    public static class OrderMappings
    {
        public static void Configure()
        {
            Mapper.CreateMap<QuoteItem, AddOrderItem>()
                .ForMember(o => o.ProductId, opt => opt.MapFrom(c => c.Product.Type.PrintAuraID))
                .ForMember(o => o.BrandId, opt => opt.MapFrom(c => c.Product.Type.BrandID))
                .ForMember(o => o.ColorId, opt => opt.MapFrom(c => c.Product.Color.PrintAuraID))
                .ForMember(o => o.SizeId, opt => opt.MapFrom(c => c.Size.PrintAuraID))
                .ForMember(o => o.Quantity, opt => opt.MapFrom(c => c.Quantity))
                .ForMember(o => o.FrontPrintId, opt => opt.MapFrom(c => c.Product.Design.FrontPrintID))
                .ForMember(o => o.FrontMockupId, opt => opt.MapFrom(c => c.Product.Design.FrontMockupID))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<Order, AddOrderRequest>()
                .ForMember(a => a.Method, opt => opt.UseValue("addorder"))
                .ForMember(a => a.BusinessName, opt => opt.UseValue("Chimaera Conspiracy"))
                .ForMember(a => a.BusinessContact, opt => opt.UseValue("Aaron Spore"))
                .ForMember(a => a.Email, opt => opt.UseValue("admin@chimaeraconspiracy.com"))
                .ForMember(a => a.AltOrderId, opt => opt.MapFrom(o => o.OrderID))
                .ForMember(a => a.ReturnLabel, opt => opt.UseValue("Chimaera Conspiracy\n2810A River Rd. S\nSalem OR, 97302\nUS"))
                .ForMember(a => a.ClientName, opt => opt.MapFrom(o => o.Quote.Address.Name))
                .ForMember(a => a.Address1, opt => opt.MapFrom(o => o.Quote.Address.Street1))
                .ForMember(a => a.Address2, opt => opt.MapFrom(o => o.Quote.Address.Street2))
                .ForMember(a => a.City, opt => opt.MapFrom(o => o.Quote.Address.City))
                .ForMember(a => a.State, opt => opt.MapFrom(o => o.Quote.Address.State))
                .ForMember(a => a.Zip, opt => opt.MapFrom(o => o.Quote.Address.Zip))
                .ForMember(a => a.Country, opt => opt.MapFrom(o => o.Quote.Address.Country))
                .ForMember(a => a.CustomerPhone, opt => opt.MapFrom(o => string.IsNullOrEmpty(o.Quote.Address.Phone) ? "5033284983" : o.Quote.Address.Phone))
                .ForMember(a => a.ShippingId, opt => opt.MapFrom(o => o.Quote.GetShippingId().ToString()))
                .ForMember(a => a.Items, opt => opt.ResolveUsing(r =>
                {
                    var source = r.Context.SourceValue as Order;
                    AddOrderItem[] requestItems = Mapper.Map<AddOrderItem[]>(source.Quote.Items);
                    var serializer = new PrintAuraJsonSerializer();
                    string jsonEncodedString = "{\"items\":" + serializer.Serialize(requestItems) + "}";
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonEncodedString);
                    return System.Convert.ToBase64String(plainTextBytes);
                }))
                .IgnoreAllNonExisting();
        }
    }
}