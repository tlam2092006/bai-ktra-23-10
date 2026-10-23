namespace hoccsharp.Models
{
    public class EmployeeModel
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string AvatarPath { get; set; } = string.Empty;
    }
}