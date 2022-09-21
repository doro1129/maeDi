using UnityEngine;

/// <summary>
/// An attack game object for the tower
/// </summary>
public class TowerAttackObject : MonoBehaviour
{
    /// <summary>
    /// Firing direction
    /// </summary>
    public Vector3 direction = Vector3.zero;
    /// <summary>
    /// Speed
    /// </summary>
    public float speed = 10;
    /// <summary>
    /// Destroyed after time limit
    /// </summary>
    public float timeLimit = 3;

    private float _timestamp = 0;

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        _timestamp += Time.deltaTime;
        
        if (_timestamp >= timeLimit)
        {
            Destroy(gameObject);
        }
    }
}
