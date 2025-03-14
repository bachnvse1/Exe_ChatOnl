using EXEChatOnl.DTOs;
using EXEChatOnl.Models;

namespace EXEChatOnl.Repository;

public interface ICartRepository
{
    void AddToCart(int customerId, int productId, int quantity);
    void UpdateToCart(int customerId, int cartId, int quantity);
    void RemoveFromCart(Product product);
    void ClearCart();
    IEnumerable<CartRequest> GetCartItems(int customerId);
    decimal GetTotalPrice();
}