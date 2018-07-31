namespace ProductShop.App.Dto.Export
{
    public class CategoryExportDto
    {
        public string Category { get; set; }

        public int ProductsCount { get; set; }

        public decimal AveragePrice { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}