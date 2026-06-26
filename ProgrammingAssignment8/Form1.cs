// Programming Assignment 8
// Marcos Esparza

// This Program creates a Windows Forms billing form for a coffee shop
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProgrammingAssignment8;

public partial class Form1 : Form
{
    // Module-level variables used to keep track of totals for the current order
    // and the final summary for all customers.
    private decimal subtotalDecimal; // Stores the subtotal for the current customer's order before tax
    private decimal totalDecimal; // Stores the total amount due for current custmer after tax
    private decimal grandTotalDecimal; // Stores the combined total sales for all comploeted customer orders
    private int customerCountInteger; // Counts how many customer orders hae been completed

    // Constants for tax rate and coffee prices.
    private const decimal TAX_RATE_Decimal = 0.08m; // 8% sales tax for takeout orders
    private const decimal CAPPUCCINO_PRICE_Decimal = 2.00m; // price of cappuccino
    private const decimal ESPRESSO_PRICE_Decimal = 2.25m; // price of espresso
    private const decimal LATTE_PRICE_Decimal = 1.75m; // price of latte
    private const decimal ICED_PRICE_Decimal = 2.50m; // price for either iced latte or iced cappuccino

    // Form controls that appear on the windows form
    private MenuStrip MenuStrip1 = new MenuStrip();

    // File menu and its menu items
    private ToolStripMenuItem FileToolStripMenuItem = new ToolStripMenuItem("&File");
    private ToolStripMenuItem NewOrderToolStripMenuItem = new ToolStripMenuItem("&New Order");
    private ToolStripMenuItem SummaryToolStripMenuItem = new ToolStripMenuItem("&Summary");
    private ToolStripMenuItem ExitToolStripMenuItem = new ToolStripMenuItem("E&xit");

    // Edit menu and its menu items
    private ToolStripMenuItem EditToolStripMenuItem = new ToolStripMenuItem("&Edit");
    private ToolStripMenuItem CalculateSelectionToolStripMenuItem = new ToolStripMenuItem("&Calculate Selection");
    private ToolStripMenuItem ClearItemToolStripMenuItem = new ToolStripMenuItem("Clear &Item");
    private ToolStripMenuItem FontToolStripMenuItem = new ToolStripMenuItem("&Font...");
    private ToolStripMenuItem ColorToolStripMenuItem = new ToolStripMenuItem("&Color...");

    // Help menu and its About option
    private ToolStripMenuItem HelpToolStripMenuItem = new ToolStripMenuItem("&Help");
    private ToolStripMenuItem AboutToolStripMenuItem = new ToolStripMenuItem("&About");

    // Group boxes organize related controls into sections
    private GroupBox OrderInformationGroupBox = new GroupBox();
    private GroupBox CoffeeSelectionsGroupBox = new GroupBox();
    private GroupBox TotalsGroupBox = new GroupBox();

    // Labels identify what each textboxt or section means
    private Label QuantityLabel = new Label();
    private Label ItemAmountLabel = new Label();
    private Label SubTotalLabel = new Label();
    private Label TaxLabel = new Label();
    private Label TotalDueLabel = new Label();

    // Textboxes display or collect information
    private TextBox QuantityTextBox = new TextBox(); // user enters the quanity here
    private TextBox ItemAmountTextBox = new TextBox(); // Shows the calculated amount of the current item
    private TextBox SubTotalTextBox = new TextBox(); // shows the subtotal for the current order
    private TextBox TaxTextBox = new TextBox(); // shows the tax amount if the order is takeout
    private TextBox TotalTextBox = new TextBox(); // shows the final total amount due

    // Checkbox used to determine whether tax is to be added or not
    private CheckBox TaxCheckBox = new CheckBox();

    // Buttons used to calcualte an item or clear for the next item
    private Button CalculateButton = new Button();
    private Button ClearButton = new Button();

    // Radio buttons let the user choose one coffee type
    private RadioButton CappuccinoRadioButton = new RadioButton();
    private RadioButton EspressoRadioButton = new RadioButton();
    private RadioButton LatteRadioButton = new RadioButton();
    private RadioButton IcedLatteRadioButton = new RadioButton();
    private RadioButton IcedCappuccinoRadioButton = new RadioButton();

    // Dialog boxes used by the edit menu
    private ColorDialog ColorDialog1 = new ColorDialog();
    private FontDialog FontDialog1 = new FontDialog();

    public Form1()
    {
        // Builds the form manually through code.
        // This replaces using the Windows Forms Designer.
        BuildForm();
    }

    private void BuildForm()
    {
        // Main form settings, setting title, width and height of the form, prevents user from resizing
        Text = "Billing Form";
        Size = new Size(850, 520);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        // Pressing enter will activate the calculate button
        AcceptButton = CalculateButton;
        // CancelButton will activate the clear button
        CancelButton = ClearButton;

        // MenuStrip setup will add files, edit them, and help to the main menu strip
        MenuStrip1.Items.AddRange(new ToolStripItem[]
        {
            FileToolStripMenuItem,
            EditToolStripMenuItem,
            HelpToolStripMenuItem
        });
        
        // Adds new orders, summary, seperator lines, and exit under the file menu
        FileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
        {
            NewOrderToolStripMenuItem,
            SummaryToolStripMenuItem,
            new ToolStripSeparator(),
            ExitToolStripMenuItem
        });
        
        // Tells the form which MenuStrip is the main menu
        EditToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
        {
            CalculateSelectionToolStripMenuItem,
            ClearItemToolStripMenuItem,
            new ToolStripSeparator(),
            FontToolStripMenuItem,
            ColorToolStripMenuItem
        });
        // Adds the menu strip to the form so it appears on the screen

        HelpToolStripMenuItem.DropDownItems.Add(AboutToolStripMenuItem);

        MainMenuStrip = MenuStrip1;
        Controls.Add(MenuStrip1);

        // Menu click events.
        NewOrderToolStripMenuItem.Click += NewOrderToolStripMenuItem_Click;
        SummaryToolStripMenuItem.Click += SummaryToolStripMenuItem_Click;
        ExitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
        CalculateSelectionToolStripMenuItem.Click += CalculateButton_Click;
        ClearItemToolStripMenuItem.Click += ClearButton_Click;
        FontToolStripMenuItem.Click += FontToolStripMenuItem_Click;
        ColorToolStripMenuItem.Click += ColorToolStripMenuItem_Click;
        AboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;

        // New order and clear item start disabled until an order begins.
        NewOrderToolStripMenuItem.Enabled = false;
        ClearItemToolStripMenuItem.Enabled = false;

        // Order Information group box
        OrderInformationGroupBox.Text = "Order Information"; // Text shown on the group box boarder
        OrderInformationGroupBox.Location = new Point(25, 50); // Position of the group box
        OrderInformationGroupBox.Size = new Size(370, 210); // Width adn height of the group box
        Controls.Add(OrderInformationGroupBox);

        QuantityLabel.Text = "&Quantity"; // Creates a keyboard shortcut with Alt
        QuantityLabel.Location = new Point(25, 35);
        QuantityLabel.AutoSize = true;
        OrderInformationGroupBox.Controls.Add(QuantityLabel);

        // Quanitity textbox where the suer types how many items they want
        QuantityTextBox.Name = "QuantityTextBox";
        QuantityTextBox.Location = new Point(130, 32);
        QuantityTextBox.Size = new Size(100, 27);
        OrderInformationGroupBox.Controls.Add(QuantityTextBox);

        //Takeout checkbox
        // If checked, tax is then added to the order
        TaxCheckBox.Name = "TaxCheckBox";
        TaxCheckBox.Text = "Ta&keout?";
        TaxCheckBox.Location = new Point(25, 75);
        TaxCheckBox.AutoSize = true;
        OrderInformationGroupBox.Controls.Add(TaxCheckBox);

        //Calcualte selection button
        // This calculates the selected items multipled by the quanity
        CalculateButton.Name = "CalculateButton";
        CalculateButton.Text = "&Calculate\nSelection";
        CalculateButton.Location = new Point(25, 115);
        CalculateButton.Size = new Size(120, 60);
        CalculateButton.Click += CalculateButton_Click;
        OrderInformationGroupBox.Controls.Add(CalculateButton);

        // Clear the Next Item Button
        // This clears the quanity amount and prepares for another item
        ClearButton.Name = "ClearButton";
        ClearButton.Text = "C&lear for Next\nItem";
        ClearButton.Location = new Point(180, 115);
        ClearButton.Size = new Size(120, 60);
        ClearButton.Enabled = false;
        ClearButton.Click += ClearButton_Click;
        OrderInformationGroupBox.Controls.Add(ClearButton);

        // Item amount label
        ItemAmountLabel.Text = "Item Amount";
        ItemAmountLabel.Location = new Point(25, 185);
        ItemAmountLabel.AutoSize = true;
        OrderInformationGroupBox.Controls.Add(ItemAmountLabel);

        // Item amount textbox
        ItemAmountTextBox.Name = "ItemAmountTextBox";
        ItemAmountTextBox.Location = new Point(130, 182);
        ItemAmountTextBox.Size = new Size(150, 27);
        ItemAmountTextBox.ReadOnly = true;
        ItemAmountTextBox.TabStop = false;
        OrderInformationGroupBox.Controls.Add(ItemAmountTextBox);

        // Coffee Selections group box
        CoffeeSelectionsGroupBox.Text = "Coffee Selections";
        CoffeeSelectionsGroupBox.Location = new Point(430, 50);
        CoffeeSelectionsGroupBox.Size = new Size(370, 210);
        Controls.Add(CoffeeSelectionsGroupBox);

        // Cappuccino radio button
        CappuccinoRadioButton.Name = "CappuccinoRadioButton";
        CappuccinoRadioButton.Text = "&Cappuccino";
        CappuccinoRadioButton.Location = new Point(25, 35);
        CappuccinoRadioButton.AutoSize = true;
        CappuccinoRadioButton.Checked = true;
        CoffeeSelectionsGroupBox.Controls.Add(CappuccinoRadioButton);

        // Espresso radio button
        EspressoRadioButton.Name = "EspressoRadioButton";
        EspressoRadioButton.Text = "&Espresso";
        EspressoRadioButton.Location = new Point(25, 70);
        EspressoRadioButton.AutoSize = true;
        CoffeeSelectionsGroupBox.Controls.Add(EspressoRadioButton);

        // latte radio button
        LatteRadioButton.Name = "LatteRadioButton";
        LatteRadioButton.Text = "&Latte";
        LatteRadioButton.Location = new Point(25, 105);
        LatteRadioButton.AutoSize = true;
        CoffeeSelectionsGroupBox.Controls.Add(LatteRadioButton);

        // ice latte radio button
        IcedLatteRadioButton.Name = "IcedLatteRadioButton";
        IcedLatteRadioButton.Text = "&Iced Latte";
        IcedLatteRadioButton.Location = new Point(25, 140);
        IcedLatteRadioButton.AutoSize = true;
        CoffeeSelectionsGroupBox.Controls.Add(IcedLatteRadioButton);

        // Ice cappuccino radio button
        IcedCappuccinoRadioButton.Name = "IcedCappuccinoRadioButton";
        IcedCappuccinoRadioButton.Text = "Iced Ca&ppuccino";
        IcedCappuccinoRadioButton.Location = new Point(25, 175);
        IcedCappuccinoRadioButton.AutoSize = true;
        CoffeeSelectionsGroupBox.Controls.Add(IcedCappuccinoRadioButton);

        // Totals group box
        TotalsGroupBox.Text = ""; // no title
        TotalsGroupBox.Location = new Point(25, 285);
        TotalsGroupBox.Size = new Size(775, 150);
        Controls.Add(TotalsGroupBox);

        // subtotal label and textbox
        SubTotalLabel.Text = "SubTotal";
        SubTotalLabel.Location = new Point(25, 30);
        SubTotalLabel.AutoSize = true;
        TotalsGroupBox.Controls.Add(SubTotalLabel);

        SubTotalTextBox.Name = "SubTotalTextBox";
        SubTotalTextBox.Location = new Point(175, 27);
        SubTotalTextBox.Size = new Size(150, 27);
        SubTotalTextBox.ReadOnly = true;
        SubTotalTextBox.TabStop = false;
        TotalsGroupBox.Controls.Add(SubTotalTextBox);


        // Tax label and textbox
        TaxLabel.Text = "Tax (if Takeout)";
        TaxLabel.Location = new Point(25, 70);
        TaxLabel.AutoSize = true;
        TotalsGroupBox.Controls.Add(TaxLabel);

        TaxTextBox.Name = "TaxTextBox";
        TaxTextBox.Location = new Point(175, 67);
        TaxTextBox.Size = new Size(150, 27);
        TaxTextBox.ReadOnly = true;
        TaxTextBox.TabStop = false;
        TotalsGroupBox.Controls.Add(TaxTextBox);


        // Total due label and button
        TotalDueLabel.Text = "Total Due";
        TotalDueLabel.Location = new Point(25, 110);
        TotalDueLabel.AutoSize = true;
        TotalsGroupBox.Controls.Add(TotalDueLabel);

        TotalTextBox.Name = "TotalTextBox";
        TotalTextBox.Location = new Point(175, 107);
        TotalTextBox.Size = new Size(150, 27);
        TotalTextBox.ReadOnly = true;
        TotalTextBox.TabStop = false;
        TotalsGroupBox.Controls.Add(TotalTextBox);

        // Starts the cursor in the quantity textbox.
        QuantityTextBox.Focus();
    }

    private void CalculateButton_Click(object? sender, EventArgs e)
    {
        // Calculates and displays the current item amount,
        // then adds the amount to the current order totals
        decimal priceDecimal = 0; // stores price of selected coffee item
        decimal taxDecimal = 0; // stores calculated tax amount
        decimal itemAmountDecimal = 0; // stores the rpice multiplied by the quantity
        int quantityInteger = 0; // stores the quanity entered by the user

        // Determine the price based on which radio button is selected
        if (CappuccinoRadioButton.Checked)
            priceDecimal = CAPPUCCINO_PRICE_Decimal;
        else if (EspressoRadioButton.Checked)
            priceDecimal = ESPRESSO_PRICE_Decimal;
        else if (LatteRadioButton.Checked)
            priceDecimal = LATTE_PRICE_Decimal;
        else if (IcedLatteRadioButton.Checked || IcedCappuccinoRadioButton.Checked)
            priceDecimal = ICED_PRICE_Decimal;

        try
        {
            // Convert the quantity textbox value into an integer
            // if user types letters or leaves blank causes a format exception
            quantityInteger = int.Parse(QuantityTextBox.Text);

            // Calculate the selected item amount
            itemAmountDecimal = priceDecimal * quantityInteger;

            // Add the item amount to the current order subtotal
            subtotalDecimal += itemAmountDecimal;

            // If takeout is checked, calculate tax using FindTax function
            if (TaxCheckBox.Checked)
                taxDecimal = FindTax(subtotalDecimal);
            else
                taxDecimal = 0;

            // Total is subtotal plus tax 
            totalDecimal = subtotalDecimal + taxDecimal;

            // Display calculated values 
            ItemAmountTextBox.Text = itemAmountDecimal.ToString("c");
            SubTotalTextBox.Text = subtotalDecimal.ToString("n");
            TaxTextBox.Text = taxDecimal.ToString("n");
            TotalTextBox.Text = totalDecimal.ToString("c");

            // Once an order begins, the tax checkbox should not change 
            TaxCheckBox.Enabled = false;

            // Enable clear and new order options after an item is calculated 
            ClearButton.Enabled = true;
            ClearItemToolStripMenuItem.Enabled = true;
            NewOrderToolStripMenuItem.Enabled = true;
        }
        catch (FormatException)
        {
            MessageBox.Show(
                "Quantity must be numeric.",
                "Data entry error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            QuantityTextBox.Focus();
            QuantityTextBox.SelectAll();
        }
    }

    // Find tax function
    private decimal FindTax(decimal amountDecimal)
    {
        // Function procedure that calculates tax.
        return amountDecimal * TAX_RATE_Decimal;
    }

    private void ClearButton_Click(object? sender, EventArgs e)
    {
        // Clears the current item selection and prepares for the next item
        // This function recieves an amount and returns 8% of that amount
        CappuccinoRadioButton.Checked = true; // resets the selected coffe back to the default option
        ItemAmountTextBox.Clear(); // clears only the most recent item amount
        QuantityTextBox.Clear(); // clears the quantity input field
        QuantityTextBox.Focus(); // places the cursor back into the quantity field
    }

    private void NewOrderToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        DialogResult responseDialogResult; // stores whether the user clicked Yes or no
        string messageString; // stores the message shown in the confirmation box

        // Ask the user before clearing the current order
        messageString = "Clear the current order figures?";

        responseDialogResult = MessageBox.Show(
            messageString,
            "Clear Order",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        // Only clear the order if the user choooses yes
        if (responseDialogResult == DialogResult.Yes)
        {
            // Clear the item fields by reusing the clearbutton_click method
            ClearButton_Click(sender, e);

            // Clear the summary display fields 
            SubTotalTextBox.Text = "";
            TaxTextBox.Text = "";
            TotalTextBox.Text = "";

            // Add the order to the grand totals only if an order exists
            if (subtotalDecimal != 0)
            {
                grandTotalDecimal += totalDecimal;
                customerCountInteger += 1;

                // Reset totals for the next customer
                subtotalDecimal = 0;
                totalDecimal = 0;
            }

            // Re-enable tax checkbox for the next order
            TaxCheckBox.Enabled = true;
            TaxCheckBox.Checked = false;

            // Disable clear and new order until another item is calculated
            ClearButton.Enabled = false;
            ClearItemToolStripMenuItem.Enabled = false;
            NewOrderToolStripMenuItem.Enabled = false;
        }
    }

    private void SummaryToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        decimal averageDecimal; // stores the average sale amount
        string messageString; // stores the summary message shown to the user

        // If the current order has not been added to totals yet, add it first
        // this line calls NewOrderTooStripMenuItem_Click to finalize it
        if (totalDecimal != 0)
            NewOrderToolStripMenuItem_Click(sender, e);

        // only show sales totals if at least one completed order exists
        if (customerCountInteger > 0)
        {
            // Calculate the average sale
            averageDecimal = grandTotalDecimal / customerCountInteger;

            // Build the message for the summary box
            messageString =
                "Number of Orders: " + customerCountInteger.ToString() +
                Environment.NewLine + Environment.NewLine +
                "Total Sales: " + grandTotalDecimal.ToString("c") +
                Environment.NewLine + Environment.NewLine +
                "Average Sales: " + averageDecimal.ToString("c");

            MessageBox.Show(
                messageString,
                "Coffee Sales Summary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        else
        {
            messageString = "No sales data to summarize.";

            MessageBox.Show(
                messageString,
                "Coffee Sales Summary",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    // Exit menu 
    private void ExitToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        // Closes the application
        Close();
    }

    private void AboutToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        // Displays program information
        string messageString;

        messageString =
            "R and R Billing" +
            Environment.NewLine + Environment.NewLine +
            "Programmed by Marcos Esparza";

        MessageBox.Show(
            messageString,
            "About R and R Billing",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    // This method opens the font dialog
    // If user chooses a font a clicks ok
    // field's textboxes will change to that font
    private void FontToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        // Allows the user to choose a new font for the summary total textboxes
        FontDialog1.Font = SubTotalTextBox.Font;

        if (FontDialog1.ShowDialog() == DialogResult.OK)
        {
            SubTotalTextBox.Font = FontDialog1.Font;
            TaxTextBox.Font = FontDialog1.Font;
            TotalTextBox.Font = FontDialog1.Font;
        }
    }

    private void ColorToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        // Allows the user to choose a new color for the summary total textboxes
        ColorDialog1.Color = SubTotalTextBox.ForeColor;

        if (ColorDialog1.ShowDialog() == DialogResult.OK)
        {
            SubTotalTextBox.ForeColor = ColorDialog1.Color;
            TaxTextBox.ForeColor = ColorDialog1.Color;
            TotalTextBox.ForeColor = ColorDialog1.Color;
        }
    }
}