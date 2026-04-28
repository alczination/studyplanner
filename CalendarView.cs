using System;
using System.Drawing;
using System.Drawing.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Label = System.Windows.Forms.Label;

namespace First;

public class CalendarView : Panel
{
    private TableLayoutPanel _weekGrid;
    private Panel _headerArea;
    private Label _weekRangeLabel;
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
        this.Controls.Clear();
        DateTime today = DateTime.Today;
        int difference = (5 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        DateTime startOfWeek = today.AddDays(-1 * difference).Date;
        DateTime endOfWeek = startOfWeek.AddDays(6);

        _headerArea = new Panel // Panel gorny
        {
            Dock = DockStyle.Top,
            Height = 38,
            BackColor = Color.Gray,
            Padding = new Padding(0,5,0,0)
        };

        Label weekRangeLabel = new Label
        {
            Text = $"{startOfWeek:dd.MM} - {endOfWeek:dd.MM}",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(40, 30),
            AutoSize = true,
            Dock = DockStyle.Left,
        };

        _headerArea.Controls.Add(weekRangeLabel);

        Button addNewTask = new Button // Add-Button
        {
            Text = $"Add +",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(weekRangeLabel.Right + 30, 2),
            Size = new Size(100, 35),
            FlatStyle = FlatStyle.Flat,
            // Dock = DockStyle.Right,
            BackColor = Color.DodgerBlue
        };
        addNewTask.Click += (sender, e) => {addNewTaskForm();};

        _headerArea.Controls.Add(addNewTask);
        this.Controls.Add(addNewTask);
        this.Controls.Add(_headerArea);

        _weekGrid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10, 40, 10, 20),
            ColumnCount = 7,
            RowCount = 2,
            BackColor = Color.White,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        };

        for (int i = 0; i < 7; i++)
        {
            _weekGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
        }

        _weekGrid.RowStyles.Clear();
        _weekGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
        _weekGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

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
                // TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            Label lblDate = new Label
            {
                Text = currentDay.Day.ToString(),
                // TextAlign = ContentAlignment.TopCenter,
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
            _weekGrid.Controls.Add(headerPanel, i, 0);

            FlowLayoutPanel tasksContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
            };

            foreach (var task in _tasks)
            {
                if (task.Date.Date == currentDay.Date)
                {
                    Label taskLabel = new Label
                    {
                        Text = task.Title,
                        BackColor = Color.LightBlue,
                        ForeColor = Color.Black,
                        AutoSize = true,
                    };
                    tasksContainer.Controls.Add(taskLabel);
                }
            }

        _weekGrid.Controls.Add(tasksContainer, i, 1);

            if (i == 0)
            {
                Button leftClick = new Button
                {
                    Text = "<",
                    Size = new Size(45, 30),
                    AutoSize = false,
                    Location = new Point(25, 60)
                };
                this.Controls.Add(leftClick);
            }

            // if (i == 5)
            // {
            //     Button rightClick = new Button
            //     {
            //         Text = ">",
            //         Size = new Size(45, 30),
            //         AutoSize = false,
            //         Location = new Point(150, 60)
            //     };
            //     this.Controls.Add(rightClick);
            // }
        }
        this.Controls.Add(_weekGrid);
       _weekGrid.BringToFront();
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

        Button CloseTabBtn = new Button
        {
            Text = "X",
            Location = new Point(800, 200),
            AutoSize = true
        };

        this.Controls.Add(title);
        this.Controls.Add(TaskTitleLabel);
        this.Controls.Add(_taskTitleBox);
        this.Controls.Add(DateLabel);
        this.Controls.Add(_datePicker);
        this.Controls.Add(AddNewTaskBtn);
        this.Controls.Add(CloseTabBtn);

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
        SetupUI();
            
        };

        CloseTabBtn.Click += (sender, e) =>
        {
            SetupUI();
        };
     }
}