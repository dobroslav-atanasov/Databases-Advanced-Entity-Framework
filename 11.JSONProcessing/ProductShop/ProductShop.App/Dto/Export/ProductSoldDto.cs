namespace ProductShop.App.Dto.Export
{
    public class ProductSoldDto
    {
        public int Count { get; set; }

        public ProductAllDto[] Products { get; set; }
    }
}