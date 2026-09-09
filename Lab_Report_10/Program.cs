using Microsoft.Data.SqlClient;

string connectionString =
    "Server=localhost;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

while (true)
{
    Console.WriteLine("\n===== Student Management System =====");
    Console.WriteLine("1. Add Student");
    Console.WriteLine("2. View Students");
    Console.WriteLine("3. Update Student");
    Console.WriteLine("4. Delete Student");
    Console.WriteLine("5. Exit");
    Console.Write("Enter your choice: ");

    string choice = Console.ReadLine();

    if (choice == "1")
    {
        AddStudent();
    }
    else if (choice == "2")
    {
        ViewStudents();
    }
    else if (choice == "3")
    {
        UpdateStudent();
    }
    else if (choice == "4")
    {  
        DeleteStudent();
    }
    else if (choice == "5")
    {
        Console.WriteLine("Program ended.");
        break;
    }
    else
    {
        Console.WriteLine("Invalid choice!");
    }
}


void AddStudent()
{
    Console.Write("Enter Student ID: ");
    int id = Convert.ToInt32(Console.ReadLine());

    Console.Write("Enter student name: ");
    string name = Console.ReadLine();

    Console.Write("Enter age: ");
    int age = Convert.ToInt32(Console.ReadLine());

    Console.Write("Enter address: ");
    string address = Console.ReadLine();

    string query = "INSERT INTO Students (Id, Name, Age, Address) " +
                   "VALUES (@Id, @Name, @Age, @Address)";

    using SqlConnection connection = new SqlConnection(connectionString);
    using SqlCommand command = new SqlCommand(query, connection);

    command.Parameters.AddWithValue("@Id", id);
    command.Parameters.AddWithValue("@Name", name);
    command.Parameters.AddWithValue("@Age", age);
    command.Parameters.AddWithValue("@Address", address);

    connection.Open();

    try
    {
        command.ExecuteNonQuery();
        Console.WriteLine("Student added successfully!");
    }
    catch (SqlException)
    {
        Console.WriteLine("Student ID already exists!");
    }
}


void ViewStudents()
{
    string query = "SELECT * FROM Students";

    using SqlConnection connection = new SqlConnection(connectionString);
    using SqlCommand command = new SqlCommand(query, connection);

    connection.Open();

    using SqlDataReader reader = command.ExecuteReader();

    Console.WriteLine("\nID\tName\tAge\tAddress");
    Console.WriteLine("--------------------------------");

    while (reader.Read())
    {
        Console.WriteLine(
            $"{reader["Id"]}\t{reader["Name"]}\t{reader["Age"]}\t{reader["Address"]}"
        );
    }
}


void UpdateStudent()
{
    Console.Write("Enter Student ID to update: ");
    int id = Convert.ToInt32(Console.ReadLine());

    Console.Write("Enter new name: ");
    string name = Console.ReadLine();

    Console.Write("Enter new age: ");
    int age = Convert.ToInt32(Console.ReadLine());

    Console.Write("Enter new address: ");
    string address = Console.ReadLine();

    string query = "UPDATE Students " +
                   "SET Name=@Name, Age=@Age, Address=@Address " +
                   "WHERE Id=@Id";

    using SqlConnection connection = new SqlConnection(connectionString);
    using SqlCommand command = new SqlCommand(query, connection);

    command.Parameters.AddWithValue("@Name", name);
    command.Parameters.AddWithValue("@Age", age);
    command.Parameters.AddWithValue("@Address", address);
    command.Parameters.AddWithValue("@Id", id);

    connection.Open();

    int rows = command.ExecuteNonQuery();

    if (rows > 0)
        Console.WriteLine("Student updated successfully!");
    else
        Console.WriteLine("Student not found!");
}


void DeleteStudent()
{
    Console.Write("Enter Student ID to delete: ");
    int id = Convert.ToInt32(Console.ReadLine());

    string query = "DELETE FROM Students WHERE Id=@Id";

    using SqlConnection connection = new SqlConnection(connectionString);
    using SqlCommand command = new SqlCommand(query, connection);

    command.Parameters.AddWithValue("@Id", id);

    connection.Open();

    int rows = command.ExecuteNonQuery();

    if (rows > 0)
        Console.WriteLine("Student deleted successfully!");
    else
        Console.WriteLine("Student not found!");
}