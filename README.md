# The-Director
The research project focuses on Director AI’s and discover the design philosophies of making one and researching directors in existing games before making my own implementation.

### Design Philosophy

There is no wrong or correct way to develop a Director AI. It does exist out of 3 key features that define what an director AI is. These are the following:

-	The Director AI has to record data about what the player is doing.
-	The Director AI has to adapt the game world based on this data.
-	The Director AI has to enforce In-World rules and logic.

Because these 3 features are the only defining element of a director AI it would be better to look at games that implemented this system to understand what a director AI really is and it basic workings. Keep in mind that these examples are not the only implementation of this AI system and that there are many more games that implemented this system.

### Left 4 Dead

The director AI in *Left 4 Dead* is perhaps the most well-known implementation of a director AI and works in 3 stages. The first stage is called the build-up and occurs when the players are not in a lot of danger. The director will increase the danger level and switch to the second stage called the peak. In this stage the danger level reached a given threshold and will this will trigger the director to give one last big attack to the players to keep them under pressure. After this the director switched to the last stage called relax. In this stage the director slows down significantly and allow the players to catch their breath and heal. In this stage the director also waits to restart the cycle but this is dependent on a number of factors. A big factor that the director takes into account during this whole process is stress level of the players. This is monitored for each player separately. 

The stress increases when:
- zombies attack the player (stress increases gradually).
- zombies are in proximity of the player (stress increases gradually).
- zombies execute special attacks (stress increases instantly).

The director will attack players with a lower stress level more. This is how the system works behind the scenes, but how does it change the game world itself. The director is responsible for spawning enemies in the world and decide which pickups (at presets) are going to spawn according to the players stress level. The director AI is also balanced and has some restrictions to make it fairer to the players. The director AI also interacts with an audio manager to give cues when certain events are happening. 

### Alien: Isolation

The director AI in *alien: isolation* is a very complicated one. It takes a lot of factors into account but this is a simplified explanation of the AI. The main purpose of the director AI is to point the alien AI to the player by giving hints of its location but the director never shares the exact location. The way it does this is by working with a menace system. If the menace is low the director will be put in active mode and send signals to the alien AI to hunt and search for the player. If the menace is high the director will be put in a passive mode to let the player catch his breath. In this mode the director will send instructions to the Alien AI to crawl into the vents and leave the player alone (for a while at least). 

The menace increases when:
- The player is close to the alien.
- The player is in the line of sight of the alien.
- The alien is on the motion tracker of the player (and if it can access the player fast).

The alien AI itself exist out of behaviour trees.

### Far Cry

The director AI in the *far cry* series serves the main purpose of an asset manager. It will spawn NPC’s like humans and animals if they are in a radius of the player and despawn them if they are out of that radius. This way the CPU and GPU can focus their resources where they need to be. This base system is used in a lot of open world game but i listed *far cry* as a prime example.

### Own implementation

#### Preproduction

The project is a simple implementation of a director AI in unity using a system similar as used in left 4 dead. It has a build-up, peak and relax stage that affects the game world and takes the player’s stress level into account. The gameplay is a twin stick shooter. It is based of my unity project from game mechanics as the foundation because it already has twin stick shooter mechanics present. It also has a different implementation of the stages and stress level because this implementation is single player.

#### Game mechanics

The first thing in the project that was changed was the enemy types, pickUps and the map itself. They are changed to make it fit the new implementation more.

#### Stress level

A stress level script and element to the HUD was added. The class can easily get the stress value in percent or value by the use of getters. It also has an increase or decrease function where it can change the stress level dependent on the value that is passed as parameter. The stress level is also visualised as a blue bar in the HUD.

```c#
public class StressLevel : MonoBehaviour
{
    private int m_CurrentStress = 0;

    public float StressPercentage
    {
        get
        {
            return ((float)m_CurrentStress / 100);
        }
    }

    public float CurrentStress
    {
        get
        {
            return (m_CurrentStress);
        }
    }

    public void IncreaseStress(int amount)
    {
        m_CurrentStress += amount;

        if (m_CurrentStress >= 100)
            m_CurrentStress = 100;
    }

    public void decreaseStress(int amount)
    {
        m_CurrentStress -= amount;
    }
}
```

#### Main Loop

This is the main loop of the director AI. It switches between the stages dependent on the phaseLength which has a startvalue of 60 seconds.


```c#
    private void DirectorLoop()
    {
        if (m_Timer < m_PhaseLength)
        {
            BuildUp();
        }
        else if (m_Timer < m_PhaseLength + m_PhaseLength / 2)
        {
            Peak();
        }
        else if (m_Timer > m_PhaseLength + m_PhaseLength / 2)
        {
            Relax();
        }
    }
```


#### BuildUp

This is the BuildUp stage of the implemented director. It will start by by writing the stage to the HUD. It will then update the stored max stress level if it increased, after that the enemy wave will spawn with normal zombies. It will then decide the enemy spawn frequency decided on how long the builUp stage is. 

```c#
    void BuildUp()
    {
        m_LevelStage = stage.buildUp;

        if (m_StressLevel.CurrentStress > m_MaxStressBuildUp)
            m_MaxStressBuildUp = m_StressLevel.CurrentStress;

        SpawnManager.Instance.ChangeGameObjects(m_NormalZombie);
        SpawnManager.Instance.SpawnWave();
        m_CurrentFrequency = m_PhaseLength / (m_PhaseLength / (8));

        Invoke("DirectorLoop", m_CurrentFrequency);
    }
```

#### Peak

The peak stage starts with writing the stage to the HUD same as the BuildUp. It will also update the stress the same way as the BuildUp stage. The first time this function gets called in this cycle it will decide which type of enemy will spawn based on the stress level in the previous stage. Then it will spawn the new enemies and the spawn frequency will depend on the max stress level in the buildup stage.

```c#
    void Peak()
    {
        m_LevelStage = stage.peak;

        if (m_StressLevel.CurrentStress > m_MaxStressPeak)
            m_MaxStressPeak = m_StressLevel.CurrentStress;

        if (m_ChangeTemplateOnce == false)
        {
            if (m_MaxStressBuildUp < 50)
            SpawnManager.Instance.ChangeGameObjects(m_HeavyZombie);
            else if (m_MaxStressBuildUp > 50)
            SpawnManager.Instance.ChangeGameObjects(m_FastZombie);

            m_ChangeTemplateOnce = true;
        }

        SpawnManager.Instance.SpawnWave();
        m_CurrentFrequency = m_PhaseLength / (40 / ((m_MaxStressBuildUp + 20)/ 10));
        Invoke("DirectorLoop", m_CurrentFrequency);
    }
```

#### Relax

The start of the relax stage is the same as the 2 other stages. It will write the current stage to the HUD. Then it will calculate the average stress level from the buildUp and peak stage. It will begin a timer and decide how long the relax stage is based on the average stress level. Then when the timer reaches that value it will start a new cycle and spawn new pickups (see title below). This function is also calls DirectorLoop more than the other stages because its important that the timer is frequently updated.

```c#
    void Relax()
    {
        m_LevelStage = stage.relax;
        m_AverageStress = (m_MaxStressBuildUp + m_MaxStressPeak) / 2;
        m_RelaxTimer += Time.deltaTime;

        if (m_RelaxTimer > m_AverageStress / 4)
        {
            m_Timer = 0.0f;
            m_MaxStressBuildUp = 0.0f;
            m_MaxStressPeak = 0.0f;
            m_PhaseLength += 2.0f;
            m_RelaxTimer = 0.0f;
            m_DoOnce = false;
            SpawnManager.Instance.ChangeGameObjects(m_NormalZombie);
            m_SpawnPickUpOnce = false;
            HandlePickUp();
        }
        Invoke("DirectorLoop",0.01f);
    }
```

#### PickUps

pickUps spawn in the relax stage of the cyclus as mentioned before. They have preset spawn locations on blue quads but how many will spawn in the relax stage is dependent on the max stress level in the buildUp and peak stage. How higher those 2 value were how more pickups will spawn. The pickups exist out of health and ammo.

```c#
    void HandlePickUp() //gets called in relax stage
    {
        if (m_SpawnPickUpOnce == false)
        {
            m_PickUpSpawn1.SpawnPickUp();

            if (m_AverageStress > 20)
                m_PickUpSpawn2.SpawnPickUp();

            if (m_AverageStress > 40)
                m_PickUpSpawn3.SpawnPickUp();

            if (m_AverageStress > 60)
                m_PickUpSpawn4.SpawnPickUp();

            if (m_AverageStress > 80)
                m_PickUpSpawn5.SpawnPickUp();

            if (m_AverageStress > 90)
                m_PickUpSpawn6.SpawnPickUp();

            m_SpawnPickUpOnce = true;
        }
    }
```

### Conclusion

Director AI's can be used to decide the flow of the game or to make the game more dynamic. The best director AI's will do both at the same time like alien isolation and left 4 dead. This implementation although simplistic tries to show this using the stress level. The feeling is that this is the reason that director AI's are beign used a lot in games, even if they have a smaller function like in far cry. The assumption after the prject was made is that director AI's are here to stay and enhance our gaming experience behind the scenes.

### Sources

general:

https://www.youtube.com/watch?v=Mnt5zxb8W0Y&t=569s&ab_channel=AIandGames

alien isolation:

https://www.gamasutra.com/blogs/TommyThompson/20171031/308027/The_Perfect_Organism_The_AI_of_Alien_Isolation.php

https://www.gamasutra.com/blogs/TommyThompson/20200520/363134/Revisiting_the_AI_of_Alien_Isolation.php

https://www.pcgamesn.com/interview-creative-assembly-alien-isolations-terrifying-alien-ai

left 4 dead:

https://left4dead.fandom.com/wiki/The_Director#:~:text=The%20Director%20(sometimes%20referred%20to,dramatics%2C%20pacing%2C%20and%20difficulty.

https://medium.com/@t2thompson/in-the-directors-chair-the-ai-of-left-4-dead-78f0d4fbf86a





