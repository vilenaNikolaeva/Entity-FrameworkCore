using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using ProductShop.XMLHelper;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ProductShop.Dtos.Export;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            // var usersXml = File.ReadAllText("./../../../Datasets/users.xml");           //Imported 100
            // var usersXml = File.ReadAllText("./../../../Datasets/products.xml");           //Imported 200
            // var usersXml = File.ReadAllText("./../../../Datasets/categories.xml");
            // var usersXml = File.ReadAllText("./../../../Datasets/categories-products.xml");
            // var result = GetProductsInRange(context);
            // Console.WriteLine(result);

            //var productsInrange = GetProductsInRange(context);
            //File.WriteAllText("../../../Results/productsInRange.xml", productsInrange);

            //var soldProducts = GetSoldProducts(context);
            //File.WriteAllText("../../../Results/soldProducts.xml", soldProducts);

            //var categroyByProductsCount = GetCategoriesByProductsCount(context);
            //File.WriteAllText("../../../Results/categoryByProductsCount.xml", categroyByProductsCount);


            var usersProduct = GetUsersWithProducts(context);
            File.WriteAllText("../../../Results/usersAndProducts.xml", usersProduct);
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            const string rootElement = "Users";

            var userResult = XmlConverter.Deserializer<ImortUserDto>(inputXml, rootElement);

            var user = userResult
                .Select(u => new User
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age
                })
                .ToArray();

            context.Users.AddRange(user);
            context.SaveChanges();

            return $"Succsessfully imported {user.Length}";
        }
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            const string rootElement = "Products";

            var productsResult = XmlConverter.Deserializer<ImortProductDto>(inputXml, rootElement);
     
            var products = productsResult
                .Select(p => new Product
                {
                    Name = p.Name,
                    Price = p.Price,
                    SellerId = p.SellerId,
                    BuyerId = p.BuyerId
                })
                .ToList();

            context.Products.AddRange(products);
             context.SaveChanges();

            return $"Succsessfully imported {products.Count}";
        }
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            const string rootElement = "Categories";

            var categoriesDto = XmlConverter.Deserializer<ImportCategoriesDto>(inputXml, rootElement);

            var categories = categoriesDto
                .Select(x => new Category
                {
                    Name = x.Name
                })
                .ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Length}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            const string rootElement = "CategoryProducts";

            var categoryProductsDto = XmlConverter.Deserializer<ImportCategoryProductDto>(inputXml, rootElement);

            var categories = categoryProductsDto
                .Where(i =>
                         context.Categories.Any(s => s.Id == i.CategoryId) &&
                         context.Products.Any(s => s.Id == i.ProductId))
                .Select(c => new CategoryProduct
                {
                    CategoryId = c.CategoryId,
                    ProductId = c.ProductId
                })
                .ToArray();

            context.CategoryProducts.AddRange(categories);
             context.SaveChanges();

            return $"Successfully imported {categories.Length}";

        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            const string rootElement = "Products";

            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ExportProductInRangeDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToList();

            var result = XmlConverter.Serialize(products, rootElement);

            return result;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            const string rootElement = "Products";

            var users = context
                .Users
                .Where(u => u.ProductsSold.Count > 0)
                .Select(u => new ExportSoldProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                            .Select(p => new UserProductDto
                            {
                                Name = p.Name,
                                Price = p.Price
                            })
                            .Take(5)
                            .ToArray()
                })
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToArray();

            var result = XmlConverter.Serialize(users, rootElement);
            
            return result;
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            const string rootElement = "Categories";

            var categoriesByProducts = context
                .Categories
                .Select(c => new ExportCategoriesByProductCountDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count(),
                    AveragePrice = c.CategoryProducts
                            .Select(cp => cp.Product)
                            .Average(p => p.Price),
                    TotalRevenue = c.CategoryProducts
                            .Select(cp => cp.Product)
                            .Sum(p => p.Price),
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(t => t.TotalRevenue)
                .Take(5)
                .ToArray();

            var result = XmlConverter.Serialize(categoriesByProducts, rootElement);

            return result;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            const string rootElement = "Users";

            var users = context
                .Users
                .Where(p => p.ProductsSold.Any())
                .Select(u => new ExportUsersDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProduct = new ExportProductCountDto
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold
                                .Select(p => new ExportProducDto
                                {
                                    Name = p.Name,
                                    Price = p.Price
                                })
                                .OrderByDescending(p=>p.Price)
                                .ToArray()
                    }
                })
                .OrderByDescending(u=>u.SoldProduct.Count)
                .Take(10)
                .ToArray();

            var result = XmlConverter.Serialize(users, rootElement);

            return result;
        }
    }
}