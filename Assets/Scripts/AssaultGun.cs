using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AssaultGun : MonoBehaviour
{
    [SerializeField] private GameObject m_BulletTemplate = null;
    [SerializeField] private GameObject m_Grenade = null;
    [SerializeField] private int m_ClipSize = 18;
    [SerializeField] private int m_TotalAmmo = 90;
    [SerializeField] private float m_FireRate = 10.0f;
    [SerializeField] private int m_GrenadeSize = 2;
    [SerializeField] private List<Transform> m_FireSockets = new List<Transform>();
    [SerializeField] private AudioSource m_FireSound = null;
    private bool m_PrimaryFireTriggered = false;
    private bool m_SecondaryFireTriggered = false;
    private int m_CurrentAmmo = 50;
    public int m_CurrentGrenades = 2;
    private float m_FireTimer = 0.0f;
    private bool m_Bursting = false;

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
        //initialises our ammo with the given values
        m_CurrentAmmo = m_ClipSize;
        m_CurrentGrenades = m_GrenadeSize;
    }

    private void Update()
    {
        //handle the countdown of the fire timer
        if (m_FireTimer > 0.0f)
        {
            m_FireTimer -= Time.deltaTime;
        }

        //if player presses primary fire and he hasnt shot yet in the firerate yet shoot one burst
        if (m_FireTimer <= 0.0f && m_PrimaryFireTriggered && m_Bursting == false)
        {
            m_Bursting = true;
            StartCoroutine(BurstFire());
        }

        //if player presses secondary fire and he hasnt shot yet in the firerate yet shoot a grenade
        if (m_FireTimer <= 0.0f && m_SecondaryFireTriggered)
        {
            FireGrenade();
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

        //spawn the bullet at the firesocket
        for (int i = 0; i < m_FireSockets.Count; i++)
        {
            Instantiate(m_BulletTemplate, m_FireSockets[i].position, m_FireSockets[i].rotation);
        }

        //set the time so we respect the firerate

        m_FireTimer += 1.0f / m_FireRate;
        if (m_FireSound)
        {
            m_FireSound.Play();
        }
    }

    private void FireGrenade()
    {
        //no ammo, we can't fire
        if (m_CurrentGrenades <= 0)
        {
            return;
        }

        //no grenade to launch
        if (m_Grenade == null)
        {
            return;
        }

        //consume a grenade
        --m_CurrentGrenades;

        //spawn the grenade at the firesocket
        for (int i = 0; i < m_FireSockets.Count; i++)
        {
            Instantiate(m_Grenade, m_FireSockets[i].position, m_FireSockets[i].rotation);
        }

        //set the time so we respect the firerate

        m_FireTimer += (1.0f / m_FireRate) / 3;
        if (m_FireSound)
        {
            m_FireSound.Play();
        }
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

    private IEnumerator BurstFire()
    {
        //shoot 3 times after each other (burst)
        FireProjectile();
        yield return new WaitForSeconds(m_FireRate);
        FireProjectile();
        yield return new WaitForSeconds(m_FireRate);
        FireProjectile();
        m_Bursting = false;
        m_FireTimer = 1.0f;
    }
}