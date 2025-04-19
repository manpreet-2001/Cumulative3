using Cumulative1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative1.Controllers
{
    public class CoursePageController : Controller
    {
        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        public IActionResult List()
        {
            List<Course> Courses = _api.ListCourse();
            return View(Courses);
        }
        public IActionResult Show(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Course NewCourse)
        {
            int Id = _api.AddCourse(NewCourse);


            return RedirectToAction("Show", new { id = Id });
        }

        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            int CId = _api.DeleteCourse(id);
            // redirects to list action
            return RedirectToAction("List");
        }
        // Edit a course (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            if (SelectedCourse == null)
            {
                return NotFound();
            }
            return View(SelectedCourse);
        }

        // Update course details (POST)
        [HttpPost]
        public IActionResult Update(int id, Course updatedCourse)
        {
            if (id != updatedCourse.CId)
            {
                return BadRequest();
            }

            _api.UpdateCourse(id, updatedCourse);
            return RedirectToAction("Show", new { id = updatedCourse.CId });
        }
    }
}
