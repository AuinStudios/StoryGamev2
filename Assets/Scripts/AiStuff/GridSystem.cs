// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this GridSystem do?
/// </summary>
public sealed class GridSystem : MonoBehaviour
{
	[SerializeField]
	private int gridx;
	[SerializeField]
	private int gridy;
    [SerializeField]
    private GameObject test;
    private void Start()
    {
        
        generategrid();
    }

    private void generategrid()
    {
        for(int x = 0; x < gridx; x++)
        {
            for(int y = 0; y <gridy; y++)
            {
                test.transform.position = new Vector3(x, 0, y);
              test =  Instantiate(test, test.transform.position, Quaternion.identity);
            }
        }
    }
}
