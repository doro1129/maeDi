using System;
using UnityEngine;

/// <summary>
/// Weapon will be fixed in the player's view
/// Weapon will smoothly rotate with player's view as much as the value of "Smooth"
/// </summary>
public class WeaponSway : MonoBehaviour
{
    // Adjust how the weapon sway feels
    [Header("Sway Weapon")]
    [SerializeField] private float Smooth;
    [SerializeField] private float Multiplier;

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Multiplier;

        // Calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // Rotate
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Smooth * Time.deltaTime);
    }
}