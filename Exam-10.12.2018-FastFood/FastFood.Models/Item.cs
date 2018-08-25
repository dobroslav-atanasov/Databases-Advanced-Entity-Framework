namespace FastFood.Models
{
    using System.Collections.Generic;

    public class Item
    {
        public Item()
        {
            this.OrderItems = new List<OrderItem>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public decimal Price { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}