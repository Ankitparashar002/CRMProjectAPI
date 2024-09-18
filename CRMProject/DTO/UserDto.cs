﻿using System.ComponentModel.DataAnnotations;

namespace CRMProject.DTO;

public class UserDto
{
    public string? Email { get; set; }
    public string Password { get; set; } = null!;
}


