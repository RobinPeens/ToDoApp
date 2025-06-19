using System;
using System.Collections.Generic;

namespace ToDoApp.DataContext;

public partial class Status
{
    public int StatusId { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<ToDo> ToDos { get; set; } = new List<ToDo>();
}
