using Microsoft.AspNetCore.Mvc;

[Route("api/customers")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly DataContext _context;

    public CustomerController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetCustomers()
    {
        var customers = _context.Customers.ToList();
        return Ok(customers);
    }
}
