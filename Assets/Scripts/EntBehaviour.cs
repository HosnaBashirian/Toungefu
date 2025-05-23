using UnityEngine;

public class EntBehaviour : MonoBehaviour {
    public Handler _handler;
    public PlayerTileMovements2 _player;

    public bool active = true;

    public int index;
    public short id;
    public short type;

    public Vector2Int coords;
    public Vector2Int facing;

    public Vector2Int start_coords;
    public Vector2Int pivot_point;

    public Vector2Int[] pivot_moves;
    public short piv_index = 0;
    public short start_piv_index = 0;

    bool piv_set = false;

    private Vector3 look_target;

    float start_y;

    public short room;

    void Start() {
        _handler = GameObject.Find("GridManager").GetComponent<Handler>();
        _player = _handler._player.GetComponent<PlayerTileMovements2>();

        // Add this enemy to the enemy list
        index = _handler.entity_list.Count;
        _handler.entity_list.Add(this);

        start_y = transform.position.y;

        Init();
    }

    public void Init() {
        coords = _handler.Vec3ToGrid(gameObject.transform.position);
        start_coords = coords;

        transform.position = new Vector3(transform.position.x, start_y, transform.position.z);

        active = true;

        if (type == 1) { // Only handle the logic for type 1 (SourTick)
            if (!piv_set) {
                // Set movement array values, the enemy will iterate linearly through array on every Handler.Tick()
                pivot_moves = new Vector2Int[8] {
                    _handler.CoordsAdd(pivot_point, new Vector2Int(  0, -1 )), _handler.CoordsAdd(pivot_point, new Vector2Int(  1, -1 )),
                    _handler.CoordsAdd(pivot_point, new Vector2Int(  1,  0 )), _handler.CoordsAdd(pivot_point, new Vector2Int(  1,  1 )),
                    _handler.CoordsAdd(pivot_point, new Vector2Int(  0,  1 )), _handler.CoordsAdd(pivot_point, new Vector2Int( -1,  1 )),
                    _handler.CoordsAdd(pivot_point, new Vector2Int( -1,  0 )), _handler.CoordsAdd(pivot_point, new Vector2Int( -1, -1 ))
                };
                // Check which index in the array, the enemy is in on start
                for (short i = 0; i < 8; i++) if (_handler.CoordsCompare(coords, pivot_moves[i])) piv_index = i;
                start_piv_index = piv_index;
                piv_set = true;
            } else {
                piv_index = start_piv_index;
                coords = pivot_moves[piv_index];
            }
        }
    }

    public void Tick() {
        if (type == 1) { // Only call SourTick if type is 1
            SourTick();
        }
    }

    private void SourTick() {
        Vector2Int prev = coords;
        Vector2Int next;

        piv_index++; // Move to next position
        if (piv_index > 7) piv_index = 0; // If at max, reset
        next = pivot_moves[piv_index];

        //gameObject.transform.position = _handler.GridToVec3(pivot_moves[piv_index]);  // Update model transform
        gameObject.transform.position = _handler.GridToVec3(next);  // Update model transform
        coords = next;

        // Set rotation
        facing = _handler.CoordsSubtract(next, prev);
        Vector3 a = Vector3.zero;
        if (_handler.CoordsCompare(facing, Handler.NORTH)) a = new Vector3(0, -90, 0);
        else if (_handler.CoordsCompare(facing, Handler.EAST)) a = new Vector3(0, 0, 0);
        else if (_handler.CoordsCompare(facing, Handler.SOUTH)) a = new Vector3(0, 90, 0);
        else if (_handler.CoordsCompare(facing, Handler.WEST)) a = new Vector3(0, 180, 0);
        transform.rotation = Quaternion.Euler(a);
    }
/*
    public void Kill() {
        active = false;

        TileNode tile = _handler.tile_arr[_handler.CoordsToIndex(coords)];
        tile.hazard = false;
        tile.enemy_index = -1;

        transform.position = new Vector3(transform.position.x, -10, transform.position.z);

        // Make sure only type 1 entities do this logic if needed
        if (type == 1) {
            // Perform any type-specific kill behavior here (e.g., special effects for SourTick)
        }
    } */
  }
