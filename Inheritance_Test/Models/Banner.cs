using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Inheritance_Test.Models;

[Table("Banner")]
public partial class Banner
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Image_URL")]
    public string? ImageUrl { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("Banner")]
    public virtual Component IdNavigation { get; set; } = null!;

    [InverseProperty("Banner")]
    public virtual ICollection<Subdetail> Subdetails { get; set; } = new List<Subdetail>();
}
