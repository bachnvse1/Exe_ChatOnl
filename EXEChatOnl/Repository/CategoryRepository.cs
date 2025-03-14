using EXEChatOnl.Models;
using Microsoft.AspNetCore.Mvc;

namespace EXEChatOnl.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly MyDbContext _context;

    public CategoryRepository(MyDbContext context)
    {
        _context = context;
    }

    public ActionResult<IEnumerable<Category>> GetAll()
    {
        return _context.Categories.ToList();
    }

    public Category? GetById(int id)
    {
        return _context.Categories.Find(id);
    }

    public void Add(Category category)
    {
        _context.Categories.Add(category);
        Save();
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
        Save();
    }

    public void Delete(int id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            Save();
        }
    }
    public void Save()
    {
        _context.SaveChanges();
    }
}