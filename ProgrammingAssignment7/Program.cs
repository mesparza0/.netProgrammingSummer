using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

// Programming Assignment 7
// Marcos Esparza
//
// This program uses Entity Framework Core with SQLite to manage an Item database.
// The assignment requires an Item model/class, an integer Id field, starting records,
// and a console menu that lets the user show, add, update, delete, remove all, or quit.

class Program
{
    static void Main()
    {
        // This statement creates the database context
        using (ItemDbContext context = new ItemDbContext())
        {
            // Creates/updates the database using EF Core migrations.
            context.Database.Migrate();

            // Adds the required starting records
            // It only adds them if the Items table is empty currently.
            AddItems(context);
        }

        // This while loop keps the main menu running until the user chooses 0 in order to quiz/close
        while (true)
        {
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("S. Show the Records");
            Console.WriteLine("A. Add New Record");
            Console.WriteLine("U. Update a Record");
            Console.WriteLine("D. Delete a Record");
            Console.WriteLine("R. Remove All Records");
            Console.WriteLine("Q. Quit");
            Console.Write("Enter your choice: ");

            // Read the user input
            // Trim() remove the extra spaces
            // ToUpper() make the input uppercase so that the user can type s or S
            string choice = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (choice == "S")
                ShowRecords();
            else if (choice == "A")
                AddNewRecord();
            else if (choice == "U")
                UpdateRecord();
            else if (choice == "D")
                DeleteRecord();
            else if (choice == "R")
                RemoveAllRecords();
            else if (choice == "Q")
            {
                // Ends the program by breaking out of the while loop
                Console.WriteLine("Exiting the application.");
                break;
            }
            else
                // This runs if the user enters something that is not a valid menu option
                Console.WriteLine("Invalid option. Choose S, A, U, D, R, or Q.");
        }
    }

    static void AddItems(ItemDbContext context)
    {
        // Prevents duplicate starting records every time the program runs.
        // Any() checks whether the Items table already has at least one record
        if (context.Items.Any())
            return;

        // ID is set to 0 because Entity Framework will automatically generate the ID value
        List<Item> items = new List<Item>
        {
            new Item { Id = 0, ItemNum = "AH74", Description = "Patience", OnHand = 9, Category = "GME", Storehouse = 3, Price = 22.99m },
            new Item { Id = 0, ItemNum = "BR23", Description = "Skittles", OnHand = 21, Category = "GME", Storehouse = 2, Price = 29.99m },
            new Item { Id = 0, ItemNum = "CD33", Description = "Wood Block Set (48 piece)", OnHand = 36, Category = "TOY", Storehouse = 1, Price = 89.49m },
            new Item { Id = 0, ItemNum = "DL51", Description = "Classic Railway Set", OnHand = 12, Category = "TOY", Storehouse = 3, Price = 107.95m },
            new Item { Id = 0, ItemNum = "DR67", Description = "Giant Star Brain Teaser", OnHand = 24, Category = "PZL", Storehouse = 2, Price = 31.95m },
            new Item { Id = 0, ItemNum = "DW23", Description = "Mancala", OnHand = 40, Category = "GME", Storehouse = 3, Price = 50.00m },
            new Item { Id = 0, ItemNum = "FD11", Description = "Rocking Horse", OnHand = 8, Category = "TOY", Storehouse = 3, Price = 124.95m },
            new Item { Id = 0, ItemNum = "FH24", Description = "Puzzle Gift Set", OnHand = 65, Category = "PZL", Storehouse = 1, Price = 38.95m },
            new Item { Id = 0, ItemNum = "KA12", Description = "Cribbage Set", OnHand = 56, Category = "GME", Storehouse = 3, Price = 75.00m },
            new Item { Id = 0, ItemNum = "KD34", Description = "Pentominoes Brain Teaser", OnHand = 60, Category = "PZL", Storehouse = 2, Price = 14.95m },
            new Item { Id = 0, ItemNum = "KL78", Description = "Pick Up Sticks", OnHand = 110, Category = "GME", Storehouse = 1, Price = 10.95m },
            new Item { Id = 0, ItemNum = "MT03", Description = "Zauberkasten Brain Teaser", OnHand = 45, Category = "PZL", Storehouse = 1, Price = 45.79m },
            new Item { Id = 0, ItemNum = "NL89", Description = "Wood Block Set (62 piece)", OnHand = 32, Category = "TOY", Storehouse = 3, Price = 119.75m },
            new Item { Id = 0, ItemNum = "TR40", Description = "Tic Tac Toe", OnHand = 75, Category = "GME", Storehouse = 2, Price = 13.99m },
            new Item { Id = 0, ItemNum = "TW35", Description = "Fire Engine", OnHand = 30, Category = "TOY", Storehouse = 2, Price = 118.95m }
        };

        // AddRange() adds all of the items in the list to the database context
        context.Items.AddRange(items);
        // SaveChanges() will perma save the new records
        context.SaveChanges();
    }

    static void ShowRecords()
    {
        // Creates a new database context for this method
        using ItemDbContext context = new ItemDbContext();

        // Gets all item records from the database 
        // OrderBy sorts them alphabetically
        List<Item> items = context.Items.OrderBy(i => i.ItemNum).ToList();

        // If the lsit is empty, there are no record to show
        if (items.Count == 0)
        {
            Console.WriteLine("\nNo records found.");
            return;
        }
        Console.WriteLine("\nITEM RECORDS");
        // Prints the column headers before printing the items rows
        PrintHeader();

        // Loops through each item and prints it in the table format
        foreach (Item item in items)
            PrintItem(item);
    }

    static void AddNewRecord()
    {
        using ItemDbContext context = new ItemDbContext();

        Console.WriteLine("\nADD NEW RECORD");

        Console.Write("Enter ItemNum: ");
        string itemNum = Console.ReadLine()?.Trim().ToUpper() ?? "";
        
        // ItemNum is required, so the program stops if the user leaves it blank
        if (string.IsNullOrWhiteSpace(itemNum))
        {
            Console.WriteLine("ItemNum cannot be empty.");
            return;
        }

        // Check if another item already has the same ItemNum
        // This prevents duplicate item numbers
        if (context.Items.Any(i => i.ItemNum == itemNum))
        {
            Console.WriteLine("That item already exists. No record was added.");
            return;
        }
        Console.Write("Enter Description: ");
        string description = Console.ReadLine()?.Trim() ?? "";

        // Description is also required 
        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Description cannot be empty.");
            return;
        }

        // These helper methods keep asking until the user enters valid data
        int onHand = ReadInt("Enter OnHand: ");
        string category = ReadString("Enter Category: ").ToUpper();
        int storehouse = ReadInt("Enter Storehouse: ");
        decimal price = ReadDecimal("Enter Price: ");

        // Creates a new item object using hte infromation given by the user
        Item newItem = new Item
        {
            Id = 0,
            ItemNum = itemNum,
            Description = description,
            OnHand = onHand,
            Category = category,
            Storehouse = storehouse,
            Price = price
        };

        // Shows the record before saving it so the user can confirm
        Console.WriteLine("\nRecord to add:");
        PrintHeader();
        PrintItem(newItem);

        Console.Write("Add this record? (Y/N): ");
        string confirm = Console.ReadLine()?.Trim().ToUpper() ?? "";

        if (confirm == "Y")
        {
            // Adding the new item to the database
            context.Items.Add(newItem);
            // Saves the new item permanetly
            context.SaveChanges();
            Console.WriteLine("Record added successfully.");
        }
        else
        {
            Console.WriteLine("Add cancelled.");
        }
    }

    static void UpdateRecord()
    {
        using ItemDbContext context = new ItemDbContext();

        Console.WriteLine("\nUPDATE A RECORD");
        Console.Write("Enter ItemNum to update: ");
        string itemNum = Console.ReadLine()?.Trim().ToUpper() ?? "";

        // Searches the first item with the matching ItemNum
        // FirstOrDefault() returns null if no matching record exisits
        Item? item = context.Items.FirstOrDefault(i => i.ItemNum == itemNum);

        if (item == null)
        {
            Console.WriteLine("Item was not found. No update was made.");
            return;
        }

        // Shows the current versions of the record before changing anything
        Console.WriteLine("\nCurrent record:");
        PrintHeader();
        PrintItem(item);

        // Sub-menu lets the user choose the exact field they want to update
        Console.WriteLine("\nUPDATE SUB-MENU");
        Console.WriteLine("D. Description");
        Console.WriteLine("O. OnHand");
        Console.WriteLine("C. Category");
        Console.WriteLine("S. Storehouse");
        Console.WriteLine("P. Price");
        Console.WriteLine("E. Exit");
        Console.Write("Choose field to update: ");

        string choice = Console.ReadLine()?.Trim().ToUpper() ?? "";

        if (choice == "D")
        {
            Console.Write("Enter new Description: ");
            string newDescription = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(newDescription))
            {  
                Console.WriteLine("Description cannot be empty.");
                return;
            }
            // Updates the desc accordingly
            item.Description = newDescription;
        }
        else if (choice == "O")
            item.OnHand = ReadInt("Enter new OnHand: ");
        else if (choice == "C")
            item.Category = ReadString("Enter new Category: ").ToUpper();
        else if (choice == "S")
            item.Storehouse = ReadInt("Enter new Storehouse: ");
        else if (choice == "P")
            item.Price = ReadDecimal("Enter new Price: ");
        else if (choice == "E")
        {
            Console.WriteLine("Update cancelled.");
            return;
        }
        else
        {
            Console.WriteLine("Invalid update option.");
            return;
        }
        // Because entity framework is tracking the item, SaveChanges() will save accordingly
        context.SaveChanges();

        // Shows the record again after the update
        Console.WriteLine("\nUpdated record:");
        PrintHeader();
        PrintItem(item);
    }

    static void DeleteRecord()
    {
        using ItemDbContext context = new ItemDbContext();

        Console.WriteLine("\nDELETE A RECORD");
        Console.Write("Enter ItemNum to delete: ");
        string itemNum = Console.ReadLine()?.Trim().ToUpper() ?? "";

        // Finds the item the user wants to delete
        Item? item = context.Items.FirstOrDefault(i => i.ItemNum == itemNum);

        if (item == null)
        {
            Console.WriteLine("Item was not found. No record was deleted.");
            return;
        }

        // Shows the item before deleting so the user can confirm
        Console.WriteLine("\nRecord to delete:");
        PrintHeader();
        PrintItem(item);

        Console.Write("Delete this record? (Y/N): ");
        string confirm = Console.ReadLine()?.Trim().ToUpper() ?? "";

        if (confirm == "Y")
        {
            // Removes the selected item
            context.Items.Remove(item);
            // Perma saves the deleted item
            context.SaveChanges();
            Console.WriteLine("Record deleted successfully.");
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }
    }

    static void RemoveAllRecords()
    {
        using ItemDbContext context = new ItemDbContext();
        // Gets every record from the database and sorts them by ItemNum
        List<Item> items = context.Items.OrderBy(i => i.ItemNum).ToList();

        if (items.Count == 0)
        {
            Console.WriteLine("\nNo records found.");
            return;
        }
        // Display all records before removing them
        Console.WriteLine("\nALL RECORDS TO BE REMOVED");
        PrintHeader();

        foreach (Item item in items)
            PrintItem(item);

        Console.Write("\nRemove ALL records? (Y/N): ");
        string confirm = Console.ReadLine()?.Trim().ToUpper() ?? "";

        if (confirm == "Y")
        {
            // RemoveRange() removes multiple records at once
            context.Items.RemoveRange(items);
            // Save the changes
            context.SaveChanges();
            Console.WriteLine("All records were removed.");
        }
        else
        {
            Console.WriteLine("Remove all cancelled.");
        }
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim() ?? "";

            if (int.TryParse(input, out int value))
                return value;

            Console.WriteLine("Invalid number. Enter a whole number.");
        }
    }

    static decimal ReadDecimal(string prompt)
    {   // This while loop keeps running until the user enters a valid integer
        while (true)
        {

            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim() ?? "";

            // TryParse converts the user input into an int
            // It will return true if the conversion works
            if (decimal.TryParse(input, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal value))
                return value;

            // If the input is not a whole number, user is promted to type again
            Console.WriteLine("Invalid price. Enter a valid money amount.");
        }
    }

    static string ReadString(string prompt)
    {
        // this loop ensures the user does not leave it blank
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim() ?? "";

            if (!string.IsNullOrWhiteSpace(input))
                return input;

            Console.WriteLine("Input cannot be empty.");
        }
    }

    static void PrintHeader()
    // Prints the table column name
    // The number inside the braces control spacing/alignment
    // Negative numbers left-align the text
    // Postive numbers right-align the text
    {
        Console.WriteLine("{0,-5}{1,-10}{2,-32}{3,10}{4,-12}{5,12}{6,12}",
            "Id", "ItemNum", "Description", "OnHand", "Category", "Storehouse", "Price");

        // Prints a divider
        Console.WriteLine(new string('-', 95));
    }

    static void PrintItem(Item item)
    {
        // Prints one item record in same format as the header
        Console.WriteLine("{0,-5}{1,-10}{2,-32}{3,10}{4,-12}{5,12}{6,12:C}",
            item.Id, item.ItemNum, item.Description, item.OnHand,
            item.Category, item.Storehouse, item.Price);
    }
}

// This class represents one record in the items
// Entity framework uses this class to create the table structure
public class Item
{
    // ID is the primary key
    public int Id { get; set; }

    // ItemNum stores the item number, such as AH74
    public string ItemNum { get; set; } = "";

    // Desc stores the name or explanation of the item
    public string Description { get; set; } = "";

    // OnHand stores the item category
    public int OnHand { get; set; }

    // Category stores the category 
    public string Category { get; set; } = "";

    // Storehouse stores the warehouse provider
    public int Storehouse { get; set; }

    // Price stores the item price
    public decimal Price { get; set; }
}


    // This class represents the database connection and database structure
internal class ItemDbContext : DbContext
{
    // Each item object represents one row in that table
    public DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // creates a unique index on ItemNum
    {
        optionsBuilder.UseSqlite("Data Source=ItemDatabase.db");
    }

    // Sets the database column type for Price
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .HasIndex(i => i.ItemNum)
            .IsUnique();

        modelBuilder.Entity<Item>()
            .Property(i => i.Price)
            .HasColumnType("decimal(10,2)");
    }
}
