namespace CarDealer.App.Dto.Export
{
    using System;

    public class CustomerExportDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsYoungerDriver { get; set; }
    }
}