# The-Director
foundation
For my research project i decided to focus on Director AI’s and discover the design philosophies of making one and researching directors in existing games before making my own implementation.

**Design Philosophy**

There is no wrong or correct way to develop a Director AI. It does exist out of 3 key features that define what an director AI is. These are the following:
-	The Director AI has to record data about what the player is doing.
-	The Director AI has to adapt the game world based on this data.
-	The Director AI has to enforce In-World rules and logic

Because these 3 features are the only defining element of a director AI I thought it would be better to look at games that implemented this system to understand what a director AI really is and it basic workings. Keep in mind that these examples are not the only implementation of this Ai system and that there are many more games that implemented this system.

**Left 4 Dead**

The director AI in left 4 dead is perhaps the most well-known implementation of a director AI and works in 3 stages. The first stage is called the build-up and occurs when the players are not in a lot of danger. The director will increase the danger level and switch to the second stage called the peak. In this stage the danger level reached a given threshold and will this will trigger the director to give one last big attack to the players to keep them under pressure. After this the director switched to the last stage called relax. In this stage the director slows down significantly and allow the players to catch breath and heal. In this stage the director also waits so restart the cycle but this is dependent on a number of factors. A big factor that the director takes into account during this whole process is stress level of the players. This is monitored for each player separately. 

The stress increases when:
- zombies attack the player (increases gradually)
- zombies are in proximity of the player (increases gradually)
- zombies execute special attacks (increases instantly)

The director will attack players with a lower stress level more. This is how the system works behind the scenes but how does it change the game world itself. The director is responsible for spawning enemies in the world and placing pickups (at presets) according to the players stress level. The director AI is also balanced and has some restrictions to make it fairer to the players. The director AI also interacts with an audio manager to give cues when certain events are happening. 

**Alien Isolation**

The director AI in alien isolation is a very complicated one. It takes a lot of factors into account but I will simplify it. The main purpose of the director AI is to point the alien AI to the player by giving hints of the players location but the director never shares the exact location of the player. The way it does this is by working with a menace system. If the menace is low the director will be put in active mode and send signals to the alien AI to hunt and search for the player. If the menace is high the director will be put to passive mode to let the player catch his breath. In this mode the director will send instructions to the Alien Ai to crawl into the vents and leave the player alone for a while at least. 

The menace increases when:
- The player is close to the alien
- The player is in the line of sight of the alien
- The alien is on the motion tracker of the player (and if it can access the player fast)

The alien AI itself exist out of behaviour trees.

**Far Cry**

The director Ai in the far cry series serves the purpose of an asset manager most of the time. It will spawn NPC’s like humans and animals t if they are in a radius of the player and despawn them if they are out of that radius. This way the CPU and GPU focus they’re resources where they need to be. This base system is used in a lot of open world game but I listed far cry as a prime example.

**Own implementation**

**Preproduction**

I decided to do a simple implementation of a director AI in unity using a system similar as used in left 4 dead. I will have a build-up, peak and relax stage that affects the game world and takes the player’s stress level into account. The gameplay will be a twin stick shooter. I also decided to use my unity project from game mechanics as the foundation because It already has twin stick shooter mechanics present. I also had to change how the stages and stress level interact with each other because my implementation is single player. In the build-up stage you’re maximum stress level will decide how much zombies will spawn in the peak stage. If you’re maximum stress level is already very high in the build-up stage then the peak stage will be more forgiving but if your maximum stress level was low the peak will be very hard. In the relax stage your maximum stress level in the peak stage will decide how much time you have to heal and collect ammo before the next cycle starts. It will also affect how much ammo and health supply spawn. The maximum amount of zombies that can spawn also increases per cycle.

**Game mechanics**

The first thing I did was change the foundation I already had to fit more into the style of game I’m going for. This meant changing the enemy types and the map and how pickups work.

**Stress level**

I added a stress level script and element to the HUD. The stress system i made is simpler than the one used in left 4 dead but has the same purpose. The stress in my implementation increases when an enemy is in 6 meters of the player (danger zone) and when an enemy succesfully atacks the player. When an enemy is in the danger zone the stress only increases 1/2 of when an enemy attacks the player. This makes sure that the stress doesn't increase too fast and that the player is overwhelmed.

**PickUps**

pickUps spawn in the relax stage of the cyclus. They have preset spawn locations on blue quads but how many will spawn in the relax stage is dependent on the max stress level in the buildUp and peak stage. How higher those 2 value were how more pickups will spawn. The pickups exist out of health and ammo.



