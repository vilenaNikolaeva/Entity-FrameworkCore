namespace Cinema.DataProcessor
{
    using System;
    using System.Linq;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
                 .Where(r => r.Rating >= rating &&
                        r.Projection.Any(t => t.Tickets.Count > 0))
                 .OrderByDescending(m => m.Rating)
                 .ThenByDescending(m => m.Projection.Sum(p => p.Tickets.Sum(t => t.Price)))
                 .Select(m => new ExportTopMoviesDto
                 {
                     MovieName = m.Title,
                     Rating = m.Rating.ToString("f2"),
                     TotalIncomes = m.Projection.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("f2"),
                     Customers = m.Projection
                     .SelectMany(p => p.Tickets
                            .Select(t => new CustomersExportDto
                            {
                                FirstName = t.Customer.FirstName,
                                LastName = t.Customer.LastName,
                                Balance = t.Customer.Balance.ToString("f2")
                            }))
                     .OrderByDescending(c => c.Balance)
                     .ThenBy(c => c.FirstName)
                     .ThenBy(c => c.LastName)
                     .ToArray()
                 }
            )
                 .Take(10)
                 .ToList();
            var result = JsonConvert.SerializeObject(movies, Formatting.Indented);
            return result;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            
            var customers = context
               .Customers
               .Where(c => c.Age >= age)
               .Select(cu => new ExportTopCustomersDto
               {
                   Customers = new Customer( "FisrtName=" + cu.FirstName + " " + "LastName=" + cu.LastName),
                   SpentMoney = cu.Tickets.Sum(t => t.Price).ToString("f2"),
                   SpentTime = new DateTime(cu.Tickets.Sum(t => t.Projection.Movie.Duration.Ticks)).ToString("hh\\:mm\\:ss")
               });

            var result = JsonConvert.SerializeObject(customers);


        }
    }
}