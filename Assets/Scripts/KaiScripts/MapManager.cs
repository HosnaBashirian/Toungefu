using UnityEngine;

public class MapManager : MonoBehaviour {
	public int cols = 20;		// Map width 
	public int rows = 20;		// Map height
	public int tileCount;		// Map surface area

	public float tileSize = 2.0f;

	public TileNode[] tileArray;

	public GameObject _wallPrefab;
	public LayerMask _wallMask;

	public Vector3 minimapPosition = new Vector3(70, -2, -40);

    void Start() {
		/*
		Instantiate(_wallPrefab, GridToVec3(new Vector2Int(5, 2)), Quaternion.identity);
		Instantiate(_wallPrefab, GridToVec3(new Vector2Int(6, 2)), Quaternion.identity);
		Instantiate(_wallPrefab, GridToVec3(new Vector2Int(7, 2)), Quaternion.identity);
		Instantiate(_wallPrefab, GridToVec3(new Vector2Int(8, 2)), Quaternion.identity);
		Instantiate(_wallPrefab, GridToVec3(new Vector2Int(9, 2)), Quaternion.identity);
		*/

		GenerateMap();
    }

	public void GenerateMap() {
		tileCount = cols * rows;				// Get surface area
		tileArray = new TileNode[tileCount];	// Initialize tile list	

		// Set scale of ground plane from map dimensions
		gameObject.transform.localScale = new Vector3(cols * tileSize, tileSize, rows * tileSize);

		// Place tiles and check for walls 
		for(short i = 0; i < tileCount; i++) {
			Vector2Int coords = IndexToCoords(i);	// Get grid position
		
			TileNode newTile = new TileNode(false, coords, new int[4]);			// Initialize tile 
			tileArray[i] = newTile;												// Place tile in array
			
			// Tile collision test setup
			Vector3 collisionTest = GridToVec3(coords);							// Get vector for wall test
			collisionTest.y = 1;
			Vector3 collTestSize = new Vector3(0.5f, 0.5f, 0.5f);
			
			// Check for wall 
			if(Physics.CheckBox(collisionTest, collTestSize, Quaternion.identity, _wallMask, QueryTriggerInteraction.Ignore)) {
				AddWall(coords);
			}
		
		}
		
	}

	public void AddWall(Vector2Int coords) {
		tileArray[CoordsToIndex(coords)] = new TileNode(true, coords, new int[4]);
		Debug.Log($"Added wall at {coords}");
	}
	
	// ----------------------------------------------------------------------------------------------------- //
	// 									  **[HELPER FUNCTIONS]**
	// ----------------------------------------------------------------------------------------------------- //
	
	// Convert Vector3 to grid coordinates 
	public Vector2Int Vec3ToGrid(Vector3 vec3) {
		return new Vector2Int( (int)((vec3.x / tileSize) + cols * 0.5f),
							   (int)((vec3.z / tileSize) + rows * 0.5f));
	}
	
	// Convert grid coordinates to Vector3
	public Vector3 GridToVec3(Vector2Int gridPos) {
		return new Vector3( ((gridPos.x * tileSize) - ((cols * tileSize) * 0.5f)) + 1.0f, 1.0f,
							((gridPos.y * tileSize) - ((rows * tileSize) * 0.5f)) + 1.0f);	
	}
	
	// Convert grid coordinates to position in 1D array
	public int CoordsToIndex(Vector2Int coords) {
		return coords.x + coords.y * cols;
	}

	// Convert index in 1D array to 2D grid coordinates
	public Vector2Int IndexToCoords(int index) {
		return new Vector2Int(index % cols,		// X
							  index / cols);	// Y
	}
	
	// Check if two bounding boxes intersect
	public bool BoxTest(Vector3 pos_a, float scale_a, Vector3 pos_b, float scale_b) {
		Vector3 min_a = new Vector3(pos_a.x - (scale_a * 0.5f), pos_a.y - (scale_a * 0.5f), pos_a.z - (scale_a * 0.5f));
		Vector3 max_a = new Vector3(pos_a.x + (scale_a * 0.5f), pos_a.y + (scale_a * 0.5f), pos_a.z + (scale_a * 0.5f));
		Vector3 min_b = new Vector3(pos_b.x - (scale_b * 0.5f), pos_b.y - (scale_b * 0.5f), pos_b.z - (scale_b * 0.5f));
		Vector3 max_b = new Vector3(pos_b.x + (scale_b * 0.5f), pos_b.y + (scale_b * 0.5f), pos_b.z + (scale_b * 0.5f));
		
		return( min_a.x <= max_b.x && max_a.x >= min_b.x &&
				min_a.y <= max_b.y && min_a.y >= min_b.y &&
				min_a.z <= max_b.z && min_a.z >= min_b.z );
	}

	//
	// ----------------------------------------------------------------------------------------------------- //
	// ----------------------------------------------------------------------------------------------------- //
}

public class TileNode {
	bool isWall;		  	// Can an entity move through this tile
	Vector2Int coords;		// Grid position of tile
	int[] entityIndices;	// List of entities contained in this tile

	public TileNode(bool _wall, Vector2Int _coords, int[] _ent_indices){
		this.isWall = _wall;
		this.coords = _coords;
		this.entityIndices = _ent_indices;
	}
}

