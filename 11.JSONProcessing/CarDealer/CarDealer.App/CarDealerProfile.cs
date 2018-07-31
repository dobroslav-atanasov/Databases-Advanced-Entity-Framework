namespace CarDealer.App
{
    using AutoMapper;
    using Dto.Import;
    using Models;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierDto, Supplier>();

            this.CreateMap<PartDto, Part>();

            this.CreateMap<CarDto, Car>();

            this.CreateMap<CustomerDto, Customer>();
        }
    }
}