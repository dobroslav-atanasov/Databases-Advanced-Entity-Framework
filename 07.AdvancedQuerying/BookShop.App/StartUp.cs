namespace BookShop
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            BookShopContext dbContext = new BookShopContext();

            using (dbContext)
            {
                int result = RemoveBooks(dbContext);
                Console.WriteLine(result);
            }
        }

        public static int RemoveBooks(BookShopContext dbContext)
        {
            Book[] books = dbContext
                .Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            dbContext.Books.RemoveRange(books);
            dbContext.SaveChanges();

            return books.Length;
        }

        public static void IncreasePrices(BookShopContext dbContext)
        {
            Book[] books = dbContext
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (Book book in books)
            {
                book.Price += 5;
            }

            dbContext.SaveChanges();
        }

        public static string GetMostRecentBooks(BookShopContext dbContext)
        {
            var categories = dbContext
                .Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks
                        .Select(bc => new
                        {
                            Title = bc.Book.Title,
                            ReleaseDate = bc.Book.ReleaseDate
                        })
                        .OrderByDescending(bc => bc.ReleaseDate)
                        .Take(3)
                })
                .OrderBy(c => c.CategoryName)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");
                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().Trim();
        }

        public static string GetTotalProfitByCategory(BookShopContext dbContext)
        {
            var categories = dbContext
                .Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Profit = c.CategoryBooks.Sum(bc => bc.Book.Copies * bc.Book.Price)
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.CategoryName)
                .ToArray();

            string result = string.Join(Environment.NewLine, categories.Select(c => $"{c.CategoryName} ${c.Profit:F2}"));
            return result;
        }

        public static string CountCopiesByAuthor(BookShopContext dbContext)
        {
            var authors = dbContext
                .Authors
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}",
                    Copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.Copies)
                .ToArray();

            string result = string.Join(Environment.NewLine, authors.Select(a => $"{a.FullName} - {a.Copies}"));
            return result;
        }

        public static int CountBooks(BookShopContext dbContext, int lengthCheck)
        {
            int countBooks = dbContext
                .Books
                .Count(b => b.Title.Length > lengthCheck);

            return countBooks;
        }

        public static string GetBooksByAuthor(BookShopContext dbContext, string input)
        {
            string[] books = dbContext
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBookTitlesContaining(BookShopContext dbContext, string input)
        {
            string[] books = dbContext
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext dbContext, string input)
        {
            string[] authors = dbContext
                .Authors
                .Where(a => a.FirstName != null && a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(a => a)
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }

        public static string GetBooksReleasedBefore(BookShopContext dbContext, string input)
        {
            DateTime date = DateTime.ParseExact(input, "dd-MM-yyyy", null);

            string[] books = dbContext
                .Books
                .Where(b => b.ReleaseDate < date)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext dbContext, string input)
        {
            string[] categories = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToLower()).ToArray();

            string[] books = dbContext
                .Books
                .Where(b => b.BookCategories.Select(bc => bc.Category.Name.ToLower()).Intersect(categories).Any())
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksNotRealeasedIn(BookShopContext dbContext, int year)
        {
            string[] books = dbContext
                .Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByPrice(BookShopContext dbContext)
        {
            string[] books = dbContext
                .Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:F2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetGoldenBooks(BookShopContext dbContext)
        {
            string[] books = dbContext
                .Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAgeRestriction(BookShopContext dbContext, string input)
        {
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(input, true);

            string[] books = dbContext
                .Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }
    }
}