using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public MonsterData monsterData;

    private float moveSpeed = 5.0f;
    private NavMeshAgent navMeshAgent;

    private Vector3 goal;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    /*
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
    */

    public void MoveTo(Vector3 goalPosition)
    {
        StopCoroutine("OnMove");
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.SetDestination(goalPosition);
        goal = goalPosition;
        StartCoroutine("OnMove");
    }

    IEnumerator OnMove()
    {
        while( true )
        {
            if ( Vector3.Distance(goal, transform.position) < 3f )
            {
                Destroy(gameObject);
            }

            if ( Vector3.Distance(navMeshAgent.destination, transform.position) < 0.15f )
            {
                transform.position = navMeshAgent.destination;
                navMeshAgent.ResetPath();

                break;
            }

            yield return null;
        }
    }
}
