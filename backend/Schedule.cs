public class Schedule
{
    public int Id { get; set; }
    public int UserId { get; set; } // ✅ Foreign key to User
    public string Time { get; set; } = string.Empty; // ✅ Store appointment time
    public User User { get; set; }  // ✅ Navigation property
}
