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
    [SerializeField] private Text m_GrenadeText = null;
    [SerializeField] private Text m_GrenadeCounter = null;
    [SerializeField] private RectTransform m_PanelEndScreen = null;
    private Health m_PlayerHealth = null;
    private ShootingBehaviour m_PlayerShootingBehaviour = null;
    private LevelLogic m_LevelLogic = null;

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

    }
}
