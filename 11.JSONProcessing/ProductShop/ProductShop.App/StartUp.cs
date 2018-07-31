namespace ProductShop.App
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AutoMapper;
    using Data;
    using Dto.Export;
    using Dto.Import;
    using Models;
    using Newtonsoft.Json;

    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext dbContext = new ProductShopContext();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            using (dbContext)
            {
                ImportUsers(dbContext, mapper);
                ImportProducts(dbContext, mapper);
                ImportCategories(dbContext, mapper);
                ImportCategoryProducts(dbContext);

                GetProductsInRange(dbContext);
                GetAllSoldProducts(dbContext);
                GetCategoriesByProductsCount(dbContext);
                GetUsersAndProducts(dbContext);
            }
        }

        private static void GetUsersAndProducts(ProductShopContext dbContext)
        {
            UserAllDto user = new UserAllDto()
            {
                UserCount = dbContext.Users.Count(us => us.ProductSold.Count >= 1),
                Users = dbContext
                    .Users
                    .Where(u => u.ProductSold.Count >= 1)
                    .OrderByDescending(u => u.ProductSold.Count)
                    .Select(u => new UserProductDto()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts = new ProductSoldDto()
                        {
                            Count = u.ProductSold.Count,
                            Products = u.ProductSold
                                .Select(p => new ProductAllDto()
                                {
                                    Name = p.Name,
                                    Price = p.Price
                                })
                                .ToArray()
                        }
                    })
                    .ToArray()
            };

            string jsonString = JsonConvert.SerializeObject(user, Formatting.Indented);
            File.WriteAllText("../../../Files/Export/users-and-products.json", jsonString);
        }

        private static void GetCategoriesByProductsCount(ProductShopContext dbContext)
        {
            CategoryExportDto[] categories = dbContext
                .Categories
                .Select(c => new CategoryExportDto()
                {
                    Category = c.Name,
                    ProductsCount = c.CategoryProducts.Select(cp => cp.Product).Count(),
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderBy(c => c.Category)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(categories, Formatting.Indented);
            File.WriteAllText("../../../Files/Export/categories-by-products.json", jsonString);
        }

        private static void GetAllSoldProducts(ProductShopContext dbContext)
        {
            UserExportDto[] users = dbContext
                .Users
                .Where(u => u.ProductSold.Count >= 1)
                .Select(u => new UserExportDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductSold
                        .Select(ps => new SoldProductDto()
                        {
                            Name = ps.Name,
                            Price = ps.Price,
                            BuyerFirstName = ps.Buyer.FirstName,
                            BuyerLastName = ps.Buyer.LastName
                        })
                        .ToArray()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("../../../Files/Export/users-sold-products.json", jsonString);
        }

        private static void GetProductsInRange(ProductShopContext dbContext)
        {
            ProductExportDto[] products = dbContext
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductExportDto()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}".Trim()
                })
                .OrderBy(p => p.Price)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText("../../../Files/Export/products-in-range.json", jsonString);
        }

        private static void ImportCategoryProducts(ProductShopContext dbContext)
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

                    CategoryProduct categoryProduct = new CategoryProduct()
                    {
                        ProductId = product,
                        CategoryId = categoryIds[index]
                    };

                    categoryProducts.Add(categoryProduct);
                }
            }

            dbContext.CategoryProducts.AddRange(categoryProducts);
            dbContext.SaveChanges();
        }

        private static void ImportCategories(ProductShopContext dbContext, IMapper mapper)
        {
            string jsonString = File.ReadAllText("../../../Files/Import/categories.json");
            CategoryDto[] categoryDtos = JsonConvert.DeserializeObject<CategoryDto[]>(jsonString);
            Category[] categories = mapper.Map<Category[]>(categoryDtos);

            dbContext.Categories.AddRange(categories);
            dbContext.SaveChanges();
        }

        private static void ImportProducts(ProductShopContext dbContext, IMapper mapper)
        {
            string jsonString = File.ReadAllText("../../../Files/Import/products.json");
            ProductDto[] productDtos = JsonConvert.DeserializeObject<ProductDto[]>(jsonString);
            Product[] products = mapper.Map<Product[]>(productDtos);

            Random random = new Random();
            int[] userIds = dbContext
                .Users
                .Select(u => u.Id)
                .ToArray();

            foreach (Product product in products)
            {
                int index = random.Next(0, userIds.Length);
                int sellerId = userIds[index];

                int buyerId = sellerId;
                while (buyerId == sellerId)
                {
                    int buyerIndex = random.Next(0, userIds.Length);
                    buyerId = userIds[buyerIndex];
                }

                product.SellerId = sellerId;
                product.BuyerId = buyerId;
            }

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();
        }

        private static void ImportUsers(ProductShopContext dbContext, IMapper mapper)
        {
            string jsonString = File.ReadAllText("../../../Files/Import/users.json");
            UserDto[] userDtos = JsonConvert.DeserializeObject<UserDto[]>(jsonString);

            User[] users = mapper.Map<User[]>(userDtos);

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }
    }
}