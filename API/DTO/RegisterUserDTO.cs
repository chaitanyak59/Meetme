using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class RegisterUserDTO
{
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
    public required string UserName { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public required string Password { get; set; }

}
