using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First;
    public class TodoView : Panel
    {
    private List<TaskItem> _tasks;
    private Label _title;
    public TodoView(List<TaskItem> mainList)
    {
        _tasks = mainList;
        Dock = DockStyle.Fill;
        BackColor = Color.FromArgb(240, 240, 240);
        RefreshList();
    }
    private void SetupUI()
    {
        Controls.Clear();
        _title = new Label
        {
            Text = "All tasks",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true
        };
        Controls.Add(_title);
            FlowLayoutPanel tasksContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Padding = new Padding(10,50,0,0)
            };

        DateTime? lastDate = null;
        var sortedTasks = _tasks.OrderBy(item => item.Date).ToList();
        foreach (var item in sortedTasks)
        {
            if (lastDate == null || item.Date.Date != lastDate.Value.Date) 
            {
            string headerText = item.Date.Date == DateTime.Today ? "Today tasks" : item.Date.ToShortDateString();

            Label dateHeader = new Label
            {
                Text = headerText,
                BackColor = Color.Navy,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 20, 0, 5)
            };
            tasksContainer.Controls.Add(dateHeader);
            lastDate = item.Date;
            };

            Label taskLabel = new Label
            {
                Text = $"{item.Title} - {item.Date.ToShortDateString()}",
                BackColor = Color.LightBlue,
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 10)
            };
            tasksContainer.Controls.Add(taskLabel);
            taskLabel.Click += (sender, e) =>
            {
                Controls.Clear();
                CalendarView view = new CalendarView(_tasks);
                view.Dock = DockStyle.Fill;
                Controls.Add(view);
                view.EditTask(item);
            };
        }
        
        if (!Controls.Contains(tasksContainer))
        {
            Controls.Add(tasksContainer);
        }
    }
    public void RefreshList()
    {
        SetupUI();
    }
}