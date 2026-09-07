using System;
using Microsoft.Data.SqlClient;

namespace AdoNetCRUD
{
    class Program
    {
        static string connectionString =
            @"Server=DESKTOP-KN3468D;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // CREATE / INSERT
        static void AddStudent()
        {
            Console.Write("Enter Student Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            string query = "INSERT INTO Students (Name, Age, Address) VALUES (@Name, @Age, @Address)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Age", age);
                    command.Parameters.AddWithValue("@Address", address);

                    connection.Open();

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Student added successfully.");
                    }
                }
            }
        }

        // READ / SELECT
        static void ViewStudents()
        {
            string query = "SELECT * FROM Students";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("\n--- Student Records ---");

                        while (reader.Read())
                        {
                            Console.WriteLine(
                                $"ID: {reader["Id"]}, " +
                                $"Name: {reader["Name"]}, " +
                                $"Age: {reader["Age"]}, " +
                                $"Address: {reader["Address"]}"
                            );
                        }
                    }
                }
            }
        }

        // UPDATE
        static void UpdateStudent()
        {
            Console.Write("Enter Student ID to update: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter New Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter New Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter New Address: ");
            string address = Console.ReadLine();

            string query =
                "UPDATE Students SET Name=@Name, Age=@Age, Address=@Address WHERE Id=@Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Age", age);
                    command.Parameters.AddWithValue("@Address", address);

                    connection.Open();

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Student updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Student not found.");
                    }
                }
            }
        }

        // DELETE
        static void DeleteStudent()
        {
            Console.Write("Enter Student ID to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            string query = "DELETE FROM Students WHERE Id=@Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Student deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Student not found.");
                    }
                }
            }
        }

        // MAIN MENU
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n==============================");
                Console.WriteLine("     STUDENT MANAGEMENT");
                Console.WriteLine("==============================");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. View Students");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Exit");
                Console.WriteLine("==============================");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                try
                {
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
                            Console.WriteLine("Exiting application...");
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}