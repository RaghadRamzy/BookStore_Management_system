using BookStore_Management_system.Data;
using Microsoft.AspNetCore.Mvc;
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
        //public async Task<IActionResult> Index()
        //{


        //}
    }
}
