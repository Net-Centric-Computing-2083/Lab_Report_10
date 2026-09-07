using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

    // Create/Insert
    static void AddStudent()
    {
        using SqlConnection con = new SqlConnection(connectionString);

        Console.Write("Enter Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Email: ");
        string email = Console.ReadLine();

        Console.Write("Enter Age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        string query =
            "INSERT INTO Students (Name, Email, Age) VALUES (@Name, @Email, @Age)";

        using SqlCommand cmd = new SqlCommand(query, con);

        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@Age", age);

        con.Open();
        cmd.ExecuteNonQuery();

        Console.WriteLine("Student added successfully.");
    }

    // Read
    static void ViewStudents()
    {
        using SqlConnection con = new SqlConnection(connectionString);

        string query = "SELECT * FROM Students";

        using SqlCommand cmd = new SqlCommand(query, con);

        con.Open();

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine(
                $"ID: {reader["Id"]}, " +
                $"Name: {reader["Name"]}, " +
                $"Email: {reader["Email"]}, " +
                $"Age: {reader["Age"]}");
        }
    }

    // Update
    static void UpdateStudent()
    {
        using SqlConnection con = new SqlConnection(connectionString);

        Console.Write("Enter Student ID: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter New Name: ");
        string name = Console.ReadLine();

        string query = "UPDATE Students SET Name=@Name WHERE Id=@Id";

        using SqlCommand cmd = new SqlCommand(query, con);

        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Id", id);

        con.Open();
        cmd.ExecuteNonQuery();

        Console.WriteLine("Student updated successfully.");
    }

    // Delete
    static void DeleteStudent()
    {
        using SqlConnection con = new SqlConnection(connectionString);

        Console.Write("Enter Student ID: ");
        int id = Convert.ToInt32(Console.ReadLine());

        string query = "DELETE FROM Students WHERE Id=@Id";

        using SqlCommand cmd = new SqlCommand(query, con);

        cmd.Parameters.AddWithValue("@Id", id);

        con.Open();
        cmd.ExecuteNonQuery();

        Console.WriteLine("Student deleted successfully.");
    }

    // Main Menu
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n--- Student Management System ---");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Update Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Exit");

            Console.Write("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddStudent();
                    break;

                case 2:
                    ViewStudents();
                    break;

                case 3:
                    UpdateStudent();
                    break;

                case 4:
                    DeleteStudent();
                    break;

                case 5:
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}