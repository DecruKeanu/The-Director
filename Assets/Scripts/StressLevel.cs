using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressLevel : MonoBehaviour
{
    private int m_CurrentStress = 0;

    public float StressPercentage
    {
        get
        {
            return ((float)m_CurrentStress / 100);
        }
    }

    public float CurrentStress
    {
        get
        {
            return (m_CurrentStress);
        }
    }

    public void IncreaseStress(int amount)
    {
        m_CurrentStress += amount;

        if (m_CurrentStress >= 100)
            m_CurrentStress = 100;
    }

    public void decreaseStress(int amount)
    {
        m_CurrentStress -= amount;
    }
}
