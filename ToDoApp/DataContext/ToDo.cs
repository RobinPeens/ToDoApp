using System;
using System.Collections.Generic;

namespace ToDoApp.DataContext;

public partial class ToDo
{
    public int ToDoId { get; set; }

    public int StatusId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Notes { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public DateTime? ViewedDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Status Status { get; set; } = null!;
}
