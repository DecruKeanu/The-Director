using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    private float m_PhaseLength = 60.0f;
    private stage m_LevelStage = stage.introduction;
    private StressLevel m_StressLevel = null;
    private float m_CurrentFrequency = 0.0f;
    private LevelLogic m_Level;
    [SerializeField] PickUpSpawner m_PickUpSpawn1 = null;
    [SerializeField] PickUpSpawner m_PickUpSpawn2 = null;
    [SerializeField] PickUpSpawner m_PickUpSpawn3 = null;
    [SerializeField] PickUpSpawner m_PickUpSpawn4 = null;
    [SerializeField] PickUpSpawner m_PickUpSpawn5 = null;
    [SerializeField] PickUpSpawner m_PickUpSpawn6 = null;
    [SerializeField] GameObject m_NormalZombie = null;
    [SerializeField] GameObject m_FastZombie = null;
    [SerializeField] GameObject m_HeavyZombie = null;
    private bool m_DoOnce = false;
    private bool m_ChangeTemplateOnce = false;
    private bool m_SpawnPickUpOnce = false;
    private float m_Timer = 0.0f;
    private float m_RelaxTimer = 0.0f;
    private float m_MaxStressBuildUp = 0.0f;
    private float m_MaxStressPeak = 0.0f;
    private float m_AverageStress = 0.0f;
    public enum stage
    {
        introduction,
        buildUp,
        peak,
        relax
    }

    public stage GetCurentStage
    {
        get
        {
            return m_LevelStage;
        }
    }

    private void Awake()
    {
        //searches for the LevelLogic in our level and stores it
        m_Level = FindObjectOfType<LevelLogic>();

        m_StressLevel = FindObjectOfType<StressLevel>();
        m_MaxStressBuildUp = m_StressLevel.CurrentStress;
        m_PickUpSpawn1.SpawnPickUp();
        m_PickUpSpawn2.SpawnPickUp();
        m_PickUpSpawn3.SpawnPickUp();
        m_PickUpSpawn4.SpawnPickUp();
        m_PickUpSpawn5.SpawnPickUp();
        m_PickUpSpawn6.SpawnPickUp();
    }

    private void Update()
    {

        m_Timer += Time.deltaTime;
        if (m_Level.m_GameStarted == false)
        {
            if (m_DoOnce == false)
            {
                Invoke("DirectorLoop", 0.0f);
                m_DoOnce = true;
            }
        }

        //when the player presses escape he can quite the build
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //spawn enemies at the start of a new wave
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
        Debug.Log(m_CurrentFrequency);
        Invoke("DirectorLoop", m_CurrentFrequency);
    }

    void Relax()
    {
        m_LevelStage = stage.relax;
        m_AverageStress = (m_MaxStressBuildUp + m_MaxStressPeak) / 2;
        m_RelaxTimer += Time.deltaTime;
        Debug.Log(m_RelaxTimer);


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

    void HandlePickUp()
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
}

