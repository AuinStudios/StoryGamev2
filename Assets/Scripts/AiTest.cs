// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///     What does this AiTest do?
/// </summary>
public sealed class AiTest : MonoBehaviour
{
	[SerializeField]
	private Transform player;
	[SerializeField]
	private NavMeshAgent nav;
    	// Start is called before the first frame update
    	private void Start()
	{
		
	}

	// Update is called once per frame
	private void Update()
	{
		nav.SetDestination(player.position);
	}
}
