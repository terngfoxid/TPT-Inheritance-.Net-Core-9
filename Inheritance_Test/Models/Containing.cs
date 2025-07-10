using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Inheritance_Test.Models;

[Table("Containing")]
public partial class Containing
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ContainerID")]
    public int? ContainerId { get; set; }

    [Column("ComponentID")]
    public int? ComponentId { get; set; }

    [ForeignKey("ComponentId")]
    [InverseProperty("Containings")]
    public virtual Component? Component { get; set; }

    [ForeignKey("ContainerId")]
    [InverseProperty("Containings")]
    public virtual Container? Container { get; set; }
}
