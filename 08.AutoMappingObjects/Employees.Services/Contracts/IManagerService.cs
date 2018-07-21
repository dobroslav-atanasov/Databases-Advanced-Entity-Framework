namespace Employees.Services.Contracts
{
    using DtoModels;

    public interface IManagerService
    {
        void SetManager(int employeeId, int managerId);

        ManagerDto ManagerInfo(int id);
    }
}