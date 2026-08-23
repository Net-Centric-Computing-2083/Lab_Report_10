using Microsoft.Data.SqlClient;

namespace ADO_NET_CRUD
{
    class Program
    {
        // Connection string for SQL Server LocalDB
        private static readonly string connectionString =
            @"Server=(localdb)\MSSQLLocalDB;Database=StudentDB;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("======================================");
                Console.WriteLine("       STUDENT MANAGEMENT SYSTEM      ");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. View Students");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Exit");
                Console.WriteLine("======================================");
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
                        Console.WriteLine("\nProgram exited successfully.");
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        Pause();
                        break;
                }
            }
        }

        // CREATE operation
        static void AddStudent()
        {
            Console.Clear();

            Console.WriteLine("========== ADD STUDENT ==========");

            Console.Write("Enter student name: ");
            string name = Console.ReadLine();

            int age;

            while (true)
            {
                Console.Write("Enter student age: ");

                if (int.TryParse(Console.ReadLine(), out age) && age > 0)
                {
                    break;
                }

                Console.WriteLine("Please enter a valid age.");
            }

            Console.Write("Enter student course: ");
            string course = Console.ReadLine();

            string query =
                "INSERT INTO Students (Name, Age, Course) " +
                "VALUES (@Name, @Age, @Course)";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Age", age);
                        command.Parameters.AddWithValue("@Course", course);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("\nStudent added successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }

            Pause();
        }

        // READ operation
        static void ViewStudents()
        {
            Console.Clear();

            Console.WriteLine("========== STUDENT LIST ==========");

            string query =
                "SELECT Id, Name, Age, Course FROM Students ORDER BY Id";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader =
                            command.ExecuteReader())
                        {
                            bool hasRecords = false;

                            Console.WriteLine();
                            Console.WriteLine(
                                "{0,-5} {1,-25} {2,-10} {3,-20}",
                                "ID", "Name", "Age", "Course");

                            Console.WriteLine(
                                new string('-', 65));

                            while (reader.Read())
                            {
                                hasRecords = true;

                                Console.WriteLine(
                                    "{0,-5} {1,-25} {2,-10} {3,-20}",
                                    reader["Id"],
                                    reader["Name"],
                                    reader["Age"],
                                    reader["Course"]);
                            }

                            if (!hasRecords)
                            {
                                Console.WriteLine("No student records found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }

            Pause();
        }

        // UPDATE operation
        static void UpdateStudent()
        {
            Console.Clear();

            Console.WriteLine("========== UPDATE STUDENT ==========");

            int id;

            while (true)
            {
                Console.Write("Enter student ID to update: ");

                if (int.TryParse(Console.ReadLine(), out id) && id > 0)
                {
                    break;
                }

                Console.WriteLine("Please enter a valid student ID.");
            }

            Console.Write("Enter new student name: ");
            string name = Console.ReadLine();

            int age;

            while (true)
            {
                Console.Write("Enter new student age: ");

                if (int.TryParse(Console.ReadLine(), out age) && age > 0)
                {
                    break;
                }

                Console.WriteLine("Please enter a valid age.");
            }

            Console.Write("Enter new student course: ");
            string course = Console.ReadLine();

            string query =
                "UPDATE Students " +
                "SET Name = @Name, Age = @Age, Course = @Course " +
                "WHERE Id = @Id";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Age", age);
                        command.Parameters.AddWithValue("@Course", course);
                        command.Parameters.AddWithValue("@Id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine(
                                "\nStudent updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine(
                                "\nStudent with the specified ID was not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }

            Pause();
        }

        // DELETE operation
        static void DeleteStudent()
        {
            Console.Clear();

            Console.WriteLine("========== DELETE STUDENT ==========");

            int id;

            while (true)
            {
                Console.Write("Enter student ID to delete: ");

                if (int.TryParse(Console.ReadLine(), out id) && id > 0)
                {
                    break;
                }

                Console.WriteLine("Please enter a valid student ID.");
            }

            string query =
                "DELETE FROM Students WHERE Id = @Id";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine(
                                "\nStudent deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine(
                                "\nStudent with the specified ID was not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }

            Pause();
        }

        // Pause the console
        static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}