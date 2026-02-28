using Microsoft.AspNetCore.Mvc;
using Shop.Entities;

namespace Shop.Controllers;

[ApiController]
[Route("Api/[Controller]")]
public class UserController : Controller
{
    private readonly ShopDBContext _dbContext;

    public UserController()
    {
        _dbContext = new ShopDBContext();
    }

    [HttpGet]
    public List<User> GetAll()
    {
        return _dbContext.Users.ToList();
    }


    [HttpGet("{id}")]
    public User Get(int id)
    {
        return _dbContext.Users.FirstOrDefault(u => u.UserId == id);
    }

}
