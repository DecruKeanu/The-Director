using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //including the UnityEngine.UI namespace
    [SerializeField] private Image m_HealthBar = null;
    [SerializeField] private Text m_PrimaryAmmo = null;
    [SerializeField] private Text m_TotalAmmo = null;
    [SerializeField] private Text m_MoneyScore = null;
    [SerializeField] private Text m_GrenadeText = null;
    [SerializeField] private Text m_GrenadeCounter = null;
    [SerializeField] private Text m_BombText = null;
    [SerializeField] private Text m_BombCounter = null;
    [SerializeField] private RectTransform m_PanelEndScreen = null;
    [SerializeField] private Text m_GameOverMoneyScore = null;
    [SerializeField] private Text m_GameWonMoneyScore = null;
    [SerializeField] public RectTransform m_PanelEndVictoryScreen = null;
    private Health m_PlayerHealth = null;
    private ShootingBehaviour m_PlayerShootingBehaviour = null;
    private LevelLogic m_LevelLogic = null;
    private float m_Timer1;
    private float m_Timer2;
    void Start()
    {
        //find an object of type player
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        //find an object of type level
        m_LevelLogic = FindObjectOfType<LevelLogic>();
        //if player exist acces its health and shootingbehaviour script and store it
        if (player)
        {
            m_PlayerHealth = player.GetComponent<Health>();
            m_PlayerShootingBehaviour = player.GetComponent<ShootingBehaviour>();
        }
        //assign our timers with their startvalues 
        m_Timer1 = m_LevelLogic.m_ExplosionTimer1;
        m_Timer2 = m_LevelLogic.m_ExplosionTimer2;
    }

    private void Update()
    {
        //keeps our HUDValues updated
        SyncData();
        if (!m_PlayerHealth)
            return;

        //if our player has is dead pause the game,enable the gameOverScreen and restart the game when the player presses R
        if (m_PlayerHealth.HeatlhPercentage <= 0)
        {
            m_PanelEndScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }

        if (!m_LevelLogic)
            return;

        //if our player won the game pause the game, enable the VictoryScreen and restart the game when the player presses R
        if (m_LevelLogic.m_GameWon == true)
        {
            m_PanelEndVictoryScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void SyncData()
    {
        //health
        if (m_HealthBar && m_PlayerHealth)
        {
            m_HealthBar.transform.localScale = new Vector3(m_PlayerHealth.HeatlhPercentage, 1.0f, 1.0f);
        }

        //ammo
        if (m_PrimaryAmmo && m_PlayerShootingBehaviour)
        {
            m_PrimaryAmmo.text = m_PlayerShootingBehaviour.PrimaryWeaponAmmo.ToString();
        }
        if (m_TotalAmmo && m_PlayerShootingBehaviour)
        {
            m_TotalAmmo.text = m_PlayerShootingBehaviour.totalAmmo.ToString();
        }
        //weapons
        if (m_PlayerShootingBehaviour && m_PlayerShootingBehaviour.m_WhichWeapon == ShootingBehaviour.Weapon.assaultGun)
        {
            m_GrenadeCounter.gameObject.SetActive(true);
            m_GrenadeText.gameObject.SetActive(true);

            m_GrenadeCounter.text = m_PlayerShootingBehaviour.getAssaultGun.m_CurrentGrenades.ToString();
        }
        else if (m_PlayerShootingBehaviour.m_WhichWeapon != ShootingBehaviour.Weapon.assaultGun)
        {
            m_GrenadeCounter.gameObject.SetActive(false);
            m_GrenadeText.gameObject.SetActive(false);
        }

        //Values dependent of levelLogic
        if (!m_LevelLogic)
            return;

        if (m_LevelLogic.m_IsBombPlaced1 == true)
        {
            if (m_Timer1 > 0.0f)
            {
                m_BombText.gameObject.SetActive(true);
                m_BombCounter.gameObject.SetActive(true);
                m_Timer1 -= Time.deltaTime;
                m_BombCounter.text = m_Timer1.ToString();
            }
            else
            {
                m_BombText.gameObject.SetActive(false);
                m_BombCounter.gameObject.SetActive(false);
            }
        }
        if (m_LevelLogic.m_IsBombPlaced2 == true)
        {
            if (m_Timer2 > 0.0f)
            {
                m_BombText.gameObject.SetActive(true);
                m_BombCounter.gameObject.SetActive(true);
                m_Timer2 -= Time.deltaTime;
                m_BombCounter.text = m_Timer2.ToString();
            }
            else
            {
                m_BombText.gameObject.SetActive(false);
                m_BombCounter.gameObject.SetActive(false);
            }
        }
        m_MoneyScore.text = m_LevelLogic.MoneyScore.ToString();
        m_GameOverMoneyScore.text = m_LevelLogic.MoneyScore.ToString();
        m_GameWonMoneyScore.text = m_LevelLogic.MoneyScore.ToString();
    }
}
