using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString =
        "Server=localhost;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n--------------------------------");
            Console.WriteLine("       STUDENT MANAGEMENT");
            Console.WriteLine("\n--------------------------------");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Update Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Exit");
            Console.WriteLine("\n--------------------------------");

            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine()!;

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
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    // CREATE
    static void AddStudent()
    {
        Console.Write("Enter Student Name: ");
        string name = Console.ReadLine()!;

        Console.Write("Enter Email: ");
        string email = Console.ReadLine()!;

        Console.Write("Enter Age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Contact No: ");
        string contactNo = Console.ReadLine()!;

        string query = @"INSERT INTO Students
                         (Name, Email, Age, ContactNo)
                         VALUES
                         (@Name, @Email, @Age, @ContactNo)";

        using SqlConnection connection =
            new SqlConnection(connectionString);

        using SqlCommand command =
            new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Age", age);
        command.Parameters.AddWithValue("@ContactNo", contactNo);

        connection.Open();

        int rows = command.ExecuteNonQuery();

        if (rows > 0)
        {
            Console.WriteLine("\nStudent added successfully!");
        }
    }

    // READ
    static void ViewStudents()
    {
        string query = "SELECT * FROM Students";

        using SqlConnection connection =
            new SqlConnection(connectionString);

        using SqlCommand command =
            new SqlCommand(query, connection);

        connection.Open();

        using SqlDataReader reader =
            command.ExecuteReader();

        Console.WriteLine("\n---------- STUDENT LIST ----------");

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
                $"Age: {reader["Age"]} | " +
                $"Contact: {reader["ContactNo"]}");
        }

        Console.WriteLine("\n--------------------------------");
    }

    // UPDATE
    static void UpdateStudent()
    {
        Console.Write("Enter Student ID to update: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter New Name: ");
        string name = Console.ReadLine()!;

        Console.Write("Enter New Email: ");
        string email = Console.ReadLine()!;

        Console.Write("Enter New Age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter New Contact No: ");
        string contactNo = Console.ReadLine()!;

        string query = @"UPDATE Students
                         SET Name = @Name,
                             Email = @Email,
                             Age = @Age,
                             ContactNo = @ContactNo
                         WHERE Id = @Id";

        using SqlConnection connection =
            new SqlConnection(connectionString);

        using SqlCommand command =
            new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Age", age);
        command.Parameters.AddWithValue("@ContactNo", contactNo);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();

        int rows = command.ExecuteNonQuery();

        if (rows > 0)
        {
            Console.WriteLine("\nStudent updated successfully!");
        }
        else
        {
            Console.WriteLine("\nStudent ID not found.");
        }
    }

    // DELETE
    static void DeleteStudent()
    {
        Console.Write("Enter Student ID to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());

        string query =
            "DELETE FROM Students WHERE Id = @Id";

        using SqlConnection connection =
            new SqlConnection(connectionString);

        using SqlCommand command =
            new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);

        connection.Open();

        int rows = command.ExecuteNonQuery();

        if (rows > 0)
        {
            Console.WriteLine("\nStudent deleted successfully!");
        }
        else
        {
            Console.WriteLine("\nStudent ID not found.");
        }
    }
}