using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public partial class HouseType
{
    [Display(Name = "類別編號")]
    public int TypeID { get; set; }

    [Display(Name = "房屋類別")]
    public string TypeName { get; set; } = null!;

    public virtual ICollection<House> House { get; set; } = new List<House>();
}
