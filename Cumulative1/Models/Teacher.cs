using Microsoft.AspNetCore.Mvc;

namespace Cumulative1.Models
{
    public class Teacher
    {

        public int TeacherId { get; set; }

        public string? TeacherFName { get; set; }

        public string? TeacherLName { get; set; }

        public DateTime Hire { get; set; }

        public Decimal Salary { get; set; }

        public string? EmployeeNumber { get; set; }

        public List<string> CourseNames { get; set; } = new List<string>();

    }
}