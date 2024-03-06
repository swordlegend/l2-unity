# L2-Unity

<p>This project aim is to create a basic playable demo of Lineage2 on Unity.</p>

This [video](https://www.youtube.com/watch?v=IEHY37bJ7nk) inspired me to start on this project.

<p>Preview of the current state of the project:</p>

![Preview](https://cdn.discordapp.com/attachments/584218502148259901/1180162232814940280/image.png?ex=65eb28ba&is=65d8b3ba&hm=9fb347f90e0969ded501640e36d58353dd8046d107e54147c7e0abee926446aa&)

![Preview2](https://cdn.discordapp.com/attachments/584218502148259901/1214221893154897940/image.png?ex=65f85342&is=65e5de42&hm=27e2098e5dc3365d8e5e5e51f821a666531256df2412214585a6b75afafd54ed&)

![Preview3](https://cdn.discordapp.com/attachments/584218502148259901/1214952502492008500/image.png?ex=65fafbb1&is=65e886b1&hm=4de069cf9464fd2d5c18ac7a3a4d87757afc0926b3119c8807df1fd1c49bf951&)

![Preview4](https://cdn.discordapp.com/attachments/584218502148259901/1214956247506485290/image.png?ex=65faff2e&is=65e88a2e&hm=b71267feb997a17efd3bee0a1926fd8bc4ad434fb0cd0fd76bce439298f74831&)

## What are the expected features?

For now the aim is to create a basic demo, therefore only basic features will be available:
- Client-side Pathfinding ✅
- Click to move and WASD movements ✅
- Camera collision ✅
- Basic UI
    - Status window ✅
    - Chat window ✅
    - Target window ✅
    - Nameplates ✅
    - Skillbar
- Basic combat ✅
- Basic RPG features 
    - HP Loss and regen 🛠️ (Players can fight mobs but wont regen)
    - Exp gain on kills
    - Leveling
- Small range of models
    - 2 races for players ✅ (FDarkElf, FDwarf)
	- A few armor sets for each race ✅ (naked set, starter set)
	- A few of weapons each type ✅
    - All Monsters of Talking island ✅
    - All NPCs of Talking island ✅
- Server/Client features (server project [here](https://gitlab.com/shnok/unity-mmo-server))
    - Player position/rotation sync ✅
    - Animation sync ✅
    - Chat ✅
    - Server Ghosting/Grid system ✅
    - NPCs ✅
    - Monsters ✅
    - Monsters AI with Pathfinding ✅
- Import Lineage2's world
    - Talking island region ✅
        - StaticMeshes ✅
        - Brushes ✅
        - Terrain ✅
        - DecoLayer ✅
- Day/Night cycle ✅
- Game sounds (FMOD project [here](https://gitlab.com/shnok/l2-unity/-/tree/main/l2-unity-fmod/))
    - Ambient sounds ✅
    - Step sounds (based on surface) ✅
	- Music ✅
    - UI sounds ✅
    - NPC sounds ✅

## How to run?

<p>Open the "Game" scene and drag&drop the 1x_1x scenes into your scene.</p>

![Import](https://cdn.discordapp.com/attachments/584218502148259901/1180168459104034877/image.png?ex=65eb2e87&is=65d8b987&hm=a869d1c373c75b9ff52b93ccddaf91ccf853af21dd5e948cce3d53217f0ca124&)

If you don't want to setup the [server](https://gitlab.com/shnok/unity-mmo-server) and just want to run in an empty map. Select the <b>Game</b> GameObject in the <b>Game</b> scene and tick the <i>"offline mode"</i> checkbox.

![offline](https://cdn.discordapp.com/attachments/584218502148259901/1182499680056250418/image.png?ex=65ea6f25&is=65d7fa25&hm=a87480c4915cbf9c2723cc2b4c32f1c39c42e3e511bb0503db3ff4a6e031c998&)



## Contributing

Feel free to fork the repository and open any pull request.
