using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_HealthpickUp = null;
    [SerializeField] private GameObject m_AmmopickUp = null;

    public void SpawnPickUp()
    {
        float dropChance = Random.Range(0, 2);
        if (dropChance == 0)
        {
            if (m_HealthpickUp)
                Instantiate(m_HealthpickUp, transform.position, transform.rotation);
        }
        else if (dropChance == 1)
        {
            if (m_AmmopickUp)
                Instantiate(m_AmmopickUp, transform.position, transform.rotation);
        }
    }
}
