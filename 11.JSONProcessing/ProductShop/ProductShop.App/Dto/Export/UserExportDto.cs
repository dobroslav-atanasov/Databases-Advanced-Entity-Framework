namespace ProductShop.App.Dto.Export
{
    public class UserExportDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public SoldProductDto[] SoldProducts { get; set; }
    }
}