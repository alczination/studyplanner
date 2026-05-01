using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace First;

public class Sidebar : Panel
{
    public event EventHandler? CalendarClicked;
    public event EventHandler? TodosClicked;
    public Sidebar()
    {
        this.Dock = DockStyle.Left;
        this.Width = 200;
        this.BackColor = Color.FromArgb(45, 45, 48);
        SetupButtons();
    }
    private void SetupButtons()
    {
        AddButton("Dashboard", DockStyle.Top, (s, e) => { MessageBox.Show("Dashboard click");});
        AddButton("Todo's", DockStyle.Top, (s, e) => TodosClicked?.Invoke(this, EventArgs.Empty));
        AddButton("Calendar", DockStyle.Top, (s, e) => CalendarClicked?.Invoke(this, EventArgs.Empty));
        AddButton("Settings", DockStyle.Bottom, (s, e) => { MessageBox.Show("Ustawienia click"); });
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