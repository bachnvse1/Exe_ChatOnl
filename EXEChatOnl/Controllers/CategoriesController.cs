using EXEChatOnl.Models;
using EXEChatOnl.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EXEChatOnl.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("_myAllowSpecificOrigins")] // Áp dụng CORS
    public class CategoriesController : ControllerBase
    {
        private readonly MyDbContext _context;
        private ICategoryRepository _categoryRepository;

        public CategoriesController(MyDbContext context, ICategoryRepository categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }

        // GET: api/Category
        [HttpGet]
        public ActionResult<IEnumerable<Category>> getAllCategories()
        {
          if (_context.Categories == null)
          {
              return NotFound();
          }
            return _categoryRepository.GetAll();
        }
    }