// Marcos Esparza
// May 29, 2026
// Programming Assignment 1

using System;
using System.ComponentModel;
using System.Security.Principal;
class Program
{
    static void Main(string[] args)
    {

        // setting all the variables that will be used to calculate the points for the credit card application and the final decision on the credit limit
        int age;
        int AddressYears;
        double income;
        int jobYears;

        // variable to store the total points for the credit card application
        int points = 0;

        // user input of age
        Console.Write("Enter your age: ");
        age = int.Parse(Console.ReadLine());

        // user input of years lived at current address
        Console.Write("Enter the amount of years lived at current address: ");
        AddressYears = int.Parse(Console.ReadLine());

        // user input of annual income
        Console.Write("Enter Annuel Income: $");
        income = double.Parse(Console.ReadLine());

        // user input of years at current job
        Console.Write("Enter the number of years at current job: ");
        jobYears = int.Parse(Console.ReadLine());

        // -------------------------------------------------
        // Age Points
        // -------------------------------------------------

        if (age <= 20)
        {
            points -= 10;
        }
        else if (age >= 21 && age <= 30)
        {
            points += 0;

        }
        else if (age >= 31 && age <= 50)
        {
            points += 20;
        }
        else
        {
            points += 25;
        }
        // -------------------------------------------------
        // address points
        // -------------------------------------------------

        if (AddressYears < 1)
        {
            points -= 5;
        }
        else if (AddressYears >= 1 && AddressYears <= 3)
        {
            points += 5;
        }
        else if (AddressYears >= 4 && AddressYears <= 8)
        {
            points += 12;
        }

        else
        {
            points += 20;
        }

        // -------------------------------------------------
        // income points
        // -------------------------------------------------

        if (income <= 15000)
        {
            points += 0;
        }
        else if (income >= 15001 && income <= 25000)
        {
            points += 12;
        }
        else if (income >= 25001 && income <= 40000)
        {
            points += 24;
        }
        else
        {
            points += 30;
        }

        // -------------------------------------------------
        // job points
        // -------------------------------------------------

        if (jobYears < 2)
        {
            points -= 4;
        }
        else if (jobYears >= 2 && jobYears <= 4)
        {
            points += 8;
        }
        else
        {
            points += 15;
        }

        Console.WriteLine();

        // -------------------------------------------------
        // final points and decision
        // -------------------------------------------------

        if (points >= -19 && points <= 20)
        {
            Console.WriteLine("No Card Issues");
        }

        else if (points >= 21 && points <= 35)
        {
            Console.WriteLine("Card Issused with $500 credit limit");
        }
        else if (points >= 36 && points <=60)
        {
            Console.WriteLine("Card Issused with $2000 credit limit");
        }
        else
        {
            Console.WriteLine("Card Issused with $5000 credit limit");
        }

        Console.ReadLine();
    }
}