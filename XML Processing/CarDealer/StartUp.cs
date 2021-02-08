using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Linq;
using CarDealer.XmlHelper;
using System.Collections.Generic;
using CarDealer.Dtos.Export;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());
            CarDealerContext context = new CarDealerContext();

            using (var db=new CarDealerContext())
            {
                //var inputXml = File.ReadAllText("./../../../Datasets/sales.xml");

                //var result = ImportSales(db, inputXml);
                //Console.WriteLine(result);

                var carsWithDistance = GetCarsWithDistance(context);
                File.WriteAllText("../../../Results/carsWithDistance.xml", carsWithDistance);

            }

            
        }
        //Problem 01
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]),
                                new XmlRootAttribute("Suppliers"));

            ImportSupplierDto[] supplierDtos;

            using (var reader=new StringReader(inputXml))
            {
                 supplierDtos =(ImportSupplierDto[]) xmlSerializer.Deserialize(reader);
            }

            var supplier = Mapper.Map<Supplier[]>(supplierDtos);

            context.Suppliers.AddRange(supplier);
            context.SaveChanges();

            return $"Successfully imported {supplier.Length}";
        }

        //Problem 02
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]),
                               new XmlRootAttribute("Parts"));

            ImportPartDto[] partsDto;

            using (var reader=new StringReader(inputXml))
            {
                partsDto = ((ImportPartDto[])xmlSerializer
                     .Deserialize(reader))
                     .Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId))
                     .ToArray();
            }
            var parts = Mapper.Map<Part[]>(partsDto);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";
        }

        //Problem 03
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            const string rootElement = "Cars";
            
            var carsDto = XmlConverter.Deserializer<ImportCarsDto>(inputXml, rootElement);
            
            var cars = new List<Car>();
           
            foreach (var carDto in carsDto)
            {
                var uniqueParts = carDto.PartCars
                    .Select(p => p.Id).Distinct().ToArray();

                var realParts = uniqueParts
                    .Where(id => context.Parts.Any(p => p.Id == id));

                var car = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance,
                    PartCars = realParts
                            .Select(id => new PartCar()
                            {
                                PartId=id
                            })
                          .ToArray()
                };
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";

        }

        //Problem 04

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            const string rootElement = "Customers";

            var customersDto = XmlConverter.Deserializer<ImortCumstomersDto>(inputXml, rootElement);

            var customers = customersDto
                .Select(c => new Customer
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }

        //Problem 05

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            const string rootElement = "Sales";

            var salesDto = XmlConverter.Deserializer<ImportSalesDto>(inputXml, rootElement);

            var sales = salesDto
                .Where(x => context.Cars.Any(c => c.Id == x.CarId))
                .Select(c => new Sale()
                {
                    CarId = c.CarId,
                    CustomerId = c.CustomerId,
                    Discount = c.Discount
                })
                .ToArray();

            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Length}";
        }

        //Problem 06
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            const string rootElement = "Cars";

            var cars = context
                .Cars
                .Where(c => c.TravelledDistance > 2000000)
                .Select(c => new ExportCarsWithDtistanceDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c=>c.Make)
                .ThenBy(c=>c.Model)
                .Take(10)
                .ToArray();

            var result=XmlConverter.Serialize(cars, rootElement);

            return result;
        }

        //Problem 07
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            const string rootElement = "Cars";

            var cars = context
                .Cars
                .Where(c => c.Model == "BMW")
                .Select(c => new CarFrommakeBWMDto()
                    {
                         Id = c.Id,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    }
                

        }
        //NOT FINISHED

    }
}