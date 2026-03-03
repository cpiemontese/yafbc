# YAFBC

YAFBC is a small Flappy Bird-style game built with Unity.

The goal of this project was to structure a small end-to-end system that remains readable, extensible and easy to reason about, while implementing the core Flappy Bird game loop in an efficient way.

## Architecture Overview

The core game loop is orchestrated by [`CameraController`](./Assets/Scripts/CameraController.cs), which:

- Manages time-based obstacle spawning
- Reuses tiles by repositioning them instead of recreating objects
- Coordinates obstacle lifecycle through a queue structure
- Handles world reset and cleanup events

Obstacle generation logic lives in [`ObstacleGeneratorController`](./Assets/Scripts/ObstacleGeneratorController.cs), which is responsible for:

- Procedural obstacle creation
- Movement coordination
- Maintaining a clear separation between generation and orchestration

[`PlayerController`](./Assets/Scripts/PlayerController.cs) focuses strictly on input handling and timing logic.

## Design Choices & Trade-offs

This is a small personal project, so some trade-offs were made intentionally:

- A central orchestrator (`CameraController`) was preferred over further abstraction to keep iteration fast
- Unity hierarchy assumptions are used pragmatically for simplicity
- Object reuse is implemented to avoid unnecessary instantiation during gameplay

If this were a larger collaborative project, I would introduce some improvements, starting from refactoring the "god object" of `CameraController`.

## Assets

The tileset was made by [safwyl](https://safwyl.itch.io/) and can be found [here](https://safwyl.itch.io/oubliette-tileset).
