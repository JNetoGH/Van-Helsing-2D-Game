# Van Helsing GDD
![imgVanHelsing](https://user-images.githubusercontent.com/24737993/236948246-0bd5f2d7-683a-418d-9d50-3cb100abdce9.png)



<br>
<hr>
<br>



## OVERVIEW

**STUDIO** <br>
Super Fun Games

**TEAM** <br>
João Neto (22200558), David Mendes (22203255), João Silva (22004451), Vasco Caleiro (22203051)

**THEME**  <br>
Movie (Van Helsing, 2004)

**MOVIE SYNOPSIS**  <br>
Van Helsing" is an action-adventure film where Gabriel Van Helsing, a legendary monster hunter,
is sent to Transylvania to help Anna Valerious rid their village of Count Dracula. Along with a friar named Carl,
they encounter dangerous obstacles including werewolves, vampires, and Dracula's offspring.
Van Helsing must confront his past, tied to Dracula's, to defeat him once and for all.

**GAME SYNOPSIS** <br>
This is a platformer action game, based on the movie "Van Helsing (2004)", focused on methodical combat and pattern learning boss fights.
It will have a fluid combat system with responsive controls with some elements drawn from the ***Soulslike***, ***Shoot 'em Up*** and  ***Run and Gun*** genres.
Being a direct inspiration from the movie "Van Helsing (2004)", it features a dark and gothic art style and features vampire related enemies and locations. We will control a demon hunter,
armed with a repeating crossbow and portable saws, with the task of defeating Dracula and any other related beings.
The player will play as Van Helsing revisiting scenarios and battles taken straight from the movie.

**TARGET AUDIENCE** <br>
The action based combat and dark fantasy / victorian themes and its harsh difficulty make this game more appealing to a mature audience.

**TARGET PLATFORM** <br>
The target platform this project will be idealized will be Desktop Windows but due to the accessibility of the controls and the mechanics we are intending on implementing,
there will always be a possibility that we might make console versions.

**UNIQUE SELLING POINTS**
* Experience first hand the iconic boss battles from the 2004 Van Helsing Title;
* Engage with the fluid movement and combat systems in this side-scrolling platformer;



<br>
<hr>
<br>



## GAMEPLAY
Includes things like the objectives, game progressions, and the game mechanics.

**OBJECTIVES/PROGRESSION** <br>
The game  is highly inspired in other games like Cuphead, Dead Cells and Mega Man, where there progression is mostly linear.
The player is meant to complete a easier part of the level first, with "easier" and more "generic" enemies, in other to reach a boss in the end of the level to a more unique and hard boss fight.
Being inspired by the ***Soulslike*** sub-genre, the player is meant to repeat the levels many times in order to get progressively better while learning the bosses and common enemies patterns.

**GAMEPLAY STYLE** <br>
The Gameplay style is inspired in the ***Shoot 'em Up*** and the ***Run and Gun*** genres, where the player is meant to try to complete the phase as quick as possible, rushing on the enemies and shooting projectiles.
This game is also meant to have some melee combat, they are dangerous strikes but also make a higher damage.

**MECHANICS OVERVIEW**

| Mechanic      | Brief Description                           |
|---------------|---------------------------------------------|
| Walk/Run      | Movement has two speeds: walk and run.      |
| Dash          | Quick movement in X axis                    |
| Fluid Jump    | Jump height varies with jump buffer.        |
| Snappy Jump   | Gravity scales with each jump stage.        |
| Arm Switch    | Switches between arms (crossbowArm/sawArm)  |
| Ranged Combat | Crossbow shoots arrows for ranged damage.   |
| Melee Combat  | Saws used for melee attacks.                |
| Werewolf Mode | Become a werewolf in final battle.          |
| Cooldown      | Weapons and abilities have cooldown timers. |

**WALK/RUN MECHANIC** <br>
There are two different kinds of movement speed, walk or run. <br>
The player can lock the speed to walk speed by pressing `Q`, then the character will move slowly. <br>
The `A`, `D`, `<-` and `->` keys are used in order to change the movement direction.

**DASH** <br>
The player can initiate a dash by pressing `SHIFT`. <br>
This is simply a quick movement in the X-axis direction. During the dash, gravity is ignored, and the player is still vulnerable to receive damage.

**FLUID JUMP** <br>
The jump system consists of two parts: a variable height system that allows the player to jump higher while pressing `SPACE`,
and a jump buffer that casts a straight line towards the ground to store a jump input if the line hits the ground while the player is falling. <br>
This buffer is useful for making sequential jumps easier. <br>
the jump system is highly customizable and allow us to set any length to the buffer and duration to the coyote time.

**SNAPPY JUMP** <br>
The jump system allows for different gravity scales to be set for each phase of the jump, leading the player to fall quicker than it rises,
resulting in a snappier jump, or what we call an anti-floaty jump. <br>

**ARM SWITCH** <br>
The player can switch between the current arm by hitting `E` (between the crossbowArm and the sawArm).

**RANGED COMBAT** <br>
The player can shoot a projectile in the form of an arrow towards the mouse position by clicking on `MB1`. <br>
This projectile can go through platforms and inflicts 1 damage to any hit enemy. <br>
Once it hits a target, the projectile disappears and cannot inflict damage anymore.

**MELEE COMBAT** <br>
The player can strike with a saw towards the mouse position by clicking on `MB1`. <br>
This strike has a small area of effect, which deals ":warning: not decided yet, but is bigger than the arrow :warning:" damage to any hit enemy in this area. <br>
In contrast with the arrow, it doesn't disappear when it hits an enemy, just when its timer ends, what makes it a good tool for crowd control.

**WEREWOLF MODE** <br>
The player becomes a werewolf while fighting against Dracula in final battle at the top of the Dracula's castle. <br>
Normal combat mechanics don't work on Dracula, so following the movie's lore, only when Van Helsing becomes a werewolf, the player can deal damage to the boss. <br>
This transformation is temporary though, and lasts only while the full moon appears on the big window in the background, which goes on and off in a cycle. <br>
This mode is meant to bring two states to the final battle: a survival state, where the player tries to survive with the game's regular mechanics,
until the fight state arrives, where finally the player can become a werewolf and deal damage to Dracula. <br>
:warning: The werewolf combat mechanics have not been decided yet. :warning:

**COOLDOWN** <br>
Weapons and abilities have their own cooldown timers(countdown), those are indicated on the GUI by a progress bar for each weapon/ability.



<br>
<hr>
<br>



## CAMERA
The game camera uses a system called SmoothSpringCamara, which is a hybrid and highly customizable implementation of a Camera Controller. <br>
There is a square-shaped area referred to as the smoothing box. While the player is inside this box,
the camera will behave with a somewhat standard Lerp behavior, whereas when the player is outside,
the camera will stop smoothing and start to chase the player at a constant speed called catchSpeed until the smoothing box catches up with the player again. <br>
This hybrid implementation is crucial to ensure the player never gets out of the camera's view. <br>
The system provides smooth transitions between camera behaviors, and the catchSpeed can be adjusted for different kinds of situations.

**SYSTEM PREVIEW** <br>
![Alt Text](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExNDhjNzQ5ZTU0YjFjYmU4YjZhY2ViOTMyZTUzNDhhYWRiNTA5MzFkNiZlcD12MV9pbnRlcm5hbF9naWZzX2dpZklkJmN0PWc/CjJWxND1tYLuD1HNcQ/giphy.gif)



<br>
<hr>
<br>



## LEVELS
For now we have only 2 levels planned, more can be added in the future, the movie has many combat scenes.

**FIRST LEVEL** <br>
It's inspired in the battle of Van Helsing against the three Dracula's brides, Verona, Aleera and Marishka,
as in the movie it played in the village next to Frankenstein's castle,
the player will play through the village until he encounters the brides, then a triple boss battle begins.


**SECOND LEVEL** <br>
This level is based on the last battle of the movie where Van Helsing faces Dracula at his castle,
it's a vertical level just like in the movie, this fight is placed on a tower. <br>
At the top the player will encounter the final battle against Dracula. <br>
![DraculasCastle](https://github.com/JNetoGH/Van-Helsing-2D-Game/assets/24737993/644d367c-8b0c-4181-932b-471f9cf2e5c6)


<br>
<hr>
<br>



## ENEMIES
We have planned 5 different enemy entities all based on existing characters from the movie. Their current health points value are not permanent.

**MINI DRACULA**  
This is a basic flying enemy, ment to pressure and deny playing area to the player.  
Currently it tracks and follows the player after a set time after spawning, but it's behaviour might change.  
  
Health Points : 3  

![Screenshot 2023-03-26 200453.png](Art%2FConcepts%2FMini%20Dracula%2FScreenshot%202023-03-26%20200453.png)
  
**DRACULA**  
This will be the final boss.  
As of now, it only moves side to side and it's attack moves will be added later.  
  
Health Points : 20  
  
![Dracula.png](Art%2FConcepts%2FDracula%2FDracula.png)  
  
**DWERGI**  
This is a basic ground enemy, intended to be able to swarm the player in numbers.  
Currently it stays idle until the player gets close, which makes it charge at the player. The current iteration has some design flaws.  
  
Health Points : 2  
  
![Screenshot_3.png](Art%2FConcepts%2FDwergi%2FScreenshot_3.png)  
  
**DRACULA'S BRIDES**  
This duo will be the first boss encounter.  
No assets or game objects have been made yet.

**COCOON**  
This is a stationary enemy structure.  
The idea is to spawn mini draculas if the player gets too close without destroying it.  
No assets or game objects have been made yet.

<br>
<hr>
<br>



## GAME ART

**Concept Arts:** https://drive.google.com/file/d/150Z57JqgJQeHUV4Fr360m37cGVkAub_Y/view?usp=share_link

**Background Moodboard:** https://drive.google.com/file/d/1liBJ6A_J-jPEEY7-Xt6eJs2CKNjhF0Aq/view?usp=share_link

**Castle (interior) Moodboard:** https://drive.google.com/file/d/17em802pnH-7-A8B9ymDteGMGmT7jL3Al/view?usp=share_link

**Main Character Moodboard:** https://drive.google.com/file/d/1z-Rc5g4LjVifg0ogxo-2iKNmlGUjbmjz/view?usp=share_link

**Enemies Moodboard:** https://drive.google.com/file/d/1eSqaHa14DMVExjXKJeABmN950DWKFwtE/view?usp=share_link

**Animation Moodboard:** https://drive.google.com/file/d/1S1PIw2aNNjHxMpPrnEvK-AdEsUBWop9C/view?usp=share_link

**Weapons Moodboard:** https://drive.google.com/file/d/1Go0igwu6Z4AhXzRtU35FLz-AJ5xbh8-h/view?usp=share_link

**Asssets Moodboard:** https://drive.google.com/file/d/1nb55yfw60occR-8jjWTNjEdHUR_wPoVd/view?usp=share_link

**UI Moodboard:** https://drive.google.com/file/d/13vZZremzW01fynp3uU-uWuBWYfekL0gX/view?usp=share_link



<br>
<hr>
<br>



## USER INTERFACE (UI)

**STYLE** <br>
The games follows a medieval victorian dark style so the Game User Interface will also be designed in this aesthetic,
we want to maximize the information the player can have at their disposal without cluttering the screen and interfering with gameplay.
For this reason we firstly intend to move most of the information of the Game User Interface information to the bottom side of the player’s screen as some levels have a vertical scrolling direction.
A clean GUI helps the player focus on gameplay, although we won’t discard the beauty factor when creating these.
  
![Untitled.jpg](Art%2FGUI%2FScreenShot%2FUntitled.jpg)

<br>
<hr>
<br>



## CHANGELOG

| Name  | Date       | Description                                  |
|-------|------------|----------------------------------------------|
| JNeto | 8-may-2023 | Movie Synopsis added                         |
| JNeto | 8-may-2023 | Target audience updated                      |
| JNeto | 8-may-2023 | Mechanics overview added to Gameplay section |
| JNeto | 8-may-2023 | Mechanics details added to Gameplay section  |
| JNeto | 9-may-2023 | Camera section added                         |
| JNeto | 9-may-2023 | Line separators added                        |
| JNeto | 9-may-2023 | Camera gif added                             |
| JNeto | 9-may-2023 | Levels Section updated                       |
| DMendes| 9-may-2023| Unique selling points updated                |
| DMendes| 9-may-2023| Enemy section added                          |


<br>



<!--stackedit_data:
eyJoaXN0b3J5IjpbNjQ1ODc4MDI2LDg2MjQ4NzU3NCwtMTcyMj
AwMDE4MCwtMTg4MzgxNjgyXX0=
-->