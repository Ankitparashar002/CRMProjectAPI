using System;
using System.Collections.Generic;

namespace CRMProject.Models;

public partial class MyProfile
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string Mobile { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? ProfileUrl { get; set; }

    public string Password { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public DateTime TokenCreated { get; set; }

    public DateTime TokenExpires { get; set; }
}
