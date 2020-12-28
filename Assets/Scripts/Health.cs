using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int m_StartHealth = 10;
    [SerializeField] private Color m_FlickerColor = Color.white;
    [SerializeField] private float m_FlickerDuration = 0.1f;
    private int m_CurrentHealth = 0;
    private Color m_StartColor;
    private Material m_AttachedMaterial;
    private EnemyCharacter m_Enemy = null;

    private void Start()
    {
        Renderer renderer = transform.GetComponentInChildren<Renderer>();
        if (renderer)
        {
            m_AttachedMaterial = renderer.material;

            if (m_AttachedMaterial)
            {
                m_StartColor = m_AttachedMaterial.GetColor("_Color");
            }

            m_Enemy = GetComponent<EnemyCharacter>();
        }
    }

    public float HeatlhPercentage
    {
        get
        {
            return ((float)m_CurrentHealth / m_StartHealth);
        }
    }

    public float CurrentHeatlh
    {
        get
        {
            return (m_CurrentHealth);
        }
    }
    private void Awake()
    {
        m_CurrentHealth = m_StartHealth;
    }

    public void Damage(int amount)
    {
        m_CurrentHealth -= amount;

        if (m_AttachedMaterial)
        {
            m_AttachedMaterial.SetColor("_Color", m_FlickerColor);
            Invoke("ResetColor", m_FlickerDuration);
        }


        if (m_CurrentHealth <= 0 && m_Enemy)
        {
            Invoke("Kill", 0.2f);
        }
    }

    public void heal(int amount)
    {
        if ((m_CurrentHealth + amount) < 100)
        {
            m_CurrentHealth += amount;
        }
        else
        {
            m_CurrentHealth = 100;
        }
    }

    private void ResetColor()
    {
        if (!m_AttachedMaterial)
        {
            return;
        }

        m_AttachedMaterial.SetColor("_Color", m_StartColor);
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
