using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Inheritance_Test.Models;

[Table("Subdetail")]
public partial class Subdetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(10)]
    public string? Sublink { get; set; }

    [StringLength(10)]
    public string? SubCode { get; set; }

    [Column("BannerID")]
    public int BannerId { get; set; }

    [ForeignKey("BannerId")]
    [InverseProperty("Subdetails")]
    public virtual Banner Banner { get; set; } = null!;
}
