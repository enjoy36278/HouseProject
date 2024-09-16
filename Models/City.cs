using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public partial class City
{
    [Display(Name = "縣市編號")]
    public int CityID { get; set; }

    [Display(Name = "縣市名稱")]
    public string CityName { get; set; } = null!;

    public virtual ICollection<House> House { get; set; } = new List<House>();
}
