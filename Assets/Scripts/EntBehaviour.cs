using UnityEngine;
using System.Collections.Generic;

public class EntBehaviour : MonoBehaviour {
	public Handler _handler;
	public PlayerTileMovements2 _player;

	public int index;
	public short id;
	public short type;

	public Vector2Int coords;
	public Vector2Int facing;

	public Vector2Int start_coords;
	public Vector2Int pivot_point;

	public Vector2Int[] valid_moves;
	public short piv_index = 0;

    void Start() {
       _handler = GameObject.Find("GridManager").GetComponent<Handler>();
	   _player = _handler._player.GetComponent<PlayerTileMovements2>();

	   index = _handler.entity_list.Count;
	   _handler.entity_list.Add(this);

	   Init();
    }

	private void Init() {
		coords = _handler.Vec3ToGrid(gameObject.transform.position);
		start_coords = coords;

		//Debug.Log($"enemy[{index}] coords: {_handler.Vec3ToGrid(gameObject.transform.position)}");

		switch(type) {
			case 0:
				break;

			case 1:
				valid_moves = new Vector2Int[8] {
					_handler.CoordsAdd(pivot_point, new Vector2Int(  0, -1 ) ), _handler.CoordsAdd(pivot_point, new Vector2Int(  1, -1 ) ),
					_handler.CoordsAdd(pivot_point, new Vector2Int(  1,  0 ) ), _handler.CoordsAdd(pivot_point, new Vector2Int(  1,  1 ) ),
					_handler.CoordsAdd(pivot_point, new Vector2Int(  0,  1 ) ), _handler.CoordsAdd(pivot_point, new Vector2Int( -1,  1 ) ),
					_handler.CoordsAdd(pivot_point, new Vector2Int( -1,  0 ) ), _handler.CoordsAdd(pivot_point, new Vector2Int( -1, -1 ) )};

				for(short i = 0; i < 8; i++) if(_handler.CoordsCompare(coords, valid_moves[i])) piv_index = i;

				break;

			case 2:
				break;
		}
	}
	
	public void Tick() {
		switch(type) {
			case 0: BonbonTick(); 	break;
			case 1: SourTick();   	break;
			case 2: BreakerTick();	break;
		}
	}
	
	private void BonbonTick() {

	}

	private void SourTick() {
		piv_index++;
		if(piv_index > 7) piv_index = 0;
		gameObject.transform.position = _handler.GridToVec3(valid_moves[piv_index]);
	}

	private void BreakerTick() {

	}
}
