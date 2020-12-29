using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField] private float m_FirstWaveStart = 0.0f;
    [SerializeField] private float m_WaveStartFrequency = 15.0f;
    [SerializeField] private float m_WaveEndFrequency = 7.0f;
    [SerializeField] private float m_WaveFrequencyIncrement = 0.5f;
    private stage m_LevelStage = stage.introduction;
    private float m_CurrentFrequency = 0.0f;
    private LevelLogic m_Level;
    private bool m_DoOnce = false;

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
    }

    private void Update()
    {
        if (m_Level.m_GameStarted == false)
        {
            if (m_DoOnce == false)
            {
                m_LevelStage = stage.buildUp;
                Invoke("StartNewWave", m_FirstWaveStart);
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
    private void StartNewWave()
    {
        SpawnManager.Instance.SpawnWave();

        m_CurrentFrequency = Mathf.Clamp(m_CurrentFrequency - m_WaveFrequencyIncrement, m_WaveEndFrequency, m_WaveStartFrequency);

        Invoke("StartNewWave", m_CurrentFrequency);
    }
}
