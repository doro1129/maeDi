using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data", order = int.MaxValue)]

public class MonsterData : ScriptableObject
{
    public string monsterName;
    public int hp;
    public int atk;
    public float attackSpeed;
    public float moveSpeed;
    public float cooldown;
    public float spawnDelay;
}
