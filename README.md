# C# Godot WorldGen Experiments

<h2>Overview</h2>
<p>A Godot C# application designed to experiment with advanced procedural generation techniques for creating immersive, dynamic game worlds. This project leverages a blend of noise algorithms, Wave Function Collapse (WFC), and carefully placed semi-static/static entities to craft richly detailed 2D environments. The goal of this application is to explore the boundaries of random generation with Godot, C#, GDScript (Godot's native scripting language, quite similar to python) to produce varied biomes and ecosystems, where flora and fauna are intelligently matched to their specific biomes, enhancing the realism and depth of the world.</p>

![image](https://github.com/codeSmithDave/C-Sharp-Godot-WorldGen-Experiments/assets/29952471/6e24b7e8-c988-4806-b14a-10923f4407bb)


<h2>Features</h2>
<ul>
 <li>**Procedural World Generation:** Utilizes a combination of noise functions to generate unique, diverse terrains, landscapes, flora, fauna & cities.</li>
 <li>**Wave Function Collapse Algorithm:** Employs WFC for pattern-based generation, ensuring that each world is both random and logically consistent.</li>
 <li>**Biome Specific Entities:** Integrates a system for defining and spawning fauna and wildlife based on the biome they are best suited to, adding an extra layer of authenticity to each generated world.</li>
 <li>**Customizable Parameters:** Offers modifiable parameters for world size, complexity, and biome types, allowing for personalized world creation experiences.</li>
</ul>

<h2>Goals</h2>
<p>The primary goal of of this application is to delve into the capabilities of the Godot engine through random world generation experiments, laying the groundwork for potentially developing a game as a creative and educational hobby.</p>

<h2>How It Works, issues, thoughts for the future</h2>
<p>This experiment employs a mix of Simplex noise and Wave Function Collapse algorithm to currently (this will be expanded upon in later iterations) generate a 2D world. I have run multiple experiments in the quest for world generation with various types of noise only, Wave Function Collapse only, and a mix of both.

As of right now, WFC is used to generate the map, however, the issue with this approach is the slow processing time for large worlds. Using noise to generate the world is much faster, however, the randomness effect reduces realism which is something important to me. The end goal is to use a a mix of both to generate a realistic 2D world, similar with Earth. These random worlds will have a north and south pole with various biomes depending on the distances from the poles. Each biome will contain flora and fauna related to it (as it can be seen with the snowy trees in the above image). Another planned future iteration would be to add chunk loading, meaning that the world will be "drawn / painted" as the player explores the world (similar to other games, like Minecraft, etc.). This will offer a massive increase in performance as we will be loading assets for a limited view instead of a massive world all at once.

**Other existing features:**
I have implemented a basic player with basic keyword controls (WASD) and a camera following the player, uncovering more of the app, as it walks around.</p>
