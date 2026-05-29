// Name : Marcos Esparza
// Date : 05/29/2026

using System;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            // get user input for the taxable income amount
            Console.Write("Enter taxable income amount: $");
            double income = double.Parse(Console.ReadLine());
            
            // variable used to store the calculated tax payable
            double taxPayable = 0;


            // No tax for incomes between $1.00 and $4461.99
            if (income >= 1.00 && income <= 4461.99)
            {
                taxPayable = 0;
            }
            
            // 30% tax for income over 4462.00 up to $17,893.99
            else if (income >= 4462.00 && income <= 17893.99)
            {
                taxPayable = (income - 4462.00) * 0.30;
            }

            //  Base tax plus 35% on amount over $17894.00
            else if (income >= 17894.00 && income <= 29499.99)
            {
                taxPayable = 4119.00 + (income - 17894.00) * 0.35;
            }

            //  Base tax plus 46% on amount over $29500.00
            else if (income >= 29500.00 && income <= 45787.99)
            {
            taxPayable = 8656.00 + (income - 29500.00) * 0.46;
            }

            // Base tax plus 60% on amount over $45788.00
            else if (income >= 45788.00)
            {
                taxPayable = 11179.00 + (income - 45788.00) * 0.60;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a positive number.");
                return;
            }
            // show final tax payable in currency format with two decimal places
            Console.WriteLine("Tax payable: ${0:F2}", taxPayable);
        }
        catch
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");   
        }
    } 
}