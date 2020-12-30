using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyCharacter : BasicCharacter
{
    private GameObject m_PlayerTarget = null;
    [SerializeField] private float m_AttackRange = 2.0f;
    private float m_StressRange = 6.0f;
    [SerializeField] private float m_FireTimer = 1.0f;
    [SerializeField] private int m_damage = 10;
    private bool m_HasAttacked;
    private float m_Timer = 0.0f;
    private float m_StressTimer = 0.0f;
    private Health m_Health = null;
    private StressLevel m_PlayerStressLevel = null;
    private bool CanIncreaseStressLevel = false;
    private void Start()
    {
        //expensive method, use with caution
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();

        if(player)
        {
            m_PlayerTarget = player.gameObject;
            m_PlayerStressLevel = player.GetComponent<StressLevel>();
        }
        m_Health = GetComponent<Health>();
        m_Timer = m_FireTimer;
    }

    private void Update()
    {
        HandleMovement();
        HandleAttacking();

        m_StressTimer += Time.deltaTime;
        CanIncreaseStressLevel = false;
        if (m_StressTimer > 1.0f)
        {
            m_StressTimer = 0.0f;
            CanIncreaseStressLevel = true;
        }
        
        if (m_Timer > 0.0f)
        {
            m_Timer -= Time.deltaTime;
        }
        if (m_Timer <= 0.0f)
        {
            m_HasAttacked = false;
            m_Timer = m_FireTimer;
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
            if ((transform.position - m_PlayerTarget.transform.position).sqrMagnitude < m_StressRange * m_StressRange)
            {
                if (CanIncreaseStressLevel)
                m_PlayerStressLevel.IncreaseStress(m_damage/2);
            }
        }
        m_MovementBehaviour.DesiredLookatPoint = m_PlayerTarget.transform.position;
    }

    private void HandleAttacking()
    {
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
                m_PlayerTarget.GetComponent<Health>().Damage(m_damage);
                m_HasAttacked = true;
                m_PlayerStressLevel.IncreaseStress(m_damage);
             }
        }
    }
}

