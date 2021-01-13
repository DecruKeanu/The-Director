# The-Director
The research project focuses on Director AI’s and discovers the design philosophies of making one and researching directors in existing games before making my own implementation.

### Design Philosophy

There is no wrong or correct way to develop a Director AI. It does exist out of 3 key features that define what an Director AI is. These are the following:

-	The Director AI has to record data about what the player is doing.
-	The Director AI has to adapt the game world or flow based on this data.
-	The Director AI has to enforce in-world rules and logic.

Because these 3 features are the only defining element of a Director AI, it would be better to look at games that implemented this system to understand what a director AI truly is. The following games listed are not the only implementations of the Director AI.

### Left 4 Dead

The Director AI in *Left 4 Dead* is perhaps the most well-known implementation of a Director AI and works in 3 stages. The first stage is called *the Build-up* and occurs when the players are not in danger. The Director will increase the danger level and transition to the second stage called *the Peak*. In this stage the danger level has reached a given threshold and this will trigger the Director to spawn a large horde near the players to keep them under pressure. After this, the Director transitions to the last stage called *Relax*. In this stage the Director relieves a lot of the presure and allows the players to catch their breath and heal. In this stage the Director also waits to restart the cycle but this is dependant on a number of factors. The primary factor that the Director takes into account during this whole process is the stress level of the players. This is monitored for each player separately. 

The stress level increases when the zombies:
- attack the player (stress levels increase gradually).
- are in proximity of the player (stress levels increase gradually).
- execute special attacks (stress level increase instantly).

The Director will prioritise players with a lower stress level. This is how the system works behind the scenes, but how does it change the game world itself. The Director is responsible for spawning enemies in the world and decides which pickups (at preset locations) are going to spawn according to the players' stress level. The Director AI has restrictions placed upon it to make it fairer towards the players. The Director AI also interacts with the Audio Director to give cues when certain events are happening. 

### Alien: Isolation

The Director AI in *Alien: Isolation* is complicated. It takes a lot of factors into account, which will be briefly explained. The main purpose of the Director AI is to point the alien AI towards the player by giving hints of the player's location, but the Director never shares the exact location. The way it achieves this is by working with the menace gauge. When the menace gauge is low, the Director instructs the alien AI to hunt the player. When the menace gauge is high, the Director will instruct the alien AI to enter its cooldown period. In this mode, the Alien AI will crawl into the vents and leave the player alone (for a while at least). 

The menace gauge increases when the player is:
- in a radius of the alien.
- in the line of sight of the alien.
- detects the alien on his motion tracker (and if it the alien can reach the player fast).

The alien AI itself exist out of a behaviour tree.

### Far Cry

The Director AI in the *Far Cry* series serves the main purpose of being an asset manager. The NPC's, like humans and animals, are spawned in a certain radius around the player and NPC's beyond that radius are despawned. This way the resources of the CPU and the GPU are more efficiently used. This base system is used in a lot of open world games, but *Far Cry* is a prime example.

### The Implementation

#### Preproduction

The project is a simple implementation of a Director AI in Unity using a similar system to *Left 4 Dead*. It has a *Build-up*, *Peak* and *Relax* stage that affects the game world and takes the player’s stress level into account. The game genre is a twin stick shooter. It is based on a prior Unity project for the course Game Mechanics as the foundation. It also has a different implementation of the stages and stress level, because this implementation is single player.

#### Game Mechanics

The first thing in the project that was changed were the enemy types and pickups.

#### Stress level

A stress level script and a visualised element to the HUD were added. The HUD can easily access the stress value in percent or value by the use of getter functions. The stress level is visualised as a blue bar in the HUD.

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (m_CurrentStress > 100)
            m_CurrentStress = 100;
    }

    public void decreaseStress(int amount)
    {
        m_CurrentStress -= amount;

        if (m_CurrentStress < 0)
            m_CurrentStress = 0;
    }
}
```

#### Main Loop

This is the main loop of the Director AI. It transitions between the stages dependant on the *phaseLength* which starts at 60 seconds.


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


#### Build-up

This is the *Build-up* stage of the Director. It will start by transferring information about the stage to the HUD. It will then update the current stress level as the new maximum stress level during the *Build-up* stage. The maximum stress level can only be increased. After that the enemy wave will spawn with normal zombies. The Director will then decide the enemy spawn frequency dependant on the duration of the *Build-up* stage.

```c#
    void BuildUp()
    {
        m_LevelStage = stage.buildUp;

        if (m_StressLevel.CurrentStress > m_MaxStressBuildUp)
            m_MaxStressBuildUp = m_StressLevel.CurrentStress;

        SpawnManager.Instance.ChangeGameObjects(m_NormalZombie);
        SpawnManager.Instance.SpawnWave();
        m_CurrentFrequency = m_PhaseLength / (m_PhaseLength / 8);

        Invoke("DirectorLoop", m_CurrentFrequency);
    }
```

#### Peak

The *Peak* stage starts with transferring the information of the stage to the HUD. It will also update the maximum stress level the same way as the *Build-up* stage. The first time this function gets called, in this cycle, the Director will decide which type of enemy will spawn based on the stress level in the previous stage. Then it will spawn the new enemies and the spawn frequency will depend on the maximum stress level in the *Build-up* stage.

```c#
    void Peak()
    {
        m_LevelStage = stage.peak;

        if (m_StressLevel.CurrentStress > m_MaxStressPeak)
            m_MaxStressPeak = m_StressLevel.CurrentStress;

        if (!m_ChangeTemplateOnce)
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

The start of the *Relax* stage is the same as the 2 previous stages. After the startup of the stage, the average stress level from the *Build-up* and *Peak* stage are calculated. A timer will start and decide how long the relax stage will be based on the average stress level. Then, when the timer reaches the calculated value, a new cycle will be started and new pickups will be spawned. This function calls the *DirectorLoop* more than the other stages, because it is important that the timer is frequently updated.

```c#
    void Relax()
    {
        m_LevelStage = stage.relax;
        m_AverageStress = (m_MaxStressBuildUp + m_MaxStressPeak) / 2;
        m_RelaxTimer += Time.deltaTime;

        SpawnPickUps();
        if (m_RelaxTimer > m_AverageStress / 3) //divided by 3 otherwise relax stage is too long
        {
            m_Timer = 0.0f;
            m_MaxStressBuildUp = 0.0f;
            m_MaxStressPeak = 0.0f;
            m_PhaseLength += 2.0f; //phaseLength is increased for harder difficulty
            m_RelaxTimer = 0.0f;
            m_DoOnce = false;
            SpawnManager.Instance.ChangeGameObjects(m_NormalZombie);
            m_SpawnPickUpOnce = false;
        }
        Invoke("DirectorLoop",0.01f);
    }
```

#### Pickups

Pickups spawn in the relax stage of the cyclus as mentioned before. They have preset spawn locations on blue quads and how many will spawn in the *Relax* stage is dependant on the max stress level in the *Build-up* and *Peak* stage. The pickups exist out of health and ammo.

```c#
    void SpawnPickUps()
    {
        if (!m_SpawnPickUpOnce)
        {
            m_SpawnPoints[0].SpawnPickUp();

            int stressValue = 10;
            int idx = 0;
            foreach (PickUpSpawner spawnPoint in m_SpawnPoints)
            {
                if (idx > 0 && m_AverageStress > stressValue)
                {
                    Debug.Log(m_AverageStress);
                    spawnPoint.SpawnPickUp();
                    stressValue += 20;
                }
                idx++;
            }
            m_SpawnPickUpOnce = true;
        }
    }
```

### Conclusion

Director AI's can be used to decide the flow of the game. This implementation, although limited, endeavours to show the inner working of the Director using the stress level. This is the reason that Director AI's are used a lot in games, even if they have a smaller function like in *Far Cry*. The conclusion from this project is that Director AI's are here to stay and enhance our gaming experience behind the scenes.

### Sources

General:

https://www.youtube.com/watch?v=Mnt5zxb8W0Y&t=569s&ab_channel=AIandGames

*Alien: Isolation*:

https://www.gamasutra.com/blogs/TommyThompson/20171031/308027/The_Perfect_Organism_The_AI_of_Alien_Isolation.php

https://www.gamasutra.com/blogs/TommyThompson/20200520/363134/Revisiting_the_AI_of_Alien_Isolation.php

https://www.pcgamesn.com/interview-creative-assembly-alien-isolations-terrifying-alien-ai

*Left 4 Dead*:

https://left4dead.fandom.com/wiki/The_Director#:~:text=The%20Director%20(sometimes%20referred%20to,dramatics%2C%20pacing%2C%20and%20difficulty.

https://medium.com/@t2thompson/in-the-directors-chair-the-ai-of-left-4-dead-78f0d4fbf86a





