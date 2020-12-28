using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    protected ShootingBehaviour m_ShootingBehaviour;
    protected MovementBehaviour m_MovementBehaviour;
    protected virtual void Awake()
    {
        m_ShootingBehaviour = GetComponent<ShootingBehaviour>();
        m_MovementBehaviour = GetComponent<MovementBehaviour>();
    }    
}

