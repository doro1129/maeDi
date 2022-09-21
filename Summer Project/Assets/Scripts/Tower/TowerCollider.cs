using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tower collider that detects enemies
/// </summary>
public class TowerCollider : MonoBehaviour
{
    private List<GameObject> _enemies = new List<GameObject>();
    /// <summary>
    /// Get enemy list
    /// </summary>
    public List<GameObject> Enemies { get { return _enemies; } }

    /// <summary>
    /// Collider shape visibility
    /// </summary>
    public bool IsVisible
    {
        get
        {
            return GetComponent<MeshRenderer>().enabled;
        }

        set
        {
            GetComponent<MeshRenderer>().enabled = value;
        }
    }

    private void Update()
    {
        List<int> destroyedEnemies = new List<int>();

        // Find destroyed enemy game objects
        for (int index = 0; index < _enemies.Count; index ++)
        {
            if (_enemies[index] == null)
            {
                destroyedEnemies.Add(index);
            }
        }

        // Remove all destroyed enemies
        for (int index = 0; index < destroyedEnemies.Count; index ++)
        {
            _enemies.RemoveAt(destroyedEnemies[index] - index);
        }
    }

    /// <summary>
    /// Check if there are any enemies nearby
    /// </summary>
    /// <returns>true if there is no enemy</returns>
    public bool IsEmpty()
    {
        return _enemies.Count == 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TowerData.EnemyTagName))
        {
            _enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TowerData.EnemyTagName))
        {
            _enemies.Remove(other.gameObject);
        }
    }
}
