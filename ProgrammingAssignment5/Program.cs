// Marcos Esparza
// Programming Assignment 5
// This program will create a small database that contains five tables
// once the tables are created, the program runs five select queries and displays each able in the console

using System;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main()
    {
        // The connection string says where the database is located
        string connectionString = "Data Source=Computer_Software.db";

        // this method is responsible for building the database
        // it will drop old versions of the table, create new tables, 
        // and insert all of the data from the assignment
        CreateTablesAndInsertData(connectionString);

        Console.WriteLine("\nCOMPUTER SOFTWARE DATABASE\n");

        // the assignment asks for five SELECT queries
        // each of these will call to DisplayTable and then runs one SELECT query
        // and then will print the results

        // Query 1: Display all rows and columns from the Computer table.
        DisplayTable(connectionString, "Computer", "SELECT CompID, MFGName, MFGModel, ProcType FROM Computer;");
        // Query 2: Display all rows and culumns from the employee table. 
        DisplayTable(connectionString, "Employee", "SELECT EmpNum, EmpFirst, EmpLast, EmpPhone FROM Employee;");
        // Query 3: Display all rows and columns from the PC table. 
        DisplayTable(connectionString, "PC", "SELECT TagNum, CompID, EmpNum, Location FROM PC;");
        // Query 4: Display all rows and columns from the Package table.
        DisplayTable(connectionString, "Package", "SELECT PackID, PackName, PackVer, PackType, PackCost FROM Package;");
        // Query 5: Display all rows and columns from the Software table. 
        DisplayTable(connectionString, "Software", "SELECT PackID, TagNum, InstDate, SoftCost FROM Software;");
        // Final message that lets the suer know the program has finished successfully.
        Console.WriteLine("\nDone. All five tables were displayed.");
    }

    static void CreateTablesAndInsertData(string connectionString)
    {   
        // Open the database connection so SQL commands can be sent to the database
        using SqliteConnection connection = new SqliteConnection(connectionString);
        connection.Open();

        // SQL string that contains all commands neeed to delete old tables, create the 
        // five required tables, insert all assigment data into said tables
        string sql = @"
            DROP TABLE IF EXISTS Software;
            DROP TABLE IF EXISTS PC;
            DROP TABLE IF EXISTS Package;
            DROP TABLE IF EXISTS Employee;
            DROP TABLE IF EXISTS Computer;

            CREATE TABLE Computer
            (
                CompID TEXT PRIMARY KEY,
                MFGName TEXT NOT NULL,
                MFGModel TEXT NOT NULL,
                ProcType TEXT NOT NULL
            );

            CREATE TABLE Employee
            (
                EmpNum INTEGER PRIMARY KEY,
                EmpFirst TEXT NOT NULL,
                EmpLast TEXT NOT NULL,
                EmpPhone TEXT NOT NULL
            );

            CREATE TABLE PC
            (
                TagNum INTEGER PRIMARY KEY,
                CompID TEXT NOT NULL,
                EmpNum INTEGER NOT NULL,
                Location TEXT NOT NULL,
                FOREIGN KEY (CompID) REFERENCES Computer(CompID),
                FOREIGN KEY (EmpNum) REFERENCES Employee(EmpNum)
            );

            CREATE TABLE Package
            (
                PackID TEXT PRIMARY KEY,
                PackName TEXT NOT NULL,
                PackVer REAL NOT NULL,
                PackType TEXT NOT NULL,
                PackCost REAL NOT NULL
            );

            CREATE TABLE Software
            (
                PackID TEXT NOT NULL,
                TagNum INTEGER NOT NULL,
                InstDate TEXT NOT NULL,
                SoftCost REAL NOT NULL,
                PRIMARY KEY (PackID, TagNum),
                FOREIGN KEY (PackID) REFERENCES Package(PackID),
                FOREIGN KEY (TagNum) REFERENCES PC(TagNum)
            );

            INSERT INTO Computer VALUES
            ('B121', 'Bantam', '48X', '486DX'),
            ('B221', 'Bantam', '48D', '486DX2'),
            ('C007', 'Cody', 'D1', '486DX'),
            ('M759', 'Lemmin', 'GRL', '486SX');

            INSERT INTO Employee VALUES
            (123, 'Melissa', 'Mendez', '874-736-8752'),
            (124, 'Ramon', 'Alvarez', '121-234-5462'),
            (562, 'Betty', 'Feinstein', '871-653-6723'),
            (611, 'Melissa', 'Dinh', '296-363-6452'),
            (745, 'Jonathan', 'Smith', '312-653-8234'),
            (823, 'Tina', 'Duarte', '708-234-7723');

            INSERT INTO PC VALUES
            (23556, 'C007', 123, 'Accounting'),
            (32808, 'M759', 611, 'Accounting'),
            (37691, 'B121', 124, 'Sales'),
            (57772, 'C007', 562, 'Info Systems'),
            (59836, 'B221', 124, 'Home'),
            (63721, 'M759', 611, 'Home'),
            (77740, 'M759', 562, 'Home');

            INSERT INTO Package VALUES
            ('AC01', 'Boise Accounting', 3.00, 'Accounting', 725.83),
            ('DB32', 'Manta', 1.50, 'Database', 380.00),
            ('DB33', 'Manta', 2.10, 'Database', 430.18),
            ('SS11', 'Limitless View', 5.30, 'Spreadsheet', 271.95),
            ('WP08', 'Words & More', 2.00, 'Word Processing', 185.00),
            ('WP09', 'Freeware Processing', 4.27, 'Word Processing', 30.00);

            INSERT INTO Software VALUES
            ('AC01', 32808, '9/13/2025', 745.95),
            ('AC01', 63721, '4/2/2025', 867.56),
            ('DB32', 32808, '12/3/2025', 380.00),
            ('DB32', 37691, '6/15/2025', 380.00),
            ('DB33', 57772, '5/27/2025', 412.77),
            ('WP08', 32808, '1/12/2024', 185.00),
            ('WP08', 37691, '6/15/2024', 227.50),
            ('WP08', 57772, '5/27/2023', 170.24),
            ('WP09', 59836, '10/30/2022', 35.00),
            ('WP09', 77740, '5/27/2024', 35.00);
        ";
        // Create a command object that contains all the SQL above.
        using SqliteCommand command = new SqliteCommand(sql, connection);
        // ExecuteNonQuery is used on all SQL commands that do not return rows
        // CREATE TABLE, DROP TABLE, and INSERT INTO
        command.ExecuteNonQuery();
    }

    static void DisplayTable(string connectionString, string tableName, string query)
    {
        // Print a new heading to the user 
        Console.WriteLine($"\n===== {tableName} =====");

        // Open a new database connection for the SELECT query
        using SqliteConnection connection = new SqliteConnection(connectionString);
        connection.Open();

        // Create a command using the SELECT query passed into the method.
        using SqliteCommand command = new SqliteCommand(query, connection);
        using SqliteDataReader reader = command.ExecuteReader();
        
        // Print the column names first
        // FieldCount tells us how many columns are returned by the query
        for (int i = 0; i < reader.FieldCount; i++)
        {
            // left align the text so the output looks like columns
            Console.Write($"{reader.GetName(i),-20}");
        }


        Console.WriteLine();
        // print the separator line
        Console.WriteLine(new string('-', reader.FieldCount * 20));

        // Read one row at a time
        while (reader.Read())
        {   
            // print each column value from the current row
            for (int i = 0; i < reader.FieldCount; i++)
            {
                // Will get the value from column i in the current row
                Console.Write($"{reader.GetValue(i),-20}");
            }
            // move to the next line after printing the full row
            Console.WriteLine();
        }
    }
}
