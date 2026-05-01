using System;
using System.Windows.Forms;
using System.Drawing;
namespace First;
public class MyForm : Form
{
    private Sidebar appSidebar;
    private Panel contentArea;
    private List<TaskItem> allTasks = new List<TaskItem>
    {
        new TaskItem("Spotkanie z klientem", DateTime.Today),
        new TaskItem("Trening", DateTime.Today.AddDays(1)),
        new TaskItem("Zakupy", DateTime.Today.AddDays(-1)),
        new TaskItem("Projekt C#", DateTime.Today.AddDays(2))
    };
    public MyForm()
    {
        InitComponents();
    }
    private void InitComponents()
    {
        Text = "Todo App";
        ClientSize = new Size(1400, 750);
        CenterToScreen();
        appSidebar = new Sidebar();
        contentArea = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.LightGray
        };
        appSidebar.CalendarClicked += (s, e) => ShowCalendar();
        appSidebar.TodosClicked += (s, e) => ShowTodos();
        Controls.Add(contentArea);
        Controls.Add(appSidebar);
    }
    private void ShowCalendar()
    {
        contentArea.Controls.Clear();
        CalendarView cal = new CalendarView(this.allTasks);
        contentArea.Controls.Add(cal);
    }

    private void ShowTodos()
    {
        contentArea.Controls.Clear();
        TodoView todoView = new TodoView(this.allTasks);
        contentArea.Controls.Add(todoView);
    }

    [STAThread]
    static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();
        Application.Run(new MyForm());
    }
}