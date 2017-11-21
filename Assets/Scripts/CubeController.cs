using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {
	GameController myGameController;
	public int myX, myY;

	// Use this for initialization
	void Start () {
		myGameController = GameObject.Find ("GameControllerObject").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown () {
		myGameController.ProcessClick (gameObject, myX, myY);
	}
}
