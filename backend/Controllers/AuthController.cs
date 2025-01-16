using System;

[HttpPost("signup")]
[AllowAnonymous]
public IActionResult SignUp([FromBody] SignUpRequest request)
{
    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
    {
        return BadRequest("Username and password are required.");
    }

    // Simulated database storage (replace with real database)
    Console.WriteLine($"User registered: {request.Username}, Firstname: {request.Firstname}");

    return Ok(new { message = "Signup successful! You can now log in." });
}

public record SignUpRequest(string Username, string Password, string Firstname);
