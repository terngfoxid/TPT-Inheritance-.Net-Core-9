This Project is Example TPT Inheritance with EF Core pls see CustomContext and MetadataFiles.

First: The DB need relation PK of weak entity use same PK of strong entity as FK , All weak entity pls set auto increase ID "off".

-----Example-----\n
Component have ID is PK\n
Banner is Component too, Banner ID is PK and FK to Component ID\n

Second: Scaffolding reverse Enginner DB to Model Class.

Third: Use partial class for create Inheritance.

-----Example-----\n
Coding like this -> Banner : Component\n

Forth: Inherit Context from Second Step and Overide Context Files with TPT on OnModelCreating Method.

-----Example-----\n
Coding like this -> NewContext : GenerateContextFormEFcore\n

Fifth: Register Your Custom Context to program.cs builder service.

-----Example-----\n
builder.Services.AddDbContext<NewContext>();\n

Good Luck!! Have Fun!!!
