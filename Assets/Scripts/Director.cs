using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField] private float m_FirstWaveStart = 0.0f;
    [SerializeField] private float m_WaveStartFrequency = 15.0f;
    [SerializeField] private float m_WaveEndFrequency = 7.0f;
    [SerializeField] private float m_WaveFrequencyIncrement = 0.5f;
    private float m_PhaseLength = 60.0f;
    private stage m_LevelStage = stage.introduction;
    private StressLevel m_StressLevel = null;
    private float m_CurrentFrequency = 0.0f;
    private LevelLogic m_Level;
    private bool m_DoOnce = false;
    private float m_Timer = 0.0f;
    private float m_RelaxTimer = 0.0f;
    private float m_MaxStressBuildUp = 0.0f;
    private float m_MaxStressPeak = 0.0f;
    private float m_AverageStress = 0.0f;
    private float m_CycleIncrease = 0.0f;
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
        m_CurrentFrequency = m_WaveStartFrequency;

        //searches for the LevelLogic in our level and stores it
        m_Level = FindObjectOfType<LevelLogic>();

        m_StressLevel = FindObjectOfType<StressLevel>();
        m_MaxStressBuildUp = m_StressLevel.CurrentStress;
    }

    private void Update()
    {
        m_Timer += Time.deltaTime;
      //  Debug.Log(m_Timer);
        if (m_Level.m_GameStarted == false)
        {
            if (m_DoOnce == false)
            {
                m_LevelStage = stage.buildUp;
                Invoke("StartNewWave", m_FirstWaveStart);
                m_DoOnce = true;
            }
        }

        if (m_Timer > m_PhaseLength + m_PhaseLength / 2)
        {
            m_LevelStage = stage.relax;
            m_AverageStress = (m_MaxStressBuildUp + m_MaxStressPeak) * 2;

            m_RelaxTimer += Time.deltaTime;
            Debug.Log(m_RelaxTimer);
            if (m_RelaxTimer > m_AverageStress / 4)
            {
                m_Timer = 0.0f;
                m_CycleIncrease += 1.0f;
                m_MaxStressBuildUp = 0.0f;
                m_MaxStressPeak = 0.0f;
                m_PhaseLength += 10.0f;
                m_DoOnce = false;
            }
        }


        //when the player presses escape he can quite the build
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        
    }

    //spawn enemies at the start of a new wave
    private void StartNewWave()
    {
        if (m_Timer < m_PhaseLength)
        {
            if (m_StressLevel.CurrentStress > m_MaxStressBuildUp)
                m_MaxStressBuildUp = m_StressLevel.CurrentStress;

            SpawnManager.Instance.SpawnWave();

            m_CurrentFrequency = m_PhaseLength / (m_PhaseLength /(8 - m_CycleIncrease));

            Invoke("StartNewWave", m_CurrentFrequency);
        }
        else if (m_Timer < m_PhaseLength + m_PhaseLength / 2)
        {
            m_LevelStage = stage.peak;

            if (m_StressLevel.CurrentStress > m_MaxStressPeak)
                m_MaxStressPeak = m_StressLevel.CurrentStress;

            SpawnManager.Instance.SpawnWave();

            m_CurrentFrequency = m_PhaseLength/(50/(m_MaxStressBuildUp/10));

            Invoke("StartNewWave", m_CurrentFrequency);
        }
    }
}
