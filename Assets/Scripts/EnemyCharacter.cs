using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyCharacter : BasicCharacter
{
    private GameObject m_PlayerTarget = null;
    [SerializeField] private float m_AttackRange = 2.0f;
    [SerializeField] private float m_FireTimer = 1.0f;
    private bool m_HasAttacked;
    private float m_Timer = 0.0f;
    private Health m_Health = null;
    [SerializeField] private GameObject m_HealthpickUp = null;
    [SerializeField] private GameObject m_AmmopickUp = null;
    private bool m_DoOnce = false;
    private float m_DropChance;
    private void Start()
    {
        //expensive method, use with caution
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();

        if(player)
        {
            m_PlayerTarget = player.gameObject;
        }
        m_Health = GetComponent<Health>();
        m_DropChance = Random.Range(0, 3);
        m_Timer = m_FireTimer;
    }

    private void Update()
    {
        HandleMovement();
        HandleAttacking();

        if (m_Timer > 0.0f)
        {
            m_Timer -= Time.deltaTime;
        }
        if (m_Timer <= 0.0f)
        {
            m_HasAttacked = false;
            m_Timer = m_FireTimer;
        }
        if (m_Health.CurrentHeatlh <= 0)
        {
            if (m_DoOnce == false)
            {
                if (m_DropChance == 1)
                {
                    if (m_HealthpickUp)
                    Instantiate(m_HealthpickUp, transform.position, transform.rotation);
                }
                else if (m_DropChance == 2)
                {
                    if (m_AmmopickUp)
                    Instantiate(m_AmmopickUp, transform.position, transform.rotation);
                }
                m_DoOnce = true;
            }
        }
    }

    private void HandleMovement()
    {
        if (m_MovementBehaviour == null)
        {
            return;
        }

        if (m_Health.HeatlhPercentage > 0)
        {
            if ((transform.position - m_PlayerTarget.transform.position).sqrMagnitude > m_AttackRange * m_AttackRange)
            {
                m_MovementBehaviour.Target = m_PlayerTarget;
            }
            else if ((transform.position - m_PlayerTarget.transform.position).sqrMagnitude < m_AttackRange * m_AttackRange)
            {
                m_MovementBehaviour.Target = this.gameObject;
            }
        }
        m_MovementBehaviour.DesiredLookatPoint = m_PlayerTarget.transform.position;
    }

    private void HandleAttacking()
    {
        if (m_ShootingBehaviour == null)
        {
            return;
        }

        if (m_PlayerTarget == null)
        {
            return;
        }

        if (m_HasAttacked)
        {
            return;
        }


        if (m_Health.HeatlhPercentage > 0)
        {
             if ((transform.position - m_PlayerTarget.transform.position).sqrMagnitude <= m_AttackRange * m_AttackRange)
             {
                 m_ShootingBehaviour.PrimaryFire();
                 if (m_ShootingBehaviour.PrimaryWeaponAmmo == 0)
                 {
                     m_ShootingBehaviour.Reload();
                 }
                m_HasAttacked = true;
             }
        }
    }
}

