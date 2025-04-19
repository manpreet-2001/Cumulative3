using System;

namespace Cumulative1.Models
{
    public class Course
    {
        public int CId { get; set; }
        public string? Ccode { get; set; }
        public int Tid { get; set; }
        public DateTime startdate { get; set; }
        public DateTime finishdate { get; set; }
        public string? Cname { get; set; }

        // Parameterless constructor
        public Course() { }
    }
}