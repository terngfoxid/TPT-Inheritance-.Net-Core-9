This Project is Example TPT Inheritance with EF Core pls see CustomContext and MetadataFiles.

First: The DB need relation PK of weak entity use same PK of strong entity as FK , All weak entity pls set auto increase ID "off".

Second: Scaffolding reverse Enginner DB to Model Class.

Third: use partial class for create Inheritance.

Forth: Inherit Context from Second Step and Overide Context Files with TPT on OnModelCreating Method.

Fifth: Register Your Custom Context to program.cs builder service.

Good Luck!! Have Fun!!!
