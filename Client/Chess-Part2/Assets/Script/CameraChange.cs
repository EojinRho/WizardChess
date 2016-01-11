using UnityEngine;
using System.Collections;

public class CameraChange : MonoBehaviour {

	void Start(){
		DontDestroyOnLoad (transform.gameObject);
		GameObject go = GameObject.Find("SocketIO");
		Client client = go.GetComponent<Client>();
		if (client.selectColor == 0) {
			transform.position = new Vector3 (3.5f, 10.0f, -5.0f);
			transform.rotation = Quaternion.Euler (45.0f, 0.0f, 0.0f);
		} else {
			transform.position = new Vector3 (3.5f, 10.0f, 12.0f);
			transform.rotation = Quaternion.Euler (45.0f, 180.0f, 0.0f);
		}
	}

	public void OnGUI(){
		if (GUI.Button (new Rect (260, 520, 100, 20), "White")) {
			transform.position = new Vector3 (3.5f, 10.0f, -5.0f);
			transform.rotation = Quaternion.Euler (45.0f, 0.0f, 0.0f);
		}
		if (GUI.Button (new Rect (410, 520, 100, 20), "Black")) {
			transform.position = new Vector3 (3.5f, 10.0f, 12.0f);
			transform.rotation = Quaternion.Euler (45.0f, 180.0f, 0.0f);
		}
	}

}
