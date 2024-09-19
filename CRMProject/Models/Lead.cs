﻿using System;
using System.Collections.Generic;

namespace CRMProject.Models;

public partial class Lead
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string Property { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public long Mobile { get; set; }

    public string Location { get; set; } = null!;

    public string Date { get; set; } = null!;

    public long AskingPrice { get; set; }

    public string TitleCheck { get; set; } = null!;

    public long Area { get; set; }

    public string Stage { get; set; } = null!;

    public string? Remarks { get; set; }

    public int InventoryId { get; set; }

    public virtual Inventory Inventory { get; set; } = null!;
}
