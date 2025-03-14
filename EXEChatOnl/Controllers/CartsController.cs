using EXEChatOnl.DTOs;
using EXEChatOnl.Models;
using EXEChatOnl.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace EXEChatOnl.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly MyDbContext _context;
    private ICartRepository _cartRepository;

    public CartsController(MyDbContext context, ICartRepository cartRepository)
    {
        _context = context;
        _cartRepository = cartRepository;
    }

    [HttpGet]
    [EnableQuery]
    public IActionResult GetAllProductsCart()
    {
        var username = User.Identity.Name;
        var user = _context.Users.FirstOrDefault(x=>x.Username == username);
        var customer = _context.Customers.FirstOrDefault(x=>x.UserId == user.Id);
        var carts = _cartRepository.GetCartItems(customer.Id).AsQueryable();
        return Ok(carts);
    }
    
    [Authorize]
    [HttpPost("addToCart/{productId:int}")]
    public IActionResult AddToCart(int productId, [FromBody] int quantity)
    {
        var username = User.Identity.Name;
        var user = _context.Users.FirstOrDefault(x=>x.Username == username);
        var customer = _context.Customers.FirstOrDefault(x=>x.UserId == user.Id);
        if (user == null && customer != null)
        {
            return Unauthorized(new { message = "Người dùng không hợp lệ" });
        }
        _cartRepository.AddToCart(customer.Id, productId, quantity);

        return Ok(new { message = "Thêm vào giỏ hàng thành công!" });
    }
    
    [Authorize]
    [HttpPut("updateCart")]
    public IActionResult UpdateToCart([FromBody] CartRequest cartRequest)
    {
        var username = User.Identity.Name;
        var user = _context.Users.FirstOrDefault(x=>x.Username == username);
        var customer = _context.Customers.FirstOrDefault(x=>x.UserId == user.Id);
        if (user == null && customer != null)
        {
            return Unauthorized(new { message = "Người dùng không hợp lệ" });
        }
        _cartRepository.UpdateToCart(customer.Id, cartRequest.Id ?? 1, cartRequest.quantity );

        return Ok(new { message = "Chỉnh sửa giỏ hàng thành công!" });
    }

}