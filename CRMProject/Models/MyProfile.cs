using System;
using System.Collections.Generic;

namespace CRMProject.Models;

public partial class MyProfile
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? ProfileUrl { get; set; }

    public string Password { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime TokenCreated { get; set; }

    public DateTime TokenExpires { get; set; }
}
