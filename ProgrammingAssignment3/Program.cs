// Marcos Esparza
// June 2026
// Programming Assignment 3

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    // This structure stores one inventory record from invento.txt.
    struct InventoryItem
    {
        public string StockID;
        public double Price;
        public string StockDescription;
        public int ItemsOnStock;
        public int ItemsOrdered;
    }

    static void Main(string[] args)
    {
        List<InventoryItem> inventoryList = new List<InventoryItem>();

        LoadInventory(inventoryList);

        int choice = -1;

        do
        {
            DisplayMenu();

            bool validChoice = int.TryParse(Console.ReadLine(), out choice);

            if (!validChoice)
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    DeleteFirstRecord(inventoryList);
                    break;

                case 2:
                    double totalPrice = SumPrices(inventoryList);
                    Console.WriteLine($"Total sum of prices: {totalPrice:C}");
                    break;

                case 3:
                    int location = FindLargestItemsOnStock(inventoryList);

                    if (location != -1)
                    {
                        Console.WriteLine("\nRecord with the largest items on stock:");
                        PrintHeader();
                        PrintOneRecord(inventoryList[location]);
                    }
                    break;

                case 4:
                    SortByDescription(inventoryList);
                    break;

                case 5:
                    SortByItemsOnStockDescending(inventoryList);
                    break;

                case 6:
                    PrintReport(inventoryList);
                    break;

                case 7:
                    DeleteUsingKey(inventoryList);
                    break;

                case 8:
                    AddNewRecord(inventoryList);
                    break;

                case 9:
                    DisplayAllRecords(inventoryList);
                    break;

                case 0:
                    Console.WriteLine("Program ended.");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

        } while (choice != 0);
    }

    // This method loads all inventory records from the input file into the list.
    static void LoadInventory(List<InventoryItem> inventoryList)
    {
        string fileName = "invento.txt";

        if (!File.Exists(fileName))
        {
            Console.WriteLine("The input file invento.txt was not found.");
            return;
        }

        string[] lines = File.ReadAllLines(fileName);

        foreach (string line in lines)
        {
            string[] parts = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 5)
            {
                InventoryItem item = new InventoryItem();

                item.StockID = parts[0];
                item.Price = double.Parse(parts[1]);
                item.StockDescription = parts[2];
                item.ItemsOnStock = int.Parse(parts[3]);
                item.ItemsOrdered = int.Parse(parts[4]);

                inventoryList.Add(item);
            }
        }

        Console.WriteLine("Inventory records loaded successfully.");
    }

    // This method displays the menu options for the user.
    static void DisplayMenu()
    {
        Console.WriteLine("\n========== Inventory Menu ==========");
        Console.WriteLine("1. Delete first record");
        Console.WriteLine("2. Sum prices");
        Console.WriteLine("3. Find largest items on stock");
        Console.WriteLine("4. Sort by stock description ascending");
        Console.WriteLine("5. Sort by items on stock descending");
        Console.WriteLine("6. Print report file");
        Console.WriteLine("7. Delete record using Stock ID");
        Console.WriteLine("8. Add new record");
        Console.WriteLine("9. Display all records");
        Console.WriteLine("0. Quit");
        Console.Write("Enter your choice: ");
    }

    // 6.1 This method deletes the first record in the table.
    static void DeleteFirstRecord(List<InventoryItem> inventoryList)
    {
        if (inventoryList.Count > 0)
        {
            inventoryList.RemoveAt(0);
            Console.WriteLine("The first record has been deleted.");
        }
        else
        {
            Console.WriteLine("The table is empty. No record was deleted.");
        }
    }

    // 6.2 This method sums the first numeric field, which is Price.
    static double SumPrices(List<InventoryItem> inventoryList)
    {
        double total = 0;

        foreach (InventoryItem item in inventoryList)
        {
            total += item.Price;
        }

        return total;
    }

    // 6.3 This method finds the location of the largest ItemsOnStock value.
    static int FindLargestItemsOnStock(List<InventoryItem> inventoryList)
    {
        if (inventoryList.Count == 0)
        {
            Console.WriteLine("The table is empty.");
            return -1;
        }

        int largestLocation = 0;

        for (int i = 1; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].ItemsOnStock > inventoryList[largestLocation].ItemsOnStock)
            {
                largestLocation = i;
            }
        }

        return largestLocation;
    }

    // 6.4 This method sorts records by stock description in ascending order.
    static void SortByDescription(List<InventoryItem> inventoryList)
    {
        inventoryList.Sort((a, b) => a.StockDescription.CompareTo(b.StockDescription));
        Console.WriteLine("The records have been sorted by stock description in ascending order.");
    }

    // 6.5 This method sorts records by items on stock in descending order.
    static void SortByItemsOnStockDescending(List<InventoryItem> inventoryList)
    {
        inventoryList.Sort((a, b) => b.ItemsOnStock.CompareTo(a.ItemsOnStock));
        Console.WriteLine("The records have been sorted by items on stock in descending order.");
    }

    // 6.6 This method prints the records to a report file.
    static void PrintReport(List<InventoryItem> inventoryList)
    {
        using (StreamWriter writer = new StreamWriter("report.txt"))
        {
            writer.WriteLine("Inventory Report".PadLeft(50));
            writer.WriteLine();

            writer.WriteLine("{0,-10} {1,10} {2,-35} {3,15} {4,15}",
                "Stock ID", "Price", "Description", "On Stock", "Ordered");

            writer.WriteLine(new string('-', 90));

            foreach (InventoryItem item in inventoryList)
            {
                writer.WriteLine("{0,-10} {1,10:C} {2,-35} {3,15} {4,15}",
                    item.StockID,
                    item.Price,
                    item.StockDescription,
                    item.ItemsOnStock,
                    item.ItemsOrdered);
            }
        }

        Console.WriteLine("The report.txt file has been created.");
    }

    // 6.7.1 This method searches for a record using the primary key, StockID.
    static int FindRecordUsingKey(List<InventoryItem> inventoryList, string key)
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].StockID.Equals(key, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    // 6.7.2 This method deletes a record at a specific list location.
    static void DeleteRecordAtAddressLocation(List<InventoryItem> inventoryList, int location)
    {
        if (location >= 0 && location < inventoryList.Count)
        {
            inventoryList.RemoveAt(location);
            Console.WriteLine("The record has been deleted.");
        }
        else
        {
            Console.WriteLine("Invalid record location.");
        }
    }

    // 6.7 This method deletes a record based on the primary key.
    static void DeleteUsingKey(List<InventoryItem> inventoryList)
    {
        Console.Write("Enter the Stock ID to delete: ");
        string key = Console.ReadLine();

        int location = FindRecordUsingKey(inventoryList, key);

        if (location == -1)
        {
            Console.WriteLine("Record was not found.");
            return;
        }

        Console.WriteLine("\nRecord found:");
        PrintHeader();
        PrintOneRecord(inventoryList[location]);

        Console.Write("Are you sure you want to delete this record? (Y/N): ");
        string answer = Console.ReadLine();

        if (answer.ToUpper() == "Y")
        {
            DeleteRecordAtAddressLocation(inventoryList, location);
        }
        else
        {
            Console.WriteLine("Delete operation canceled.");
        }
    }

    // 6.8 This method adds a new record after checking if the StockID is available.
    static void AddNewRecord(List<InventoryItem> inventoryList)
    {
        Console.Write("Enter new Stock ID: ");
        string key = Console.ReadLine();

        int location = FindRecordUsingKey(inventoryList, key);

        if (location != -1)
        {
            Console.WriteLine("That Stock ID is already in use. Record was not added.");
            return;
        }

        InventoryItem newItem = new InventoryItem();

        newItem.StockID = key;

        Console.Write("Enter price: ");
        while (!double.TryParse(Console.ReadLine(), out newItem.Price) || newItem.Price < 0)
        {
            Console.Write("Invalid price. Enter price again: ");
        }

        Console.Write("Enter stock description: ");
        newItem.StockDescription = Console.ReadLine();

        Console.Write("Enter items on stock: ");
        while (!int.TryParse(Console.ReadLine(), out newItem.ItemsOnStock) || newItem.ItemsOnStock < 0)
        {
            Console.Write("Invalid number. Enter items on stock again: ");
        }

        Console.Write("Enter items ordered: ");
        while (!int.TryParse(Console.ReadLine(), out newItem.ItemsOrdered) || newItem.ItemsOrdered < 0)
        {
            Console.Write("Invalid number. Enter items ordered again: ");
        }

        Console.WriteLine("\nNew record entered:");
        PrintHeader();
        PrintOneRecord(newItem);

        Console.Write("Do you want to add this record? (Y/N): ");
        string answer = Console.ReadLine();

        if (answer.ToUpper() == "Y")
        {
            inventoryList.Add(newItem);
            Console.WriteLine("The new record has been added.");
        }
        else
        {
            Console.WriteLine("Add operation canceled.");
        }
    }

    // 6.9 This method displays all records on the screen.
    static void DisplayAllRecords(List<InventoryItem> inventoryList)
    {
        if (inventoryList.Count == 0)
        {
            Console.WriteLine("The table is empty.");
            return;
        }

        PrintHeader();

        foreach (InventoryItem item in inventoryList)
        {
            PrintOneRecord(item);
        }
    }

    // This helper method prints the table headings.
    static void PrintHeader()
    {
        Console.WriteLine();
        Console.WriteLine("{0,-10} {1,10} {2,-35} {3,15} {4,15}",
            "Stock ID", "Price", "Description", "On Stock", "Ordered");

        Console.WriteLine(new string('-', 90));
    }

    // This helper method prints one formatted record.
    static void PrintOneRecord(InventoryItem item)
    {
        Console.WriteLine("{0,-10} {1,10:C} {2,-35} {3,15} {4,15}",
            item.StockID,
            item.Price,
            item.StockDescription,
            item.ItemsOnStock,
            item.ItemsOrdered);
    }
}