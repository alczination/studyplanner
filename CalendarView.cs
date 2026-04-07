using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

using Label = System.Windows.Forms.Label;

namespace First;

public class CalendarView : Panel
{

    private TableLayoutPanel weekGrid;
    private Label weekRangeLabel;
    private TableLayoutPanel monthGrid;
    private TableLayoutPanel dayGrid;
    public CalendarView()
    {
        this.Dock = DockStyle.Fill;
        this.BackColor = Color.FromArgb(240, 240, 240);
        SetupUI();
    }

    private void SetupUI()
    {
        // Label title = new Label
        // {
        //     Text = "My Calendar",
        //     Font = new Font("Segoe UI", 16, FontStyle.Bold),
        //     Location = new Point(20, 10),
        //     AutoSize = true
        // };

        // this.Controls.Add(title);

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

        weekRangeLabel = new Label
        {
            Text = $"{startOfWeek:dd.MM} - {endOfWeek:dd.MM}",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(20, 15),
            AutoSize = true,
            ForeColor = Color.FromArgb(64, 64, 64)
        };
        headerArea.Controls.Add(weekRangeLabel);
        this.Controls.Add(weekRangeLabel);

        weekGrid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(20),
            ColumnCount = 7,
            RowCount = 2,
            BackColor = Color.White,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        };

        this.Padding = new Padding(20, 0, 20, 20);

        weekGrid.ColumnStyles.Clear();
        for (int i = 0; i < 5; i++)
        {
            weekGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
        }
        weekGrid.RowStyles.Clear();
        weekGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
        weekGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        string[] days = { "Mon", "Tue", "Wed", "Thu", "Fr"};

        for (int i = 0; i < 5; i++)
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
        }



        this.Controls.Add(weekGrid);
    }
}