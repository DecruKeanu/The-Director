using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SubmachineGun : MonoBehaviour
{
    [SerializeField] private GameObject m_BulletTemplate = null;
    [SerializeField] private int m_ClipSize = 50;
    [SerializeField] private float m_FireRate = 25.0f;
    [SerializeField] private List<Transform> m_FireSockets = new List<Transform>();
    private bool m_PrimaryFireTriggered = false;
    private bool m_SecondaryFireTriggered = false;
    private int m_CurrentAmmo = 50;
    private float m_FireTimer = 0.0f;
    [SerializeField] private AudioSource m_FireSound = null;
    [SerializeField] private int m_TotalAmmo = 250;
    public int CurrentAmmo
    {
        get
        {
            return m_CurrentAmmo;
        }
    }
    public int TotalAmmo
    {
        get
        {
            return m_TotalAmmo;
        }
    }
    public void increase(int amount)
    {
        m_TotalAmmo += amount;

    }

    private void Awake()
    {
        //initialises our ammo with the given value
        m_CurrentAmmo = m_ClipSize;
    }

    private void Update()
    {
        //handle the countdown of the fire timer
        if (m_FireTimer > 0.0f)
        {
            m_FireTimer -= Time.deltaTime;
        }

        //if player presses primary fire and he hasnt shot yet in the firerate call FirePrimaryProjectile();
        if (m_FireTimer <= 0.0f && m_PrimaryFireTriggered)
        {
            
           FireProjectile();
            if (CurrentAmmo > 0)
            {
                if (m_FireSound)
                {
                    m_FireSound.Play();
                }
            }
        }

        //if player presses primary fire and he hasnt shot yet in the firerate call FireSecondaryProjectile();
        if (m_FireTimer <= 0.0f && m_SecondaryFireTriggered)
        {

            FireProjectile();
        }
        //the trigger will release by itself
        //if we still are firing, we will recieve new fire input
        m_PrimaryFireTriggered = false;
        m_SecondaryFireTriggered = false;
    }
        

    private void FireProjectile()
    {
        //no ammo, we can't fire
        if (m_CurrentAmmo <= 0)
        {
            return;
        }

        //no bullet to fire
        if (m_BulletTemplate == null)
        {
            return;
        }

        //consume a bullet
        --m_CurrentAmmo;

        for (int i= 0; i < m_FireSockets.Count;i++)
        {
            Instantiate(m_BulletTemplate, m_FireSockets[i].position, m_FireSockets[i].rotation);
        }

        //set the time so we respect the firerate
       m_FireTimer += 1.0f / m_FireRate;
    }


    public void PrimaryFire()
    {
        m_PrimaryFireTriggered = true;
    }

    public void SecondaryFire()
    {
        m_SecondaryFireTriggered = true;
    }

    public void Reload()
    {
        //handle reload
        if (m_TotalAmmo > 0)
        {
            m_TotalAmmo -= (m_ClipSize - m_CurrentAmmo);
            m_CurrentAmmo = m_ClipSize;
        }
        if (m_TotalAmmo < 0)
        {
            m_TotalAmmo = 0;
        }
    }
}

