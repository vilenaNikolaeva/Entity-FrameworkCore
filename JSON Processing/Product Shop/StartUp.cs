using Newtonsoft.Json;
using Product_Shop.Data;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db=new ProductShopContext())
            {
                //var inputJson = File.ReadAllText("./../../../Dataset/categories-products.json");
                var result = GetUsersWithProducts(db);

                Console.WriteLine(result);
            }
        }
        //Problem 01
       public  static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Seccessfully imported {users.Length}";
        }
        //Problem 02
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Seccessfully imported {products.Length}";
        }
        //Problem 03
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson);

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Seccessfully imported {categories.Length}";
        }
        //Problem 04 
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categories = context
                .Categories
                .Select(c => c.Id);
            var products = context
                .Products
                .Select(p => p.Id);

            var categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
            var validEntities = new List<CategoryProduct>();

            foreach (var categoryProduct in categoriesProducts)
            {
                validEntities.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(validEntities);
            context.SaveChanges();

            return $"Successfully imported {validEntities.Count}";
                
        }
        //Problem 05
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name= p.Name,
                    price= p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"

                })
                .ToList();

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }
        //Problem 06
        public static string GetSoldProducts(ProductShopContext context)
        {
            var soldProducts = context
                .Users
                .Where(u => u.ProductsSold.Count > 1)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                    .Select(u => new
                    {
                        name = u.Name,
                        price = u.Price,
                        buyerFirstName = u.Buyer.FirstName,
                        buyerLastName = u.Buyer.LastName

                    })
                })
                .OrderBy(u => u.lastName)
                .ThenBy(u => u.firstName)
                .ToList();

            var json = JsonConvert.SerializeObject(soldProducts, Formatting.Indented);

            return json;
        }
        //Problem 07
        public static string GetCategoriesByProductCount(ProductShopContext context)
        {
            var categories = context
                .Categories
                .Select(c => new
                {
                    category = c.Name,
                    productCount = c.CategoryProducts
                        .Select(cp => cp.Product)
                        .Count(),
                    avaragePrice = c.CategoryProducts
                        .Average(p => p.Product.Price).ToString("f2"),
                    totalRevanue = c.CategoryProducts
                        .Select(cp => cp.Product)
                        .Sum(p => p.Price)
                })
                .OrderByDescending(c => c.productCount)
                .ToList();


           var json = JsonConvert.SerializeObject(categories,Formatting.Indented);

            return json;
        }
        //Problem 08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context
                .Users                                 
                .Where(u => u.ProductsSold.Any())
                .Select(u => new                        
                {
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new                
                    {
                        count = u.ProductsSold
                        .Where(p => p.Buyer != null)
                          .Count(),
                        products = u.ProductsSold    
                        .Where(p=>p.Buyer !=null)
                                .Select(ps => new       
                                {
                                    name = ps.Name,
                                    price = ps.Price
                                })
                                .ToList()
                    }
                })
                .OrderByDescending(u => u.soldProducts.count)
                .ToList();

            var usersOutput = new
            {
                usersCount = users.Count(),
                users = users
            };

            var json = JsonConvert.SerializeObject(usersOutput,new JsonSerializerSettings()
            { 
                Formatting=Formatting.Indented,
                NullValueHandling=NullValueHandling.Ignore
            });

            return json;
        }
    }
}