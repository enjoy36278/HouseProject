using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public partial class Login
{
    [Display(Name = "帳號")]
    [Required(ErrorMessage = "請輸入帳號")]
    [StringLength(10, ErrorMessage = "帳號最多10碼")]
    public string Account { get; set; } = null!;

    [Display(Name = "密碼")]
    [Required(ErrorMessage = "請輸入密碼")]
    [StringLength(12, ErrorMessage = "密碼最多12碼")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
