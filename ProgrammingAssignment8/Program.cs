using System;
using System.Windows.Forms;

namespace ProgrammingAssignment8;

static class Program
{    /// The main entry point for the application.
    /// This starts the Windows Forms program and opens Form1.
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}