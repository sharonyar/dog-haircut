using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

[Route("api/customers")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly AppointmentService _appointmentService;

    public CustomerController(AppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public IActionResult GetAllCustomers()
    {
        try
        {
            var customers = _appointmentService.GetAllCustomers();

            if (!customers.Any())
            {
                return NotFound(new { message = "No customers found." });
            }

            return Ok(customers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving customers.", error = ex.Message });
        }
    }

    [HttpPut("appointment")]
    [Authorize]
    public IActionResult SetAppointmentTime([FromBody] UpdateAppointmentRequest request)
    {
        int userId = GetUserIdFromToken();
        string errorMessage;

        if (_appointmentService.SetAppointmentTime(userId, request.AppointmentTime, out errorMessage))
        {
            return Ok(new { message = "Appointment time updated successfully." });
        }

        return BadRequest(new { message = errorMessage });
    }

    [HttpDelete("appointment")]
    [Authorize]
    public IActionResult DeleteAppointment()
    {
        int userId = GetUserIdFromToken();
        string errorMessage;

        if (_appointmentService.DeleteAppointment(userId, out errorMessage))
        {
            return Ok(new { message = "User deleted successfully." });
        }

        return BadRequest(new { message = errorMessage });
    }

    [HttpPut("me")]
    [Authorize]
    public IActionResult EditMyProfile([FromBody] UpdateUserRequest request)
    {
        int userId = GetUserIdFromToken();
        string errorMessage;

        if (_appointmentService.EditUserProfile(userId, request.Name, out errorMessage))
        {
            return Ok(new { message = "Profile updated successfully." });
        }

        return BadRequest(new { message = errorMessage });
    }

    private int GetUserIdFromToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
    }

    public class UpdateAppointmentRequest
    {
        public string AppointmentTime { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Name { get; set; }
    }
}
