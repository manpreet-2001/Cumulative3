using Microsoft.AspNetCore.Mvc;

namespace Cumulative1.Models
{
    public class TeacherWithCoursesViewModel : Controller
    {
        public Teacher Teacher { get; set; }
        public List<string> Courses { get; set; }
    }
}