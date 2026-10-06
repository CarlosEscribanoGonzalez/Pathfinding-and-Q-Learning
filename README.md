## Overview
Unity project (C#) with two AI practices: reinforcement learning with **Q-Learning** and pathfinding with **A\*** and a **Horizon Search**.

## Features
**Pathfinding: A\* maze solver**
* Character that finds the exit of a maze generated in a 2D grid using the A* algorithm
* Configurable seed
<p align = "center">
  <img width="710" height="400" alt="astar" src="https://github.com/user-attachments/assets/f427c1fa-ea43-4aa3-ae84-414f70ad8040" />
</p>

**Pathfinding: Horizon Search with enemies**
* The same character must defeat all the enemies before leaving the maze, chasing them with a horizon search
* The exit is avoided while there are enemies left, so the character cannot reach it by accident
* Once the last enemy is defeated, the character heads to the exit
<p align = "center">
  <img width="710" height="400" alt="horizon" src="https://github.com/user-attachments/assets/e4f84ed2-0982-4262-b4b9-4d4890412b79" />
</p>

**Q-Learning: agent that escapes from an opponent**
* Configurable learning rate, discount factor and exploration rate
* Separate scenes for training and testing
* Learned Q-table saved to a file and loaded in the testing scene
* States are abstracted from the map (nearby walls and relative position of the opponent), so the agent performs well on maps it has never seen
<p align = "center">
  <img width="710" height="400" alt="QLearning" src="https://github.com/user-attachments/assets/db6633f4-4ee1-4135-bb44-08488597bf4a" />
</p>

## Technologies
* Unity
* C#

## Usage
* Open the project in Unity
* Q-Learning: run the training scene to generate the Q-table, then the testing scene to watch the trained agent
* Pathfinding: open the A* scene or the Horizon scene
