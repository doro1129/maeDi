using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData monsterData;

    void Update()
    {
        Move();
    }

    public void PrintMonsterData()
    {
        Debug.Log("몬스터 이름 : " + monsterData.monsterName);
        Debug.Log("체력 : " + monsterData.hp);
        Debug.Log("공격력 : " + monsterData.atk);
        Debug.Log("공격 속도 : " + monsterData.attackSpeed);
        Debug.Log("이동 속도 : " + monsterData.moveSpeed);
        Debug.Log("쿨타임 : " + monsterData.cooldown);
        Debug.Log("------------------------------------------");
    }

    void Move()
    {
        transform.Translate(Vector3.right * Time.deltaTime);
    }
}
