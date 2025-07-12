# This Project is Example TPT Inheritance with EF Core pls see CustomContext and MetadataFiles.

## First: The DB need relation PK of weak entity use same PK of strong entity as FK , All weak entity pls set auto increase ID "off".

-----Example-----
> Component have ID is PK
> Banner is Component too, Banner ID is PK and FK to Component ID

## Second: Scaffolding reverse Enginner DB to Model Class.

## Third: Use partial class for create Inheritance.

-----Example-----
> Banner : Component

## Forth: Inherit Context from Second Step and Overide Context Files with TPT on OnModelCreating Method.

-----Example-----
> NewContext : GenerateContextFormEFcore

## Fifth: Register Your Custom Context to program.cs builder service.

-----Example-----
> builder.Services.AddDbContext<NewContext>();

Good Luck!! Have Fun!!!
