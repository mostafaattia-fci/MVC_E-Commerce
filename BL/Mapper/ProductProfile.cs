
using AutoMapper;
using BLL.DTOs.Admin;
using DA.Models;
namespace BLL.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductAdminDto>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ImageUrl,
                           opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<ProductAdminDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageURL));

            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<DA.Models.Product, DTOs.Product.ProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.Rating,
                    opt => opt.MapFrom(src => src.Reviews.Any() ? src.Reviews.Average(r => r.Rating): 0))
                .ForMember(dest => dest.ReviewsCount,
                    opt => opt.MapFrom(src => src.Reviews.Count));
        }
    }
}
