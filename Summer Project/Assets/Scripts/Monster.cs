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
            //임시로 몬스터가 목적지 근처로 가면 destroy함.
            //이후에는 player가 hp를 다 깎아야 destroy 가능하도록 수정할 예정.
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
