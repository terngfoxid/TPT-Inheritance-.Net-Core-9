using Inheritance_Test.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inheritance_Test.Models
{
    public partial class Component
    {
        protected Component() {
            this.Type = this.GetType().FullName;
        }

        private CustomContext db = new CustomContext();
        public Component create()
        {
            db.Add(this);
            db.SaveChanges();
            SetIdValue();
            return this;
        }

        public Component update()
        {
            db.Update(this);
            db.SaveChanges();
            SetIdValue();
            return this;
        }

        public virtual void SetIdValue()
        {
            Component component = (Component)this;
            this.Id = component.Id;
        }
    }
}
