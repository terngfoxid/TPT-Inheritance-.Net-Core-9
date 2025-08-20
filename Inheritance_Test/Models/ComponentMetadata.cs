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
        public void create()
        {
            db.Add(this);
            db.SaveChanges();
            dynamic result = this;
            result.Id = this.Id;
        }

        public void update()
        {
            db.Update(this);
            db.SaveChanges();
            dynamic result = this;
            result.Id = this.Id;
        }
    }
}
