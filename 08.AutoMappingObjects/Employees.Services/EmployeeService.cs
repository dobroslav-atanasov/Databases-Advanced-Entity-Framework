namespace Employees.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Contracts;
    using Data;
    using DtoModels;
    using Models;

    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeContext dbContext;

        public EmployeeService(EmployeeContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddEmployee(EmployeeDto employeeDto)
        {
            Employee employee = Mapper.Map<Employee>(employeeDto);
            this.dbContext.Employees.Add(employee);
            this.dbContext.SaveChanges();
        }

        public void SetBirthday(int id, DateTime date)
        {
            Employee employee = this.dbContext.Employees.Find(id);

            if (employee == null)
            {
                throw new ArgumentException($"Employee with id {id} does not exist!");
            }

            employee.Birthday = date;
            this.dbContext.SaveChanges();
        }

        public void SetAddress(int id, string address)
        {
            Employee employee = this.dbContext.Employees.Find(id);

            if (employee == null)
            {
                throw new ArgumentException($"Employee with id {id} does not exist!");
            }

            employee.Address = address;
            this.dbContext.SaveChanges();
        }

        public EmployeeDto EmployeeInfo(int id)
        {
            Employee employee = this.dbContext.Employees.Find(id);

            if (employee == null)
            {
                throw new ArgumentException($"Employee with id {id} does not exist!");
            }

            EmployeeDto employeeDto = Mapper.Map<EmployeeDto>(employee);
            return employeeDto;
        }

        public EmployeePersonalDto EmployeePersonalInfo(int id)
        {
            Employee employee = this.dbContext.Employees.Find(id);

            if (employee == null)
            {
                throw new ArgumentException($"Employee with id {id} does not exist!");
            }

            EmployeePersonalDto employeePersonalDto = Mapper.Map<EmployeePersonalDto>(employee);
            return employeePersonalDto;
        }

        public IEnumerable<EmployeeManagerDto> ListAllEmployees(int age)
        {
            Employee[] employees = this.dbContext
                .Employees
                .Where(e => e.Birthday != null && this.CalculateAge(e.Birthday.Value) > age)
                .OrderByDescending(e => e.Salary)
                .ToArray();

            IEnumerable<EmployeeManagerDto> employeeManagerDtos = Mapper.Map<Employee[], IEnumerable<EmployeeManagerDto>>(employees);
            return employeeManagerDtos;
        }

        private int CalculateAge(DateTime birthday)
        {
            DateTime currentTime = DateTime.Now;

            int age = new DateTime(currentTime.Subtract(birthday).Ticks).Year;
            return age;
        }
    }
}