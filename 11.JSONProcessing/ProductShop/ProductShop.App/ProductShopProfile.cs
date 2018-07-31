namespace ProductShop.App
{
    using AutoMapper;
    using Dto.Import;
    using Models;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UserDto, User>();

            this.CreateMap<ProductDto, Product>();

            this.CreateMap<CategoryDto, Category>();
        }
    }
}