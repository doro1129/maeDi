using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StageRound
{
    public List<MonsterSpawnOrder> SpawnOrder;
}

[Serializable]
public struct MonsterSpawnOrder
{
    public int Type;
    public int SpawnPosition;
    public int DestinationPosition;
    public int Length;
}

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsterPrefab;
    public Transform[] spawnTransforms;
    public Transform[] destinationTransforms;

    public List<StageRound> rounds = new List<StageRound>();

    private int currentCount = 0;
    private int currentRound = 0;
    private int currentEnemy = 0;

    float roundTimer;
    float spawnTimer;

    public int roundDelay;

    // Start is called before the first frame update
    void Awake()
    {
        roundTimer = 0.0f;
        spawnTimer = 0.0f;
    }

    private void Update()
    {
        roundTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        SpawnMonster();
    }

    public void SpawnMonster()
    {
        if (currentRound < rounds.Count &&
            roundTimer >= roundDelay)
        {
            MonsterSpawnOrder enemy = rounds[currentRound].SpawnOrder[currentEnemy];
            Monster curmonster = monsterPrefab[enemy.Type].GetComponent<Monster>();

            if (spawnTimer < curmonster.monsterData.spawnDelay) return;

            GameObject enemyObject = monsterPrefab[enemy.Type];

            Monster newMonster = Instantiate(enemyObject,
                spawnTransforms[enemy.SpawnPosition].position,
                enemyObject.transform.rotation).GetComponent<Monster>();
            newMonster.name = newMonster.monsterData.monsterName;

            newMonster.MoveTo(destinationTransforms[enemy.DestinationPosition].position);

            spawnTimer = 0.0f;
            currentCount++;

            if (currentCount >= enemy.Length)
            {
                currentCount = 0;
                currentEnemy++;
            }

            if (currentEnemy >= rounds[currentRound].SpawnOrder.Count)
            {
                currentEnemy = 0;
                currentRound++;
                roundTimer = 0.0f;
            }
        }
    }
}
