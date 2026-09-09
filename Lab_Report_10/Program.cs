
using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString =
    "Server=GAYATRI;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n===== Student Management System =====");
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
                    Console.WriteLine("Program exited.");
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
        string name = Console.ReadLine();

        Console.Write("Enter Age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Address: ");
        string address = Console.ReadLine();

        using SqlConnection con = new SqlConnection(connectionString);

        string query =
            "INSERT INTO Students (Name, Age, Address) VALUES (@Name, @Age, @Address)";

        using SqlCommand cmd = new SqlCommand(query, con);

        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Age", age);
        cmd.Parameters.AddWithValue("@Address", address);

        con.Open();

        int result = cmd.ExecuteNonQuery();

        if (result > 0)
            Console.WriteLine("Student added successfully!");
    }

    // READ
    static void ViewStudents()
    {
        using SqlConnection con = new SqlConnection(connectionString);

        string query = "SELECT * FROM Students";

        using SqlCommand cmd = new SqlCommand(query, con);

        con.Open();

        using SqlDataReader reader = cmd.ExecuteReader();

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

        using SqlConnection con = new SqlConnection(connectionString);

        string query =
            "UPDATE Students SET Name=@Name, Age=@Age, Address=@Address WHERE Id=@Id";

        using SqlCommand cmd = new SqlCommand(query, con);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Age", age);
        cmd.Parameters.AddWithValue("@Address", address);

        con.Open();

        int result = cmd.ExecuteNonQuery();

        if (result > 0)
            Console.WriteLine("Student updated successfully!");
        else
            Console.WriteLine("Student not found.");
    }

    // DELETE
    static void DeleteStudent()
    {
        Console.Write("Enter Student ID to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());

        using SqlConnection con = new SqlConnection(connectionString);

        string query = "DELETE FROM Students WHERE Id=@Id";

        using SqlCommand cmd = new SqlCommand(query, con);

        cmd.Parameters.AddWithValue("@Id", id);

        con.Open();

        int result = cmd.ExecuteNonQuery();

        if (result > 0)
            Console.WriteLine("Student deleted successfully!");
        else
            Console.WriteLine("Student not found.");
    }
}