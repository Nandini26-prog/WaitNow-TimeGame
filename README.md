# Wait... NOW! 

A minimalist 2D precision timing game developed in Unity. This project was built iteratively around the theme of "Time", focusing on core mechanics, game feel, and dynamic difficulty scaling.

 **[Play the WebGL build on itch.io here](https://nansquare.itch.io/waitnow))**
 **[View the iterative development process on Miro here]( https://miro.com/app/board/uXjVG08_35Y=/?share_link_id=708873105142)**

## Core Concept
The game represents a broken clock mechanism. A metronome bar swings rapidly across the screen, and the player must press `SPACE` at the exact millisecond it crosses the target zone to sync the time. 

## Controls
* **SPACE:** Start the game / Sync the bar.
* **R:** Restart upon Game Over.

## Mechanics & Level Progression
The game uses a dynamic threshold system. Accumulating 3 progress points (Perfect = 2pts, Nice = 1pt) triggers a Level Up, which alters the game state:
* **Nice Hit (+1 Score):** Bar lands within the target zone.
* **Perfect Hit (+5 Score):** Bar lands within the exact dead-center 20% of the zone.
* **Level 1:** Static center target.
* **Level 2:** Target zone teleports to a random X-coordinate after every successful hit.
* **Level 3:** Target zone shrinks in width, demanding higher precision.
* **Level 4+:** Both the metronome bar and the target zone move simultaneously.

## Technical Implementation
* **Engine:** Unity (2D Core Built-in Render Pipeline)
* **Platform:** WebGL
* **Highlights:** Frame-independent movement (`Time.deltaTime`), custom UI animations via Coroutines, dynamic screen bounds calculation, and `Vector3.SmoothDamp` for trailing visual effects.

*Note: The Unity `Library` folder has been intentionally excluded via `.gitignore` to keep the repository clean.*
