using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public partial class House
{
    [Display(Name = "房屋編號")]
    public int HouseID { get; set; }

    [Display(Name = "房屋名稱")]
    public string HouseName { get; set; } = null!;

    [Display(Name = "坪數")]
    public float Square { get; set; }

    [Display(Name = "租金")]
    public int Rent { get; set; }

    public int CityID { get; set; }

    [Display(Name = "區域")]
    public string District { get; set; } = null!;

    [Display(Name = "地址")]
    public string Address { get; set; } = null!;

    [DataType(DataType.MultilineText)]
    [Display(Name = "說明")]
    public string? Note { get; set; }

    public int TypeID { get; set; }

    public int MemberID { get; set; }

    public string? PhotoName { get; set; }

    public virtual City? City { get; set; } = null!;

    public virtual Member? Member { get; set; } = null!;

    public virtual HouseType? Type { get; set; } = null!;
}
