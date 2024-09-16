using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public partial class Member
{
    [DisplayName("會員編號")]
    public int MemberID { get; set; }

    [Display(Name = "會員名稱")]
    public string MemberName { get; set; } = null!;

    [Display(Name = "性別")]
    public bool Gender { get; set; }

    [Display(Name = "電話")]
    public string Phone { get; set; } = null!;

    public virtual ICollection<House> House { get; set; } = new List<House>();
}
