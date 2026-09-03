using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// MySQL Docker connection
string connectionString =
    "Server=127.0.0.1;Port=3307;Database=LabStudentDB;User ID=root;Password=hello123;";


// ======================================================
// HOME
// ======================================================
app.MapGet("/", () =>
{
    return Results.Content("""
        <!DOCTYPE html>
        <html>
        <head>
            <title>Student Management System</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f6f8;
                    margin: 0;
                    padding: 50px;
                }

                .container {
                    width: 500px;
                    margin: auto;
                    background: white;
                    padding: 35px;
                    border-radius: 12px;
                    box-shadow: 0 4px 15px rgba(0,0,0,0.1);
                    text-align: center;
                }

                h1 {
                    color: #333;
                }

                a {
                    display: block;
                    padding: 13px;
                    margin: 12px 0;
                    background-color: #007bff;
                    color: white;
                    text-decoration: none;
                    border-radius: 6px;
                }

                a:hover {
                    background-color: #0056b3;
                }
            </style>
        </head>

        <body>
            <div class="container">

                <h1>Student Management System</h1>

                <a href="/create">Create Student</a>
                <a href="/read">View Students</a>
                <a href="/update">Update Student</a>
                <a href="/delete">Delete Student</a>

            </div>
        </body>
        </html>
        """, "text/html");
});


// ======================================================
// CREATE - FORM
// ======================================================
app.MapGet("/create", () =>
{
    return Results.Content("""
        <!DOCTYPE html>
        <html>
        <head>
            <title>Create Student</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f6f8;
                    padding: 40px;
                }

                .container {
                    width: 500px;
                    margin: auto;
                    background: white;
                    padding: 30px;
                    border-radius: 12px;
                    box-shadow: 0 4px 15px rgba(0,0,0,0.1);
                }

                h1 {
                    text-align: center;
                }

                label {
                    font-weight: bold;
                }

                input {
                    width: 100%;
                    padding: 10px;
                    margin: 8px 0 18px;
                    box-sizing: border-box;
                    border: 1px solid #ccc;
                    border-radius: 5px;
                }

                button {
                    width: 100%;
                    padding: 12px;
                    background-color: #28a745;
                    color: white;
                    border: none;
                    border-radius: 5px;
                    cursor: pointer;
                }

                button:hover {
                    background-color: #218838;
                }

                .back {
                    background-color: #6c757d;
                    text-align: center;
                    display: block;
                    padding: 12px;
                    margin-top: 15px;
                    color: white;
                    text-decoration: none;
                    border-radius: 5px;
                }
            </style>
        </head>

        <body>
            <div class="container">

                <h1>Create Student</h1>

                <form method="post" action="/create">

                    <label>Name</label>
                    <input type="text" name="Name" required>

                    <label>Age</label>
                    <input type="number" name="Age" required>

                    <label>Address</label>
                    <input type="text" name="Address">

                    <label>Email</label>
                    <input type="email" name="Email">

                    <button type="submit">Add Student</button>

                </form>

                <a class="back" href="/">Back to Home</a>

            </div>
        </body>
        </html>
        """, "text/html");
});


// ======================================================
// CREATE - INSERT
// ======================================================
app.MapPost("/create", async (HttpRequest request) =>
{
    var form = await request.ReadFormAsync();

    string name = form["Name"].ToString();
    string address = form["Address"].ToString();
    string email = form["Email"].ToString();

    if (!int.TryParse(form["Age"], out int age))
    {
        return Results.Content("""
            <h2>Invalid age.</h2>
            <a href="/create">Go Back</a>
            """, "text/html");
    }

    string query = """
        INSERT INTO Students
        (Name, Age, Address, Email)
        VALUES
        (@Name, @Age, @Address, @Email)
        """;

    try
    {
        using var connection =
            new MySqlConnection(connectionString);

        await connection.OpenAsync();

        using var command =
            new MySqlCommand(query, connection);

        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Age", age);
        command.Parameters.AddWithValue("@Address", address);
        command.Parameters.AddWithValue("@Email", email);

        await command.ExecuteNonQueryAsync();

        return Results.Redirect("/read");
    }
    catch (Exception ex)
    {
        return Results.Content($"""
            <h2>Error while adding student</h2>
            <p>{ex.Message}</p>
            <a href="/create">Go Back</a>
            """, "text/html");
    }
});


// ======================================================
// READ
// ======================================================
app.MapGet("/read", async () =>
{
    string html = """
        <!DOCTYPE html>
        <html>
        <head>
            <title>View Students</title>

            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f6f8;
                    padding: 40px;
                }

                .container {
                    width: 90%;
                    margin: auto;
                    background: white;
                    padding: 30px;
                    border-radius: 12px;
                    box-shadow: 0 4px 15px rgba(0,0,0,0.1);
                }

                h1 {
                    text-align: center;
                }

                table {
                    width: 100%;
                    border-collapse: collapse;
                    margin-top: 20px;
                }

                th {
                    background-color: #007bff;
                    color: white;
                    padding: 12px;
                }

                td {
                    padding: 12px;
                    border: 1px solid #ddd;
                }

                tr:nth-child(even) {
                    background-color: #f2f2f2;
                }

                .back {
                    display: inline-block;
                    margin-top: 20px;
                    padding: 10px 20px;
                    background-color: #6c757d;
                    color: white;
                    text-decoration: none;
                    border-radius: 5px;
                }
            </style>
        </head>

        <body>

        <div class="container">

        <h1>Student Records</h1>

        <table>

            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Age</th>
                <th>Address</th>
                <th>Email</th>
            </tr>
        """;

    string query = "SELECT * FROM Students ORDER BY Id";

    try
    {
        using var connection =
            new MySqlConnection(connectionString);

        await connection.OpenAsync();

        using var command =
            new MySqlCommand(query, connection);

        // Important:
        // ExecuteReaderAsync() returns a DbDataReader
        // in the current MySql.Data version.
        using var reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            html += $"""
                <tr>
                    <td>{reader["Id"]}</td>
                    <td>{reader["Name"]}</td>
                    <td>{reader["Age"]}</td>
                    <td>{reader["Address"]}</td>
                    <td>{reader["Email"]}</td>
                </tr>
                """;
        }

        html += """
            </table>

            <a class="back" href="/">Back to Home</a>

            </div>

            </body>
            </html>
            """;

        return Results.Content(html, "text/html");
    }
    catch (Exception ex)
    {
        return Results.Content($"""
            <h2>Database Error</h2>
            <p>{ex.Message}</p>
            <a href="/">Back to Home</a>
            """, "text/html");
    }
});


// ======================================================
// UPDATE - FORM
// ======================================================
app.MapGet("/update", () =>
{
    return Results.Content("""
        <!DOCTYPE html>
        <html>
        <head>
            <title>Update Student</title>

            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f6f8;
                    padding: 40px;
                }

                .container {
                    width: 500px;
                    margin: auto;
                    background: white;
                    padding: 30px;
                    border-radius: 12px;
                    box-shadow: 0 4px 15px rgba(0,0,0,0.1);
                }

                h1 {
                    text-align: center;
                }

                label {
                    font-weight: bold;
                }

                input {
                    width: 100%;
                    padding: 10px;
                    margin: 8px 0 18px;
                    box-sizing: border-box;
                    border: 1px solid #ccc;
                    border-radius: 5px;
                }

                button {
                    width: 100%;
                    padding: 12px;
                    background-color: #ffc107;
                    border: none;
                    border-radius: 5px;
                    cursor: pointer;
                }

                button:hover {
                    background-color: #e0a800;
                }

                .back {
                    display: block;
                    margin-top: 15px;
                    padding: 12px;
                    background-color: #6c757d;
                    color: white;
                    text-align: center;
                    text-decoration: none;
                    border-radius: 5px;
                }
            </style>
        </head>

        <body>

        <div class="container">

            <h1>Update Student</h1>

            <form method="post" action="/update">

                <label>Student ID</label>
                <input type="number" name="Id" required>

                <label>New Name</label>
                <input type="text" name="Name" required>

                <label>New Age</label>
                <input type="number" name="Age" required>

                <label>New Address</label>
                <input type="text" name="Address">

                <label>New Email</label>
                <input type="email" name="Email">

                <button type="submit">Update Student</button>

            </form>

            <a class="back" href="/">Back to Home</a>

        </div>

        </body>
        </html>
        """, "text/html");
});


// ======================================================
// UPDATE - DATABASE
// ======================================================
app.MapPost("/update", async (HttpRequest request) =>
{
    var form = await request.ReadFormAsync();

    if (!int.TryParse(form["Id"], out int id))
    {
        return Results.Content("""
            <h2>Invalid Student ID.</h2>
            <a href="/update">Go Back</a>
            """, "text/html");
    }

    if (!int.TryParse(form["Age"], out int age))
    {
        return Results.Content("""
            <h2>Invalid age.</h2>
            <a href="/update">Go Back</a>
            """, "text/html");
    }

    string name = form["Name"].ToString();
    string address = form["Address"].ToString();
    string email = form["Email"].ToString();

    string query = """
        UPDATE Students
        SET
            Name = @Name,
            Age = @Age,
            Address = @Address,
            Email = @Email
        WHERE Id = @Id
        """;

    try
    {
        using var connection =
            new MySqlConnection(connectionString);

        await connection.OpenAsync();

        using var command =
            new MySqlCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Age", age);
        command.Parameters.AddWithValue("@Address", address);
        command.Parameters.AddWithValue("@Email", email);

        int result =
            await command.ExecuteNonQueryAsync();

        if (result == 0)
        {
            return Results.Content("""
                <h2>Student not found.</h2>
                <a href="/update">Go Back</a>
                """, "text/html");
        }

        return Results.Redirect("/read");
    }
    catch (Exception ex)
    {
        return Results.Content($"""
            <h2>Error while updating student</h2>
            <p>{ex.Message}</p>
            <a href="/update">Go Back</a>
            """, "text/html");
    }
});


// ======================================================
// DELETE - FORM
// ======================================================
app.MapGet("/delete", () =>
{
    return Results.Content("""
        <!DOCTYPE html>
        <html>
        <head>
            <title>Delete Student</title>

            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f6f8;
                    padding: 40px;
                }

                .container {
                    width: 500px;
                    margin: auto;
                    background: white;
                    padding: 30px;
                    border-radius: 12px;
                    box-shadow: 0 4px 15px rgba(0,0,0,0.1);
                }

                h1 {
                    text-align: center;
                    color: #dc3545;
                }

                label {
                    font-weight: bold;
                }

                input {
                    width: 100%;
                    padding: 10px;
                    margin: 8px 0 18px;
                    box-sizing: border-box;
                    border: 1px solid #ccc;
                    border-radius: 5px;
                }

                button {
                    width: 100%;
                    padding: 12px;
                    background-color: #dc3545;
                    color: white;
                    border: none;
                    border-radius: 5px;
                    cursor: pointer;
                }

                button:hover {
                    background-color: #c82333;
                }

                .back {
                    display: block;
                    margin-top: 15px;
                    padding: 12px;
                    background-color: #6c757d;
                    color: white;
                    text-align: center;
                    text-decoration: none;
                    border-radius: 5px;
                }
            </style>
        </head>

        <body>

        <div class="container">

            <h1>Delete Student</h1>

            <form method="post" action="/delete">

                <label>Student ID</label>

                <input type="number" name="Id" required>

                <button type="submit">Delete Student</button>

            </form>

            <a class="back" href="/">Back to Home</a>

        </div>

        </body>
        </html>
        """, "text/html");
});


// ======================================================
// DELETE - DATABASE
// ======================================================
app.MapPost("/delete", async (HttpRequest request) =>
{
    var form = await request.ReadFormAsync();

    if (!int.TryParse(form["Id"], out int id))
    {
        return Results.Content("""
            <h2>Invalid Student ID.</h2>
            <a href="/delete">Go Back</a>
            """, "text/html");
    }

    string query =
        "DELETE FROM Students WHERE Id = @Id";

    try
    {
        using var connection =
            new MySqlConnection(connectionString);

        await connection.OpenAsync();

        using var command =
            new MySqlCommand(query, connection);

        command.Parameters.AddWithValue("@Id", id);

        int result =
            await command.ExecuteNonQueryAsync();

        if (result == 0)
        {
            return Results.Content("""
                <h2>Student not found.</h2>
                <a href="/delete">Go Back</a>
                """, "text/html");
        }

        return Results.Redirect("/read");
    }
    catch (Exception ex)
    {
        return Results.Content($"""
            <h2>Error while deleting student</h2>
            <p>{ex.Message}</p>
            <a href="/delete">Go Back</a>
            """, "text/html");
    }
});


app.Run();
