using Cumulative1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Mysqlx.Notice;
using MySqlX.XDevAPI.Common;
using System.Xml.Linq;

namespace Cumulative1.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly SchooldbContext _context;
        public StudentAPIController(SchooldbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(template: "listStudents")]
        public List<Student> ListStudent()
        {
            List<Student> Students = new List<Student>();
            using (MySqlConnection Connect = _context.AccessDatabase())
            {
                Connect.Open();
                MySqlCommand Command = Connect.CreateCommand();
                Command.CommandText = "Select * from students";
                Command.Prepare();
                using (MySqlDataReader Result = Command.ExecuteReader())
                {
                    while (Result.Read())
                    {
                        int Id = Convert.ToInt32(Result["studentid"]);
                        string SFName = Result["studentfname"].ToString();
                        string SLName = Result["studentlname"].ToString();
                        string SNumber = Result["studentnumber"].ToString();
                        DateTime EnrollDate = Convert.ToDateTime(Result["enrolldate"]);
                        Student CurrentStudent = new Student()
                        {
                            Id = Id,
                            SFName = SFName,
                            SLName = SLName,
                            EnrollDate = EnrollDate,
                            SNumber = SNumber

                        };

                        Students.Add(CurrentStudent);

                    }
                }

            }
            return Students;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {

            Student SelectedStudents = new Student();

            using (MySqlConnection Connect = _context.AccessDatabase())
            {
                Connect.Open();
                MySqlCommand Command = Connect.CreateCommand();
                Command.CommandText = "Select * from students WHERE studentid = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader Result = Command.ExecuteReader())
                {
                    while (Result.Read())
                    {
                        int Id = Convert.ToInt32(Result["studentid"]);
                        string SFName = Result["studentfname"].ToString();
                        string SLName = Result["studentlname"].ToString();
                        string SNumber = Result["studentnumber"].ToString();
                        DateTime EnrollDate = Convert.ToDateTime(Result["enrolldate"]);


                        SelectedStudents.Id = Id;
                        SelectedStudents.SFName = SFName;
                        SelectedStudents.SLName = SLName;
                        SelectedStudents.EnrollDate = EnrollDate;
                        SelectedStudents.SNumber = SNumber;

                    }
                }
            }


            return SelectedStudents;
        }
        /// <summary>
        /// Adds a new student to the database.
        /// Example:
        /// POST api/Student/AddStudent
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///   "SFName": "Alice",
        ///   "SLName": "Smith",
        ///   "SNumber": "67890",
        ///   "EnrollDate": "2024-12-10"
        /// }
        /// Response:
        /// 20 (ID of the newly inserted student)
        /// </summary>
        /// <param name="StudentData">An object containing student details (first name, last name, student number, and enrollment date).</param>
        /// <returns>The ID of the newly inserted student or 0 if the operation fails.</returns>
        [HttpPost(template: "AddStudent")]
        public int AddStudent([FromBody] Student StudentData)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                // Use the correct column name
                Command.CommandText = "INSERT INTO students (studentfname, studentlname, studentnumber, enrolldate) VALUES (@SFName, @SLName, @SNumber, @EnrollDate)";
                Command.Parameters.AddWithValue("@SFName", StudentData.SFName);
                Command.Parameters.AddWithValue("@SLName", StudentData.SLName);
                Command.Parameters.AddWithValue("@SNumber", StudentData.SNumber);
                Command.Parameters.AddWithValue("@EnrollDate", StudentData.EnrollDate);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);
            }
        }

        /// <summary>
        /// Deletes an existing student from the database by their ID.
        /// Example:
        /// DELETE api/Student/DeleteStudent/1
        /// ->
        /// 1 (number of rows affected, 1 if successful, 0 if no rows were deleted)
        /// </summary>
        /// <param name="Id">The ID of the student to delete.</param>
        /// <returns>The number of rows affected (1 if successful, 0 if the student does not exist).</returns>
        /// 
        [HttpDelete(template: "DeleteStudent/{Id}")]
        public int DeleteStudent(int Id)
        {
            Console.WriteLine($"Received ID for deletion: {Id}"); // Log the ID

            try
            {
                using (MySqlConnection Connection = _context.AccessDatabase())
                {
                    Connection.Open();
                    MySqlCommand Command = Connection.CreateCommand();

                    // Log the SQL command
                    Command.CommandText = "DELETE FROM students WHERE studentid = @id";
                    Command.Parameters.AddWithValue("@id", Id);
                    Console.WriteLine($"Executing SQL: {Command.CommandText} with ID: {Id}"); // Log the SQL command

                    // Execute the command and check the number of rows affected
                    int rowsAffected = Command.ExecuteNonQuery();
                    Console.WriteLine($"Rows affected: {rowsAffected}"); // Log the number of rows affected

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Student deleted successfully.");
                        return rowsAffected; // Return the number of rows affected
                    }
                    else
                    {
                        Console.WriteLine("No student found with the given ID. Deletion failed.");
                        return 0; // Return 0 if no rows were affected
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                // Log SQL-specific errors
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                Console.WriteLine($"SQL Error Code: {sqlEx.Number}");
                return 0; // Return 0 on failure
            }
            catch (Exception ex)
            {
                // Log general errors
                Console.WriteLine($"Error: {ex.Message}");
                return 0; // Return 0 on failure
            }
        }

        [HttpPut(template: "UpdateStudent/{StudentId}")]
        public Student UpdateStudent(int StudentId, [FromBody] Student StudentData)
        {

            using (MySqlConnection Connect = _context.AccessDatabase())
            {
                Connect.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connect.CreateCommand();

                // parameterize query
                Command.CommandText = "UPDATE students SET studentfname=@SFName, studentlname=@SLName,enrolldate=@EnrollDate, studentnumber=@SNumber WHERE studentid=@Id";
                Command.Parameters.AddWithValue("@SFName", StudentData.SFName);
                Command.Parameters.AddWithValue("@SLName", StudentData.SLName);
                Command.Parameters.AddWithValue("@EnrollDate", StudentData.EnrollDate);
                Command.Parameters.AddWithValue("@SNumber", StudentData.SNumber);

                // Add student ID to specify which student to update
                Command.Parameters.AddWithValue("@Id", StudentId);

                // Execute the query to update the student's details
                Command.ExecuteNonQuery();
            }

            // Return the updated student details
            return FindStudent(StudentId);
        }



    }
}