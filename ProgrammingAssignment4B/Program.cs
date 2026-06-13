// Marcos Esparza
// June 7, 2026
// Programming Assignment 4B
// Employee Base Class with CommissionWorker and PieceWorker Subclasses
// This program reads employee.txt and creates a gross pay report

using System;
using System.Collections.Generic;
using System.IO;

namespace ProgrammingAssignment4B
{
    internal class Program
    {
        // Base class: Employee 
        // stores the information that all employees ahve in common
        // employee id, first name, last name
        public class Employee
        {
            // Data members
            private int id_num;
            private string first_name;
            private string last_name;

            // Default constructor
            public Employee()
            {
                id_num = 0;
                first_name = "No Name";
                last_name = "No Name";
            }

            // Set-all constructor
            // This constructor will allow the progam to create an employee object
            // and then give values to all the main employee fields
            public Employee(int id_num, string first_name, string last_name)
            {
                this.id_num = id_num;
                this.first_name = first_name;
                this.last_name = last_name;
            }

            // Set all data members at one time
            // Can be used after an object has already been created
            public void setData(int id_num, string first_name, string last_name)
            {
                this.id_num = id_num;
                this.first_name = first_name;
                this.last_name = last_name;
            }

            // Setters allow the program to change private data members
            // from outside the class in a controlled way
            public void setId(int id_num)
            {
                this.id_num = id_num;
            }

            public void setFirstName(string first_name)
            {
                this.first_name = first_name;
            }

            public void setLastName(string last_name)
            {
                this.last_name = last_name;
            }

            // Getters that allow the prgram to safely acess private data members
            // from outside the class
            public int getId()
            {
                return id_num;
            }
            public string getFirstName()
            {
                return first_name;
            }

            public string getLastName()
            {
                return last_name;
            }

            // Virtual displayData method
            // This method will format the basic employee infomration 
            // the -12 and -15 values line up the output columns
            public virtual string displayData()
            {
                return $"{id_num,-12}{first_name,-15}{last_name,-15}";
            }

            // Virtual earnings method
            // This method will return 0 in the base employee class
            // because a general employee does not have a specific pay formula

            // the CommisionWorker and PieceWorker override this method
            public virtual double earnings()
            {
                return 0;
            }

            // Virtual method will return employee type
            public virtual string getEmployeeType()
            {
                return "Employee";
            }

            // Method used for the output report 
            // will call getEmployeeType() and earnings()
            public virtual string reportLine()
            {
                return $"{getEmployeeType(),-22}{id_num,-12}{first_name,-15}{last_name,-15}{earnings(),10:C}";
            }
        }

        // Derived class: CommissionWorker
        // CommissionWorker is a child class of Emplyee
        // will inherit ID number, first name and last name from the employee
        // includes salary, commission rate, and sales

        // formula used salary/52 + sales * commission rate
        public class CommissionWorker : Employee
        {
            // Data members
            private double salary;
            private double comm_rate;
            private double sales;

            // Default constructor
            public CommissionWorker() : base()
            {
                salary = 0;
                comm_rate = 0;
                sales = 0;
            }

            // Set-all constructor recieves both the employee data and commissionworker specific data
            public CommissionWorker(int id_num, string first_name, string last_name,
                                    double salary, double comm_rate, double sales)
                : base(id_num, first_name, last_name)
            {
                this.salary = salary;
                this.comm_rate = comm_rate;
                this.sales = sales;
            }

            // Set all data members sets both the inhertied employee data and the commissionworker specfic data
            public void setData(int id_num, string first_name, string last_name,
                                double salary, double comm_rate, double sales)
            {
                base.setData(id_num, first_name, last_name);
                this.salary = salary;
                this.comm_rate = comm_rate;
                this.sales = sales;
            }

            // Setters
            public void setSalary(double salary)
            {
                this.salary = salary;
            }

            public void setCommRate(double comm_rate)
            {
                this.comm_rate = comm_rate;
            }

            public void setSales(double sales)
            {
                this.sales = sales;
            }

            // Getters
            public double getSalary()
            {
                return salary;
            }

            public double getCommRate()
            {
                return comm_rate;
            }

            public double getSales()
            {
                return sales;
            }

            // Override displayData is replacing the parent version with a version made specfically for CommissionWorker
            public override string displayData()
            {
                return base.displayData() +
                       $"{salary,-15:C}{comm_rate,-15}{sales,-15:C}";
            }

            // Override earnings
            // calculate the weekly pay for a commision worker
            // Weekly pay = salary / 52 + sales * commission rate
            public override double earnings()
            {
                return (salary / 52) + (sales * comm_rate);
            }

            // Override employee type
            public override string getEmployeeType()
            {
                return "Commission Worker";
            }
        }

        // Derived class: PieceWorker
        // PieceWorker is another child class of Employee
        // will inherit ID, first, last name. 

        // paid based on how many items they produce
        // wage per price * quantity
        public class PieceWorker : Employee
        {
            // Data members
            private double wage_per_piece;
            private int quantity;

            // Default constructor
            public PieceWorker() : base()
            {
                wage_per_piece = 0;
                quantity = 0;
            }

            // Set-all constructor to set all the shared employee data
            public PieceWorker(int id_num, string first_name, string last_name,
                               double wage_per_piece, int quantity)
                : base(id_num, first_name, last_name)
            {
                this.wage_per_piece = wage_per_piece;
                this.quantity = quantity;
            }

            // Set all data members
            // this method will update all PieceWorker data at the same time
            public void setData(int id_num, string first_name, string last_name,
                                double wage_per_piece, int quantity)
            {
                base.setData(id_num, first_name, last_name);
                this.wage_per_piece = wage_per_piece;
                this.quantity = quantity;
            }

            // Setters
            public void setWagePerPiece(double wage_per_piece)
            {
                this.wage_per_piece = wage_per_piece;
            }

            public void setQuantity(int quantity)
            {
                this.quantity = quantity;
            }

            // Getters
            public double getWagePerPiece()
            {
                return wage_per_piece;
            }

            public int getQuantity()
            {
                return quantity;
            }

            // Override displayData
            // This prints the inherited employee data first,
            // then add wage per piece and quantity
            public override string displayData()
            {
                return base.displayData() +
                       $"{wage_per_piece,-15:C}{quantity,-15}";
            }

            // Override earnings
            // calculate the weekly pay for a piece worker
            // Weekly pay = wage per piece * quantity
            public override double earnings()
            {
                return wage_per_piece * quantity;
            }

            // Override employee type
            // will output the correct emoployee type for the report
            public override string getEmployeeType()
            {
                return "Piece Worker";
            }
        }

        static void Main(string[] args)
        {
            // List that stores Employee objects.
            // This demonstrates polymorphism because the list can hold
            // CommissionWorker and PieceWorker objects as Employee objects
            List<Employee> employees = new List<Employee>();

            // name of the input file
            string inputFile = "employee.txt";

            // name of output file
            string outputFile = "gross_pay_report.txt";

            // Check if employee.txt exists
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("Error: employee.txt was not found.");
                Console.WriteLine("Make sure employee.txt is inside the ProgrammingAssignment4B folder.");
                return;
            }

            // Read all lines from the input file
            string[] lines = File.ReadAllLines(inputFile);

            int skippedRecords = 0;

            foreach (string line in lines)
            {
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                // Split the line by spaces or tabs
                string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                // First value tells us the employee type
                string empType = parts[0];

                // C = Commission Worker
                if (empType == "C")
                {
                    int id = int.Parse(parts[1]);
                    string firstName = parts[2];
                    string lastName = parts[3];
                    double salary = double.Parse(parts[4]);
                    double commRate = double.Parse(parts[5]);
                    double sales = double.Parse(parts[6]);

                    CommissionWorker worker = new CommissionWorker(id, firstName, lastName, salary, commRate, sales);

                    employees.Add(worker);
                }
                // P = Piece Worker
                else if (empType == "P")
                {
                    int id = int.Parse(parts[1]);
                    string firstName = parts[2];
                    string lastName = parts[3];
                    double wagePerPiece = double.Parse(parts[4]);
                    int quantity = int.Parse(parts[5]);

                    PieceWorker worker = new PieceWorker(id, firstName, lastName, wagePerPiece, quantity);

                    employees.Add(worker);
                }
                else
                {
                    // S and H will be skipped
                    skippedRecords++;
                }
            }

            // Create the output report file
            // StreamWriter is used to generate the text file
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                // report title
                writer.WriteLine("Gross-pay salary report");
                writer.WriteLine();

                // report column headings
                writer.WriteLine($"{ "Employee",-22}{ "Employee",-12}{ "First",-15}{ "Last",-15}{ "Weekly",10}");
                writer.WriteLine($"{ "Type",-22}{ "Number",-12}{ "Name",-15}{ "Name",-15}{ "Pay",10}");
                writer.WriteLine(new string('-', 74));

                // Polymorphism happens here
                // Each object calls its own version of earnings()
                // the program will automatically use the correct earnings() and getEmployeeType() methods
                // from on the actual object type
                foreach (Employee emp in employees)
                {
                    writer.WriteLine(emp.reportLine());
                }

                writer.WriteLine();
                writer.WriteLine("Records skipped because they were not assigned classes: " + skippedRecords);
            }

            Console.WriteLine("Programming Assignment 4B completed successfully.");
            Console.WriteLine("Output file created: " + outputFile);
            Console.WriteLine("Employees processed: " + employees.Count);
            Console.WriteLine("Records skipped: " + skippedRecords);
        }
    }
}