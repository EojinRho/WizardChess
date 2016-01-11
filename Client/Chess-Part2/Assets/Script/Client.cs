using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class Client : MonoBehaviour {
	private SocketIOComponent socket;
	private PlayerControl _PlayerControl;
	Dictionary<string, string> data;

	void Start () {
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		GameObject playercontrolobject = GameObject.Find ("__GameManager");
		_PlayerControl = playercontrolobject.GetComponent<PlayerControl> ();

		Debug.Log (socket);
		if (socket == null) {
			Debug.Log("fuck you!");
		}

		socket.On ("connection", Connection);
		socket.On("toclient", ToClient);
	}

	public void Update(){
		
	}

	public void Connection(SocketIOEvent e){
		Debug.Log (e.data ["msg"]);
	}

	public void ToClient(SocketIOEvent e){
		Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
		Debug.Log (e.data ["msg"].ToString().Trim());
		_PlayerControl.TakeInput (e.data ["msg"].ToString().Trim('"'));
	}
}
