
using System.ComponentModel.DataAnnotations.Schema;

namespace Inheritance_Test.Models
{
    public partial class Banner:Component
    {
        [Column("ID")]
        public int BannerID { get; set; }
    }
}
