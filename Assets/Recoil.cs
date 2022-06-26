// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this Recoil do?
/// </summary>
public sealed class Recoil : MonoBehaviour
{
	private Vector3 pos1;
	[SerializeField]
	private float godown = 0;
	[SerializeField]
	private Transform head;
    // Start is called before the first frame update
    private void Start()
	{
		
	}

	// Update is called once per frame
	private void Update()
	{
		pos1 = new Vector3(0, godown, 0);
		head.position = Vector3.Lerp(head.position, pos1, 1 * Time.deltaTime);
		
	}


	
}
