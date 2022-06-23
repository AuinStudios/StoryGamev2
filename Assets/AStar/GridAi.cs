// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this aiPathFinding do?
/// </summary>

public enum GridGizmos
{
    None,
    Walkable,
    NonWalkable,
    Both,
    pathfind
}

public sealed class GridAi : MonoBehaviour
{
    [Header("Grid Config"), Tooltip("Changes apply only on Start()")]
    [SerializeField]
    private LayerMask unwalkableMask;
    [SerializeField]
    private Vector3 gridOffset = Vector3.zero;
    [SerializeField]
    private Vector3 gridSize = Vector3.one;
    [SerializeField, Range(0.125f, 1.0f)]
    private float nodeRadius = 0.25f;
    // grid final variables
    private Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeZ;

    // other variables
    // <here>
    [Header("pathfinding stuff")]
    [SerializeField]
    private Transform Player;
    [Header("Debug")]
    [SerializeField]
    private GridGizmos gridGizmos = GridGizmos.None;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2.0f;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridSize.z / nodeDiameter);

        CreateGrid();
    }
    public int Maxsize
    {
        get
        {
            return gridSizeX * gridSizeZ;
        }
    }
    // gets the neighbour nodes
    public List<Node> getneighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int z = -1; z <= 1; z++)
            {
                // if its 0 then it breaks out idk why this is here
                if( x == 0  && z == 0)
                {
                    continue;
                }
                // checks the nearby nodes
                int checkx = node.gridX + x;
                int checkz = node.GridZ + z;
                // idk what this does
                if(checkx >= 0  && checkx < gridSizeX && checkz >= 0 && checkz < gridSizeZ)
                {
                    neighbours.Add(grid[checkx, checkz]);
                }
               
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldpositon)
    {
        // TODO idk what this does so help me understand it andreas
        // i think this finds the node in the grid that the player is at
        float percentX = ((worldpositon.x + gridSize.x / 2 ) - gridOffset.x) / gridSize.x ;
        float percentZ = ((worldpositon.z + gridSize.z / 2) - gridOffset.z) / gridSize.z ;
        // this clamps the values between 0 and one 
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);
        // this puts them all togehter to get the players positon i think
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z =  Mathf.RoundToInt( (gridSizeZ - 1) * percentZ);
        // returns the  positon
        return grid[x, z];
    }
    private void CreateGrid()
    {
        Debug.LogFormat("gridSizeX: {0} | gridSizeZ: {1}", gridSizeX, gridSizeZ);

        grid = new Node[gridSizeX, gridSizeZ];
        Vector3 worldBottomLeft = transform.position - Vector3.right * ((gridSize.x / 2.0f) - gridOffset.x) - Vector3.forward * ((gridSize.y / 2.0f) - gridOffset.y);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + 
                                                       Vector3.forward * (z * nodeDiameter + nodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                grid[x, z] = new Node(walkable, worldPoint, x , z );
            }
        }
    }
    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x + gridOffset.x, transform.position.y + gridOffset.y, transform.position.z + gridOffset.z), new Vector3(gridSize.x, gridSize.y, gridSize.z));

        if (grid != null)
        {
            Node playernode = NodeFromWorldPoint(Player.position);
            foreach (Node node in grid)
            {
             

                Gizmos.color = node.walkable ? Color.green : Color.red;
               // if (playernode == node)
               // {
               //     Gizmos.color = Color.blue;
               // }
               if(path != null)
                {
                    
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.cyan;
                       
                    }
                }
                switch (gridGizmos)
                {
                    case GridGizmos.None:
                        break;
                    case GridGizmos.Walkable:
                        if (node.walkable)
                            DrawNode(node);
                        break;
                    case GridGizmos.NonWalkable:
                        if (!node.walkable)
                            DrawNode(node);
                        break;
                    case GridGizmos.Both:
                        DrawNode(node);
                        break;
                    case GridGizmos.pathfind:
                        if (path != null)
                        {

                            if (path.Contains(node))
                            {
                                Gizmos.color = Color.cyan;
                                DrawNode(node);
                            }
                        }
                        break;
                }
            }
        }
    }
    private void DrawNode(Node node)
    {
        Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
    }
}
