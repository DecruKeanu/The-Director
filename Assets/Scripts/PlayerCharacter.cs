using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BasicCharacter
{
    private Plane m_CursorMovementPlane;
    public bool m_IsSettingWeapon;
    private float m_StressTimer = 0.0f;
    private StressLevel m_StressLevel = null;
    protected override void Awake()
    {
        base.Awake();
        m_CursorMovementPlane = new Plane(Vector3.up, transform.position);
        m_StressLevel = FindObjectOfType<StressLevel>();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleFireInput();
        HandleStressLevel();
    }

    void HandleMovementInput()
    {
        if (!m_MovementBehaviour)
        {
            return;
        }
        //movement
        float horizontalMovement = Input.GetAxis("MovementHorizontal");
        float verticalMovement = Input.GetAxis("MovementVertical");

        Vector3 movement = horizontalMovement * Vector3.right + verticalMovement * Vector3.forward;

        m_MovementBehaviour.DesiredMovementDirection = movement;

        //rotation
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 positionOfMouseInWorld = transform.position;

        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo,100000.0f,LayerMask.GetMask("Ground")))
        {
            positionOfMouseInWorld = hitInfo.point;
        }
        else
        {
            m_CursorMovementPlane.Raycast(mouseRay, out float distance);
            positionOfMouseInWorld = mouseRay.GetPoint(distance);
        }
        if (m_IsSettingWeapon == false)
        {
            m_MovementBehaviour.DesiredLookatPoint = positionOfMouseInWorld;
        }
    }

    void HandleFireInput()
    {
        if (!m_ShootingBehaviour)
        {
            return;
        }

        //fire
        m_ShootingBehaviour.m_IsAssaultGun = false;
        if (m_ShootingBehaviour.m_IsAssaultGun == false)
        {
            if (Input.GetAxis("PrimaryFire") > 0.0f)
            {
                m_ShootingBehaviour.PrimaryFire();
            }
            if (Input.GetAxis("SecondaryFire") > 0.0f)
            {
                m_ShootingBehaviour.SecondaryFire();
            }
        }
        else if (m_ShootingBehaviour.m_IsAssaultGun == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                m_ShootingBehaviour.PrimaryFire();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                m_ShootingBehaviour.SecondaryFire();
            }
        }

        //reload
        if (Input.GetAxis("Reload") > 0.0f)
        {
            m_ShootingBehaviour.Reload();
        }
    }

    void HandleStressLevel()
    {
        m_StressTimer += Time.deltaTime;
        if (m_StressTimer > 4.0)
        {
            if (m_StressLevel.CurrentStress > 0)
            {
                m_StressLevel.decreaseStress(5);
                m_StressTimer = 0.0f;
            }
        }
    }
}
