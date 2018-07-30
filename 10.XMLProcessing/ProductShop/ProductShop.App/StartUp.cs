namespace ProductShop.App
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using Dto.Export;
    using Dto.Import;
    using Models;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

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
                GetSoldProducts(dbContext);
                GetCategoriesByProductsCount(dbContext);
                GetUsersAndProducts(dbContext);
            }
        }

        private static void GetUsersAndProducts(ProductShopContext dbContext)
        {
            var users = new UserRootDto()
            {
                Count = dbContext.Users.Count(),
                Users = dbContext
                    .Users
                    .Where(u => u.ProductSold.Count >= 1)
                    .OrderByDescending(u => u.ProductSold.Count)
                    .Select(u => new UserExportDto()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age.ToString(),
                        Products = new ProductSoldRootDto()
                        {
                            Count = u.ProductSold.Count,
                            ProductSoldDtos = u.ProductSold.Select(s => new ProductSoldDto()
                            {
                                Name = s.Name,
                                Price = s.Price
                            })
                            .ToArray()
                        }
                    })
                    .ToArray()
            };

            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(UserRootDto), new XmlRootAttribute("users"));
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), users, xmlNamespaces);

            File.WriteAllText("../../../Files/Export/users-and-products.xml", sb.ToString());
        }

        private static void GetCategoriesByProductsCount(ProductShopContext dbContext)
        {
            CategoryExportDto[] categories = dbContext
                .Categories
                .Select(c => new CategoryExportDto()
                {
                    Name = c.Name,
                    ProductCount = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(c => c.ProductCount)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryExportDto[]), new XmlRootAttribute("categories"));
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), categories, xmlNamespaces);

            File.WriteAllText("../../../Files/Export/categories-by-products.xml", sb.ToString());
        }

        private static void GetSoldProducts(ProductShopContext dbContext)
        {
            SellerUserDto[] users = dbContext
                .Users
                .Where(u => u.ProductSold.Count >= 1)
                .Select(u => new SellerUserDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Products = u.ProductSold
                        .Select(ps => new SoldProductDto()
                        {
                            Name = ps.Name,
                            Price = ps.Price
                        })
                        .ToArray()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(SellerUserDto[]), new XmlRootAttribute("users"));
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), users, xmlNamespaces);

            File.WriteAllText("../../../Files/Export/users-sold-products.xml", sb.ToString());
        }

        private static void GetProductsInRange(ProductShopContext dbContext)
        {
            ProductExportDto[] products = dbContext
                .Products
                .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.Buyer != null)
                .Select(p => new ProductExportDto()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}" ?? p.Buyer.LastName
                })
                .OrderBy(p => p.Price)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(ProductExportDto[]), new XmlRootAttribute("products"));
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), products, xmlNamespaces);

            File.WriteAllText("../../../Files/Export/products-in-range.xml", sb.ToString());
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

                    CategoryProduct categoryProduct = new CategoryProduct(categoryIds[index], product);
                    categoryProducts.Add(categoryProduct);
                }
            }

            dbContext.CategoryProducts.AddRange(categoryProducts);
            dbContext.SaveChanges();
        }

        private static void ImportCategories(ProductShopContext dbContext, IMapper mapper)
        {
            string xmlString = File.ReadAllText("../../../Files/Import/categories.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("categories"));
            CategoryDto[] deserializedCategories = (CategoryDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Category> categories = new List<Category>();
            foreach (CategoryDto categoryDto in deserializedCategories)
            {
                if (!IsValid(categoryDto))
                {
                    continue;
                }

                Category category = mapper.Map<Category>(categoryDto);
                categories.Add(category);
            }

            dbContext.Categories.AddRange(categories);
            dbContext.SaveChanges();
        }

        private static void ImportProducts(ProductShopContext dbContext, IMapper mapper)
        {
            string xmlString = File.ReadAllText("../../../Files/Import/products.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(ProductDto[]), new XmlRootAttribute("products"));
            ProductDto[] deserializedProducts = (ProductDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Product> products = new List<Product>();
            int[] userIds = dbContext
                .Users
                .Select(u => u.Id)
                .ToArray();
            Random random = new Random();

            foreach (ProductDto productDto in deserializedProducts)
            {
                if (!IsValid(productDto))
                {
                    continue;
                }

                int index = random.Next(0, userIds.Length);
                int sellerId = userIds[index];

                int buyerId = sellerId;
                while (buyerId == sellerId)
                {
                    int buyerIndex = random.Next(0, userIds.Length);
                    buyerId = userIds[buyerIndex];
                }

                Product product = mapper.Map<Product>(productDto);
                product.BuyerId = buyerId;
                product.SellerId = sellerId;
                products.Add(product);
            }

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();
        }

        private static void ImportUsers(ProductShopContext dbContext, IMapper mapper)
        {
            string xmlString = File.ReadAllText("../../../Files/Import/users.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("users"));
            UserDto[] deserializedUsers = (UserDto[])serializer.Deserialize(new StringReader(xmlString));

            List<User> users = new List<User>();
            foreach (UserDto userDto in deserializedUsers)
            {
                if (!IsValid(userDto))
                {
                    continue;
                }

                User user = mapper.Map<User>(userDto);
                users.Add(user);
            }

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }

        public static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}