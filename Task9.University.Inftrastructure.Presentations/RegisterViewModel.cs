﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task9.University.Infrastructure.Presentations;
public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)] 
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage ="Ther {0} must ber atleast {2} characters long. ", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name ="Confirm Password")]
    [Compare("Password", ErrorMessage = "Password do not match. ")]
    public string ConfirmPassword { get; set; }

    public string? ReturnUrl { get; set; }
}
