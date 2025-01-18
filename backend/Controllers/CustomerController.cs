using Microsoft.AspNetCore.Authorization;
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
    public IActionResult GetAllCustomers()
    {
        var customers = _context.Users
            .Where(u => !string.IsNullOrEmpty(u.Name)) // ✅ Only return users with names
            .Select(u => new
            {
                u.Id,
                u.Name // ✅ Only return Name (hide password)
            })
            .ToList();

        if (!customers.Any())
        {
            return NotFound("No customers found.");
        }

        return Ok(customers);
    }

    [HttpPut("me")]
    [Authorize] // ✅ Requires authentication
    public IActionResult EditMyProfile([FromBody] UpdateUserRequest request)
    {
        int userId = GetUserIdFromToken(); // ✅ Get logged-in user ID
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        // ✅ Only allow changing `Name`
        user.Name = request.Name;

        _context.SaveChanges();

        return Ok(new { message = "Profile updated successfully." });
    }

    // ✅ Extract `userId` from JWT Token
    private int GetUserIdFromToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
    }

    // ✅ DTO for updating user profile
    public class UpdateUserRequest
    {
        public string Name { get; set; }
    }


}
