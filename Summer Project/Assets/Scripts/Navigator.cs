using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Navigator of 3D Tiled Map for Unity
/// </summary>
public class Navigator : MonoBehaviour
{
    /// <summary>
    /// Target GameMap
    /// </summary>
    public GameMap TargetGameMap;

    /// <summary>
    /// Is debugging mode
    /// </summary>
    public bool IsDebugging = false;

    /*
     * +-------------<GIZMO>-------------+
     * |                                 |
     * |               +Y       /        |
     * |                |     /          |
     * |                |   /            |
     * |                | /              |
     * |   ------------(*)---------- +Z  |
     * |              / |                |
     * |            /   |                |
     * |          /     |                |
     * |        +X      |                |
     * |                                 |
     * +---------------------------------+
     */

    /// <summary>
    /// Navigation Directions
    /// </summary>
    private static Vector3Int[] Directions = {    // 26's Directions
        new Vector3Int(-1,  0,  0),    // X AXIS
        new Vector3Int( 1,  0,  0),
        new Vector3Int( 0,  1,  0),    // Y AXIS
        new Vector3Int( 0, -1,  0),
        new Vector3Int( 0,  0,  1),    // Z AXIS
        new Vector3Int( 0,  0, -1),
        new Vector3Int(-1,  0, -1),    // EDGES OF XZ PLANE
        new Vector3Int(-1,  0,  1),
        new Vector3Int( 1,  0,  1),
        new Vector3Int( 1,  0, -1),
        new Vector3Int(-1,  1,  0),    // EDGES OF XY PLANE
        new Vector3Int(-1, -1,  0),
        new Vector3Int( 1, -1,  0),
        new Vector3Int( 1,  1,  0),
        new Vector3Int( 0,  1, -1),    // EDGES OF YZ PLANE
        new Vector3Int( 0,  1,  1),
        new Vector3Int( 0, -1, -1),
        new Vector3Int( 0, -1,  1),
        new Vector3Int(-1,  1, -1),    // OTHERS
        new Vector3Int(-1,  1,  1),
        new Vector3Int(-1, -1, -1),
        new Vector3Int(-1, -1,  1),
        new Vector3Int( 1,  1, -1),
        new Vector3Int( 1,  1,  1),
        new Vector3Int( 1, -1, -1),
        new Vector3Int( 1, -1,  1),
    };

    /// <summary>
    /// Initializing Multidimensional Arrays
    /// </summary>
    /// <param name="array">Arrays</param>
    /// <param name="value">Default Value</param>
    /// <typeparam name="T">The data type for element of arrays</typeparam>
    private void FillMapArray<T>(ref T[][,] array, T value)
    {
        int depth = this.TargetGameMap.MaxSize.z;

        array = new T[depth][,];

        for (int z = 0; z < depth; z++)
        {
            var height = this.TargetGameMap.Graph[z].GetLength(0);
            var width = this.TargetGameMap.Graph[z].GetLength(1);

            array[z] = new T[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    array[z][y, x] = value;
                }
            }
        }
    }

    /// <summary>
    /// Highlighting a tile for debugging
    /// (IsDebugging required)
    /// </summary>
    /// <param name="position">Position of the tile</param>
    /// <param name="color">Highlighting color</param>
    private void DebugTile(Vector3Int position, Color color)
    {
        // IsDebugging required
        if (!IsDebugging) return;

        // Get a tile and MeshRenderer component
        var tileObject = this.TargetGameMap.Graph[position.z][position.y, position.x];
        var meshRenderer = tileObject.GetComponent<MeshRenderer>();

        // Set color of the material
        var highlightMaterial = new Material(meshRenderer.sharedMaterial);
        highlightMaterial.color = color;

        // Apply the material
        meshRenderer.sharedMaterial = highlightMaterial;
    }

    /// <summary>
    /// Get shortest path between start to destination
    /// </summary>
    /// <param name="startPosition">Start Position</param>
    /// <param name="destinationPosition">Destination Position</param>
    /// <param name="directionCount">The Count of Navigator.Directions</param>
    /// <param name="maxSearchCount">Limitation of the searching count</param>
    /// <returns>The path to destination</returns>
    public List<Transform> ShortestPath(
        Vector3Int startPosition,
        Vector3Int destinationPosition,
        int directionCount = 26,
        int maxSearchCount = 1000
    )
    {
        // Map size
        var mapSize = this.TargetGameMap.MaxSize;

        // The distances of between start to destination in the map
        var distances = new float[mapSize.z][,];
        this.FillMapArray<float>(ref distances, float.MaxValue);

        // Set previous positions
        var prev = new Vector3Int[mapSize.z][,];
        this.FillMapArray<Vector3Int>(ref prev, Vector3Int.zero);

        // Set visited positions
        var visited = new bool[mapSize.z][,];
        this.FillMapArray<bool>(ref visited, false);

        // Make a priority queue
        var nodes = new PriorityQueue<float, Vector3Int>((a, b) => a.Priority < b.Priority);

        // Set start position to nodes
        nodes.Push(0, startPosition);

        // Counter
        int counter = 0;

        while (!nodes.IsEmpty())
        {
            counter++;

            if (counter > maxSearchCount)
            {
                throw new Exception("Navigator: Exceeded the MaxSearchCount");
            }

            var current = nodes.Pop().Value;
            visited[current.z][current.y, current.x] = true;

            DebugTile(current, Color.red);

            if (current == destinationPosition)
            {
                // Find the goal!
                break;
            }

            for (int i = 0; i < directionCount; i++)
            {
                var nextPosition = current + Navigator.Directions[i];
                var nextDistance = Vector3Int.Distance(nextPosition, destinationPosition);
                
                if (!this.TargetGameMap.HasTile(nextPosition))
                {
                    // Out of range
                    continue;
                }
                else if (visited[nextPosition.z][nextPosition.y, nextPosition.x])
                {
                    // Already visited
                    continue;
                }
                else if (nextDistance > distances[nextPosition.z][nextPosition.y, nextPosition.x])
                {
                    // The nextDistance is not best distance
                    continue;
                }
                else
                {
                    var tagName = this.TargetGameMap.Graph[nextPosition.z][nextPosition.y, nextPosition.x].tag;
                    
                    if (GameMap.GetTileType(tagName) != GameMap.TileType.MonsterRoad)
                    {
                        // The next tile is not "MonsterRoad"
                        continue;
                    }
                    else
                    {
                        // Update
                        distances[nextPosition.z][nextPosition.y, nextPosition.x] = nextDistance;
                        prev[nextPosition.z][nextPosition.y, nextPosition.x] = current;

                        // Add a node
                        nodes.Push(nextDistance, nextPosition);
                    }
                }
            }
        }

        // The shortest path from the starting point to the destination
        List<Transform> path = new();
        path.Add(this.TargetGameMap.Graph[destinationPosition.z][destinationPosition.y, destinationPosition.x].transform);

        Vector3Int prevPosition = destinationPosition;
        DebugTile(prevPosition, Color.green);

        while (prevPosition != Vector3Int.zero && prevPosition != startPosition)
        {
            prevPosition = prev[prevPosition.z][prevPosition.y, prevPosition.x];
            path.Add(this.TargetGameMap.Graph[prevPosition.z][prevPosition.y, prevPosition.x].transform);

            DebugTile(prevPosition, Color.green);
        }

        // Return path
        return path;
    }
}
