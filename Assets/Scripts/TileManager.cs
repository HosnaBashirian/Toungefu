using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class TileManager : MonoBehaviour
{
    public int width = 11;
    public int height = 21;
    public Vector3 tileSize = new Vector3(2, 2, 2);
    public Vector3 startCoordinate = new Vector3(70, -2, -40);

    public GameObject tilePrefab;
    public Material wallMaterial;
    public Material doorMaterial;
    public Material playerMaterial;
    
    private List<List<Vector3>> groundGrid; // stores tile positions
    private List<List<Node>> nodeGrid; // stores node data for each tile
    private Dictionary<Vector2Int, GameObject> tileDictionary; // stores tile references by coordinates
    private Vector2Int previousPlayerGridPos = new Vector2Int(-1, -1); 

    private Walls walls;
    private Doors doors;
    private Transform player;

    void Start()
    {
        walls = new Walls();
        doors = new Doors();
        player = GameObject.FindWithTag("Player").transform;
        tileDictionary = new Dictionary<Vector2Int, GameObject>();
        GenerateGrid();
        ApplyWallMaterials();
        ApplyDoorMaterials();
    }

    private void Update()
    {
        UpdatePlayerPos();
    }

    private void GenerateGrid()
    {
        groundGrid = new List<List<Vector3>>();
        nodeGrid = new List<List<Node>>();

        for (int row = 0; row < width; row++)
        {
            groundGrid.Add(new List<Vector3>());
            nodeGrid.Add(new List<Node>());

            for (int col = 0; col < height; col++)
            {
                Vector3 tilePosition = startCoordinate + new Vector3(row * tileSize.x, 0, col * tileSize.z);
                groundGrid[row].Add(tilePosition);

                GameObject tileObj = Instantiate(tilePrefab, tilePosition, Quaternion.identity);

                Node node = new Node(new Vector2Int(row, col));
                nodeGrid[row].Add(node);

                tileObj.name = $"Tile ({row}, {col})";
                // Debug.Log($"created tile: {tileObj.name} at position ({row}, {col})");

                tileDictionary[new Vector2Int(row, col)] = tileObj;

                TextMeshPro label = tileObj.GetComponentInChildren<TextMeshPro>();
                if (label != null)
                {
                    label.fontSize = 30;
                    label.text = $"{row}, {col}";
                }
            }
        }
    }

    private void ApplyWallMaterials()
    {
        foreach (var wallPosition in walls.wallGrids)
        {
            int row = wallPosition.x;
            int col = wallPosition.y;

            // ensure the wall position is within the grid bounds
            if (row >= 0 && row < width && col >= 0 && col < height)
            {
                // retrieve the tile object from the dictionary
                if (tileDictionary.TryGetValue(new Vector2Int(row, col), out GameObject tileObj))
                {
                    // find the child object named Mesh
                    Transform meshChild = tileObj.transform.Find("Mesh");
                    if (meshChild != null)
                    {
                        // get the Renderer component of the child object
                        Renderer renderer = meshChild.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            // change the material of the child object
                            renderer.material = wallMaterial;
                        }
                    }
                }
            }
        }
    }
    
    private void ApplyDoorMaterials()
    {
        foreach (var doorPosition in doors.DoorGrids)
        {
            int row = doorPosition.x;
            int col = doorPosition.y;
            
            if (row >= 0 && row < width && col >= 0 && col < height)
            {
                if (tileDictionary.TryGetValue(new Vector2Int(row, col), out GameObject tileObj))
                {
                    Transform meshChild = tileObj.transform.Find("Mesh");
                    if (meshChild != null)
                    {
                        Renderer renderer = meshChild.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material = doorMaterial;
                        }
                    }
                }
            }
        }
    }

    public void UpdatePlayerPos()
    {
        if (player != null)
        {
            Vector3 playerWorldPos = player.position;
            Vector2Int playerGridPos = WorldToGridPosition(playerWorldPos);

            if (playerGridPos != previousPlayerGridPos)
            {
                if (previousPlayerGridPos.x >= 0 && previousPlayerGridPos.y >= 0 &&
                    tileDictionary.TryGetValue(previousPlayerGridPos, out GameObject previousTile))
                {
                    Renderer previousRenderer = previousTile.GetComponent<Renderer>();
                    if (previousRenderer != null)
                    {
                        previousRenderer.material = tilePrefab.GetComponent<Renderer>().sharedMaterial; // reset to default material
                    }
                }
                if (tileDictionary.TryGetValue(playerGridPos, out GameObject currentTile))
                {
                    Renderer currentRenderer = currentTile.GetComponent<Renderer>();
                    if (currentRenderer != null)
                    {
                        currentRenderer.material = playerMaterial; // set player material
                    }
                }
                
                previousPlayerGridPos = playerGridPos;
            }
        }
    }

    private Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        int row = Mathf.FloorToInt((worldPosition.x - startCoordinate.x) / tileSize.x);
        int col = Mathf.FloorToInt((worldPosition.z - startCoordinate.z) / tileSize. z);

        row = Mathf.Clamp(row, 0, width - 1);
        col = Mathf.Clamp(col, 0, height - 1);

        return new Vector2Int(row, col);
    }

    private void UpdateGingyPos()
    {
        
    }

    public class Node
    {
        public Vector2Int coords;

        public Node(Vector2Int coords)
        {
            this.coords = coords;
        }
    }

    public class Walls
    {
        public List<Vector2Int> wallGrids = new List<Vector2Int>();

        public Walls()
        {
            Vector2Int[] wallPositions = new Vector2Int[]
            {
                new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2),
                new Vector2Int(1, 4), new Vector2Int(1, 5), new Vector2Int(1, 6), new Vector2Int(1, 7),
                new Vector2Int(1, 8), new Vector2Int(1, 9), new Vector2Int(1, 10), new Vector2Int(1, 11),
                new Vector2Int(1, 12), new Vector2Int(1, 13), new Vector2Int(2, 0), new Vector2Int(2, 13),
                new Vector2Int(3, 0), new Vector2Int(3, 1), new Vector2Int(3, 2), new Vector2Int(3, 4),
                new Vector2Int(3, 5), new Vector2Int(3, 6), new Vector2Int(3, 7), new Vector2Int(3, 8),
                new Vector2Int(3, 9), new Vector2Int(3, 11), new Vector2Int(3, 12),
                new Vector2Int(3, 13), new Vector2Int(4, 0), new Vector2Int(4, 6), new Vector2Int(4, 13),
                new Vector2Int(5, 0), new Vector2Int(5, 2), new Vector2Int(5, 4), new Vector2Int(5, 6),
                new Vector2Int(5, 6), new Vector2Int(5, 13), new Vector2Int(6, 0), new Vector2Int(6, 6),
                new Vector2Int(6, 13),
                new Vector2Int(7, 0), new Vector2Int(7, 2), new Vector2Int(7, 4), new Vector2Int(7, 6),
                new Vector2Int(7, 13), new Vector2Int(8, 0), new Vector2Int(8, 1), new Vector2Int(8, 2),
                new Vector2Int(8, 3), new Vector2Int(8, 4), new Vector2Int(8, 5), new Vector2Int(8, 6),
                new Vector2Int(8, 7), new Vector2Int(8, 8), new Vector2Int(8, 9), new Vector2Int(8, 10),
                new Vector2Int(8, 11), new Vector2Int(8, 12), new Vector2Int(8, 13)
            };

            wallGrids.AddRange(wallPositions);
        }
    }

    public class Doors
    {
        public List<Vector2Int> DoorGrids = new List<Vector2Int>();

        public Doors()
        {
            Vector2Int[] doorPositions = new Vector2Int[]
            {
                new Vector2Int(1, 3), new Vector2Int(3, 10)
            };
            
            DoorGrids.AddRange(doorPositions);
        }
    }
    
}

// private void OnDrawGizmos()
// {
//     Gizmos.color = Color.blue;
//     for (int row = 0; row < groundGrid.Count; row++)
//     {
//         for (int col = 0; col < groundGrid[row].Count; col++)
//         {
//             Vector3 tilePosition = startCoordinate + new Vector3(row * tileSize.x, 0, col * tileSize.z);
//             Gizmos.DrawWireCube(tilePosition, tileSize);
//         }
//     }
// }