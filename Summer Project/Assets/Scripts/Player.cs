using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Joystick PlayerJoystick;
    public float Speed = 5;

    private SpriteRenderer spriteRenderer;
    private Animator m_Animator;
    private bool isRightSide = true;

    void Awake()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Movement
        transform.Translate(Vector3.up * Speed * PlayerJoystick.Vertical * Time.deltaTime);
        transform.Translate(Vector3.right * Speed * PlayerJoystick.Horizontal * Time.deltaTime);

        // When player move(doMoveL, doMoveR, doStop)
        if (PlayerJoystick.Horizontal > 0)
        {
            isRightSide = true;
            m_Animator.ResetTrigger("doStop");
            m_Animator.ResetTrigger("doMoveL");
            m_Animator.SetTrigger("doMoveR");
        }
        else if (PlayerJoystick.Horizontal < 0)
        {
            isRightSide = false;
            m_Animator.ResetTrigger("doStop");
            m_Animator.ResetTrigger("doMoveR");
            m_Animator.SetTrigger("doMoveL");
        }

        if (PlayerJoystick.Horizontal == 0 || PlayerJoystick.Vertical == 0)
        {
            //m_Animator.ResetTrigger("doMoveL");
            //m_Animator.ResetTrigger("doMoveR");
            //m_Animator.SetTrigger("doStop");

            if (!isRightSide)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    public void ToggleAxisDirection()
    {
        PlayerJoystick.AxisDirection = PlayerJoystick.AxisDirection == JoysticAxis.Both
        ? JoysticAxis.Horizontal
        : JoysticAxis.Both;
    }
}
