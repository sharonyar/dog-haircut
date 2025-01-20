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
            .Select(u => new
            {
                u.Id,
                u.Name,
                AppointmentTime = u.AppointmentTime != null
                    ? u.AppointmentTime.ToString()  // ✅ Convert to string
                    : "No Appointment"
            })
            .ToList();

        return Ok(customers);
    }



    // ✅ Helper function to generate a default time if missing
    private static string GenerateRandomTime()
    {
        Random rand = new Random();
        int hour = rand.Next(8, 18);
        int minute = rand.Next(0, 60);
        return $"{hour:D2}:{minute:D2}"; // ✅ Format: HH:mm
    }



    [HttpPut("appointment")]
    [Authorize]
    public IActionResult SetAppointmentTime([FromBody] UpdateAppointmentRequest request)
    {
        int userId = GetUserIdFromToken();
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        if (string.IsNullOrWhiteSpace(request.AppointmentTime))
        {
            return BadRequest(new { message = "Invalid appointment time." });
        }

        user.AppointmentTime = request.AppointmentTime; // ✅ Update appointment time
        _context.Users.Update(user); // ✅ Ensure EF Core detects the change
        _context.SaveChanges(); // ✅ Save the update

        return Ok(new { message = "Appointment time updated successfully.", appointmentTime = user.AppointmentTime });
    }

    // ✅ DTO to receive appointment time from frontend
    public class UpdateAppointmentRequest
    {
        public string AppointmentTime { get; set; }
    }

    // ✅ Extract `userId` from JWT Token
    private int GetUserIdFromToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
    }

    [HttpDelete("appointment")]
    [Authorize]
    public IActionResult DeleteAppointment()
    {
        int userId = GetUserIdFromToken();
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        _context.Users.Remove(user); // ✅ Completely remove user from database
        _context.SaveChanges();

        return Ok(new { message = "User deleted successfully." });
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


    // ✅ DTO for updating user profile
    public class UpdateUserRequest
    {
        public string Name { get; set; }
    }


}
