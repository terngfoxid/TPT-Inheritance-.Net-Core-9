using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Inheritance_Test.Models;

[Table("Page")]
public partial class Page
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public string? Pagename { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("Page")]
    public virtual Container IdNavigation { get; set; } = null!;
}
