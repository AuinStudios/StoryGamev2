// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this AiTest do?
/// </summary>
public sealed class AiTest : MonoBehaviour
{
	private enum AgentStates
    {
		wander,
		aggro,
		attack,
		die,
        specialattack,
    }

    private float distanceBetweenPlayer;

    private Vector3 targetPosition;

    private bool isMoving = false;
    private bool isWaiting = false;
    private bool isWaitingForAttack = false;

    [Header("Agent Propertys")] 
    [SerializeField]
    private EnemysScriptableobject stats;
    [SerializeField, Range(1.0f, 15.0f)]
    private float agentVisibility = 5.0f;
    [SerializeField, Range(0.1f, 2.0f)]
    private float distanceThreashold = 1.0f;
    [SerializeField]
	private Transform player;
    [SerializeField]
    private AgentStates agentState = AgentStates.wander;
   // [SerializeField, Range(1.0f, 5.0f)]
   // private float speed = 3.0f;

    [Header("Delay between patrols")]
    [SerializeField, Range(0.5f, 5.0f)]
    private float minDelay = 2.5f;
    [SerializeField, Range(6.0f, 10.0f)]
    private float maxDelay = 8.0f;
    private float currentDelay;
  
    // Start is called before the first frame update
    private void Start()
	{
        
	}

#if old
    #region Old code

    // Update is called once per frame
    private void Update()
	{
        switch (agentState)
        {
            case AgentStates.wander:
            {
                if (!isMoving)
                {
                    Debug.Log("Finding new position to patrol.");

                    targetPosition = transform.position + Random.insideUnitSphere * agentVisibility;

                    isMoving = true;
                }
                else
                {
                    agent.SetDestination(targetPosition);

                    if (agent.remainingDistance <= distanceThreashold && !isWaiting)
                    {
                        StartCoroutine(WaitForNextPatrol());
                    }
                }
                distanceBetweenPlayer = Vector3.Distance(player.position, transform.position);
                if (distanceBetweenPlayer <= 20)
                {
                    agent.speed = stats.speed * 2;
                    agentState = AgentStates.aggro;

                }
            }
            break;
            case AgentStates.aggro:
            {
                agent.SetDestination(player.position);
                distanceBetweenPlayer = Vector3.Distance(player.position, transform.position);
                if (distanceBetweenPlayer >= 30)
                {
                    agent.speed = stats.speed;
                    agentState = AgentStates.wander;
                }
                else if (distanceBetweenPlayer <= 3)
                {
                    agentState = AgentStates.attack;
                    agent.speed = stats.speed;
                }
            }
            break;
            case AgentStates.attack:
            {
                distanceBetweenPlayer = Vector3.Distance(player.position, transform.position);
                // play some hiting animationa and if player hits it he gets damaged
               
                
                if(distanceBetweenPlayer > 3 && !isWaitingForAttack)
                {
                    agentState = AgentStates.aggro;
                    agent.speed = stats.speed * 2;
                }
                else if (distanceBetweenPlayer < 2)
                {
                   agent.velocity = new Vector3(0, 0, 0);
                } 
                else if (!isWaitingForAttack)
                {
                    isWaitingForAttack = true;
                    StartCoroutine(WaitforNextAttack());
                }
            }
            break;
            case AgentStates.die:
            {

            }
            break;
            case AgentStates.specialattack:
            {

            }
            break;
            default:
            break;
        }
	}

    private IEnumerator WaitForNextPatrol()
    {
        // this is patrol
        isWaiting = true;
        
        currentDelay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(currentDelay);
        
        isMoving = false;
        isWaiting = false;
    }

    private IEnumerator WaitforNextAttack()
    {
        // this is attack
        
        Debug.Log("hit");
        agent.speed = 0;
        agent.velocity = new Vector3(0,0,0);
        currentDelay = Random.Range(stats.minAttackDelay, stats.minAttackDelay);
        yield return new WaitForSeconds(currentDelay);
        agent.speed = stats.speed * 2;
        isWaitingForAttack = false;
    }
    #endregion
#endif

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, agentVisibility);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceThreashold);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
    }
}
