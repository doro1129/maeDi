using System;
using UnityEngine;

/// <summary>
/// Defence Tower Data
/// </summary>
[CreateAssetMenu(fileName = "Tower Data", menuName = "Scriptable Objects/Tower Data", order = int.MaxValue)]
public class TowerData : ScriptableObject
{
    /// <summary>
    /// A tag name of the enemy for tower
    /// </summary>
    public static readonly String EnemyTagName = "Enemy";
    
    /// <summary>
    /// Attack Point
    /// </summary>
    public float attackPoint;
    /// <summary>
    /// Attack Speed (Delay time between attack and attack)
    /// </summary>
    public float attackSpeed;
    /// <summary>
    /// Attack Object (like a bullet)
    /// </summary>
    public GameObject attackObject;
}
