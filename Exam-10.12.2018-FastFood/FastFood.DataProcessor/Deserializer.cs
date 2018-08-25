namespace FastFood.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Dto.Import;
    using Models;
    using Models.Enums;
    using Newtonsoft.Json;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";
        private const string SuccessOrder = "Order for {0} on {1} added";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            EmployeeDto[] employeeDtos = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Employee> employees = new List<Employee>();

            foreach (EmployeeDto dto in employeeDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!context.Positions.Any(p => p.Name == dto.Position))
                {
                    Position position = new Position()
                    {
                        Name = dto.Position
                    };

                    context.Positions.Add(position);
                    context.SaveChanges();
                }

                Employee employee = new Employee()
                {
                    Name = dto.Name,
                    Age = dto.Age,
                    PositionId = context.Positions.Single(p => p.Name == dto.Position).Id
                };

                employees.Add(employee);
                sb.AppendLine(string.Format(SuccessMessage, dto.Name));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            ItemDto[] itemDtos = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Item> items = new List<Item>();

            foreach (ItemDto dto in itemDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!context.Categories.Any(c => c.Name == dto.Category))
                {
                    Category category = new Category()
                    {
                        Name = dto.Category
                    };

                    context.Categories.Add(category);
                    context.SaveChanges();
                }

                if (items.Any(i => i.Name == dto.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Item item = new Item()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    CategoryId = context.Categories.Single(c => c.Name == dto.Category).Id
                };

                items.Add(item);
                sb.AppendLine(string.Format(SuccessMessage, dto.Name));
            }

            context.Items.AddRange(items);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            OrderDto[] deserializedOrders = (OrderDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();

            foreach (OrderDto dto in deserializedOrders)
            {
                if (string.IsNullOrWhiteSpace(dto.Customer))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!context.Employees.Any(e => e.Name == dto.Employee))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                DateTime dateTime;
                bool isValidDate = DateTime.TryParseExact(dto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                if (!isValidDate)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                OrderType orderType;
                bool isValidOrderType = Enum.TryParse<OrderType>(dto.Type, out orderType);
                if (!isValidOrderType)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool isValidItems = true;
                foreach (ItemXmlDto itemDto in dto.Items)
                {
                    if (itemDto.Quantity <= 0 || !context.Items.Any(i => i.Name == itemDto.Name))
                    {
                        isValidItems = false;
                        break;
                    }
                }

                if (!isValidItems)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Order order = new Order()
                {
                    Customer = dto.Customer,
                    DateTime = dateTime,
                    Type = orderType,
                    EmployeeId = context.Employees.Single(e => e.Name == dto.Employee).Id
                };

                context.Orders.Add(order);
                context.SaveChanges();
                sb.AppendLine(string.Format(SuccessOrder, dto.Customer, dateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)));

                List<OrderItem> orderItems = new List<OrderItem>();
                foreach (ItemXmlDto itemDto in dto.Items)
                {
                    OrderItem orderItem = new OrderItem()
                    {
                        Order = order,
                        ItemId = context.Items.Single(i => i.Name == itemDto.Name).Id,
                        Quantity = itemDto.Quantity
                    };

                    orderItems.Add(orderItem);
                }

                context.OrderItems.AddRange(orderItems);
                context.SaveChanges();
            }

            return sb.ToString().Trim();
        }

        public static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}