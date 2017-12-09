# Project Guide
## SpaceInvaders
### - Managers/ScoreManager
tracking the points gained by each scoring Sprite.

### - Sprites - Concrete Sprite entities
- LivingEntity - a Sprite having Health as a class member which has the ability to loose souls.
- FightingEntity - derived from LivingEntity. a Sprite having Health & ShootingMachine as class members.
- EnemiesMatrix - a data structure used to hold and handle a group of Enemy objects sharing the same behavior.
- BarriersLine - a data structure used to hold and handle a group of Barrier objects sharing the same behavior.
- Bullet - a Sprite representing the weapon used in this game.

### - GameData
- BulletsFactory - a static class providing a Factory-method for generating different Bullet objects aimed for a specific entity: PlayerSpaceShip or Enemy.
- EnemiesFactory - a static class providing a Factory-method for generating different Enemy objects to be placed and handled in an EnemiesMatrix instance.


## Infrastructure
A framework providing the services and modules which will be used by the game.
#### Managers
A set of classes providing the abillity to access and change data, per single frame, regarding user-input, game entities(sprites) collisions, sound effects and active\inactive play-screens.

### Object-Model
#### Animators
- A set of animators classes. Each concrete animator's (such as resizing or rotating animator) goal is to change the properties (such as height or position) of a referecned Sprite, per each frame, for a defined amount of time. By doing so, the animation effect is achieved. Once the animation is done, the referenced Sprite will be reverted to its original form.
