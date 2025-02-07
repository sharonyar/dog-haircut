namespace backend.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly DataContext _context;

        public ScheduleController(DataContext context)
        {
            _context = context;
        }

        // ✅ GET: Fetch all scheduled appointments
        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            var appointments = _context.Schedules
                .Select(s => new
                {
                    s.Id,
                    s.UserId,
                    s.User.Name,
                    s.Time
                })
                .ToList();

            return Ok(appointments);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddAppointment([FromBody] CreateAppointmentRequest request)
        {
            try
            {
                int userId = GetUserIdFromToken();

                // ✅ Check if the user already has an appointment
                if (_context.Schedules.Any(s => s.UserId == userId))
                {
                    return BadRequest(new { message = "You already have an appointment." }); // ✅ Return JSON
                }

                var newAppointment = new Schedule
                {
                    UserId = userId,
                    Time = request.Time
                };

                _context.Schedules.Add(newAppointment);
                _context.SaveChanges();

                return Ok(newAppointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the appointment.", error = ex.Message });
            }
        }


        // ✅ PUT: Edit appointment time
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult EditAppointment(int id, [FromBody] EditAppointmentRequest request)
        {
            int userId = GetUserIdFromToken();
            var appointment = _context.Schedules.FirstOrDefault(s => s.Id == id && s.UserId == userId);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            appointment.Time = request.Time;
            _context.SaveChanges();

            return Ok(appointment);
        }

        // ✅ DELETE: Remove an appointment
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteAppointment(int id)
        {
            int userId = GetUserIdFromToken();
            var appointment = _context.Schedules.FirstOrDefault(s => s.Id == id && s.UserId == userId);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            _context.Schedules.Remove(appointment);
            _context.SaveChanges();

            return Ok(new { message = "Appointment canceled." });
        }

        // ✅ Get UserId from JWT Token
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        // ✅ DTOs
        public class CreateAppointmentRequest
        {
            public string Time { get; set; }
        }

        public class EditAppointmentRequest
        {
            public string Time { get; set; }
        }
    }

}
