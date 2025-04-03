using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EXEChatOnl.DTOs;
using EXEChatOnl.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EXEChatOnl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MyDbContext _context;

        private readonly IMapper _mapper;

        public OrdersController(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var customer = _context.Customers.FirstOrDefault(x => x.UserId == Int32.Parse(userId));
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var order = new Order
                    {
                        CustomerId = customer.Id,
                        CreatedAt = DateTime.Now,
                        TotalPrice = request.TotalPrice,
                        Status = request.Status,
                    };
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    foreach (var item in request.OrderDetails)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Price / item.Quantity
                        };
                        _context.OrderDetails.Add(orderDetail);
                    }

                    await _context.SaveChangesAsync();

                    var cartUser = _context.Carts.Where(x => x.CustomerId == customer.Id).ToList();
                    _context.Carts.RemoveRange(cartUser);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return Ok(new
                    {
                        message = "Đơn hàng được tạo thành công!"
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { message = "Lỗi khi tạo đơn hàng", error = ex.Message });
                }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersRequest>>> GetOrders()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cusId = _context.Customers.FirstOrDefault(x => x.UserId == Int32.Parse(userId))?.Id;
            if (userRole == null) return Forbid();

            IQueryable<Order> query = _context.Orders.Where(x => x.IsDeleted == false);

            if (userRole != "admin")
            {
                if (string.IsNullOrEmpty(userId)) return Forbid();
                query = query.Where(x => x.CustomerId == cusId);
            }

            var orders = query.ProjectTo<OrdersRequest>(_mapper.ConfigurationProvider).ToList();
            return Ok(orders);
        }
    }
}
