# Performance Bounceback Project
This project documents (through this write-up and changes) a number of 
modifications made to improve performance of a VR application.  The game itself
allows the player to move around the room, throwing balls onto trampolines
within a count down period.

# Gameplay Development (not optimization)
* Added timing logistics to game manager
    * initial score, time remain counter, public variable for initial game time.
    * added callback structure (instad of in update loop)
    * remove `Update` function and instead use `InvokeRepeating` every half second
* Modified trampline to use functional score inrement
* Modified scoreboard 
    * register and support callback instead of update
    * created four scoreboards along walls for easy visibility
* Sound addition
    * added bounce sound for ball when it hits trampoline
    * added music loop that gradually goes up in pitch
* BallSpawner: modified to check if game is running in update loop


# Scripting
## Update and Start Optimization
* Score: After connecting scoreboard, created cached text variable 
  instead of pulling back a child component every `Update` frame.
* Trampoline: Move functions called in `Update` to `Start`

# Profiling
## Spotting Spikes


## Spotting Constant Time Waste

## On-device Optimization (Vive)


# General Graphics 
## Optimizing Lights
bake, mixed, realtime
confirm mipmap for distance

## Optimizing Shadows
range and number of fall off regions

## Optimizing Anti-Alias
oversampling vs distance



# Administrata Considerations
This project is part of [Udacity](https://www.udacity.com "Udacity - Be in demand")'s [VR Developer Nanodegree](https://www.udacity.com/course/vr-developer-nanodegree--nd017).

## Content Sources
* [starting bugle race](http://free-loops.com/2091-bugle-call-race.html)
* [buzzer sound](http://www.orangefreesounds.com/game-show-buzzer-sound/)
* [ball bounce](https://freesound.org/people/1479009/sounds/411552/)
* [PimPoy music loop](https://www.dl-sounds.com/royalty-free/pim-poy-pocket/)

## Time Consumed
* Raw Log (to be simplified)
* 2h 25m

## Versions
- Unity 2017.3.0f3+ (for development), originally created in Unity 2017.2.0f3
