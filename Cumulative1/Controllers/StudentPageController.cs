using Cumulative1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative1.Controllers
{
    public class StudentPageController : Controller
    {
        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }
        public IActionResult List()
        {
            List<Student> Students = _api.ListStudent();
            return View(Students);
        }
        public IActionResult Show(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Student NewStudent)
        {
            int Id = _api.AddStudent(NewStudent);


            return RedirectToAction("Show", new { id = Id });
        }


        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            int TeacherId = _api.DeleteStudent(id);
            // redirects to list action
            return RedirectToAction("List");
        }
        //Edit 
        public IActionResult Edit(int id)
        {
            // Find the selected student by ID
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);  // Pass the student data to the Edit view
        }

        // POST method to handle the updated student data
        [HttpPost]
        public IActionResult Update(int id, string SFName, string SLName, DateTime EnrollDate, string SNumber)
        {
            // Create a new Student object to store the updated values
            Student UpdatedStudent = new Student();
            UpdatedStudent.SFName = SFName;
            UpdatedStudent.SLName = SLName;
            UpdatedStudent.EnrollDate = EnrollDate;
            UpdatedStudent.SNumber = SNumber;

            // Call the API to update the student in the database
            _api.UpdateStudent(id, UpdatedStudent);

            // Redirect to the Show page to display the updated student details
            return RedirectToAction("Show", new { id = id });
        }


    }
}