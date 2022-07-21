using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public Joystick PlayerJoystick;
    public float Speed = 10;

    void Update()
    {
        // Movement
        transform.Translate(Vector3.up * Speed * PlayerJoystick.Vertical * Time.deltaTime);
        transform.Translate(Vector3.right * Speed * PlayerJoystick.Horizontal * Time.deltaTime);
    }

    public void ToggleAxisDirection()
    {
        PlayerJoystick.AxisDirection = PlayerJoystick.AxisDirection == JoysticAxis.Both 
        ? JoysticAxis.Horizontal
        : JoysticAxis.Both;
    }
}
