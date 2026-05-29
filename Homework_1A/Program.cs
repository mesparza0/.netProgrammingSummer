// Question 1 - 

using System;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Pay Rate: ");
            double payRate = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter amount of Hours worked: ");
            double hoursWorked = Convert.ToDouble(Console.ReadLine());

            double grossPay = payRate * hoursWorked;
            double taxes = grossPay * 0.0495;
            double netPay = grossPay - taxes;

            Console.WriteLine();
            Console.WriteLine("Payroll Summary");
            Console.WriteLine("-----------------");
            Console.WriteLine("Employee Name: " + name);
            Console.WriteLine("Gross Employee Pay: " + grossPay.ToString("C"));
            Console.WriteLine("Taxes: " + taxes.ToString("C"));
            Console.WriteLine("Net Employee Pay: " + netPay.ToString("C"));
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
            Console.WriteLine("Error details: " + ex.Message);
        }
    
    }
}