using System;
using System.Collections.Generic;

namespace CRMProject.Models;

public partial class Lead
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public string? Property { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public DateTime? Date { get; set; }

    public string? AskingPrice { get; set; }

    public string? TitleCheck { get; set; }

    public string? Area { get; set; }

    public string? Stage { get; set; }

    public string? Remarks { get; set; }
}
