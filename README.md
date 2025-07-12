# This Project is Example TPT Inheritance with EF Core pls see CustomContext and MetadataFiles.

## First: The DB need relation PK of weak entity use same PK of strong entity as FK , All weak entity pls set auto increase ID "off".
> Component's ID is PK

> Banner is Subtype of Component, Banner's ID is PK and FK relate to Component's ID

## Second: Scaffolding reverse Enginner DB to Model Class.
VS Code
> dotnet ef dbcontext scaffold "Connection String" Microsoft.EntityFrameworkCore.SqlServer

Visual Studio
> Scaffold-DbContext 'Connection String' Microsoft.EntityFrameworkCore.SqlServer

## Third: Use partial class for create Inheritance.
> public partial class Banner : Component

## Forth: Inherit Context from Second Step and Overide Context Files with TPT at OnModelCreating Method.
> public partial class NewContext : GenerateContextFormEFcore

Overide OnModelCreating Method.
> protected override void OnModelCreating(ModelBuilder modelBuilder){  
>   .....your overide code  
> }

Example Basic TPT Config.
> modelBuilder.Entity<Component>().UseTptMappingStrategy().ToTable("Component");  
> modelBuilder.Entity<Banner>().ToTable("Banner");

Example Advance TPT Config.  
Some relation need to Add Ignore Config like this.
> modelBuilder.Entity<Component>().UseTptMappingStrategy().ToTable("Component");  
> modelBuilder.Entity<Container>().UseTptMappingStrategy().ToTable("Container");  
> modelBuilder.Entity<Container>().Ignore(c => c.Page);  
> modelBuilder.Entity<Page>().ToTable("Page");

Because
> Container is Inherit from Component.  
> --> public partial class Container : Component  
> Page is Inherit from Container.  
> --> public partial class Page : Container  
> EF core is confuse between (Component)Page.Page and (Component)Page.Container.Page

Example Expert TPT Config.  
Some Many to Many relation between Superclass(Parent) and Subclass(Child) need to Add Ignore Config like this.
> modelBuilder.Entity<Containing>(entity =>  
> {  
>   entity.Ignore(C => C.Container);  
>   entity.HasOne(d => d.Component).WithMany(p => p.Containings).HasConstraintName("FK_Containing_Component");  
> });

Because
> Container is Subtype of Component, but it can containing other Component (Many to Many with Containing Table)  
> EF core is confuse between (Component)Containing.Component and (Component)Containing.Container

## Fifth: Register Your Custom Context to program.cs builder service.
> builder.Services.AddDbContext<NewContext>();

Good Luck!! Have Fun!!!
