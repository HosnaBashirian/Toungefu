using TMPro;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int width = 11;
    public int height = 21;
    public Vector3 tileSize = new Vector3(2,2,2);
    private Vector3[,] groundGrid;
    private Node[,] nodeGrid;
    public Vector3 startCoordinate = new Vector3(-10, -2, -40);

    public GameObject tilePrefab;
    void Start()
    {
        GenerateGrid();
        
    }
    
    void Update()
    {
    }

    private void GenerateGrid()
    {
        groundGrid = new Vector3[width, height];
        nodeGrid = new Node[width, height];
        for (int row = 0; row < groundGrid.GetLength(0); row++)
        {
            for (int col = 0; col < groundGrid.GetLength(1); col++)
            {
                groundGrid[row, col] = startCoordinate + new Vector3(row * tileSize.x, 0, col * tileSize.z);
                GameObject tileObj = Instantiate(tilePrefab, groundGrid[row, col], Quaternion.identity);

                nodeGrid[row, col] = new Node(new Vector2Int(row, col));
                tileObj.name = $"Tile_{nodeGrid[row, col].coords.x}, {nodeGrid[row, col].coords.y}";
                
                TextMeshPro label = tileObj.GetComponentInChildren<TextMeshPro>();
                if (label != null)
                {
                    label.text = $"{nodeGrid[row, col].coords.x}, {nodeGrid[row, col].coords.y}";
                }
            }
        }
    }
    
    public class Node
    {
        public Vector2Int coords;

        public Node(Vector2Int coords)
        {
            this.coords = coords;
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     for (int row = 0; row < groundGrid.GetLength(0); row++)
    //     {
    //         for (int col = 0; col < groundGrid.GetLength(1); col++)
    //         {
    //             Vector3 tilePosition = startCoordinate + new Vector3(row * tileSize.x, 0, col * tileSize.z);
    //             Gizmos.DrawWireCube(tilePosition, tileSize);
    //         }
    //     }
    // }
}
