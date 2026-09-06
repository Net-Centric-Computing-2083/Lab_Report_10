using System;
using Microsoft.Data.SqlClient;

class Program
{
    // Database connection string
    static string connectionString =
        "Server=AAKRITI-PC;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

   
    // CREATE - Add Student
   
    static void AddStudent()
    {
        Console.WriteLine("\n--Add Student--");

        Console.Write("Enter Student Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Email: ");
        string email = Console.ReadLine();

        Console.Write("Enter Age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Course: ");
        string course = Console.ReadLine();

        string query =
            "INSERT INTO Students (Name, Email, Age, Course) " +
            "VALUES (@Name, @Email, @Age, @Course)";

        try
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Age", age);
                    command.Parameters.AddWithValue("@Course", course);

                    connection.Open();

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("\nStudent added successfully!");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nError: " + ex.Message);
        }
    }


    // ==============================
    // READ - View Students
    // ==============================
    static void ViewStudents()
    {
        Console.WriteLine("\n---Student Records---");

        string query = "SELECT * FROM Students";

        try
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No student records found.");
                            return;
                        }

                        Console.WriteLine(
                            "\nID\tName\t\tEmail\t\t\tAge\tCourse");

                        Console.WriteLine(
                            "----------------");

                        while (reader.Read())
                        {
                            Console.WriteLine(
                                $"{reader["Id"]}\t" +
                                $"{reader["Name"]}\t\t" +
                                $"{reader["Email"]}\t\t" +
                                $"{reader["Age"]}\t" +
                                $"{reader["Course"]}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nError: " + ex.Message);
        }
    }


    // ==============================
    // UPDATE - Update Student
    // ==============================
    static void UpdateStudent()
    {
        Console.WriteLine("\n===== Update Student =====");

        Console.Write("Enter Student ID to update: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter New Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter New Email: ");
        string email = Console.ReadLine();

        Console.Write("Enter New Age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter New Course: ");
        string course = Console.ReadLine();

        string query =
            "UPDATE Students " +
            "SET Name = @Name, Email = @Email, Age = @Age, Course = @Course " +
            "WHERE Id = @Id";

        try
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Age", age);
                    command.Parameters.AddWithValue("@Course", course);

                    connection.Open();

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine(
                            "\nStudent updated successfully!");
                    }
                    else
                    {
                        Console.WriteLine(
                            "\nStudent ID not found.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nError: " + ex.Message);
        }
    }


    // ==============================
    // DELETE - Delete Student
    // ==============================
    static void DeleteStudent()
    {
        Console.WriteLine("\n===== Delete Student =====");

        Console.Write("Enter Student ID to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());

        string query =
            "DELETE FROM Students WHERE Id = @Id";

        try
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine(
                            "\nStudent deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine(
                            "\nStudent ID not found.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nError: " + ex.Message);
        }
    }


    // ==============================
    // MAIN MENU
    // ==============================
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n======");
            Console.WriteLine("     STUDENT MANAGEMENT SYSTEM");
            Console.WriteLine("=======");

            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Update Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Exit");

            Console.Write("\nEnter your choice: ");

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
                        Console.WriteLine(
                            "\nProgram exited successfully.");
                        return;

                    default:
                        Console.WriteLine(
                            "\nInvalid choice. Please try again.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "\nInvalid input. Please enter valid data.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}