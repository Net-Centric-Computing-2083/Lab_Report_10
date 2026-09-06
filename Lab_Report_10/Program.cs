using Microsoft.Data.SqlClient;

namespace Lab10_ADO_NET_CRUD
{
    class Program
    {
        // Change this connection string according to your SQL Server
        static string connectionString =
            "Server=PRESHU;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("====================================");
                Console.WriteLine("       STUDENT MANAGEMENT SYSTEM");
                Console.WriteLine("       ADO.NET CRUD APPLICATION");
                Console.WriteLine("====================================");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. View Students");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Exit");
                Console.WriteLine("====================================");

                Console.Write("Enter your choice: ");
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
                        Console.WriteLine("\nThank you for using the application.");
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice!");
                        Pause();
                        break;
                }
            }
        }

        // CREATE
        static void AddStudent()
        {
            Console.Clear();
            Console.WriteLine("========== ADD STUDENT ==========\n");

            Console.Write("Enter Name: ");
            string? name = Console.ReadLine();

            Console.Write("Enter Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Phone: ");
            string? phone = Console.ReadLine();

            Console.Write("Enter Course: ");
            string? course = Console.ReadLine();

            Console.Write("Enter Batch: ");
            string? batch = Console.ReadLine();

            Console.Write("Enter Joined Year: ");
            int joinedYear = Convert.ToInt32(Console.ReadLine());

            string query = @"
                INSERT INTO Students
                (Name, Age, Phone, Course, Batch, JoinedYear)
                VALUES
                (@Name, @Age, @Phone, @Course, @Batch, @JoinedYear)";

            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Course", course);
                command.Parameters.AddWithValue("@Batch", batch);
                command.Parameters.AddWithValue("@JoinedYear", joinedYear);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("\nStudent added successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }

            Pause();
        }

        // READ
        static void ViewStudents()
        {
            Console.Clear();
            Console.WriteLine("========== STUDENT LIST ==========\n");

            string query = "SELECT * FROM Students";

            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                using SqlCommand command = new SqlCommand(query, connection);

                using SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine(
                    "{0,-5} {1,-20} {2,-5} {3,-15} {4,-15} {5,-12} {6,-12}",
                    "ID",
                    "Name",
                    "Age",
                    "Phone",
                    "Course",
                    "Batch",
                    "Joined Year"
                );

                Console.WriteLine(new string('-', 90));

                bool hasRecords = false;

                while (reader.Read())
                {
                    hasRecords = true;

                    Console.WriteLine(
                        "{0,-5} {1,-20} {2,-5} {3,-15} {4,-15} {5,-12} {6,-12}",
                        reader["Id"],
                        reader["Name"],
                        reader["Age"],
                        reader["Phone"],
                        reader["Course"],
                        reader["Batch"],
                        reader["JoinedYear"]
                    );
                }

                if (!hasRecords)
                {
                    Console.WriteLine("No student records found.");
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

            Console.Write("Enter Student ID to update: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter New Name: ");
            string? name = Console.ReadLine();

            Console.Write("Enter New Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter New Phone: ");
            string? phone = Console.ReadLine();

            Console.Write("Enter New Course: ");
            string? course = Console.ReadLine();

            Console.Write("Enter New Batch: ");
            string? batch = Console.ReadLine();

            Console.Write("Enter New Joined Year: ");
            int joinedYear = Convert.ToInt32(Console.ReadLine());

            string query = @"
                UPDATE Students
                SET Name = @Name,
                    Age = @Age,
                    Phone = @Phone,
                    Course = @Course,
                    Batch = @Batch,
                    JoinedYear = @JoinedYear
                WHERE Id = @Id";

            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Course", course);
                command.Parameters.AddWithValue("@Batch", batch);
                command.Parameters.AddWithValue("@JoinedYear", joinedYear);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("\nStudent updated successfully!");
                }
                else
                {
                    Console.WriteLine("\nStudent with the given ID was not found.");
                }
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

            Console.Write("Enter Student ID to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            string query = "DELETE FROM Students WHERE Id = @Id";

            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("\nStudent deleted successfully!");
                }
                else
                {
                    Console.WriteLine("\nStudent with the given ID was not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }

            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}