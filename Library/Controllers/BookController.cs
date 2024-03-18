using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace Library.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly LibraryDbContext dbContext;

        public BookController(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await dbContext
                .Books
                .AsNoTracking()
                .Select(b => new AllBooksViewModel
                {
                    ImageUrl = b.ImageUrl,
                    Title = b.Title,
                    Author = b.Author,
                    Rating = b.Rating.ToString(),
                    Category = b.Category.Name,
                    Id = b.Id
                }).ToListAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var currentUserId = GetUserId();

            var currentUserBooks = await dbContext
                .Books
                .Include(b => b.UsersBooks)
                .Where(b => b.UsersBooks.Any(ub => ub.CollectorId == currentUserId))
                .AsNoTracking()
                .Select(b => new MyBooksViewModel
                {
                    ImageUrl = b.ImageUrl,
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    Category = b.Category.Name,
                    Id = b.Id
                }).ToListAsync();

            return View(currentUserBooks);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var bookForm = new AddBookViewModel();
            bookForm.Categories = await GetCategories();

            return View(bookForm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookViewModel bookForm)
        {
            decimal rating;

            if (!decimal.TryParse(bookForm.Rating, out rating) || rating < 0 || rating > 10)
            {
                ModelState.AddModelError(nameof(bookForm.Rating), "Rating must be a number between 0 and 10.");

                bookForm.Categories = await GetCategories();

                return View(bookForm);
            }

            if (!ModelState.IsValid)
            {
                bookForm.Categories = await GetCategories();

                return View(bookForm);
            }

            Book newBook = new Book()
            {
                Title = bookForm.Title,
                Author = bookForm.Author,
                Description = bookForm.Description,
                ImageUrl = bookForm.Url,
                //Rating = bookForm.Rating,
                Rating = decimal.Parse(bookForm.Rating),
                CategoryId = bookForm.CategoryId
            };

            await dbContext.AddAsync(newBook);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int id)
        {
            var currentUserId = GetUserId();

            if (!await dbContext.Books
                .AsNoTracking()
                .AnyAsync(b => b.Id == id))
            {
                return BadRequest();
            }

            if (await dbContext.IdentityUserBooks
                .AsNoTracking()
                .AnyAsync(iub => iub.BookId == id && iub.CollectorId == currentUserId))
            {
                return RedirectToAction(nameof(All));
            }

            var userBook = new IdentityUserBook()
            {
                BookId = id,
                CollectorId = currentUserId
            };

            await dbContext.IdentityUserBooks.AddAsync(userBook);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int id)
        {
            var currentUserId = GetUserId();

            if (!await dbContext.Books
                .AsNoTracking()
                .AnyAsync(b => b.Id == id))
            {
                return BadRequest();
            }

            if (!await dbContext.IdentityUserBooks
                .AsNoTracking()
                .AnyAsync(iub => iub.BookId == id && iub.CollectorId == currentUserId))
            {
                return RedirectToAction(nameof(Mine));
            }

            var currentUserBook = await dbContext.IdentityUserBooks
                .Where(iub => iub.CollectorId == currentUserId)
                .FirstOrDefaultAsync();

            if (currentUserBook == null)
            {
                return RedirectToAction(nameof(Mine));
            }

            dbContext.IdentityUserBooks.Remove(currentUserBook);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Mine));
        }


        private async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            return await dbContext.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        private string GetUserId()
        {
            string id = string.Empty;

            if (User != null)
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return id;
        }
    }
}
