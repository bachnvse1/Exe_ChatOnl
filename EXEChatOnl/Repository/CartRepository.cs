using AutoMapper;
using EXEChatOnl.DTOs;
using EXEChatOnl.Models;

namespace EXEChatOnl.Repository;

public class CartRepository : ICartRepository
{
    private readonly List<Cart> _cartItems = new();
    private readonly MyDbContext _context;
    private readonly IMapper _mapper;

    public CartRepository(MyDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void AddToCart(int customerId, int productId, int quantity)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        var cart = _context.Carts.Where(c => c.CustomerId == customerId).ToList();
        
        var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            _context.Carts.Update(existingItem);
            _context.SaveChanges();
        }
        else
        {
            _context.Carts.Add(new Cart()
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = quantity,
            });
            _context.SaveChanges();
        }
    }
    
    public void UpdateToCart(int customerId, int cartId, int quantity)
    {
        var cart = _context.Carts.FirstOrDefault(c => c.CustomerId == customerId && c.Id == cartId);
        if (cart.Quantity <= 1 && quantity == -1)
        {
            _context.Carts.Remove(cart);
            _context.SaveChanges();
        }
        else
        {
            if (cart != null)
            {
                cart.Quantity += quantity;
                _context.Carts.Update(cart);
                _context.SaveChanges();
            }
        }
    }

    public void RemoveFromCart(Product product)
    {
        var item = _cartItems.FirstOrDefault(c => c.Product.Id == product.Id);
        if (item != null)
        {
            _cartItems.Remove(item);
        }
    }

    public void ClearCart()
    {
        _cartItems.Clear();
    }

    public IEnumerable<CartRequest> GetCartItems(int customerId)
    {
        var carts = _context.Carts.Where(x=>x.CustomerId == customerId).ToList();
        var mappedCarts = _mapper.Map<List<CartRequest>>(carts);
        foreach (var cart in mappedCarts)
        {
            cart.imageProduct = _context.Products.FirstOrDefault(p => p.Id == cart.ProductId).ImageUrl;
            cart.productName = _context.Products.FirstOrDefault(p => p.Id == cart.ProductId).Name;
            cart.productPrice = _context.Products.FirstOrDefault(p => p.Id == cart.ProductId).Price;
            cart.totalPrice = cart.productPrice * cart.quantity;
        }

        return mappedCarts.AsQueryable();
    }

    public decimal GetTotalPrice()
    {
        return (decimal)_cartItems.Sum(item => (item.Product.Price ?? 0 ) * item.Quantity);
    }
}
