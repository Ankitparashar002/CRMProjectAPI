using System;
using System.Collections.Generic;

namespace CRMProject.Models;

public partial class TaskDashboard
{
    public int Id { get; set; }

    public string Task { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? AssignTo { get; set; }

    public string? Labels { get; set; }

    public string? Date { get; set; }

    public string? Note { get; set; }
}
