# BlueprintsEverywhere v0.2
Mod for Rimworld (Alpha 13)

Requires Community Core Library (v0.13.1.1 or greater)

Currently allows wall blueprints to be placed on stone and undiscovered cells. Colonists will build these walls if they can, however, prison rooms will expand wherever possible when you modify their walls so be careful.

See [Demo.webm](https://cdn.rawgit.com/lost-RD/BlueprintsEverywhere/master/Demo.webm) for a quick video demonstration.

No longer modifies classes in a super lazy way but definitely not made with mod compatibility in mind. Custom stone types or ores will not be handled by this mod because I have manually specified which things can have wall blueprints placed on them. If you know how to solve this, get in touch.

##Plans:

* Work done on wall should occur without a blueprint such that the cell is never anything less than full. Possibly could use the plant cut method (suggested by RawCode).

* Load the list of things that walls can be designated onto from ThingDefs to make the mod compatible with XML changes.