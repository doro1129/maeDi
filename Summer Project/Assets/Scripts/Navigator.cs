using System;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    public GameMap Target;

    private static Vector3Int[] Directions = {
        new Vector3Int( 0, -1,  0),
        new Vector3Int( 0,  1,  0),
        new Vector3Int(-1,  0,  0),
        new Vector3Int( 1,  0,  0),
        new Vector3Int(-1, -1,  0),
        new Vector3Int(-1,  1,  0),
        new Vector3Int( 1, -1,  0),
        new Vector3Int( 1,  1,  0),

        new Vector3Int( 0, -1, -1),
        new Vector3Int( 0,  1, -1),
        new Vector3Int(-1,  0, -1),
        new Vector3Int( 1,  0, -1),
        new Vector3Int(-1, -1, -1),
        new Vector3Int(-1,  1, -1),
        new Vector3Int( 1, -1, -1),
        new Vector3Int( 1,  1, -1),
        
        new Vector3Int( 0, -1,  1),
        new Vector3Int( 0,  1,  1),
        new Vector3Int(-1,  0,  1),
        new Vector3Int( 1,  0,  1),
        new Vector3Int(-1, -1,  1),
        new Vector3Int(-1,  1,  1),
        new Vector3Int( 1, -1,  1),
        new Vector3Int( 1,  1,  1),
    };

    private void Start()
    {
        // var firstPosition = this.Target.Graph[0][0, 0].transform.position;
        // Debug.Log($"firstPosition = {firstPosition}");
        // Debug.Log($"maxSize = {this.Target.MaxSize}");

        var startPosition = new Vector3Int(17, 5, 0);
        //var destinationPosition = new Vector3Int(4, 5, 2);
        var destinationPosition = new Vector3Int(5, 19, 0);
        var result = ShortestPath(startPosition, destinationPosition, 24);
    }

    private void FillMapArray<T>(ref T[][,] array, T value)
    {
        int depth = this.Target.MaxSize.z;

        array = new T[depth][,];

        for (int z = 0; z < depth; z++)
        {
            var height = this.Target.Graph[z].GetLength(0);
            var width = this.Target.Graph[z].GetLength(1);

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

    private float ManhattanDistance(Vector3 posA, Vector3 posB)
    {
        float dx = Mathf.Abs(posA.x - posB.x);
        float dy = Mathf.Abs(posA.x - posB.x);
        float dz = Mathf.Abs(posA.x - posB.x);

        return 4 * (dx + dy + dz);
    }

    private void HighlightTile(Vector3Int position, Color color)
    {
        var tileObject = this.Target.Graph[position.z][position.y, position.x];
        var meshRenderer = tileObject.GetComponent<MeshRenderer>();

        var highlightMaterial = new Material(meshRenderer.sharedMaterial);
        highlightMaterial.color = color;

        meshRenderer.sharedMaterial = highlightMaterial;
    }

    public List<Transform> ShortestPath(
        Vector3Int startPosition,
        Vector3Int destinationPosition,
        int directionCount = 4,
        bool isThreeDimenstional = true,
        int maxSearchCount = 1000
    )
    {
        // Map size
        var mapSize = this.Target.MaxSize;

        // The distances of between start to destination in the map
        var distances = new float[mapSize.z][,];
        this.FillMapArray<float>(ref distances, float.MaxValue);

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

            HighlightTile(current, Color.red);

            if (current == destinationPosition)
            {
                // Find the goal!
                break;
            }

            for (int i = 0; i < directionCount; i++)
            {
                var nextPosition = current + Navigator.Directions[i];

                if (!this.Target.HasTile(nextPosition))
                {
                    // Out of range
                    continue;
                }

                if (visited[nextPosition.z][nextPosition.y, nextPosition.x])
                {
                    // Already visited
                    continue;
                }

                //var nextDistance = this.ManhattanDistance(nextPosition, destinationPosition);
                var nextDistance = Vector3Int.Distance(nextPosition, destinationPosition);
                if (nextDistance > distances[nextPosition.z][nextPosition.y, nextPosition.x])
                {
                    // The nextDistance is not best distance
                    continue;
                }

                var tagName = this.Target.Graph[nextPosition.z][nextPosition.y, nextPosition.x].tag;
                if (GameMap.GetTileType(tagName) != GameMap.TileType.MonsterRoad)
                {
                    // The next tile is not "MonsterRoad"
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
        HighlightTile(prevPosition, Color.green);

        Debug.Log("======");
        while (prevPosition != Vector3Int.zero && prevPosition != startPosition)
        {
            Debug.Log(prevPosition);

            prevPosition = prev[prevPosition.z][prevPosition.y, prevPosition.x];
            path.Add(this.Target.Graph[prevPosition.z][prevPosition.y, prevPosition.x].transform);

            HighlightTile(prevPosition, Color.green);
        }
        Debug.Log("======");

        // Return path
        return path;
    }
}
