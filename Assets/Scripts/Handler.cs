using UnityEngine;
using System.Collections.Generic;

public class Handler : MonoBehaviour {
	// Grid direction definitions
	public static Vector2Int NORTH = new Vector2Int(  0, -1 );
	public static Vector2Int EAST  = new Vector2Int( -1,  0 );
	public static Vector2Int SOUTH = new Vector2Int(  0,  1 );
	public static Vector2Int WEST  = new Vector2Int(  1,  0 );
	public static Vector2Int[] DIR_ENUM = new Vector2Int[] { NORTH, EAST, SOUTH, WEST };

	// Entitity ID definitions 
	public enum ENTITY_ID {
		bonbon, 		// 0
		worm,			// 1 
		jawbreaker		// 2
	};

	public int map_w;
	public int map_h;
	public int tile_count;
	public float tile_size;
	public TileNode[] tile_arr;
	
	public int entity_count;
	//public Entity[] entity_arr;
	
	public List<EntBehaviour> entity_list;
	
	public LayerMask _wallMask;
	public GameObject _wallPrefab;

	public GameObject _player;

    void Start() {
		_player = GameObject.Find("Player");		
		entity_count = 0;

		map_w+=2;
		map_h+=2;
		GenerateMap();
    }

	public void GenerateMap() {
		tile_count = map_w * map_h;				// Get surface area
		tile_arr = new TileNode[tile_count];	// Initialize tile list	

		Vector2Int pivot_test_pos0 = Vec3ToGrid(new Vector3(8.0f, 1.1f, -17.0f));
		Vector3 piv_test0 = GridToVec3(pivot_test_pos0);
		piv_test0.y = 3;
		Instantiate(_wallPrefab, piv_test0, Quaternion.identity);
		Debug.Log($"Sour pivot0: {pivot_test_pos0}");

		Vector2Int pivot_test_pos1 = Vec3ToGrid(new Vector3(8.0f, 1.1f, -11.0f));
		Vector3 piv_test1 = GridToVec3(pivot_test_pos1);
		piv_test1.y = 3;
		Instantiate(_wallPrefab, piv_test1, Quaternion.identity);
		Debug.Log($"Sour pivot1: {pivot_test_pos1}");

		// Place tiles and check for walls 
		for(short i = 0; i < tile_count; i++) {
			Vector2Int coords = IndexToCoords(i);	// Get grid position
			
			// Tile collision test setup
			Vector3 collisionTest = GridToVec3(coords);
			collisionTest.y = 1;
			Vector3 collTestSize = new Vector3(0.5f, 0.5f, 0.5f);
			
			// Check for wall 
			bool is_wall = false;
			if(Physics.CheckBox(collisionTest, collTestSize, Quaternion.identity, _wallMask, QueryTriggerInteraction.Ignore)) {
				is_wall = true;
				/*
				Vector3 pos = GridToVec3(coords);
				pos.y = 3;
				Instantiate(_wallPrefab, pos, Quaternion.identity);
				*/
				//Debug.Log($"Added wall at {coords}, {collisionTest}");
			}

			TileNode newTile = new TileNode(is_wall, coords, new int[4]);		// Initialize tile 
			tile_arr[i] = newTile;												// Place tile in array
		}
	}

	public void ProcessTurn() {
		Debug.Log("Processing turn...");

		for(short i = 0; i < entity_list.Count; i++) {
			entity_list[i].Tick();
		}
	}

	// ----------------------------------------------------------------------------------------------------- //
	// 									  **[HELPER FUNCTIONS]**
	// ----------------------------------------------------------------------------------------------------- //
	
	// Convert Vector3 to grid coordinates 
	public Vector2Int Vec3ToGrid(Vector3 vec3) {
		return new Vector2Int( (int)((vec3.x / tile_size) + map_w * 0.5f),
							   (int)((vec3.z / tile_size) + map_h * 0.5f));
	}
	
	// Convert grid coordinates to Vector3
	public Vector3 GridToVec3(Vector2Int gridPos) {
		return new Vector3( ((gridPos.x * tile_size) - ((map_w * tile_size) * 0.5f)) + 1.0f, 1.0f,
							((gridPos.y * tile_size) - ((map_h * tile_size) * 0.5f)) + 1.0f);	
	}
	
	// Convert grid coordinates to position in 1D array
	public int CoordsToIndex(Vector2Int coords) {
		return coords.x + coords.y * map_w;
	}

	// Convert index in 1D array to 2D grid coordinates
	public Vector2Int IndexToCoords(int index) {
		return new Vector2Int(index % map_w,	// X
							  index / map_w);	// Y
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

	public Vector2Int CoordsAdd(Vector2Int a, Vector2Int b) {
		return new Vector2Int(a.x + b.x, a.y + b.y);
	}

	public bool CoordsCompare(Vector2Int a, Vector2Int b) {
		return(a.x - b.x == 0 && a.y - b.y == 0);
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

