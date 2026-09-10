using Microsoft.Data.SqlClient;

class Program
{
    // SQL Server connection string
    static string connectionString =
        "Server=localhost;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

    static void Main()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("======================================");
            Console.WriteLine("       STUDENT MANAGEMENT SYSTEM");
            Console.WriteLine("======================================");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Update Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Exit");
            Console.WriteLine("======================================");
            Console.Write("Enter choice: ");

            string? choice = Console.ReadLine();

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
                    Console.WriteLine("\nThank you!");
                    return;

                default:
                    Console.WriteLine("\nInvalid choice!");
                    Pause();
                    break;
            }
        }
    }

    // CREATE / INSERT
    static void AddStudent()
    {
        Console.Clear();
        Console.WriteLine("========== ADD STUDENT ==========\n");

        Console.Write("Enter Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Enter Age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Phone: ");
        string phone = Console.ReadLine() ?? "";

        Console.Write("Enter Course: ");
        string course = Console.ReadLine() ?? "";

        Console.Write("Enter Batch: ");
        string batch = Console.ReadLine() ?? "";

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = """
                INSERT INTO Students (Name, Age, Phone, Course, Batch)
                VALUES (@Name, @Age, @Phone, @Course, @Batch)
                """;

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Age", age);
            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Course", course);
            command.Parameters.AddWithValue("@Batch", batch);

            int rows = command.ExecuteNonQuery();

            if (rows > 0)
                Console.WriteLine("\nStudent added successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nError: " + ex.Message);
        }

        Pause();
    }

    // READ / SELECT
    static void ViewStudents()
    {
        Console.Clear();
        Console.WriteLine("========== STUDENT RECORDS ==========\n");

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = "SELECT Id, Name, Age, Phone, Course, Batch FROM Students";

            using SqlCommand command = new SqlCommand(query, connection);

            using SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine(
                "{0,-5} {1,-15} {2,-5} {3,-15} {4,-15} {5,-10}",
                "ID", "Name", "Age", "Phone", "Course", "Batch");

            Console.WriteLine(new string('-', 70));

            while (reader.Read())
            {
                Console.WriteLine(
                    "{0,-5} {1,-15} {2,-5} {3,-15} {4,-15} {5,-10}",
                    reader["Id"],
                    reader["Name"],
                    reader["Age"],
                    reader["Phone"],
                    reader["Course"],
                    reader["Batch"]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nError: " + ex.Message);
        }

        Pause();
    }

    // UPDATE
    static void UpdateStudent()
    {
        Console.Clear();
        Console.WriteLine("========== UPDATE STUDENT ==========\n");

        Console.Write("Enter Student ID: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter New Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Enter New Age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter New Phone: ");
        string phone = Console.ReadLine() ?? "";

        Console.Write("Enter New Course: ");
        string course = Console.ReadLine() ?? "";

        Console.Write("Enter New Batch: ");
        string batch = Console.ReadLine() ?? "";

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = """
                UPDATE Students
                SET Name = @Name,
                    Age = @Age,
                    Phone = @Phone,
                    Course = @Course,
                    Batch = @Batch
                WHERE Id = @Id
                """;

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Age", age);
            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Course", course);
            command.Parameters.AddWithValue("@Batch", batch);

            int rows = command.ExecuteNonQuery();

            if (rows > 0)
                Console.WriteLine("\nStudent updated successfully!");
            else
                Console.WriteLine("\nStudent ID not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nError: " + ex.Message);
        }

        Pause();
    }

    // DELETE
    static void DeleteStudent()
    {
        Console.Clear();
        Console.WriteLine("========== DELETE STUDENT ==========\n");

        Console.Write("Enter Student ID: ");
        int id = Convert.ToInt32(Console.ReadLine());

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = "DELETE FROM Students WHERE Id = @Id";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);

            int rows = command.ExecuteNonQuery();

            if (rows > 0)
                Console.WriteLine("\nStudent deleted successfully!");
            else
                Console.WriteLine("\nStudent ID not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nError: " + ex.Message);
        }

        Pause();
    }

    static void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}