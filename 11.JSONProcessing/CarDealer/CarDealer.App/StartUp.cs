namespace CarDealer.App
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
            CarDealerContext dbContext = new CarDealerContext();
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            IMapper mapper = config.CreateMapper();

            using (dbContext)
            {
                ImportSuppliers(dbContext, mapper);
                ImportParts(dbContext, mapper);
                ImportCars(dbContext, mapper);
                ImportPartCars(dbContext);
                ImportCustomers(dbContext, mapper);
                ImportSales(dbContext);

                GetAllCustomers(dbContext);
                GetAllCarsToyota(dbContext);
                GetLocalSuppliers(dbContext);
                GetCarsWithParts(dbContext);
                GetTotalSalesByCustomer(dbContext);
                GetSalesWithAppliedDiscount(dbContext);
            }
        }

        private static void GetSalesWithAppliedDiscount(CarDealerContext dbContext)
        {
            SaleDto[] sales = dbContext
                .Sales
                .Select(s => new SaleDto()
                {
                    Car = new CarSaleDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount / 100m,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price) - (s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100m)
                })
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(sales, Formatting.Indented);
            File.WriteAllText("../../../Files/Export/sales-discounts.json", jsonString);
        }

        private static void GetTotalSalesByCustomer(CarDealerContext dbContext)
        {
            CustomerSaleDto[] sales = dbContext
                .Customers
                .Where(c => c.Sales.Count >= 1)
                .Select(c => new CustomerSaleDto()
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Select(s => s.Car.PartCars.Select(pc => pc.Part).Sum(pc => pc.Price)).Sum()
                })
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(sales, Formatting.Indented);
            File.WriteAllText("../../../Files/Export/customers-total-sales.json", jsonString);
        }

        private static void GetCarsWithParts(CarDealerContext dbContext)
        {
            CarExportDto[] cars = dbContext
                .Cars
                .Select(c => new CarExportDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars
                        .Select(pc => new PartExportDto()
                        {
                            Name = pc.Part.Name,
                            Price = pc.Part.Price
                        })
                        .ToArray()
                })
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(cars, Formatting.Indented);
            File.WriteAllText("../../../Files/Export/cars-and-parts.json", jsonString);
        }

        private static void GetLocalSuppliers(CarDealerContext dbContext)
        {
            LocalSupplierDto[] suppliers = dbContext
                .Suppliers
                .Select(s => new LocalSupplierDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .OrderBy(s => s.Name)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(suppliers, Formatting.Indented);
            File.WriteAllText("../../../Files/Export/local-suppliers.json", jsonString);
        }

        private static void GetAllCarsToyota(CarDealerContext dbContext)
        {
            ToyotaCarDto[] cars = dbContext
                .Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new ToyotaCarDto()
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(cars, Formatting.Indented);
            File.WriteAllText("../../../Files/Export/toyota-cars.json", jsonString);
        }

        private static void GetAllCustomers(CarDealerContext dbContext)
        {
            CustomerExportDto[] customers = dbContext
                .Customers
                .Select(c => new CustomerExportDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungerDriver = c.IsYoungDriver
                })
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungerDriver)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText("../../../Files/Export/ordered-customers.json", jsonString);
        }

        private static void ImportSales(CarDealerContext dbContext)
        {
            int[] discounts = new[] { 0, 5, 10, 15, 20, 30, 40, 50 };
            int[] carIds = dbContext
                .Cars
                .Select(c => c.Id)
                .ToArray();

            int[] customerIds = dbContext
                .Customers
                .Select(c => c.Id)
                .ToArray();

            List<Sale> sales = new List<Sale>();
            Random random = new Random();
            for (int i = 0; i < carIds.Length; i++)
            {
                int carId = random.Next(1, carIds.Length);
                int customerId = random.Next(1, customerIds.Length);
                int discount = discounts[random.Next(0, discounts.Length)];

                Sale sale = new Sale()
                {
                    Discount = discount,
                    CarId = carId,
                    CustomerId = customerId
                };

                sales.Add(sale);
            }

            dbContext.Sales.AddRange(sales);
            dbContext.SaveChanges();
        }

        private static void ImportCustomers(CarDealerContext dbContext, IMapper mapper)
        {
            string jsonString = File.ReadAllText("../../../Files/Import/customers.json");
            CustomerDto[] customerDtos = JsonConvert.DeserializeObject<CustomerDto[]>(jsonString);
            Customer[] customers = mapper.Map<Customer[]>(customerDtos);

            dbContext.Customers.AddRange(customers);
            dbContext.SaveChanges();
        }

        private static void ImportPartCars(CarDealerContext dbContext)
        {
            int numberOfCars = dbContext.Cars.Count();

            List<PartCar> partCars = new List<PartCar>();
            for (int i = 1; i <= numberOfCars; i++)
            {
                PartCar partCar = new PartCar();
                partCar.CarId = i;
                partCar.PartId = new Random().Next(1, 132);

                partCars.Add(partCar);
            }

            dbContext.PartCars.AddRange(partCars);
            dbContext.SaveChanges();
        }

        private static void ImportCars(CarDealerContext dbContext, IMapper mapper)
        {
            string jsonString = File.ReadAllText("../../../Files/Import/cars.json");
            CarDto[] carDtos = JsonConvert.DeserializeObject<CarDto[]>(jsonString);
            Car[] cars = mapper.Map<Car[]>(carDtos);

            dbContext.Cars.AddRange(cars);
            dbContext.SaveChanges();
        }

        private static void ImportParts(CarDealerContext dbContext, IMapper mapper)
        {
            string jsonString = File.ReadAllText("../../../Files/Import/parts.json");
            PartDto[] partDtos = JsonConvert.DeserializeObject<PartDto[]>(jsonString);

            List<Part> parts = new List<Part>();
            foreach (PartDto partDto in partDtos)
            {
                Part part = mapper.Map<Part>(partDto);
                int supplierId = new Random().Next(1, 32);
                part.SupplierId = supplierId;
                parts.Add(part);
            }

            dbContext.Parts.AddRange(parts);
            dbContext.SaveChanges();
        }

        private static void ImportSuppliers(CarDealerContext dbContext, IMapper mapper)
        {
            string jsonString = File.ReadAllText("../../../Files/Import/suppliers.json");
            SupplierDto[] supplierDtos = JsonConvert.DeserializeObject<SupplierDto[]>(jsonString);
            Supplier[] suppliers = mapper.Map<Supplier[]>(supplierDtos);

            dbContext.Suppliers.AddRange(suppliers);
            dbContext.SaveChanges();
        }
    }
}