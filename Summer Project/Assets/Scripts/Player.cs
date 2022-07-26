using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Joystick PlayerJoystick;
    public float Speed = 5;

    private SpriteRenderer spriteRenderer;
    private Animator m_Animator;

    void Awake()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Get joystick position
        Vector2 joystick = PlayerJoystick.Position;

        // Movement
        transform.Translate(new Vector3(1, 1, 0) * Speed * joystick * Time.deltaTime);

        // Set moving animation
        m_Animator.SetBool("isMoving", joystick != Vector2.zero);

        // Set sprite direction
        if (joystick.x != 0) spriteRenderer.flipX = joystick.x < 0;
    }
    
    public void ToggleAxisDirection()
    {
        PlayerJoystick.AxisDirection = PlayerJoystick.AxisDirection == JoysticAxis.Both
        ? JoysticAxis.Horizontal
        : JoysticAxis.Both;
    }
}
