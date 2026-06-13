// Marcos Esparza
// June 8, 2026
// Programming Assignment 4A
// Employee Base Class with CommissionWorker and PieceWorker Subclasses

using System;

namespace ProgrammingAssignment4A
{
    internal class Program
    {
        // Base class: Employee
        public class Employee
        {
            // Data members for Employee and will be common to all subclasses
            private int id_num;
            private string first_name;
            private string last_name;

            // Default constructor that starts data members with the default values
            public Employee()
            {
                id_num = 0;
                first_name = "No Name";
                last_name = "No Name";
            }

            // Set-all constructor that will take the parameters and set the data members
            public Employee(int id_num, string first_name, string last_name)
            {
                this.id_num = id_num;
                this.first_name = first_name;
                this.last_name = last_name;
            }

            // Set all data method that will take the parameters and set the data members
            public void setData(int id_num, string first_name, string last_name)
            {
                this.id_num = id_num;
                this.first_name = first_name;
                this.last_name = last_name;
            }

            // Setters for each data member
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

            // Getters for each data member
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

            // Virtual method so subclasses can override it 
            // in order to display their specific data 
            public virtual string displayData()
            {
                return $"{id_num,-10} {last_name,-15} {first_name,-15}";
            }

            // Virtual earnings method
            // this will be overridden if the subcases have differrent 
            // ways to calculate earnings
            public virtual string earnings()
            {
                return $"{displayData()} Weekly Pay: {0:C}";
            }
        }

        // Derived class: CommissionWorker that is inherited by Employee
        public class CommissionWorker : Employee
        {
            // Data members that are specific to the CommissionWorker subclass
            private double salary;
            private double comm_rate;
            private double sales;

            // Default constructor that starts the data member with those default values
            public CommissionWorker() : base()
            {
                salary = 0;
                comm_rate = 0;
                sales = 0;
            }

            // Set-all constructor that will take the parameters and set the data members
            public CommissionWorker(int id_num, string first_name, string last_name,
                                    double salary, double comm_rate, double sales)
                : base(id_num, first_name, last_name)
            {
                this.salary = salary;
                this.comm_rate = comm_rate;
                this.sales = sales;
            }

            // Set all data method that will take the parameter and then set the data members
            public void setData(int id_num, string first_name, string last_name,
                                double salary, double comm_rate, double sales)
            {
                base.setData(id_num, first_name, last_name);
                this.salary = salary;
                this.comm_rate = comm_rate;
                this.sales = sales;
            }

            // Setters for each data member
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

            // Getters for each data member
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

            // Override displayData from Employee
            // to display the data specific to the CommissionWoreker subclass
            public override string displayData()
            {
                return base.displayData() +
                       $" {salary,15:C} {comm_rate,15:P2} {sales,15:C}";
            }

            // Override earnings from Employee to calculate the weekly pay for the CommissionWorker subclass
            public override string earnings()
            {
                double weeklyPay = (salary / 52) + (sales * comm_rate);

                return $"CommissionWorker  {base.displayData()} Weekly Pay: {weeklyPay:C}";
            }
        }

        // Derived class: PieceWorker
        // inherited by Employee and has its own data members, constructors, setters, getters, and overrides the displayData and earnings methods
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

            // Set-all constructor
            public PieceWorker(int id_num, string first_name, string last_name,
                               double wage_per_piece, int quantity)
                : base(id_num, first_name, last_name)
            {
                this.wage_per_piece = wage_per_piece;
                this.quantity = quantity;
            }

            // Set all data
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

            // Override displayData from Employee
            public override string displayData()
            {
                return base.displayData() +
                       $" {wage_per_piece,15:C} {quantity,10}";
            }

            // Override earnings from Employee
            public override string earnings()
            {
                double weeklyPay = wage_per_piece * quantity;

                return $"PieceWorker       {base.displayData()} Weekly Pay: {weeklyPay:C}";
            }
        }

        static void Main(string[] args)
        {
            // The assignment says to test the application using the employee data.
            // I am assigned CommissionWorker and PieceWorker only.

            Console.WriteLine("Programming Assignment 4A");
            Console.WriteLine("Employee, CommissionWorker, and PieceWorker");
            Console.WriteLine();

            // Create a CommissionWorker object using the assignment test data
            // Using the set-all constructor to set the data members
            CommissionWorker employee3 = new CommissionWorker(
                356, "Anthony", "Mendez", 30563.56, 0.003, 57864.53
            );

            // Create a PieceWorker object using the assignment test data
            // Using the set-all constructor to set the data members
            PieceWorker employee4 = new PieceWorker(
                452, "Jimmy", "James", 0.50, 1201
            );

            // Display data for CommissionWorker and then calculate earnings
            Console.WriteLine("Commission Worker Data:");
            Console.WriteLine(employee3.displayData());
            Console.WriteLine(employee3.earnings());
            Console.WriteLine();

            // Display data for PieceWorker and calculate earnings
            Console.WriteLine("Piece Worker Data:");
            Console.WriteLine(employee4.displayData());
            Console.WriteLine(employee4.earnings());
            Console.WriteLine();

            // Polymorphism test
            // Create two Employee references and then assign them to the CommissionWorker
            // and PieceWorker objects
            Employee one;
            Employee two;

            // Assign the CommissionWorker and PieceWorker objects to the Employee references
            one = new CommissionWorker(356, "Anthony", "Mendez", 30563.56, 0.003, 57864.53);
            two = new PieceWorker(452, "Jimmy", "James", 0.50, 1201);

            Console.WriteLine("Polymorphism Test:");
            Console.WriteLine(one.earnings());
            Console.WriteLine(two.earnings());

            Console.WriteLine();
            Console.WriteLine("Test successful!");
        }
    }
}