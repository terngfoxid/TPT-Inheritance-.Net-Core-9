using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Inheritance_Test.Models;

[Table("Component")]
public partial class Component
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public string? Name { get; set; }

    [InverseProperty("IdNavigation")]
    public virtual Banner? Banner { get; set; }

    [InverseProperty("IdNavigation")]
    public virtual Container? Container { get; set; }

    [InverseProperty("Component")]
    public virtual ICollection<Containing> Containings { get; set; } = new List<Containing>();

    [InverseProperty("IdNavigation")]
    public virtual Textbox? Textbox { get; set; }
}
