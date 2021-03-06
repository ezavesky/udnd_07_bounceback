# Performance Bounceback Project
This project documents (through this write-up and changes) a number of 
modifications made to improve performance of a VR application.  The game itself
allows the player to move around the room, throwing balls onto trampolines
within a count down period.

1. [Gameplay Modifications](#gameplay-development-not-optimization)
2. [Profiling and Scripting Optimizations](#profiling)
3. [Graphics Optimizations](#general-graphics)
4. [Source and Administrata](#administrata-considerations)

# Gameplay Development (not optimization)
* Added timing logistics to game manager
    * initial score, time remain counter, public variable for initial game time.
    * added callback structure (instad of in update loop)
    * remove `Update` function and instead use `InvokeRepeating` every half second
    * add game state for begin, play, end of game
* Sound addition
    * added bounce sound for ball when it hits trampoline
    * added music loop that gradually goes up in pitch
    * addded reverb for "factory like" envirionment:w
* Throwable objects
    * a potential issue was found where ball release was using a 
      negative velocity; this issue requires additional investigation (TODO)
    * add touch pad detection to restart the game    
* Instructions
    * add instruction panel that shows/hides when game is done/starts

# Scripting
## Update and Start Optimization
* Score: After connecting scoreboard, created cached text variable 
  instead of pulling back a child component every `Update` frame. **(r1)**

# Profiling
## Spotting Spikes
* BallSpawner **(r0)**
    * modified to check if game is running in update loop
    * reuse balls that are too old (since spawn) or too far away from user


## Spotting Constant Time Waste
Looking at profiler, a lot of time was wasted in the
`Update` call of the *Trampoline* script.

* Modified trampoline to use functional score increment **(r1)**
    * Move functions called in `Update` to `Start` 
    * Remove excessive search and component retrieves from `Update`
* Modified scoreboard **(r1)**
    * register and support callback instead of update
    * created four scoreboards along walls for easy visibility

## On-device Optimization (Vive)
* Confirmed no extreme usage, monitor ball bounce **(r2 Vive)**

<table style='width:100%'>
<tr>
    <th>revision</th>
    <th>commentary and notes</th>
    <th>example profiler image</th>
</tr>
<tr>
    <td>r0</td>
    <td>initial execution time time, with a lot of time devoted to scripting (vsync is removed here)</td>
    <td> <a href="docs/time_0_raw.png" target="_new"><img src="docs/time_0_raw.png" width="100%" /></a></td>
</tr>
<tr>
    <td>r1</td>
    <td>before revision, profile indicator for expensive update operation</td>
    <td> <a href="docs/time_0b_updates.png" target="_new"><img src="docs/time_0b_updates.png" width="100%" /></a></td>
</tr>
<tr>
    <td>r1</td>
    <td>fixed with game callback and trampoline update improvements</td>
    <td> <a href="docs/time_1_callback.png" target="_new"><img src="docs/time_1_callback.png" width="100%" /></a></td>
</tr>
<tr>
    <td>r2</td>
    <td>demonstration of impact from trampoline movement, next optmization</td>
    <td> <a href="docs/time_2_trampmove.png" target="_new"><img src="docs/time_2_trampmove.png" width="100%" /></a></td>
</tr>
<tr>
    <td>r2 (Vive)</td>
    <td>demonstration of bounce/collider effect on runtime performance</td>
    <td> <a href="docs/time_3_bounce.png" target="_new"><img src="docs/time_3_bounce.png" width="100%" /></a></td>
</tr>
<tr>
    <td>r3 (Vive)</td>
    <td>minimal effect of runtime performance after light probe</td>
    <td> <a href="docs/time_4_play_vive.png" target="_new"><img src="docs/time_4_play_vive.png" width="100%" /></a></td>
</tr>
</table>
   

# Graphics, Quality, Lights
## Optimizing Lights
* Mixed lighting - modified all lights to be "baked" instead of real-time **(r2)**
* Ground Trampolines - updated ground trampoines (those that are not moving) 
  to be static **(r2)**
* Added light probe around specific lights, where largest expected area of
  contrast is found. **(r3)**
* Set frametime to 90Hz
* Air Trampolines - add Rigidbody because all moving objects get rigid bodies
* Lighting Path - set `forward` because of benefits for processing time

<table style='width:100%'>
<tr>
    <th>revision</th>
    <th>commentary and notes</th>
    <th>example profiler image</th>
</tr>
<tr>
    <td>r0</td>
    <td>initial draw time, with note of the high draw calls, triangles, and verticies</td>
    <td> <a href="docs/draws_0_raw.png" target="_new"><img src="docs/draws_0_raw.png" width="100%" /></a></td>
</tr>
<tr>
    <td>r1</td>
    <td>reduced draws from updated spawner, scoreboard, game manager</td>
    <td> <a href="docs/draws_1_spawner.png" target="_new"><img src="docs/draws_1_spawner.png" width="100%" /></a></td>
</tr>
<tr>
    <td>r2</td>
    <td>halved draws from ground trampolines being made static <em>(both r1 and r2 ar erear facing statistics)</em></td>
    <td> <a href="docs/draws_2_static_tramp.png" target="_new"><img src="docs/draws_2_static_tramp.png" width="100%" /></a></td>
</tr>
<tr>
    <td>r3 (vive)</td>
    <td>draws during play on device</td>
    <td> <a href="docs/draws_3_play_vive.png" target="_new"><img src="docs/draws_3_play_vive.png" width="100%" /></a></td>
</tr>
<tr>
    <td>r5 (vive)</td>
    <td>static batching enabled draws during play on device</td>
    <td> <a href="docs/draws_5_batched_vive.png" target="_new"><img src="docs/draws_5_batched_vive.png" width="100%" /></a></td>
</tr>
</table>
   

## Optimizing Graphics
* Confirmed that all textures have mipmap already enabled (**Textures -> Generate Mip Maps**)
* Shadow regions looked reasonable, with four-part setting (**Project Settings -> Quality -> Cascade Splits**)
* Anti aliasing turned on for top three quality settings (**Project Settings -> Quality -> Anti Aliasing -> ~4x or 8x**)
* Enable static batching (**Build Settings -> Player Settings -> Dynamic Batching**) **(v5)**

# Administrata Considerations
This project is part of [Udacity](https://www.udacity.com "Udacity - Be in demand")'s [VR Developer Nanodegree](https://www.udacity.com/course/vr-developer-nanodegree--nd017).

## Content Sources
* [starting bugle race](http://free-loops.com/2091-bugle-call-race.html)
* [buzzer sound](http://www.orangefreesounds.com/game-show-buzzer-sound/)
* [ball bounce](https://freesound.org/people/1479009/sounds/411552/)
* [PimPoy music loop](https://www.dl-sounds.com/royalty-free/pim-poy-pocket/)

## Time Consumed
* 9h 20m - most time was spent on custom assets instead of optimization; 
  it still has to be a fun game to play!
* There was a lot of investigative and experimental time lost around light
  probes, which shoudln't have been the case.  An issue was created for others
  to hopefully see and learn from: https://discussions.udacity.com/t/light-probe-tricks/708956.

## Versions
- Unity 2017.3.0f3+ (for development), originally created in Unity 2017.2.0f3
