namespace Employees.Services
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Contracts;
    using Data;
    using DtoModels;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ManagerService : IManagerService
    {
        private readonly EmployeeContext dbContext;

        public ManagerService(EmployeeContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SetManager(int employeeId, int managerId)
        {
            Employee employee = this.dbContext.Employees.Find(employeeId);
            Employee manager = this.dbContext.Employees.Find(managerId);

            if (employee == null || manager == null)
            {
                throw new ArgumentException("Employee or manager does not exist!");
            }

            if (employee == manager)
            {
                throw new ArgumentException("The employee cannot be manager of himself!");
            }

            employee.ManagerId = manager.Id;
            this.dbContext.SaveChanges();
        }

        public ManagerDto ManagerInfo(int id)
        {
            Employee manager = this.dbContext
                .Employees
                .Include(e => e.Employees)
                .FirstOrDefault(e => e.Id == id);

            if (manager == null)
            {
                throw new ArgumentException($"Employee with id {id} does not exist!");
            }

            ManagerDto managerDto = Mapper.Map<ManagerDto>(manager);
            return managerDto;
        }
    }
}