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
    public MonsterType Type;
    public SpawnTrasform Position;
    public int Length;
}

public enum MonsterType { Bear, Monkey, Penguin, Pig, Rabbit, Sheep }
public enum SpawnTrasform { TopLeft, TopRight, BottomLeft, BottomRight }

public class MonsterSpawner : MonoBehaviour
{
    public List<MonsterData> monsterDatas;
    public List<GameObject> monsterPrefab;
    public Transform[] spawnTransforms;

    public List<StageRound> rounds = new List<StageRound>();

    private int _currentCount = 0;
    private int _currentRound = 0;
    private int _currentEnemy = 0;

    float roundTimer;
    float spawnTimer;
    public int roundDelay;

    // Start is called before the first frame update
    void Start()
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
        if (_currentRound < rounds.Count &&
            roundTimer >= roundDelay)
        {
            MonsterSpawnOrder enemy = rounds[_currentRound].SpawnOrder[_currentEnemy];
            int type = (int)enemy.Type;
            int position = (int)enemy.Position;

            if (spawnTimer < monsterDatas[type].spawnDelay) return;

            GameObject enemyObject = monsterPrefab[type];

            Monster newMonster = Instantiate(enemyObject,
                spawnTransforms[position].position,
                enemyObject.transform.rotation).GetComponent<Monster>();
            newMonster.monsterData = monsterDatas[type];
            newMonster.name = newMonster.monsterData.monsterName;
            newMonster.PrintMonsterData();

            spawnTimer = 0.0f;
            _currentCount++;

            if (_currentCount >= enemy.Length)
            {
                _currentCount = 0;
                _currentEnemy++;
            }

            if (_currentEnemy >= rounds[_currentRound].SpawnOrder.Count)
            {
                _currentEnemy = 0;
                _currentRound++;
                roundTimer = 0.0f;
            }
        }
    }
}
