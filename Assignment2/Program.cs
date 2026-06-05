// Marcos Esparza
// June 2nd, 2026


using System;
using System.Data.Common;
using System.IO;
using System.Runtime.InteropServices;

class Program
{
    // this constant stores the grand mean given to us in the assigment instructions
    // will be used for all the deviations
    const double GRAND_MEAN = 57.3;

    static void Main(string[] args)
    {
        // this array will store the name of the 12 months from the file
        string[] months = new string[12];

        // this 2D array will store the noise level data for each month and year (12 rows for months, 5 columns for years)
        int[,] data = new int[12, 5];

        // this string variable stores the name of the file we will read the data from  
        string fileName = "trend.txt";

        // this function will call the trend.txt file and load the data into the months and data arrays
        LoadData(fileName, months, data);

        // these arrays store the monthly means and deviations calculated from the data array
        // the monthly averages are then calculatd across the 5 years of each month
        double[] monthlyMeans = CalculateMonthlyMeans(data);
        double[] monthlyDeviations = CalculateMonthlyDeviations(monthlyMeans);

        // these arrays store the yearly averages and yearly deviations
        double[] yearlyMeans = CalculateYearlyMeans(data);
        double[] yearlyDeviations = CalculateYearlyDeviations(yearlyMeans);

        // this array stores the quarterly deviations calculated from the monthly means
        double[] quarterlyMeans = CalculateQuarterlyMeans(monthlyMeans);

        // this will print the complete report to the console using all the data and calculations we have done
        PrintReport(months, data, monthlyMeans, monthlyDeviations, quarterlyMeans, yearlyMeans, yearlyDeviations);
    }
    // this method will read each line from the file
    static void LoadData(string fileName, string[] months, int[,] data)
    {
        // read all the lines from trend.txt and store them in an array of strings called lines
        string[] lines = File.ReadAllLines(fileName);

        // loop through each row of the file
        for (int row = 0; row < lines.Length; row++)
        {
            // split into seperate parts using space 
            string[] parts = lines[row].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            months[row] = parts[0];

            for (int col = 0; col < 5; col++)
            {
                data[row, col] = int.Parse(parts[col + 1]);
            }
        }
    }

    // this method will calculate the average value for each month 
    // adds 5 year values and divides by 5 to get avg
    static double[] CalculateMonthlyMeans(int[,] data)
    {
        double[] averages = new double[12];

        for (int row = 0; row < 12; row++)
        {
            int sum = 0;

            for (int col = 0; col < 5; col++)
            {
                sum += data[row, col];
            }

            averages[row] = sum / 5.0;
        }
        return averages;

    }
    // this method will calculate the deviation of each month
    static double[] CalculateMonthlyDeviations(double[] monthlyMeans)
    {
        double[] deviations = new double[12];

        for (int i = 0; i < monthlyMeans.Length; i++)
        {
            deviations[i] = Math.Pow(GRAND_MEAN - monthlyMeans[i], 2);

        }

        return deviations;
    }

    // this method will valculate the average value for each year
    // adds 12 month values and divides by 12 to get avg
    static double[] CalculateYearlyMeans(int[,] data)
    {
        double[] average = new double[5];
        for (int col = 0; col < 5; col++)
        {
            int sum = 0;
            for (int row = 0; row < 12; row++)
            {
                sum += data[row, col];
            }
            average[col] = (double)sum / 12;
        }
        return average;
    }

    // this method will calculate the deviation of each year
    static double[] CalculateYearlyDeviations(double[] yearlyMeans)
    {
        double[] deviations = new double[5];

        for (int i = 0; i < yearlyMeans.Length; i++)
        {
            deviations[i] = Math.Pow(GRAND_MEAN - yearlyMeans[i], 2);
        }

        return deviations;
    }

    // this method will calculate the average value for each quarter
    // adds 3 month values and divides by 3 to get avg
    static double[] CalculateQuarterlyMeans(double[] monthlyMeans)
    {
        double[] deviations = new double[4];

        for (int quarter = 0; quarter < 4; quarter++)
        {
            int startMonth = quarter * 3;

            double quarterlyMean =
                (monthlyMeans[startMonth] +
                 monthlyMeans[startMonth + 1] +
                 monthlyMeans[startMonth + 2]) / 3.0;

            deviations[quarter] = Math.Pow(GRAND_MEAN - quarterlyMean, 2);
        }

        return deviations;
    }
        // this method is what prints the final report to the console, it takes in all the data and calculations we have done and formats it into a readable report
    static void PrintReport(
        string[] months,
        int[,] data,
        double[] monthlyAverages,
        double[] monthlyDeviations,
        double[] quarterlyDeviations,
        double[] yearlyAverages,
        double[] yearlyDeviations)
    {
        // title of the report
        Console.WriteLine("TREND-SEASONAL-NOISE ANALYSIS");
        Console.WriteLine();

        Console.WriteLine("{0,-6}{1,6}{2,6}{3,6}{4,6}{5,6}{6,12}{7,14}{8,16}",
            "", "2020", "2021", "2022", "2023", "2024", "Monthly", "Monthly", "Quarterly");

        Console.WriteLine("{0,-6}{1,6}{2,6}{3,6}{4,6}{5,6}{6,12}{7,14}{8,16}",
            "", "", "", "", "", "", "Average", "Deviation", "Deviation");
        // loop through each month and print each row of data
        for (int row = 0; row < 12; row++)
        {
            Console.Write("{0,-6}", months[row]);
// print the noise level data for each year of the month    
            for (int col = 0; col < 5; col++)
            {
                Console.Write("{0,6}", data[row, col]);
            }

            Console.Write("{0,12:F1}", monthlyAverages[row]);
            Console.Write("{0,14:F2}", monthlyDeviations[row]);
            // if statement to print the quarterly deviations in the correct rows (after each quarter)
            if (row == 2)
            {
                Console.Write("{0,16:F2}", quarterlyDeviations[0]);
            } // else if statements to print the quarterly deviations in the correct rows (after each quarter)
            else if (row == 5)
            {
                Console.Write("{0,16:F2}", quarterlyDeviations[1]);
            }
            else if (row == 8)
            {
                Console.Write("{0,16:F2}", quarterlyDeviations[2]);
            }
            else if (row == 11)
            {
                Console.Write("{0,16:F2}", quarterlyDeviations[3]);
            }

            Console.WriteLine();
        }
        // this will print a line to separate the monthly data from the yearly data
        Console.WriteLine(new string('-', 90));

        Console.Write("{0,-18}", "Yearly Average");

        for (int i = 0; i < yearlyAverages.Length; i++)
        {
            Console.Write("{0,8:F2}", yearlyAverages[i]);
        }
        // this will print a line to separate the yearly averages from the yearly deviations
        Console.WriteLine();

        Console.Write("{0,-18}", "Yearly Deviation");
        // this will print the yearly deviations in a formatted way
        for (int i = 0; i < yearlyDeviations.Length; i++)
        {
            Console.Write("{0,8:F2}", yearlyDeviations[i]);
        }

        Console.WriteLine();
    }
}