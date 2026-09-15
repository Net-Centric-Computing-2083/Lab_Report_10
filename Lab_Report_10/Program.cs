using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString =
        @"Server=DIWAKAR-PC;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

    static void Main()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("     STUDENT MANAGEMENT SYSTEM");
            Console.WriteLine("================================");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Update Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Exit");
            Console.WriteLine("================================");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine() ?? "";

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

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    // CREATE
    static void AddStudent()
    {
        Console.Write("\nEnter student name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Enter email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Enter age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        string query =
            "INSERT INTO Students (Name, Email, Age) " +
            "VALUES (@Name, @Email, @Age)";

        using SqlConnection connection =
            new SqlConnection(connectionString);

        using SqlCommand command =
            new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Age", age);

        connection.Open();

        int result = command.ExecuteNonQuery();

        if (result > 0)
        {
            Console.WriteLine("\nStudent added successfully.");
        }
    }

    // READ
    static void ViewStudents()
    {
        string query = "SELECT Id, Name, Email, Age FROM Students";

        using SqlConnection connection =
            new SqlConnection(connectionString);

        using SqlCommand command =
            new SqlCommand(query, connection);

        connection.Open();

        using SqlDataReader reader =
            command.ExecuteReader();

        Console.WriteLine("\n================================");
        Console.WriteLine("          STUDENT LIST");
        Console.WriteLine("================================");

        if (!reader.HasRows)
        {
            Console.WriteLine("No students found.");
            return;
        }

        while (reader.Read())
        {
            Console.WriteLine(
                $"ID: {reader["Id"]} | " +
                $"Name: {reader["Name"]} | " +
                $"Email: {reader["Email"]} | " +
                $"Age: {reader["Age"]}"
            );
        }
    }

    // UPDATE
    static void UpdateStudent()
    {
        Console.Write("\nEnter student ID to update: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter new name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Enter new email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Enter new age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        string query =
            "UPDATE Students " +
            "SET Name=@Name, Email=@Email, Age=@Age " +
            "WHERE Id=@Id";

        using SqlConnection connection =
            new SqlConnection(connectionString);

        using SqlCommand command =
            new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Age", age);

        connection.Open();

        int result = command.ExecuteNonQuery();

        if (result > 0)
        {
            Console.WriteLine("\nStudent updated successfully.");
        }
        else
        {
            Console.WriteLine("\nStudent not found.");
        }
    }

    // DELETE
    static void DeleteStudent()
    {
        Console.Write("\nEnter student ID to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());

        string query =
            "DELETE FROM Students WHERE Id=@Id";

        using SqlConnection connection =
            new SqlConnection(connectionString);

        using SqlCommand command =
            new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);

        connection.Open();

        int result = command.ExecuteNonQuery();

        if (result > 0)
        {
            Console.WriteLine("\nStudent deleted successfully.");
        }
        else
        {
            Console.WriteLine("\nStudent not found.");
        }
    }
}
