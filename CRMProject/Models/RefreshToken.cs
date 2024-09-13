using System;
using System.Collections.Generic;

namespace CRMProject.Models;

public partial class RefreshToken
{
    public string Token { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Expires { get; set; }
}
