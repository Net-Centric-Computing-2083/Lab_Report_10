using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString =
        @"Server=localhost;Database=STUDENT;Trusted_Connection=True;TrustServerCertificate=True;";

    static void Main()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine(" STUDENT MANAGEMENT SYSTEM ");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Update Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine()!;

            Console.Clear();

            switch (choice)
            {
                case "1":
                    AddStudent(connectionString);
                    break;

                case "2":
                    ViewStudents(connectionString);
                    break;

                case "3":
                    UpdateStudent(connectionString);
                    break;

                case "4":
                    DeleteStudent(connectionString);
                    break;

                case "5":
                    Console.WriteLine("Exiting application...");
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    static void AddStudent(string connectionString)
    {
        Console.Write("Enter student name: ");
        string name = Console.ReadLine()!;

        Console.Write("Enter age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter address: ");
        string address = Console.ReadLine()!;

        string query = @"INSERT INTO Students (Name, Age, Address)
                         VALUES (@Name, @Age, @Address)";

        using SqlConnection connection = new SqlConnection(connectionString);
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Age", age);
        command.Parameters.AddWithValue("@Address", address);

        connection.Open();

        int rows = command.ExecuteNonQuery();

        Console.WriteLine($"{rows} student record inserted successfully.");
    }

    static void ViewStudents(string connectionString)
    {
        string query = "SELECT Id, Name, Age, Address FROM Students";

        using SqlConnection connection = new SqlConnection(connectionString);
        using SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        Console.WriteLine("--- Student Records ---");

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

    static void UpdateStudent(string connectionString)
    {
        Console.Write("Enter student ID to update: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter new name: ");
        string name = Console.ReadLine()!;

        Console.Write("Enter new age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter new address: ");
        string address = Console.ReadLine()!;

        string query = @"UPDATE Students
                         SET Name = @Name,
                             Age = @Age,
                             Address = @Address
                         WHERE Id = @Id";

        using SqlConnection connection = new SqlConnection(connectionString);
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Age", age);
        command.Parameters.AddWithValue("@Address", address);

        connection.Open();

        int rows = command.ExecuteNonQuery();

        if (rows > 0)
            Console.WriteLine("Student updated successfully.");
        else
            Console.WriteLine("Student not found.");
    }

    static void DeleteStudent(string connectionString)
    {
        Console.Write("Enter student ID to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());

        string query = "DELETE FROM Students WHERE Id = @Id";

        using SqlConnection connection = new SqlConnection(connectionString);
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);

        connection.Open();

        int rows = command.ExecuteNonQuery();

        if (rows > 0)
            Console.WriteLine("Student deleted successfully.");
        else
            Console.WriteLine("Student not found.");
    }
}