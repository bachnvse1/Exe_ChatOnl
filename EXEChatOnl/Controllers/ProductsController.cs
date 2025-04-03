using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EXEChatOnl.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EXEChatOnl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.OData.Query;

namespace EXEChatOnl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("_myAllowSpecificOrigins")] // Áp dụng CORS
    public class ProductsController : ControllerBase
    {
        private readonly MyDbContext _context;

        private readonly IMapper _mapper;

        public ProductsController(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductSearchRequest>>> GetProducts()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == null)
            {
                return Forbid();
            }
            if (_context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products.ToListAsync();
            
            var mappedProducts = _mapper.Map<List<ProductSearchRequest>>(products);


            return Ok(mappedProducts.AsQueryable());
        }


        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductSearchRequest request)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "admin")
            {
                return Forbid();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            product.Name = request.Name ?? product.Name;
            product.Description = request.Description ?? product.Description;
            product.Price = request.Price ?? product.Price;
            product.ImageUrl = request.ImageUrl ?? product.ImageUrl;
            product.ShopeeUrl = request.ShopeeUrl ?? product.ShopeeUrl;
            product.CategoryId = request.CategoryId ?? product.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(CreateProductRequest request)
        {
            // Kiểm tra quyền của người dùng
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "admin")
            {
                return Forbid();  // Nếu không phải admin, trả về lỗi Forbidden
            }

            if (_context.Products == null)
            {
                return Problem("Entity set 'DBContext.Products' is null.");
            }

            try
            {
                // Kiểm tra giá trị price
                if (request.Price <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Giá sản phẩm phải lớn hơn 0!"  // Trả về thông báo khi giá sản phẩm không hợp lệ
                    });
                }

                // Kiểm tra danh mục sản phẩm
                var category = _context.Categories.FirstOrDefault(c => c.Name == request.Category);
                if (category == null)
                {
                    return BadRequest(new
                    {
                        message = "Danh mục sản phẩm không hợp lệ!"  // Trả về thông báo khi không tìm thấy danh mục
                    });
                }

                // Tạo sản phẩm mới
                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    Description = request.Description,
                    CategoryId = category.Id,
                    ShopeeUrl = "",  // Có thể cập nhật sau
                    ImageUrl = request.Image
                };

                // Thêm sản phẩm vào cơ sở dữ liệu
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Trả về thông báo thành công
                return Ok(new
                {
                    message = "Tạo sản phẩm thành công!"  // Thông báo thành công
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    message = $"Lỗi khi tạo sản phẩm: {e.Message}"  // Thông báo lỗi chi tiết
                });
            }
        }


        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "admin")
            {
                return Forbid();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.IsDeleted = !product.IsDeleted;
            await _context.SaveChangesAsync();

            return Ok(new { message = product.IsDeleted ? "Sản phẩm đã bị vô hiệu hóa." : "Sản phẩm đã được kích hoạt lại." });
        }


        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
