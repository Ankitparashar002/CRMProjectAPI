using System;
using System.Collections.Generic;

namespace CRMProject.Models;

public partial class Task
{
    public int Id { get; set; }

    public string Task1 { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? AssignTo { get; set; }

    public string? Labels { get; set; }

    public string Date { get; set; } = null!;

    public string? Note { get; set; }
}
