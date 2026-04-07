using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace First;

public class Sidebar : Panel
{
    public event EventHandler? CalendarClicked;
    public Sidebar()
    {
        // Dock on the left side
        this.Dock = DockStyle.Left;
        this.Width = 200;
        this.BackColor = Color.FromArgb(45, 45, 48);
        SetupButtons();
    }

// Adding buttons
    private void SetupButtons()
    {
        AddButton("Dashboard", DockStyle.Top, (s, e) => { MessageBox.Show("Dashboard click");});
        AddButton("Zadania", DockStyle.Top, (s, e) => { MessageBox.Show("Zadania click"); });
        AddButton("Kalendarz", DockStyle.Top, (s, e) => CalendarClicked?.Invoke(this, EventArgs.Empty));
        AddButton("Ustawienia", DockStyle.Bottom, (s, e) => { MessageBox.Show("Ustawienia click"); });
    }

    private void AddButton(string text, DockStyle dock, EventHandler onClick)
    {
        Button btn = new Button
        {
            Text = text,
            Height = 50,
            Dock = dock,
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(20, 0, 0, 0),
            Font = new Font("Segoe UI", 10)
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.Click += onClick;
        this.Controls.Add(btn);
    }
}