using BookStore_Management_system.Data;
using BookStore_Management_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Management_system.Controllers
{
    public class BookController : Controller
    {
        private readonly BookStoreDbContext _context;
        public BookController(BookStoreDbContext context)
        {
            _context = context;
        }
        // to retreive all books including the category.
       
        
            public async Task<IActionResult> Index(string? searchString)
             {
                  var query = _context.Books
                   .Include(b => b.Category)
                   .AsQueryable();

                  if (!string.IsNullOrEmpty(searchString))
                   {
                      query = query.Where(b =>
                       b.Title.Contains(searchString) ||
                       b.Author.Contains(searchString));
             }

              var books = await query.ToListAsync();

              ViewBag.SearchString = searchString;

                return View(books);
        }

      
        // create book
        [HttpGet]
        public  IActionResult Create()
        {
            var categories = _context.Category.ToList();
            ViewBag.Category = new SelectList(_context.Category, "Id", "Name");  // wanna change this viewBag later.
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine(
                            $"Property: {item.Key}, Error: {error.ErrorMessage}"
                        );
                    }
                }
            }
            if (ModelState.IsValid)
            {
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Category = new SelectList(_context.Category, "Id", "Name");
            return View(book);
        }
        // edit book
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.Category = new SelectList(_context.Category, "Id", "Name", book.CategoryId);
            return View(book);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                var existingBook = await _context.Books.FindAsync(book.Id);

                if (existingBook == null)
                {
                    return NotFound();
                }

                //existingBook.Title = book.Title;
                //existingBook.Author = book.Author;
                //existingBook.ISBN = book.ISBN;
                //existingBook.Price = book.Price;
                //existingBook.Quantity = book.Quantity;
                //existingBook.CategoryId = book.CategoryId;
                _context.Entry(existingBook).CurrentValues.SetValues(book); // instead of these lines but be careful when you using this line
                                                                            // because it will update all the properties of the existingBook with the values from the book object,
                                                                            // including any properties that may not have been changed. Make sure to validate and
                                                                            // sanitize the input data before using this approach.

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(
                _context.Category,
                "Id",
                "Name",
                book.CategoryId
            );

            return View(book);
        }

        // delete book
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books
        .Include(b => b.Category)
        .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // view book details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

    }
}
