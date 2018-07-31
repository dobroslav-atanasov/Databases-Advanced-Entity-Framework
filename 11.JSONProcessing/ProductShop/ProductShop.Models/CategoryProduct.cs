namespace ProductShop.Models
{
    public class CategoryProduct
    {
        public CategoryProduct()
        {
        }

        public CategoryProduct(int categoryId, int productId)
        {
            this.CategoryId = categoryId;
            this.ProductId = productId;
        }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}