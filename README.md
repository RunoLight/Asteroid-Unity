# Asteroids Clone Game - GitHub Repository
Welcome! This repository contains the implementation of a 2D clone of the original Asteroids game with more modern graphics, some VFX. The goal of this game is to score as many points as possible by shooting asteroids and UFOs while avoiding collisions with them.

## Contents

* [Game description](#game-description)
  * [Spaceship Weapons](#spaceship-weapons)
  * [Game Mechanics](#game-mechanics)
* [User Interface (UI)](#user-interface-ui)
* [Development Details](#development-details)
* [Collision Convention](#collision-convention)
* [Game Modules (Assembly Definitions/.asmdef)](#game-modules-assembly-definitionsasmdef)
* [Restrictions](#restrictions)

## Game Description
In this game, the player controls a spaceship that can rotate left and right, move forward, and shoot. The movement of the spaceship includes acceleration and inertia. The screen acts as a portal, allowing the spaceship to appear on the opposite side when it moves beyond the screen boundaries.

### Spaceship Weapons
The spaceship is equipped with two types of weapons:

* Bullets: When bullets hit an asteroid, the asteroid breaks into smaller, faster-moving fragments. Bullets can also destroy these fragments and UFO. Disappear when destroys anything.
* Laser: The laser destroys all objects it collide with. The player has a limited number of laser shots, which replenish over time.

### Game Mechanics
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
* Game Logic and Presentation: Separated via Assembly Definitions to ensure a clean architecture.
* Game Logic Classes are not inherit from MonoBehaviour.
* Input Management: Unity's Input System.

## Collision Convention

Every object detects itself collision with every objects it need to have a collision logic with. There's no middleware that notifies both objects about collision. Collision is detects via Unity Colliders (Movement is not Unity's physics based, see Restrictions).

## Game Modules (Assembly Definitions/.asmdef):

```
Assemblies:

1) Asteroid.Presentation.Abstraction - Abstractions on all presentations/views of the game

2) Asteroid.Presentation - Implementation of presentation/view game part
┣━Asteroid.Presentation.Abstraction
┗━Unity.TextMeshPro

3) Asteroid.Input - Scripts to support input actions & Auto-generated InputSystem input actions
┗━Unity.InputSystem

4) Asteroid.GameLogic - Game Logic implementation, fully dependent on abstractions
┗━Asteroid.Presentation.Abstraction

5) Asteroid.EntryPoint - Entry Point that initializes everything and passes all the dependencies implementations into logic classes
┣━Asteroid.GameLogic
┣━Asteroid.Presentation
┣━Asteroid.Presentation.Abstraction
┗━Asteroid.Input

6) Asteroid.Editor - Editor scripts for displaying readme and github links

Assemblies dependency tree:

Asteroid.EntryPoint
┣━Asteroid.GameLogic
┃ ┗━Asteroid.Presentation.Abstraction
┣━Asteroid.Presentation
┃ ┣━Asteroid.Presentation.Abstraction
┃ ┗━Unity.TextMeshPro
┣━Asteroid.Presentation.Abstraction
┗━Asteroid.Input
  ┗━Unity.InputSystem
Asteroid.Editor
```

## Restrictions:
* Do not use third-party frameworks.
* No Singleton pattern.
* Do not use preview or experimental versions of Unity packages.
* Do not use Unity's physics system for object movement.