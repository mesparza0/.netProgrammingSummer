using System;

class Program
{
    static void Main(string[] args)
    {
        // defintions

        string item1;
        string item2;

        double price1;
        double price2;

        int quantity1;
        int quantity2;

        double total1;
        double total2;

        double subtotal;
        double salesTax;
        double finalTotal;

        const double TAX_RATE = 0.08;

        // input

        Console.WriteLine("Welcome to the Coffee Shop!");
        Console.Write("Enter the name of the first item: ");
        item1 = Console.ReadLine();

        Console.Write("Enter the price of the " + item1 + ": ");
        price1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter the quantity of the " + item1 + ": ");
        quantity1 = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine();

        Console.Write("Enter the name of the second item: ");
        item2 = Console.ReadLine();

        Console.Write("Enter the price of the " + item2 + ": ");
        price2 = Convert.ToDouble(Console.ReadLine());  

        Console.Write("Enter the quantity of the " + item2 + ": ");
        quantity2 = Convert.ToInt32(Console.ReadLine());

        // process selction

        total1 = price1 * quantity1;
        total2 = price2 * quantity2;

        subtotal = total1 + total2;
        salesTax = subtotal * TAX_RATE;
        finalTotal = subtotal + salesTax;

        //output


        Console.WriteLine("\nReceipt:");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"{item1} x {quantity1} @ ${price1:F2} each: ${total1:F2}");
        Console.WriteLine($"{item2} x {quantity2} @ ${price2:F2} each: ${total2:F2}");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Subtotal: ${subtotal:F2}");
        Console.WriteLine($"Sales Tax (8%): ${salesTax:F2}");   
        Console.WriteLine($"Total: ${finalTotal:F2}");
    }
}
