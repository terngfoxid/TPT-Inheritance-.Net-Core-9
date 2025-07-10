using System.ComponentModel.DataAnnotations.Schema;

namespace Inheritance_Test.Models
{
    public partial class Container:Component
    {
        [Column("ID")]
        public int ContainerID { get; set; }
    }

    public partial class Page : Container
    {
        [Column("ID")]
        public int PageID { get; set; }
    }
}
