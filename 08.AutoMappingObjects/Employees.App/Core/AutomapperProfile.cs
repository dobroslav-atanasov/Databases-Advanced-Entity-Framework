namespace Employees.App.Core
{
    using AutoMapper;
    using DtoModels;
    using Models;

    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            this.CreateMap<Employee, EmployeeDto>();

            this.CreateMap<EmployeeDto, Employee>();

            this.CreateMap<Employee, EmployeePersonalDto>();

            this.CreateMap<EmployeePersonalDto, Employee>();

            this.CreateMap<Employee, ManagerDto>();

            this.CreateMap<Employee, EmployeeManagerDto>();
        }
    }
}