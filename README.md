# Risk ASP

## Description

Risk ASP is an implementation of the board game 🎲 [Risk](https://en.wikipedia.org/wiki/Risk_(game))) using the [Unity Game Engine](https://unity.com/).

### AI

The AI is implemented using [Answer Set Programming](https://en.wikipedia.org/wiki/Answer_set_programming) (ASP) and integrated using the [EmbASP](https://github.com/DeMaCS-UNICAL/EmbASP) framework [for C#](https://github.com/DeMaCS-UNICAL/EmbASP-CSharp/tree/master).

## Play 🎮

You can download and play over <https://farfi55.itch.io/riskasp>, on your favourite OS (Windows, Linux, Mac)

## Installation

### Requirements

- [Unity Game Engine](https://unity.com/) (tested with version 2021.3.23f1)

### Setup

1. Clone the repository
2. Open the project in Unity
3. Open the scene `Assets/Scenes/Menu.unity`
4. Start the Game
5. Select the number of players and their types (Human or AI)
   options are:
   1. FCC: Farfi-Checcho-Ciccio AI ([ai source code](./Assets/Scripts/EmbASP/AIs/FCC-group/))
   2. MPS: Marco-Pasquale-Simone AI ([ai source code](./Assets/Scripts/EmbASP/AIs/MPS-group/))
   3. Dumb: Random AI ([ai source code](./Assets/Scripts/EmbASP/AIs/dumb/))
   4. Human: Human Player
6. Press Play
7. Enjoy!

## How to Play

### Game Rules 

The rules of the game are the same as the original game ([Italian Edition](https://en.wikipedia.org/wiki/RisiKo!)), with the following exceptions:

- The game is played with 2 to 6 players
- The game ends when a player conquers the entire world

for more information see this [rules guide (italian)](https://risiko.it/wp-content/uploads/2017/10/Regolamento-Risiko.pdf).

### Controls

If you're playing as a human player, you can use the following controls:

- Left Click on a territory
  for each phase the selection has a different meaning
  - in the reinforce phase, you select the territory to reinforce
  - in the attack phase, you first select the attacking territory and then the attacked territory
  - in the fortify phase, you first select the territory from which to take the reinforcements and then the territory to reinforce
- Skip Button: Skip the current phase (in reinforce this will spend all the remaining reinforcements randomly)
- Exchange Button: Exchange the cards for reinforcements (only if you have 3 cards with a valid combination)
  
## Authors

FCC Group:

- [Alessio Farfaglia](@Farfi55)
- [Francesco Gallo](@CiccioGallo13)
- [Francesco Strangis](@checcostra)

MPS Group:

- [Marco Duca](@markducks)
- [Pasquale Tudda](@ryuk4real)
- [Simone Rotundo](@simonerotundo)

## Screenshots

![Main menu](./screenshots/main-menu.png)
![turn 15 cards exchange](./screenshots/turn-15-cards.png)
![turn 22 domination](./screenshots/turn-22-domination.png)
![turn 25 victory screen](./screenshots/turn-25-victory-screen.png)
