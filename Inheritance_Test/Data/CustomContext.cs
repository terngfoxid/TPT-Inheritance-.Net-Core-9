using Inheritance_Test.Models;
using Microsoft.EntityFrameworkCore;

namespace Inheritance_Test.Data
{
    public partial class CustomContext:InheritanceTestContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Overide With TPT
            modelBuilder.Entity<Component>().UseTptMappingStrategy().ToTable("Component");

            modelBuilder.Entity<Banner>().ToTable("Banner");
            modelBuilder.Entity<Banner>().HasBaseType<Component>();

            modelBuilder.Entity<Textbox>().ToTable("Textbox");
            modelBuilder.Entity<Textbox>().HasBaseType<Component>();

            modelBuilder.Entity<Container>().UseTptMappingStrategy().ToTable("Container");
            modelBuilder.Entity<Container>().HasBaseType<Component>();
            modelBuilder.Entity<Container>().Ignore(c => c.Page);

            modelBuilder.Entity<Page>().ToTable("Page");
            modelBuilder.Entity<Page>().HasBaseType<Container>();


            //Overide Many to Many Relation
            modelBuilder.Entity<Containing>(entity =>
            {
                entity.Ignore(C => C.Container);

                entity.HasOne(d => d.Component).WithMany(p => p.Containings).HasConstraintName("FK_Containing_Component");
            });

            modelBuilder.Entity<Subdetail>(entity =>
            {
                entity.Property(e => e.SubCode).IsFixedLength();
                entity.Property(e => e.Sublink).IsFixedLength();

                entity.HasOne(d => d.Banner).WithMany(p => p.Subdetails)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subdetail_Banner");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
