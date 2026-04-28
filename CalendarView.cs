using System;
using System.Drawing;
using System.Drawing.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

using Label = System.Windows.Forms.Label;

namespace First;

public class CalendarView : Panel
{
    private TableLayoutPanel _weekGrid;
    private Label _weekRangeLabel;
    // private TableLayoutPanel monthGrid;
    // private TableLayoutPanel dayGrid;
    private List<TaskItem> _tasks;
    public CalendarView(List<TaskItem> mainList)
    {
        _tasks = mainList;
        this.Dock = DockStyle.Fill;
        this.BackColor = Color.FromArgb(240, 240, 240);
        SetupUI();
    }
    private void SetupUI()
    {
        DateTime today = DateTime.Today;
        int difference = (5 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        DateTime startOfWeek = today.AddDays(-1 * difference);
        DateTime endOfWeek = startOfWeek.AddDays(6);

        Panel headerArea = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            Padding = new Padding(20, 10, 20, 0)
        };

        Label weekRangeLabel = new Label
        {
            Text = $"{startOfWeek:dd.MM} - {endOfWeek:dd.MM}",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(20, -45),
            AutoSize = true,
            ForeColor = Color.FromArgb(64, 64, 64)
        };
        // headerArea.Controls.Add(weekRangeLabel);
        this.Controls.Add(headerArea);

        Button addNewTask = new Button
        {
            Text = $"Add +",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(220, -45),
            AutoSize = true,
            // ForeColor = Color.FromArgb(64,64,64)
        };
        addNewTask.Click += (sender, e) => {addNewTaskForm();};
        headerArea.Controls.Add(addNewTask);
        // this.Controls.Add(addNewTask);

        TableLayoutPanel weekGrid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(20),
            ColumnCount = 7,
            RowCount = 2,
            BackColor = Color.White,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        };
        this.Padding = new Padding(20, 45, 20, 20);

        weekGrid.ColumnStyles.Clear();
        for (int i = 0; i < 7; i++)
        {
            weekGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
        }
        weekGrid.RowStyles.Clear();
        weekGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
        weekGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        string[] days = { "Mon", "Tue", "Wed", "Thu", "Fr", "Sa", "Sun"};

        for (int i = 0; i < 7; i++)
        {
            DateTime currentDay = startOfWeek.AddDays(i);
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Label lblName = new Label
            {
                Text = days[i],
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            Label lblDate = new Label
            {
                Text = currentDay.Day.ToString(),
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Fill,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 12)
            };
            
            if (currentDay == DateTime.Today)
            {
                lblDate.ForeColor = Color.DodgerBlue;
                lblDate.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }

            headerPanel.Controls.Add(lblDate);
            headerPanel.Controls.Add(lblName);
            weekGrid.Controls.Add(headerPanel, i, 0);

            if (i == 0)
            {
                Button leftClick = new Button
                {
                    Text = "left",
                    Size = new Size(45, 30),
                    AutoSize = false,
                    Location = new Point(25, 60)
                };
                this.Controls.Add(leftClick);
            }
        }
        this.Controls.Add(weekGrid);
    }
        private void addNewTaskForm()
    {
        this.Controls.Clear();
        Label title = new Label
        {
            Text = "Add a new task",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(220, 100),
            AutoSize = true
        };

        Label TaskTitleLabel = new Label
        {
            Text = "Name",
            Font = new Font("Segoe UI", 14),
            Location = new Point(220, 150),
            AutoSize = true
        };

        TextBox _taskTitleBox = new TextBox
        {
            Multiline = false,
            Size = new Size(300, 150),
            Location = new Point(300, 150),
            Font = new Font("Segoe UI", 12)
        };

        Label DateLabel = new Label
        {
            Text = "Date",
            Font = new Font("Segoe UI", 14),
            Location = new Point(220, 250),
            AutoSize = true
        };

        DateTimePicker _datePicker = new DateTimePicker
        {
            Location = new Point(220, 350),
            Width = 200,
            Format = DateTimePickerFormat.Short,
        };

        Button AddNewTaskBtn = new Button
        {
            Text = "Add",
            Location = new Point(220,400),
            AutoSize = true
        };

        this.Controls.Add(title);
        this.Controls.Add(TaskTitleLabel);
        this.Controls.Add(_taskTitleBox);
        this.Controls.Add(DateLabel);
        this.Controls.Add(_datePicker);
        this.Controls.Add(AddNewTaskBtn);

        DateTime date =_datePicker.Value;

        AddNewTaskBtn.Click += (sender, e) =>
        {
            if (string.IsNullOrEmpty(_taskTitleBox.Text))
            {
                MessageBox.Show("Please enter a title");
                return;
            }

        TaskItem newTask = new TaskItem(_taskTitleBox.Text, date);
        _tasks.Add(newTask);
        this.Controls.Clear();
        SetupUI();
            
        };
    }
}