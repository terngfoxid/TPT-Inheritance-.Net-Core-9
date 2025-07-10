using System.ComponentModel.DataAnnotations.Schema;

namespace Inheritance_Test.Models
{
    public partial class Textbox : Component
    {
        [Column("ID")]
        public int TextboxID { get; set; }
    }
}
