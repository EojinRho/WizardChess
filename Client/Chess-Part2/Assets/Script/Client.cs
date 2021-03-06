﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class Client : MonoBehaviour {
	private SocketIOComponent socket;
	private PlayerControl _PlayerControl;
	private Dictionary<string, string> data;
	GameObject go;
	GameObject playercontrolobject;
	private string User = " ";
	public int selectColor = 0;
	private string[] OptionColor = new string[]{"white","black"};
	string LoginFail = " ";
	private bool login = true;

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
	}

	void Start () {
		DontDestroyOnLoad (transform.gameObject);
		go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();


		Debug.Log (socket);
		if (socket == null) {
			Debug.Log("fuck you!");
		}

		socket.On ("connection", Connection);
		socket.On("loginconfirm", LoginConfirm);
		socket.On("toclient", ToClient);
	}
		

	public void ToClient(SocketIOEvent e){
		Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
		Debug.Log (e.data ["msg"].ToString().Trim());
		playercontrolobject = GameObject.Find ("__GameManager");
		_PlayerControl = playercontrolobject.GetComponent<PlayerControl> ();
		//_PlayerControl.TakeInput (e.data ["msg"].ToString().Trim('"'));
	}

	public void Update(){
		if (Input.GetKey ("space")) {
			data = new Dictionary<string, string> ();
			data ["msg"] = "noen";
			socket.Emit("fromclient",new JSONObject(data));
			login = false;
		}
	}

	void OnGUI(){
		if (login) {
			GUI.Label (new Rect (10, 170, 100, 20), "----Login----");
			GUI.Label (new Rect (10, 200, 100, 20), "Name");
			User = GUI.TextField (new Rect (60, 200, 100, 20), User);
			GUI.Label (new Rect (10, 230, 100, 20), "Color");
			GUI.Label (new Rect (30, 300, 100, 20), LoginFail);
			selectColor = GUI.SelectionGrid (new Rect (60, 230, 160, 20), selectColor, OptionColor, OptionColor.Length, GUI.skin.toggle);

			if (GUI.Button (new Rect (10, 260, 100, 20), "Login")) {
				data = new Dictionary<string, string> ();
				data ["user"] = User;
				switch (selectColor) {
				case 0:
					data ["color"] = "white";
					break;
				case 1:
					data ["color"] = "black";
					break;
				}
				socket.Emit ("login", new JSONObject (data));
			}
		}
	}

	public void Connection(SocketIOEvent e){
		Debug.Log (e.data ["msg"]);
	}

	public void LoginConfirm(SocketIOEvent e){
		Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
		Debug.Log (e.data ["user"].ToString().Trim('"'));
		Debug.Log (e.data ["msg"].ToString().Trim('"'));
		if (e.data ["msg"].ToString ().Trim ('"').Equals ("Success")) {
			Application.LoadLevel (1);
			Debug.Log("Yeah!");
			login = false;
		} else if (e.data ["msg"].ToString ().Trim ('"').Equals ("Fail")) {
			Debug.Log("Fuck login failed");
			LoginFail = e.data ["msg"].ToString ().Trim ('"');
		} else {
			Debug.Log("none of my biz");
		}
	}
}
