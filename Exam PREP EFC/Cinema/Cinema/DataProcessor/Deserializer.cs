namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesDto = JsonConvert.DeserializeObject<MoviesImportDto[]>(jsonString);
            var movies = new List<Movie>();
            var sb = new StringBuilder();

            foreach (var dto in moviesDto)
            {
                if (IsValid(dto))
                {
                    var movie = new Movie()
                    {
                        Title = dto.Title,
                        Genre = dto.Genre,
                        Duration = dto.Duration,
                        Rating = dto.Rating,
                        Director = dto.Director
                    };
                    movies.Add(movie);
                    sb.AppendLine(string.Format(SuccessfulImportMovie, dto.Title, dto.Genre, dto.Rating.ToString("f2")));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }

            }
            context.Movies.AddRange(movies);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallSeatsDb = JsonConvert.DeserializeObject<HallImportDto[]>(jsonString);
            var hallSeats = new List<Hall>();
            var sb=new StringBuilder();

            foreach (var dto in hallSeatsDb)
            {
                if (IsValid(dto))
                {
                    var hall = new Hall()
                    {
                        Name = dto.Name,
                        Is4Dx = dto.Is4Dx,
                        Is3D = dto.Is3D,
                    };
                    context.Halls.Add(hall);
                    AddSeatsInToDatabase(context, hall.Id, dto.Seats);
                    var projectionType = GetProjectionType(hall);
                    sb.AppendLine(string.Format(SuccessfulImportHallSeat, dto.Name, projectionType, dto.Seats));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }
           context.SaveChanges();
            return sb.ToString().Trim();
        }

        
        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var serializerDto = new XmlSerializer(typeof(ImportProjectionDto[]),
                                new XmlRootAttribute("Projections"));
            var serializer =(ImportProjectionDto[])serializerDto.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            foreach (var dto in serializer)
            {
                if (IsValid(dto) 
                    && IsValidMovieId(context,dto.MovieId)
                    && IsValidHallId(context, dto.HallId))
                {
                    var projection = new Projection()
                    {
                        MovieId = dto.MovieId,
                        HallId = dto.HallId,
                        DateTime = DateTime.ParseExact(dto.DataTime,
                                                       "yyyy-MM-dd HH:mm:ss",
                                                       CultureInfo.InvariantCulture)
                    };
                    context.Projections.Add(projection);
                    sb.AppendLine(String.Format(
                        SuccessfulImportProjection, 
                        projection.Movie.Title,
                        projection.DateTime.ToString("MM//dd/yyyy",CultureInfo.InvariantCulture)));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }
           
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializationDto = new XmlSerializer(typeof(CustomerImportDto[])
                , new XmlRootAttribute("Customers"));
            var serialization = (CustomerImportDto[])serializationDto.Deserialize(new StringReader(xmlString));
    
            var sb = new StringBuilder();

            foreach (var c in serialization)
            {
                if (IsValid(c))
                {
                    var customer = new Customer()
                    {
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Age = c.Age,
                        Balance = c.Balance,
                    };
                    context.Customers.Add(customer);
                    AddCustomerTickets(context,customer.Id, c.Tickets);
                    sb.AppendLine(String.Format(SuccessfulImportCustomerTicket, c.FirstName, c.LastName, c.Tickets.Length));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }
            
           context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static void AddCustomerTickets(CinemaContext context, int customerId, CustomerTicketstDto[] ticketsDto)
        {

            var tickets = new List<Ticket>();

            foreach (var dto in ticketsDto)
            {
                if (IsValid(dto))
                {
                    var ticket = new Ticket()
                    {
                        ProjectionId = dto.ProjectionId,
                        CustomerId = customerId,
                        Price = dto.Price
                    };
                    tickets.Add(ticket);
                }
            }
            context.Tickets.AddRange(tickets);
          context.SaveChanges();

        }

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationResult, true);

            return result;

        }
        private static string GetProjectionType(Hall hall)
        {
            var result = "Normal";

            if (hall.Is3D && hall.Is4Dx) result = "4Dx/3D";
            else if (hall.Is3D) result = "3D";
            else if (hall.Is4Dx) result = "4Dx";

            return result;
        }

        private static void AddSeatsInToDatabase(CinemaContext context, int hallId, int seatsCount)
        {
            var seats = new List<Seat>();

            for (int i = 0; i < seatsCount; i++)
            {
                seats.Add(new Seat { HallId = hallId });
            }
            context.AddRange(seats);
            context.SaveChanges();
        }
        private static bool IsValidHallId(CinemaContext context, int hallId)
        {
            return context.Halls.Any(h => h.Id == hallId);
        }

        private static bool IsValidMovieId(CinemaContext context, int movieId)
        {
            return context.Movies.Any(m => m.Id == movieId);
        }
    }
}