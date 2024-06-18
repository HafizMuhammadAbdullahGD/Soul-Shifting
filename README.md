Mechanics Design Document: Soul Shifting
Document Overview
This document outlines the mechanics for the new game being developed, focusing on Movement, attack mechanics, health mechanics, and soul mechanics. It details how players interact with creatures through attacks, manage health, and utilize the unique soul-shifting ability to control other creatures.
Game overview
"Soul Shifter" is an innovative action-adventure game where players embody a soul that must possess various creatures to survive. The core mechanic involves weakening creatures through attacks, reducing their health to a specific threshold to enable soul possession.
Requirements
Technical Requirements
Platform:
PC
Game Engine: 
Unity latest version

Gameplay requirements
Creatures
Unity Models
-Utilize two default free Unity models to represent the creatures at the start. These models will be used as placeholders during early testing phases and can be customized or replaced with original models in later stages of development.
Creature & Soul Movement
-Key Bindings: Players use the W, A, S, and D keys to navigate creatures on the battlefield. This control scheme is familiar to most players and ensures intuitive movement.

Camera Control
-The camera smoothly follows the character that the player currently controls. The camera’s motion is designed to be fluid and responsive without causing disorientation or sudden jolts.

Attack Mechanics
Attack Animation
- Type: Simple Attack
- Execution: Attack animation is triggered when the player is within hitting distance of the target creature.
- Resource: Any free-to-use single attack animation will do. For example, you can use the Punch Animation by DFLEX or the Rapid Punching Animation by BPoole.
- Attack Cooldown: Wait 2 seconds between attacks.

Attack Range
- Distance: The effective range within which the attack can connect with the target, known as the hitting distance.

Attack Damage
- Damage Output: Each successful attack reduces the target creature's health by 10 HP.

Attack Key
- Key Binding: Press the left mouse button to perform an attack.

Health Mechanics
Starting Health
- Initial HP: Every creature starts with 100 HP.

Health Reduction
- Damage Calculation: Health decreases based on the number of successful attacks (e.g., 10 attacks reduce health by 100 HP).

Death and Respawn
- Zero HP Condition: If a creature's health reaches 0 HP, the creature dies.
- Respawn Mechanism: Upon death, the creature reappears at a predefined default location with full health restored (100 HP).

Soul Mechanics
Overview
- The soul represents the player, who needs to possess bodies to survive. The soul is depicted as a sphere and can move in any direction.

Form
- The soul will be represented by a sphere.

Movement
- The soul can move in any direction. 



Default State
- The player starts as the soul.

Attack
- All the attack mechanics of a creature except attack animation:
  - Attack Range: The effective range within which the attack can connect with the target, known as the hitting distance.
  - Attack Damage: Each successful attack reduces the target creature's health by 10 HP.
  - Attack Key: Press the left mouse button to perform an attack.
  - Attack Cooldown: Wait 2 seconds between attacks (no animation needed for the soul).

Possessing a Creature
- When conditions allow (target creature's health between 10 HP and 30 HP), pressing the 'E' key initiates auto-movement of the soul sphere into the center of the creature, making the soul invisible.
- Once the possession is complete, the player controls the creature.

Exiting a Body
- Press and hold the 'E' key for 5 seconds to exit the body. The soul will automatically move out from the creature's chest, and the creature will die.

Soul Health
- Initial HP: The soul starts with 100 HP.
- Health Decrease: When not possessing a body, the soul's HP decreases at a rate of 5 HP every second.
- Death and Respawn: If the soul's HP reaches 0, it dies and respawns at a default location.
Deliverables
Unity Project:
A complete Unity project containing all assets, scripts, and scenes required for the game "Soul Shifter".
Well-organized project structure with clear documentation on how to build and run the game.
Source code with comments and documentation explaining key components and functionalities.
Gameplay Video:

A high-quality video demonstrating the core gameplay mechanics, including:
Initial state as the soul.
Attacking creatures and reducing their health.
Possessing creatures once their health is lowered.
Exiting possessed bodies and returning to the soul state.
The video should be 5-10 minutes long, showcasing smooth transitions and responsive controls.

GOOD LUCK!



