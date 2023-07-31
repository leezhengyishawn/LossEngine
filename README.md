# Loss Engine
2D Game Engine featuring physics, C# scripting and tile creation
Loss Engine is a 2D game engine and editor made for the courses GAM200 and GAM250 at university. Using it, we made the platformer Gero Pero. The team included of 5 Engineers (including me), 2 Game Designers and 2 Artists

![image](https://github.com/leezhengyishawn/LossEngine/assets/100258469/28276fae-d33c-4d79-9fbf-ef61795704a3)

# 2. Responsibilities
My main responsibilities were to stream assets into the engine and editor. This involved understanding the various assets (FBX, DDS, PNG) and working with the rendering programmer on how to show it on the screen. 

Following that, I also created debugging and profiling tools to help measure the performance and bottlenecks of my asset loading and other functions of the editor.

Gameplay wise, my biggest contribution was to create the boss fight. I coded the various parts of the boss to add life to it's movements. This resulted in a moving character that required zero additional art assets to move. All the attack patterns and responses was also achieved using code only. If I were to do the project again, I would have written a tweening tool that could interpolate between key frames and positions.

https://github.com/leezhengyishawn/LossEngine/assets/100258469/b0fd020f-347a-445a-a107-1818fb6500f4

All animations in this boss fight is done through code so there's minimal memory impact.

# 3. How to Use
Run Executable/LossEditor.exe it will take a while to load. This was before I learned how to binarize and streamline loading of asset files in YadaEngine.

Clicking on a game object will bring up it's stats on the Inspector. From there, you can edit it just like in Unity or Unreal.

To create a new script, create it in the Scripts folder and make sure it's included in the csproj file.

You can load any of the scenes in Resources/Scenes to try out the game.

Alternatively, you can install GeroPero_Setup.exe to just play the game. 
