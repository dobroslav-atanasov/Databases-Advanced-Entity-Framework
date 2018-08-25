namespace FastFood.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Dto.Export;
    using Microsoft.EntityFrameworkCore;
    using Models.Enums;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            OrderType type = Enum.Parse<OrderType>(orderType);

            //var employee = context
            //    .Employees
            //    .Where(e => e.Name == employeeName)
            //    .Select(e => new
            //    {
            //        Name = e.Name,
            //        Orders = e.Orders.Where(o => o.Type == type)
            //            .Select(o => new
            //            {
            //                Cusotmer = o.Customer,
            //                Items = o.OrderItems
            //                    .Select(oi => new
            //                    {
            //                        Name = oi.Item.Name,
            //                        Price = oi.Item.Price,
            //                        Quantity = oi.Quantity
            //                    })
            //                    .ToArray(),
            //                TotalPrice = o.OrderItems.Sum(oi => oi.Quantity * oi.Item.Price)
            //            })
            //            .OrderByDescending(o => o.TotalPrice)
            //            .ThenByDescending(o => o.Items.Length)
            //            .ToArray(),
            //        TotalMade = e.Orders.Where(tm => tm.Type == type).Sum(tm => tm.OrderItems.Sum(oi => oi.Quantity * oi.Item.Price))
            //    })
            //    .FirstOrDefault();

            //string jsonString = JsonConvert.SerializeObject(employee, Formatting.Indented);

            //return jsonString;

            var employee = context
                .Employees
                .Where(e => e.Name == employeeName)
                .Select(e => new
                {
                    e.Name,
                    Orders = e.Orders.Where(o => o.Type == type).Select(o => new
                        {
                            o.Customer,
                            Items = o.OrderItems.Select(oi => new
                            {
                                oi.Item.Name,
                                oi.Item.Price,
                                oi.Quantity
                            }).ToArray(),
                            TotalPrice = o.OrderItems.Sum(oi => oi.Item.Price * oi.Quantity)
                        })
                        .OrderByDescending(o => o.TotalPrice)
                        .ThenByDescending(o => o.Items.Length)
                        .ToArray(),
                    TotalMade = e.Orders
                        .Where(o => o.Type == type)
                        .Sum(o => o.OrderItems.Sum(oi => oi.Item.Price * oi.Quantity))
                })
                .SingleOrDefault();

            var json = JsonConvert.SerializeObject(employee, Formatting.Indented);

            return json;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            string[] searchedCategories = categoriesString.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            CategoryExportDto[] categories = context
                .Categories
                .Include(c => c.Items)
                .Where(c => searchedCategories.Contains(c.Name))
                .Select(c => new CategoryExportDto()
                {
                    Name = c.Name,
                    MostPopularItem = c.Items.Select(i => new MostPopularItemDto()
                    {
                        Name = i.Name,
                        TotalMade = i.OrderItems.Sum(oi => oi.Quantity * oi.Item.Price),
                        TimesSold = i.OrderItems.Sum(oi => oi.Quantity)
                    })
                    .OrderByDescending(i => i.TotalMade)
                    .ThenByDescending(i => i.TimesSold)
                    .First()
                })
                .OrderByDescending(c => c.MostPopularItem.TotalMade)
                .ThenByDescending(c => c.MostPopularItem.TimesSold)
                .ToArray();


            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryExportDto[]), new XmlRootAttribute("Categories"));
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), categories, xmlNamespaces);

            return sb.ToString().Trim();
        }
    }
}