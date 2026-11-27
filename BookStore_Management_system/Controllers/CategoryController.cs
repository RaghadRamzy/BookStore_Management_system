using BookStore_Management_system.Data;
using BookStore_Management_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Management_system.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BookStoreDbContext _context;
        public CategoryController(BookStoreDbContext context)
        {
            _context = context;
        }
                // get :category/index
                //it retreives all categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Category.ToListAsync();
            return View(categories);
        }
        // ====================================================Create category =========================================
        [HttpGet]   // to catch all get request with create action 
        // get :category/create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]  // to catch all post request with create action
        // post :category/create 
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _context.Category.AddAsync(category); // this tells EF core that we wanna insert this category into the database.
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index)); // to redirect to the index action after creating a new category to make the user see the changes
            }
            return View(category); // if the model state is not valid, we return the same view with the category object to show validation errors
        }
        // ====================================================Edit category =========================================
        [HttpGet]   // to catch all get request with edit action
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]  // to catch all post request with edit action
        public async Task<IActionResult> Edit( Category category)
        {
            
           var NewCategory = await _context.Category.FindAsync(category.Id);
            if (NewCategory == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                NewCategory.Name = category.Name;
                NewCategory.Description = category.Description;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } 
            return View(category);

        }
        // ====================================================Delete category =========================================

        [HttpGet]
         public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
           return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Category category)
        {
            var cat = await _context.Category.FindAsync(category.Id);
            
            if (cat == null)
            {
                return NotFound();
            }
            _context.Category.Remove(cat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
