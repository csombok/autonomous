# Autonomous

Autonomous is a car game player AI framework written in [MonoGame](http://www.monogame.net/).
The goal of the game is implementing a player and reach the finish line before the others.

# How to setup

1. Install latest [Monogame SDK](http://www.monogame.net/downloads/)
2. Clone GitHub repository
```
git clone https://github.com/csombok/autonomous.git
```
3. Open solution in Visual Studio 2017
4. Build solution with AnyCPU configuration
5. Build solution with x64 configuration
6. Run autonomous.exe (GUI mode)

# Run game in interactive mode

The easiest way to test player plugins is to run the game with GUI. 
Run with GUI:

```
autonomous.exe
```

GUI mode: 
![GUI mode](autonomousgui.png)

## Viewport & Camera selection

### Viewport selection

GUI mode provides a manual viewport selection as follows:

* **F1**: select 1st player's viewport only
* **F2**: select 2nd player's viewport only
* **F3**: select 3rd player's viewport only
* **F4**: select 4th player's viewport only
* **F5**: show all players 
* **F9**: automatic viewport and camera mode

### Camera views

The following cameraviews can be selected:

* **F6**: birdview camera
* **F7**: inside camera
* **F8**: rear camera

# Run game in command cline mode

You can run the application in CLI mode by passing *-quiet* CLI option.

```
autononous.exe -quiet 
```

## CLI parameters

* *-quiet*: CLI mode
* *-timeAcceleration*: time acceleration to speed up testing
* *-traffic*: traffic intensity (values: 0.1f - 1.0f defalt: 0.5f)
* *-length*: course length (default: 1000f)
* *-tournamen*: tournament mode

## Tournament mode

Tournament mode allows interactive game play for multiple players in multiple rounds in different scenarios.

```
autononous.exe -tournament
```

# Add new player

By implementing *IPlayer* interface you can add a new player to the game.
Each player plugin assembly has an isolated game loop by running a separate thread managed by the framework.

### Game loop events

* **Initialize**: Initialize the game play
* **Update**: Game loop update. It is invoked in every update cycle by passing *gameState* parameter consisting game object states.
* **Finish**: Game finish event

Check the *SamplePlayer* project to see how to implement a new player. 

```csharp
    [ExportMetadata("PlayerName", "Example")]
    public class SamplePlayer : IPlayer
    {
        private string _playerId;

        public void Initialize(string playerId)
        {
            _playerId = playerId;
        }

        public PlayerAction Update(GameState gameState)
        {
            // Add logic for game loop update
        }

        public void Finish()
        {
            // Add logic for game finish
        }

    }
```

# Game loop
TODO

### Game state
TODO

### Game control actions
TODO

### Collision and damage
TODO

# Scoring

After each game scores are saved to **results.csv** file in the following form:

TODO

Score logic:

* Winner: **8pt**
* 2nd position: **4pt**
* 3rd position: **2pt**
* 4th position: **0pt**
