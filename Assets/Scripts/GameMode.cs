using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] private float m_FirstWaveStart = 0.0f;
    [SerializeField] private float m_WaveStartFrequency = 15.0f;
    [SerializeField] private float m_WaveEndFrequency = 7.0f;
    [SerializeField] private float m_WaveFrequencyIncrement = 0.5f;
    private float m_CurrentFrequency = 0.0f;
    private LevelLogic m_Level;
    private bool m_DoOnce = false;

    private void Awake()
    {
        m_CurrentFrequency = m_WaveStartFrequency;

        //searches for the LevelLogic in our level and stores it
       m_Level = FindObjectOfType<LevelLogic>();
    }

    private void Update()
    {
        //when the second bom is placed activate the enemySpawns
        if (m_Level.Bomb2Placed)
        {
            if (m_DoOnce == false)
            {
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
