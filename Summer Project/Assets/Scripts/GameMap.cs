using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Map of the game for 3D Tilemap
/// </summary>
public class GameMap : MonoBehaviour
{
    /// <summary>
    /// The type of the tile
    /// </summary>
    public enum TileType
    {
        Ground,
        MinableObject,
        MonsterRoad,
        Tower,
    }
    
    /// <summary>
    /// The grid component
    /// </summary>
    public Grid Grid;
    /// <summary>
    /// Relative position when an object is placed on a tile
    /// </summary>
    public Vector3 TileObjectOffset = Vector3.zero;
    /// <summary>
    /// Tile local scale
    /// </summary>
    public Vector3 TileScale = Vector3.one;

    private List<Tilemap> layers;
    private List<Vector2Int> layersPosition;
    private GameObject[][,] graph;
    private Vector3Int maxSize = Vector3Int.zero;

    /// <summary>
    /// Get layers (readonly)
    /// </summary>
    public List<Tilemap> Layers { get { return layers; } }
    /// <summary>
    /// Get graph (readonly)
    /// </summary>
    public GameObject[][,] Graph { get { return graph; } }
    /// <summary>
    /// Get maximum size of the map (readonly)
    /// </summary>
    public Vector3Int MaxSize { get { return maxSize; } }

    private void Awake()
    {
        // Check grid parameters
        if (Grid.cellLayout != GridLayout.CellLayout.Rectangle)
            Debug.LogWarning("The grid cell layout is expected to Rectangle");
        if (Grid.cellSwizzle != GridLayout.CellSwizzle.XZY)
            Debug.LogWarning($"The grid cell swizzle is expected to XZY (grid.cellSwizzle: {Grid.cellSwizzle})");
        
        // Set Graph
        SetGraph();
    }

    /// <summary>
    /// Set graph
    /// </summary>
    public void SetGraph()
    {
        // Get tilemap components as layers in children of grid
        layers = Grid.gameObject.GetComponentsInChildren<Tilemap>().ToList();
        // Sorting by Y position (ascending)
        layers.Sort((a, b) => a.transform.position.y < b.transform.position.y ? -1 : 1);
        
        // Initialize Variables 
        maxSize = new Vector3Int(0, 0, layers.Count);
        graph = new GameObject[maxSize.z][,];
        layersPosition = new List<Vector2Int>();
        Vector2Int minLayerPosition = new Vector2Int(int.MaxValue, int.MaxValue);
        
        for (int layerIndex = 0; layerIndex < maxSize.z; layerIndex ++)
        {
            Tilemap tilemap = layers[layerIndex];
            Transform layer = tilemap.transform;
            Vector3Int minPosition = new Vector3Int(int.MaxValue, int.MaxValue);
            Vector3Int maxPosition = new Vector3Int(int.MinValue, int.MinValue);
            
            // Check tiles
            for (int childIndex = 0; childIndex < layer.childCount; childIndex ++)
            {
                // Get child
                Transform child = layer.GetChild(childIndex);
                // Get tile position
                Vector3Int position = tilemap.WorldToCell(child.position);
                
                // Check tile scale
                if (child.localScale != TileScale)
                    Debug.LogWarning($"The tile scale docs not match tileScale (layer: {layerIndex}, child: {childIndex})");

                // Set minPosition
                minPosition = Vector3Int.Min(minPosition, position);
                // Set maxPosition
                maxPosition = Vector3Int.Max(maxPosition, position);
            }
            
            // Update min layer position
            minLayerPosition = Vector2Int.Min(minLayerPosition, (Vector2Int)minPosition);
            
            // Set layer position
            layersPosition.Add((Vector2Int)minPosition);
            
            // Set graph size
            Vector3Int size = maxPosition - minPosition + Vector3Int.one;
            graph[layerIndex] = new GameObject[size.y, size.x];

            // Set array
            for (int childIndex = 0; childIndex < layer.childCount; childIndex ++)
            {
                // Get child
                Transform child = layer.GetChild(childIndex);
                // Get position
                Vector3Int position = tilemap.WorldToCell(child.position) - minPosition;
                // Set a tile
                graph[layerIndex][position.y, position.x] = child.gameObject;
            }
            
            // Update maxHeight, maxWidth
            maxSize.y = Math.Max(maxSize.y, size.y);
            maxSize.x = Math.Max(maxSize.x, size.x);
        }

        // Update layer position
        for (int layerIndex = 0; layerIndex < maxSize.z; layerIndex ++)
            layersPosition[layerIndex] -= minLayerPosition;
    }

    /// <summary>
    /// Create a graph with the topmost tile
    /// </summary>
    /// <param name="graph2d">Result array</param>
    public void GetGraph2d(out GameObject[,] graph2d)
    {
        graph2d = new GameObject[maxSize.y, maxSize.x];

        for (int z = 0; z < maxSize.z; z ++)
        {
            int height = graph[z].GetLength(0);
            int width = graph[z].GetLength(1);
            Vector2Int layerPosition = layersPosition[z];

            for (int y = 0; y < height; y ++)
            {
                for (int x = 0; x < width; x ++)
                {
                    if (graph[z][y, x] == null) continue;
                    graph2d[y + layerPosition.y, x + layerPosition.x] = graph[z][y, x];
                }
            }
        }
    }

    /// <summary>
    /// Get specific tiles in the map
    /// </summary>
    /// <param name="type">Tile type</param>
    /// <returns>The list of the tiles</returns>
    public List<GameObject> GetTiles(TileType type)
    {
        List<GameObject> result = new List<GameObject>();

        for (int z = 0; z < maxSize.z; z ++)
        {
            int height = graph[z].GetLength(0);
            int width = graph[z].GetLength(1);
            
            for (int y = 0; y < height; y ++)
            {
                for (int x = 0; x < width; x ++)
                {
                    GameObject tile = graph[z][y, x];
                    if (tile != null && GetTileType(tile.tag) == type)
                        result.Add(tile);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Get specific tiles in the topmost tiles
    /// </summary>
    /// <param name="type">Tile type</param>
    /// <returns>The list of the tiles</returns>
    public List<GameObject> GetTiles2d(TileType type)
    {
        List<GameObject> result = new List<GameObject>();
        GetGraph2d(out var graph2d);

        for (int y = 0; y < maxSize.y; y ++)
        {
            for (int x = 0; x < maxSize.x; x ++)
            {
                if (graph2d[y, x] != null && GetTileType(graph2d[y, x].tag) == type)
                    result.Add(graph2d[y, x]);
            }
        }

        return result;
    }

    /// <summary>
    /// Convert world position to cell position
    /// </summary>
    /// <param name="position">World position</param>
    /// <returns>Cell position</returns>
    /// <exception cref="Exception">The worldPosition and layer do not match each other (Out of range)</exception>
    public Vector3Int WorldToCell(Vector3 position)
    {
        foreach (var layer in layers)
        {
            float ypos = layer.transform.position.y;
            float height = layer.transform.localScale.y;

            if (ypos <= position.y && position.y <= ypos + height)
                return layer.WorldToCell(position);
        }

        throw new Exception("The worldPosition and layer do not match each other (Out of range)");
    }

    /// <summary>
    /// Check if a tile exists in a specific position
    /// </summary>
    /// <param name="position">Cell position</param>
    /// <returns>If true the tile exists at that position</returns>
    public bool HasTile(Vector3Int position)
    {
        try
        {
            return graph[position.z][position.y, position.x] != null;
        }
        catch
        {
            // NullReferenceException
            return false;
        }
    }

    /// <summary>
    /// Place the GameObject on the Tile
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="targetObject">Tile GameObject</param>
    public void PutObject(GameObject gameObject, GameObject targetObject)
    {
        Instantiate(gameObject, targetObject.transform.position + TileObjectOffset, gameObject.transform.rotation);
    }

    /// <summary>
    /// Place the GameObject on the Tile
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="position">Cell position</param>
    /// <exception cref="Exception">There is not a tile</exception>
    public void PutObject(GameObject gameObject, Vector3Int position)
    {
        if (!HasTile(position))
            throw new Exception("There is not a tile");
        
        GameObject tile = graph[position.z][position.y, position.x];
        PutObject(gameObject, tile);
    }

    /// <summary>
    /// Place the GameObject on the Tile
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="worldPosition">World position</param>
    public void PutObject(GameObject gameObject, Vector3 worldPosition)
    {
        PutObject(gameObject, WorldToCell(worldPosition));
    }
    
    /// <summary>
    /// Convert GameObject tag name to TileType enumeration
    /// </summary>
    /// <param name="tagName">Tag name</param>
    /// <returns>Tile type</returns>
    public static TileType? GetTileType(String tagName)
    {
        switch (tagName)
        {
            case "GroundTile":
                return TileType.Ground;
            case "MinableObjectTile":
                return TileType.MinableObject;
            case "MonsterRoadTile":
                return TileType.MonsterRoad;
            case "TowerTile":
                return TileType.Tower;
            default:
                return null;
        }
    }
}
