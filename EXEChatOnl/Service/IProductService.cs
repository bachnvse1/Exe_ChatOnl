using System.Collections.Generic;
using EXEChatOnl.Models;

namespace EXEChatOnl.Service
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        
        Product GetProductById(int id);
        
        void AddProduct(Product product);
        
        void UpdateProduct(Product product);
        
        void DeleteProduct(int id);
    }
}