using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Book_Shop.Data;
using Book_Shop.Models.Enums;


namespace Book_Shop
{
    class StartUp
    {
        static void Main(string[] args)
        {
            var context = new BookShopContext();

            using (context)
            {
                //Problem 1
                // Console.WriteLine(GetBooksAgeRestriction(context);

                //Problem 2
                //Console.WriteLine(GetGoldenBooks(context));

                //Problem 3
                //Console.WriteLine(GetBooksByPrice(context));

                //Problem 4
                //Console.WriteLine(GetBookNotReleasedIn(context));

                //Problem 5
                //Console.WriteLine(GetBooksByCategory(context));

                //Problem 6
                //Console.WriteLine(GetBooksReleasedBefore(context));

                //Problem 7 
                //Console.WriteLine(GetAuthorNamesEndingIn(context));

                //Problem 8 
                //Console.WriteLine(GetBooksTitlesContaining(context));

                //Problem 9
                //Console.WriteLine(GetBooksByAuthor(context));

                //Problme 10
                //Console.WriteLine(CountBooks(context));

                //Problem 11
                //Console.WriteLine(CountCopiesByAuthor(context));

                //Problem 12
                //Console.WriteLine(GetTotalProfitByCategory(context));

                //Problem 13
                //Console.WriteLine(GetMostRecentBooks(context));

                //Problem 14
                //Console.WriteLine(IncreasePrices(context));

                //Problem15
                //Console.WriteLine(RemoveBooks(context));
            }
        }

        public static string GetBooksAgeRestriction(BookShopContext context, string command = null)
        {
            var sb = new StringBuilder();

            if (command == null)
            {
                command = Console.ReadLine();
            }

            var ageRestriction = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);

            var books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .ToList()
                .OrderBy(b => b);

            foreach (var book in books)
            {
                sb.AppendLine($"{book}");
            }
            return sb.ToString().TrimEnd();

        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var goldenBooks = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .ToList()
                .OrderBy(b => b.BookId);

            foreach (var book in goldenBooks)
            {
                sb.AppendLine($"{book.Title}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .ToList()
                .OrderBy(p => p.Price);

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBookNotReleasedIn(BookShopContext context, int? year = null)
        {
            if (year == null)
            {
                year = int.Parse(Console.ReadLine());
            }
            var sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .ToList()
                .OrderBy(b => b.BookId);

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByCategory(BookShopContext context, string input = null)
        {
            if (input == null)
            {
                input = Console.ReadLine();
            }
            var sb = new StringBuilder();

            var categories = input.Split().ToList();

            var booksCategory = context.Books
                .Select(b => new
                {
                    b.Title,
                    categoryName = b.BookCategories
                            .Select(bc => bc.Category.Name)
                            .SingleOrDefault()
                })
                .Where(b => categories.Any(bc => bc.ToLower() == b.categoryName.ToLower()))
                .ToList()
                .OrderBy(b => b.Title);

            foreach (var book in booksCategory)
            {
                sb.AppendLine($"{book.Title}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string input = null)
        {
            var sb = new StringBuilder();
            if (input == null)
            {
                input = Console.ReadLine();
            }
            var date = DateTime.ParseExact(input, "dd-mm-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < date)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    ReleaseDate = b.ReleaseDate.Value
                })
                .ToList()
                .OrderByDescending(b => b.ReleaseDate);

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}- {book.EditionType} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input = null)
        {
            var sb = new StringBuilder();

            if (input == null)
            {
                input = Console.ReadLine();
            }

            var names = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                })
                .ToList()
                .OrderBy(a => a.FullName);

            foreach (var name in names)
            {
                sb.AppendLine($"{name.FullName}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksTitlesContaining(BookShopContext context, string input = null)
        {
            var sb = new StringBuilder();

            if (input==null)
            {
                input = Console.ReadLine();
            }

            var books = context.Books
                .Where(b => b.Title.Contains(input.ToLower()))
                .Select(b => new
                {
                    b.Title
                })
                .ToList()
                .OrderBy(b => b.Title);

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByAuthor(BookShopContext context, string input = null)
        {
            var sb = new StringBuilder();

            if (input==null)
            {
                input = Console.ReadLine();
            }

            var books = context.Books
                .Where(a => a.Author.LastName.StartsWith(input.ToLower()))
                .Select(b => new
                {
                    b.Title,
                    Author=b.Author.FirstName+" "+b.Author.LastName,
                    b.BookId
                })
                .ToList()
                .OrderBy(b => b.BookId);

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.Author})");
            }
            return sb.ToString().TrimEnd(); 
        }
        public static int CountBooks(BookShopContext context, int? lenghtCheck = null)
        {
            var sb = new StringBuilder();

            if (lenghtCheck==null)
            {
                lenghtCheck = int.Parse(Console.ReadLine());
            }

            var books = context.Books
                .Where(b => b.Title.Length > lenghtCheck)
                .Count();

            return books;
        }
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Authors
                .Select(a => new
                {
                    Name = a.FirstName + " " + a.LastName,
                    Copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.Copies)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Name} - {book.Copies}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Categories
                .Select(b => new
                {
                    b.Name,
                    TotalProfit = b.CategoryBooks
                            .Select(c => c.Book.Price * c.Book.Copies)
                            .Sum()
                })
                .OrderByDescending(t => t.TotalProfit)
                .ThenBy(t => t.Name)
                .ToList();


            foreach (var book in books)
            {
                sb.AppendLine($"{book.Name} ${book.TotalProfit}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var booksByCategory = context.Categories
                            .Select(c => new
                            {
                                c.Name,
                                Books = c.CategoryBooks
                                    .Select(b => new
                                    {
                                        Title = b.Book.Title,
                                        Date = b.Book.ReleaseDate.Value,
                                        Copies = b.Book.Copies
                                    })
                                    .OrderByDescending(b => b.Date)
                                    .Take(3)
                                    .ToArray()
                            })
                            .ToArray()
                            .OrderBy(c => c.Name);

            foreach (var categoryBooks in booksByCategory)
            {
                sb.AppendLine($"--{categoryBooks.Name}");
                foreach (var book in categoryBooks.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.Date.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static string IncreasePrices (BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(b =>b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Price+=5}");
            }
            return sb.ToString().TrimEnd();
        }
        public static int RemoveBooks(BookShopContext context)
        {
            var removedBooks = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            var booksCount = removedBooks.Count();

            context.Books.RemoveRange(removedBooks);
           
            return booksCount;
        }
    }
}
