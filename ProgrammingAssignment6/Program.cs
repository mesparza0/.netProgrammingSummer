// Marcos Esparza
// Programming Assignment 6
// This program creates the SalesComp database, builds all required tables,
// inserts the assignment data, runs five SQL SELECT queries,
// and saves each report into its own text file.
using System;
using System.Globalization;
using System.IO;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main()
    {
        //Foreign Keys=True helps enforce referential integrity between related tables
        string connectionString = "Data Source=SalesComp.db;Foreign Keys=True;";

        // creating the tables and insert all starting data
        CreateTablesAndInsertData(connectionString);

        // these 5 methods will each run one SQL query and create one text report file
        GenerateReport1(connectionString);
        GenerateReport2(connectionString);
        GenerateReport3(connectionString);
        GenerateReport4(connectionString);
        GenerateReport5(connectionString);

        // These messages let the user know that the program finished successfully
        Console.WriteLine("SalesComp database was created successfully.");
        Console.WriteLine("Five report files were generated:");
        Console.WriteLine("Report1_InvoiceTotals.txt");
        Console.WriteLine("Report2_Customer10011Invoices.txt");
        Console.WriteLine("Report3_TennesseeVendors.txt");
        Console.WriteLine("Report4_LowStockProducts.txt");
        Console.WriteLine("Report5_ProductsNotSold.txt");
    }


    // this method will open a connection to the database
    // it can be used by the table creation method and every report method
    static SqliteConnection OpenConnection(string connectionString)
    {

        // create a new database connection
        SqliteConnection connection = new SqliteConnection(connectionString);
        connection.Open();

        // SQLite requires this command
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();
        
        // return the opened connection so the calling method can use it
        return connection;
    }

    // this method will create the give database tabels and intert all assigment data
    // tables are dropped to start so the program can run multiple times without duplicates
    static void CreateTablesAndInsertData(string connectionString)
    {
        // open a connection to SalesComp database
        using SqliteConnection connection = OpenConnection(connectionString);

        // this sql string will contain all commands that are needed ot reset and rebuild the the database
        string sql = @"
            DROP TABLE IF EXISTS [Line];
            DROP TABLE IF EXISTS Invoice;
            DROP TABLE IF EXISTS Product;
            DROP TABLE IF EXISTS Customer;
            DROP TABLE IF EXISTS Vendor;

            CREATE TABLE Vendor
            (
                VEND_CODE INTEGER PRIMARY KEY,
                VEND_NAME TEXT NOT NULL,
                VEND_CONTACT TEXT NOT NULL,
                VEND_AREACODE TEXT NOT NULL,
                VEND_PHONE TEXT NOT NULL,
                VEND_STATE TEXT NOT NULL,
                VEND_ORDER TEXT NOT NULL
            );

            CREATE TABLE Customer
            (
                CUST_CODE INTEGER PRIMARY KEY,
                CUST_LNAME TEXT NOT NULL,
                CUST_FNAME TEXT NOT NULL,
                CUST_INITIAL TEXT,
                CUST_AREACODE TEXT NOT NULL,
                CUST_PHONE TEXT NOT NULL,
                CUST_BALANCE REAL NOT NULL
            );

            CREATE TABLE Product
            (
                PROD_CODE TEXT PRIMARY KEY,
                PROD_DESCRIPT TEXT NOT NULL,
                PROD_INDATE TEXT NOT NULL,
                PROD_QOH INTEGER NOT NULL,
                PROD_MIN INTEGER NOT NULL,
                PROD_PRICE REAL NOT NULL,
                PROD_DISCOUNT REAL NOT NULL,
                VEND_CODE INTEGER,
                FOREIGN KEY (VEND_CODE) REFERENCES Vendor(VEND_CODE)
            );

            CREATE TABLE Invoice
            (
                INV_NUMBER INTEGER PRIMARY KEY,
                CUST_CODE INTEGER NOT NULL,
                INV_DATE TEXT NOT NULL,
                FOREIGN KEY (CUST_CODE) REFERENCES Customer(CUST_CODE)
            );

            CREATE TABLE [Line]
            (
                INV_NUMBER INTEGER NOT NULL,
                LINE_NUMBER INTEGER NOT NULL,
                PROD_CODE TEXT NOT NULL,
                LINE_UNITS INTEGER NOT NULL,
                LINE_PRICE REAL NOT NULL,
                PRIMARY KEY (INV_NUMBER, LINE_NUMBER),
                FOREIGN KEY (INV_NUMBER) REFERENCES Invoice(INV_NUMBER),
                FOREIGN KEY (PROD_CODE) REFERENCES Product(PROD_CODE)
            );

            INSERT INTO Vendor VALUES
            (21225, 'Bryson, Inc.', 'Smithson', '615', '223-3234', 'TN', 'Y'),
            (21226, 'SuperLoo, Inc.', 'Flushing', '904', '215-8995', 'FL', 'N'),
            (21231, 'D&E Supply', 'Singh', '615', '228-3245', 'TN', 'Y'),
            (21344, 'Gomez Bros.', 'Ortega', '615', '889-2546', 'KY', 'N'),
            (22567, 'Dome Supply', 'Smith', '901', '678-1419', 'GA', 'N'),
            (23119, 'Randsets Ltd.', 'Anderson', '901', '678-3998', 'GA', 'Y'),
            (24004, 'Brackman Bros.', 'Browning', '615', '228-1410', 'TN', 'N'),
            (24288, 'ORDVA, Inc.', 'Hakford', '615', '898-1234', 'TN', 'Y'),
            (25443, 'B&K, Inc.', 'Smith', '904', '227-0093', 'FL', 'N'),
            (25501, 'Damal Supplies', 'Smythe', '615', '890-3529', 'TN', 'N'),
            (25595, 'Rubicon Systems', 'Orton', '904', '456-0092', 'FL', 'Y');

            INSERT INTO Customer VALUES
            (10010, 'Ramas', 'Alfred', 'A', '615', '844-2573', 0),
            (10011, 'Dunne', 'Leona', 'K', '713', '894-1238', 0),
            (10012, 'Smith', 'Kathy', 'W', '615', '894-2285', 345.86),
            (10013, 'Olowski', 'Paul', 'F', '615', '894-2180', 536.75),
            (10014, 'Orlando', 'Myron', NULL, '615', '222-1672', 0),
            (10015, 'O''Brian', 'Amy', 'B', '713', '442-3381', 0),
            (10016, 'Brown', 'James', 'G', '615', '297-1228', 221.19),
            (10017, 'Williams', 'George', NULL, '615', '290-2556', 768.93),
            (10018, 'Farriss', 'Anne', 'G', '713', '382-7185', 216.55),
            (10019, 'Smith', 'Olette', 'K', '615', '297-3809', 0);

            INSERT INTO Product VALUES
            ('11QER/31', 'Power painter, 15 psi., 3-nozzle', '2021-11-03', 8, 5, 109.99, 0.00, 25595),
            ('13-Q2/P2', '7.25-in. pwr. saw blade', '2021-12-13', 32, 15, 14.99, 0.05, 21344),
            ('14-Q1/L3', '9.00-in. pwr. saw blade', '2021-11-13', 18, 12, 17.49, 0.00, 21344),
            ('1546-QQ2', 'Hrd. cloth, 1/4-in., 2x50', '2022-01-15', 15, 8, 39.95, 0.00, 23119),
            ('1558-QW1', 'Hrd. cloth, 1/2-in., 3x50', '2022-01-15', 23, 5, 43.99, 0.00, 23119),
            ('2232/QTY', 'B&D jigsaw, 12-in. blade', '2021-12-30', 8, 5, 109.92, 0.05, 24288),
            ('2232/QWE', 'B&D jigsaw, 8-in. blade', '2021-12-24', 6, 5, 99.87, 0.05, 24288),
            ('2238/QPD', 'B&D cordless drill, 1/2-in.', '2022-01-20', 12, 5, 38.95, 0.05, 25595),
            ('23109-HB', 'Claw hammer', '2022-01-20', 23, 10, 9.95, 0.10, 21225),
            ('23114-AA', 'Sledge hammer, 12 lb.', '2022-01-02', 8, 5, 14.40, 0.05, NULL),
            ('54778-2T', 'Rat-tail file, 1/8-in. fine', '2021-12-15', 43, 20, 4.99, 0.00, 21344),
            ('89-WRE-Q', 'Hicut chain saw, 16 in.', '2022-02-07', 11, 5, 256.99, 0.05, 24288),
            ('PVC23DRT', 'PVC pipe, 3.5-in., 8-ft', '2022-02-20', 188, 75, 5.87, 0.00, NULL),
            ('SM-18277', '1.25-in. metal screw, 25', '2022-03-01', 172, 75, 6.99, 0.00, 21225),
            ('SW-23116', '2.5-in. wd. screw, 50', '2022-02-24', 237, 100, 8.45, 0.00, 21231),
            ('WR3/TT3', 'Steel matting, 4''x8''x1/6"", .5"" mesh', '2022-01-17', 18, 5, 119.95, 0.10, 25595);

            INSERT INTO Invoice VALUES
            (1001, 10014, '2022-01-16'),
            (1002, 10011, '2022-01-16'),
            (1003, 10012, '2022-01-16'),
            (1004, 10011, '2022-01-17'),
            (1005, 10018, '2022-01-17'),
            (1006, 10014, '2022-01-17'),
            (1007, 10015, '2022-01-17'),
            (1008, 10011, '2022-01-17');

            INSERT INTO [Line] VALUES
            (1001, 1, '13-Q2/P2', 1, 14.99),
            (1001, 2, '23109-HB', 1, 9.95),
            (1002, 1, '54778-2T', 2, 4.99),
            (1003, 1, '2238/QPD', 1, 38.95),
            (1003, 2, '1546-QQ2', 1, 39.95),
            (1003, 3, '13-Q2/P2', 5, 14.99),
            (1004, 1, '54778-2T', 3, 4.99),
            (1004, 2, '23109-HB', 2, 9.95),
            (1005, 1, 'PVC23DRT', 12, 5.87),
            (1006, 1, 'SM-18277', 3, 6.99),
            (1006, 2, '2232/QTY', 1, 109.92),
            (1006, 3, '23109-HB', 1, 9.95),
            (1006, 4, '89-WRE-Q', 1, 256.99),
            (1007, 1, '13-Q2/P2', 2, 14.99),
            (1007, 2, '54778-2T', 1, 4.99),
            (1008, 1, 'PVC23DRT', 5, 5.87),
            (1008, 2, 'WR3/TT3', 3, 119.95),
            (1008, 3, '23109-HB', 1, 9.95);
        ";
        // create a SQL command object using the long SQL script
        using SqliteCommand command = new SqliteCommand(sql, connection);
        command.ExecuteNonQuery();
    }


    // Report 1: 
    // This will list customer information, invoice number, adn invoice total
    static void GenerateReport1(string connectionString)
    {
        using SqliteConnection connection = OpenConnection(connectionString);

        // this query will join customer, invoice and line
        string query = @"
            SELECT 
                C.CUST_CODE,
                C.CUST_FNAME,
                C.CUST_LNAME,
                I.INV_NUMBER,
                ROUND(SUM(L.LINE_NUMBER * L.LINE_PRICE), 2) AS INVOICE_TOTAL
            FROM Customer C
            JOIN Invoice I ON C.CUST_CODE = I.CUST_CODE
            JOIN [Line] L ON I.INV_NUMBER = L.INV_NUMBER
            GROUP BY C.CUST_CODE, C.CUST_FNAME, C.CUST_LNAME, I.INV_NUMBER
            ORDER BY C.CUST_CODE, I.INV_NUMBER;
        ";
        //StreadWriter create the report 1 text file
        using StreamWriter writer = new StreamWriter("Report1_InvoiceTotals.txt");
        using SqliteCommand command = new SqliteCommand(query, connection);
        using SqliteDataReader reader = command.ExecuteReader();

        //grandTotal keeps track of the total for all invoice totals
        double grandTotal = 0;

        writer.WriteLine("INVOICE TOTALS PER INVOICE AND CUSTOMER");
        writer.WriteLine();
        writer.WriteLine("{0,-12}{1,-16}{2,-16}{3,-12}{4,10}", "Customer", "Customer", "Customer", "Invoice", "Total");
        writer.WriteLine("{0,-12}{1,-16}{2,-16}{3,-12}{4,10}", "Code", "First Name", "Last Name", "Number", "");
        writer.WriteLine(new string('-', 66));
        // read each row returned by the query andw rite it in the report file
        while (reader.Read())
        {
            int custCode = Convert.ToInt32(reader["CUST_CODE"]);
            string first = reader["CUST_FNAME"].ToString() ?? "";
            string last = reader["CUST_LNAME"].ToString() ?? "";
            int invoice = Convert.ToInt32(reader["INV_NUMBER"]);
            double total = Convert.ToDouble(reader["INVOICE_TOTAL"]);

            grandTotal += total;

            writer.WriteLine("{0,-12}{1,-16}{2,-16}{3,-12}{4,10:0.00}",
                custCode, first, last, invoice, total);
        }

        writer.WriteLine(new string('-', 66));
        writer.WriteLine("{0,-56}{1,10:0.00}", "TOTALS", grandTotal);
    }
    // Report 2:
    // List invoice line details for only the customer with code 10011
    static void GenerateReport2(string connectionString)
    {
        using SqliteConnection connection = OpenConnection(connectionString);
        // this query will join Invoice, Line and Product
        string query = @"
            SELECT
                I.INV_NUMBER,
                L.LINE_NUMBER,
                L.PROD_CODE,
                P.PROD_DESCRIPT,
                L.LINE_UNITS,
                L.LINE_PRICE,
                ROUND(L.LINE_UNITS * L.LINE_PRICE, 2) AS LINE_TOTAL
            FROM Invoice I
            JOIN [Line] L ON I.INV_NUMBER = L.INV_NUMBER
            JOIN Product P ON L.PROD_CODE = P.PROD_CODE
            WHERE I.CUST_CODE = 10011
            ORDER BY I.INV_NUMBER, L.LINE_NUMBER;
        ";
        //StreamWriter will create the report 2 text file
        using StreamWriter writer = new StreamWriter("Report2_Customer10011Invoices.txt");
        using SqliteCommand command = new SqliteCommand(query, connection);
        using SqliteDataReader reader = command.ExecuteReader();
        // these variables are what is used to calculate the total at the bottom of the report
        int totalUnits = 0;
        double totalPrice = 0;
        double totalLineTotal = 0;

        writer.WriteLine("INVOICES BELONGING TO CUSTOMER WITH CODE: 10011");
        writer.WriteLine();
        writer.WriteLine("{0,-10}{1,-8}{2,-12}{3,-42}{4,8}{5,10}{6,10}",
            "INVOICE", "LINE", "CODE", "DESCRIPTION", "UNITS", "PRICE", "TOTAL");
        writer.WriteLine(new string('-', 100));
        // read each row returened by the query and write it to the report file
        while (reader.Read())
        {
            int invoice = Convert.ToInt32(reader["INV_NUMBER"]);
            int line = Convert.ToInt32(reader["LINE_NUMBER"]);
            string code = reader["PROD_CODE"].ToString() ?? "";
            string description = reader["PROD_DESCRIPT"].ToString() ?? "";
            int units = Convert.ToInt32(reader["LINE_UNITS"]);
            double price = Convert.ToDouble(reader["LINE_PRICE"]);
            double lineTotal = Convert.ToDouble(reader["LINE_TOTAL"]);

            // add the current row values to the report totals
            totalUnits += units;
            totalPrice += price;
            totalLineTotal += lineTotal;

            writer.WriteLine("{0,-10}{1,-8}{2,-12}{3,-42}{4,8}{5,10:0.00}{6,10:0.00}",
                invoice, line, code, description, units, price, lineTotal);
        }

        writer.WriteLine(new string('-', 100));
        writer.WriteLine("{0,-72}{1,8}{2,10:0.00}{3,10:0.00}",
            "TOTALS", totalUnits, totalPrice, totalLineTotal);
    }
    // Report 3:
    // Lists vendors that are either located in Tennese or have the area code 615
    static void GenerateReport3(string connectionString)
    {

        // This quert selects vendor contact information
        // The OR condition matches vendors with the area code 615 OR state TN
        using SqliteConnection connection = OpenConnection(connectionString);

        string query = @"
            SELECT 
                VEND_CODE,
                VEND_NAME,
                VEND_CONTACT,
                VEND_PHONE,
                VEND_STATE
            FROM Vendor
            WHERE VEND_AREACODE = '615' OR VEND_STATE = 'TN'
            ORDER BY VEND_CODE;
        ";

        // SteadWriter creates the Report 3 Text file
        using StreamWriter writer = new StreamWriter("Report3_TennesseeVendors.txt");
        using SqliteCommand command = new SqliteCommand(query, connection);
        using SqliteDataReader reader = command.ExecuteReader();

        writer.WriteLine("VENDORS LOCATED IN THE STATE OF TENNESSEE OR WITH AREA CODE: 615");
        writer.WriteLine();
        writer.WriteLine("{0,-12}{1,-22}{2,-18}{3,-14}{4,-10}",
            "VEND CODE", "VEND NAME", "VEND CONTACT", "VEND PHONE", "VEND STATE");
        writer.WriteLine(new string('-', 76));
        // Read each row and write it to the report file
        while (reader.Read())
        {
            writer.WriteLine("{0,-12}{1,-22}{2,-18}{3,-14}{4,-10}",
                reader["VEND_CODE"],
                reader["VEND_NAME"],
                reader["VEND_CONTACT"],
                reader["VEND_PHONE"],
                reader["VEND_STATE"]);
        }
    }

    // Report 4:
    // List products where the quanity on hand is only five items or less above the minimum
    static void GenerateReport4(string connectionString)
    {
        using SqliteConnection connection = OpenConnection(connectionString);
        // this querty finds products close to their minum stock level
        string query = @"
            SELECT
                PROD_CODE,
                PROD_DESCRIPT,
                PROD_INDATE,
                PROD_QOH,
                PROD_MIN
            FROM Product
            WHERE PROD_QOH - PROD_MIN BETWEEN 0 AND 5
            ORDER BY PROD_CODE;
        ";
        // SteamWriter creates the report 4 text file
        using StreamWriter writer = new StreamWriter("Report4_LowStockProducts.txt");
        using SqliteCommand command = new SqliteCommand(query, connection);
        using SqliteDataReader reader = command.ExecuteReader();

        writer.WriteLine("PRODUCTS WITH FIVE ITEMS OR LESS HIGHER THAN THEIR MINIMUM LEVEL");
        writer.WriteLine();
        writer.WriteLine("{0,-12}{1,-42}{2,-15}{3,10}{4,10}",
            "PROD_CODE", "PROD_DESCRIPT", "PROD_INDATE", "PROD_QOH", "PROD_MIN");
        writer.WriteLine(new string('-', 90));
        // read each row returned by the query and write it to the report file
        while (reader.Read())
        {   
            // this will format the date before writing it to the report
            string date = FormatDate(reader["PROD_INDATE"].ToString() ?? "");

            writer.WriteLine("{0,-12}{1,-42}{2,-15}{3,10}{4,10}",
                reader["PROD_CODE"],
                reader["PROD_DESCRIPT"],
                date,
                reader["PROD_QOH"],
                reader["PROD_MIN"]);
        }
    }
    // Report 5
    // List products that have not been sold yet
    static void GenerateReport5(string connectionString)
    {
        using SqliteConnection connection = OpenConnection(connectionString);
        // this querty will select products that are not found in the Line Table
        // The NOT IN subquery removes products that already appear on the invoice
        string query = @"
            SELECT
                P.PROD_CODE,
                P.PROD_DESCRIPT,
                P.PROD_INDATE,
                P.PROD_QOH
            FROM Product P
            WHERE P.PROD_CODE NOT IN
            (
                SELECT PROD_CODE
                FROM [Line]
            )
            ORDER BY P.PROD_DESCRIPT;
        ";
        //StreamWriter creates the report 5 text file
        using StreamWriter writer = new StreamWriter("Report5_ProductsNotSold.txt");
        using SqliteCommand command = new SqliteCommand(query, connection);
        using SqliteDataReader reader = command.ExecuteReader();

        writer.WriteLine("PRODUCTS IN STOCK WITHOUT A PLACED ORDER");
        writer.WriteLine();
        writer.WriteLine("{0,-12}{1,-42}{2,-15}{3,10}",
            "PROD_CODE", "PROD_DESCRIPT", "PROD_INDATE", "PROD_QOH");
        writer.WriteLine(new string('-', 80));
        // read each row returned by the query and write it into the report file
        while (reader.Read())
        {      // Format the date before writing it to the report
            string date = FormatDate(reader["PROD_INDATE"].ToString() ?? "");

            writer.WriteLine("{0,-12}{1,-42}{2,-15}{3,10}",
                reader["PROD_CODE"],
                reader["PROD_DESCRIPT"],
                date,
                reader["PROD_QOH"]);
        }
    }
    // This method will help format the database dates so they can look professioanl on the report
    static string FormatDate(string dateText)
    {
        DateTime date = DateTime.Parse(dateText, CultureInfo.InvariantCulture);
        return date.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
    }
}