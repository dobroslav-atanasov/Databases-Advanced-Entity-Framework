namespace ProductShop.App
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Data;
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext dbContext = new ProductShopContext();

            using (dbContext)
            {
                ImportUsers(dbContext);
                ImportProducts(dbContext);
                ImportCategories(dbContext);
                SetCategoryProducts(dbContext);

                GetProductsInRange(dbContext);
                GetSoldProducts(dbContext);
                GetCategoriesByProducts(dbContext);
                GetUsersAndProducts(dbContext);
            }
        }

        private static void GetUsersAndProducts(ProductShopContext dbContext)
        {
            var users = dbContext
                .Users
                .Where(u => u.ProductSold.Count >= 1)
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    Products = u.ProductSold
                        .Select(ps => new
                        {
                            Name = ps.Name,
                            Price = ps.Price
                        })
                })
                .OrderByDescending(u => u.Products.Count())
                .ThenBy(u => u.LastName)
                .ToArray();

            XDocument document = new XDocument();
            document.Add(new XElement("users"));
            document.Root.SetAttributeValue("count", users.Length);

            foreach (var user in users)
            {
                var userXElement = new XElement("user");
                userXElement.SetAttributeValue("first-name", user.FirstName);
                userXElement.SetAttributeValue("last-name", user.LastName);
                userXElement.SetAttributeValue("age", user.Age);

                var soldProducts = new XElement("sold-products");
                soldProducts.SetAttributeValue("count", user.Products.Count());
                foreach (var product in user.Products)
                {
                    var productXElement = new XElement("product");
                    productXElement.SetAttributeValue("name", product.Name);
                    productXElement.SetAttributeValue("price", product.Price);

                    soldProducts.Add(productXElement);
                }

                userXElement.Add(soldProducts);
                document.Element("users").Add(userXElement);
            }

            string xmlString = document.ToString(SaveOptions.None);
            Console.WriteLine(xmlString);
            document.Save("../../../Files/users-and-products.xml");
        }

        private static void GetCategoriesByProducts(ProductShopContext dbContext)
        {
            var categories = dbContext
                .Categories
                .Select(c => new
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Select(cp => cp.Product).Count(),
                    Average = c.CategoryProducts.Select(cp => cp.Product.Price).Average(),
                    Revenue = c.CategoryProducts.Select(cp => cp.Product.Price).Sum()
                })
                .OrderByDescending(c => c.Count)
                .ToArray();

            XDocument document = new XDocument();
            document.Add(new XElement("categories"));

            foreach (var category in categories)
            {
                var categoryXElement = new XElement("category");
                categoryXElement.SetAttributeValue("name", category.Name);

                categoryXElement.Add(new XElement("products-count", category.Count), 
                    new XElement("average-price", category.Average),
                    new XElement("total-revenue", category.Revenue));

                document.Element("categories").Add(categoryXElement);
            }

            string xmlString = document.ToString(SaveOptions.None);
            Console.WriteLine(xmlString);
            document.Save("../../../Files/categories-by-products.xml");
        }

        private static void GetSoldProducts(ProductShopContext dbContext)
        {
            var users = dbContext
                .Users
                .Where(u => u.ProductSold.Count >= 1)
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Products = u.ProductSold
                        .Select(p => new
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                })
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToArray();

            XDocument document = new XDocument();
            document.Add(new XElement("users"));

            foreach (var user in users)
            {
                var userXElement = new XElement("user");
                userXElement.SetAttributeValue("first-name", user.FirstName);
                userXElement.SetAttributeValue("last-name", user.LastName);

                var products = new XElement("sold-products");
                foreach (var product in user.Products)
                {
                    products.Add(new XElement("product", new XElement("name", product.Name), new XElement("price", product.Price)));
                }

                userXElement.Add(products);
                document.Element("users").Add(userXElement);
            }

            string xmlString = document.ToString(SaveOptions.None);
            Console.WriteLine(xmlString);
            document.Save("../../../Files/users-sold-products.xml");
        }

        private static void GetProductsInRange(ProductShopContext dbContext)
        {
            var products = dbContext
                .Products
                .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.BuyerId != null)
                .Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .OrderBy(p => p.Price)
                .ToArray();

            XDocument document = new XDocument();
            document.Add(new XElement("products"));
            foreach (var product in products)
            {
                document.Root.Add(new XElement("product", new XElement("name", product.Name), new XElement("price", product.Price), new XElement("buyer", product.Buyer)));
            }

            string xmlString = document.ToString(SaveOptions.None);
            Console.WriteLine(xmlString);
            document.Save("../../../Files/products-in-range.xml");
        }

        private static void SetCategoryProducts(ProductShopContext dbContext)
        {
            int[] productIds = dbContext
                .Products
                .Select(p => p.Id)
                .ToArray();

            int[] categoryIds = dbContext
                .Categories
                .Select(c => c.Id)
                .ToArray();

            Random random = new Random();
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();
            foreach (int product in productIds)
            {
                for (int i = 0; i < 3; i++)
                {
                    int index = random.Next(0, categoryIds.Length);
                    while (categoryProducts.Any(cp => cp.ProductId == product && cp.CategoryId == categoryIds[index]))
                    {
                        index = random.Next(0, categoryIds.Length);
                    }

                    CategoryProduct categoryProduct = new CategoryProduct(categoryIds[index], product);
                    categoryProducts.Add(categoryProduct);
                }
            }

            dbContext.CategoryProducts.AddRange(categoryProducts);
            dbContext.SaveChanges();
        }

        private static void ImportCategories(ProductShopContext dbContext)
        {
            string xmlPath = "../../../Files/categories.xml";
            string xmlString = File.ReadAllText(xmlPath);
            XDocument document = XDocument.Parse(xmlString);

            var elements = document.Root.Elements();
            List<Category> categories = new List<Category>();

            foreach (XElement element in elements)
            {
                string name = element.Element("name").Value;

                Category category = new Category(name);
                categories.Add(category);
            }

            dbContext.Categories.AddRange(categories);
            dbContext.SaveChanges();
        }

        private static void ImportProducts(ProductShopContext dbContext)
        {
            string xmlPath = "../../../Files/products.xml";
            string xmlString = File.ReadAllText(xmlPath);
            XDocument document = XDocument.Parse(xmlString);

            var elements = document.Root.Elements();
            List<Product> products = new List<Product>();
            int[] userIds = dbContext
                .Users
                .Select(u => u.Id)
                .ToArray();
            Random random = new Random();

            foreach (XElement element in elements)
            {
                string name = element.Element("name").Value;
                decimal price = decimal.Parse(element.Element("price").Value);

                int index = random.Next(0, userIds.Length);
                int sellerId = userIds[index];

                int buyerId = sellerId;
                while (buyerId == sellerId)
                {
                    int buyerIndex = random.Next(0, userIds.Length);
                    buyerId = userIds[buyerIndex];
                }

                Product product = new Product(name, price, sellerId, buyerId);
                products.Add(product);
            }

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();
        }

        private static void ImportUsers(ProductShopContext dbContext)
        {
            string xmlPath = "../../../Files/users.xml";
            string xmlString = File.ReadAllText(xmlPath);
            XDocument document = XDocument.Parse(xmlString);

            var elements = document.Root.Elements();
            List<User> users = new List<User>();

            foreach (XElement element in elements)
            {
                string firstName = element.Attribute("firstName")?.Value;
                string lastName = element.Attribute("lastName").Value;
                int? age = null;

                if (element.Attribute("age") != null)
                {
                    age = int.Parse(element.Attribute("age").Value);
                }

                User user = new User(firstName, lastName, age);
                users.Add(user);
            }

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }
    }
}