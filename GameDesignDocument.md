# Game Design Document: LeapUp

## 1. Overview

LeapUp is an endless climbing game where players control a square and aim to climb platforms as high as possible. The game features dynamically generated platforms made of blocks, with various patterns and behaviors. The objective is to climb as high as possible while avoiding touching any red blocks, called death blocks.

### 2. Gameplay

#### 2.1 Objective

The objective of the game is to climb as high as possible by jumping from one platform to another while avoiding a line of death blocks that go up progressively.

#### 2.2 Controls

- Left Arrow: Move the square to the left.
- Right Arrow: Move the square to the right.
- Space bar: Jump.

#### 2.3 Platform Generation

The game generates platforms dynamically based on predefined patterns and behaviors added as the player goes up. The following variations are implemented in the platform generation:

- Block Movement: Some platforms have blocks that move sideways, adding a challenge to the gameplay.
- Intermittent Blocks: Certain platforms have intermittent blocks that appear and disappear periodically, requiring precise timing and coordination.
- Platform Height: Platforms are generated at different heights, offering varying levels of difficulty and requiring strategic jumping.

#### 2.4 Line of Death Blocks

When the player reaches a height of 7 meters, a line of death blocks gradually moves upwards from the bottom. If the player touches this line, they die, and the game restarts from the ground. This mechanic adds an additional challenge and raises the stakes as the player climbs higher.

### 3. User Interface

The user interface (UI) consists of the following elements:

- Score: Displays the player's current height or distance climbed.
- Try again Button: When player dies he can start over from the ground using this buton.

### 4. Art and Audio

#### 4.1 Art Style

The game has a neon-style visual theme, with vibrant colors and a glowing effect on all blocks and letters. This visual style creates an immersive neon vibe throughout the gameplay.

#### 4.2 Sound Effects and Music

The game will include catchy 90's background music that complements the neon theme. Sound effects will accompany actions such as jumping and dying.

### 5. Progression and Difficulty

The game offers an endless climbing experience, where players can aim to achieve higher scores. The difficulty gradually increases as the player climbs higher, with block movements and more challenging patterns.

### 6. Target Platforms

LeapUp will be developed for windows and mobile platforms, including iOS and Android, to leverage the touch controls and reach a wide audience of mobile gamers.

### 7. Conclusion

LeapUp provides an engaging and challenging endless climbing experience with dynamic platform generation, a neon visual style, and intuitive controls. By climbing higher, players can test their skills and challenge themselves to reach new heights while enjoying the immersive neon atmosphere.
