using System;
using System.Linq;

namespace backend.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AppointmentService
    {
        private readonly DataContext _context;

        public AppointmentService(DataContext context)
        {
            _context = context;
        }

        public List<object> GetAllCustomers()
        {
            return _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    AppointmentTime = u.AppointmentTime ?? "No Appointment"
                })
                .ToList<object>();
        }

        public bool SetAppointmentTime(int userId, string appointmentTime, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    errorMessage = "User not found.";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(appointmentTime))
                {
                    errorMessage = "Invalid appointment time.";
                    return false;
                }

                user.AppointmentTime = appointmentTime;
                _context.Users.Update(user);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Database error: {ex.Message}";
                return false;
            }
        }

        public bool DeleteAppointment(int userId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    errorMessage = "User not found.";
                    return false;
                }

                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Database error: {ex.Message}";
                return false;
            }
        }

        public bool EditUserProfile(int userId, string newName, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    errorMessage = "User not found.";
                    return false;
                }

                user.Name = newName;
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Database error: {ex.Message}";
                return false;
            }
        }
    }

}
