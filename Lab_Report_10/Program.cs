using Microsoft.Data.SqlClient;
using System.Data;

class Program
{
    // SQL Server default instance
    // Your SQL Server service is MSSQLSERVER,
    // so localhost is the correct server name.

    // Connection to master database
    // Used to create StudentDB if it does not exist.
    static string masterConnectionString =
        @"Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

    // Connection to StudentDB
    static string connectionString =
        @"Server=localhost;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";


    // =========================================================
    // CREATE DATABASE AND TABLE
    // =========================================================
    static void InitializeDatabase()
    {
        try
        {
            // Connect to the master database
            using (SqlConnection con = new SqlConnection(masterConnectionString))
            {
                con.Open();

                string createDatabaseQuery = @"
                    IF NOT EXISTS
                    (
                        SELECT name
                        FROM sys.databases
                        WHERE name = 'StudentDB'
                    )
                    BEGIN
                        CREATE DATABASE StudentDB;
                    END";

                using (SqlCommand cmd = new SqlCommand(createDatabaseQuery, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Database checked/created successfully.");

            // Connect to StudentDB
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string createTableQuery = @"
                    IF NOT EXISTS
                    (
                        SELECT *
                        FROM INFORMATION_SCHEMA.TABLES
                        WHERE TABLE_NAME = 'Students'
                    )
                    BEGIN
                        CREATE TABLE Students
                        (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name VARCHAR(100) NOT NULL,
                            Email VARCHAR(150) NOT NULL,
                            Age INT NOT NULL
                        );
                    END";

                using (SqlCommand cmd = new SqlCommand(createTableQuery, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Students table checked/created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("DATABASE INITIALIZATION ERROR");
            Console.WriteLine("======================================");
            Console.WriteLine(ex.Message);
            Console.WriteLine();
            Console.WriteLine("Make sure SQL Server is running.");
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            Environment.Exit(1);
        }
    }


    // =========================================================
    // ADD STUDENT
    // =========================================================
    static void AddStudent()
    {
        try
        {
            Console.WriteLine();
            Console.WriteLine("========== ADD STUDENT ==========");

            Console.Write("Enter Name: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty.");
                return;
            }

            Console.Write("Enter Email: ");
            string? email = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Email cannot be empty.");
                return;
            }

            Console.Write("Enter Age: ");

            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Please enter a valid age.");
                return;
            }

            if (age <= 0 || age > 150)
            {
                Console.WriteLine("Please enter a valid age between 1 and 150.");
                return;
            }

            string query = @"
                INSERT INTO Students (Name, Email, Age)
                VALUES (@Name, @Email, @Age)";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = name;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 150).Value = email;
                cmd.Parameters.Add("@Age", SqlDbType.Int).Value = age;

                con.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Student added successfully!");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Student was not added.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error while adding student:");
            Console.WriteLine(ex.Message);
        }
    }


    // =========================================================
    // VIEW STUDENTS
    // =========================================================
    static void ViewStudents()
    {
        try
        {
            Console.WriteLine();
            Console.WriteLine("========== ALL STUDENTS ==========");

            string query = @"
                SELECT Id, Name, Email, Age
                FROM Students
                ORDER BY Id";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
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
            }

            Console.WriteLine("=================================");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error while viewing students:");
            Console.WriteLine(ex.Message);
        }
    }


    // =========================================================
    // UPDATE STUDENT
    // =========================================================
    static void UpdateStudent()
    {
        try
        {
            Console.WriteLine();
            Console.WriteLine("========== UPDATE STUDENT ==========");

            Console.Write("Enter Student ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Please enter a valid ID.");
                return;
            }

            Console.Write("Enter New Name: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty.");
                return;
            }

            string query = @"
                UPDATE Students
                SET Name = @Name
                WHERE Id = @Id";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = name;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                con.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Student updated successfully!");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Student with that ID was not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error while updating student:");
            Console.WriteLine(ex.Message);
        }
    }


    // =========================================================
    // DELETE STUDENT
    // =========================================================
    static void DeleteStudent()
    {
        try
        {
            Console.WriteLine();
            Console.WriteLine("========== DELETE STUDENT ==========");

            Console.Write("Enter Student ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Please enter a valid ID.");
                return;
            }

            string query = @"
                DELETE FROM Students
                WHERE Id = @Id";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                con.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Student deleted successfully!");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Student with that ID was not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("Error while deleting student:");
            Console.WriteLine(ex.Message);
        }
    }


    // =========================================================
    // MAIN MENU
    // =========================================================
    static void Main()
    {
        Console.WriteLine("======================================");
        Console.WriteLine("     STUDENT MANAGEMENT SYSTEM");
        Console.WriteLine("======================================");

        // Create database and table automatically
        InitializeDatabase();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("--- Student Management System ---");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Update Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Exit");

            Console.Write("Enter your choice: ");

            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Please enter a number from 1 to 5.");
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
                    Console.WriteLine();
                    Console.WriteLine("Thank you for using Student Management System.");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please select 1-5.");
                    break;
            }
        }
    }
}