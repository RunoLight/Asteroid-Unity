# Asteroids Clone Game - GitHub Repository
Welcome! This repository contains the implementation of a 2D clone of the original Asteroids game with more modern graphics, some VFX. The goal of this game is to score as many points as possible by shooting asteroids and UFOs while avoiding collisions with them.

## Game Description
In this game, the player controls a spaceship that can rotate left and right, move forward, and shoot. The movement of the spaceship includes acceleration and inertia. The screen acts as a portal, allowing the spaceship to appear on the opposite side when it moves beyond the screen boundaries.

## Spaceship Weapons
The spaceship is equipped with two types of weapons:

* Bullets: When bullets hit an asteroid, the asteroid breaks into smaller, faster-moving fragments. Bullets can also destroy these fragments and UFO. Disappear when destroys anything.
* Laser: The laser destroys all objects it collide with. The player has a limited number of laser shots, which replenish over time.

## Game Mechanics
Colliding with an asteroid, fragment, or UFO results in a game-over message displaying the player's score and an invitation to start a new game.
Asteroids and UFOs periodically appear after the game starts. Asteroids move in random directions, while UFOs chase the player. Asteroids and UFOs do not collide with each other.

## User Interface (UI)
The game UI displays the following information about the spaceship:

* Coordinates
* Rotation angle
* Instantaneous speed (Velocity)
* Number of laser charges
* Laser cooldown time

## Development Details
* Programming Language: C#
* Project Setup: Unity 2022.3.25f1, Target Platform: Windows
* Game Logic and Presentation: Separated to ensure a clean architecture.
* Game Logic Classes: Should not inherit from MonoBehaviour.
* Assembly Definitions: Must be used.
* Input Management: Utilize Unity's Input System.
* Assets: Any assets for UI and graphics can be used, as their quality is not considered in the evaluation.

## Collision Convention

Every object detects itself collision with every objects it need to have a collision logic with. There's no middleware that notifies both objects about collision. Collision is detects via Unity Colliders (Movement is not Unity's physics based, see Restrictions).

## Restrictions:
* Do not use third-party frameworks.
* Avoid Singleton pattern.
* Do not use preview or experimental versions of Unity packages.
* Do not use Unity's physics system for object movement.

## Evaluation Criteria
The test task will be evaluated based on:
* Understanding of object-oriented programming.
* Appropriate use of design patterns.
* Code style and clarity.
