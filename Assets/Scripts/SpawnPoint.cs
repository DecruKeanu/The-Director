using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject m_SpawnTemplate = null;

    private void OnEnable()
    {
        SpawnManager.Instance.RegisterSpawnPoint(this);
    }

    private void OnDisable()
    {
        SpawnManager.Instance.UnRegisterSpawnPoint(this);
    }
    //spawn the template at the location of our spawnpoint
    public GameObject Spawn()
    {
        return Instantiate(m_SpawnTemplate, transform.position, transform.rotation);
    }
}
