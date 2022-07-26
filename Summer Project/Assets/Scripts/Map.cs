using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Joystick PlayerJoystick;

    void Start()
    {
        
    }

    public void TogglePlatformer()
    {
        PlayerJoystick.AxisDirection = JoysticAxis.Horizontal;
    }

    public void ToggleTopView()
    {
        PlayerJoystick.AxisDirection = JoysticAxis.Both;
    }
}
