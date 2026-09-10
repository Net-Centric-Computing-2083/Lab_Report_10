using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString =
        "Server=SAURAVBASNET\\MSSQLSERVER01;Database=StudentManagementSytem;Trusted_Connection=True;TrustServerCertificate=True;";

    // CREATE / INSERT
    static void AddStudent()
    {
        using SqlConnection con = new SqlConnection(connectionString);

        Console.Write("Enter Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Enter Roll: ");
        string roll = Console.ReadLine() ?? "";

        Console.Write("Enter Faculty: ");
        string faculty = Console.ReadLine() ?? "";

        string query =
            "INSERT INTO StudentInfo (name, Roll, Faculty) " +
            "VALUES (@name, @Roll, @Faculty)";

        using SqlCommand cmd = new SqlCommand(query, con);

        cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar, 100).Value = name;
        cmd.Parameters.Add("@Roll", System.Data.SqlDbType.VarChar, 50).Value = roll;
        cmd.Parameters.Add("@Faculty", System.Data.SqlDbType.VarChar, 100).Value = faculty;

        con.Open();

        cmd.ExecuteNonQuery();

        Console.WriteLine("Student added successfully.");
    }


    // READ
    static void ViewStudents()
    {
        using SqlConnection con = new SqlConnection(connectionString);

        string query =
            "SELECT id, name, Roll, Faculty FROM StudentInfo";

        using SqlCommand cmd = new SqlCommand(query, con);

        con.Open();

        using SqlDataReader reader = cmd.ExecuteReader();

        Console.WriteLine("\n--- Student List ---");

        if (!reader.HasRows)
        {
            Console.WriteLine("No students found.");
            return;
        }

        while (reader.Read())
        {
            Console.WriteLine(
                $"ID: {reader["id"]}, " +
                $"Name: {reader["name"]}, " +
                $"Roll: {reader["Roll"]}, " +
                $"Faculty: {reader["Faculty"]}"
            );
        }
    }


    // UPDATE
    static void UpdateStudent()
    {
        using SqlConnection con = new SqlConnection(connectionString);

        Console.Write("Enter Student ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        Console.Write("Enter New Name: ");
        string name = Console.ReadLine() ?? "";

        string query =
            "UPDATE StudentInfo SET name = @name WHERE id = @id";

        using SqlCommand cmd = new SqlCommand(query, con);

        cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar, 100).Value = name;
        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

        con.Open();

        int rowsAffected = cmd.ExecuteNonQuery();

        if (rowsAffected > 0)
        {
            Console.WriteLine("Student updated successfully.");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }


    // DELETE
    static void DeleteStudent()
    {
        using SqlConnection con = new SqlConnection(connectionString);

        Console.Write("Enter Student ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        string query =
            "DELETE FROM StudentInfo WHERE id = @id";

        using SqlCommand cmd = new SqlCommand(query, con);

        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

        con.Open();

        int rowsAffected = cmd.ExecuteNonQuery();

        if (rowsAffected > 0)
        {
            Console.WriteLine("Student deleted successfully.");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }


    // MAIN MENU
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

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Please enter a valid number.");
                continue;
            }

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
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}