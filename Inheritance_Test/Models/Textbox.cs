using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Inheritance_Test.Models;

[Table("Textbox")]
public partial class Textbox
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public string? Header { get; set; }

    [Column(TypeName = "text")]
    public string? Text { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("Textbox")]
    public virtual Component IdNavigation { get; set; } = null!;
}
