using System;
using System.Collections.Generic;

namespace CRMProject.Models;

public partial class CustomerInventoryListing
{
    public int Ciid { get; set; }

    public int InventoryId { get; set; }

    public int CustomerId { get; set; }
}
