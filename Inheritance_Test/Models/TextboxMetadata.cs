using System.ComponentModel.DataAnnotations.Schema;

namespace Inheritance_Test.Models
{
    public partial class Textbox : Component
    {
        public override void SetIdValue()
        {
            Component component = (Component)this;
            this.Id = component.Id;
        }
    }
}
