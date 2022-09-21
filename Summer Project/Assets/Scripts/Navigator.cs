using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    public GameMap Target;

    private void FillMapArray<T>(ref T[][,] array, T value)
    {
        int depth = this.Target.MaxSize.z;

        array = new T[depth][,];

        for (int z = 0; z < depth; z++)
        {
            var height = this.Target.Graph[z].GetLength(0);
            var width = this.Target.Graph[z].GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    array[z][y, x] = value;
                }
            }
        }
    }

    private float ManhattanDistance(Vector3 posA, Vector3 posB)
    {
        float dx = Mathf.Abs(posA.x - posB.x);
        float dy = Mathf.Abs(posA.x - posB.x);
        float dz = Mathf.Abs(posA.x - posB.x);

        return 4 * (dx + dy + dz);
    }

    public List<Transform> ShortestPath(
        Vector3Int startPosition,
        Vector3Int destinationPosition,
        int directionCount = 4,
        bool isThreeDimenstional = true
    )
    {
        // Check parameters
        Debug.Assert(directionCount == 4 || directionCount == 8, "The directionCount is expected 4 or 8");

        // Directions
        var directions = new List<Vector3Int>();

        directions.Add(Vector3Int.up);
        directions.Add(Vector3Int.right);
        directions.Add(Vector3Int.down);
        directions.Add(Vector3Int.left);

        // TODO: Working later
        // Reset the directions
        // {
        //     int y;
        //     int x;

        //     for (y = -1; y <= 1; y++)
        //     {
        //         for (x = -1; x <= 1; x++)
        //         {
        //             directions.Add(new Vector3(x, y));
        //         }
        //     }
        // }

        // Map size
        var mapSize = this.Target.MaxSize;

        // The distances of between start to destination in the map
        var distances = new float[mapSize.z][,];
        this.FillMapArray<float>(ref distances, -1);

        //
        var prev = new Vector3Int[mapSize.z][,];
        this.FillMapArray<Vector3Int>(ref prev, Vector3Int.zero);

        //
        var visited = new bool[mapSize.z][,];
        this.FillMapArray<bool>(ref visited, false);

        //
        var nodes = new PriorityQueue<float, Vector3Int>((a, b) => a.Priority < b.Priority);

        //
        nodes.Push(0, startPosition);

        while (!nodes.IsEmpty())
        {
            var current = nodes.Pop().Value;
            visited[current.z][current.y, current.x] = true;

            if (current == destinationPosition)
            {
                // Find the goal!
                break;
            }

            foreach (var direction in directions)
            {
                var nextPosition = current + direction;

                if (
                    nextPosition.z < 0 || nextPosition.z >= visited.Length ||
                    nextPosition.y < 0 || nextPosition.y >= visited[nextPosition.z].GetLength(0) || 
                    nextPosition.x < 0 || nextPosition.x >= visited[nextPosition.z].GetLength(1)
                )
                {
                    // Out of range
                    continue;
                }

                if (visited[nextPosition.z][nextPosition.y, nextPosition.x])
                {
                    // Visited
                    continue;
                }

                var nextDistance = this.ManhattanDistance(nextPosition, destinationPosition);
                if (nextDistance > distances[nextPosition.z][nextPosition.y, nextPosition.x])
                {
                    continue;
                }

                var tagName = this.Target.Graph[nextPosition.z][nextPosition.y, nextPosition.x].tag;
                if (GameMap.GetTileType(tagName) != GameMap.TileType.MonsterRoad)
                {
                    continue;
                }

                // Update
                distances[nextPosition.z][nextPosition.y, nextPosition.x] = nextDistance;
                prev[nextPosition.z][nextPosition.y, nextPosition.x] = current;

                // Add a node
                nodes.Push(nextDistance, nextPosition);
            }
        }

        //
        List<Transform> path = new();
        path.Add(this.Target.Graph[destinationPosition.z][destinationPosition.y, destinationPosition.x].transform);

        Vector3Int prevPosition = destinationPosition;

        while (prevPosition != startPosition)
        {
            prevPosition = prev[prevPosition.z][prevPosition.y, prevPosition.x];
            path.Add(this.Target.Graph[prevPosition.z][prevPosition.y, prevPosition.x].transform);
        }

        // Return path
        return path;
    }
}
