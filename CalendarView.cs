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
    private int _weekOffset = 0;
    private int difference
    {
        get
        {
            DateTime referenceDate = DateTime.Today.AddDays(_weekOffset * 7);
            return (7 + (referenceDate.DayOfWeek - DayOfWeek.Monday)) % 7;
        }
    }
    public CalendarView(List<TaskItem> mainList)
    {
        _tasks = mainList;
        Dock = DockStyle.Fill;
        BackColor = Color.FromArgb(240, 240, 240);
        SetupUI();
    }
    private void SetupUI()
    {
            Controls.Clear();
            DateTime referenceDate = DateTime.Today.AddDays(_weekOffset * 7);
            DateTime startOfWeek = referenceDate.AddDays(-1 * difference).Date;
            DateTime endOfWeek = startOfWeek.AddDays(6);

        // Header
            _headerArea = new Panel 
            {
                Dock = DockStyle.Top,
                Height = 38,
                BackColor = Color.Gray,
                Padding = new Padding(0,5,0,0)
            };

        // Week range counter
            _weekRangeLabel = new Label
            {
                Text = $"{startOfWeek:dd.MM} - {endOfWeek:dd.MM}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(40, 30),
                AutoSize = true,
                Dock = DockStyle.Left,
            };
            _headerArea.Controls.Add(_weekRangeLabel);

        // Add-new-task button
            Button addNewTask = new Button 
            {
                Text = $"Add +",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(_weekRangeLabel.Right + 30, 2),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.DodgerBlue
            };
            addNewTask.Click += (sender, e) => {AddNewTask();};
            _headerArea.Controls.Add(addNewTask);
            Controls.Add(addNewTask);
      
        // Today button
            Button today = new Button
            {
                Text = $"Today",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(addNewTask.Right + 20, 2),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Green
            };
            today.Click += (sender, e) => {Carousel(0);};
            _headerArea.Controls.Add(today);
            Controls.Add(today);
            Controls.Add(_headerArea);

        // Week Grid
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
                    Dock = DockStyle.Top,
                    Height = 25,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };

                Label lblDate = new Label
                {
                    Text = currentDay.Day.ToString(),
                    Dock = DockStyle.Fill,
                    ForeColor = Color.Gray,
                    Font = new Font("Segoe UI", 12)
                };
                    // Current Day Marker
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
                    WrapContents = false
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
                            Width = tasksContainer.Width - 50,
                            Margin = new Padding(5),
                        };
                        tasksContainer.Controls.Add(taskLabel);
                        taskLabel.Click += (sender, e) =>
                        {
                            EditTask(task);
                        };
                    }
                }
            _weekGrid.Controls.Add(tasksContainer, i, 1);
            // Left button
                if (i == 0)
                {
                    Button leftButton = new Button
                    {
                        Text = "<",
                        Size = new Size(45, 30),
                        AutoSize = false,
                        Location = new Point(25, 60),
                        FlatStyle = FlatStyle.Flat
                    };
                    Controls.Add(leftButton);
                    leftButton.Click += (sender, e) => {Carousel(-1);};
                }

            // Right button
                if (i == 5)
                {
                    Button rightButton = new Button
                    {
                        Text = ">",
                        Size = new Size(45, 30),
                        AutoSize = false,
                        Location = new Point(1130, 60),
                        FlatStyle = FlatStyle.Flat
                    };
                    Controls.Add(rightButton);
                    rightButton.Click += (sender, e) => {Carousel(1);};
                }
            }
            Controls.Add(_weekGrid);
            // _weekGrid.BringToFront();
    }
    private void AddNewTask()
    {
        Controls.Clear();
        Label TitleAddLabel = new Label
        {
            Text = "Add a new task",
            Font = new Font("Segoe UI", 17, FontStyle.Bold),
            Location = new Point(60, 30),
            AutoSize = true
        };

        Label TaskTitleLabel = new Label
        {
            Text = "Name",
            Font = new Font("Segoe UI", 14),
            Location = new Point(70, 70),
            AutoSize = true
        };

        TextBox _taskTitleBox = new TextBox
        {
            Multiline = false,
            Size = new Size(300, 150),
            Location = new Point(150, 70),
            Font = new Font("Segoe UI", 12)
        };

        Label DateLabel = new Label
        {
            Text = "Date",
            Font = new Font("Segoe UI", 14),
            Location = new Point(70, 120),
            AutoSize = true
        };

        DateTimePicker _datePicker = new DateTimePicker
        {
            Font = new Font("Segoe UI", 14),
            Location = new Point(150, 120),
            Width = 200,
            Format = DateTimePickerFormat.Short,
        };

        Button AddNewTaskBtn = new Button
        {
            Text = "Add",
            Location = new Point(150,400),
            AutoSize = true
        };

        Button CloseTabBtn = new Button
        {
            Text = "Close",
            Location = new Point(250, 400),
            AutoSize = true
        };

        Controls.Add(TitleAddLabel);
        Controls.Add(TaskTitleLabel);
        Controls.Add(_taskTitleBox);
        Controls.Add(DateLabel);
        Controls.Add(_datePicker);
        Controls.Add(AddNewTaskBtn);
        Controls.Add(CloseTabBtn);
        DateTime date =_datePicker.Value;

        AddNewTaskBtn.Click += (sender, e) =>
        {
            if (string.IsNullOrEmpty(_taskTitleBox.Text))
            {
                MessageBox.Show("Please enter a title");
                return;
            }

        DateTime selectedDate = _datePicker.Value.Date;
        TaskItem newTask = new TaskItem(_taskTitleBox.Text, selectedDate);
        _tasks.Add(newTask);
        SetupUI();
        };

        CloseTabBtn.Click += (sender, e) =>
        {
            SetupUI();
        };
     }
    public void EditTask(TaskItem item)
    {
        Controls.Clear();

        Label TitleEditLabel = new Label
        {
            Text = "Edit task",
            Location = new Point(60, 30),
            Font = new Font("Segoe UI", 17, FontStyle.Bold),
            AutoSize = true 
        };

        Label TitleLabel = new Label
        {
            Text = "Task name:",
            Location = new Point(60, 80),
            Font = new Font("Segoe UI", 14),
            AutoSize = true
        };

        TextBox Name = new TextBox
        {
            Text = item.Title,
            Location = new Point(200, 80),
            Size = new Size(200, 20),
            AutoSize = true
        };

        Label DateLabel = new Label
        {
            Text = "Date",
            Font = new Font("Segoe UI", 14),
            Location = new Point(90, 120)
        };

        DateTimePicker Date = new DateTimePicker
        {
            Value = item.Date,
            Font = new Font("Segoe UI", 14),
            Location = new Point(200, 120),
            Width = 200,
            Format = DateTimePickerFormat.Short,
        };

        Button SaveBtn = new Button
        {
            Text = "Save",
            Location = new Point(200, 400),
            AutoSize = true
        };

        Button CloseTabBtn = new Button
        {
            Text = "Close",
            Location = new Point(300, 400),
            AutoSize = true
        };

        Button DeleteBtn = new Button
        {
            Text = "Delete",
            Location = new Point (400, 400),
            AutoSize = true
        };

        Controls.Add(TitleEditLabel);
        Controls.Add(TitleLabel);
        Controls.Add(Name);
        Controls.Add(DateLabel);
        Controls.Add(Date);
        Controls.Add(SaveBtn);
        Controls.Add(CloseTabBtn);
        Controls.Add(DeleteBtn);
        
        SaveBtn.Click += (sender, e) =>
        {
            if (string.IsNullOrWhiteSpace(Name.Text))
            {
                MessageBox.Show("Please enter a title");
                return;
            }
            item.Title = Name.Text;
            item.Date = Date.Value.Date;
            SetupUI();
        };
        CloseTabBtn.Click += (sender, e) => {SetupUI();};
        DeleteBtn.Click += (sender, e) =>
        {
            _tasks.Remove(item);
            SetupUI();
        };
  
    }
    private void Carousel(int offset)
    {
        if (offset == 1)
        {
            _weekOffset++;
        }
      
        else if (offset == -1)
        {
            _weekOffset--;
        }

        else if (offset == 0)
        {
            _weekOffset = 0;
        }
    
        SetupUI();
    }
}