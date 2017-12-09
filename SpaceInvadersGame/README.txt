## Folders Guide
### Infrastructure
A framework providing the services and modules which will be used by the game.
#### Managers
A set of classes providing the abillity to access and change data, per single frame, regarding user-input, game entities(sprites) collisions, sound effects and active\inactive play-screens.

### Object-Model & Animators
- Game entities classes (such as Sprite & CollidableSprite) which define the basic behavior of game objects in that framework.\
- A set of animators classes. Each concrete animator's (such as resizing or rotating animator) goal is to change the properties (such a height or position) of a referecned Sprite, per EACH FRAME, for a defined amount of time, and buy thus the animation effect is achieved. Once the animation is done, the referenced Sprite will be reverted to its original form.

