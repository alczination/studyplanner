using System;
using System.Windows.Forms;
using System.Drawing;

namespace First;

public class MyForm : Form
{
    private Sidebar appSidebar;
    private Panel contentArea;

    public MyForm()
    {
        InitComponents();
    }
    private void InitComponents()
    {
        this.Text = "Todo App";
        this.ClientSize = new Size(1000, 600);
        this.CenterToScreen();

        appSidebar = new Sidebar();

        contentArea = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.LightGray
        };

        appSidebar.CalendarClicked += (s, e) => ShowCalendar();
        this.Controls.Add(contentArea);
        this.Controls.Add(appSidebar);
    }

    private void ShowCalendar()
    {
        contentArea.Controls.Clear();
        CalendarView cal = new CalendarView();
        contentArea.Controls.Add(cal);
    }

    [STAThread]
    static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MyForm());
    }
}