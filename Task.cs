using System;

namespace First;

public class TaskItem
{
    public string Title
    {
        get; set;
    }
    public DateTime Date
    {
        get; set;
    }
    // public Color DisplayColor
    // {
    //     get; set;
    // }
    // private List Category;
    // private List Priority;

    public TaskItem (string title, DateTime date)
    {
        Title = title;
        Date = date;
    }
}