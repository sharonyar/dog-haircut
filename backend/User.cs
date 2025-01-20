using System;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string AppointmentTime { get; set; } = GenerateRandomTime(); // ✅ Set random time by default

    private static string GenerateRandomTime()
    {
        Random rand = new Random();
        int hour = rand.Next(8, 18); // ✅ Random hour between 08:00 and 17:59
        int minute = rand.Next(0, 60); // ✅ Random minutes between 00 and 59
        return $"{hour:D2}:{minute:D2}"; // ✅ Format: HH:mm (e.g., "14:35")
    }
}
