using AutoMapper;
using BLL.DTOs.Admin;
using DA.Models;

namespace BLL.Mapper
{
    public class AdminOrderProfile : Profile
    {
        public AdminOrderProfile()
        {
            CreateMap<OrderItem, OrderItemAdminDto>()
                .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.Name))
                .ForMember(d => d.ProductImageUrl, opt => opt.MapFrom(s => s.Product.ImageUrl));

            CreateMap<Order, OrderAdminListDto>()
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.User.FullName))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

            CreateMap<Order, OrderAdminDetailsDto>()
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.User.FullName))
                .ForMember(d => d.CustomerEmail, opt => opt.MapFrom(s => s.User.Email))
                .ForMember(d => d.Items, opt => opt.MapFrom(s => s.OrderItems))
                .ForMember(d => d.ShippingAddress, opt => opt.MapFrom(s => "TODO: Map Address")); // ◀️ يحتاج لوجيك خاص
        }
    }
}
