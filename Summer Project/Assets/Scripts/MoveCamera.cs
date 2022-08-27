using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move camera with player
/// </summary>
public class MoveCamera : MonoBehaviour
{
    /// <summary>
    /// Update the camera position where the player is.
    /// Camera Position shold be the player's camera.
    /// </summary>
    public Transform cameraPosition;

    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
