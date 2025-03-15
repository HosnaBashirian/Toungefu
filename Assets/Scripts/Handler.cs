using UnityEngine;
using System.Collections.Generic;

public class Handler : MonoBehaviour {
	// Grid direction definitions
	public static Vector2Int NORTH = new Vector2Int(  0,  1 );
	public static Vector2Int EAST  = new Vector2Int( -1,  0 );
	public static Vector2Int SOUTH = new Vector2Int(  0, -1 );
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
	
	public List<EntBehaviour> entity_list;
	
	public LayerMask _wallMask;
	public GameObject _wallPrefab;

	public GameObject _player;
	public PlayerTileMovements2 player_ctl;

	public Vector2Int player_coords;
	public Vector2Int player_facing;

	public short player_atk_range = 2;

    void Start() {
		_player = GameObject.Find("Player");
		player_ctl = _player.GetComponent<PlayerTileMovements2>();

		map_w+=2;
		map_h+=2;
		GenerateMap();
    }

	public void GenerateMap() {
		tile_count = map_w * map_h;				// Get surface area
		tile_arr = new TileNode[tile_count];	// Initialize tile list	

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
			}

			TileNode newTile = new TileNode(is_wall, coords);	// Initialize tile 
			tile_arr[i] = newTile;								// Place tile in array
		}

		RefreshTiles(false);
	}

	public void ProcessTurn() {
		Debug.Log("Processing turn...");
		
		// Update Player position
		player_coords = Vec3ToGrid(_player.transform.position);
		Debug.Log($"Player Position: {player_coords}");

		RefreshTiles(true);
		
		// Check collisions
		if(tile_arr[CoordsToIndex(player_coords)].hazard) {
			// Kill player...
			Debug.Log($"Player collided with enemy at {player_coords}!");
			LevelReset();
		}
	}

	public void RefreshTiles(bool tick_enemies) {
		// Clean data
		for(int i = 0; i < tile_count; i++) { 
			tile_arr[i].hazard = false;
			tile_arr[i].enemy_index = -1;
		}
		
		// Write data
		for(short i = 0; i < entity_list.Count; i++) {
			if(entity_list[i].active) {
				if(tick_enemies) entity_list[i].Tick();
				
				TileNode tile = tile_arr[CoordsToIndex(entity_list[i].coords)];
				tile.hazard = true;

				tile.enemy_index = i;
				Debug.Log($"Enemy[{i}] located at: {tile.coords}");
			}
		}
	}

	public void ProcessPlayerAttack() {
		player_facing = player_ctl.gridFacing;
		RefreshTiles(false);

		// Find which tiles are hit 
		Vector2Int[] tile_hit = new Vector2Int[4];
		Vector2Int prev_check = CoordsAdd(player_coords, new Vector2Int(-1, 0));
		for(short i = 0; i < 4; i++) {
			//tile_hit[i] = CoordsAdd(new Vector2Int(player_coords.x-1, player_coords.y), CoordsScale(player_facing, i+1));
			//Debug.Log($"Collision check set for: {tile_hit[i]}");

			tile_hit[i] = CoordsAdd(prev_check, player_facing);
			prev_check = tile_hit[i];
		}

		for(short i = 0; i < 4; i++) {
			if(CoordsToIndex(tile_hit[i]) >= 0 && CoordsToIndex(tile_hit[i]) <= tile_count) {
				TileNode tile = tile_arr[CoordsToIndex(tile_hit[i])];
				Debug.Log($"Attack collision testing at {tile.coords}");

				if(tile.enemy_index > -1) {
					Debug.Log($"Hit enemy at {tile_hit[i]}!");
					entity_list[tile.enemy_index].Kill();
				}
			}
		}

		RefreshTiles(false);
	}

	public void LevelReset() {
		_player.transform.position = player_ctl.spawnPoint;
		player_ctl.movePoint.position = player_ctl.spawnPoint;

		for(short i = 0; i < entity_list.Count; i++) {
			entity_list[i].coords = entity_list[i].start_coords;
			entity_list[i].active = true;
			if(entity_list[i].type > 0) entity_list[i].GetComponentInChildren<MeshRenderer>().enabled = true;
			entity_list[i].Init();
		}

		RefreshTiles(false);
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

	public Vector2Int CoordsSubtract(Vector2Int a, Vector2Int b) {
		return new Vector2Int(a.x - b.x, a.y - b.y);
	}

	public bool CoordsCompare(Vector2Int a, Vector2Int b) {
		return(a.x - b.x == 0 && a.y - b.y == 0);
	}

	public Vector2Int CoordsScale(Vector2Int coords, int scale) {
		return new Vector2Int(coords.x * scale, coords.y * scale);
	}

	//
	// ----------------------------------------------------------------------------------------------------- //
	// ----------------------------------------------------------------------------------------------------- //
}

public class TileNode {
	public bool is_wall;		  	// Can an entity move through this tile
	public bool hazard;				// Will the tile damage the player when landing on it
	public Vector2Int coords;		// Grid position of tile
	public short enemy_index; 		// Index of enemy on this tile, -1 if empty

	public TileNode(bool _wall, Vector2Int _coords) {
		this.is_wall = _wall;
		this.coords = _coords;
	}
}

