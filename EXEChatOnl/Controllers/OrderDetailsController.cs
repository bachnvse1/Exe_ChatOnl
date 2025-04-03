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
    public class OrderDetailsController : ControllerBase
    {
        private readonly MyDbContext _context;

        private readonly IMapper _mapper;

        public OrderDetailsController(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailsRequest>>> GetOrderDetails()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userRole == null || string.IsNullOrEmpty(userId)) return Forbid();

            var cusId = _context.Customers.FirstOrDefault(x => x.UserId == Int32.Parse(userId))?.Id;
            if (cusId == null) return NotFound("Customer not found");

            IQueryable<Order> orderQuery = _context.Orders.Where(x => x.IsDeleted == false);

            if (userRole != "admin")
            {
                orderQuery = orderQuery.Where(x => x.CustomerId == cusId);
            }

            var orderIds = await orderQuery.Select(o => o.Id).ToListAsync();

            var orderDetails = await _context.OrderDetails
                .Where(od => orderIds.Contains(od.OrderId))
                .ProjectTo<OrderDetailsRequest>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(orderDetails);
        }

    }
}
