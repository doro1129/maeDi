using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Item and Probability for MinableObject
/// </summary>
[System.Serializable]
public struct ProbabilityItem
{
    public float probability;
    public GameObject item;
}

/// <summary>
/// Minable Object
/// </summary>
public class MinableObject : MonoBehaviour
{
    /// <summary>
    /// Hit Point
    /// (It can be changed at runtime,
    /// but the Reset function must be called after changing the value)
    /// </summary>
    public float HitPoint = 1;

    /// <summary>
    /// Drop Items
    /// </summary>
    public List<ProbabilityItem> DropItems = new List<ProbabilityItem>();

    /// <summary>
    /// Current Hit Point (Readonly)
    /// </summary>
    public float CurrentPoint { get { return currentPoint; } }

    private float currentPoint;

    private void Awake()
    {
        // Reset current point at start of the MinableObject
        Reset();
    }

    /// <summary>
    /// Reset the HitPoint
    /// </summary>
    public void Reset()
    {
        currentPoint = HitPoint;
    }

    /// <summary>
    /// Hit and check the HitPoint
    /// (When HitPoint becomes negative, it is reset)
    /// </summary>
    /// <param name="point">Attack point</param>
    /// <returns>Whether HitPoint is negative or not</returns>
    public bool Hit(float point)
    {
        currentPoint -= point;

        bool result = currentPoint <= 0;
        if (result) Reset();

        return result;
    }

    /// <summary>
    /// Get a random item from DropItems
    /// </summary>
    /// <returns>A random item</returns>
    public GameObject GetItem()
    {
        Debug.Assert(DropItems.Count != 0, "DropItems must contain at least element.");

        float random = Random.Range(0.0f, 1.0f);
        float sumOfProbability = DropItems.Sum(x => x.probability);

        float current = 0;
        GameObject item = null;

        for (int i = 0; i < DropItems.Count; i ++)
        {
            float probability = DropItems[i].probability / sumOfProbability;
            current += probability;

            if (current >= random)
            {
                item = DropItems[i].item;
                break;
            }
        }

        Debug.Assert(item != null, "Item is null.");
        return item;
    }
}
