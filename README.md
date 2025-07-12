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
> public partial class Banner:Component

## Forth: Inherit Context from Second Step and Overide Context Files with TPT at OnModelCreating Method.
> public partial class NewContext : GenerateContextFormEFcore

Overide OnModelCreating Method
> protected override void OnModelCreating(ModelBuilder modelBuilder)

Example TPT Config
> modelBuilder.Entity<Component>().UseTptMappingStrategy().ToTable("Component");  
> modelBuilder.Entity<Banner>().ToTable("Banner");

## Fifth: Register Your Custom Context to program.cs builder service.
> builder.Services.AddDbContext<NewContext>();

Good Luck!! Have Fun!!!
