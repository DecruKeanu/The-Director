using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform m_PlayerTransform;
    private Vector3 m_CameraOffset;

    [Range(0.01f,1.00f)]
    public float smoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        m_CameraOffset = transform.position - m_PlayerTransform.position;
    }

    // LateUpdate is called after update methods
    void Update()
    {
        Vector3 newPos = m_PlayerTransform.position + m_CameraOffset;

        transform.position = Vector3.Slerp(transform.position,newPos,smoothFactor);
    }
}
