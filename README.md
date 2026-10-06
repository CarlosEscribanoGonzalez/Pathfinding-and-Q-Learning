## Overview
Unity project (C#) with two AI practices: reinforcement learning with **Q-Learning** and pathfinding with **A\*** and a **Horizon Search**.

## Features
**Q-Learning: agent that escapes from an opponent**
* Configurable learning rate, discount factor and exploration rate
* Separate scenes for training and testing
* Learned Q-table saved to a file and loaded in the testing scene
* States are abstracted from the map (nearby walls and relative position of the opponent), so the agent performs well on maps it has never seen

**Pathfinding: A\* maze solver**
* Character that finds the exit of a maze generated in a 2D grid using the A* algorithm
* Configurable seed

**Pathfinding: Horizon Search with enemies**
* The same character must defeat all the enemies before leaving the maze, chasing them with a horizon search
* The exit is avoided while there are enemies left, so the character cannot reach it by accident
* Once the last enemy is defeated, the character heads to the exit

## Technologies
* Unity
* C#

## Usage
* Open the project in Unity
* Q-Learning: run the training scene to generate the Q-table, then the testing scene to watch the trained agent
* Pathfinding: open the A* scene or the Horizon scene
