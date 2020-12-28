using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region SINGLETON
    private static SpawnManager instance;

    public static SpawnManager Instance
    {
        get
        {
            if(instance == null && applicationQuiting == false)
            {
                //find it in case it was placed in the scene
                instance = FindObjectOfType<SpawnManager>();
                {
                    if (instance == null)
                    {
                        //none was found in the scene, create a new instance
                        GameObject newObject = new GameObject("Singleton_SpawnManager");
                        instance = newObject.AddComponent<SpawnManager>();
                    }
                }
            }
            return instance;
        }
    }
    private static bool applicationQuiting = false;

    public void OnApplicationQuit()
    {
        applicationQuiting = true;
    }
    private void Awake()
    {
        //we want this object to persist when scenes change
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private List<SpawnPoint> m_SpawnPoints = new List<SpawnPoint>();

    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        if (!m_SpawnPoints.Contains(spawnPoint))
        {
            m_SpawnPoints.Add(spawnPoint);
        }
    }
    public void UnRegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        m_SpawnPoints.Remove(spawnPoint);
    }

    //update is called once per frame
    void Update()
    {
        //remove any objects that are null
        m_SpawnPoints.RemoveAll(s => s == null);
    }

    public void SpawnWave()
    {
        foreach (SpawnPoint point in m_SpawnPoints)
        {
            point.Spawn();
        }
    }
}
