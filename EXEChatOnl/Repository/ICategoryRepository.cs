using EXEChatOnl.Models;
using Microsoft.AspNetCore.Mvc;

namespace EXEChatOnl.Repository;

public interface ICategoryRepository
{
    ActionResult<IEnumerable<Category>> GetAll();
    Category? GetById(int id);
    void Add(Category category);
    void Update(Category category);
    void Delete(int id);
}