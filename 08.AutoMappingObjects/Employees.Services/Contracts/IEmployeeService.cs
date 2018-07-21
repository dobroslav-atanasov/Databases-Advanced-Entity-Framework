namespace Employees.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using DtoModels;

    public interface IEmployeeService
    {
        void AddEmployee(EmployeeDto employeeDto);

        void SetBirthday(int id, DateTime date);

        void SetAddress(int id, string address);

        EmployeeDto EmployeeInfo(int id);

        EmployeePersonalDto EmployeePersonalInfo(int id);

        IEnumerable<EmployeeManagerDto> ListAllEmployees(int age);
    }
}