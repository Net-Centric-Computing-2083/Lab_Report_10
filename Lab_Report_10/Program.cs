using System;
using Microsoft.Data.SqlClient;

namespace Lab_Report_10
{
    class Program
    {
        static string connectionString =
            "Server=LAPTOP-O0Q5AS3J;Database=LabReport10DB;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Student Management Menu =====");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. View Students");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        ViewStudents();
                        break;
                    case "3":
                        UpdateStudent();
                        break;
                    case "4":
                        DeleteStudent();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AddStudent()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Age: ");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Course: ");
            string course = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Students (Name, Email, Age, Course) VALUES (@Name, @Email, @Age, @Course)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Course", course);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Student added successfully." : "Failed to add student.");
            }
        }

        static void ViewStudents()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Students";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("\nId\tName\tEmail\tAge\tCourse");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Id"]}\t{reader["Name"]}\t{reader["Email"]}\t{reader["Age"]}\t{reader["Course"]}");
                }
                reader.Close();
            }
        }

        static void UpdateStudent()
        {
            Console.Write("Enter Id of student to update: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter new Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter new Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter new Age: ");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter new Course: ");
            string course = Console.ReadLine();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Students SET Name=@Name, Email=@Email, Age=@Age, Course=@Course WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Course", course);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Student updated successfully." : "No student found with that Id.");
            }
        }

        static void DeleteStudent()
        {
            Console.Write("Enter Id of student to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Students WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Student deleted successfully." : "No student found with that Id.");
            }
        }
    }
}