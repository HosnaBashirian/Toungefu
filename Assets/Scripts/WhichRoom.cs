using UnityEngine;

public class WhichRoom : MonoBehaviour {
	public Handler _handler;

    void Start() {
		_handler = GameObject.Find("GridManager").GetComponent<Handler>();
    }

	void OnTriggerEnter(Collider obj) {
		if(obj.gameObject.name == "Player") {
			switch(this.gameObject.name) {
				case "Room0": _handler.in_room = 0; break;
				case "Room1": _handler.in_room = 1; break;
				case "Room2": _handler.in_room = 2; break;
				case "Room3": _handler.in_room = 3; break;
				case "Room4": _handler.in_room = 4; break;
				case "Room5": _handler.in_room = 5; break;
				case "Room6": _handler.in_room = 6; break;
				case "Room7": _handler.in_room = 7; break;
			}
		}
	}
}
