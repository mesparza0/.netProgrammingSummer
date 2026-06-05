// Marcos Esparza
// May 31st, 2026

using System;
class Program
{
    static void Main(string[] args)
    {

        // shows the title of the program to the user
        Console.WriteLine("Input Items to Generate Loan Repayment Schedule");


        // prompts the user to enter the total loan amount and will also validate whether
        // the amount is between 5k and 500k
        Console.Write("Enter Loan Amount:");
        double loanAmount = double.Parse(Console.ReadLine());

        while (loanAmount < 5000 || loanAmount > 500000)
        {
            Console.WriteLine("Loan must be between $5,000 and $500,000. Please enter a valid loan amount.");
            loanAmount = double.Parse(Console.ReadLine());
        }

        // ask the user for the yearly interest rate and then converts it to a decimal by dividing by 100
        Console.Write("Yearly Interest Rate:");
        double yearlyInterestRate = double.Parse(Console.ReadLine()) / 100;

        // ask the user how many years they will need to repay the loan
        Console.Write("Years of repayment:");
        int years = int.Parse(Console.ReadLine());

        // ask the user how many payments they will make be making each year
        Console.Write("Payments per year: ");
        int paymentsPerYear = int.Parse(Console.ReadLine());

        int totalPayments = years * paymentsPerYear;
        double payment = CalculatePayment(loanAmount, yearlyInterestRate, paymentsPerYear, totalPayments);


        // print the total number of payments and payment amount to the user
        Console.WriteLine($"\nTotal Payments: {totalPayments}");
        Console.WriteLine($"Monthly Payment: {payment:C}");
        // this is the start of the repayment scehdule, 
        Console.WriteLine("\nLoan Repayment Schedule");
        Console.WriteLine("Payment Num\tDate\t\tPayment\t\tInterest\tPrincipal\tBalance");

        double balance = loanAmount;
        double totalPaid = 0;
        // stores the total interest paid over the life of the loan
        double totalInterest = 0;
        // starts the payment scehdule using today's date
        DateTime paymentDate = DateTime.Today;

        Console.WriteLine($"0\t\t{paymentDate.ToShortDateString()}\t$0.00\t\t$0.00\t\t$0.00\t\t{balance:C}");

        // for loop to go through every payment until the loan is paid off 
        for (int i = 1; i <= totalPayments; i++)
        {
            paymentDate = paymentDate.AddMonths(12 / paymentsPerYear);

            double interest = balance * (yearlyInterestRate / paymentsPerYear);
            double principal = payment - interest;

            // if statement to make sure the final prinicpal payment does not go over the remaining balance
            if (principal > balance)
            {
                principal = balance;
                payment = interest + principal;
            }

            balance -= principal;

            if (balance < 0.01)
            {
                balance = 0;
            }

            // add this payment to the total amount paid and add the interest to the total interest paid
            totalPaid += payment;
            totalInterest += interest;

            Console.WriteLine($"{i}\t\t{paymentDate.ToShortDateString()}\t{payment:C}\t{interest:C}\t\t{principal:C}\t\t{balance:C}");
        }

        // this will print the final totals after the schedule is finished 
        Console.WriteLine($"\nTotal Payment: {totalPaid:C}");
        Console.WriteLine($"Total Interest: {totalInterest:C}");
    }

    // this function will calculate the payment amount using the loan amount 
    static double CalculatePayment(double loanAmount, double yearlyRate, int paymentsPerYear, int totalPayments)
    {   // will convert yearly interest into the interest rate per payment period
        double periodicRate = yearlyRate / paymentsPerYear;

        double payment = (loanAmount * periodicRate) /
                         (1 - Math.Pow(1 + periodicRate, -totalPayments));

        return payment;
    }
}